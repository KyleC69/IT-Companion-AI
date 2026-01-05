using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("review_item")]
[Index("target_kind", "target_uid", Name = "ix_review_item_target")]
public partial class review_item
{
    [Key]
    public Guid id { get; set; }

    public Guid review_run_id { get; set; }

    [StringLength(50)]
    public string target_kind { get; set; } = null!;

    [StringLength(1000)]
    public string target_uid { get; set; } = null!;

    [StringLength(50)]
    public string? status { get; set; }

    public string? summary { get; set; }

    [InverseProperty("review_item")]
    public virtual ICollection<review_issue> review_issues { get; set; } = new List<review_issue>();

    [ForeignKey("review_run_id")]
    [InverseProperty("review_items")]
    public virtual review_run review_run { get; set; } = null!;
}
