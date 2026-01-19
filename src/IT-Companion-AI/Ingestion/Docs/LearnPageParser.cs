#nullable enable

using HtmlAgilityPack;

using ITCompanionAI.Services;



namespace ITCompanionAI.Ingestion.Docs;


public sealed class LearnPageParser
{
    private readonly HttpClientService _httpClient;








    public LearnPageParser()
    {
        _httpClient = App.GetService<HttpClientService>();
    }








    /// <summary>
    ///     Parses a Microsoft Learn page from the specified URL and extracts its main content, metadata, and code blocks
    ///     into a structured result.
    /// </summary>
    /// <remarks>
    ///     The method targets the main article content of a Microsoft Learn page for extraction. Only
    ///     the primary article section is parsed; other page elements are ignored. The returned result includes the page in
    ///     markdown format, along with any identified sections and code blocks.
    /// </remarks>
    /// <param name="url">The URL of the Microsoft Learn page to parse. Must be a valid, accessible HTTP or HTTPS address.</param>
    /// <param name="ingestionRunId">The unique identifier for the ingestion run associated with this parsing operation.</param>
    /// <param name="sourceSnapshotId">
    ///     The unique identifier of the source snapshot representing the state of the source at the
    ///     time of parsing.
    /// </param>
    /// <returns>
    ///     A <see cref="LearnPageParseResult" /> containing the parsed page metadata, content sections, and code blocks
    ///     extracted from the specified Learn page.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the main content of the Learn page cannot be located in the HTML
    ///     document.
    /// </exception>
    public LearnPageParseResult Parse(string url, Guid ingestionRunId, Guid sourceSnapshotId)
    {
        var htmlstring = _httpClient.GetWebDocument(url);


        HtmlDocument? doc = null;

        if (doc is null)
        {
            return new LearnPageParseResult();
        }

        DateTime now = DateTime.Now;

        // Anchor on Learn main article
        HtmlNode? article = doc.DocumentNode.SelectSingleNode("//article[@id='main']")
                            ?? doc.DocumentNode.SelectSingleNode("//*[@id='main-content']")
                            ?? doc.DocumentNode.SelectSingleNode("//body");

        if (article is null)
        {
            throw new InvalidOperationException("Unable to locate main content on Learn page.");
        }

        HtmlNode? titleNode = article.SelectSingleNode(".//h1")
                              ?? doc.DocumentNode.SelectSingleNode("//h1")
                              ?? doc.DocumentNode.SelectSingleNode("//title");

        var title = HtmlEntity.DeEntitize(titleNode?.InnerText?.Trim() ?? string.Empty);

        var pageSemanticUid = HashUtils.ComputeSemanticUidForPage(url);

        // Use only the main article HTML as the "raw" content
        var articleHtml = article.InnerHtml;
        var pageMarkdown = HtmlToMarkdown.Convert(articleHtml);

        DocPage page = new()
        {
            Id = Guid.NewGuid(),
            SemanticUid = pageSemanticUid,
            SourceSnapshotId = sourceSnapshotId,
            SourcePath = url,
            Title = title,
            Language = "en-us", // Could be parsed from URL if needed
            Url = url,
            RawMarkdown = pageMarkdown,
            VersionNumber = 1,
            CreatedIngestionRunId = ingestionRunId,
            UpdatedIngestionRunId = null,
            RemovedIngestionRunId = null,
            ValidFromUtc = now,
            ValidToUtc = null,
            IsActive = true,
            ContentHash = HashUtils.ComputeSha256(pageMarkdown)
        };

        var (sections, codeBlocks) = ExtractSectionsAndCodeBlocks(article, page, ingestionRunId, now);

        return new LearnPageParseResult
        {
            Page = page,
            Sections = sections,
            CodeBlocks = codeBlocks
        };
    }








    private static (List<DocSection> sections, List<CodeBlock> codeBlocks) ExtractSectionsAndCodeBlocks(
        HtmlNode article,
        DocPage page,
        Guid ingestionRunId,
        DateTime nowUtc)
    {
        var sections = new List<DocSection>();
        var codeBlocks = new List<CodeBlock>();

        // Learn API pages: major content is under <article id="main"> with h2/h3 structure
        HtmlNodeCollection headingNodes = article.SelectNodes(".//h2 | .//h3")
                                          ?? new HtmlNodeCollection(null);

        var orderIndex = 0;

        foreach (HtmlNode headingNode in headingNodes)
        {
            orderIndex++;

            var headingId = headingNode.GetAttributeValue("id", string.Empty);
            var headingText = HtmlEntity.DeEntitize(headingNode.InnerText.Trim());
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
                nowUtc);

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
                DocPageId = page.Id,
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

            // Patch code blocks with section linkage + semantic UID
            foreach (CodeBlock cb in localCodeBlocks)
            {
                cb.DocSectionId = sectionId;

                cb.SemanticUid = HashUtils.ComputeSemanticUidForCodeBlock(
                    page.SemanticUid,
                    sectionSemanticUid,
                    cb.Language,
                    cb.Content);
            }

            sections.Add(section);
            codeBlocks.AddRange(localCodeBlocks);
        }

