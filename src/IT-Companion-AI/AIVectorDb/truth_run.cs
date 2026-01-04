// Project Name: SKAgent
// File Name: truth_run.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("truth_run")]
[Index("snapshot_id", Name = "idx_truth_run_snapshot_id")]
public class truth_run
{
    [Key] public Guid id { get; set; }

    public Guid snapshot_id { get; set; }

    public DateTime timestamp_utc { get; set; }

    [StringLength(200)] public string schema_version { get; set; } = null!;

    [InverseProperty("truth_run")] public virtual ICollection<api_feature> features { get; set; } = [];

    [ForeignKey("snapshot_id")]
    [InverseProperty("truth_runs")]
    public virtual source_snapshot snapshot { get; set; } = null!;
}