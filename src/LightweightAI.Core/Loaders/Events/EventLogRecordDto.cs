// Project Name: LightweightAI.Core
// File Name: EventLogRecordDto.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Events;


public sealed class EventLogRecordDto
{
    public string LogName { get; set; } = "";
    public string ProviderName { get; set; } = "";
    public int EventId { get; set; }
    public string Level { get; set; } = "";
    public DateTimeOffset TimeCreated { get; set; }
    public string MachineName { get; set; } = "";
    public string Message { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";
    public string ChangeType { get; set; } = "";
}