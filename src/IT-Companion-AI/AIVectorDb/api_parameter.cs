// Project Name: SKAgent
// File Name: api_parameter.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("api_parameter")]
[Index("api_member_id", Name = "idx_api_parameter_member_id")]
public class api_parameter
{
    [Key] public Guid id { get; set; }

    public Guid api_member_id { get; set; }

    [StringLength(200)] public string? name { get; set; }

    [StringLength(400)] public string? type { get; set; }

    [StringLength(50)] public string? nullable_annotation { get; set; }

    public int? position { get; set; }

    [StringLength(50)] public string? modifier { get; set; }

    public bool? has_default_value { get; set; }

    public string? default_value_literal { get; set; }

    [StringLength(50)] public string? api_version { get; set; }

    [ForeignKey("api_member_id")]
    [InverseProperty("api_parameters")]
    public virtual api_member api_member { get; set; } = null!;
}