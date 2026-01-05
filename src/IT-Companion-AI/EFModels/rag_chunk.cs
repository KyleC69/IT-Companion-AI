using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("rag_chunk")]
[Index("kind", Name = "ix_rag_chunk_kind")]
[Index("chunk_uid", Name = "uq_rag_chunk_uid", IsUnique = true)]
public partial class rag_chunk
{
    [Key]
    public Guid id { get; set; }

    public Guid rag_run_id { get; set; }

    [StringLength(1000)]
    public string chunk_uid { get; set; } = null!;

    [StringLength(100)]
    public string? kind { get; set; }

    public string? text { get; set; }

    public string? metadata_json { get; set; }

    [StringLength(1536)]
    [Unicode(false)]
    public string? embedding_vector { get; set; }

    [ForeignKey("rag_run_id")]
    [InverseProperty("rag_chunks")]
    public virtual rag_run rag_run { get; set; } = null!;
}
