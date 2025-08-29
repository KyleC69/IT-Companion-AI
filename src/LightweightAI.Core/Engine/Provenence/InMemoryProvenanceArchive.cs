// Project Name: LightweightAI.Core
// File Name: InMemoryProvenanceArchive.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Interfaces;


namespace LightweightAI.Core.Engine.Provenence;


public class InMemoryProvenanceArchive : IProvenanceArchive
{
    private readonly Dictionary<Guid, List<ProvenanceEntry>> _archive = new();





    public void Archive(Guid envelopeId, IEnumerable<ProvenanceEntry> entries)
    {
        if (!this._archive.TryGetValue(envelopeId, out List<ProvenanceEntry>? list))
        {
            list = new List<ProvenanceEntry>();
            this._archive[envelopeId] = list;
        }

        list.AddRange(entries);
        // TODO: Implement retention policies, pruning, etc.
    }





    public IReadOnlyList<ProvenanceEntry> Get(Guid id)
    {
        return this._archive.TryGetValue(id, out List<ProvenanceEntry>? list) ? list : Array.Empty<ProvenanceEntry>();
    }
}