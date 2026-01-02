using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class ExecutionRun
{
    public Guid Id { get; set; }

    public Guid SnapshotId { get; set; }

    public Guid SampleRunId { get; set; }

    public DateTime TimestampUtc { get; set; }

    public string? EnvironmentJson { get; set; }

    public string SchemaVersion { get; set; } = null!;

    public virtual ICollection<ExecutionResult> ExecutionResults { get; set; } = new List<ExecutionResult>();

    public virtual SampleRun SampleRun { get; set; } = null!;

    public virtual SourceSnapshot Snapshot { get; set; } = null!;
}
