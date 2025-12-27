using Microsoft.Extensions.VectorData;

namespace SkAgentGroup.AgentFramework.Memory;

public sealed class AgentMemoryRecord
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");

    public required string AgentId { get; init; }
    public required string AgentName { get; init; }

    [VectorStoreData]
    public required string Text { get; init; }

    // Optional: classify by type (episodic, declarative, plan, intent, etc.)
    public string MemoryType { get; init; } = "episodic";

    public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow;

    // Arbitrary tags (e.g., topic, fileId, conversationId)
    public IDictionary<string, string> Tags { get; init; } =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    [VectorStoreVector(256)]
    public ReadOnlyMemory<float> Vector { get; internal set; }


    [VectorStoreKey]
    public string Key { get; internal set; }

    public Task<IReadOnlyList<AgentMemoryRecord>> GetRecentAsync(string agentId, int count, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<AgentMemoryRecord>> SearchAsync(string agentId, string query, int topK, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task StoreAsync(AgentMemoryRecord record, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}












