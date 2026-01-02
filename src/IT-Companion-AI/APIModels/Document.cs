using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class Document
{
    public Guid Id { get; set; }

    public string ExternalId { get; set; } = null!;

    public string Source { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Version { get; set; }

    public string Status { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string? LastError { get; set; }

    public virtual ICollection<Chunk> Chunks { get; set; } = new List<Chunk>();
}
