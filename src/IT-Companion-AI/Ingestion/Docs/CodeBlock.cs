namespace ITCompanionAI.Ingestion.Docs;


public sealed class CodeBlock
{
    public Guid Id { get; set; }
    public Guid DocSectionId { get; set; }
    public string SemanticUid { get; set; }
    public string Language { get; set; }
    public string Content { get; set; }
    public string DeclaredPackages { get; set; }
    public string Tags { get; set; }
    public string InlineComments { get; set; }
    public int VersionNumber { get; set; } = 1;
    public Guid CreatedIngestionRunId { get; set; }
    public Guid? UpdatedIngestionRunId { get; set; }
    public Guid? RemovedIngestionRunId { get; set; }
    public DateTime ValidFromUtc { get; set; }
    public DateTime? ValidToUtc { get; set; }
    public bool IsActive { get; set; } = true;
    public byte[] ContentHash { get; set; }
}