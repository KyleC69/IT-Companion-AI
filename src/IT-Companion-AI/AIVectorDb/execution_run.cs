// Project Name: SKAgent
// File Name: execution_run.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("execution_run")]
[Index("sample_run_id", Name = "idx_execution_run_sample_run_id")]
[Index("snapshot_id", Name = "idx_execution_run_snapshot_id")]
public class execution_run
{
    [Key] public Guid id { get; set; }

    public Guid snapshot_id { get; set; }

    public Guid sample_run_id { get; set; }

    public DateTime timestamp_utc { get; set; }

    public string? environment_json { get; set; }

    [StringLength(200)] public string schema_version { get; set; } = null!;

    [InverseProperty("execution_run")]
    public virtual ICollection<execution_result> execution_results { get; set; } = [];

    [ForeignKey("sample_run_id")]
    [InverseProperty("execution_runs")]
    public virtual sample_run sample_run { get; set; } = null!;

    [ForeignKey("snapshot_id")]
    [InverseProperty("execution_runs")]
    public virtual source_snapshot snapshot { get; set; } = null!;
}