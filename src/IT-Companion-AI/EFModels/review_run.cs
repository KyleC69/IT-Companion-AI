using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("review_run")]
[Index("snapshot_id", "timestamp_utc", Name = "ix_review_run_snapshot_time")]
public partial class review_run
{
    [Key]
    public Guid id { get; set; }

    public Guid snapshot_id { get; set; }

    public DateTime timestamp_utc { get; set; }

    [StringLength(200)]
    public string schema_version { get; set; } = null!;

    [InverseProperty("review_run")]
    public virtual ICollection<review_item> review_items { get; set; } = new List<review_item>();

    [ForeignKey("snapshot_id")]
    [InverseProperty("review_runs")]
    public virtual source_snapshot snapshot { get; set; } = null!;
}
