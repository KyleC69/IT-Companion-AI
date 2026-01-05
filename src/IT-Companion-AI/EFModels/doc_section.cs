using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("doc_section")]
[Index("content_hash", Name = "ix_doc_section_content_hash")]
[Index("doc_page_id", "order_index", Name = "ix_doc_section_doc_page_order")]
[Index("semantic_uid", "is_active", Name = "ix_doc_section_semantic_active")]
[Index("semantic_uid", "version_number", Name = "uq_doc_section_semantic_version", IsUnique = true)]
public partial class doc_section
{
    [Key]
    public Guid id { get; set; }

    public Guid doc_page_id { get; set; }

    [StringLength(1000)]
    public string semantic_uid { get; set; } = null!;

    [StringLength(400)]
    public string? heading { get; set; }

    public int? level { get; set; }

    public string? content_markdown { get; set; }

    public int? order_index { get; set; }

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

    [InverseProperty("doc_section")]
    public virtual ICollection<code_block> code_blocks { get; set; } = new List<code_block>();

    [ForeignKey("created_ingestion_run_id")]
    [InverseProperty("doc_sectioncreated_ingestion_runs")]
    public virtual ingestion_run created_ingestion_run { get; set; } = null!;

    [ForeignKey("doc_page_id")]
    [InverseProperty("doc_sections")]
    public virtual doc_page doc_page { get; set; } = null!;

    [ForeignKey("removed_ingestion_run_id")]
    [InverseProperty("doc_sectionremoved_ingestion_runs")]
    public virtual ingestion_run? removed_ingestion_run { get; set; }

    [ForeignKey("semantic_uid_hash")]
    [InverseProperty("doc_sections")]
    public virtual semantic_identity? semantic_uid_hashNavigation { get; set; }

    [ForeignKey("updated_ingestion_run_id")]
    [InverseProperty("doc_sectionupdated_ingestion_runs")]
    public virtual ingestion_run updated_ingestion_run { get; set; } = null!;
}
