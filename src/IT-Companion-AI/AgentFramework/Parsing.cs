// Project Name: SKAgent
// File Name: Parsing.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers





// ============================================================================
// HTML / MARKDOWN PARSING
// ============================================================================


using HtmlAgilityPack;


namespace ITCompanionAI.AgentFramework;


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

        HtmlDocument doc = new();
        doc.LoadHtml(html);

        StringBuilder sb = new();
        HtmlNode body = doc.DocumentNode.SelectSingleNode("//body") ?? doc.DocumentNode;
        ExtractText(body, sb);
        return sb.ToString();
    }







    public string ParseMarkdown(string markdown)
    {
        // Simple pass-through for now; you can wire a real Markdown parser later if desired.
        return markdown ?? string.Empty;
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

        foreach (HtmlNode child in node.ChildNodes) ExtractText(child, sb);
    }
}