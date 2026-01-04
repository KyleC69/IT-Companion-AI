// Project Name: SKAgent
// File Name: doc_page.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("doc_page")]
[Index("source_snapshot_id", Name = "idx_doc_page_snapshot_id")]
[Index("source_snapshot_id", "doc_uid", Name = "uq_doc_page_uid_per_snapshot", IsUnique = true)]
public class doc_page
{
    [Key] public Guid id { get; set; }

    public Guid source_snapshot_id { get; set; }

    [StringLength(200)] public string doc_uid { get; set; } = null!;

    public string? source_path { get; set; }

    [StringLength(400)] public string? title { get; set; }

    [StringLength(200)] public string? language { get; set; }

    public string? url { get; set; }

    public string? raw_markdown { get; set; }

    [StringLength(50)] public string? api_version { get; set; }

    [InverseProperty("doc_page")] public virtual ICollection<doc_section> doc_sections { get; set; } = [];

    [ForeignKey("source_snapshot_id")]
    [InverseProperty("doc_pages")]
    public virtual source_snapshot source_snapshot { get; set; } = null!;
}