using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class ReviewRun
{
    public Guid Id { get; set; }

    public Guid SnapshotId { get; set; }

    public DateTime TimestampUtc { get; set; }

    public string SchemaVersion { get; set; } = null!;

    public virtual ICollection<ReviewItem> ReviewItems { get; set; } = new List<ReviewItem>();

    public virtual SourceSnapshot Snapshot { get; set; } = null!;
}
