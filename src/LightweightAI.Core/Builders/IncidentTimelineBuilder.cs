// Project Name: LightweightAI.Core
// File Name: IncidentTimelineBuilder.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Interfaces;
using LightweightAI.Core.Models;


namespace LightweightAI.Core.Builders;


public class IncidentTimelineBuilder
{
    public IncidentTimelineBuilder WithMemoryAnomalies(
        IEventLoader<MemoryAnomalyEvent> loader)
    {
        foreach (var anomaly in loader.Load())
            _events.Add(new TimelineEvent
            {
                TimestampUtc = anomaly.TimestampUtc,
                Category = "MemoryAnomaly",
                Details = $"{anomaly.AnomalyType} (Severity: {anomaly.Severity})",
                Provenance = anomaly.Provenance
            });
        return this;
    }
}