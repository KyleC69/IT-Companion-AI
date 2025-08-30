// Project Name: LightweightAI.Core
// File Name: NetworkConnectionLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.NetworkConnector;


public sealed class NetworkConnectionLoaderConfig
{
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromSeconds(30);
    public bool DeltaOnly { get; init; } = true;
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
}