// Project Name: LightweightAI.Core
// File Name: IEtwSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public interface IEtwSink
{
    Task EmitBatchAsync(IReadOnlyList<EtwEventEnvelope> batch, CancellationToken ct);
}