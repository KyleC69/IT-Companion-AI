// Project Name: LightweightAI.Core
// File Name: ModuleRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Modules;


public sealed class ModuleRecord
{
    public int Pid { get; set; }
    public string ProcessName { get; set; } = "";
    public string ModuleName { get; set; } = "";
    public string FilePath { get; set; } = "";
    public long BaseAddress { get; set; }
    public int ModuleSize { get; set; }
    public bool Signed { get; set; }
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";
    public string ChangeType { get; set; } = "";
}