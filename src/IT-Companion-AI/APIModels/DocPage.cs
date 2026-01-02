using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class DocPage
{
    public Guid Id { get; set; }

    public Guid SourceSnapshotId { get; set; }

    public string DocUid { get; set; } = null!;

    public string? SourcePath { get; set; }

    public string? Title { get; set; }

    public string? Language { get; set; }

    public string? Url { get; set; }

    public string? RawMarkdown { get; set; }

    public virtual ICollection<DocSection> DocSections { get; set; } = new List<DocSection>();

    public virtual SourceSnapshot SourceSnapshot { get; set; } = null!;
}
