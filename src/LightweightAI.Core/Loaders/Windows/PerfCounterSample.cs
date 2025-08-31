// Project Name: LightweightAI.Core
// File Name: PerfCounterSample.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public sealed class PerfCounterSample
{
    public string Category { get; set; } = "";
    public string Instance { get; set; } = "";
    public string CounterName { get; set; } = "";
    public double? Value { get; set; }
    public string? Unit { get; set; }
    public DateTime UtcTimestamp { get; set; }
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";

    // Provenance flags
    public string ValueStatus { get; set; } = "Good"; // Good|NaN|Infinity|Error
    public string? Error { get; set; } // Optional error classification
}