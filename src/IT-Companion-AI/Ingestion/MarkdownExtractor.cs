#nullable enable

using Markdig;
using Markdig.Extensions.CustomContainers;
using Markdig.Extensions.Tables;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;





public static class MarkdownExtractor
{

    public static List<ExtractedItem> Extract(string markdown)
    {


        var results = new List<ExtractedItem>();
        var index = 0;

        // --- 1. YAML front matter ---
        // ---------------------------------------------------------
        // 1. MANUAL YAML FRONT-MATTER EXTRACTION
        // ---------------------------------------------------------
        string? yamlBlock = null;

        if (markdown.StartsWith("---"))
        {
            var end = markdown.IndexOf("\n---", 3, StringComparison.Ordinal);
            if (end > -1)
            {
                yamlBlock = markdown.Substring(0, end + 4).Trim();
                markdown = markdown.Substring(end + 4).TrimStart();

                results.Add(new ExtractedItem
                {
                        Index = index++,
                        Kind = "DocPageMeta",
                        Content = yamlBlock
                });
            }
        }





        // --- 2. Section extraction ---
        MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
                .UseCustomContainers()
                .UsePipeTables()
                .Build();

        MarkdownDocument doc = Markdown.Parse(markdown, pipeline);

        HeadingBlock? currentHeading = null;
        var sectionBuffer = new List<string>();



        void FlushSection()
        {
            if (currentHeading == null || sectionBuffer.Count == 0)
            {
                return;
            }

            results.Add(new ExtractedItem
            {
                    Index = index++,
                    Kind = "Section",
                    Heading = ExtractInlineText(currentHeading.Inline),
                    HeadingLevel = currentHeading.Level,
                    Content = string.Join("\n", sectionBuffer)
            });

            sectionBuffer.Clear();
        }



        foreach (Block node in doc)
            switch (node)
            {
                case YamlFrontMatterBlock:
                    continue;

                case HeadingBlock heading:
                    FlushSection();
                    currentHeading = heading;
                    continue;

                // --- Learn-style :::code blocks ---
                case CustomContainer container
                        when container.Info?.StartsWith("code") == true:
                {
                    FlushSection();

                    results.Add(new ExtractedItem
                    {
                            Index = index++,
                            Kind = "CodeBlock",
                            Content = ExtractCodeDirective(container.Info)
                    });

                    continue;
                }

                // --- Ignore :::image blocks ---
                case CustomContainer container
                        when container.Info?.StartsWith("image") == true:
                    continue;

                // --- Other custom containers treated as text blocks ---
                case CustomContainer container:
                {
                    FlushSection();

                    results.Add(new ExtractedItem
                    {
                            Index = index++,
                            Kind = "TextBlock",
                            Content = ExtractContainerText(container)
                    });

                    continue;
                }

                case FencedCodeBlock code:
                    FlushSection();
                    results.Add(new ExtractedItem
                    {
                            Index = index++,
                            Kind = "CodeBlock",
                            Content = code.Lines.ToString()
                    });
                    continue;

                case Table table:
                    FlushSection();
                    results.Add(new ExtractedItem
                    {
                            Index = index++,
                            Kind = "Table",
                            Content = ExtractTable(table)
                    });
                    continue;

                default:
                    if (currentHeading != null)
                    {
                        var text = ExtractBlockText(node);
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            sectionBuffer.Add(text);
                        }
                    }

                    break;
            }

        FlushSection();
        return results;
    }








    // ---------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------








    private static string ExtractInlineText(ContainerInline? inline)
    {
        if (inline == null)
        {
            return "";
        }

        var parts = new List<string>();
        foreach (Inline child in inline)
            switch (child)
            {
                case LiteralInline lit:
                    parts.Add(lit.Content.ToString());
                    break;

                case LinkInline link:
                    parts.Add(ExtractInlineText(link)); // flatten
                    break;
            }

        return string.Join("", parts);
    }








    private static string ExtractBlockText(Block block)
    {
        switch (block)
        {
            case ParagraphBlock p:
                return ExtractInlineText(p.Inline);

            case ListBlock list:
                var items = new List<string>();
                foreach (ListItemBlock li in list)
                {
                    foreach (Block sub in li)
                    {
                        var t = ExtractBlockText(sub);
                        if (!string.IsNullOrWhiteSpace(t))
                        {
                            items.Add("- " + t);
                        }
                    }
                }

                return string.Join("\n", items);

            default:
                return "";
        }
    }








    private static string ExtractContainerText(CustomContainer container)
    {
        var lines = new List<string>();
        foreach (Block block in container)
        {
            var text = ExtractBlockText(block);
            if (!string.IsNullOrWhiteSpace(text))
            {
                lines.Add(text);
            }
        }

        return string.Join("\n", lines);
    }








    private static string ExtractCodeDirective(string info)
    {
        // Example:
        // :::code language="csharp" source="snippets/foo.cs" id="Snippet1":::
        return info;
    }








    private static string ExtractTable(Table table)
    {
        var rows = new List<string>();

        foreach (TableRow row in table)
        {
            var cells = new List<string>();
            foreach (TableCell cell in row)
            {
                var cellText = new List<string>();
                foreach (Block block in cell)
                {
                    var t = ExtractBlockText(block);
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        cellText.Add(t);
                    }
                }

                cells.Add(string.Join(" ", cellText));
            }

            rows.Add("| " + string.Join(" | ", cells) + " |");
        }

        return string.Join("\n", rows);
    }








    public class ExtractedItem
    {
        public int Index { get; set; }
        public string? Kind { get; set; } // "DocPageMeta", "Section", "CodeBlock", "TextBlock", "Table"
        public string? Heading { get; set; }
        public string? Content { get; set; }
        public int HeadingLevel { get; set; }
    }
}