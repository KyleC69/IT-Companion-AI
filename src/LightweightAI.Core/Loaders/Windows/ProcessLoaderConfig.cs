// Project Name: LightweightAI.Core
// File Name: ProcessLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public sealed class ProcessLoaderConfig
{
    // Cadence
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromSeconds(10);

    // Emission mode
    public bool ChangeOnly { get; init; } = true;

    // Optional enrichments
    public bool IncludeCommandLine { get; init; } = true;
    public bool IncludeUser { get; init; } = true;
    public bool IncludeSessionId { get; init; } = true;

    // Signature verification options
    public bool VerifyOnline { get; init; } = false; // Online revocation can be slow; default to offline

    // Behavior
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
}