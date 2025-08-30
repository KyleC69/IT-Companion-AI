// Project Name: LightweightAI.Core
// File Name: Ewma.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Engine.FastDetectors;


/// <summary>
///     Adapter exposing the existing <see cref="EwmaDetector" /> under the expected path for checklist alignment.
/// </summary>
public sealed class Ewma(EwmaDetector impl)
{
    public float UpdateAndScore(EncodedEvent e, DateTimeOffset nowUtc)
    {
        return impl.UpdateAndScore(e, nowUtc);
    }
}