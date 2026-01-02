using System;
using System.Collections.Generic;
using Microsoft.Data.SqlTypes;

namespace ITCompanionAI;

public partial class ReconciledChunk
{
    public Guid Id { get; set; }

    public string Symbol { get; set; } = null!;

    public string? Namespace { get; set; }

    public string? Version { get; set; }

    public string Summary { get; set; } = null!;

    public SqlVector<float> Embedding { get; set; }

    public double Confidence { get; set; }

    public int SourceCount { get; set; }
}
