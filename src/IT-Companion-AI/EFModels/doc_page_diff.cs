using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("doc_page_diff")]
[Index("doc_uid", Name = "ix_doc_page_diff_doc")]
public partial class doc_page_diff
{
    [Key]
    public Guid id { get; set; }

    public Guid snapshot_diff_id { get; set; }

    [StringLength(1000)]
    public string doc_uid { get; set; } = null!;

    [StringLength(200)]
    public string? change_kind { get; set; }

    public string? detail_json { get; set; }

    [ForeignKey("snapshot_diff_id")]
    [InverseProperty("doc_page_diffs")]
    public virtual snapshot_diff snapshot_diff { get; set; } = null!;
}
