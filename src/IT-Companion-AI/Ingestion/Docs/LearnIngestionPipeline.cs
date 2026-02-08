using ITCompanionAI.Services;

using Microsoft.Extensions.Logging;

using RMarkdown = ReverseMarkdown;




namespace ITCompanionAI.Ingestion.Docs;





public sealed class LearnIngestionPipeline
{
    private static readonly HttpClient _http = App.GetService<HttpClientService>();
    private static readonly System.Diagnostics.TraceSource Log = new("DocsIngestion", System.Diagnostics.SourceLevels.All);
    private readonly ContentExtractor _extractor = new();
    private readonly ILogger<LearnIngestionPipeline> _logger = App.GetService<ILogger<LearnIngestionPipeline>>();
    private readonly RMarkdown.Converter _reverseMarkdown;
    private readonly TocFetcher _tocFetcher = new();








    public LearnIngestionPipeline()
    {
        _reverseMarkdown = new RMarkdown.Converter(); // optionally pass Config
    }








    /*

    public async Task IngestAsync(string baseUrl)
    {
        _logger.LogTrace("Starting Document ingestion run");
        // 1. Fetch TOC JSON

        var toc = await _tocFetcher.FetchAsync(baseUrl);
        Log.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "TOC root items: {0}", toc?.Count ?? 0);
        // 2. Normalize TOC hrefs to full URLs
        TocFetcher.TocNormalizer.Normalize(toc, baseUrl);
        // 3. Flatten TOC
        var flat = TocFlattener.Flatten(toc).ToList();
        Log.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "Flattened TOC entries: {0}", flat.Count);
        //############################################
        //##
        //##    Starting page ingestion here
        // 4.
        foreach (FlatTocEntry entry in flat)
        {
            if (string.IsNullOrWhiteSpace(entry.Url))
            {
                Log.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "Skipping entry with blank Url. Title={0}", entry.Title);
                continue;
            }

            _logger.LogInformation($"[INGEST] {entry.Url}");

            // 5. Fetch HTML
            string html;
            try
            {
                using HttpResponseMessage resp = await _http.GetAsync(entry.Url, HttpCompletionOption.ResponseHeadersRead);
                _ = resp.EnsureSuccessStatusCode();

                html = await resp.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"[ERROR] Fetching {entry.Url}: {ex.Message}");
                Log.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0, "Fetch failed Url={0} Error={1}", entry.Url, ex.Message);
                continue;
            }

            //##
            //############################################
            // 6. Extract main HTML content
            var mainHtml = _extractor.ExtractContent(html, ExtractionMode.Html, DocumentType.LearnPage);


            if (string.IsNullOrWhiteSpace(mainHtml))
            {
                Log.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "Skipping entry with empty <main>. Url={0}", entry.Url);
                continue;
            }

            //##
            //##
            //##
            //############################################
            // 7. HTML → Markdown
            var markdown = _reverseMarkdown.Convert(mainHtml);

            // 8. Normalize Markdown (Markdig)
            var normalized = Markdown.Normalize(markdown);

            // 9. Hash for change detection
            var hash = HashUtil.ComputeHash(normalized);

            // 10. Check existing


            // 11. Build page model
            DocPage page = new()
            {

                    SemanticUid = entry.Uid,
                    SourceSnapshotId = default,
                    SourcePath = null,
                    Url = entry.Url,
                    RawMarkdown = null,
                    VersionNumber = 0,
                    CreatedIngestionRunId = default,
                    UpdatedIngestionRunId = null,
                    RemovedIngestionRunId = null,
                    ValidFromUtc = default,
                    ValidToUtc = null,
                    IsActive = false,
                    ContentHash = new byte[]
                    {
                    },
                    SemanticUidHash = new byte[]
                    {
                    },
                    RawPageSource = null,
                    CreatedIngestionRun = null,
                    DocSections = null,
                    SourceSnapshot = null,

                    Title = entry.Title,
                    Language = null


            };

            // 12. Persist
            //await SaveDocumentAsync(page);

            Console.WriteLine($"[SAVED] {entry.Url}");





        }

    }
    */
}