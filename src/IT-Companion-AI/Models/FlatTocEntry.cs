namespace ITCompanionAI.Models;





public sealed class FlatTocEntry
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string Uid { get; set; }
    public int Depth { get; set; }
    public string[] Breadcrumb { get; set; }
}