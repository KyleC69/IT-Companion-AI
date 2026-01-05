// Project Name: SKAgent
// File Name: Sample.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class Sample
{
    public Sample()
    {
        SampleApiMemberLinks_SampleId = new List<SampleApiMemberLink>();
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid SampleRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string SampleUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string FeatureUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    public string Code { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string EntryPoint { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string TargetFramework { get; set; }

    public string PackageReferences { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string DerivedFromCodeUid { get; set; }

    public string Tags { get; set; }


    public virtual SampleRun SampleRun_SampleRunId { get; set; }


    public virtual ICollection<SampleApiMemberLink> SampleApiMemberLinks_SampleId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}