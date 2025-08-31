// Project Name: LightweightAI.Core
// File Name: IFileSystemSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.FileSystem;


public interface IFileSystemSink
{
    Task EmitBatchAsync(IReadOnlyList<FileRecord> batch, CancellationToken ct);
}