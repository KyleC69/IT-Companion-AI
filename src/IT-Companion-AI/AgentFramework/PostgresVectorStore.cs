#if false
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Connectors.PgVector;

using SkAgentGroup.AgentFramework.Memory;

namespace SkAgentGroup.AgentFramework;

/// <summary>
/// Minimal Postgres-backed vector store for <see cref="AgentMemoryRecord"/>.
/// Requires the <c>pgvector</c> extension installed in the target database.
/// Class renamed to RagPostgresVectorStore to avoid collisions with existing PostgresVectorStore class.
/// </summary>
public sealed class RagPostgresVectorStore : IAsyncDisposable
{
    private readonly PostgresVectorStore _store;
    private readonly PostgresVectorStoreRecordCollection<string, AgentMemoryRecord> _collection;

    /// <summary>
    /// Creates a Postgres-backed vector store wrapper.
    /// </summary>
    /// <param name="connectionString">Connection string to the Postgres database with pgvector enabled.</param>
    /// <param name="options">Optional PgVector store options.</param>
    /// <param name="collectionName">Collection name used to persist <see cref="AgentMemoryRecord"/> vectors.</param>
    public RagPostgresVectorStore(
        string connectionString,
        PostgresVectorStoreOptions? options = null,
        string collectionName = "agent_memory")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        _store = options is null
            ? new PostgresVectorStore(connectionString)
            : new PostgresVectorStore(connectionString, options);

        _collection = _store.GetCollection<string, AgentMemoryRecord>(collectionName);
    }

    /// <summary>
    /// Gets the underlying record collection.
    /// </summary>
    public PostgresVectorStoreRecordCollection<string, AgentMemoryRecord> Collection => _collection;

    /// <summary>
    /// Upserts a memory record into the backing collection.
    /// </summary>
    public Task UpsertAsync(AgentMemoryRecord record, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(record);
        return _collection.UpsertAsync(record, cancellationToken);
    }

    /// <summary>
    /// Searches the collection using the provided vector, returning up to <paramref name="limit"/> results.
    /// </summary>
    public async Task<IReadOnlyList<(AgentMemoryRecord Record, double Score)>> SearchAsync(
        ReadOnlyMemory<float> embedding,
        int limit,
        CancellationToken cancellationToken = default)
    {
        if (limit <= 0)
        {
            return Array.Empty<(AgentMemoryRecord, double)>();
        }

        var results = new List<(AgentMemoryRecord, double)>(capacity: limit);

        await foreach (var item in _collection.VectorSearchAsync(embedding, limit, cancellationToken: cancellationToken)
            .ConfigureAwait(false))
        {
            results.Add((item.Record, item.Score));
        }

        return results;
    }

    public ValueTask DisposeAsync() => _store.DisposeAsync();
}
#endif
