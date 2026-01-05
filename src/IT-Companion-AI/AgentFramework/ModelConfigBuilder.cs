// Project Name: SKAgent
// File Name: ModelConfigBuilder.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Text.Json;

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;


namespace ITCompanionAI.AgentFramework;


public sealed class ModelConfig
{
    public int NumLayers { get; init; }
    public int NumAttentionHeads { get; init; }
    public int NumKeyValueHeads { get; init; }
    public int HiddenSize { get; init; }
    public int HeadDim { get; init; }
    public int VocabSize { get; init; }
    public int MaxPositionEmbeddings { get; init; }

    public string InputIdsName { get; init; } = "";
    public string AttentionMaskName { get; init; } = "";
    public string PositionIdsName { get; init; } = "";

    public string PastKeyFormat { get; init; } = "";
    public string PastValueFormat { get; init; } = "";
    public string PresentKeyFormat { get; init; } = "";
    public string PresentValueFormat { get; init; } = "";

    public int[] KvEmptyShape { get; init; } = Array.Empty<int>();
    public TensorElementType KvElementType { get; init; }
}




public sealed class ModelConfigBuilder
{
    private readonly string _configPath;
    private readonly InferenceSession _session;







    public ModelConfigBuilder(string configPath, InferenceSession session)
    {
        if (string.IsNullOrWhiteSpace(configPath))
        {
            throw new ArgumentException("Config path cannot be null or empty.", nameof(configPath));
        }

        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException("Config file not found.", configPath);
        }

        _configPath = configPath;
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }







    public ModelConfig Build()
    {
        var json = File.ReadAllText(_configPath);
        using var doc = JsonDocument.Parse(json);

        JsonElement root = doc.RootElement;

        // -------------------------------
        // 1. Extract config.json values
        // -------------------------------
        var numLayers = GetInt(root, "num_hidden_layers");
        var numHeads = GetInt(root, "num_attention_heads");
        var numKvHeads = GetInt(root, "num_key_value_heads");
        var hiddenSize = GetInt(root, "hidden_size");
        var vocabSize = GetInt(root, "vocab_size");
        var maxPos = GetInt(root, "max_position_embeddings");

        // head_dim = hidden_size / num_attention_heads
        var headDim = hiddenSize / numHeads;

        // -------------------------------
        // 2. Extract ONNX metadata
        // -------------------------------
        IReadOnlyDictionary<string, NodeMetadata>? inputs = _session.InputMetadata;

        var inputIdsName = FindInputName(inputs, "input_ids");
        var attentionMaskName = FindInputName(inputs, "attention_mask");
        var positionIdsName = FindInputName(inputs, "position_ids");

        // KV naming formats
        var pastKeyFormat = DetectFormat(inputs.Keys, ".key");
        var pastValueFormat = DetectFormat(inputs.Keys, ".value");

        // Present outputs
        var presentKeyFormat = DetectFormat(_session.OutputMetadata.Keys, ".key");
        var presentValueFormat = DetectFormat(_session.OutputMetadata.Keys, ".value");

        // -------------------------------
        // 3. Determine KV layer count
        // -------------------------------
        var kvLayers = inputs.Keys
            .Where(k => k.Contains(".key") && k.Contains("past_key_values"))
            .Select(k => int.Parse(k.Split('.')[1]))
            .Distinct()
            .OrderBy(i => i)
            .ToArray();

        if (kvLayers.Length == 0)
        {
            throw new InvalidOperationException("ONNX model exposes no past_key_values.* inputs.");
        }

        var onnxLayerCount = kvLayers.Length;

        if (onnxLayerCount != numLayers)
        {
            throw new InvalidOperationException(
                $"Layer count mismatch: config.json says {numLayers}, ONNX exposes {onnxLayerCount} KV layers.");
        }

        // -------------------------------
        // 4. Determine KV element type
        // -------------------------------
        NodeMetadata firstKv = inputs[pastKeyFormat.Replace("%d", "0")];
        TensorElementType kvElementType = firstKv.ElementDataType as TensorElementType? ??
                                          throw new InvalidOperationException("Unable to determine KV element type.");



        // -------------------------------
        // 5. Determine KV empty shape
        // -------------------------------
        var kvShape = firstKv.Dimensions.ToArray();

        // Replace past_seq_len with 0
        if (kvShape.Length < 4)
        {
            throw new InvalidOperationException("KV tensor shape is invalid.");
        }

        kvShape[^2] = 0; // past_seq_len dimension

        // -------------------------------
        // 6. Build definitive config
        // -------------------------------
        return new ModelConfig
        {
            NumLayers = numLayers,
            NumAttentionHeads = numHeads,
            NumKeyValueHeads = numKvHeads,
            HiddenSize = hiddenSize,
            HeadDim = headDim,
            VocabSize = vocabSize,
            MaxPositionEmbeddings = maxPos,

            InputIdsName = inputIdsName,
            AttentionMaskName = attentionMaskName,
            PositionIdsName = positionIdsName,

            PastKeyFormat = pastKeyFormat,
            PastValueFormat = pastValueFormat,
            PresentKeyFormat = presentKeyFormat,
            PresentValueFormat = presentValueFormat,

            KvEmptyShape = kvShape,
            KvElementType = kvElementType
        };
    }







    // -------------------------------
    // Helpers
    // -------------------------------







    private static int GetInt(JsonElement root, string name)
    {
        return !root.TryGetProperty(name, out JsonElement prop)
            ? throw new InvalidOperationException($"Missing required config field '{name}'.")
            : !prop.TryGetInt32(out var value)
                ? throw new InvalidOperationException($"Config field '{name}' is not an integer.")
                : value;
    }







    private static string FindInputName(IReadOnlyDictionary<string, NodeMetadata> inputs, string expected)
    {
        var match = inputs.Keys.FirstOrDefault(k => k.Equals(expected, StringComparison.OrdinalIgnoreCase));
        return match ?? throw new InvalidOperationException($"ONNX model does not expose required input '{expected}'.");
    }







    private static string DetectFormat(IEnumerable<string> names, string suffix)
    {
        foreach (var name in names)
        {
            if (name.Contains("%d"))
            {
                return name;
            }

            // Example: past_key_values.0.key → convert to past_key_values.%d.key
            if (name.Contains(suffix) && name.Any(char.IsDigit))
            {
                var parts = name.Split('.');
                if (parts.Length >= 3 && int.TryParse(parts[1], out _))
                {
                    parts[1] = "%d";
                    return string.Join(".", parts);
                }
            }
        }

        throw new InvalidOperationException($"Unable to detect KV naming format for suffix '{suffix}'.");
    }
}