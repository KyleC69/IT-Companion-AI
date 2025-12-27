#pragma warning disable SKEXP0001, SKEXP0101, SKEXP0010
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.SemanticKernel.Memory;

namespace SkAgentGroup.AgentFramework;

/// <summary>
/// File-backed <see cref="IMemoryStore"/> implementation that tracks collections of semantic memories
/// and persists them to disk.
/// </summary>
public sealed class FileMemoryStore : IMemoryStore
{
    private readonly string _filePath;
    private readonly object _syncRoot = new();
    private readonly Dictionary<string, Dictionary<string, StoredRecord>> _collections = new(StringComparer.OrdinalIgnoreCase);

    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public FileMemoryStore(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("filePath cannot be null or empty.", nameof(filePath));
        }

        _filePath = filePath;
        LoadFromDisk();
    }

    public Task CreateCollectionAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ValidateCollectionName(collectionName);

        lock (_syncRoot)
        {
            if (_collections.ContainsKey(collectionName))
            {
                return Task.CompletedTask;
            }

            _collections[collectionName] = new Dictionary<string, StoredRecord>(StringComparer.OrdinalIgnoreCase);
            PersistInternal();
        }

        return Task.CompletedTask;
    }

    public Task DeleteCollectionAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ValidateCollectionName(collectionName);

        lock (_syncRoot)
        {
            if (_collections.Remove(collectionName))
            {
                PersistInternal();
            }
        }

        return Task.CompletedTask;
    }

    public Task<bool> DoesCollectionExistAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ValidateCollectionName(collectionName);

        lock (_syncRoot)
        {
            return Task.FromResult(_collections.ContainsKey(collectionName));
        }
    }

    public async IAsyncEnumerable<string> GetCollectionsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        List<string> names;
        lock (_syncRoot)
        {
            names = _collections.Keys.ToList();
        }

        foreach (var name in names)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return name;
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    public Task<MemoryRecord?> GetAsync(string collectionName, string key, bool withEmbedding = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ValidateCollectionName(collectionName);
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }

        MemoryRecord? result = null;
        lock (_syncRoot)
        {
            if (_collections.TryGetValue(collectionName, out var collection) && collection.TryGetValue(key, out var record))
            {
                result = record.ToMemoryRecord(withEmbedding);
            }
        }

        return Task.FromResult(result);
    }

    public async IAsyncEnumerable<MemoryRecord> GetBatchAsync(
        string collectionName,
        IEnumerable<string> keys,
        bool withEmbeddings = false,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (keys == null)
        {
            throw new ArgumentNullException(nameof(keys));
        }

        cancellationToken.ThrowIfCancellationRequested();
        ValidateCollectionName(collectionName);

        var keyList = keys.ToList();
        List<StoredRecord> records;

        lock (_syncRoot)
        {
            records = !_collections.TryGetValue(collectionName, out var collection)
                ? new List<StoredRecord>()
                : keyList
                    .Select(key => collection.TryGetValue(key, out var record) ? record : null)
                    .Where(record => record != null)
                    .Select(record => record!)
                    .ToList();
        }

        foreach (var record in records)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return record.ToMemoryRecord(withEmbeddings);
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    public Task RemoveAsync(string collectionName, string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ValidateCollectionName(collectionName);
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }

        lock (_syncRoot)
        {
            if (_collections.TryGetValue(collectionName, out var collection) && collection.Remove(key))
            {
                PersistInternal();
            }
        }

        return Task.CompletedTask;
    }

    public Task RemoveBatchAsync(string collectionName, IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        if (keys == null)
        {
            throw new ArgumentNullException(nameof(keys));
        }

        cancellationToken.ThrowIfCancellationRequested();
        ValidateCollectionName(collectionName);

        lock (_syncRoot)
        {
            if (!_collections.TryGetValue(collectionName, out var collection))
            {
                return Task.CompletedTask;
            }

            var removed = false;
            foreach (var key in keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                removed |= collection.Remove(key);
            }

            if (removed)
            {
                PersistInternal();
            }
        }

        return Task.CompletedTask;
    }

    public Task<string> UpsertAsync(string collectionName, MemoryRecord record, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ValidateCollectionName(collectionName);
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        var metadata = record.Metadata ?? throw new InvalidOperationException("MemoryRecord metadata is required.");
        var embeddingArray = record.Embedding.ToArray();
        var key = string.IsNullOrWhiteSpace(record.Key) ? metadata.Id : record.Key;

        if (string.IsNullOrWhiteSpace(key))
        {
            key = Guid.NewGuid().ToString("N");
            metadata = new MemoryRecordMetadata(
                metadata.IsReference,
                key,
                metadata.Text ?? string.Empty,
                metadata.Description ?? string.Empty,
                metadata.ExternalSourceName ?? string.Empty,
                metadata.AdditionalMetadata ?? string.Empty);
        }

        var storedRecord = new StoredRecord((MemoryRecordMetadata)metadata.Clone(), embeddingArray, key, record.Timestamp);

        lock (_syncRoot)
        {
            if (!_collections.TryGetValue(collectionName, out var collection))
            {
                collection = new Dictionary<string, StoredRecord>(StringComparer.OrdinalIgnoreCase);
                _collections[collectionName] = collection;
            }

            collection[key] = storedRecord;
            PersistInternal();
        }

        return Task.FromResult(key);
    }

    public async IAsyncEnumerable<string> UpsertBatchAsync(
        string collectionName,
        IEnumerable<MemoryRecord> records,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (records == null)
        {
            throw new ArgumentNullException(nameof(records));
        }

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var record in records)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var key = await UpsertAsync(collectionName, record, cancellationToken).ConfigureAwait(false);
            yield return key;
        }
    }

    public async IAsyncEnumerable<(MemoryRecord, double)> GetNearestMatchesAsync(
        string collectionName,
        ReadOnlyMemory<float> embedding,
        int limit,
        double minRelevanceScore = 0,
        bool withEmbeddings = false,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ValidateCollectionName(collectionName);
        if (limit <= 0)
        {
            yield break;
        }

        List<(StoredRecord Record, double Score)> scored;
        lock (_syncRoot)
        {
            if (!_collections.TryGetValue(collectionName, out var collection) || collection.Count == 0)
            {
                scored = new List<(StoredRecord, double)>();
            }
            else
            {
                scored = collection.Values
                    .Select(record =>
                    {
                        var score = ComputeCosineSimilarity(embedding.Span, record.Embedding);
                        return (Record: record, Score: score);
                    })
                    .Where(tuple => tuple.Score >= minRelevanceScore)
                    .OrderByDescending(tuple => tuple.Score)
                    .Take(limit)
                    .Select(tuple => (tuple.Record, tuple.Score))
                    .ToList();
            }
        }

        foreach (var (record, score) in scored)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return (record.ToMemoryRecord(withEmbeddings), score);
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    public Task<(MemoryRecord, double)?> GetNearestMatchAsync(
        string collectionName,
        ReadOnlyMemory<float> embedding,
        double minRelevanceScore = 0,
        bool withEmbedding = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ValidateCollectionName(collectionName);

        (MemoryRecord, double)? result = null;
        lock (_syncRoot)
        {
            if (_collections.TryGetValue(collectionName, out var collection) && collection.Count > 0)
            {
                var best = collection.Values
                    .Select(record =>
                    {
                        var score = ComputeCosineSimilarity(embedding.Span, record.Embedding);
                        return (Record: record, Score: score);
                    })
                    .OrderByDescending(tuple => tuple.Score)
                    .FirstOrDefault();

                if (best.Record != null && best.Score >= minRelevanceScore)
                {
                    result = (best.Record.ToMemoryRecord(withEmbedding), best.Score);
                }
            }
        }

        return Task.FromResult(result);
    }

    private void LoadFromDisk()
    {
        if (!File.Exists(_filePath))
        {
            return;
        }

        try
        {
            using var stream = File.OpenRead(_filePath);
            var snapshot = JsonSerializer.Deserialize<StoreSnapshot>(stream, SerializerOptions);
            if (snapshot?.Collections == null)
            {
                return;
            }

            _collections.Clear();
            foreach (var collection in snapshot.Collections)
            {
                if (string.IsNullOrWhiteSpace(collection.Name))
                {
                    continue;
                }

                var map = new Dictionary<string, StoredRecord>(StringComparer.OrdinalIgnoreCase);
                foreach (var record in collection.Records ?? Enumerable.Empty<RecordSnapshot>())
                {
                    var metadata = record.Metadata?.ToMetadata();
                    if (metadata == null || record.Embedding == null)
                    {
                        continue;
                    }

                    var key = string.IsNullOrWhiteSpace(record.Key) ? metadata.Id : record.Key;
                    map[key] = new StoredRecord(metadata, record.Embedding, key, record.Timestamp);
                }

                _collections[collection.Name] = map;
            }
        }
        catch
        {
            _collections.Clear();
        }
    }

    private void PersistInternal()
    {
        try
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var snapshot = new StoreSnapshot
            {
                Collections = _collections.Select(collection => new CollectionSnapshot
                {
                    Name = collection.Key,
                    Records = collection.Value.Values.Select(RecordSnapshot.FromStoredRecord).ToList()
                }).ToList()
            };

            using var stream = File.Create(_filePath);
            JsonSerializer.Serialize(stream, snapshot, SerializerOptions);
        }
        catch
        {
            // Persistence failures are ignored intentionally to avoid crashing the host;
            // data will remain available in-memory and the next successful write will repair the file.
        }
    }

    private static void ValidateCollectionName(string collectionName)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
        {
            throw new ArgumentException("Collection name cannot be null or empty.", nameof(collectionName));
        }
    }

    private static double ComputeCosineSimilarity(ReadOnlySpan<float> left, IReadOnlyList<float> right)
    {
        if (left.IsEmpty || right.Count == 0 || left.Length != right.Count)
        {
            return 0d;
        }

        double dot = 0;
        double magnitudeLeft = 0;
        double magnitudeRight = 0;

        for (var i = 0; i < left.Length; i++)
        {
            var l = left[i];
            var r = right[i];

            dot += l * r;
            magnitudeLeft += l * l;
            magnitudeRight += r * r;
        }

        if (magnitudeLeft == 0 || magnitudeRight == 0)
        {
            return 0d;
        }

        return dot / (Math.Sqrt(magnitudeLeft) * Math.Sqrt(magnitudeRight));
    }

    private sealed class StoredRecord
    {
        internal StoredRecord(MemoryRecordMetadata metadata, float[] embedding, string? key, DateTimeOffset? timestamp)
        {
            Metadata = metadata;
            Embedding = embedding;
            Key = key;
            Timestamp = timestamp;
        }

        internal MemoryRecordMetadata Metadata { get; }
        internal float[] Embedding { get; }
        internal string? Key { get; }
        internal DateTimeOffset? Timestamp { get; }

        internal MemoryRecord ToMemoryRecord(bool includeEmbedding)
        {
            var embedding = includeEmbedding ? new ReadOnlyMemory<float>(Embedding) : ReadOnlyMemory<float>.Empty;
            return MemoryRecord.FromMetadata((MemoryRecordMetadata)Metadata.Clone(), embedding, Key, Timestamp);
        }
    }

    private sealed class StoreSnapshot
    {
        public List<CollectionSnapshot> Collections { get; set; } = new();
    }

    private sealed class CollectionSnapshot
    {
        public string? Name { get; set; }
        public List<RecordSnapshot>? Records { get; set; }
    }

    private sealed class RecordSnapshot
    {
        public MemoryRecordMetadataDto? Metadata { get; set; }
        public float[]? Embedding { get; set; }
        public string? Key { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        public static RecordSnapshot FromStoredRecord(StoredRecord stored) => new()
        {
            Metadata = MemoryRecordMetadataDto.FromMetadata(stored.Metadata),
            Embedding = stored.Embedding,
            Key = stored.Key,
            Timestamp = stored.Timestamp
        };
    }

    private sealed class MemoryRecordMetadataDto
    {
        public bool IsReference { get; set; }
        public string? Id { get; set; }
        public string? Text { get; set; }
        public string? Description { get; set; }
        public string? ExternalSourceName { get; set; }
        public string? AdditionalMetadata { get; set; }

        public MemoryRecordMetadata? ToMetadata()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return null;
            }

            return new MemoryRecordMetadata(
                IsReference,
                Id,
                Text ?? string.Empty,
                Description ?? string.Empty,
                ExternalSourceName ?? string.Empty,
                AdditionalMetadata ?? string.Empty);
        }

        public static MemoryRecordMetadataDto FromMetadata(MemoryRecordMetadata metadata) => new()
        {
            IsReference = metadata.IsReference,
            Id = metadata.Id,
            Text = metadata.Text,
            Description = metadata.Description,
            ExternalSourceName = metadata.ExternalSourceName,
            AdditionalMetadata = metadata.AdditionalMetadata
        };
    }
}
