// Project Name: LightweightAI.Core
// File Name: ProvenanceLog.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers

// Consolidated variant: This Training.ProvenanceLog delegates to core Provenence.ProvenanceLog to avoid divergence.

using LightweightAI.Core.Engine.Provenence;
using LightweightAI.Core.Engine.Interfaces;

public class ProvenanceLog
{
    private readonly LightweightAI.Core.Engine.Provenence.ProvenanceLog _inner = new();

    public IReadOnlyList<ProvenanceEntry> Entries => _inner.Entries;

    public void Add(string stage, string detail, ProvImportance parameters)
    {
        _inner.Add(stage, detail, parameters);
    }

    public void Add(string stage, string detail, ProvImportance parameters, object? extra)
    {
        _inner.Add(stage, detail, parameters, extra);
    }

    public void PruneNonEssential(Guid envelopeId, TimeSpan ageThreshold, IProvenanceArchive archive)
    {
        _inner.PruneNonEssential(envelopeId, ageThreshold, archive);
    }
}