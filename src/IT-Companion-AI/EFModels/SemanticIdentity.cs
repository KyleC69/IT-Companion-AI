// Project Name: SKAgent
// File Name: SemanticIdentity.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class SemanticIdentity
{
    public SemanticIdentity()
    {
        ApiFeatures_SemanticUidHash = new List<ApiFeature>();
        DocPages_SemanticUidHash = new List<DocPage>();
        DocSections_SemanticUidHash = new List<DocSection>();
        OnCreated();
    }







    [NotNullValidator] public byte[] UidHash { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string Uid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string Kind { get; set; }

    [NotNullValidator] public DateTime CreatedUtc { get; set; }

    public string Notes { get; set; }


    public virtual ICollection<ApiFeature> ApiFeatures_SemanticUidHash { get; set; }


    public virtual ICollection<DocPage> DocPages_SemanticUidHash { get; set; }


    public virtual ICollection<DocSection> DocSections_SemanticUidHash { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}