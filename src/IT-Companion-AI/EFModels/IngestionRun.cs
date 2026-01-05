// Project Name: SKAgent
// File Name: IngestionRun.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class IngestionRun
{
    public IngestionRun()
    {
        ApiFeatures_CreatedIngestionRunId = new List<ApiFeature>();
        ApiFeatures_UpdatedIngestionRunId = new List<ApiFeature>();
        ApiFeatures_RemovedIngestionRunId = new List<ApiFeature>();
        ApiMembers_CreatedIngestionRunId = new List<ApiMember>();
        ApiMembers_UpdatedIngestionRunId = new List<ApiMember>();
        ApiMembers_RemovedIngestionRunId = new List<ApiMember>();
        ApiTypes_CreatedIngestionRunId = new List<ApiType>();
        ApiTypes_UpdatedIngestionRunId = new List<ApiType>();
        ApiTypes_RemovedIngestionRunId = new List<ApiType>();
        CodeBlocks_CreatedIngestionRunId = new List<CodeBlock>();
        CodeBlocks_UpdatedIngestionRunId = new List<CodeBlock>();
        CodeBlocks_RemovedIngestionRunId = new List<CodeBlock>();
        DocPages_CreatedIngestionRunId = new List<DocPage>();
        DocPages_UpdatedIngestionRunId = new List<DocPage>();
        DocPages_RemovedIngestionRunId = new List<DocPage>();
        DocSections_CreatedIngestionRunId = new List<DocSection>();
        DocSections_UpdatedIngestionRunId = new List<DocSection>();
        DocSections_RemovedIngestionRunId = new List<DocSection>();
        SourceSnapshots_IngestionRunId = new List<SourceSnapshot>();
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public DateTime TimestampUtc { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string SchemaVersion { get; set; }

    public string Notes { get; set; }


    public virtual ICollection<ApiFeature> ApiFeatures_CreatedIngestionRunId { get; set; }


    public virtual ICollection<ApiFeature> ApiFeatures_UpdatedIngestionRunId { get; set; }


    public virtual ICollection<ApiFeature> ApiFeatures_RemovedIngestionRunId { get; set; }


    public virtual ICollection<ApiMember> ApiMembers_CreatedIngestionRunId { get; set; }


    public virtual ICollection<ApiMember> ApiMembers_UpdatedIngestionRunId { get; set; }


    public virtual ICollection<ApiMember> ApiMembers_RemovedIngestionRunId { get; set; }


    public virtual ICollection<ApiType> ApiTypes_CreatedIngestionRunId { get; set; }


    public virtual ICollection<ApiType> ApiTypes_UpdatedIngestionRunId { get; set; }


    public virtual ICollection<ApiType> ApiTypes_RemovedIngestionRunId { get; set; }


    public virtual ICollection<CodeBlock> CodeBlocks_CreatedIngestionRunId { get; set; }


    public virtual ICollection<CodeBlock> CodeBlocks_UpdatedIngestionRunId { get; set; }


    public virtual ICollection<CodeBlock> CodeBlocks_RemovedIngestionRunId { get; set; }


    public virtual ICollection<DocPage> DocPages_CreatedIngestionRunId { get; set; }


    public virtual ICollection<DocPage> DocPages_UpdatedIngestionRunId { get; set; }


    public virtual ICollection<DocPage> DocPages_RemovedIngestionRunId { get; set; }


    public virtual ICollection<DocSection> DocSections_CreatedIngestionRunId { get; set; }


    public virtual ICollection<DocSection> DocSections_UpdatedIngestionRunId { get; set; }


    public virtual ICollection<DocSection> DocSections_RemovedIngestionRunId { get; set; }


    public virtual ICollection<SourceSnapshot> SourceSnapshots_IngestionRunId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}