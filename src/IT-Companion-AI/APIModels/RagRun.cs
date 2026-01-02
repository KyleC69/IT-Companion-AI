using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class RagRun
{
    public Guid Id { get; set; }

    public Guid SnapshotId { get; set; }

    public DateTime TimestampUtc { get; set; }

    public string SchemaVersion { get; set; } = null!;

    public virtual ICollection<RagChunk> RagChunks { get; set; } = new List<RagChunk>();

    public virtual SourceSnapshot Snapshot { get; set; } = null!;
}
