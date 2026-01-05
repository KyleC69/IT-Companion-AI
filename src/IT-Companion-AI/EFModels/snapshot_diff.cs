using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("snapshot_diff")]
[Index("old_snapshot_id", "new_snapshot_id", Name = "ix_snapshot_diff_old_new")]
public partial class snapshot_diff
{
    [Key]
    public Guid id { get; set; }

    public Guid old_snapshot_id { get; set; }

    public Guid new_snapshot_id { get; set; }

    public DateTime timestamp_utc { get; set; }

    [StringLength(200)]
    public string schema_version { get; set; } = null!;

    [InverseProperty("snapshot_diff")]
    public virtual ICollection<api_member_diff> api_member_diffs { get; set; } = new List<api_member_diff>();

    [InverseProperty("snapshot_diff")]
    public virtual ICollection<api_type_diff> api_type_diffs { get; set; } = new List<api_type_diff>();

    [InverseProperty("snapshot_diff")]
    public virtual ICollection<doc_page_diff> doc_page_diffs { get; set; } = new List<doc_page_diff>();

    [ForeignKey("new_snapshot_id")]
    [InverseProperty("snapshot_diffnew_snapshots")]
    public virtual source_snapshot new_snapshot { get; set; } = null!;

    [ForeignKey("old_snapshot_id")]
    [InverseProperty("snapshot_diffold_snapshots")]
    public virtual source_snapshot old_snapshot { get; set; } = null!;
}
