// Project Name: LightweightAI.Core
// File Name: ITaskSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public interface ITaskSink
{
    Task EmitBatchAsync(IReadOnlyList<TaskRecord> batch, CancellationToken ct);
}