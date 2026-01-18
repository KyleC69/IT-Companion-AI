using HtmlAgilityPack;





public sealed class ContentExtractor
{
    public string ExtractMainHtml(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);

        // Learn pages: main content is under <main>
        HtmlNode main = doc.DocumentNode.SelectSingleNode("//main");
        if (main == null)
            return string.Empty;

        // Optionally strip nav, feedback, etc. here if needed
        return main.InnerHtml;
    }








    public string ExtractText(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);
        return doc.DocumentNode.InnerText;
    }
}