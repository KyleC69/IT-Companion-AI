// Project Name: SKAgent
// File Name: CodeBlock.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class CodeBlock
{
    public CodeBlock()
    {
        IsActive = true;
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid DocSectionId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string SemanticUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    public string Content { get; set; }

    public string DeclaredPackages { get; set; }

    public string Tags { get; set; }

    public string InlineComments { get; set; }

    [NotNullValidator] public int VersionNumber { get; set; }

    [NotNullValidator] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }


    public virtual DocSection DocSection_DocSectionId { get; set; }


    public virtual IngestionRun IngestionRun_CreatedIngestionRunId { get; set; }


    public virtual IngestionRun IngestionRun_UpdatedIngestionRunId { get; set; }


    public virtual IngestionRun IngestionRun_RemovedIngestionRunId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}