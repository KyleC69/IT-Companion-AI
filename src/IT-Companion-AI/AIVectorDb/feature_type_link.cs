// Project Name: SKAgent
// File Name: feature_type_link.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("feature_type_link")]
[Index("feature_id", Name = "idx_feature_type_link_feature_id")]
public class feature_type_link
{
    [Key] public Guid id { get; set; }

    public Guid feature_id { get; set; }

    [StringLength(200)] public string type_uid { get; set; } = null!;

    [StringLength(50)] public string? role { get; set; }

    [ForeignKey("feature_id")]
    [InverseProperty("feature_type_links")]
    public virtual api_feature feature { get; set; } = null!;
}