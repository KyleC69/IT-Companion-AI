// Project Name: LightweightAI.Core
// File Name: ThreadRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed class ThreadRecord
{
    public int Pid { get; set; }
    public int Tid { get; set; }
    public string ProcessName { get; set; } = "";
    public DateTime? StartTime { get; set; }
    public string PriorityLevel { get; set; } = "";
    public string ThreadState { get; set; } = "";
    public string? WaitReason { get; set; }
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";
    public string ChangeType { get; set; } = "";
}