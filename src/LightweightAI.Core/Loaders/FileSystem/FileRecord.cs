// Project Name: LightweightAI.Core
// File Name: FileRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.FileSystem;


public sealed class FileRecord
{
    public string FullPath { get; set; } = "";
    public string Name { get; set; } = "";
    public string DirectoryPath { get; set; } = "";
    public long Size { get; set; }
    public DateTime CreationTimeUtc { get; set; }
    public DateTime LastWriteTimeUtc { get; set; }
    public string Attributes { get; set; } = "";
    public string Hash { get; set; } = "";
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";
    public string ChangeType { get; set; } = "";
}