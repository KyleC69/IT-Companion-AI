// Project Name: LightweightAI.Core
// File Name: DecisionOutput.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


// === DecisionOutput.cs ===


using System.Collections.Immutable;


namespace LightweightAI.Core.Engine;


public sealed record DecisionOutput(
    string MetricKey,
    DateTimeOffset MetricWindowStart,
    DateTimeOffset MetricWindowEnd,
    double Score,
    bool IsAlert,
    ImmutableDictionary<string, double> Payload
)
{
    // Provenance / envelope metadata
    public Guid EventId { get; init; }
    public string FusionSignature { get; init; } = string.Empty;
    public string ModelId { get; init; } = string.Empty;
    public string ModelVersion { get; init; } = string.Empty;
    public double Severity { get; init; }
    public string SeverityScaleRef { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}



public sealed record MetricDecision(
    string MetricKey,
    DateTimeOffset MetricWindowStart,
    DateTimeOffset MetricWindowEnd,
    double Score,
    bool IsAlert,
    ImmutableDictionary<string, double> Payload
);