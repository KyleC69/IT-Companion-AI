// Project Name: LightweightAI.Core
// File Name: EventContext.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;

/// <summary>
/// Canonical event envelope passed into rule evaluation and early enrichment stages
/// containing host, source, event id, timestamp and a flexible payload map for
/// additional extracted attributes.
/// </summary>
public record EventContext(
    string HostId,
    string SourceId,
    int EventId,
    DateTime Timestamp,
    Dictionary<string, object> Payload
);