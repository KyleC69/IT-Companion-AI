// Project Name: LightweightAI.Core
// File Name: IEventSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Events;


public interface IEventSink
{
    Task EmitBatchAsync(IReadOnlyList<EventEnvelope> batch, CancellationToken ct);
}