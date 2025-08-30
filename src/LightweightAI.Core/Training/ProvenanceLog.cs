// Project Name: LightweightAI.Core
// File Name: ProvenanceLog.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


// Consolidated variant: This Training.ProvenanceLog delegates to core Provenance.ProvenanceLog to avoid divergence.


using LightweightAI.Core.Engine.Provenance;
using LightweightAI.Core.Interfaces;
using LightweightAI.Core.Loaders.qANDa;


namespace LightweightAI.Core.Training;


public class ProvenanceLog
{
    private readonly ProvenanceLog _inner = new();

    public IReadOnlyList<ProvenanceEntry> Entries => this._inner.Entries;





    public void Add(string stage, string detail, ProvImportance parameters)
    {
        this._inner.Add(stage, detail, parameters);
    }





    public void Add(string stage, string detail, ProvImportance parameters, object? extra)
    {
        this._inner.Add(stage, detail, parameters, extra);
    }





    public void PruneNonEssential(Guid envelopeId, TimeSpan ageThreshold, IProvenanceArchive archive)
    {
        this._inner.PruneNonEssential(envelopeId, ageThreshold, archive);
    }
}