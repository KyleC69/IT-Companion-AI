// Project Name: LightweightAI.Core
// File Name: AggregatedMetric.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public readonly record struct AggregatedMetric(
    string Key,
    DateTimeOffset WindowStart,
    DateTimeOffset WindowEnd,
    long Count,
    double WeightedScore,
    System.Collections.Immutable.ImmutableDictionary<string, double> Dimensions
);