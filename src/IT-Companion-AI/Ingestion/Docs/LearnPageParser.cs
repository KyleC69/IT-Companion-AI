#nullable enable

using HtmlAgilityPack;

using ITCompanionAI.Utilities;

using Microsoft.Extensions.Logging;




namespace ITCompanionAI.Ingestion.Docs;





public sealed class LearnPageParser
{
    private ILogger<LearnPageParser>? _logger;








    public async Task<LearnPageParseResult?> ParseAsync(string pagePath, Guid? ingestionRunId, Guid? sourceSnapshotId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(pagePath))
        {
            throw new ArgumentException("Page path cannot be null or whitespace.", nameof(pagePath));
        }

        _logger = App.GetService<ILogger<LearnPageParser>>();
        cancellationToken.ThrowIfCancellationRequested();
        LearnPageParseResult? result = new();
        DocPage page = new();

        using IDisposable? scope = _logger.BeginScope(new Dictionary<string, object>
        {
                ["PagePath"] = pagePath,
                ["IngestionRunId"] = ingestionRunId,
                ["SourceSnapshotId"] = sourceSnapshotId
        });

        try
        {
            var markdown = await File.ReadAllTextAsync(pagePath, cancellationToken);
            Guid PageID = Guid.NewGuid();
            cancellationToken.ThrowIfCancellationRequested();

            var extracted = MarkdownExtractor.Extract(markdown);
            //  Transer page metadata and content for the page as a whole
            page.Url = pagePath; // Assuming URL is the same as the file path for now TODO: change db schema to this can map to either url or path depending on source type
            page.CreatedIngestionRunId = ingestionRunId ?? Guid.Empty;
            page.SourceSnapshotId = sourceSnapshotId ?? Guid.Empty;
            page.Id = PageID;
            page.Language = "en"; // default to English for now, we can enhance the parser later to detect language from metadata or content
            page.RawMarkdown = markdown;
            page.SourcePath = pagePath;
            page.ValidFromUtc = DateTime.Now;
            page.ContentHash = HashUtils.ComputeSha256(markdown);

            //Document/Page sections by kind
            foreach (MarkdownExtractor.ExtractedItem item in extracted)
                switch (item.Kind)
                {
                    case "DocPageMeta":

                        page.Title = item.Content.Split('\n').FirstOrDefault(line => line.StartsWith("title:", StringComparison.OrdinalIgnoreCase))?
                                .Split(':', 2)[1].Trim() ?? "Untitled";
                        _logger.LogInformation("Extracted DocPage metadata from markdown front matter.");
                        break;
                    case "Heading":
                        _logger.LogInformation("Extracted heading: {Heading}", item.Content);
                        break;
                    case "Paragraph":
                        _logger.LogInformation("Extracted paragraph with length {Length}.", item.Content.Length);
                        break;
                    case "CodeBlock":
                        _logger.LogInformation("Extracted code block with length {Length}.", item.Content.Length);
                        break;
                    default:
                        _logger.LogInformation("Extracted item of kind {Kind} with length {Length}.", item.Kind, item.Content.Length);
                        break;
                }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse learn page.");
            return null;

        }

