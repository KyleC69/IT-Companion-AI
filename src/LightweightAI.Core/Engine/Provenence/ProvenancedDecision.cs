// Added missing properties required by sinks and logger
using System.Collections.Immutable;

namespace LightweightAI.Core.Engine.Provenence;

public sealed class ProvenancedDecision
{
<<<<<<< HEAD
    public required MetricDecision Metrics { get; init; }

    public float EventId { get; init; }
    public required string FusionSignature { get; init; }
    public required string ModelId { get; init; }
    public required string ModelVersion { get; init; }
    public double Severity { get; init; }
    public required string SeverityScaleRef { get; init; }
    public DateTime Timestamp { get; init; }
=======
    public MetricDecision Metrics { get; set; } = null!;
    public int EventId { get; set; }
    public string FusionSignature { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public string ModelVersion { get; set; } = string.Empty;
    public double Severity { get; set; }
    public double Risk { get; set; }
    public string SeverityScaleRef { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public DateTimeOffset TimestampUtc => Timestamp == default ? DateTimeOffset.UtcNow : new DateTimeOffset(Timestamp, TimeSpan.Zero);
    public string CorrelationId { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public IImmutableDictionary<string, string> Tags { get; set; } = ImmutableDictionary<string, string>.Empty;
>>>>>>> 32a1a31 (WIP Refactor mostly complete, Loaders and Telemetry hooks implemented. Does not build. Push for GH action manifest)
}