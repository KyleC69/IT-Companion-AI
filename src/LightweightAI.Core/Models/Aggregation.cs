// Project Name: LightweightAI.Core
// File Name: Aggregation.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

using LightweightAI.Core.Engine;



namespace LightweightAI.Core.Models;


public readonly record struct EventRecord(
    string SourceId,
    string Provider,
    string EventId,
    string Actor,
    string Subject,
    string Host,
    DateTimeOffset Timestamp,
    long Sequence,
    int Severity,
    string PayloadHash,
    ImmutableDictionary<string, string> Fields
);



public readonly record struct TrendPoint(DateTimeOffset At, double Value);





public sealed record AggregatorConfig(
    TimeSpan Window,
    ImmutableArray<string> GroupBy,
    ImmutableDictionary<string, double> DimensionWeights
);



public sealed record TrendConfig(
    int Window,
    double Alpha
);



public sealed record AnomalyConfig(
    double Alpha,
    double ZThreshold,
    double MinVariance
);



public sealed class RefineryContext
{
    public string SourceId { get; init; } = "";
    public string Provider { get; init; } = "";
    public DateTimeOffset Now { get; init; } = DateTimeOffset.UtcNow;
    public ImmutableDictionary<string, string> Hints { get; init; } = ImmutableDictionary<string, string>.Empty;
}



public interface IDataRefineryStep
{
    EventRecord Execute(in EventRecord e, in RefineryContext ctx);
}



public interface IDataRefineryPipeline
{
    EventRecord Process(in EventRecord e, in RefineryContext ctx);
}



public interface IAggregator
{
    IEnumerable<AggregatedMetric> Aggregate(IEnumerable<EventRecord> events, AggregatorConfig cfg);
}



public interface IUnifiedAggregator
{
    IEnumerable<AggregatedMetric> AggregateUnified(IEnumerable<EventRecord> events, AggregatorConfig cfg);
}



public interface ISnapshotTrendModel
{
    Snapshot Current { get; }


    Snapshot Update(in TrendPoint p);
}



public interface IStreamAnomalyModel
{
    AnomalySignal Update(in TrendPoint p);
}



public sealed class DataRefineryPipeline : IDataRefineryPipeline
{
    private readonly ImmutableArray<IDataRefineryStep> _steps;






    public DataRefineryPipeline(IEnumerable<IDataRefineryStep> steps)
    {
        _steps = steps.ToImmutableArray();
    }






    public EventRecord Process(in EventRecord e, in RefineryContext ctx)
    {
        EventRecord cur = e;
        foreach (IDataRefineryStep step in _steps)
        {
            cur = step.Execute(cur, ctx);
        }

        return cur;
    }
}



public sealed class NormalizeFieldsStep : IDataRefineryStep
{
    public EventRecord Execute(in EventRecord e, in RefineryContext ctx)
    {
        ImmutableDictionary<string, string> map = e.Fields;
        if (!map.ContainsKey("actor") && !string.IsNullOrWhiteSpace(e.Actor))
        {
            map = map.SetItem("actor", e.Actor);
        }

        if (!map.ContainsKey("host") && !string.IsNullOrWhiteSpace(e.Host))
        {
            map = map.SetItem("host", e.Host);
        }

        if (!map.ContainsKey("provider") && !string.IsNullOrWhiteSpace(e.Provider))
        {
            map = map.SetItem("provider", e.Provider);
        }

        return e with { Fields = map };
    }
}



public sealed class EnrichProvenanceStep : IDataRefineryStep
{
    public EventRecord Execute(in EventRecord e, in RefineryContext ctx)
    {
        ImmutableDictionary<string, string> map = e.Fields
            .SetItem("source_id", e.SourceId)
            .SetItem("provider", e.Provider)
            .SetItem("event_id", e.EventId)
            .SetItem("seq", e.Sequence.ToString());
        return e with { Fields = map };
    }
}



public sealed class DeduplicateStep : IDataRefineryStep
{
    public EventRecord Execute(in EventRecord e, in RefineryContext ctx)
    {
        var collisionKey = StableHash(e.SourceId, e.Provider, e.EventId, e.Sequence, e.PayloadHash);
        return e with { Fields = e.Fields.SetItem("collision_key", collisionKey) };
    }






