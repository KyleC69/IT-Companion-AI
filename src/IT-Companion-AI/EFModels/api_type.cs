using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("api_type")]
[Index("content_hash", Name = "ix_api_type_content_hash")]
[Index("semantic_uid_hash", "is_active", Name = "ix_api_type_semantic_active")]
[Index("semantic_uid_hash", "valid_from_utc", Name = "ix_api_type_semantic_valid_from")]
[Index("semantic_uid_hash", "version_number", Name = "uq_api_type_semantic_version", IsUnique = true)]
public partial class api_type
{
    [Key]
    public Guid id { get; set; }

    [StringLength(1000)]
    public string semantic_uid { get; set; } = null!;

    public Guid source_snapshot_id { get; set; }

    [StringLength(400)]
    public string? name { get; set; }

    [StringLength(1000)]
    public string? namespace_path { get; set; }

    [StringLength(200)]
    public string? kind { get; set; }

    [StringLength(200)]
    public string? accessibility { get; set; }

    public bool? is_static { get; set; }

    public bool? is_generic { get; set; }

    public bool? is_abstract { get; set; }

    public bool? is_sealed { get; set; }

    public bool? is_record { get; set; }

    public bool? is_ref_like { get; set; }

    [StringLength(1000)]
    public string? base_type_uid { get; set; }

    public string? interfaces { get; set; }

    [StringLength(1000)]
    public string? containing_type_uid { get; set; }

    public string? generic_parameters { get; set; }

    public string? generic_constraints { get; set; }

    public string? summary { get; set; }

    public string? remarks { get; set; }

    public string? attributes { get; set; }

    public string? source_file_path { get; set; }

    public int? source_start_line { get; set; }

    public int? source_end_line { get; set; }

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

    [InverseProperty("api_type")]
    public virtual ICollection<api_member> api_members { get; set; } = new List<api_member>();

    [ForeignKey("created_ingestion_run_id")]
    [InverseProperty("api_typecreated_ingestion_runs")]
    public virtual ingestion_run created_ingestion_run { get; set; } = null!;

    [ForeignKey("removed_ingestion_run_id")]
    [InverseProperty("api_typeremoved_ingestion_runs")]
    public virtual ingestion_run? removed_ingestion_run { get; set; }

    [ForeignKey("semantic_uid_hash")]
    [InverseProperty("api_types")]
    public virtual semantic_identity? semantic_uid_hashNavigation { get; set; }

    [ForeignKey("source_snapshot_id")]
    [InverseProperty("api_types")]
    public virtual source_snapshot source_snapshot { get; set; } = null!;

    [ForeignKey("updated_ingestion_run_id")]
    [InverseProperty("api_typeupdated_ingestion_runs")]
    public virtual ingestion_run updated_ingestion_run { get; set; } = null!;
}
