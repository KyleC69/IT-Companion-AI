// Project Name: LightweightAI.Core
// File Name: ProvenanceInfo.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


/// <summary>
///     Provenance metadata for tracing the origin and retrieval of an event.
/// </summary>
public class ProvenanceInfo
{
    /// <summary>
    ///     Identifier for the source system or component.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    ///     UTC timestamp when the event was retrieved into the current pipeline.
    /// </summary>
    public DateTime RetrievedAtUtc { get; set; }

    /// <summary>
    ///     Optional correlation ID for linking related events.
    /// </summary>
    public string CorrelationId { get; set; }

    /// <summary>
    ///     Optional: Original schema version from the source.
    /// </summary>
    public string SourceSchemaVersion { get; set; }
}