using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("sample_run")]
[Index("snapshot_id", "timestamp_utc", Name = "ix_sample_run_snapshot_time")]
public partial class sample_run
{
    [Key]
    public Guid id { get; set; }

    public Guid snapshot_id { get; set; }

    public DateTime timestamp_utc { get; set; }

    [StringLength(200)]
    public string schema_version { get; set; } = null!;

    [InverseProperty("sample_run")]
    public virtual ICollection<execution_run> execution_runs { get; set; } = new List<execution_run>();

    [InverseProperty("sample_run")]
    public virtual ICollection<sample> samples { get; set; } = new List<sample>();

    [ForeignKey("snapshot_id")]
    [InverseProperty("sample_runs")]
    public virtual source_snapshot snapshot { get; set; } = null!;
}
