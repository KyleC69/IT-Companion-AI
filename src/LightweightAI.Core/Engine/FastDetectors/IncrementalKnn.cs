// Project Name: LightweightAI.Core
// File Name: IncrementalKnn.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Analyzers;
using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Engine.FastDetectors;


/// <summary>
///     Adapter exposing the existing <see cref="WindowedKnnDensity" /> under expected path.
/// </summary>
public sealed class IncrementalKnn(WindowedKnnDensity impl)
{
    public float UpdateAndScore(EncodedEvent e, DateTimeOffset nowUtc)
    {
        return impl.UpdateAndScore(e, nowUtc);
    }
}