using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class FeatureDocLink
{
    public Guid Id { get; set; }

    public Guid FeatureId { get; set; }

    public string DocUid { get; set; } = null!;

    public string? SectionUid { get; set; }

    public virtual Feature Feature { get; set; } = null!;
}
