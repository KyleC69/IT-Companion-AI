using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("feature_member_link")]
[Index("feature_id", Name = "ix_feature_member_link_feature")]
[Index("member_uid", Name = "ix_feature_member_link_member")]
public partial class feature_member_link
{
    [Key]
    public Guid id { get; set; }

    public Guid feature_id { get; set; }

    [StringLength(1000)]
    public string member_uid { get; set; } = null!;

    [StringLength(50)]
    public string? role { get; set; }

    [ForeignKey("feature_id")]
    [InverseProperty("feature_member_links")]
    public virtual api_feature feature { get; set; } = null!;
}
