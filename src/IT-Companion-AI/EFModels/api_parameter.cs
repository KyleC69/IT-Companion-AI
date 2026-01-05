using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("api_parameter")]
[Index("content_hash", Name = "ix_api_parameter_content_hash")]
[Index("api_member_id", "position", "is_active", Name = "ix_api_parameter_member_position_active")]
[Index("api_member_id", "position", "version_number", Name = "uq_api_parameter_member_position_version", IsUnique = true)]
public partial class api_parameter
{
    [Key]
    public Guid id { get; set; }

    public Guid api_member_id { get; set; }

    [StringLength(200)]
    public string? name { get; set; }

    [StringLength(1000)]
    public string? type_uid { get; set; }

    [StringLength(50)]
    public string? nullable_annotation { get; set; }

    public int? position { get; set; }

    [StringLength(50)]
    public string? modifier { get; set; }

    public bool? has_default_value { get; set; }

    public string? default_value_literal { get; set; }

    public int version_number { get; set; }

    public Guid created_ingestion_run_id { get; set; }

    public Guid updated_ingestion_run_id { get; set; }

    public Guid? removed_ingestion_run_id { get; set; }

    public DateTime valid_from_utc { get; set; }

    public DateTime? valid_to_utc { get; set; }

    public bool is_active { get; set; }

    [MaxLength(32)]
    public byte[]? content_hash { get; set; }

    [ForeignKey("api_member_id")]
    [InverseProperty("api_parameters")]
    public virtual api_member api_member { get; set; } = null!;

    [ForeignKey("created_ingestion_run_id")]
    [InverseProperty("api_parametercreated_ingestion_runs")]
    public virtual ingestion_run created_ingestion_run { get; set; } = null!;

    [ForeignKey("removed_ingestion_run_id")]
    [InverseProperty("api_parameterremoved_ingestion_runs")]
    public virtual ingestion_run? removed_ingestion_run { get; set; }

    [ForeignKey("updated_ingestion_run_id")]
    [InverseProperty("api_parameterupdated_ingestion_runs")]
    public virtual ingestion_run updated_ingestion_run { get; set; } = null!;
}
