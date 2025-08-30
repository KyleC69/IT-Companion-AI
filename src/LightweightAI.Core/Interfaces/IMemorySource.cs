// Project Name: LightweightAI.Core
// File Name: IMemorySource.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Builders;


namespace LightweightAI.Core.Interfaces;


/// <summary>
///     Contract for a memory anomaly source.
/// </summary>
public interface IMemorySource
{
    /// <summary>
    ///     Retrieves anomalies detected by this source.
    /// </summary>
    /// <returns>Sequence of memory anomaly events.</returns>
    IEnumerable<MemoryAnomalyEvent> GetAnomalies();
}