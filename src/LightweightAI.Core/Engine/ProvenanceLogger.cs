// Project Name: LightweightAI.Core
// File Name: ProvenanceLogger.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine;


namespace AICompanion.Tests;


// === ProvenanceLogger.cs ===
public class ProvenanceLogger : IProvenanceLogger
{
    public void Log(ProvenancedDecision decision)
    {
        Console.WriteLine($"[PROVENANCE] Event={decision.EventId} | FusionSig={decision.FusionSignature} " +
                          $"| Model={decision.ModelId}@{decision.ModelVersion} | Severity={decision.Severity} " +
                          $"| ScaleRev={decision.SeverityScaleRef} | Time={decision.Timestamp:o}");
    }
}



public sealed record ProvenancedDecision
{
    public required MetricDecision Metrics { get; init; }

    public float EventId { get; init; }
    public required string FusionSignature { get; init; }
    public required string ModelId { get; init; }
    public required string ModelVersion { get; init; }
    public double Severity { get; init; }
    public string SeverityScaleRef { get; init; } //  HACK:  Property is not being used consistently. Must FIX
    public DateTime Timestamp { get; init; }
}