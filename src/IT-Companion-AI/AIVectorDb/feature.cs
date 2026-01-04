// Project Name: SKAgent
// File Name: feature.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("api_feature")]
[Index("truth_run_id", Name = "idx_feature_truth_run_id")]
[Index("feature_uid", Name = "uq_feature_uid", IsUnique = true)]
public class api_feature
{
    [Key] public Guid id { get; set; }

    public Guid truth_run_id { get; set; }

    [StringLength(200)] public string feature_uid { get; set; } = null!;

    [StringLength(400)] public string? name { get; set; }

    [StringLength(200)] public string? language { get; set; }

    public string? description { get; set; }

    public string? tags { get; set; }

    [StringLength(200)] public string? introduced_in_version { get; set; }

    [StringLength(200)] public string? last_seen_version { get; set; }

    [StringLength(50)] public string? api_version { get; set; }

    [InverseProperty("feature")] public virtual ICollection<feature_doc_link> feature_doc_links { get; set; } = [];

    [InverseProperty("feature")]
    public virtual ICollection<feature_member_link> feature_member_links { get; set; } = [];

    [InverseProperty("feature")] public virtual ICollection<feature_type_link> feature_type_links { get; set; } = [];

    [ForeignKey("truth_run_id")]
    [InverseProperty("features")]
    public virtual truth_run truth_run { get; set; } = null!;
}