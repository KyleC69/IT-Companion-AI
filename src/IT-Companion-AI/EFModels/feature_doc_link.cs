using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("feature_doc_link")]
[Index("doc_uid", "section_uid", Name = "ix_feature_doc_link_doc")]
[Index("feature_id", Name = "ix_feature_doc_link_feature")]
public partial class feature_doc_link
{
    [Key]
    public Guid id { get; set; }

    public Guid feature_id { get; set; }

    [StringLength(1000)]
    public string doc_uid { get; set; } = null!;

    [StringLength(1000)]
    public string? section_uid { get; set; }

    [ForeignKey("feature_id")]
    [InverseProperty("feature_doc_links")]
    public virtual api_feature feature { get; set; } = null!;
}
