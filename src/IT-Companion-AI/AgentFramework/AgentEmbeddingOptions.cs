namespace SkAgentGroup.AgentFramework;


public sealed class AgentEmbeddingOptions
{
    public string AgentId { get; init; } = string.Empty;
    public string AgentName { get; init; } = string.Empty;

    // Backend / model control
    public string? ModelId { get; init; }           // e.g. "text-embedding-3-small"
    public string? BackendName { get; init; }       // e.g. "openai", "local-gguf", "azure-openai"

    // Behavior flags
    public bool LogEmbeddingRequests { get; init; } = false;
    public bool IncludeAgentMetadataInPayload { get; init; } = true;

    // Any other knobs you’ll want later
    public IDictionary<string, object> CustomSettings { get; }
        = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
}

// This is what your agents deal with when *they* request embeddings
public sealed class AgentEmbeddingRequest
{
    public required string Text { get; init; }

    // mandatory agent metadata
    public required string AgentId { get; init; }
    public required string AgentName { get; init; }

    // optional specialization
    public AgentEmbeddingOptions? Options { get; init; }
}
