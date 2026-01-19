namespace ITCompanionAI.Models;


public sealed class DocPage
{
    public Guid Id { get; set; }
    public string SemanticUid { get; set; } = string.Empty;
    public Guid SourceSnapshotId { get; set; }
    public string SourcePath { get; set; }
    public string Title { get; set; }
    public string Language { get; set; }
    public string Url { get; set; } = string.Empty;
    public string RawMarkdown { get; set; }
    public int VersionNumber { get; set; } = 1;
    public Guid CreatedIngestionRunId { get; set; }
    public Guid? UpdatedIngestionRunId { get; set; }
    public Guid? RemovedIngestionRunId { get; set; }
    public DateTime ValidFromUtc { get; set; }
    public DateTime? ValidToUtc { get; set; }
    public bool IsActive { get; set; } = true;
    public byte[] ContentHash { get; set; }
}