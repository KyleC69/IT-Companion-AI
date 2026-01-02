using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class TruthRun
{
    public Guid Id { get; set; }

    public Guid SnapshotId { get; set; }

    public DateTime TimestampUtc { get; set; }

    public string SchemaVersion { get; set; } = null!;

    public virtual ICollection<Feature> Features { get; set; } = new List<Feature>();

    public virtual SourceSnapshot Snapshot { get; set; } = null!;
}
