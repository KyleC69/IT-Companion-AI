// Project Name: SKAgent
// File Name: sample_run.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("sample_run")]
[Index("snapshot_id", Name = "idx_sample_run_snapshot_id")]
public class sample_run
{
    [Key] public Guid id { get; set; }

    public Guid snapshot_id { get; set; }

    public DateTime timestamp_utc { get; set; }

    [StringLength(200)] public string schema_version { get; set; } = null!;

    [InverseProperty("sample_run")] public virtual ICollection<execution_run> execution_runs { get; set; } = [];

    [InverseProperty("sample_run")] public virtual ICollection<code_sample> samples { get; set; } = [];

    [ForeignKey("snapshot_id")]
    [InverseProperty("sample_runs")]
    public virtual source_snapshot snapshot { get; set; } = null!;
}