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



        string innerhtml = doc.DocumentNode.InnerHtml;


        Converter converter = new(new Config
        {
            GithubFlavored = true,
            RemoveComments = true,
            SmartHrefHandling = true,
            UnknownTags = Config.UnknownTagsOption.PassThrough
        });



        string markdown = converter.Convert(innerhtml);

        Console.WriteLine(markdown);



        // Normalize with Markdig
        //   return Markdig.Markdown.Normalize(markdown);
        return default;
    }
}