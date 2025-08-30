// Project Name: LightweightAI.Core
// File Name: RegistryKeySpec.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Registry;


public sealed class RegistryKeySpec
{
    public string Hive { get; init; } = "";
    public string Path { get; init; } = "";
    public int Depth { get; init; } = 0;
}