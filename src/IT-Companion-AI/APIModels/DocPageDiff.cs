using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class DocPageDiff
{
    public Guid Id { get; set; }

    public Guid SnapshotDiffId { get; set; }

    public string DocUid { get; set; } = null!;

    public string? ChangeKind { get; set; }

    public string? DetailJson { get; set; }

    public virtual SnapshotDiff SnapshotDiff { get; set; } = null!;
}
