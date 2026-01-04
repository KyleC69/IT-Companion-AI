// Project Name: SKAgent
// File Name: rag_run.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("rag_run")]
[Index("snapshot_id", Name = "idx_rag_run_snapshot_id")]
public class rag_run
{
    [Key] public Guid id { get; set; }

    public Guid snapshot_id { get; set; }

    public DateTime timestamp_utc { get; set; }

    [StringLength(200)] public string schema_version { get; set; } = null!;

    [InverseProperty("rag_run")] public virtual ICollection<rag_chunk> rag_chunks { get; set; } = [];

    [ForeignKey("snapshot_id")]
    [InverseProperty("rag_runs")]
    public virtual source_snapshot snapshot { get; set; } = null!;
}