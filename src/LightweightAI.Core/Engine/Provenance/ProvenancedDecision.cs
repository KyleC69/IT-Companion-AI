// Project Name: LightweightAI.Core
// File Name: ProvenancedDecision.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections.Immutable;

namespace LightweightAI.Core.Engine.Provenance
{
    // Harmonized with usages in alerts & pipeline (TimestampUtc, CorrelationId, Risk, Summary expected)
    public sealed class ProvenancedDecision
    {
        public MetricDecision Metrics { get; set; } = null!; // core metric decision

        // Identity / provenance
        public int EventId { get; set; }
        public string FusionSignature { get; set; } = string.Empty;
        public string ModelId { get; set; } = string.Empty;
        public string ModelVersion { get; set; } = string.Empty;

        // Core scoring / severity
        public double Severity { get; set; }
        public double Risk { get; set; } // additional risk scalar used by sinks

        // Timing
        public DateTime Timestamp { get; set; } // kept for backward compat
        public DateTimeOffset TimestampUtc => Timestamp == default ? DateTimeOffset.UtcNow : new DateTimeOffset(Timestamp, TimeSpan.Zero);

        // Narrative / correlation
        public string CorrelationId { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        // Optional tags/extended attributes
        public IImmutableDictionary<string, string> Tags { get; set; } = ImmutableDictionary<string, string>.Empty;
    }
}