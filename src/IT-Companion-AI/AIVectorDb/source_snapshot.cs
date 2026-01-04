// Project Name: SKAgent
// File Name: source_snapshot.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("source_snapshot")]
[Index("ingestion_run_id", Name = "idx_source_snapshot_ingestion_run_id")]
[Index("snapshot_uid", Name = "uq_source_snapshot_uid", IsUnique = true)]
public class source_snapshot
{
    [Key] public Guid id { get; set; }

    public Guid ingestion_run_id { get; set; }

    [StringLength(200)] public string snapshot_uid { get; set; } = null!;

    public string? repo_url { get; set; }

    [StringLength(200)] public string? branch { get; set; }

    [StringLength(200)] public string? repocommit { get; set; }

    [StringLength(200)] public string? language { get; set; }

    [StringLength(200)] public string? package_name { get; set; }

    [StringLength(200)] public string? package_version { get; set; }

    public string? config_json { get; set; }

    [StringLength(50)] public string? api_version { get; set; }

    [InverseProperty("source_snapshot")] public virtual ICollection<api_type> api_types { get; set; } = [];

    [InverseProperty("source_snapshot")] public virtual ICollection<doc_page> doc_pages { get; set; } = [];

    [InverseProperty("snapshot")] public virtual ICollection<execution_run> execution_runs { get; set; } = [];

    [ForeignKey("ingestion_run_id")]
    [InverseProperty("source_snapshots")]
    public virtual ingestion_run ingestion_run { get; set; } = null!;

    [InverseProperty("snapshot")] public virtual ICollection<rag_run> rag_runs { get; set; } = [];

    [InverseProperty("snapshot")] public virtual ICollection<review_run> review_runs { get; set; } = [];

    [InverseProperty("snapshot")] public virtual ICollection<sample_run> sample_runs { get; set; } = [];

    [InverseProperty("new_snapshot")]
    public virtual ICollection<snapshot_diff> snapshot_diffnew_snapshots { get; set; } = [];

    [InverseProperty("old_snapshot")]
    public virtual ICollection<snapshot_diff> snapshot_diffold_snapshots { get; set; } = [];

    [InverseProperty("snapshot")] public virtual ICollection<truth_run> truth_runs { get; set; } = [];
}