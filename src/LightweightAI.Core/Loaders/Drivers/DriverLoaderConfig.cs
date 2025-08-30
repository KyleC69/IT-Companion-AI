// Project Name: LightweightAI.Core
// File Name: DriverLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Drivers;


public sealed class DriverLoaderConfig
{
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromMinutes(5);
    public bool DeltaOnly { get; init; } = true;
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
    public List<string>? IncludeNames { get; init; }
    public List<string>? ExcludeNames { get; init; }
}