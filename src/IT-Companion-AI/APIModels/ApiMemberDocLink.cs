using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class ApiMemberDocLink
{
    public Guid Id { get; set; }

    public Guid ApiMemberId { get; set; }

    public string DocUid { get; set; } = null!;

    public string? SectionUid { get; set; }

    public virtual ApiMember ApiMember { get; set; } = null!;
}
