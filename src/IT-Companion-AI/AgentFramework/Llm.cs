// Project Name: SKAgent
// File Name: Llm.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Globalization;

using HFTokenizer = Tokenizers.HuggingFace.Tokenizer;


// ============================================================================
// ONNX: Embeddings + LLM
// ============================================================================


namespace ITCompanionAI.AgentFramework;


public interface IEmbeddingClient
{
    Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken = default);
}




public interface ILLMClient
{
    Task<string> CompleteAsync(string prompt, CancellationToken cancellationToken = default);
}




/// <summary>
///     ONNX embedding client for a bge-small-en-like model.
///     Assumes the model takes input_ids and attention_mask and outputs a single embedding vector.
///     You may need to adjust input/output names to match your specific model.
/// </summary>
public sealed class OnnxEmbeddingClient : IEmbeddingClient, IDisposable
{
    private readonly string _attentionMaskName;
    private readonly int _embeddingDim;
    private readonly string _inputIdsName;
    private readonly int _maxTokens;
    private readonly string _outputName;
    private readonly bool _requiresTokenTypeIds;
    private readonly InferenceSession _session;
    private readonly HFTokenizer.Tokenizer _tokenizer;
    private readonly string? _tokenTypeIdsName;







    public OnnxEmbeddingClient(
        string modelPath,
        HFTokenizer.Tokenizer tokenizer,
        int maxTokens = 512,
        string inputIdsName = "input_ids",
        string attentionMaskName = "attention_mask",
        string outputName = "last_hidden_state",
        int embeddingDim = 384,
        string? tokenTypeIdsName = "token_type_ids")
    {
        if (!File.Exists(modelPath))
        {
            throw new FileNotFoundException("Embedding model not found", modelPath);
        }

        _session = new InferenceSession(modelPath);
        _tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer));
        _maxTokens = maxTokens;
        _inputIdsName = inputIdsName;
        _attentionMaskName = attentionMaskName;
        _outputName = outputName;
        _embeddingDim = embeddingDim;
        _tokenTypeIdsName = string.IsNullOrWhiteSpace(tokenTypeIdsName) ? null : tokenTypeIdsName;
        _requiresTokenTypeIds = _tokenTypeIdsName is not null && _session.InputMetadata.ContainsKey(_tokenTypeIdsName);
    }







    public void Dispose()
    {
        _session.Dispose();
    }







    /// <summary>
    ///     Generates an embedding vector for the specified input text using the underlying ONNX model.
    /// </summary>
    /// <remarks>
    ///     If the model outputs a sequence of vectors, mean pooling is applied to produce a single
    ///     embedding vector. The length of the returned array corresponds to the embedding dimension expected by the
    ///     model.
    /// </remarks>
    /// <param name="text">The input text to be embedded. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a single-precision floating point
    ///     array representing the embedding vector for the input text.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the tokenizer produces no encoding for the given text, or if the model output shape does not match the
    ///     expected embedding dimension.
    /// </exception>
    public Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // HuggingFace tokenizers return an Encoding (or sequence). We need the token IDs as an indexable list.
        HFTokenizer.Encoding? encoding = _tokenizer.Encode(text, false).FirstOrDefault();
        if (encoding is null)
        {
            throw new InvalidOperationException("Tokenizer produced no encoding for the given text.");
        }

        List<long> tokenIds = encoding.Ids.Select(id => (long)id).ToList();
        if (tokenIds.Count > _maxTokens)
        {
            tokenIds = tokenIds.Take(_maxTokens).ToList();
        }

        var sequenceLength = tokenIds.Count;
        DenseTensor<long> inputIds = new(new[] { 1, sequenceLength });
        DenseTensor<long> attentionMask = new(new[] { 1, sequenceLength });
        DenseTensor<long>? tokenTypeIds =
            _requiresTokenTypeIds ? new DenseTensor<long>(new[] { 1, sequenceLength }) : null;

        for (var i = 0; i < sequenceLength; i++)
        {
            inputIds[0, i] = tokenIds[i];
            attentionMask[0, i] = 1;
            tokenTypeIds?[0, i] = 0;
        }

        List<NamedOnnxValue> inputs = new()
        {
            NamedOnnxValue.CreateFromTensor(_inputIdsName, inputIds),
            NamedOnnxValue.CreateFromTensor(_attentionMaskName, attentionMask)
        };

        if (tokenTypeIds is not null)
        {
            inputs.Add(NamedOnnxValue.CreateFromTensor(_tokenTypeIdsName!, tokenTypeIds));
        }

        using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = _session.Run(inputs);
        var output = results.First(r => r.Name == _outputName).AsEnumerable<float>().ToArray();

        // Model may output sequence; we take mean pooling as a simple, stable strategy.
        if (sequenceLength != 0 && output.Length % sequenceLength == 0 && output.Length != _embeddingDim)
        {
            var hiddenSize = output.Length / sequenceLength;
            var pooled = new float[hiddenSize];
            for (var t = 0; t < sequenceLength; t++)
            for (var h = 0; h < hiddenSize; h++)
                pooled[h] += output[t * hiddenSize + h];

            for (var h = 0; h < hiddenSize; h++) pooled[h] /= sequenceLength;

            return Task.FromResult(pooled);
        }

        // Or model directly outputs a single vector
        return output.Length == _embeddingDim
            ? Task.FromResult(output)
            : throw new InvalidOperationException(
                $"Unexpected embedding output shape. Length={output.Length}, expected {_embeddingDim} or multiple of token count.");
    }
}




