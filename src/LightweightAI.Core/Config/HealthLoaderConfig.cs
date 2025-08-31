// Project Name: LightweightAI.Core
// File Name: HealthLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Config;


public sealed class HealthLoaderConfig
{
    public List<string> Metrics { get; init; } = new() { "cpu", "memory", "disk", "net" };
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromSeconds(30);
    public bool DeltaOnly { get; init; } = true;
    public double ChangeThreshold { get; init; } = 1.0; // %
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
}