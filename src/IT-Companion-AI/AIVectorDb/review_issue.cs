// Project Name: SKAgent
// File Name: review_issue.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("review_issue")]
[Index("review_item_id", Name = "idx_review_issue_item_id")]
[Index("related_member_uid", Name = "idx_review_issue_related_member_uid")]
public class review_issue
{
    [Key] public Guid id { get; set; }

    public Guid review_item_id { get; set; }

    [StringLength(200)] public string? code { get; set; }

    [StringLength(50)] public string? severity { get; set; }

    [StringLength(200)] public string? related_member_uid { get; set; }

    public string? details { get; set; }

    [ForeignKey("review_item_id")]
    [InverseProperty("review_issues")]
    public virtual review_item review_item { get; set; } = null!;
}