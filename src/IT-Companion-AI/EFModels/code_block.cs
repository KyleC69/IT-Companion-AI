using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("code_block")]
[Index("content_hash", Name = "ix_code_block_content_hash")]
public partial class code_block
{
    [Key]
    public Guid id { get; set; }

    public Guid doc_section_id { get; set; }

    [StringLength(1000)]
    public string? semantic_uid { get; set; }

    [StringLength(200)]
    public string? language { get; set; }

    public string? content { get; set; }

    public string? declared_packages { get; set; }

    public string? tags { get; set; }

    public string? inline_comments { get; set; }

    public int version_number { get; set; }

    public Guid created_ingestion_run_id { get; set; }

    public Guid updated_ingestion_run_id { get; set; }

    public Guid? removed_ingestion_run_id { get; set; }

    public DateTime valid_from_utc { get; set; }

    public DateTime? valid_to_utc { get; set; }

    public bool is_active { get; set; }

    [MaxLength(32)]
    public byte[]? content_hash { get; set; }

    [ForeignKey("created_ingestion_run_id")]
    [InverseProperty("code_blockcreated_ingestion_runs")]
    public virtual ingestion_run created_ingestion_run { get; set; } = null!;

    [ForeignKey("doc_section_id")]
    [InverseProperty("code_blocks")]
    public virtual doc_section doc_section { get; set; } = null!;

    [ForeignKey("removed_ingestion_run_id")]
    [InverseProperty("code_blockremoved_ingestion_runs")]
    public virtual ingestion_run? removed_ingestion_run { get; set; }

    [ForeignKey("updated_ingestion_run_id")]
    [InverseProperty("code_blockupdated_ingestion_runs")]
    public virtual ingestion_run updated_ingestion_run { get; set; } = null!;
}
