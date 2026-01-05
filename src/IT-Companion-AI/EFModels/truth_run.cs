using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("truth_run")]
[Index("snapshot_id", "timestamp_utc", Name = "ix_truth_run_snapshot_time")]
public partial class truth_run
{
    [Key]
    public Guid id { get; set; }

    public Guid snapshot_id { get; set; }

    public DateTime timestamp_utc { get; set; }

    [StringLength(200)]
    public string schema_version { get; set; } = null!;

    [InverseProperty("truth_run")]
    public virtual ICollection<api_feature> api_features { get; set; } = new List<api_feature>();

    [ForeignKey("snapshot_id")]
    [InverseProperty("truth_runs")]
    public virtual source_snapshot snapshot { get; set; } = null!;
}
