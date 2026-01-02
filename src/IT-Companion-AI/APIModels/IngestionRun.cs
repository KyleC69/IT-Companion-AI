using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class IngestionRun
{
    public Guid Id { get; set; }

    public DateTime TimestampUtc { get; set; }

    public string SchemaVersion { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual ICollection<SourceSnapshot> SourceSnapshots { get; set; } = new List<SourceSnapshot>();
}
