// Project Name: LightweightAI.Core
// File Name: MetricDecision.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


/// <summary>
///     Immutable representation of an aggregated metric scoring outcome over a fixed time window.
///     This precedes enrichment with provenance / model metadata (see <see cref="DecisionOutput" />)
///     and is suitable for lightweight transport inside the fusion pipeline.
/// </summary>
public sealed record MetricDecision(
    string MetricKey,
    DateTimeOffset MetricWindowStart,
    DateTimeOffset MetricWindowEnd,
    double Score,
    bool IsAlert,
    System.Collections.Immutable.ImmutableDictionary<string, double> Payload
);