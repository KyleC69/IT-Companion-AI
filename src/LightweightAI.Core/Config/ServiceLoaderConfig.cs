// Project Name: LightweightAI.Core
// File Name: ServiceLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Config;


public sealed class ServiceLoaderConfig
{
    // Cadence
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromMinutes(5);

    // Emission mode
    public bool ChangeOnly { get; init; } = true;

    // Include driver services (kernel/file system drivers)
    public bool IncludeDrivers { get; init; } = true;

    // Signature verification options
    public bool VerifyOnline { get; init; } = false; // Online revocation can be slow; default to offline

    // Behavior
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
}