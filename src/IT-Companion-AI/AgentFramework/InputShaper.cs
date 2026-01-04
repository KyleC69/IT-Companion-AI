// Project Name: SKAgent
// File Name: InputShaper.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using ITCompanionAI.AgentFramework;

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Tokenizers;



public sealed class InputShaper
{
    private readonly ModelConfig _config;
    private readonly InferenceSession _session;
    private readonly Tokenizer _tokenizer;





    public InputShaper(InferenceSession session, ModelConfig config, Tokenizer tokenizer)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer));
    }





    /// <summary>
    ///     Build inputs for the initial forward pass (no prior KV cache).
    /// </summary>
    public IReadOnlyList<NamedOnnxValue> BuildInitialInputs(string prompt)
    {
        if (prompt is null)
        {
            throw new ArgumentNullException(nameof(prompt));
        }

        List<int> tokenIds = _tokenizer.EncodeToIds(prompt).ToList();
        if (tokenIds.Count == 0)
        {
            throw new InvalidOperationException("Tokenizer produced no tokens for the given prompt.");
        }

        const int batchSize = 1;
        var seqLen = tokenIds.Count;

        IReadOnlyDictionary<string, NodeMetadata> meta = _session.InputMetadata;
        List<NamedOnnxValue> inputs = new(meta.Count);

        foreach (KeyValuePair<string, NodeMetadata> kvp in meta)
        {
            var name = kvp.Key;
            NodeMetadata nodeMeta = kvp.Value;
            TensorElementType elementType = nodeMeta.ElementDataType;
            var dims = nodeMeta.Dimensions ?? Array.Empty<int>();

            if (name.Equals(_config.InputIdsName, StringComparison.Ordinal))
            {
                var resolved = ResolveTokenDims(name, dims, batchSize, seqLen);
                inputs.Add(CreateInputIds(name, elementType, resolved, tokenIds));
                continue;
            }

            if (name.Equals(_config.AttentionMaskName, StringComparison.Ordinal))
            {
                var resolved = ResolveTokenDims(name, dims, batchSize, seqLen);
                inputs.Add(CreateAttentionMask(name, elementType, resolved, seqLen));
                continue;
            }

            if (!string.IsNullOrEmpty(_config.PositionIdsName) &&
                name.Equals(_config.PositionIdsName, StringComparison.Ordinal))
            {
                var resolved = ResolveTokenDims(name, dims, batchSize, seqLen);
                inputs.Add(CreatePositionIds(name, elementType, resolved, seqLen));
                continue;
            }

            if (IsPastKeyName(name) || IsPastValueName(name))
            {
                // IMPORTANT:
                // Do not provide past_key_values.* tensors on the initial pass.
                // For Float16 models, ONNX Runtime can fail marshalling empty/zero-length tensors.
                // Let the model run without a cache; cache will be provided on subsequent steps.
                continue;
            }

            // Everything else: zero-init based on metadata.
            var defaultResolved = ResolveGenericDims(name, dims, batchSize, seqLen);
            inputs.Add(CreateZeroTensor(name, elementType, defaultResolved));
        }

        ValidateProvidedInputsAgainstMetadata(inputs, meta);

        return inputs;
    }





    // ===================== DIM RESOLUTION =====================





    private static int[] ResolveTokenDims(string name, IReadOnlyList<int> dims, int batchSize, int seqLen)
    {
        if (dims.Count == 0)
        {
            return new[] { batchSize, seqLen }; // conservative fallback
        }

        var resolved = new int[dims.Count];
        for (var i = 0; i < dims.Count; i++)
        {
            var d = dims[i];
            if (d != -1)
            {
                resolved[i] = d;
                continue;
            }

            // For token-like tensors, assume [batch, seq, ...]
            resolved[i] = i == 0 ? batchSize : i == 1 ? seqLen : 1;
        }

        return resolved;
    }





    /// <summary>
    ///     Resolve KV cache dims using model config first, then metadata.
    /// </summary>
    private int[] ResolveKvDims(string name, IReadOnlyList<int> dims, int batchSize)
    {
        if (dims.Count == 0 && _config.KvEmptyShape.Length == 0)
        {
            throw new InvalidOperationException(
                $"KV input '{name}' has no dims in metadata and KvEmptyShape is not configured.");
        }

        // Prefer explicit KvEmptyShape if rank matches.
        if (_config.KvEmptyShape.Length == dims.Count && _config.KvEmptyShape.Length > 0)
        {
            var resolved = new int[dims.Count];
            for (var i = 0; i < dims.Count; i++)
            {
                var metaDim = dims[i];
                var cfgDim = _config.KvEmptyShape[i];

                if (metaDim != -1 && metaDim != cfgDim)
                {
                    throw new InvalidOperationException(
                        $"KV dim mismatch for '{name}' axis {i}. Metadata: {metaDim}, KvEmptyShape: {cfgDim}.");
                }

                resolved[i] = cfgDim;
            }

            // Normalize batch dim to requested batchSize if needed.
            if (resolved[0] != batchSize && resolved[0] != -1)
            {
                resolved[0] = batchSize;
            }

            return resolved;
        }

        // Fallback: infer from metadata + config.
        // Expected pattern (common): [batch, num_kv_heads, past_seq_len, head_dim]
        var r = new int[dims.Count];
        for (var i = 0; i < dims.Count; i++)
        {
            var d = dims[i];
            if (d != -1)
            {
                r[i] = d;
                continue;
            }

            r[i] = i switch
            {
                0 => batchSize,
                1 => _config.NumKeyValueHeads > 0
                    ? _config.NumKeyValueHeads
                    : Math.Max(_config.NumAttentionHeads, 1), // number of heads: prefer NumKeyValueHeads if non-zero
                2 => 0, // initial past sequence length is zero
                3 => _config.HeadDim > 0 ? _config.HeadDim : 1, // head dim from config
                _ => 1
            };
        }

        return r;
    }





    private static int[] ResolveGenericDims(string name, IReadOnlyList<int> dims, int batchSize, int seqLen)
    {
        if (dims.Count == 0)
        {
            return Array.Empty<int>();
        }

        var resolved = new int[dims.Count];
        for (var i = 0; i < dims.Count; i++)
        {
            var d = dims[i];
            if (d != -1)
            {
                resolved[i] = d;
                continue;
            }

            resolved[i] = i == 0 ? batchSize : i == 1 ? seqLen : 1;
        }

        return resolved;
    }





    private bool IsPastKeyName(string name)
    {
        if (string.IsNullOrEmpty(_config.PastKeyFormat))
        {
            return false;
        }

        // PastKeyFormat expected like "past_key_values.{0}.key"
        // We simply check if the formatted pattern with any valid layer matches.
        for (var layer = 0; layer < _config.NumLayers; layer++)
        {
            var expected = string.Format(_config.PastKeyFormat, layer);
            if (name.Equals(expected, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }





    private bool IsPastValueName(string name)
    {
        if (string.IsNullOrEmpty(_config.PastValueFormat))
        {
            return false;
        }

        for (var layer = 0; layer < _config.NumLayers; layer++)
        {
            var expected = string.Format(_config.PastValueFormat, layer);
            if (name.Equals(expected, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }





    // ===================== SEMANTIC TENSORS =====================





    private static NamedOnnxValue CreateInputIds(
        string name,
        TensorElementType elementType,
        int[] dims,
        IReadOnlyList<int> tokenIds)
    {
        var seqLen = tokenIds.Count;

        return elementType switch
        {
            TensorElementType.Int64 => CreateTensor(name, dims, (DenseTensor<long> t) =>
            {
                for (var i = 0; i < seqLen; i++) t[0, i] = tokenIds[i];
            }),
            TensorElementType.Int32 => CreateTensor(name, dims, (DenseTensor<int> t) =>
            {
                for (var i = 0; i < seqLen; i++) t[0, i] = tokenIds[i];
            }),
            _ => throw new NotSupportedException(
                $"Input '{name}' (input_ids) has element type {elementType}. Only Int32/Int64 are supported.")
        };
    }





    private static NamedOnnxValue CreateAttentionMask(
        string name,
        TensorElementType elementType,
        int[] dims,
        int seqLen)
    {
        return elementType switch
        {
            TensorElementType.Int64 => CreateTensor(name, dims, (DenseTensor<long> t) =>
            {
                for (var i = 0; i < seqLen; i++) t[0, i] = 1L;
            }),
            TensorElementType.Int32 => CreateTensor(name, dims, (DenseTensor<int> t) =>
            {
                for (var i = 0; i < seqLen; i++) t[0, i] = 1;
            }),
            TensorElementType.Bool => CreateTensor(name, dims, (DenseTensor<bool> t) =>
            {
                for (var i = 0; i < seqLen; i++) t[0, i] = true;
            }),
            _ => throw new NotSupportedException(
                $"Input '{name}' (attention_mask) has element type {elementType}. Only Int32/Int64/Bool are supported.")
        };
    }





    private static NamedOnnxValue CreatePositionIds(
        string name,
        TensorElementType elementType,
        int[] dims,
        int seqLen)
    {
        return elementType switch
        {
            TensorElementType.Int64 => CreateTensor(name, dims, (DenseTensor<long> t) =>
            {
                for (var i = 0; i < seqLen; i++) t[0, i] = i;
            }),
            TensorElementType.Int32 => CreateTensor(name, dims, (DenseTensor<int> t) =>
            {
                for (var i = 0; i < seqLen; i++) t[0, i] = i;
            }),
            _ => throw new NotSupportedException(
                $"Input '{name}' (position_ids) has element type {elementType}. Only Int32/Int64 are supported.")
        };
    }





    private NamedOnnxValue CreateInitialKvTensor(
        string name,
        TensorElementType elementTypeFromMeta,
        int[] dims)
    {
        if (dims == null || dims.Length == 0)
        {
            throw new ArgumentException("KV tensor dimensions must be provided.", nameof(dims));
        }

        // If config declares a KV type, validate it matches metadata.
        // Otherwise, metadata is the source of truth.
        TensorElementType type = _config.KvElementType != default ? _config.KvElementType : elementTypeFromMeta;

        if (_config.KvElementType != default && _config.KvElementType != elementTypeFromMeta)
        {
            throw new InvalidOperationException(
                $"KV input '{name}' element type mismatch. " +
                $"Model metadata: {elementTypeFromMeta}, Config: {_config.KvElementType}.");
        }

        // ONNX Runtime cannot marshal Float16 tensors when any axis has length 0, so coerce to 1.
        var safeDims = (int[])dims.Clone();
        if (type == TensorElementType.Float16)
        {
            for (var i = 0; i < safeDims.Length; i++)
                if (safeDims[i] == 0)
                {
                    safeDims[i] = 1;
                }
        }

        return CreateZeroTensor(name, type, safeDims);
    }





    // ===================== GENERIC TENSORS =====================





    private static NamedOnnxValue CreateZeroTensor(
        string name,
        TensorElementType elementType,
        int[] dims)
    {
        return elementType switch
        {
            TensorElementType.Int64 => CreateTensor(name, dims, (DenseTensor<long> _) => { }),
            TensorElementType.Int32 => CreateTensor(name, dims, (DenseTensor<int> _) => { }),
            TensorElementType.Float => CreateTensor(name, dims, (DenseTensor<float> _) => { }),
            TensorElementType.Float16 => CreateTensor(name, dims, (DenseTensor<Half> _) => { }),
            TensorElementType.Bool => CreateTensor(name, dims, (DenseTensor<bool> _) => { }),
            _ => throw new NotSupportedException(
                $"Input '{name}' has unsupported element type {elementType} in CreateZeroTensor.")
        };
    }





    private static NamedOnnxValue CreateTensor<T>(
        string name,
        int[] dims,
        Action<DenseTensor<T>> fill)
    {
        DenseTensor<T> tensor = new(dims);
        fill(tensor);
        return NamedOnnxValue.CreateFromTensor(name, tensor);
    }





    // ===================== VALIDATION =====================





    private static void ValidateProvidedInputsAgainstMetadata(
        IReadOnlyCollection<NamedOnnxValue> inputs,
        IReadOnlyDictionary<string, NodeMetadata> meta)
    {
        Dictionary<string, NamedOnnxValue> dict = inputs.ToDictionary(i => i.Name, StringComparer.Ordinal);

        // Require core inputs.
        foreach (var required in new[] { "input_ids", "attention_mask" })
            if (!dict.ContainsKey(required))
            {
                throw new InvalidOperationException($"Missing required input '{required}'.");
            }

        // Validate only what we provide.
        foreach (NamedOnnxValue input in inputs)
        {
            if (!meta.TryGetValue(input.Name, out NodeMetadata? nodeMeta))
            {
                throw new InvalidOperationException($"Input '{input.Name}' is not present in model input metadata.");
            }

            var expectedDims = nodeMeta.Dimensions ?? Array.Empty<int>();
            var actualDims = GetShape(input, nodeMeta.ElementDataType);

            if (expectedDims.Length != actualDims.Length)
            {
                throw new InvalidOperationException(
                    $"Rank mismatch for '{input.Name}'. Expected {expectedDims.Length}, actual {actualDims.Length}.");
            }

            for (var i = 0; i < expectedDims.Length; i++)
            {
                var expected = expectedDims[i];
                var actual = actualDims[i];

                if (expected == -1)
                {
                    continue;
                }

                if (expected != actual)
                {
                    throw new InvalidOperationException(
                        $"Dim mismatch for '{input.Name}' axis {i}. Expected {expected}, actual {actual}.");
                }
            }
        }
    }





    private static int[] GetShape(NamedOnnxValue value, TensorElementType elementType)
    {
        return elementType switch
        {
            TensorElementType.Int64 => value.AsTensor<long>().Dimensions.ToArray(),
            TensorElementType.Int32 => value.AsTensor<int>().Dimensions.ToArray(),
            TensorElementType.Float => value.AsTensor<float>().Dimensions.ToArray(),
            TensorElementType.Float16 => value.AsTensor<Half>().Dimensions.ToArray(),
            TensorElementType.Bool => value.AsTensor<bool>().Dimensions.ToArray(),
            _ => throw new NotSupportedException(
                $"Unsupported element type {elementType} when reading shape for '{value.Name}'.")
        };
    }
}