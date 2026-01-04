// Project Name: SKAgent
// File Name: api_member.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("api_member")]
[Index("api_type_id", Name = "idx_api_member_api_type_id")]
[Index("api_type_id", "member_uid_hash", Name = "uq_api_member_uid_hash_per_type", IsUnique = true)]
public class api_member
{
    [Key] public Guid id { get; set; }

    public Guid api_type_id { get; set; }

    public string member_uid { get; set; } = null!;

    [StringLength(400)] public string? name { get; set; }

    [StringLength(200)] public string? kind { get; set; }

    [StringLength(200)] public string? method_kind { get; set; }

    [StringLength(200)] public string? accessibility { get; set; }

    public bool? is_static { get; set; }

    public bool? is_extension_method { get; set; }

    public bool? is_async { get; set; }

    public bool? is_virtual { get; set; }

    public bool? is_override { get; set; }

    public bool? is_abstract { get; set; }

    public bool? is_sealed { get; set; }

    public bool? is_readonly { get; set; }

    public bool? is_const { get; set; }

    public bool? is_unsafe { get; set; }

    [StringLength(400)] public string? return_type { get; set; }

    [StringLength(50)] public string? return_nullable { get; set; }

    public string? generic_parameters { get; set; }

    public string? generic_constraints { get; set; }

    public string? summary { get; set; }

    public string? remarks { get; set; }

    public string? attributes { get; set; }

    public string? source_file_path { get; set; }

    public int? source_start_line { get; set; }

    public int? source_end_line { get; set; }

    [StringLength(64)] [Unicode(false)] public string? member_uid_hash { get; set; }

    [StringLength(50)] public string? api_version { get; set; }

    [InverseProperty("api_member")]
    public virtual ICollection<api_member_doc_link> api_member_doc_links { get; set; } = [];

    [InverseProperty("api_member")] public virtual ICollection<api_parameter> api_parameters { get; set; } = [];

    [ForeignKey("api_type_id")]
    [InverseProperty("api_members")]
    public virtual api_type api_type { get; set; } = null!;
}