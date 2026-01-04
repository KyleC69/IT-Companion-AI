// Project Name: SKAgent
// File Name: snapshot_diff.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("snapshot_diff")]
[Index("new_snapshot_id", Name = "idx_snapshot_diff_new_snapshot_id")]
[Index("old_snapshot_id", Name = "idx_snapshot_diff_old_snapshot_id")]
public class snapshot_diff
{
    [Key] public Guid id { get; set; }

    public Guid old_snapshot_id { get; set; }

    public Guid new_snapshot_id { get; set; }

    public DateTime timestamp_utc { get; set; }

    [StringLength(200)] public string schema_version { get; set; } = null!;

    [InverseProperty("snapshot_diff")] public virtual ICollection<api_member_diff> api_member_diffs { get; set; } = [];

    [InverseProperty("snapshot_diff")] public virtual ICollection<api_type_diff> api_type_diffs { get; set; } = [];

    [InverseProperty("snapshot_diff")] public virtual ICollection<doc_page_diff> doc_page_diffs { get; set; } = [];

    [ForeignKey("new_snapshot_id")]
    [InverseProperty("snapshot_diffnew_snapshots")]
    public virtual source_snapshot new_snapshot { get; set; } = null!;

    [ForeignKey("old_snapshot_id")]
    [InverseProperty("snapshot_diffold_snapshots")]
    public virtual source_snapshot old_snapshot { get; set; } = null!;
}