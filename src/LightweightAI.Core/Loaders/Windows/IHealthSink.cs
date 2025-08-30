// Project Name: LightweightAI.Core
// File Name: IHealthSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public interface IHealthSink
{
    Task EmitBatchAsync(IReadOnlyList<HealthRecord> batch, CancellationToken ct);
}