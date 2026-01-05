// Project Name: SKAgent
// File Name: RagChunk.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class RagChunk
{
    public RagChunk()
    {
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid RagRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string ChunkUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 100, RangeBoundaryType.Inclusive)]
    public string Kind { get; set; }

    public string Text { get; set; }

    public string MetadataJson { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1536, RangeBoundaryType.Inclusive)]
    public string EmbeddingVector { get; set; }


    public virtual RagRun RagRun_RagRunId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}