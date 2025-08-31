// Added missing properties required by sinks and logger


using System.Collections.Immutable;



namespace LightweightAI.Core.Models;

public sealed class ProvenancedDecision
{
    public MetricDecision Metrics { get; set; } = null!;
    public int EventId { get; set; }
    public string FusionSignature { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public string ModelVersion { get; set; } = string.Empty;
    public double Severity { get; set; }
    public double Risk { get; set; }
    public string SeverityScaleRef { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public DateTimeOffset TimestampUtc => this.Timestamp == default ? DateTimeOffset.UtcNow : new DateTimeOffset(this.Timestamp, TimeSpan.Zero);
    public string CorrelationId { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public IImmutableDictionary<string, string> Tags { get; set; } = ImmutableDictionary<string, string>.Empty;
}