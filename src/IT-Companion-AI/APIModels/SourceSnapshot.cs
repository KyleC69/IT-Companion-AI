using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class SourceSnapshot
{
    public Guid Id { get; set; }

    public Guid IngestionRunId { get; set; }

    public string SnapshotUid { get; set; } = null!;

    public string? RepoUrl { get; set; }

    public string? Branch { get; set; }

    public string? Repocommit { get; set; }

    public string? Language { get; set; }

    public string? PackageName { get; set; }

    public string? PackageVersion { get; set; }

    public string? ConfigJson { get; set; }

    public virtual ICollection<ApiType> ApiTypes { get; set; } = new List<ApiType>();

    public virtual ICollection<DocPage> DocPages { get; set; } = new List<DocPage>();

    public virtual ICollection<ExecutionRun> ExecutionRuns { get; set; } = new List<ExecutionRun>();

    public virtual IngestionRun IngestionRun { get; set; } = null!;

    public virtual ICollection<RagRun> RagRuns { get; set; } = new List<RagRun>();

    public virtual ICollection<ReviewRun> ReviewRuns { get; set; } = new List<ReviewRun>();

    public virtual ICollection<SampleRun> SampleRuns { get; set; } = new List<SampleRun>();

    public virtual ICollection<SnapshotDiff> SnapshotDiffNewSnapshots { get; set; } = new List<SnapshotDiff>();

    public virtual ICollection<SnapshotDiff> SnapshotDiffOldSnapshots { get; set; } = new List<SnapshotDiff>();

    public virtual ICollection<TruthRun> TruthRuns { get; set; } = new List<TruthRun>();
}
