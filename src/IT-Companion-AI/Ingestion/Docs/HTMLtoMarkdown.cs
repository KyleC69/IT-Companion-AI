using HtmlAgilityPack;

using ReverseMarkdown;



namespace ITCompanionAI.Ingestion.Docs;


public static class HtmlToMarkdown
{
    private static readonly Converter _converter = new(new Config
    {
        GithubFlavored = true,
        RemoveComments = true,
        SmartHrefHandling = true
    });








    public static string Convert(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        // Clean HTML with HtmlAgilityPack first
        HtmlDocument doc = new();
        doc.LoadHtml(html);

        // Remove Learn-specific noise
        RemoveLearnChrome(doc);

        var innerhtml = doc.DocumentNode.InnerHtml;

        // Convert to Markdown
        var cleanedHtml = Cleaner.PreTidy(innerhtml, false);
        var markdown = Convert(cleanedHtml);

        // Normalize with Markdig
        return Markdig.Markdown.Normalize(markdown);
    }








    private static void RemoveLearnChrome(HtmlDocument doc)
    {
        // Remove nav, footer, ads, sidebars, etc.
        var selectors = new[]
        {
            "//nav",
            "//footer",
            "//*[@id='left-nav']",
            "//*[@id='right-rail']",
            "//*[@class='feedback-container']",
            "//*[@class='rating-container']"
        };

        foreach (var sel in selectors)
        {
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(sel);
            if (nodes == null)
            {
                continue;
            }

            foreach (HtmlNode n in nodes)
            {
                n.Remove();
            }
        }
    }
}