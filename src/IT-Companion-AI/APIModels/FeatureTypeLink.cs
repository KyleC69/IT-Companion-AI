using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class FeatureTypeLink
{
    public Guid Id { get; set; }

    public Guid FeatureId { get; set; }

    public string TypeUid { get; set; } = null!;

    public string? Role { get; set; }

    public virtual Feature Feature { get; set; } = null!;
}
