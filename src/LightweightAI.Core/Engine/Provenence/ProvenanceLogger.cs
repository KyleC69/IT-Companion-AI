// Project Name: LightweightAI.Core
// File Name: ProvenanceLogger.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Interfaces;


namespace LightweightAI.Core.Engine.Provenence;


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