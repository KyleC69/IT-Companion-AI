// Project Name: LightweightAI.Core
// File Name: FusionConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections.Immutable;


namespace LightweightAI.Core.Engine;


public sealed record FusionConfig(
    ImmutableDictionary<string, double> FieldWeights,
    double AlertThreshold
);