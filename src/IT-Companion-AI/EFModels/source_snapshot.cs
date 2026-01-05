using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("source_snapshot")]
[Index("ingestion_run_id", "snapshot_uid", Name = "ix_source_snapshot_ingestion_run")]
[Index("ingestion_run_id", "snapshot_uid_hash", Name = "ix_source_snapshot_ingestion_run_hash")]
[Index("snapshot_uid_hash", Name = "uq_source_snapshot_uid_hash", IsUnique = true)]
public partial class source_snapshot
{
    [Key]
    public Guid id { get; set; }

    public Guid ingestion_run_id { get; set; }

    [StringLength(1000)]
    public string snapshot_uid { get; set; } = null!;

    public string? repo_url { get; set; }

    [StringLength(200)]
    public string? branch { get; set; }

    [StringLength(200)]
    public string? repo_commit { get; set; }

    [StringLength(200)]
    public string? language { get; set; }

    [StringLength(200)]
    public string? package_name { get; set; }

    [StringLength(200)]
    public string? package_version { get; set; }

    public string? config_json { get; set; }

    [MaxLength(32)]
    public byte[]? snapshot_uid_hash { get; set; }

    [InverseProperty("source_snapshot")]
    public virtual ICollection<api_type> api_types { get; set; } = new List<api_type>();

    [InverseProperty("source_snapshot")]
    public virtual ICollection<doc_page> doc_pages { get; set; } = new List<doc_page>();

    [InverseProperty("snapshot")]
    public virtual ICollection<execution_run> execution_runs { get; set; } = new List<execution_run>();

    [ForeignKey("ingestion_run_id")]
    [InverseProperty("source_snapshots")]
    public virtual ingestion_run ingestion_run { get; set; } = null!;

    [InverseProperty("snapshot")]
    public virtual ICollection<rag_run> rag_runs { get; set; } = new List<rag_run>();

    [InverseProperty("snapshot")]
    public virtual ICollection<review_run> review_runs { get; set; } = new List<review_run>();

    [InverseProperty("snapshot")]
    public virtual ICollection<sample_run> sample_runs { get; set; } = new List<sample_run>();

    [InverseProperty("new_snapshot")]
    public virtual ICollection<snapshot_diff> snapshot_diffnew_snapshots { get; set; } = new List<snapshot_diff>();

    [InverseProperty("old_snapshot")]
    public virtual ICollection<snapshot_diff> snapshot_diffold_snapshots { get; set; } = new List<snapshot_diff>();

    [InverseProperty("snapshot")]
    public virtual ICollection<truth_run> truth_runs { get; set; } = new List<truth_run>();
}
