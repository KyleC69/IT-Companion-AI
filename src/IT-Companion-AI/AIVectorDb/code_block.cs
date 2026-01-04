// Project Name: SKAgent
// File Name: code_block.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("code_block")]
[Index("doc_section_id", Name = "idx_code_block_section_id")]
public class code_block
{
    [Key] public Guid id { get; set; }

    public Guid doc_section_id { get; set; }

    [StringLength(200)] public string? code_uid { get; set; }

    [StringLength(200)] public string? language { get; set; }

    public string? content { get; set; }

    public string? declared_packages { get; set; }

    public string? tags { get; set; }

    public string? inline_comments { get; set; }

    [StringLength(50)] public string? api_version { get; set; }

    [ForeignKey("doc_section_id")]
    [InverseProperty("code_blocks")]
    public virtual doc_section doc_section { get; set; } = null!;
}