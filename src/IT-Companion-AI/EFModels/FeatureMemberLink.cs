// Project Name: SKAgent
// File Name: FeatureMemberLink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class FeatureMemberLink
{
    public FeatureMemberLink()
    {
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid FeatureId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string MemberUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string Role { get; set; }


    public virtual ApiFeature ApiFeature_FeatureId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}