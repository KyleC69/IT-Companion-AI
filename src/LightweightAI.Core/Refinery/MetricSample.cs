// Project Name: LightweightAI.Core
// File Name: MetricSample.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Refinery;
// Shared EventId enforcement

<<<<<<< HEAD
    // ===== METRIC REFINERY =====

    // ===== Q&A REFINERY =====
=======

// ===== METRIC REFINERY =====

// ===== Q&A REFINERY =====
>>>>>>> 32a1a31 (WIP Refactor mostly complete, Loaders and Telemetry hooks implemented. Does not build. Push for GH action manifest)

    // ====== SAMPLE DOMAIN TYPES (simplified) ======
    public sealed record MetricSample(
        string EventId,
        string MetricKey,
        double Value,
        DateTimeOffset Timestamp,
        string SourceId
    );
}