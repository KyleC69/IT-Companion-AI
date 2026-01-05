// Project Name: SKAgent
// File Name: DocSection.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class DocSection
{
    public DocSection()
    {
        IsActive = true;
        CodeBlocks_DocSectionId = new List<CodeBlock>();
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid DocPageId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string SemanticUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Heading { get; set; }

    public int? Level { get; set; }

    public string ContentMarkdown { get; set; }

    public int? OrderIndex { get; set; }

    [NotNullValidator] public int VersionNumber { get; set; }

    [NotNullValidator] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }


    public virtual ICollection<CodeBlock> CodeBlocks_DocSectionId { get; set; }


    public virtual DocPage DocPage_DocPageId { get; set; }


    public virtual IngestionRun IngestionRun_CreatedIngestionRunId { get; set; }


    public virtual IngestionRun IngestionRun_UpdatedIngestionRunId { get; set; }


    public virtual IngestionRun IngestionRun_RemovedIngestionRunId { get; set; }


    public virtual SemanticIdentity SemanticIdentity_SemanticUidHash { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}