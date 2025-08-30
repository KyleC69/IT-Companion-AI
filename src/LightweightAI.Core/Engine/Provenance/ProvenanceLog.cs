// Project Name: LightweightAI.Core
// File Name: ProvenanceLog.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Interfaces;
using LightweightAI.Core.Loaders.qANDa;


namespace LightweightAI.Core.Engine.Provenance;


public class ProvenanceLog
{
    private readonly List<ProvenanceEntry> _entries = new();

    public IReadOnlyList<ProvenanceEntry> Entries => this._entries.AsReadOnly();





    public void Add(string stage, string detail, ProvImportance importance = ProvImportance.Important,
        object? parameters = null)
    {
        this._entries.Add(new ProvenanceEntry(stage, DateTime.UtcNow, detail, importance, parameters));
    }





    // Provide overload to match training namespace LightweightAI.Core.variant signature
    public void Add(string stage, string detail, ProvImportance importance)
    {
        this._entries.Add(new ProvenanceEntry(stage, DateTime.UtcNow, detail, importance));
    }





    // Prune: move verbose, old entries to archive; add one summary breadcrumb.
    public void PruneNonEssential(Guid envelopeId, TimeSpan ageThreshold, IProvenanceArchive archive)
    {
        DateTime cutoff = DateTime.UtcNow - ageThreshold;
        List<ProvenanceEntry> toArchive = this._entries
            .Where(e => e.Importance == ProvImportance.Verbose && e.TimestampUtc < cutoff).ToList();

        if (toArchive.Count == 0) return;

        archive.Archive(envelopeId, toArchive);
        this._entries.RemoveAll(e => toArchive.Contains(e));

        Add("provenance",
            $"Pruned {toArchive.Count} verbose entries older than {ageThreshold}",
            ProvImportance.Important,
            new { prunedCount = toArchive.Count, thresholdSeconds = (int)ageThreshold.TotalSeconds });
    }
}