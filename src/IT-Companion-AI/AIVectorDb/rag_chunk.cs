// Project Name: SKAgent
// File Name: rag_chunk.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("rag_chunk")]
[Index("kind", Name = "idx_rag_chunk_kind")]
[Index("rag_run_id", Name = "idx_rag_chunk_run_id")]
[Index("chunk_uid", Name = "uq_rag_chunk_uid", IsUnique = true)]
public class rag_chunk
{
    [Key] public Guid id { get; set; }

    public Guid rag_run_id { get; set; }

    [StringLength(200)] public string chunk_uid { get; set; } = null!;

    [StringLength(100)] public string? kind { get; set; }

    public string? text { get; set; }

    public string? metadata_json { get; set; }

    [MaxLength(1536)] public SqlVector<float>? embedding_vector { get; set; }

    [ForeignKey("rag_run_id")]
    [InverseProperty("rag_chunks")]
    public virtual rag_run rag_run { get; set; } = null!;
}