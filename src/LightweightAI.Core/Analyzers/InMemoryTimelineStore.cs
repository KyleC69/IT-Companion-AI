// Project Name: LightweightAI.Core
// File Name: InMemoryTimelineStore.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Interfaces;
using LightweightAI.Core.Models;


namespace LightweightAI.Core.Analyzers;


public sealed class InMemoryTimelineStore : ITimelineStore
{
    private readonly Dictionary<string, List<TimelineEvent>> _events = new(StringComparer.OrdinalIgnoreCase);
    private long _seq;





    public Task<long> NextSequenceAsync(CancellationToken ct)
    {
        return Task.FromResult(Interlocked.Increment(ref this._seq));
    }





    public Task PruneAsync(string incidentId, int keepLast, CancellationToken ct)
    {
        if (this._events.TryGetValue(incidentId, out List<TimelineEvent>? list) && list.Count > keepLast &&
            keepLast > 0)
        {
            List<TimelineEvent> ordered = list.OrderBy(OrderKey).ToList();
            this._events[incidentId] = ordered.Skip(Math.Max(0, ordered.Count - keepLast)).ToList();
        }

        return Task.CompletedTask;
    }





    public Task<bool> AppendAsync(TimelineEvent evt, CancellationToken ct)
    {
        if (!this._events.TryGetValue(evt.IncidentId, out List<TimelineEvent>? list))
        {
            list = new List<TimelineEvent>();
            this._events[evt.IncidentId] = list;
        }

        if (list.Any(e => e.EventId.Equals(evt.EventId, StringComparison.OrdinalIgnoreCase)))
            return Task.FromResult(false);

        list.Add(evt);
        return Task.FromResult(true);
    }





    public Task<IReadOnlyList<TimelineEvent>> GetEventsAsync(string incidentId, CancellationToken ct)
    {
        if (this._events.TryGetValue(incidentId, out List<TimelineEvent>? list))
            return Task.FromResult<IReadOnlyList<TimelineEvent>>(list.OrderBy(OrderKey).ToList());
        return Task.FromResult<IReadOnlyList<TimelineEvent>>(Array.Empty<TimelineEvent>());
    }





    private static object OrderKey(TimelineEvent e)
    {
        return (e.ObservedAtUtc, e.SourceOrder, e.EventId);
    }
}