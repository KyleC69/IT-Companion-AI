// Project Name: SKAgent
// File Name: doc_page_diff.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("doc_page_diff")]
[Index("snapshot_diff_id", Name = "idx_doc_page_diff_snapshot_id")]
public class doc_page_diff
{
    [Key] public Guid id { get; set; }

    public Guid snapshot_diff_id { get; set; }

    [StringLength(200)] public string doc_uid { get; set; } = null!;

    [StringLength(200)] public string? change_kind { get; set; }

    public string? detail_json { get; set; }

    [ForeignKey("snapshot_diff_id")]
    [InverseProperty("doc_page_diffs")]
    public virtual snapshot_diff snapshot_diff { get; set; } = null!;
}