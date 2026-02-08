#nullable enable

using HtmlAgilityPack;

using ITCompanionAI.Services;

using Microsoft.Extensions.Logging;




namespace ITCompanionAI.Ingestion.Docs;





public sealed class LearnPageParser
{
    private readonly HttpClientService _httpClient;
    private ILogger<LearnPageParser> _logger;








    public async Task<LearnPageParseResult?> ParseAsync(string url, Guid ingestionRunId, Guid sourceSnapshotId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or whitespace.", nameof(url));
        }

        _logger = App.GetService<ILogger<LearnPageParser>>();
        cancellationToken.ThrowIfCancellationRequested();

        using IDisposable? scope = _logger.BeginScope(new Dictionary<string, object>
        {
                ["Url"] = url,
                ["IngestionRunId"] = ingestionRunId,
                ["SourceSnapshotId"] = sourceSnapshotId
        });

        try
        {
            var htmlstring = await SourceExtractor.GetRenderedHtmlAsync(url, cancellationToken);
            if (string.IsNullOrWhiteSpace(htmlstring))
            {
                _logger.LogWarning("Fetched HTML was empty for URL: {Url}", url);
                return null;
            }

            cancellationToken.ThrowIfCancellationRequested();


            HtmlDocument doc = new();
            doc.LoadHtml(htmlstring);


            //DO NOT CHANGE
            DateTime now = DateTime.Now;

            HtmlNode? article = doc.DocumentNode?.SelectSingleNode("//div[@class='content'][2]")
                                ?? doc.DocumentNode?.SelectSingleNode("//*[@id='main-content']")
                                ?? doc.DocumentNode?.SelectSingleNode("//body");

            if (article is null)
            {
                _logger.LogWarning("Unable to locate main content on Learn page.");
            }

            HtmlNode? titleNode = article.SelectSingleNode(".//h1")
                                  ?? doc.DocumentNode?.SelectSingleNode("//h1")
                                  ?? doc.DocumentNode?.SelectSingleNode("//title");

            var title = HtmlEntity.DeEntitize(titleNode?.InnerText?.Trim() ?? string.Empty);

            var pageSemanticUid = HashUtils.ComputeSemanticUidForPage(url);

            var articleHtml = article.InnerHtml ?? string.Empty;
            var pageMarkdown = HtmlToMarkdown.Convert(articleHtml);

            DocPage page = new()
            {
                    Id = Guid.NewGuid(),
                    SemanticUid = pageSemanticUid,
                    SourceSnapshotId = sourceSnapshotId,
                    SourcePath = url,
                    Title = title,
                    Language = "en-us",
                    Url = url,
                    RawMarkdown = pageMarkdown,
                    RawPageSource = htmlstring,
                    VersionNumber = 1,
                    CreatedIngestionRunId = ingestionRunId,
                    UpdatedIngestionRunId = null,
                    RemovedIngestionRunId = null,
                    ValidFromUtc = now,
                    ValidToUtc = null,
                    IsActive = true,
                    ContentHash = HashUtils.ComputeSha256(pageMarkdown)
            };

            var (sections, codeBlocks) = ExtractSectionsAndCodeBlocks(article, page, ingestionRunId, now, _logger);


            _logger.LogInformation("Learn page parsed successfully.Url={url} Sections={SectionCount} CodeBlocks={CodeBlockCount}", url, sections.Count, codeBlocks.Count);

            return new LearnPageParseResult
            {
                    Page = page,
                    Sections = sections,
                    CodeBlocks = codeBlocks
            };
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Learn page parsing canceled.");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Learn page parsing failed.");
            return null;
        }
    }








    private static (List<DocSection> sections, List<CodeBlock> codeBlocks) ExtractSectionsAndCodeBlocks(HtmlNode article, DocPage page, Guid ingestionRunId, DateTime nowUtc, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(article);
        ArgumentNullException.ThrowIfNull(page);
        ArgumentNullException.ThrowIfNull(logger);

        var sections = new List<DocSection>();
        var codeBlocks = new List<CodeBlock>();

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

                var level = headingNode.Name switch
                {
                        "h2" => 2,
                        "h3" => 3,
                        _ => (int?)null
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
        var codeBlocks = new List<CodeBlock>();

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