// Project Name: LightweightAI.Core
// File Name: EventContext.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public record EventContext(
    string HostId,
    string SourceId,
    int EventId,
    DateTime Timestamp,
    Dictionary<string, object> Payload
);