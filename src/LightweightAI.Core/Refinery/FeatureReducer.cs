// Project Name: LightweightAI.Core
// File Name: FeatureReducer.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Refinery;


/// <summary>
///     Placeholder dimensionality reduction component. Will apply PCA / feature hashing in future.
///     Currently passes through input encoded event unchanged for pipeline compatibility.
/// </summary>
public sealed class FeatureReducer
{
    public EncodedEvent Reduce(EncodedEvent encoded)
    {
        return encoded;
    }
}