// Project Name: SKAgent
// File Name: SnapshotDiff.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class SnapshotDiff
{
    public SnapshotDiff()
    {
        ApiMemberDiffs_SnapshotDiffId = new List<ApiMemberDiff>();
        ApiTypeDiffs_SnapshotDiffId = new List<ApiTypeDiff>();
        DocPageDiffs_SnapshotDiffId = new List<DocPageDiff>();
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid OldSnapshotId { get; set; }

    [NotNullValidator] public Guid NewSnapshotId { get; set; }

    [NotNullValidator] public DateTime TimestampUtc { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string SchemaVersion { get; set; }


    public virtual ICollection<ApiMemberDiff> ApiMemberDiffs_SnapshotDiffId { get; set; }


    public virtual ICollection<ApiTypeDiff> ApiTypeDiffs_SnapshotDiffId { get; set; }


    public virtual ICollection<DocPageDiff> DocPageDiffs_SnapshotDiffId { get; set; }


    public virtual SourceSnapshot SourceSnapshot_OldSnapshotId { get; set; }


    public virtual SourceSnapshot SourceSnapshot_NewSnapshotId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}