// Project Name: LightweightAI.Core
// File Name: FusionEngine.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections.Immutable;
using System.Runtime.CompilerServices;

using AICompanion.Tests;


namespace LightweightAI.Core.Engine;


public sealed class FusionEngine : IFusionEngine
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DecisionOutput Fuse(DecisionInput input, FusionConfig cfg)
    {
        ImmutableDictionary<string, double>.Builder contrib =
            ImmutableDictionary.CreateBuilder<string, double>(StringComparer.OrdinalIgnoreCase);
        var score = 0d;

        foreach (KeyValuePair<string, double> kv in cfg.FieldWeights)
        {
            var val = 0d;
            if (kv.Key.Equals("count", StringComparison.OrdinalIgnoreCase))
                val = input.Metric.Count;
            else if (kv.Key.Equals("weighted_score", StringComparison.OrdinalIgnoreCase))
                val = input.Metric.WeightedScore;
            else if (kv.Key.Equals("trend_delta", StringComparison.OrdinalIgnoreCase))
                val = input.Trend.TrendDelta;
            else if (kv.Key.Equals("std_dev", StringComparison.OrdinalIgnoreCase))
                val = input.Trend.StdDev;
            else if (kv.Key.Equals("ema", StringComparison.OrdinalIgnoreCase))
                val = input.Trend.Ema;
            else if (kv.Key.Equals("z_score", StringComparison.OrdinalIgnoreCase))
                val = input.Anomaly.ZScore;
            else if (input.Metric.Dimensions.TryGetValue(kv.Key, out var dimVal))
                val = dimVal;

            var weighted = val * kv.Value;
            contrib[kv.Key] = weighted;
            score += weighted;
        }

        var isAlert = score >= cfg.AlertThreshold;

        return new DecisionOutput(
            input.Metric.Key,
            input.Metric.WindowStart,
            input.Metric.WindowEnd,
            score,
            isAlert,
            contrib.ToImmutable()
        );
    }
}