using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class FeatureMemberLink
{
    public Guid Id { get; set; }

    public Guid FeatureId { get; set; }

    public string MemberUid { get; set; } = null!;

    public string? Role { get; set; }

    public virtual Feature Feature { get; set; } = null!;
}
