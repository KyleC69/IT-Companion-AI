// Project Name: SKAgent
// File Name: doc_section.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("doc_section")]
[Index("doc_page_id", Name = "idx_doc_section_page_id")]
public class doc_section
{
    [Key] public Guid id { get; set; }

    public Guid doc_page_id { get; set; }

    [StringLength(200)] public string section_uid { get; set; } = null!;

    [StringLength(400)] public string? heading { get; set; }

    public int? level { get; set; }

    public string? content_markdown { get; set; }

    public int? order_index { get; set; }

    [InverseProperty("doc_section")] public virtual ICollection<code_block> code_blocks { get; set; } = [];

    [ForeignKey("doc_page_id")]
    [InverseProperty("doc_sections")]
    public virtual doc_page doc_page { get; set; } = null!;
}