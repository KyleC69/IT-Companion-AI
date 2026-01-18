using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanionAI.Ingestion.Docs;

using ReverseMarkdown;

using HtmlAgilityPack;

public static class HtmlToMarkdown
{
    private static readonly Converter _converter = new Converter(new Config
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
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Remove Learn-specific noise
        RemoveLearnChrome(doc);

        var cleanedHtml = doc.DocumentNode.InnerHtml;

        // Convert to Markdown
        var markdown = _converter.Convert(cleanedHtml);

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
            var nodes = doc.DocumentNode.SelectNodes(sel);
            if (nodes == null) continue;

            foreach (var n in nodes)
            {
                n.Remove();
            }
        }
    }
}