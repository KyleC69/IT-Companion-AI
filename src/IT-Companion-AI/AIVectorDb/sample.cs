// Project Name: SKAgent
// File Name: sample.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("sample")]
[Index("feature_uid", Name = "idx_sample_feature_uid")]
[Index("sample_run_id", Name = "idx_sample_sample_run_id")]
[Index("sample_uid", Name = "uq_sample_uid", IsUnique = true)]
public class code_sample
{
    [Key] public Guid id { get; set; }

    public Guid sample_run_id { get; set; }

    [StringLength(200)] public string sample_uid { get; set; } = null!;

    [StringLength(200)] public string? feature_uid { get; set; }

    [StringLength(200)] public string? language { get; set; }

    public string? code { get; set; }

    [StringLength(400)] public string? entry_point { get; set; }

    [StringLength(200)] public string? target_framework { get; set; }

    public string? package_references { get; set; }

    [StringLength(200)] public string? derived_from_code_uid { get; set; }

    public string? tags { get; set; }

    [StringLength(50)] public string? api_version { get; set; }

    [InverseProperty("sample")]
    public virtual ICollection<sample_api_member_link> sample_api_member_links { get; set; } = [];

    [ForeignKey("sample_run_id")]
    [InverseProperty("samples")]
    public virtual sample_run sample_run { get; set; } = null!;
}