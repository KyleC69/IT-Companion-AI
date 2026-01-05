using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("api_type_diff")]
[Index("type_uid", Name = "ix_api_type_diff_type")]
public partial class api_type_diff
{
    [Key]
    public Guid id { get; set; }

    public Guid snapshot_diff_id { get; set; }

    [StringLength(1000)]
    public string type_uid { get; set; } = null!;

    [StringLength(200)]
    public string? change_kind { get; set; }

    public string? detail_json { get; set; }

    [ForeignKey("snapshot_diff_id")]
    [InverseProperty("api_type_diffs")]
    public virtual snapshot_diff snapshot_diff { get; set; } = null!;
}
