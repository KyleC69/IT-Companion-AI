using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("doc_page")]
[Index("content_hash", Name = "ix_doc_page_content_hash")]
[Index("semantic_uid", "is_active", Name = "ix_doc_page_semantic_active")]
[Index("semantic_uid", "valid_from_utc", Name = "ix_doc_page_semantic_valid_from")]
[Index("semantic_uid", "version_number", Name = "uq_doc_page_semantic_version", IsUnique = true)]
public partial class doc_page
{
    [Key]
    public Guid id { get; set; }

    [StringLength(1000)]
    public string semantic_uid { get; set; } = null!;

    public Guid source_snapshot_id { get; set; }

    public string? source_path { get; set; }

    [StringLength(400)]
    public string? title { get; set; }

    [StringLength(200)]
    public string? language { get; set; }

    public string? url { get; set; }

    public string? raw_markdown { get; set; }

    public int version_number { get; set; }

    public Guid created_ingestion_run_id { get; set; }

    public Guid updated_ingestion_run_id { get; set; }

    public Guid? removed_ingestion_run_id { get; set; }

    public DateTime valid_from_utc { get; set; }

    public DateTime? valid_to_utc { get; set; }

    public bool is_active { get; set; }

    [MaxLength(32)]
    public byte[]? content_hash { get; set; }

    [MaxLength(32)]
    public byte[]? semantic_uid_hash { get; set; }

    [ForeignKey("created_ingestion_run_id")]
    [InverseProperty("doc_pagecreated_ingestion_runs")]
    public virtual ingestion_run created_ingestion_run { get; set; } = null!;

    [InverseProperty("doc_page")]
    public virtual ICollection<doc_section> doc_sections { get; set; } = new List<doc_section>();

    [ForeignKey("removed_ingestion_run_id")]
    [InverseProperty("doc_pageremoved_ingestion_runs")]
    public virtual ingestion_run? removed_ingestion_run { get; set; }

    [ForeignKey("semantic_uid_hash")]
    [InverseProperty("doc_pages")]
    public virtual semantic_identity? semantic_uid_hashNavigation { get; set; }

    [ForeignKey("source_snapshot_id")]
    [InverseProperty("doc_pages")]
    public virtual source_snapshot source_snapshot { get; set; } = null!;

    [ForeignKey("updated_ingestion_run_id")]
    [InverseProperty("doc_pageupdated_ingestion_runs")]
    public virtual ingestion_run updated_ingestion_run { get; set; } = null!;
}
