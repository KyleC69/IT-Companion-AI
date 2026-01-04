// Project Name: SKAgent
// File Name: api_type.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("api_type")]
[Index("source_snapshot_id", Name = "idx_api_type_snapshot_id")]
public class api_type
{
    [Key] public Guid id { get; set; }

    public Guid source_snapshot_id { get; set; }

    public string type_uid { get; set; } = null!;

    [StringLength(400)] public string? name { get; set; }

    [Column("namespace")]
    [StringLength(400)]
    public string? _namespace { get; set; }

    [StringLength(200)] public string? kind { get; set; }

    [StringLength(200)] public string? accessibility { get; set; }

    public bool? is_static { get; set; }

    public bool? is_generic { get; set; }

    public bool? is_abstract { get; set; }

    public bool? is_sealed { get; set; }

    public bool? is_record { get; set; }

    public bool? is_ref_like { get; set; }

    [StringLength(400)] public string? base_type { get; set; }

    public string? interfaces { get; set; }

    [StringLength(400)] public string? containing_type_uid { get; set; }

    public string? generic_parameters { get; set; }

    public string? generic_constraints { get; set; }

    public string? summary { get; set; }

    public string? remarks { get; set; }

    public string? attributes { get; set; }

    public string? source_file_path { get; set; }

    public int? source_start_line { get; set; }

    public int? source_end_line { get; set; }

    [StringLength(50)] public string? api_version { get; set; }

    [StringLength(64)] [Unicode(false)] public string? type_uid_hash { get; set; }

    [InverseProperty("api_type")] public virtual ICollection<api_member> api_members { get; set; } = [];

    [ForeignKey("source_snapshot_id")]
    [InverseProperty("api_types")]
    public virtual source_snapshot source_snapshot { get; set; } = null!;
}