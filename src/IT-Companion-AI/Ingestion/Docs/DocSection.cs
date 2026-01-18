namespace ITCompanionAI.Ingestion.Docs;


public sealed class DocSection
{
    public Guid Id { get; set; }
    public Guid DocPageId { get; set; }
    public string SemanticUid { get; set; } = string.Empty;
    public string Heading { get; set; }
    public int? Level { get; set; }
    public string ContentMarkdown { get; set; }
    public int? OrderIndex { get; set; }
    public int VersionNumber { get; set; } = 1;
    public Guid CreatedIngestionRunId { get; set; }
    public Guid? UpdatedIngestionRunId { get; set; }
    public Guid? RemovedIngestionRunId { get; set; }
    public DateTime ValidFromUtc { get; set; }
    public DateTime? ValidToUtc { get; set; }
    public bool IsActive { get; set; } = true;
    public byte[] ContentHash { get; set; }
}