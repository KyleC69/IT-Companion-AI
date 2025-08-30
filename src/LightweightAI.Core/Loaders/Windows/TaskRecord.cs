// Project Name: LightweightAI.Core
// File Name: TaskRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public sealed class TaskRecord
{
    public string Path { get; set; } = "";
    public string Name { get; set; } = "";
    public string Author { get; set; } = "";
    public string State { get; set; } = "";
    public DateTime LastRunTime { get; set; }
    public DateTime NextRunTime { get; set; }
    public string Triggers { get; set; } = "";
    public string Actions { get; set; } = "";
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";
    public string ChangeType { get; set; } = "";
}