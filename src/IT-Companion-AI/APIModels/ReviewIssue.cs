using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class ReviewIssue
{
    public Guid Id { get; set; }

    public Guid ReviewItemId { get; set; }

    public string? Code { get; set; }

    public string? Severity { get; set; }

    public string? RelatedMemberUid { get; set; }

    public string? Details { get; set; }

    public virtual ReviewItem ReviewItem { get; set; } = null!;
}
