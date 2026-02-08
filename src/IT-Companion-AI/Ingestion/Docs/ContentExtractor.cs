using HtmlAgilityPack;

using ReverseMarkdown;





//Class of helpers to extract and clean content from various types of documents and output in various string formats (HTML, plain text, markdown)
public sealed class ContentExtractor
{


    //Decision switch the various extraction modes and document types
    public string ExtractContent(string html, ExtractionMode mode, DocumentType docType)
    {
        //Extract contents based on mode AND document type. IT MUST BE BOTH MODE FIRST AND DOC TYPE SECOND.
        switch (mode)
        {
            case ExtractionMode.Html:
                return docType switch
                {
                        DocumentType.LearnPage => ExtractLearnMainHtml(html),
                        DocumentType.BlogPost => ExtractBlogHtml(html),
                        //Add more document types as needed
                        _ => throw new NotImplementedException($"HTML extraction not implemented for document type {docType}")
                };
            case ExtractionMode.Text:
                return ExtractText(html);
            case ExtractionMode.Markdown:
                return docType switch
                {
                        DocumentType.LearnPage => ExtractLearnMainMarkdown(),
                        //Add more document types as needed
                        _ => throw new NotImplementedException($"Markdown extraction not implemented for document type {docType}")
                };
            case ExtractionMode.Code:
            {
                return docType switch
                {
                        DocumentType.LearnPage => ExtractCodeFromLearnPage(html),
                        //Add more document types as needed
                        _ => throw new NotImplementedException($"Code extraction not implemented for document type {docType}")
                };
            }
            default:
                throw new NotImplementedException($"Extraction mode {mode} not implemented");
        }
    }








    //Extracts the main HTML content from a Learn page - Should be under the <main> tag
    public string ExtractLearnMainHtml(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);

        // Learn pages: main content is under <main>
        HtmlNode main = doc.DocumentNode.SelectSingleNode("//main");
        if (HtmlNode.IsEmptyElement(nameof(main)))
        {
            return string.Empty;
        }

        var cleaned = Cleaner.PreTidy(main.InnerHtml, false);


        // Optionally strip nav, feedback, etc. here if needed
        return cleaned;
    }








    private string ExtractCodeFromLearnPage(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);
        string[] codeBlocks = [];
        // Learn pages: code blocks are typically within <pre><code> or just <code> tags inside the main content
        HtmlNodeCollection codeNodes = doc.DocumentNode.SelectNodes("//pre//code | //code");

        Converter markdownConverter = new();
        foreach (HtmlNode node in codeNodes)
        {
            var codeText = node.InnerText;
            var markdownCodeBlock = $"```\n{codeText}\n```";
            node.InnerHtml = markdownCodeBlock;
        }

        var markdownContent = string.Join("\n", codeNodes.Select(node => node.InnerText));
        var result = markdownConverter.Convert(markdownContent);

        return result;
    }








    public string ExtractLearnHtml(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);
        // Learn pages: main content is under <main>
        HtmlNode main = doc.DocumentNode.SelectSingleNode("//main");
        return main == null ? string.Empty : main.InnerHtml;
    }








    public string ExtractLearnMainMarkdown()
    {
        throw new NotImplementedException();
    }








    private string ExtractBlogHtml(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);
        // Blog posts: main content is often under <article>
        HtmlNode article = doc.DocumentNode.SelectSingleNode("//article");
        return article == null ? string.Empty : article.InnerHtml;
    }








    public string ExtractText(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);
        return doc.DocumentNode.InnerText;
    }
}





public enum ExtractionMode
{
    Html,
    Text,
    Markdown,
    Code
}





public enum DocumentType
{
    LearnPage,
    BlogPost,
    FileSystem,
    SourceCode,
    Other
}