namespace SkAgentGroup.AgentFramework.Memory;

public interface IAgentMemory
    {
        Task StoreAsync(AgentMemoryRecord record, CancellationToken ct = default);
        Task<IReadOnlyList<AgentMemoryRecord>> SearchAsync(string agentId, string query, int topK, CancellationToken ct = default);
        Task<IReadOnlyList<AgentMemoryRecord>> GetRecentAsync(string agentId, int count, CancellationToken ct = default);
    }












