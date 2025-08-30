// Project Name: LightweightAI.Core
// File Name: MemoryRegionLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public sealed class MemoryRegionLoaderConfig
{
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromMinutes(5);
    public bool DeltaOnly { get; init; } = true;
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;

    public bool IncludeReservedRegions { get; init; } = false;
    public int MaxRegionsPerProcess { get; init; } = 0; // 0 = unlimited
    public HashSet<string>? IncludeOnlyProcesses { get; init; }
    public HashSet<string>? ExcludeProcesses { get; init; }
}