// Project Name: LightweightAI.Core
// File Name: RawEvent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine.Models;


public sealed record RawEvent(
    string SourceKey,
    int EventId,
    DateTimeOffset TimestampUtc,
    string Host,
    string? User,
    string Severity, // normalized: Verbose, Info, Warn, Error, Critical
    IReadOnlyDictionary<string, object?> Fields,
    string ProvenanceTag // e.g., "sysmon/evtx:SHA256=…"
);