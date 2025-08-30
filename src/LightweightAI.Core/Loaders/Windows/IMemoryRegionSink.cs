// Project Name: LightweightAI.Core
// File Name: IMemoryRegionSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public interface IMemoryRegionSink
{
    Task EmitBatchAsync(IReadOnlyList<MemoryRegionRecord> batch, CancellationToken ct);
}