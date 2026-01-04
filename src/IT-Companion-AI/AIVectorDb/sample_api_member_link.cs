// Project Name: SKAgent
// File Name: sample_api_member_link.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("sample_api_member_link")]
[Index("sample_id", Name = "idx_sample_api_member_link_sample_id")]
public class sample_api_member_link
{
    [Key] public Guid id { get; set; }

    public Guid sample_id { get; set; }

    [StringLength(200)] public string member_uid { get; set; } = null!;

    [ForeignKey("sample_id")]
    [InverseProperty("sample_api_member_links")]
    public virtual code_sample sample { get; set; } = null!;
}