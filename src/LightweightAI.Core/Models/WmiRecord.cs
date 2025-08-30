// Project Name: LightweightAI.Core
// File Name: WmiRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed class WmiRecord
{
    public string Namespace { get; init; } = string.Empty;
    public string QueryName { get; init; } = string.Empty; // WQL
    public string ClassName { get; init; } = string.Empty;
    public Dictionary<string, string?> Properties { get; init; } = new(StringComparer.OrdinalIgnoreCase);

    // Provenance
    public string Host { get; init; } = string.Empty;
    public string SourceId { get; init; } = string.Empty;
    public string LoaderName { get; init; } = string.Empty;
    public string SchemaVersion { get; init; } = string.Empty;
    public string CollectionMethod { get; init; } = string.Empty;
    public string RecordId { get; init; } = string.Empty;

    // Change tracking
    public string ChangeType { get; set; } = "Unchanged"; // Added|Removed|Modified|Unchanged
}