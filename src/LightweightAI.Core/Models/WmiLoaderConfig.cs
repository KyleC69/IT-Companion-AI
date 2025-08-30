// Project Name: LightweightAI.Core
// File Name: WmiLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed class WmiLoaderConfig
{
    public List<WmiQuerySpec> Queries { get; init; } = new();
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromMinutes(5);
    public bool DeltaOnly { get; init; } = true;
    public TimeSpan QueryTimeout { get; init; } = TimeSpan.FromSeconds(5);
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
}