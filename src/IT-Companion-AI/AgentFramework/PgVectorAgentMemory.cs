using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;

using SkAgentGroup.AgentFramework.Memory;

namespace SkAgentGroup.AgentFramework;

public sealed class PgVectorAgentMemory : IAgentMemory
{
    private readonly VectorStoreCollection<string, AgentMemoryRecord> _collection;
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;

    public PgVectorAgentMemory(
        VectorStoreCollection<string, AgentMemoryRecord> collection,
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
    {
        _collection = collection;
        _embeddingGenerator = embeddingGenerator;
    }

    public async Task StoreAsync(
        AgentMemoryRecord record,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(record);

        var embedding = await _embeddingGenerator.GenerateAsync(
            record.Text,
            options: null,
            cancellationToken: cancellationToken);

        record.Vector = embedding.Vector;

        await _collection.UpsertAsync(record, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<AgentMemoryRecord>> SearchAsync(
        string agentId,
        string query,
        int topK = 5,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(agentId);
        ArgumentNullException.ThrowIfNull(query);

        var queryEmbedding = await _embeddingGenerator.GenerateAsync(
            query,
            options: null,
            cancellationToken: cancellationToken);

        var results = new List<AgentMemoryRecord>(capacity: Math.Max(1, topK));

        await foreach (var item in _collection.SearchAsync(queryEmbedding.Vector, topK, cancellationToken: cancellationToken)
            .ConfigureAwait(false))
        {
            if (!string.Equals(item.Record.AgentId, agentId, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            results.Add(item.Record);
        }

        return results;
    }

    public async Task<IReadOnlyList<AgentMemoryRecord>> GetRecentAsync(
        string agentId,
        int count = 10,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(agentId);

        var queryEmbedding = await _embeddingGenerator.GenerateAsync(
            string.Empty,
            options: null,
            cancellationToken: cancellationToken);

        var results = new List<AgentMemoryRecord>(capacity: Math.Max(1, count));

        await foreach (var item in _collection.SearchAsync(queryEmbedding.Vector, count, cancellationToken: cancellationToken)
            .ConfigureAwait(false))
        {
            if (string.Equals(item.Record.AgentId, agentId, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(item.Record);
            }
        }

        return results
            .OrderByDescending(r => r.CreatedAtUtc)
            .Take(count)
            .ToList();
    }
}

