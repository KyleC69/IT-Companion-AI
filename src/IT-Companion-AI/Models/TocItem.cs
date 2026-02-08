namespace ITCompanionAI.Models;





public sealed class TocItem
{
    public string Uid { get; set; }
    public string Name { get; set; }
    public string Href { get; set; }
    public List<TocItem> Items { get; set; } = [];
}