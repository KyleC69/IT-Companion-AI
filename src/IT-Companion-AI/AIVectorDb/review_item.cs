// Project Name: SKAgent
// File Name: review_item.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("review_item")]
[Index("review_run_id", Name = "idx_review_item_run_id")]
[Index("target_uid", Name = "idx_review_item_target_uid")]
public class review_item
{
    [Key] public Guid id { get; set; }

    public Guid review_run_id { get; set; }

    [StringLength(50)] public string target_kind { get; set; } = null!;

    [StringLength(200)] public string target_uid { get; set; } = null!;

    [StringLength(50)] public string? status { get; set; }

    public string? summary { get; set; }

    [InverseProperty("review_item")] public virtual ICollection<review_issue> review_issues { get; set; } = [];

    [ForeignKey("review_run_id")]
    [InverseProperty("review_items")]
    public virtual review_run review_run { get; set; } = null!;
}