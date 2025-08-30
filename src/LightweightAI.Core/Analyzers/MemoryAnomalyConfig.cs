// Project Name: LightweightAI.Core
// File Name: MemoryAnomalyConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Analyzers;


public sealed class MemoryAnomalyConfig
{
    public int DwellThreshold { get; init; } = 2; // intervals to persist before alert
    public int ClearThreshold { get; init; } = 2; // intervals to clear after no triggers

    public bool FlagRWX { get; init; } = true;
    public bool FlagPrivateExec { get; init; } = true;
    public bool FlagImageProtMismatch { get; init; } = true;
    public bool FlagGuardPages { get; init; } = false;
    public bool FlagLargeRegion { get; init; } = false;
    public long LargeRegionBytes { get; init; } = 50 * 1024 * 1024; // 50 MB
}