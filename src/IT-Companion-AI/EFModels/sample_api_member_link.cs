using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("sample_api_member_link")]
[Index("member_uid", Name = "ix_sample_api_member_link_member")]
public partial class sample_api_member_link
{
    [Key]
    public Guid id { get; set; }

    public Guid sample_id { get; set; }

    [StringLength(1000)]
    public string member_uid { get; set; } = null!;

    [ForeignKey("sample_id")]
    [InverseProperty("sample_api_member_links")]
    public virtual sample sample { get; set; } = null!;
}
