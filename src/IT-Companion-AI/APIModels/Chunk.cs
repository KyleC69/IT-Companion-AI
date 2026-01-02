using System;
using System.Collections.Generic;
using Microsoft.Data.SqlTypes;

namespace ITCompanionAI;

public partial class Chunk
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public int ChunkIndex { get; set; }

    public string Text { get; set; } = null!;

    public int TokenCount { get; set; }

    public SqlVector<float> Embedding { get; set; }

    public string? Section { get; set; }

    public string? Symbol { get; set; }

    public string? Kind { get; set; }

    public bool Verified { get; set; }

    public double Confidence { get; set; }

    public bool Deprecated { get; set; }

    public virtual Document Document { get; set; } = null!;
}
