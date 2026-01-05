using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("api_member_diff")]
[Index("member_uid", Name = "ix_api_member_diff_member")]
public partial class api_member_diff
{
    [Key]
    public Guid id { get; set; }

    public Guid snapshot_diff_id { get; set; }

    [StringLength(1000)]
    public string member_uid { get; set; } = null!;

    [StringLength(200)]
    public string? change_kind { get; set; }

    public string? old_signature { get; set; }

    public string? new_signature { get; set; }

    public bool? breaking { get; set; }

    public string? detail_json { get; set; }

    [ForeignKey("snapshot_diff_id")]
    [InverseProperty("api_member_diffs")]
    public virtual snapshot_diff snapshot_diff { get; set; } = null!;
}
