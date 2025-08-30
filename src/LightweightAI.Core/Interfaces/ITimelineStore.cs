// Project Name: LightweightAI.Core
// File Name: ITimelineStore.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Models;


namespace LightweightAI.Core.Interfaces;


public interface ITimelineStore
{
    Task<long> NextSequenceAsync(CancellationToken ct);
    Task<bool> AppendAsync(TimelineEvent evt, CancellationToken ct); // returns false if duplicate by EventId
    Task<IReadOnlyList<TimelineEvent>> GetEventsAsync(string incidentId, CancellationToken ct);
    Task PruneAsync(string incidentId, int keepLast, CancellationToken ct); // optional compaction
}