// Project Name: LightweightAI.Core
// File Name: HandleLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public sealed class HandleLoaderConfig
{
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromMinutes(5);
    public bool DeltaOnly { get; init; } = true;
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
}