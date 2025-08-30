// Project Name: LightweightAI.Core
// File Name: MemoryAnomalyLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Interfaces;


namespace LightweightAI.Core.Builders;


public class MemoryAnomalyLoader(IMemorySource source) : IEventLoader<MemoryAnomalyEvent>
{
    private readonly IMemorySource _source = source ?? throw new ArgumentNullException(nameof(source));

    // Public convenience method
    public IEnumerable<MemoryAnomalyEvent> LoadEvents() => Enumerate();

    // Explicit interface implementation (in case of subtle signature mismatch)
    IEnumerable<MemoryAnomalyEvent> IEventLoader<MemoryAnomalyEvent>.LoadEvents() => Enumerate();

    private IEnumerable<MemoryAnomalyEvent> Enumerate()
    {
        foreach (MemoryAnomalyEvent anomaly in this._source.GetAnomalies())
            yield return new MemoryAnomalyEvent
            {
                SchemaVersion = "1.0",
                TimestampUtc = anomaly.TimestampUtc,
                ProcessId = anomaly.ProcessId,
                AnomalyType = anomaly.AnomalyType,
                Severity = anomaly.Severity,
                Provenance = new ProvenanceInfo
                {
                    Source = "MemoryAnomalyLoader",
                    RetrievedAtUtc = DateTime.UtcNow
                }
            };
    }
}