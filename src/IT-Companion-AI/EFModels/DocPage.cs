// Project Name: SKAgent
// File Name: DocPage.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class DocPage
{
    public DocPage()
    {
        IsActive = true;
        DocSections_DocPageId = new List<DocSection>();
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string SemanticUid { get; set; }

    [NotNullValidator] public Guid SourceSnapshotId { get; set; }

    public string SourcePath { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Title { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    public string Url { get; set; }

    public string RawMarkdown { get; set; }

    [NotNullValidator] public int VersionNumber { get; set; }

    [NotNullValidator] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }


    public virtual SourceSnapshot SourceSnapshot_SourceSnapshotId { get; set; }


    public virtual IngestionRun IngestionRun_CreatedIngestionRunId { get; set; }


    public virtual IngestionRun IngestionRun_UpdatedIngestionRunId { get; set; }


    public virtual IngestionRun IngestionRun_RemovedIngestionRunId { get; set; }


    public virtual SemanticIdentity SemanticIdentity_SemanticUidHash { get; set; }


    public virtual ICollection<DocSection> DocSections_DocPageId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}