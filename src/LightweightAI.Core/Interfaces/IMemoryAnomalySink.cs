// Project Name: LightweightAI.Core
// File Name: IMemoryAnomalySink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Models;


namespace LightweightAI.Core.Interfaces;


public interface IMemoryAnomalySink
{
    Task EmitAnomaliesAsync(IReadOnlyList<MemoryAnomaly> anomalies, CancellationToken ct);
}