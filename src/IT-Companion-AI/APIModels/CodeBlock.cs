using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class CodeBlock
{
    public Guid Id { get; set; }

    public Guid DocSectionId { get; set; }

    public string? CodeUid { get; set; }

    public string? Language { get; set; }

    public string? Content { get; set; }

    public string? DeclaredPackages { get; set; }

    public string? Tags { get; set; }

    public string? InlineComments { get; set; }

    public virtual DocSection DocSection { get; set; } = null!;
}
