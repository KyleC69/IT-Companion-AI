using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("semantic_identity")]
[Index("kind", "uid_hash", Name = "ix_semantic_identity_kind_uid_hash")]
public partial class semantic_identity
{
    [StringLength(1000)]
    public string uid { get; set; } = null!;

    [Key]
    [MaxLength(32)]
    public byte[] uid_hash { get; set; } = null!;

    [StringLength(50)]
    public string kind { get; set; } = null!;

    public DateTime created_utc { get; set; }

    public string? notes { get; set; }

    [InverseProperty("semantic_uid_hashNavigation")]
    public virtual ICollection<api_feature> api_features { get; set; } = new List<api_feature>();

    [InverseProperty("semantic_uid_hashNavigation")]
    public virtual ICollection<api_member> api_members { get; set; } = new List<api_member>();

    [InverseProperty("semantic_uid_hashNavigation")]
    public virtual ICollection<api_type> api_types { get; set; } = new List<api_type>();

    [InverseProperty("semantic_uid_hashNavigation")]
    public virtual ICollection<doc_page> doc_pages { get; set; } = new List<doc_page>();

    [InverseProperty("semantic_uid_hashNavigation")]
    public virtual ICollection<doc_section> doc_sections { get; set; } = new List<doc_section>();
}
