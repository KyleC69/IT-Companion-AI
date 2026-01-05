// Project Name: SKAgent
// File Name: ApiFeature.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class ApiFeature
{
    public ApiFeature()
    {
        IsActive = true;
        FeatureDocLinks_FeatureId = new List<FeatureDocLink>();
        FeatureMemberLinks_FeatureId = new List<FeatureMemberLink>();
        FeatureTypeLinks_FeatureId = new List<FeatureTypeLink>();
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string SemanticUid { get; set; }

    [NotNullValidator] public Guid TruthRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Name { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    public string Description { get; set; }

    public string Tags { get; set; }

    [NotNullValidator] public int VersionNumber { get; set; }

    [NotNullValidator] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }


    public virtual TruthRun TruthRun_TruthRunId { get; set; }


    public virtual IngestionRun IngestionRun_CreatedIngestionRunId { get; set; }


    public virtual IngestionRun IngestionRun_UpdatedIngestionRunId { get; set; }


    public virtual IngestionRun IngestionRun_RemovedIngestionRunId { get; set; }


    public virtual SemanticIdentity SemanticIdentity_SemanticUidHash { get; set; }


    public virtual ICollection<FeatureDocLink> FeatureDocLinks_FeatureId { get; set; }


    public virtual ICollection<FeatureMemberLink> FeatureMemberLinks_FeatureId { get; set; }


    public virtual ICollection<FeatureTypeLink> FeatureTypeLinks_FeatureId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}