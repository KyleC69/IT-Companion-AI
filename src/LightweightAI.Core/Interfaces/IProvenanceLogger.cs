// Ensure interface exposes Log used by FusionBroker


using LightweightAI.Core.Engine.Provenance;

namespace LightweightAI.Core.Interfaces;

public interface IProvenanceLogger
{
    void Log(ProvenancedDecision decision);
}