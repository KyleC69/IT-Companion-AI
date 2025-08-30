// Project Name: LightweightAI.Core
// File Name: IMetricSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public interface IMetricSink
{
    Task EmitBatchAsync(IReadOnlyList<PerfCounterSample> batch, CancellationToken ct);
}