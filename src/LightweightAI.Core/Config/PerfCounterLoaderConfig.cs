// Project Name: LightweightAI.Core
// File Name: PerfCounterLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Config;


public sealed class PerfCounterLoaderConfig
{
    // Example: @"\Processor(_Total)\% Processor Time"
    //          @"\LogicalDisk(*)\Disk Transfers/sec"
    public List<string> Paths { get; init; } = new();

    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromSeconds(5);

    // Emission
    public int BatchSize { get; init; } = 512;

    // Limit counter explosion on wildcards
    public int MaxCounters { get; init; } = 5000;

    // Refresh cadence for wildcard instance enumeration
    public TimeSpan? InstanceRefreshInterval { get; init; } = TimeSpan.FromMinutes(5);

    // Try to suppress small fluctuations
    public bool DeltaOnly { get; init; } = false;
    public double DeltaEpsilon { get; init; } = 0.001;

    // Warm-up reads help counters that need two samples
    public int WarmupReads { get; init; } = 1;

    // Safety
    public int MaxErrorsPerCycle { get; init; } = 50;
    public bool FailFast { get; init; } = false;

    // Audit logging to standard logger (records themselves carry provenance)
    public bool AuditLog { get; init; } = true;
}