using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class SampleRun
{
    public Guid Id { get; set; }

    public Guid SnapshotId { get; set; }

    public DateTime TimestampUtc { get; set; }

    public string SchemaVersion { get; set; } = null!;

    public virtual ICollection<ExecutionRun> ExecutionRuns { get; set; } = new List<ExecutionRun>();

    public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();

    public virtual SourceSnapshot Snapshot { get; set; } = null!;
}