        return (sections, codeBlocks);
    }








    /// <summary>
    ///     Collects the HTML for a section (heading + content until the next heading of same/higher level)
    ///     and extracts code blocks as markdown.
    /// </summary>
    private static (string sectionHtml, List<CodeBlock> codeBlocks) CollectSectionHtmlAndCodeBlocks(
        HtmlNode headingNode,
        HtmlNode article,
        DocPage page,
        Guid ingestionRunId,
        DateTime nowUtc)
    {
        StringBuilder builder = new();
        var codeBlocks = new List<CodeBlock>();

        // Include the heading itself in the HTML fragment
        builder.Append(headingNode.OuterHtml);

        HtmlNode current = headingNode.NextSibling;

        while (current != null)
        {
            if (current.NodeType == HtmlNodeType.Element &&
                (current.Name == "h2" || current.Name == "h3"))
                // Stop at next peer heading
            {
                break;
            }

            // Skip Learn chrome/feedback wrappers by class/id if necessary (can be extended)
            if (IsLearnChromeNode(current))
            {
                current = current.NextSibling;
                continue;
            }

            if (current.NodeType == HtmlNodeType.Element)
            {
                // Extract code blocks inside this segment
                HtmlNodeCollection codeNodes = current.SelectNodes(".//pre/code");
                if (codeNodes != null)
                {
                    foreach (HtmlNode codeNode in codeNodes)
                    {
                        var languageClass = codeNode.GetAttributeValue("class", string.Empty);
                        var language = ExtractLanguageFromClass(languageClass);

                        var markdownCodeContent = HtmlToMarkdown.Convert(codeNode.OuterHtml);

                        CodeBlock codeBlock = new()
                        {
                            Id = Guid.NewGuid(),
                            DocSectionId = Guid.Empty, // patched later
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
                            SemanticUid = HashUtils.ComputeSemanticUidForCodeBlock(
                                page.SemanticUid,
                                string.Empty, // patched later with section UID
                                language,
                                markdownCodeContent)
                        };

                        codeBlocks.Add(codeBlock);
                    }
                }

                builder.Append(current.OuterHtml);
            }
            else if (current.NodeType == HtmlNodeType.Text)
            {
                builder.Append(current.InnerText);
            }

            current = current.NextSibling;
        }

        return (builder.ToString(), codeBlocks);
    }








    /// <summary>
    ///     Filter out Learn UI chrome (nav, feedback, rating widgets, etc.) if they appear inside article.
    ///     Extend as you encounter noise.
    /// </summary>
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
        if (classes.Contains("feedback", StringComparison.OrdinalIgnoreCase) ||
            classes.Contains("rating", StringComparison.OrdinalIgnoreCase) ||
            classes.Contains("breadcrumb", StringComparison.OrdinalIgnoreCase) ||
            classes.Contains("toc", StringComparison.OrdinalIgnoreCase) ||
            classes.Contains("sidebar", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }








    private static string ExtractLanguageFromClass(string classAttr)
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








/*
    public LearnPageParseResult Parse(string url)
    {
        HtmlWeb web = new();
        HtmlDocument doc = web.Load(url);

        DateTime now = DateTime.UtcNow;

        // Anchor on Learn main article
        HtmlNode article = doc.DocumentNode.SelectSingleNode("//article[@id='main']")
                           ?? doc.DocumentNode.SelectSingleNode("//*[@id='main-content']")
                           ?? doc.DocumentNode.SelectSingleNode("//body");

        if (article is null) throw new InvalidOperationException("Unable to locate main content on Learn page.");

        HtmlNode titleNode = article.SelectSingleNode(".//h1")
                             ?? doc.DocumentNode.SelectSingleNode("//h1")
                             ?? doc.DocumentNode.SelectSingleNode("//title");

        var title = HtmlEntity.DeEntitize(titleNode?.InnerText?.Trim() ?? string.Empty);

        var pageSemanticUid = HashUtils.ComputeSemanticUidForPage(url);

        // Use only the main article HTML as the "raw" content
        var articleHtml = article.InnerHtml;
        var pageMarkdown = HtmlToMarkdown.Convert(articleHtml);


        //select all the links from the table of contents
      //  HtmlNode? toc = article.SelectSingleNode("//*[@id='ms--toc-content']");

        HtmlNodeCollection? links = toc?.SelectNodes("//a[@href]");
        if (links is null || links.Count == 0)
            throw new InvalidOperationException("No links found in the table of contents.");

        return default;
    }
*/
}