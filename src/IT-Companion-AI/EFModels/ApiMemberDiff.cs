// Project Name: SKAgent
// File Name: ApiMemberDiff.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class ApiMemberDiff
{
    public ApiMemberDiff()
    {
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid SnapshotDiffId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string MemberUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string ChangeKind { get; set; }

    public string OldSignature { get; set; }

    public string NewSignature { get; set; }

    public bool? Breaking { get; set; }

    public string DetailJson { get; set; }


    public virtual SnapshotDiff SnapshotDiff_SnapshotDiffId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}