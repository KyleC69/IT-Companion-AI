// Project Name: LightweightAI.Core
// File Name: FusionConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;

/// <summary>
/// Immutable configuration snapshot for the fusion engine containing a map of
/// field names to weights and the alert threshold used to classify an aggregated
/// score. Treat as versionable / cacheable config enabling deterministic replay.
/// </summary>
public sealed record FusionConfig(
    System.Collections.Immutable.ImmutableDictionary<string, double> FieldWeights,
    double AlertThreshold
);