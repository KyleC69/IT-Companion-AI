// Project Name: LightweightAI.Core
// File Name: ProvenanceLog.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


// Consolidated variant: This Training.ProvenanceLog delegates to core Provenance.ProvenanceLog to avoid divergence.


using LightweightAI.Core.Loaders.Conversational;



namespace LightweightAI.Core.Training;

/// <summary>
/// Represents a log that tracks the provenance of operations or events during the training process.
/// </summary>
/// <remarks>
/// This class provides functionality to record and manage provenance entries, 
/// which include details about specific stages, their importance, and associated parameters.
/// It supports pruning of non-essential entries based on specified criteria.
/// </remarks>
public class ProvenanceLog
{
    
    public ProvenanceLog()
    {
        _entries = new List<ProvenanceEntry>();
    }
    
    private List<ProvenanceEntry> _entries = new();






    public ProvenanceLog(List<ProvenanceEntry> entries)
    {
        _entries = entries;
    }






    public ProvenanceLog(IReadOnlyList<ProvenanceEntry> entries)
    {
        _entries = entries.ToList();
    }
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