using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Keyless]
public partial class v_api_type_current
{
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
}
