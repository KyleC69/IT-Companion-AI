// Project Name: SKAgent
// File Name: ingestion_run.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ITCompanionAI.AIVectorDb;


[Table("ingestion_run")]
public class ingestion_run
{
    [Key] public Guid id { get; set; }

    public DateTime timestamp_utc { get; set; }

    [StringLength(200)] public string schema_version { get; set; } = null!;

    public string? notes { get; set; }

    [StringLength(50)] public string? api_version { get; set; }

    [InverseProperty("ingestion_run")] public virtual ICollection<source_snapshot> source_snapshots { get; set; } = [];
}