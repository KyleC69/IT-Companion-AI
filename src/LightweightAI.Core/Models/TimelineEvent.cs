// Project Name: LightweightAI.Core
// File Name: TimelineEvent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed class TimelineEvent
{
    public string IncidentId { get; init; } = "";
    public string EventId { get; init; } = "";
    public string SourceId { get; init; } = ""; // e.g., "threads", "memregions", "fusion"
    public string LoaderName { get; init; } = ""; // e.g., "ThreadLoader"
    public string SchemaVersion { get; init; } = "";
    public string RecordId { get; init; } = ""; // source-stable identity
    public string ChangeType { get; init; } = ""; // Added/Modified/Removed/AlertOpened/AlertUpdated/AlertClosed
    public DateTime ObservedAtUtc { get; init; } // time the source observed
    public DateTime ReportedAtUtc { get; init; } // time we ingested
    public long MonotonicSeq { get; init; } // append-only sequence
    public long SourceOrder { get; init; } // stable per-source tie-breaker
    public string Summary { get; init; } = ""; // human-friendly lead
    public Dictionary<string, string> Evidence { get; init; } = new(); // selective fields for explainability
    public string ContentHash { get; init; } = ""; // hash of evidence payload for dedupe
    public string ProvenanceJson { get; init; } = ""; // raw minimal JSON snapshot (contract-pure)
    public DateTime TimestampUtc { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public object? Provenance { get; set; }
}