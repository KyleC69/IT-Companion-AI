using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class SampleApiMemberLink
{
    public Guid Id { get; set; }

    public Guid SampleId { get; set; }

    public string MemberUid { get; set; } = null!;

    public virtual Sample Sample { get; set; } = null!;
}
