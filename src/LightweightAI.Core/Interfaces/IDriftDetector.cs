// Project Name: LightweightAI.Core
// File Name: IDriftDetector.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Interfaces;


/// <summary>
///     Contract for slow/long-horizon drift detectors operating on a univariate stream of composite scores
///     (or other aggregated metric). Implementations should return a score in [0,1] where higher indicates
///     stronger evidence of distributional shift and set <paramref name="isDrift" /> when a detection threshold
///     is crossed.
/// </summary>
public interface IDriftDetector
{
    string Name { get; }
    double Update(double value, DateTimeOffset timestampUtc, out bool isDrift);
}