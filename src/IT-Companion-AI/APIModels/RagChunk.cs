using System;
using System.Collections.Generic;
using Microsoft.Data.SqlTypes;

namespace ITCompanionAI;

public partial class RagChunk
{
    public Guid Id { get; set; }

    public Guid RagRunId { get; set; }

    public string ChunkUid { get; set; } = null!;

    public string? Kind { get; set; }

    public string? Text { get; set; }

    public string? MetadataJson { get; set; }

    public SqlVector<float>? EmbeddingVector { get; set; }

    public virtual RagRun RagRun { get; set; } = null!;
}
