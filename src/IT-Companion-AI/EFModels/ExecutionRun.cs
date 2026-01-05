// Project Name: SKAgent
// File Name: ExecutionRun.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class ExecutionRun
{
    public ExecutionRun()
    {
        ExecutionResults_ExecutionRunId = new List<ExecutionResult>();
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid SnapshotId { get; set; }

    [NotNullValidator] public Guid SampleRunId { get; set; }

    [NotNullValidator] public DateTime TimestampUtc { get; set; }

    public string EnvironmentJson { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string SchemaVersion { get; set; }


    public virtual ICollection<ExecutionResult> ExecutionResults_ExecutionRunId { get; set; }


    public virtual SourceSnapshot SourceSnapshot_SnapshotId { get; set; }


    public virtual SampleRun SampleRun_SampleRunId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}