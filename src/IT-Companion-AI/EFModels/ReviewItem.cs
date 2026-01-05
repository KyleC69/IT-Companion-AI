// Project Name: SKAgent
// File Name: ReviewItem.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class ReviewItem
{
    public ReviewItem()
    {
        ReviewIssues_ReviewItemId = new List<ReviewIssue>();
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid ReviewRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string TargetKind { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string TargetUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string Status { get; set; }

    public string Summary { get; set; }


    public virtual ICollection<ReviewIssue> ReviewIssues_ReviewItemId { get; set; }


    public virtual ReviewRun ReviewRun_ReviewRunId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}