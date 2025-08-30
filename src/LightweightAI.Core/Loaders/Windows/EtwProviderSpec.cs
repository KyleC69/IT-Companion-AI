// Project Name: LightweightAI.Core
// File Name: EtwProviderSpec.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public sealed class EtwProviderSpec
{
    public string? ProviderName { get; init; }
    public Guid ProviderGuid { get; init; } = Guid.Empty;
    public long Keywords { get; init; } = -1; // -1 = all
    public int Level { get; init; } = 5; // Verbose default
}