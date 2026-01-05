using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("api_member")]
[Index("content_hash", Name = "ix_api_member_content_hash")]
[Index("semantic_uid_hash", "is_active", Name = "ix_api_member_semantic_active")]
[Index("semantic_uid_hash", "valid_from_utc", Name = "ix_api_member_semantic_valid_from")]
[Index("api_type_id", "member_uid_hash", Name = "ix_api_member_type_hash", IsUnique = true)]
[Index("semantic_uid_hash", "version_number", Name = "uq_api_member_semantic_version", IsUnique = true)]
public partial class api_member
{
    [Key]
    public Guid id { get; set; }

    [StringLength(1000)]
    public string semantic_uid { get; set; } = null!;

    public Guid api_type_id { get; set; }

    [StringLength(400)]
    public string? name { get; set; }

    [StringLength(200)]
    public string? kind { get; set; }

    [StringLength(200)]
    public string? method_kind { get; set; }

    [StringLength(200)]
    public string? accessibility { get; set; }

    public bool? is_static { get; set; }

    public bool? is_extension_method { get; set; }

    public bool? is_async { get; set; }

    public bool? is_virtual { get; set; }

    public bool? is_override { get; set; }

    public bool? is_abstract { get; set; }

    public bool? is_sealed { get; set; }

    public bool? is_readonly { get; set; }

    public bool? is_const { get; set; }

    public bool? is_unsafe { get; set; }

    [StringLength(1000)]
    public string? return_type_uid { get; set; }

    [StringLength(50)]
    public string? return_nullable { get; set; }

    public string? generic_parameters { get; set; }

    public string? generic_constraints { get; set; }

    public string? summary { get; set; }

    public string? remarks { get; set; }

    public string? attributes { get; set; }

    public string? source_file_path { get; set; }

    public int? source_start_line { get; set; }

    public int? source_end_line { get; set; }

    [MaxLength(32)]
    public byte[]? member_uid_hash { get; set; }

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

    [InverseProperty("api_member")]
    public virtual ICollection<api_parameter> api_parameters { get; set; } = new List<api_parameter>();

    [ForeignKey("api_type_id")]
    [InverseProperty("api_members")]
    public virtual api_type api_type { get; set; } = null!;

    [ForeignKey("created_ingestion_run_id")]
    [InverseProperty("api_membercreated_ingestion_runs")]
    public virtual ingestion_run created_ingestion_run { get; set; } = null!;

    [ForeignKey("removed_ingestion_run_id")]
    [InverseProperty("api_memberremoved_ingestion_runs")]
    public virtual ingestion_run? removed_ingestion_run { get; set; }

    [ForeignKey("semantic_uid_hash")]
    [InverseProperty("api_members")]
    public virtual semantic_identity? semantic_uid_hashNavigation { get; set; }

    [ForeignKey("updated_ingestion_run_id")]
    [InverseProperty("api_memberupdated_ingestion_runs")]
    public virtual ingestion_run updated_ingestion_run { get; set; } = null!;
}
