// Project Name: LightweightAI.Core
// File Name: FusionConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public sealed record FusionConfig(
    System.Collections.Immutable.ImmutableDictionary<string, double> FieldWeights,
    double AlertThreshold
);