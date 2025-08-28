// Project Name: LightweightAI.Core
// File Name: Contracts.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public readonly record struct ProvenanceKey(Guid HostId, string SourceId, int RawEventId);



public sealed record MetricReading(
    DateTime TimestampUtc,
    string Signal, // e.g., "sec.failed_logons_per_min"
    double Value,
    ProvenanceKey Provenance);



public sealed record AnomalySignal(
    MetricReading Reading,
    bool IsAnomaly,
    double Score, // model score
    double PValue, // lower = more anomalous
    string Detector, // e.g., "ssa_spike"
    string Notes,
    double ZScore)
{
}



public interface IClock
{
    DateTime UtcNow { get; }
}



public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}