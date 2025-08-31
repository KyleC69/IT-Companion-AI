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



public readonly record struct AnomalySignal(
    DateTimeOffset At,
    double Value,
    double ZScore,
    bool IsAnomaly
);



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
        this._steps = steps.ToImmutableArray();
    }





    public EventRecord Process(in EventRecord e, in RefineryContext ctx)
    {
        var cur = e;
        foreach (var step in this._steps) cur = step.Execute(cur, ctx);
        return cur;
    }
}



public sealed class NormalizeFieldsStep : IDataRefineryStep
{
    public EventRecord Execute(in EventRecord e, in RefineryContext ctx)
    {
        var map = e.Fields;
        if (!map.ContainsKey("actor") && !string.IsNullOrWhiteSpace(e.Actor))
            map = map.SetItem("actor", e.Actor);
        if (!map.ContainsKey("host") && !string.IsNullOrWhiteSpace(e.Host))
            map = map.SetItem("host", e.Host);
        if (!map.ContainsKey("provider") && !string.IsNullOrWhiteSpace(e.Provider))
            map = map.SetItem("provider", e.Provider);
        return e with { Fields = map };
    }
}



public sealed class EnrichProvenanceStep : IDataRefineryStep
{
    public EventRecord Execute(in EventRecord e, in RefineryContext ctx)
    {
        var map = e.Fields
            .SetItem("source_id", e.SourceId)
            .SetItem("provider", e.Provider)
            .SetItem("event_id", e.EventId)
            .SetItem("seq", e.Sequence.ToString());
        return e with { Fields = map };
    }
}



public sealed class DeduplicateStep : IDataRefineryStep
{
    private static string StableHash(string sourceId, string provider, string eventId, long seq, string payloadHash)
    {
        var input = $"{sourceId}|{provider}|{eventId}|{seq}|{payloadHash}";
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        Span<byte> hash = stackalloc byte[32];
        SHA256.HashData(bytes, hash);
        return Convert.ToHexString(hash);
    }





    public EventRecord Execute(in EventRecord e, in RefineryContext ctx)
    {
        var collisionKey = StableHash(e.SourceId, e.Provider, e.EventId, e.Sequence, e.PayloadHash);
        return e with { Fields = e.Fields.SetItem("collision_key", collisionKey) };
    }
}



public sealed class DeterministicAggregator : IAggregator
{
    public IEnumerable<AggregatedMetric> Aggregate(IEnumerable<EventRecord> events, AggregatorConfig cfg)
    {
        var ordered = events
            .OrderBy(e => e.Timestamp)
            .ThenBy(e => e.Sequence)
            .ThenBy(e => e.PayloadHash, StringComparer.Ordinal);

        var byWindow = ordered.GroupBy(e =>
        {
            var startTicks = e.Timestamp.UtcDateTime.Ticks - e.Timestamp.UtcDateTime.Ticks % cfg.Window.Ticks;
            var start = new DateTimeOffset(new DateTime(startTicks, DateTimeKind.Utc));
            DateTimeOffset end = start.Add(cfg.Window);
            return (start, end);
        });

        foreach (var win in byWindow)
        {
            var grouped = win.GroupBy(e => BuildKey(e, cfg.GroupBy));
            foreach (var g in grouped)
            {
                long count = 0;
                var weighted = 0d;
                var dims = ImmutableDictionary<string, double>.Empty.ToBuilder();

                foreach (var e in g)
                {
                    count++;
                    foreach (var kv in cfg.DimensionWeights)
                        if (e.Fields.TryGetValue(kv.Key, out var s) && double.TryParse(s, out var v))
                        {
                            if (dims.ContainsKey(kv.Key)) dims[kv.Key] += v * kv.Value;
                            else dims[kv.Key] = v * kv.Value;
                        }

                    weighted += e.Severity;
                }

                yield return new AggregatedMetric(
                    Key: g.Key,
                    WindowStart: win.Key.start,
                    WindowEnd: win.Key.end,
                    Count: count,
                    WeightedScore: weighted,
                    Dimensions: dims.ToImmutable()
                );
            }
        }
    }





    private static string BuildKey(in EventRecord e, ImmutableArray<string> fields)
    {
        if (fields.Length == 0) return "all";
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







public sealed class SnapshotTrendModel : ISnapshotTrendModel
{
    private readonly TrendConfig _cfg;
    private readonly Queue<TrendPoint> _window;
    private double _ema;
    private bool _hasLast;
    private TrendPoint _last;
    private double _sum;
    private double _sumSquares;








    public Snapshot Current { get; private set; }





    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Snapshot Update(in TrendPoint p)
    {
        if (this._window.Count == 0)
            this._ema = p.Value;
        else
            this._ema = this._cfg.Alpha * p.Value + (1 - this._cfg.Alpha) * this._ema;

        this._window.Enqueue(p);
        this._sum += p.Value;
        this._sumSquares += p.Value * p.Value;

        if (this._window.Count > this._cfg.Window)
        {
            var old = this._window.Dequeue();
            this._sum -= old.Value;
            this._sumSquares -= old.Value * old.Value;
        }

        var n = this._window.Count;
        var mean = this._sum / Math.Max(1, n);
        var variance = Math.Max(0, this._sumSquares / Math.Max(1, n) - mean * mean);
        var std = Math.Sqrt(variance);

        var trendDelta = 0d;
        if (this._hasLast) trendDelta = p.Value - this._last.Value;
        this._last = p;
        this._hasLast = true;

        this.Current = new Snapshot(
            At: p.At,
            Count: n,
            Sum: this._sum,
            Mean: mean,
            StdDev: std,
            Ema: this._ema,
            TrendDelta: trendDelta
        );

        return this.Current;
    }
}



public sealed partial class StreamAnomalyModel : IStreamAnomalyModel
{
    private readonly AnomalyConfig _cfg;
    private double _ema;
    private double _emaSq;
    private bool _init;





    public StreamAnomalyModel(AnomalyConfig cfg)
    {
        this._cfg = cfg;
        this._ema = 0d;
        this._emaSq = 0d;
        this._init = false;
    }





    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AnomalySignal Update(in TrendPoint p)
    {
        if (!this._init)
        {
            this._ema = p.Value;
            this._emaSq = p.Value * p.Value;
            this._init = true;
            return new AnomalySignal(p.At, p.Value, 0, false);
        }

        this._ema = this._cfg.Alpha * p.Value + (1 - this._cfg.Alpha) * this._ema;
        this._emaSq = this._cfg.Alpha * p.Value * p.Value + (1 - this._cfg.Alpha) * this._emaSq;

        var variance = Math.Max(this._cfg.MinVariance, this._emaSq - this._ema * this._ema);
        var std = Math.Sqrt(variance);
        var z = std > 0 ? (p.Value - this._ema) / std : 0d;
        var isAnom = Math.Abs(z) >= this._cfg.ZThreshold;

        return new AnomalySignal(p.At, p.Value, z, isAnom);
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