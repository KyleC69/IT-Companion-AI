using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class ApiMemberDiff
{
    public Guid Id { get; set; }

    public Guid SnapshotDiffId { get; set; }

    public string MemberUid { get; set; } = null!;

    public string? ChangeKind { get; set; }

    public string? OldSignature { get; set; }

    public string? NewSignature { get; set; }

    public bool? Breaking { get; set; }

    public string? DetailJson { get; set; }

    public virtual SnapshotDiff SnapshotDiff { get; set; } = null!;
}
