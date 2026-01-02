using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class SnapshotDiff
{
    public Guid Id { get; set; }

    public Guid OldSnapshotId { get; set; }

    public Guid NewSnapshotId { get; set; }

    public DateTime TimestampUtc { get; set; }

    public string SchemaVersion { get; set; } = null!;

    public virtual ICollection<ApiMemberDiff> ApiMemberDiffs { get; set; } = new List<ApiMemberDiff>();

    public virtual ICollection<ApiTypeDiff> ApiTypeDiffs { get; set; } = new List<ApiTypeDiff>();

    public virtual ICollection<DocPageDiff> DocPageDiffs { get; set; } = new List<DocPageDiff>();

    public virtual SourceSnapshot NewSnapshot { get; set; } = null!;

    public virtual SourceSnapshot OldSnapshot { get; set; } = null!;
}
