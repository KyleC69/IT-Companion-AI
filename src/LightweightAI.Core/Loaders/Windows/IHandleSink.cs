// Project Name: LightweightAI.Core
// File Name: IHandleSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public interface IHandleSink
{
    Task EmitBatchAsync(IReadOnlyList<HandleRecord> batch, CancellationToken ct);
}