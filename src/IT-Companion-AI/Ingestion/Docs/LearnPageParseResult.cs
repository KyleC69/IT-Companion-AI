namespace ITCompanionAI.Ingestion.Docs;


public sealed class LearnPageParseResult
{
    public DocPage Page { get; set; } = default!;
    public List<DocSection> Sections { get; set; } = new();
    public List<CodeBlock> CodeBlocks { get; set; } = new();
}