namespace ITCompanionAI.Models;


public sealed class DocsPage
{
    public string Url { get; set; }
    public string Uid { get; set; }
    public string Title { get; set; }
    public string[] Breadcrumb { get; set; }
    public string Html { get; set; }
    public string Markdown { get; set; }
    public string NormalizedMarkdown { get; set; }
    public string Hash { get; set; }
    public DateTimeOffset LastFetched { get; set; }
}