// Project Name: LightweightAI.Core
// File Name: HandleRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed class HandleRecord
{
    public int Pid { get; set; }
    public string ProcessName { get; set; } = "";
    public string HandleType { get; set; } = "";
    public string ObjectName { get; set; } = "";
    public uint AccessMask { get; set; }
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";
    public string ChangeType { get; set; } = "";
}