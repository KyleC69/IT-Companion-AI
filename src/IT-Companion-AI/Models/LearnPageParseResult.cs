namespace ITCompanionAI.Models;





public sealed class LearnPageParseResult
{
    public DocPage Page { get; set; } = new();
    public List<DocSection> Sections { get; set; } = new();
    public List<CodeBlock> CodeBlocks { get; set; } = new();
}