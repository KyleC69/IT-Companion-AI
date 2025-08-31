// Project Name: LightweightAI.Core
// File Name: RegistryLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Loaders.Registry;



namespace LightweightAI.Core.Config;


public sealed class RegistryLoaderConfig
{
    public List<RegistryKeySpec> Keys { get; init; } = new();
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromMinutes(5);
    public bool DeltaOnly { get; init; } = true;
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
}