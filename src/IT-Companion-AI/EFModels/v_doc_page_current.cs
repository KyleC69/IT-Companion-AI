using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Keyless]
public partial class v_doc_page_current
{
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
}
