using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightAI.Core.Abstractions;

public interface ISourceLoader
{
    /// <summary>Stream normalized raw events for a given source.</summary>
    IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request, CancellationToken ct = default);
}

public sealed record SourceRequest(string SourceKey, DateTimeOffset? SinceUtc = null, DateTimeOffset? UntilUtc = null, IReadOnlyDictionary<string, string>? Parameters = null);

public sealed record RawEvent(
    string SourceKey,
    int EventId,
    DateTimeOffset TimestampUtc,
    string Host,
    string? User,
    string Severity, // normalized: Verbose, Info, Warn, Error, Critical
    IReadOnlyDictionary<string, object?> Fields,
    string ProvenanceTag // e.g., "sysmon/evtx:SHA256=…"
);

public interface INormalizer
{
    /// <summary>Coerce types, fill nulls, align timestamps, normalize severity.</summary>
    RawEvent Normalize(RawEvent input);
}

public interface IFeatureEncoder
{
    /// <summary>Transform normalized event into feature vector.</summary>
    EncodedEvent Encode(RawEvent normalized);
}

public sealed record EncodedEvent(
    string SourceKey,
    DateTimeOffset TimestampUtc,
    string Host,
    ReadOnlyMemory<float> Dense,
    IReadOnlyDictionary<string, int> SparseIndex // one-hot: key -> 1 index
);

public interface IDetector
{
    /// <summary>Update internal state and return a score in [0,1]. Higher = more anomalous.</summary>
    float UpdateAndScore(in EncodedEvent example, DateTimeOffset nowUtc);
    string Name { get; }
}

public interface IAlertDispatcher
{
    Task DispatchAsync(ProvenancedDecision decision, CancellationToken ct = default);
}

public interface IAppendOnlyAuditLog
{
    Task AppendAsync(AuditRecord record, CancellationToken ct = default);
    Task<IReadOnlyList<AuditRecord>> ReadRangeAsync(long fromSequence, int maxCount, CancellationToken ct = default);
}

public sealed record AuditRecord(long Sequence, DateTimeOffset TimestampUtc, string Actor, string Action, string PayloadJson, string HashHex, string? PreviousHashHex);
public sealed record ProvenancedDecision(DateTimeOffset TimestampUtc, string CorrelationId, float Risk, string Severity, string Summary, string DetailJson, string ProvenanceChainHash);
