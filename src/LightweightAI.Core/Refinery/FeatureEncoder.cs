// Project Name: LightweightAI.Core
// File Name: FeatureEncoder.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Abstractions;
using LightweightAI.Core.Analyzers;
using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Refinery;


/// <summary>
///     High level feature encoder that delegates to a concrete <see cref="IFeatureEncoder" /> (currently
///     <see cref="OneHotEncoder" />)
///     providing a stable location that matches the implementation matrix expectations.
/// </summary>
public sealed class FeatureEncoder(OneHotEncoder oneHot)
{
    public EncodedEvent Encode(RawEvent normalized)
    {
        return oneHot.Encode(normalized);
    }
}