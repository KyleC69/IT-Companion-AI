// Project Name: LightweightAI.Core
// File Name: EventLogLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Events;


public sealed class EventLogLoaderConfig
{
    // Channels to read (e.g., "System", "Application", "Security")
    public List<string> Channels { get; init; } = new() { "System", "Application" };

    // Optional XPath filter applied per channel (e.g., "*[System/Level=2]")
    public string? XPathFilter { get; init; } = null;

    // Where to start on first run
    public EventStartPosition StartAt { get; init; } = EventStartPosition.End;

    // Polling cadence and batching
    public TimeSpan PollInterval { get; init; } = TimeSpan.FromSeconds(2);
    public int BatchSize { get; init; } = 512;
    public int MaxPerChannelPerPoll { get; init; } = 2048;

    // Payload options
    public bool IncludeXml { get; init; } = false;
    public bool IncludeRenderedMessage { get; init; } = true;
    public bool IncludeProperties { get; init; } = false;

    // Deduplication
    public TimeSpan DedupWindow { get; init; } = TimeSpan.FromSeconds(10);
    public int DedupMaxPerWindow { get; init; } = 5; // 0 or negative disables dedup limiting
    public int DedupCapacity { get; init; } = 100_000; // safety bound for dedup map

    // Behavior
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
}