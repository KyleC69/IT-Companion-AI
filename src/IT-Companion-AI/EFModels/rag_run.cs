using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("rag_run")]
[Index("snapshot_id", "timestamp_utc", Name = "ix_rag_run_snapshot_time")]
public partial class rag_run
{
    [Key]
    public Guid id { get; set; }

    public Guid snapshot_id { get; set; }

    public DateTime timestamp_utc { get; set; }

    [StringLength(200)]
    public string schema_version { get; set; } = null!;

    [InverseProperty("rag_run")]
    public virtual ICollection<rag_chunk> rag_chunks { get; set; } = new List<rag_chunk>();

    [ForeignKey("snapshot_id")]
    [InverseProperty("rag_runs")]
    public virtual source_snapshot snapshot { get; set; } = null!;
}
