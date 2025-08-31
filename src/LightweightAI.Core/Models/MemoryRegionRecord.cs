// Project Name: LightweightAI.Core
// File Name: MemoryRegionRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed class MemoryRegionRecord
{
    public int Pid { get; set; }
    public string ProcessName { get; set; } = "";
    public string BaseAddress { get; set; } = "";
    public string AllocationBase { get; set; } = "";
    public long RegionSize { get; set; }
    public string State { get; set; } = "";
    public string Protect { get; set; } = "";
    public string AllocationProtect { get; set; } = "";
    public string Type { get; set; } = "";
    public string MappedPath { get; set; } = "";
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";
    public string ChangeType { get; set; } = "";
}