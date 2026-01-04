// Project Name: SKAgent
// File Name: api_member_doc_link.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("api_member_doc_link")]
[Index("api_member_id", Name = "idx_api_member_doc_link_member_id")]
public class api_member_doc_link
{
    [Key] public Guid id { get; set; }

    public Guid api_member_id { get; set; }

    [StringLength(200)] public string doc_uid { get; set; } = null!;

    [StringLength(200)] public string? section_uid { get; set; }

    [ForeignKey("api_member_id")]
    [InverseProperty("api_member_doc_links")]
    public virtual api_member api_member { get; set; } = null!;
}