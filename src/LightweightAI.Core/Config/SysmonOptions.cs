// Project Name: LightweightAI.Core
// File Name: SysmonOptions.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Config;


/// <summary>
///     Configuration options for SysmonLoader.
/// </summary>
public sealed class SysmonOptions
{
    /// <summary>XPath query to filter Sysmon events.</summary>
    public string Query { get; init; } = "*[System/Provider/@Name='Microsoft-Windows-Sysmon']";

    /// <summary>Event log channel to read from.</summary>
    public string Channel { get; init; } = "Microsoft-Windows-Sysmon/Operational";

    /// <summary>Number of events to process before yielding control.</summary>
    public int BatchSize { get; init; } = 512;

    /// <summary>If true, prefer schema-aware property names when available.</summary>
    public bool UseSchemaNames { get; init; } = true;

    /// <summary>Enable bookmark persistence to resume from last processed event.</summary>
    public bool EnableBookmarks { get; init; } = true;

    /// <summary>Schema version emitted by this loader into each record.</summary>
    public string SchemaVersion { get; init; } = "1.1";
}