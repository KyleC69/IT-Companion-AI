using ITCompanionAI.Ingestion.Docs;




namespace ITCompanionAI.Models;





public sealed class LearnPageParseResult
{
    public DocPage Page { get; set; } = default;
    public List<DocSection> Sections { get; set; } = [];
    public List<CodeBlock> CodeBlocks { get; set; } = [];
}