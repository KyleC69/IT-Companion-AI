// Project Name: LightweightAI.Core
// File Name: RegistryRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Registry;


public sealed class RegistryRecord
{
    public string KeyPath { get; set; } = "";
    public string ValueName { get; set; } = "";
    public string ValueData { get; set; } = "";
    public string ValueKind { get; set; } = "";
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";
    public string ChangeType { get; set; } = "";
}