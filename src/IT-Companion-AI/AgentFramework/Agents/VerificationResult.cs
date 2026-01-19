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