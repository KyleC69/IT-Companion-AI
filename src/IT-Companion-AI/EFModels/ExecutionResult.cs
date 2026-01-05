// Project Name: SKAgent
// File Name: ExecutionResult.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class ExecutionResult
{
    public ExecutionResult()
    {
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [NotNullValidator] public Guid ExecutionRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string SampleUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 100, RangeBoundaryType.Inclusive)]
    public string Status { get; set; }

    public string BuildLog { get; set; }

    public string RunLog { get; set; }

    public string ExceptionJson { get; set; }

    public int? DurationMs { get; set; }


    public virtual ExecutionRun ExecutionRun_ExecutionRunId { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}