/// <summary>
///     ONNX LLM client for a phi-2-like model with a GPT-style interface.
///     Here we assume simple prompt-in / text-out via "input_ids" and "logits" or similar.
///     For simplicity, we implement greedy decoding with a max token limit.
///     You may need to adjust IO names to match your model.
/// </summary>
public sealed class OnnxLLMClient : ILLMClient, IDisposable
{
    private readonly string _attentionMaskName;
    private readonly string _inputIdsName;
    private readonly int _kvDim;
    private readonly TensorElementType _kvElementType;
    private readonly int[]? _kvEmptyShape;
    private readonly int _kvHeads;
    private readonly int _maxNewTokens;
    private readonly int _numPastLayers;
    private readonly string _outputName;
    private readonly string _positionIdsName;
    private readonly bool _requiresAttentionMask;
    private readonly bool _requiresPastKeyValues;
    private readonly bool _requiresPositionIds;
    private readonly InferenceSession _session;
    private readonly Tokenizer _tokenizer;







    public OnnxLLMClient(
        string modelPath,
        Tokenizer tokenizer,
        int maxNewTokens = 256,
        string inputIdsName = "input_ids",
        string outputName = "logits",
        string positionIdsName = "position_ids",
        string attentionMaskName = "attention_mask")
    {
        if (!File.Exists(modelPath))
        {
            throw new FileNotFoundException("LLM model not found", modelPath);
        }

        _session = new InferenceSession(modelPath);
        _tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer));
        _maxNewTokens = maxNewTokens;
        _inputIdsName = inputIdsName;
        _outputName = outputName;
        _positionIdsName = positionIdsName;
        _attentionMaskName = attentionMaskName;

        _requiresPositionIds = _session.InputMetadata.ContainsKey(_positionIdsName);
        _requiresAttentionMask = _session.InputMetadata.ContainsKey(_attentionMaskName);
        _requiresPastKeyValues = _session.InputMetadata.Keys
            .Any(k => k.StartsWith("past_key_values.", StringComparison.Ordinal));

        if (_requiresPastKeyValues)
        {
            KeyValuePair<string, NodeMetadata>[] kvLayerMetas = _session.InputMetadata
                .Where(kvp => kvp.Key.StartsWith("past_key_values.", StringComparison.Ordinal)
                              && kvp.Key.EndsWith(".key", StringComparison.Ordinal))
                .OrderBy(kvp => ParseLayerIndex(kvp.Key))
                .ToArray();

            if (kvLayerMetas.Length == 0)
            {
                throw new InvalidOperationException("Model expects past_key_values inputs but none were found.");
            }

            _numPastLayers = kvLayerMetas.Length;

            NodeMetadata kvMeta = kvLayerMetas[0].Value;
            var dims = kvMeta.Dimensions ?? Array.Empty<int>();
            _kvElementType = kvMeta.ElementDataType;

            if (dims.Length == 5)
            {
                _kvHeads = NormalizeDim(dims[2], 32);
                _kvDim = NormalizeDim(dims[4], 64);
                _kvEmptyShape = new[]
                {
                    NormalizeDim(dims[0], 2), // 2
                    NormalizeDim(dims[1], 1), // batch
                    _kvHeads, // num_heads
                    0, // past_seq_len
                    _kvDim // head_dim
                };
            }
            else if (dims.Length == 4)
            {
                _kvHeads = NormalizeDim(dims[1], 32);
                _kvDim = NormalizeDim(dims[3], 64);
                _kvEmptyShape = new[]
                {
                    NormalizeDim(dims[0], 1), // batch
                    _kvHeads, // num_heads
                    0, // past_seq_len
                    _kvDim // head_dim
                };
            }
            else
            {
                throw new NotSupportedException($"Unsupported past_key_values rank {dims.Length}");
            }
        }
        else
        {
            _numPastLayers = 0;
            _kvHeads = 0;
            _kvDim = 0;
            _kvEmptyShape = null;
            _kvElementType = TensorElementType.Float;
        }
    }







    public void Dispose()
    {
        _session.Dispose();
    }







    /// <summary>
    /// </summary>
    /// <param name="prompt"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> CompleteAsync(string prompt, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Encode prompt
        List<int> promptTokenIds = _tokenizer.EncodeToIds(prompt).ToList();
        List<int> tokens = new(promptTokenIds.Count + _maxNewTokens);
        tokens.AddRange(promptTokenIds);

        // Build completion separately to avoid TrimCompletion/tokenizer round-trip issues
        List<int> completionTokenIds = new(_maxNewTokens);

        // Persist KV cache across steps
        List<DenseTensor<float>> pastKeysFloat = new();
        List<DenseTensor<float>> pastValuesFloat = new();
        List<DenseTensor<Half>> pastKeysHalf = new();
        List<DenseTensor<Half>> pastValuesHalf = new();

        for (var step = 0; step < _maxNewTokens; step++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            OnnxLLMClientInputContext context = new(
                _requiresPastKeyValues,
                _requiresPositionIds,
                _requiresAttentionMask,
                _inputIdsName,
                _positionIdsName,
                _attentionMaskName,
                _numPastLayers,
                _kvEmptyShape,
                _kvElementType);

            List<NamedOnnxValue> inputs = BuildInferenceInputs(
                tokens,
                step,
                context,
                pastKeysFloat,
                pastValuesFloat,
                pastKeysHalf,
                pastValuesHalf);

            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = _session.Run(inputs);

            var nextId = SelectNextToken(results);

            tokens.Add(nextId);
            completionTokenIds.Add(nextId);

            UpdateKvCache(results, pastKeysFloat, pastValuesFloat, pastKeysHalf, pastValuesHalf);

            // NOTE: '0' is not universally EOS. Prefer tokenizer/model-configured EOS IDs.
            // If your tokenizer exposes EOS token id(s), check those here instead.
            if (nextId == 0)
            {
                break;
            }
        }

        var completionText = _tokenizer.Decode(completionTokenIds);
        return Task.FromResult(completionText.Trim());
    }







    private List<NamedOnnxValue> BuildInferenceInputs(
        IList<int> tokens,
        int step,
        OnnxLLMClientInputContext context,
        List<DenseTensor<float>>? pastKeysFloat,
        List<DenseTensor<float>>? pastValuesFloat,
        List<DenseTensor<Half>>? pastKeysHalf,
        List<DenseTensor<Half>>? pastValuesHalf)
    {
        return BuildInferenceInputsCore(
            tokens,
            step,
            context,
            pastKeysFloat,
            pastValuesFloat,
            pastKeysHalf,
            pastValuesHalf);
    }







    internal static List<NamedOnnxValue> BuildInferenceInputsCore(
        IList<int> tokens,
        int step,
        OnnxLLMClientInputContext context,
        List<DenseTensor<float>>? pastKeysFloat,
        List<DenseTensor<float>>? pastValuesFloat,
        List<DenseTensor<Half>>? pastKeysHalf,
        List<DenseTensor<Half>>? pastValuesHalf)
    {
        if (tokens == null || tokens.Count == 0)
        {
            throw new ArgumentException("Token list is null or empty.", nameof(tokens));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        static bool HasValidCache<T>(IReadOnlyList<DenseTensor<T>>? keys, IReadOnlyList<DenseTensor<T>>? values,
            int expectedLayers)
            where T : struct
        {
            if (keys == null || values == null || keys.Count != expectedLayers || values.Count != expectedLayers)
            {
                return false;
            }

            for (var layer = 0; layer < expectedLayers; layer++)
                if (keys[layer] == null || values[layer] == null || keys[layer].Length == 0 ||
                    values[layer].Length == 0)
                {
                    return false;
                }

            return true;
        }

        var contextLen = tokens.Count;
        var useIncremental = context.RequiresPastKeyValues && step > 0;
        var currentLen = useIncremental ? 1 : contextLen;
        var startIndex = contextLen - currentLen;

        // Core inputs
        DenseTensor<long> inputTensor = new(new[] { 1, currentLen });
        DenseTensor<long>? positionIds =
            context.RequiresPositionIds ? new DenseTensor<long>(new[] { 1, currentLen }) : null;
        DenseTensor<long>? attentionMask =
            context.RequiresAttentionMask ? new DenseTensor<long>(new[] { 1, contextLen }) : null;

        for (var i = 0; i < currentLen; i++)
        {
            var tokenIndex = startIndex + i;
            inputTensor[0, i] = tokens[tokenIndex];
            positionIds?[0, i] = tokenIndex;
        }

        if (attentionMask != null)
        {
            for (var i = 0; i < contextLen; i++) attentionMask[0, i] = 1;
        }

        List<NamedOnnxValue> inputs = new()
        {
            NamedOnnxValue.CreateFromTensor(context.InputIdsName, inputTensor)
        };

        if (positionIds != null)
        {
            inputs.Add(NamedOnnxValue.CreateFromTensor(context.PositionIdsName, positionIds));
        }

        if (attentionMask != null)
        {
            inputs.Add(NamedOnnxValue.CreateFromTensor(context.AttentionMaskName, attentionMask));
        }

        // --- KV CACHE HANDLING ---

        if (context.RequiresPastKeyValues)
        {
            if (context.KvEmptyShape == null || context.KvEmptyShape.Length == 0)
            {
                throw new InvalidOperationException("KV cache shape (context.KvEmptyShape) is not initialized.");
            }

            if (context.KvElementType == TensorElementType.Float16)
            {
                if (HasValidCache(pastKeysHalf, pastValuesHalf, context.NumPastLayers))
                {
                    AddCachedKvInputs(inputs, pastKeysHalf!, pastValuesHalf!);
                }
                else
                {
                    if (step != 0)
                    {
                        throw new InvalidOperationException("Half KV cache is missing after initial decoding.");
                    }

                    AddEmptyKvInputs<Half>(inputs, context.NumPastLayers, context.KvEmptyShape);
                }
            }
            else if (context.KvElementType == TensorElementType.Float)
            {
                if (HasValidCache(pastKeysFloat, pastValuesFloat, context.NumPastLayers))
                {
                    AddCachedKvInputs(inputs, pastKeysFloat!, pastValuesFloat!);
                }
                else
                {
                    if (step != 0)
                    {
                        throw new InvalidOperationException("Float KV cache is missing after initial decoding.");
                    }

                    AddEmptyKvInputs<float>(inputs, context.NumPastLayers, context.KvEmptyShape);
                }
            }
            else
            {
                throw new NotSupportedException($"Unsupported KV element type: {context.KvElementType}");
            }
        }

        // Final guard: ensure required KV inputs are present if model expects them
        if (context.RequiresPastKeyValues && step > 0)
        {
            var hasKv0Key = inputs.Any(i => i.Name == "past_key_values.0.key");
            if (!hasKv0Key)
            {
                var names = string.Join(", ", inputs.Select(i => i.Name));
                throw new InvalidOperationException(
                    $"Model requires past_key_values but inputs do not contain 'past_key_values.0.key'. " +
                    $"Inputs: {names}");
            }
        }

        return inputs;
    }







    /// <summary>
    /// </summary>
    /// <param name="results"></param>
    /// <returns></returns>
    private int SelectNextToken(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results)
    {
        Tensor<float> logitsTensor = results.First(r => r.Name == _outputName).AsTensor<float>();
        var vocabSize = logitsTensor.Dimensions[^1];
        var logits = logitsTensor.ToArray();
        ArraySegment<float> lastLogits = new(logits, logits.Length - vocabSize, vocabSize);
        return ArgMax(lastLogits);
    }







    private void UpdateKvCache(
        IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results,
        List<DenseTensor<float>>? pastKeysFloat,
        List<DenseTensor<float>>? pastValuesFloat,
        List<DenseTensor<Half>>? pastKeysHalf,
        List<DenseTensor<Half>>? pastValuesHalf)
    {
        if (!_requiresPastKeyValues)
        {
            return;
        }

        if (_kvElementType == TensorElementType.Float)
        {
            RefreshCache(results, pastKeysFloat!, pastValuesFloat!);
        }
        else
        {
            RefreshCache(results, pastKeysHalf!, pastValuesHalf!);
        }
    }







    private void RefreshCache<T>(
        IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results,
        List<DenseTensor<T>> pastKeys,
        List<DenseTensor<T>> pastValues)
        where T : struct
    {
        pastKeys.Clear();
        pastValues.Clear();

        for (var layer = 0; layer < _numPastLayers; layer++)
        {
            pastKeys.Add(CloneTensor(results.First(r => r.Name == $"present.{layer}.key").AsTensor<T>()));
            pastValues.Add(CloneTensor(results.First(r => r.Name == $"present.{layer}.value").AsTensor<T>()));
        }
    }







    private static DenseTensor<T> CloneTensor<T>(Tensor<T> tensor)
        where T : struct
    {
        var dims = tensor.Dimensions.ToArray();
        T[] data = tensor.ToArray();
        return new DenseTensor<T>(data, dims);
    }







    internal static int ArgMax(ArraySegment<float> logits)
    {
        var bestIndex = 0;
        var bestValue = logits[0];
        for (var i = 1; i < logits.Count; i++)
            if (logits[i] > bestValue)
            {
                bestValue = logits[i];
                bestIndex = i;
            }

        return bestIndex;
    }







    internal static string TrimCompletion(string full, string prompt)
    {
        return full.StartsWith(prompt, StringComparison.Ordinal) ? full.Substring(prompt.Length).Trim() : full.Trim();
    }







    private static int NormalizeDim(int metaDim, int fallback)
    {
        return metaDim > 0 ? metaDim : fallback;
    }







    internal static int ParseLayerIndex(string inputName)
    {
        const string prefix = "past_key_values.";
        var start = prefix.Length;
        var end = inputName.IndexOf('.', start);
        if (start >= inputName.Length || end <= start)
        {
            return 0;
        }

        var slice = inputName.Substring(start, end - start);
        return int.TryParse(slice, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
            ? value
            : 0;
    }







    /// <summary>
    ///     Creates an empty KV tensor of the specified shape and type.
    /// </summary>
    /// <typeparam name="T">The value type (float or Half).</typeparam>
    /// <param name="shape">The shape of the tensor.</param>
    /// <returns>A DenseTensor of the specified type and shape, filled with default values.</returns>
    private static DenseTensor<T> CreateEmptyKv<T>(int[] shape) where T : struct
    {
        return shape == null || shape.Length == 0
            ? throw new ArgumentException("Shape must be a non-empty array.", nameof(shape))
            : new DenseTensor<T>(shape);
    }







    private static void AddEmptyKvInputs<T>(List<NamedOnnxValue> inputs, int numLayers, int[] shape)
        where T : struct
    {
        if (shape == null)
        {
            throw new ArgumentNullException(nameof(shape));
        }

        // ONNX Runtime cannot marshal Half tensors if any dimension is zero-length.
        // Some models advertise a dynamic past_seq_len as 0 for the empty cache, so coerce it to 1.
        var safeShape = (int[])shape.Clone();
        if (safeShape.Length >= 3 && safeShape[2] == 0)
        {
            safeShape[2] = 1;
        }

        for (var layer = 0; layer < numLayers; layer++)
        {
            inputs.Add(NamedOnnxValue.CreateFromTensor($"past_key_values.{layer}.key", CreateEmptyKv<T>(safeShape)));
            inputs.Add(NamedOnnxValue.CreateFromTensor($"past_key_values.{layer}.value", CreateEmptyKv<T>(safeShape)));
        }
    }







    private static void AddCachedKvInputs<T>(List<NamedOnnxValue> inputs, IReadOnlyList<DenseTensor<T>> keys,
        IReadOnlyList<DenseTensor<T>> values)
        where T : struct
    {
        if (keys.Count != values.Count)
        {
            throw new InvalidOperationException("KV cache keys and values counts do not match.");
        }

        for (var layer = 0; layer < keys.Count; layer++)
        {
            inputs.Add(NamedOnnxValue.CreateFromTensor($"past_key_values.{layer}.key", keys[layer]));
            inputs.Add(NamedOnnxValue.CreateFromTensor($"past_key_values.{layer}.value", values[layer]));
        }
    }
}




internal sealed record OnnxLLMClientInputContext(
    bool RequiresPastKeyValues,
    bool RequiresPositionIds,
    bool RequiresAttentionMask,
    string InputIdsName,
    string PositionIdsName,
    string AttentionMaskName,
    int NumPastLayers,
    int[]? KvEmptyShape,
    TensorElementType KvElementType);