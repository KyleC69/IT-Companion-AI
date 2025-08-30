// Project Name: LightweightAI.Core
// File Name: IncidentSnapshot.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Models;


namespace LightweightAI.Core.Analyzers;


public sealed class IncidentSnapshot
{
    public string IncidentId { get; init; } = "";
    public DateTime FirstSeenUtc { get; init; }
    public DateTime LastSeenUtc { get; init; }
    public IReadOnlyList<TimelineEvent> OrderedEvents { get; init; } = Array.Empty<TimelineEvent>();
}