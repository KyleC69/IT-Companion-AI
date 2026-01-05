using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Keyless]
public partial class v_api_feature_current
{
    public Guid id { get; set; }

    [StringLength(1000)]
    public string semantic_uid { get; set; } = null!;

    public Guid truth_run_id { get; set; }

    [StringLength(400)]
    public string? name { get; set; }

    [StringLength(200)]
    public string? language { get; set; }

    public string? description { get; set; }

    public string? tags { get; set; }

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
