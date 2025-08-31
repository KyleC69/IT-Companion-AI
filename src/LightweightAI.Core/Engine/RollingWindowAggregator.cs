// Project Name: LightweightAI.Core
// File Name: RollingWindowAggregator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections.Concurrent;



namespace LightweightAI.Core.Engine;


/// <summary>
///     Maintains simple rolling (time based) statistics per source key for incoming encoded events.
///     Window length is fixed; older samples are evicted on insert. Computes snapshot statistics
///     required to populate <see cref="Snapshot" /> and <see cref="Core.Models.AggregatedMetric" /> records.
/// </summary>
internal sealed class RollingWindowAggregator(TimeSpan windowLength)
{
    private readonly ConcurrentDictionary<string, DequeEntry> _state = new();





    public (AggregatedMetric metric, Snapshot snapshot) Add(string key, DateTimeOffset ts, double value)
    {
        DequeEntry entry = _state.GetOrAdd(key, _ => new DequeEntry());
        entry.LastTs = ts;
        entry.Samples.AddLast((ts, value));
        entry.Sum += value;
        entry.SumSq += value * value;

        // EMA (alpha derived from window size ~ simple heuristic)
        var alpha = Math.Clamp(2.0 / Math.Max(2.0, windowLength.TotalSeconds / 5.0 + 1.0), 0.01, 0.5);
        entry.Ema = entry.Samples.Count == 1 ? value : (float)(alpha * value + (1 - alpha) * entry.Ema);

        // Evict old
        DateTimeOffset cutoff = ts - windowLength;
        while (entry.Samples.First is not null && entry.Samples.First.Value.ts < cutoff)
        {
            (DateTimeOffset ts, double value) old = entry.Samples.First.Value;
            entry.Samples.RemoveFirst();
            entry.Sum -= old.value;
            entry.SumSq -= old.value * old.value;
        }

        var count = entry.Samples.Count;
        var mean = count > 0 ? entry.Sum / count : 0.0;
        var variance = count > 1 ? Math.Max(0.0, (entry.SumSq - count * mean * mean) / (count - 1)) : 0.0;
        var stdDev = Math.Sqrt(variance);
        DateTimeOffset firstTs = entry.Samples.First?.Value.ts ?? ts;
        var trendDelta = value - mean;

        var metric = new AggregatedMetric(
            key,
            firstTs,
            ts,
            count,
            mean,
            System.Collections.Immutable.ImmutableDictionary<string, double>.Empty.Add("std_dev", stdDev)
        );

        var snap = new Snapshot(
            ts,
            count,
            entry.Sum,
            mean,
            stdDev,
            entry.Ema,
            trendDelta
        );
        return (metric, snap);
    }





    private sealed class DequeEntry
    {
        public readonly LinkedList<(DateTimeOffset ts, double value)> Samples = new();
        public double Ema; // exponential moving average
        public DateTimeOffset LastTs;
        public double Sum; // sum of values
        public double SumSq; // sum of squares
    }
}