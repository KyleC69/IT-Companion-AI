// Ensure interface exposes Log used by FusionBroker


namespace LightweightAI.Core.Interfaces;

public interface IProvenanceLogger
{
    void Log(ProvenancedDecision decision);
}