        return result;
    }








    private static (List<DocSection> sections, List<CodeBlock> codeBlocks) ExtractSectionsAndCodeBlocks(HtmlNode article, DocPage page, Guid ingestionRunId, DateTime nowUtc, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(article);
        ArgumentNullException.ThrowIfNull(page);
        ArgumentNullException.ThrowIfNull(logger);

        List<DocSection> sections = [];
        List<CodeBlock> codeBlocks = [];

        HtmlNodeCollection headingNodes = article.SelectNodes(".//h2 | .//h3")
                                          ?? new HtmlNodeCollection(null);

        var orderIndex = 0;

        foreach (HtmlNode headingNode in headingNodes)
        {
            orderIndex++;

            var headingTextForLog = string.Empty;
            try
            {
                var headingId = headingNode.GetAttributeValue("id", string.Empty);
                var headingText = HtmlEntity.DeEntitize(headingNode.InnerText?.Trim() ?? string.Empty);
                headingTextForLog = headingText;

                int? level = headingNode.Name switch
                {
                        "h2" => 2,
                        "h3" => 3,
                        _ => null
                };

                var (sectionHtml, localCodeBlocks) = CollectSectionHtmlAndCodeBlocks(
                        headingNode,
                        article,
                        page,
                        ingestionRunId,
                        nowUtc,
                        logger);

                var sectionMarkdown = HtmlToMarkdown.Convert(sectionHtml);
                var sectionSemanticUid = HashUtils.ComputeSemanticUidForSection(
                        page.SemanticUid,
                        string.IsNullOrWhiteSpace(headingId) ? headingText : headingId,
                        level ?? 0,
                        orderIndex);

                Guid sectionId = Guid.NewGuid();

                DocSection section = new()
                {
                        Id = sectionId,
                        DocPageId = page.Id, // ✅ page -> section FK
                        SemanticUid = sectionSemanticUid,
                        Heading = headingText,
                        Level = level,
                        ContentMarkdown = sectionMarkdown,
                        OrderIndex = orderIndex,
                        VersionNumber = 1,
                        CreatedIngestionRunId = ingestionRunId,

                        UpdatedIngestionRunId = null,
                        RemovedIngestionRunId = null,
                        ValidFromUtc = nowUtc,
                        ValidToUtc = null,
                        IsActive = true,
                        ContentHash = HashUtils.ComputeSha256(sectionMarkdown)
                };

                foreach (CodeBlock cb in localCodeBlocks)
                {
                    cb.DocSectionId = sectionId; // ✅ section -> codeblock FK

                    var language = string.IsNullOrWhiteSpace(cb.Language) ? "unknown" : cb.Language;
                    var content = cb.Content ?? string.Empty;

                    cb.SemanticUid = HashUtils.ComputeSemanticUidForCodeBlock(
                            page.SemanticUid,
                            sectionSemanticUid,
                            language,
                            content) ?? string.Empty;
                }

                sections.Add(section);
                codeBlocks.AddRange(localCodeBlocks);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Section extraction failed. Heading='{Heading}' OrderIndex={OrderIndex}", headingTextForLog, orderIndex);
            }
        }

        return (sections, codeBlocks);
    }








    private static (string sectionHtml, List<CodeBlock> codeBlocks) CollectSectionHtmlAndCodeBlocks(HtmlNode headingNode, HtmlNode article, DocPage page, Guid ingestionRunId, DateTime nowUtc, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(headingNode);
        ArgumentNullException.ThrowIfNull(article);
        ArgumentNullException.ThrowIfNull(page);
        ArgumentNullException.ThrowIfNull(logger);

        StringBuilder builder = new();
        List<CodeBlock> codeBlocks = [];

        _ = builder.Append(headingNode.OuterHtml);

        HtmlNode? current = headingNode.NextSibling;

        while (current != null)
        {
            if (current.NodeType == HtmlNodeType.Element &&
                (current.Name == "h2" || current.Name == "h3"))
            {
                break;
            }

            if (IsLearnChromeNode(current))
            {
                current = current.NextSibling;
                continue;
            }

            if (current.NodeType == HtmlNodeType.Element)
            {
                HtmlNodeCollection? codeNodes = current.SelectNodes(".//pre/code");
                if (codeNodes != null)
                {
                    foreach (HtmlNode codeNode in codeNodes)
                        try
                        {
                            var languageClass = codeNode.GetAttributeValue("class", string.Empty);
                            var language = ExtractLanguageFromClass(languageClass) ?? "unknown";

                            var markdownCodeContent = HtmlToMarkdown.Convert(codeNode.OuterHtml);

                            if (string.IsNullOrWhiteSpace(markdownCodeContent))
                            {
                                continue;
                            }

                            CodeBlock codeBlock = new()
                            {
                                    Id = Guid.NewGuid(),
                                    DocSectionId = Guid.Empty, // patched later when sectionId is known
                                    Language = language,
                                    Content = markdownCodeContent,
                                    DeclaredPackages = null,
                                    Tags = null,
                                    InlineComments = null,
                                    VersionNumber = 1,
                                    CreatedIngestionRunId = ingestionRunId,
                                    UpdatedIngestionRunId = null,
                                    RemovedIngestionRunId = null,
                                    ValidFromUtc = nowUtc,
                                    ValidToUtc = null,
                                    IsActive = true,
                                    ContentHash = HashUtils.ComputeSha256(markdownCodeContent),
                                    SemanticUid = HashUtils.ComputeSemanticUidForCodeBlock(page.SemanticUid, string.Empty, language, markdownCodeContent) ?? string.Empty
                            };

                            codeBlocks.Add(codeBlock);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Code block extraction failed.");
                        }
                }

                _ = builder.Append(current.OuterHtml);
            }
            else if (current.NodeType == HtmlNodeType.Text)
            {
                _ = builder.Append(current.InnerText);
            }

            current = current.NextSibling;
        }

        return (builder.ToString(), codeBlocks);
    }








    private static bool IsLearnChromeNode(HtmlNode node)
    {
        if (node.NodeType != HtmlNodeType.Element)
        {
            return false;
        }

        var id = node.GetAttributeValue("id", string.Empty);
        if (id.Contains("toc", StringComparison.OrdinalIgnoreCase) ||
            id.Contains("breadcrumbs", StringComparison.OrdinalIgnoreCase) ||
            id.Contains("rating", StringComparison.OrdinalIgnoreCase) ||
            id.Contains("feedback", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var classes = string.Join(" ", node.GetClasses());
        return classes.Contains("feedback", StringComparison.OrdinalIgnoreCase) ||
               classes.Contains("rating", StringComparison.OrdinalIgnoreCase) ||
               classes.Contains("breadcrumb", StringComparison.OrdinalIgnoreCase) ||
               classes.Contains("toc", StringComparison.OrdinalIgnoreCase) ||
               classes.Contains("sidebar", StringComparison.OrdinalIgnoreCase);
    }








    private static string? ExtractLanguageFromClass(string classAttr)
    {
        if (string.IsNullOrWhiteSpace(classAttr))
        {
            return null;
        }

        var parts = classAttr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var p in parts)
        {
            if (p.StartsWith("language-", StringComparison.OrdinalIgnoreCase))
            {
                return p["language-".Length..];
            }

            if (p.StartsWith("lang-", StringComparison.OrdinalIgnoreCase))
            {
                return p["lang-".Length..];
            }
        }

        return null;
    }
}