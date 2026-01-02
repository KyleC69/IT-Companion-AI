using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class ExecutionResult
{
    public Guid Id { get; set; }

    public Guid ExecutionRunId { get; set; }

    public string SampleUid { get; set; } = null!;

    public string? Status { get; set; }

    public string? BuildLog { get; set; }

    public string? RunLog { get; set; }

    public string? ExceptionJson { get; set; }

    public int? DurationMs { get; set; }

    public virtual ExecutionRun ExecutionRun { get; set; } = null!;
}
