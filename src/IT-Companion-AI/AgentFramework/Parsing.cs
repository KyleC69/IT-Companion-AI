using System;
using System.Collections.Generic;
using System.Text;
    using HtmlAgilityPack;

// ============================================================================
// HTML / MARKDOWN PARSING
// ============================================================================

namespace SkKnowledgeBase.Parsing;


public interface IContentParser
{
    string ParseHtml(string html);
    string ParseMarkdown(string markdown);
}

public sealed class HtmlMarkdownContentParser : IContentParser
{
    public string ParseHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var sb = new StringBuilder();
        var body = doc.DocumentNode.SelectSingleNode("//body") ?? doc.DocumentNode;
        ExtractText(body, sb);
        return sb.ToString();
    }

    private static void ExtractText(HtmlNode node, StringBuilder sb)
    {
        if (node.NodeType == HtmlNodeType.Text)
        {
            var text = node.InnerText;
            if (!string.IsNullOrWhiteSpace(text))
            {
                sb.AppendLine(text.Trim());
            }
        }

        foreach (var child in node.ChildNodes)
        {
            ExtractText(child, sb);
        }
    }

    public string ParseMarkdown(string markdown)
    {
        // Simple pass-through for now; you can wire a real Markdown parser later if desired.
        return markdown ?? string.Empty;
    }
}
