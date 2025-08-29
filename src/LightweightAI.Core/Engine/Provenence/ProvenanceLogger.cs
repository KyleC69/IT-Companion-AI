// Project Name: LightweightAI.Core
// File Name: ProvenanceLogger.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Interfaces;


namespace LightweightAI.Core.Engine.Provenence;


// === ProvenanceLogger.cs ===
/// <summary>
/// Simple console-based implementation of <see cref="IProvenanceLogger"/> intended for
/// early development. Emits formatted decision metadata; production version should
/// enforce append-only storage and integrity hashing.
/// </summary>
public class ProvenanceLogger : IProvenanceLogger
{
    public void Log(ProvenancedDecision decision)
    {
        Console.WriteLine($"[PROVENANCE] Event={decision.EventId} | FusionSig={decision.FusionSignature} " +
                          $"| Model={decision.ModelId}@{decision.ModelVersion} | Severity={decision.Severity} " +
                          $"| ScaleRev={decision.SeverityScaleRef} | Time={decision.Timestamp:o}");
    }
}