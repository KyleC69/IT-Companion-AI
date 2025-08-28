// Project Name: LightweightAI.Core
// File Name: QAEnvelope.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine.models;


public record QAEnvelope<TAnswer>(
    Guid Id,
    string Question,
    TAnswer Answer,
    TrainingContext TrainingContext,
    ProvenanceLog Provenance
)
{
    // Append‑only helper
    public QAEnvelope<TAnswer> WithProvenance(string stage, string detail, object? parameters = null)
    {
        this.Provenance.Add(stage, detail,ProvImportance.Important, parameters);
        return this;
    }
}