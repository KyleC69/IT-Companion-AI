using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("api_feature")]
[Index("content_hash", Name = "ix_api_feature_content_hash")]
[Index("semantic_uid", "is_active", Name = "ix_api_feature_semantic_active")]
[Index("semantic_uid", "version_number", Name = "uq_api_feature_semantic_version", IsUnique = true)]
public partial class api_feature
{
    [Key]
    public Guid id { get; set; }

    [StringLength(1000)]
    public string semantic_uid { get; set; } = null!;

    public Guid truth_run_id { get; set; }

    [StringLength(400)]
    public string? name { get; set; }

    [StringLength(200)]
    public string? language { get; set; }

    public string? description { get; set; }

    public string? tags { get; set; }

    public int version_number { get; set; }

    public Guid created_ingestion_run_id { get; set; }

    public Guid updated_ingestion_run_id { get; set; }

    public Guid? removed_ingestion_run_id { get; set; }

    public DateTime valid_from_utc { get; set; }

    public DateTime? valid_to_utc { get; set; }

    public bool is_active { get; set; }

    [MaxLength(32)]
    public byte[]? content_hash { get; set; }

    [MaxLength(32)]
    public byte[]? semantic_uid_hash { get; set; }

    [ForeignKey("created_ingestion_run_id")]
    [InverseProperty("api_featurecreated_ingestion_runs")]
    public virtual ingestion_run created_ingestion_run { get; set; } = null!;

    [InverseProperty("feature")]
    public virtual ICollection<feature_doc_link> feature_doc_links { get; set; } = new List<feature_doc_link>();

    [InverseProperty("feature")]
    public virtual ICollection<feature_member_link> feature_member_links { get; set; } = new List<feature_member_link>();

    [InverseProperty("feature")]
    public virtual ICollection<feature_type_link> feature_type_links { get; set; } = new List<feature_type_link>();

    [ForeignKey("removed_ingestion_run_id")]
    [InverseProperty("api_featureremoved_ingestion_runs")]
    public virtual ingestion_run? removed_ingestion_run { get; set; }

    [ForeignKey("semantic_uid_hash")]
    [InverseProperty("api_features")]
    public virtual semantic_identity? semantic_uid_hashNavigation { get; set; }

    [ForeignKey("truth_run_id")]
    [InverseProperty("api_features")]
    public virtual truth_run truth_run { get; set; } = null!;

    [ForeignKey("updated_ingestion_run_id")]
    [InverseProperty("api_featureupdated_ingestion_runs")]
    public virtual ingestion_run updated_ingestion_run { get; set; } = null!;
}
