// Project Name: SKAgent
// File Name: RagRun.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class RagRun
{
    public RagRun()
    {
        RagChunks_RagRunId = new List<RagChunk>();
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid SnapshotId { get; set; }

    [NotNullValidator] public DateTime TimestampUtc { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string SchemaVersion { get; set; }


    public virtual ICollection<RagChunk> RagChunks_RagRunId { get; set; }


    public virtual SourceSnapshot SourceSnapshot_SnapshotId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}