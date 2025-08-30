// Project Name: LightweightAI.Core
// File Name: FileSystemLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.FileSystem;


public sealed class FileSystemLoaderConfig
{
    public List<PathSpec> Paths { get; init; } = new();
    public List<string>? IncludePatterns { get; init; }
    public List<string>? ExcludePatterns { get; init; }
    public bool IncludeHash { get; init; } = false;
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromMinutes(5);
    public bool DeltaOnly { get; init; } = true;
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
}