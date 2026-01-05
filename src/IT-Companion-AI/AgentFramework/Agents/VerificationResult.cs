// Project Name: SKAgent
// File Name: VerificationResult.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Text.Json.Serialization;


// ============================================================================
// AGENTS: Verification + Reconciliation
// ============================================================================


namespace ITCompanionAI.AgentFramework.Agents;


public sealed class VerificationResult
{
    [JsonPropertyName("verified")] public bool Verified { get; init; }

    [JsonPropertyName("confidence")] public double Confidence { get; init; }

    [JsonPropertyName("deprecated")] public bool Deprecated { get; init; }

    [JsonPropertyName("notes")] public string Notes { get; init; } = string.Empty;
}