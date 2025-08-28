// Project Name: LightweightAI.Core
// File Name: ProvenenceLog.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


public record TrainingContext(
    string DatasetRevision,
    string PreprocessingHash,
    string ModelVersion,
    DateTime TrainingDateUtc,
    string[] Hyperparameters,
    string[] Metrics
);



public record ProvenanceEntry(
    string Stage,
    DateTime TimestampUtc,
    string Detail,
    object? Parameters = null
);



public class ProvenanceLog
{
    private readonly List<ProvenanceEntry> _entries = new();

    public IReadOnlyList<ProvenanceEntry> Entries => this._entries.AsReadOnly();





    public void Add(string stage, string detail, object? parameters = null)
    {
        this._entries.Add(new ProvenanceEntry(stage, DateTime.UtcNow, detail, parameters));
    }
}



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
        this.Provenance.Add(stage, detail, parameters);
        return this;
    }
}