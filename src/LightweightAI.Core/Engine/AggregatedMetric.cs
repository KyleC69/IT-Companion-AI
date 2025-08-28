// Project Name: LightweightAI.Core
// File Name: AggregatedMetric.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections.Immutable;


namespace LightweightAI.Core.Engine;


public readonly record struct AggregatedMetric(
    string Key,
    DateTimeOffset WindowStart,
    DateTimeOffset WindowEnd,
    long Count,
    double WeightedScore,
    ImmutableDictionary<string, double> Dimensions
);