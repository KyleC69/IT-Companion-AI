// Project Name: SKAgent
// File Name: feature_doc_link.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("feature_doc_link")]
[Index("feature_id", Name = "idx_feature_doc_link_feature_id")]
public class feature_doc_link
{
    [Key] public Guid id { get; set; }

    public Guid feature_id { get; set; }

    [StringLength(200)] public string doc_uid { get; set; } = null!;

    [StringLength(200)] public string? section_uid { get; set; }

    [ForeignKey("feature_id")]
    [InverseProperty("feature_doc_links")]
    public virtual api_feature feature { get; set; } = null!;
}