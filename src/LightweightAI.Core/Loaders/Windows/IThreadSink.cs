// Project Name: LightweightAI.Core
// File Name: IThreadSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public interface IThreadSink
{
    Task EmitBatchAsync(IReadOnlyList<ThreadRecord> batch, CancellationToken ct);
}