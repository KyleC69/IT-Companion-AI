// Project Name: LightweightAI.Core
// File Name: MetricDecision.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public sealed record MetricDecision(
    string MetricKey,
    DateTimeOffset MetricWindowStart,
    DateTimeOffset MetricWindowEnd,
    double Score,
    bool IsAlert,
    System.Collections.Immutable.ImmutableDictionary<string, double> Payload
);