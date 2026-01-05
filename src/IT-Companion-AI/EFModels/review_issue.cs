using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("review_issue")]
[Index("severity", Name = "ix_review_issue_severity")]
public partial class review_issue
{
    [Key]
    public Guid id { get; set; }

    public Guid review_item_id { get; set; }

    [StringLength(200)]
    public string? code { get; set; }

    [StringLength(50)]
    public string? severity { get; set; }

    [StringLength(1000)]
    public string? related_member_uid { get; set; }

    public string? details { get; set; }

    [ForeignKey("review_item_id")]
    [InverseProperty("review_issues")]
    public virtual review_item review_item { get; set; } = null!;
}
