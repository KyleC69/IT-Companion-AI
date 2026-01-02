using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class DocSection
{
    public Guid Id { get; set; }

    public Guid DocPageId { get; set; }

    public string SectionUid { get; set; } = null!;

    public string? Heading { get; set; }

    public int? Level { get; set; }

    public string? ContentMarkdown { get; set; }

    public int? OrderIndex { get; set; }

    public virtual ICollection<CodeBlock> CodeBlocks { get; set; } = new List<CodeBlock>();

    public virtual DocPage DocPage { get; set; } = null!;
}
