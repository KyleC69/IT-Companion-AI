// Project Name: LightweightAI.Core
// File Name: EventEnvelope.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Events;


public sealed class EventEnvelope
{
    // Core identity
    public string ProviderName { get; set; } = "";
    public int EventId { get; set; }
    public int Level { get; set; }
    public string LevelName { get; set; } = "";
    public int Task { get; set; }
    public string TaskName { get; set; } = "";
    public int Opcode { get; set; }
    public string OpcodeName { get; set; } = "";
    public ulong Keywords { get; set; }
    public string[] KeywordsDisplay { get; set; } = Array.Empty<string>();
    public string Channel { get; set; } = "";
    public string Computer { get; set; } = "";
    public long RecordId { get; set; }
    public string? ActivityId { get; set; }
    public string? RelatedActivityId { get; set; }
    public string? UserSid { get; set; }
    public string? UserName { get; set; }

    // Timing
    public DateTime UtcTimestamp { get; set; }

    // Payload
    public string? RenderedMessage { get; set; }
    public string? EventXml { get; set; }
    public string[]? Properties { get; set; }

    // Normalization
    public string NormalizedMessage { get; set; } = "";
    public string EventHash { get; set; } = "";

    // Provenance
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordGuid { get; set; } = "";
}