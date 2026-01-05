// Project Name: SKAgent
// File Name: SampleApiMemberLink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class SampleApiMemberLink
{
    public SampleApiMemberLink()
    {
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid SampleId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string MemberUid { get; set; }


    public virtual Sample Sample_SampleId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}