    private static string StableHash(string sourceId, string provider, string eventId, long seq, string payloadHash)
    {
        var input = $"{sourceId}|{provider}|{eventId}|{seq}|{payloadHash}";
        var bytes = Encoding.UTF8.GetBytes(input);
        Span<byte> hash = stackalloc byte[32];
        SHA256.HashData(bytes, hash);
        return Convert.ToHexString(hash);
    }
}



public sealed class DeterministicAggregator : IAggregator
{
    public IEnumerable<AggregatedMetric> Aggregate(IEnumerable<EventRecord> events, AggregatorConfig cfg)
    {
        IOrderedEnumerable<EventRecord> ordered = events
            .OrderBy(e => e.Timestamp)
            .ThenBy(e => e.Sequence)
            .ThenBy(e => e.PayloadHash, StringComparer.Ordinal);

        IEnumerable<IGrouping<(DateTimeOffset start, DateTimeOffset end), EventRecord>> byWindow = ordered.GroupBy(e =>
        {
            var startTicks = e.Timestamp.UtcDateTime.Ticks - e.Timestamp.UtcDateTime.Ticks % cfg.Window.Ticks;
            var start = new DateTimeOffset(new DateTime(startTicks, DateTimeKind.Utc));
            DateTimeOffset end = start.Add(cfg.Window);
            return (start, end);
        });

        foreach (IGrouping<(DateTimeOffset start, DateTimeOffset end), EventRecord> win in byWindow)
        {
            IEnumerable<IGrouping<string, EventRecord>> grouped = win.GroupBy(e => BuildKey(e, cfg.GroupBy));
            foreach (IGrouping<string, EventRecord> g in grouped)
            {
                long count = 0;
                var weighted = 0d;
                var dims = ImmutableDictionary<string, double>.Empty.ToBuilder();

                foreach (EventRecord e in g)
                {
                    count++;
                    foreach (KeyValuePair<string, double> kv in cfg.DimensionWeights)
                    {
                        if (e.Fields.TryGetValue(kv.Key, out var s) && double.TryParse(s, out var v))
                        {
                            if (dims.ContainsKey(kv.Key))
                            {
                                dims[kv.Key] += v * kv.Value;
                            }
                            else
                            {
                                dims[kv.Key] = v * kv.Value;
                            }
                        }
                    }

                    weighted += e.Severity;
                }

                yield return new AggregatedMetric(
                    g.Key,
                    win.Key.start,
                    win.Key.end,
                    count,
                    weighted,
                    dims.ToImmutable()
                );
            }
        }
    }






    private static string BuildKey(in EventRecord e, ImmutableArray<string> fields)
    {
        if (fields.Length == 0)
        {
            return "all";
        }

        var parts = ArrayPool<string>.Shared.Rent(fields.Length);
        try
        {
            for (var i = 0; i < fields.Length; i++)
            {
                var f = fields[i];
                e.Fields.TryGetValue(f, out var v);
                parts[i] = $"{f}={v ?? ""}";
            }

            return string.Join("|", parts.AsSpan(0, fields.Length).ToArray());
        }
        finally
        {
            ArrayPool<string>.Shared.Return(parts, true);
        }
    }
}




public static class UnifiedAggregatorFactory
{
    public static IUnifiedAggregator CreateDefault()
    {
        return new UnifiedAggregator(new IAggregator[]
        {
            new DeterministicAggregator()
        });
    }
}



public sealed class DataRefineries
{
    public static IDataRefineryPipeline CreateDefaultPipeline()
    {
        return new DataRefineryPipeline(new IDataRefineryStep[]
        {
            new NormalizeFieldsStep(),
            new EnrichProvenanceStep(),
            new DeduplicateStep()
        });
    }
}



public sealed class SnapShotModels
{
    public static ISnapshotTrendModel CreateSnapshotTrend(TrendConfig cfg)
    {
        return new SnapshotTrendModel(cfg);
    }






    public static IStreamAnomalyModel CreateStreamAnomaly(AnomalyConfig cfg)
    {
        return new StreamAnomalyModel(cfg);
    }
}