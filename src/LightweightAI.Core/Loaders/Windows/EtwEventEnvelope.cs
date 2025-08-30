// Project Name: LightweightAI.Core
// File Name: EtwEventEnvelope.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public sealed class EtwEventEnvelope
{
    // Identity
    public string ProviderName { get; set; } = "";
    public Guid ProviderGuid { get; set; }
    public string EventName { get; set; } = "";
    public int Task { get; set; }
    public string TaskName { get; set; } = "";
    public int Opcode { get; set; }
    public string OpcodeName { get; set; } = "";
    public int Level { get; set; }
    public ulong Keywords { get; set; }

    // Correlation and process
    public int ProcessId { get; set; }
    public int ThreadId { get; set; }
    public string? ProcessName { get; set; }
    public string? ActivityId { get; set; }
    public string? RelatedActivityId { get; set; }

    // Timing
    public DateTime UtcTimestamp { get; set; }

    // Payload
    public string? RenderedMessage { get; set; }
    public Dictionary<string, string>? Payload { get; set; }

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