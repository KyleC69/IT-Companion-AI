// Project Name: LightweightAI.Core
// File Name: PathSpec.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.FileSystem;


public sealed class PathSpec
{
    public string Path { get; init; } = "";
    public bool Recursive { get; init; } = true;
}