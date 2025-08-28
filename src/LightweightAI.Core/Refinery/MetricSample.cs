// Project Name: LightweightAI.Core
// File Name: MetricSample.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.DataRefineries
{
    // Shared EventId enforcement


    // ===== METRIC REFINERY =====


    // ===== Q&A REFINERY =====
}



// ====== SAMPLE DOMAIN TYPES (simplified) ======
public sealed record MetricSample(
    string EventId,
    string MetricKey,
    double Value,
    DateTimeOffset Timestamp,
    string SourceId
);