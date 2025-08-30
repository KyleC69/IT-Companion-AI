// Project Name: LightweightAI.Core
// File Name: NetworkConnectionRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.NetworkConnector;


public sealed class NetworkConnectionRecord
{
    public string Protocol { get; set; } = "";
    public string LocalAddress { get; set; } = "";
    public int LocalPort { get; set; }
    public string RemoteAddress { get; set; } = "";
    public int RemotePort { get; set; }
    public string State { get; set; } = "";
    public int OwningPid { get; set; }
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";
    public string ChangeType { get; set; } = "";
}