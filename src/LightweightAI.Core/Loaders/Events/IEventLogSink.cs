// Project Name: LightweightAI.Core
// File Name: IEventLogSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Events;


public interface IEventLogSink
{
    Task EmitBatchAsync(IReadOnlyList<EventLogRecordDto> batch, CancellationToken ct);
}