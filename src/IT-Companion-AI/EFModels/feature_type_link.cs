using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("feature_type_link")]
[Index("feature_id", Name = "ix_feature_type_link_feature")]
[Index("type_uid", Name = "ix_feature_type_link_type")]
public partial class feature_type_link
{
    [Key]
    public Guid id { get; set; }

    public Guid feature_id { get; set; }

    [StringLength(1000)]
    public string type_uid { get; set; } = null!;

    [StringLength(50)]
    public string? role { get; set; }

    [ForeignKey("feature_id")]
    [InverseProperty("feature_type_links")]
    public virtual api_feature feature { get; set; } = null!;
}
