using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class ReviewItem
{
    public Guid Id { get; set; }

    public Guid ReviewRunId { get; set; }

    public string TargetKind { get; set; } = null!;

    public string TargetUid { get; set; } = null!;

    public string? Status { get; set; }

    public string? Summary { get; set; }

    public virtual ICollection<ReviewIssue> ReviewIssues { get; set; } = new List<ReviewIssue>();

    public virtual ReviewRun ReviewRun { get; set; } = null!;
}
