// Project Name: SKAgent
// File Name: SourceSnapshot.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class SourceSnapshot
{
    public SourceSnapshot()
    {
        ApiTypes_SourceSnapshotId = new List<ApiType>();
        DocPages_SourceSnapshotId = new List<DocPage>();
        ExecutionRuns_SnapshotId = new List<ExecutionRun>();
        RagRuns_SnapshotId = new List<RagRun>();
        ReviewRuns_SnapshotId = new List<ReviewRun>();
        SampleRuns_SnapshotId = new List<SampleRun>();
        SnapshotDiffs_OldSnapshotId = new List<SnapshotDiff>();
        SnapshotDiffs_NewSnapshotId = new List<SnapshotDiff>();
        TruthRuns_SnapshotId = new List<TruthRun>();
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid IngestionRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string SnapshotUid { get; set; }

    public string RepoUrl { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Branch { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string RepoCommit { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string PackageName { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string PackageVersion { get; set; }

    public string ConfigJson { get; set; }

    public byte[] SnapshotUidHash { get; set; }


    public virtual ICollection<ApiType> ApiTypes_SourceSnapshotId { get; set; }


    public virtual ICollection<DocPage> DocPages_SourceSnapshotId { get; set; }


    public virtual ICollection<ExecutionRun> ExecutionRuns_SnapshotId { get; set; }


    public virtual ICollection<RagRun> RagRuns_SnapshotId { get; set; }


    public virtual ICollection<ReviewRun> ReviewRuns_SnapshotId { get; set; }


    public virtual ICollection<SampleRun> SampleRuns_SnapshotId { get; set; }


    public virtual ICollection<SnapshotDiff> SnapshotDiffs_OldSnapshotId { get; set; }


    public virtual ICollection<SnapshotDiff> SnapshotDiffs_NewSnapshotId { get; set; }


    public virtual IngestionRun IngestionRun_IngestionRunId { get; set; }


    public virtual ICollection<TruthRun> TruthRuns_SnapshotId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}