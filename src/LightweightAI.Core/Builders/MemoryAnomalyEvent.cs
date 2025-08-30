// Project Name: LightweightAI.Core
// File Name: MemoryAnomalyEvent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Builders;


/// <summary>
///     Represents a detected memory anomaly, with provenance metadata.
/// </summary>
public class MemoryAnomalyEvent
{
    /// <summary>
    ///     Schema version for this event's data contract.
    /// </summary>
    public string SchemaVersion { get; set; }

    /// <summary>
    ///     UTC timestamp when the anomaly was detected.
    /// </summary>
    public DateTime TimestampUtc { get; set; }

    /// <summary>
    ///     Process identifier associated with the anomaly.
    /// </summary>
    public int ProcessId { get; set; }

    /// <summary>
    ///     Type of anomaly detected.
    /// </summary>
    public AnomalyType AnomalyType { get; set; }

    /// <summary>
    ///     Severity of the anomaly.
    /// </summary>
    public SeverityLevel Severity { get; set; }

    /// <summary>
    ///     Optional: Source or subsystem that reported the anomaly.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    ///     Optional: Additional context or diagnostic details.
    /// </summary>
    public string Details { get; set; }

    public ProvenanceInfo Provenance { get; set; }
}