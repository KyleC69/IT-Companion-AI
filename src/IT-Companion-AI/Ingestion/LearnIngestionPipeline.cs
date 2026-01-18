using System.Data;
using System.Text.Json;

using ITCompanionAI.Ingestion.Docs;
using ITCompanionAI.Services;
using ITCompanionAI.Utilities;

using Markdig;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using ReverseMarkdown;



namespace ITCompanionAI.Ingestion;


public sealed class LearnIngestionPipeline
{
    private static readonly HttpClient _http = App.GetService<HttpClientService>();
    private static readonly System.Diagnostics.TraceSource Log = new("DocsIngestion", System.Diagnostics.SourceLevels.All);
    private readonly ContentExtractor _extractor = new();
    private readonly ILogger<LearnIngestionPipeline> _logger = App.GetService<ILogger<LearnIngestionPipeline>>();
    private readonly Converter _reverseMarkdown;
    private readonly TocFetcher _tocFetcher = new();








    public LearnIngestionPipeline()
    {
        _reverseMarkdown = new Converter(); // optionally pass Config
    }








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
                resp.EnsureSuccessStatusCode();

                html = await resp.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"[ERROR] Fetching {entry.Url}: {ex.Message}");
                Log.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0, "Fetch failed Url={0} Error={1}", entry.Url, ex.Message);
                continue;
            }

            // 6. Extract main HTML content
            var mainHtml = _extractor.ExtractMainHtml(html);
            if (string.IsNullOrWhiteSpace(mainHtml))
            {
                Log.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "Skipping entry with empty <main>. Url={0}", entry.Url);
                continue;
            }

            // 7. HTML → Markdown
            var markdown = _reverseMarkdown.Convert(mainHtml);

            // 8. Normalize Markdown (Markdig)
            var normalized = Markdown.Normalize(markdown);

            // 9. Hash for change detection
            var hash = HashUtil.ComputeHash(normalized);

            // 10. Check existing


            // 11. Build page model
            DocsPage page = new() { Url = entry.Url };
            page.Uid = entry.Uid;
            page.Title = entry.Title;
            page.Breadcrumb = entry.Breadcrumb;
            page.Html = html; // Raw response from server
            page.Markdown = markdown;
            page.NormalizedMarkdown = normalized;
            page.Hash = hash;
            page.LastFetched = DateTimeOffset.UtcNow;

            // 12. Persist
            await SaveDocumentAsync(page);

            Console.WriteLine($"[SAVED] {entry.Url}");
        }
    }








    private async Task SaveDocumentAsync(DocsPage page)
    {
        Verify.NotNull(page, nameof(page));
        var sqlConnectionString = "Data Source=DESKTOP-NC01091;Initial Catalog=AIDataRAG;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";
        await using SqlConnection connection = new(sqlConnectionString);
        await connection.OpenAsync();

        // Check if document already exists
        SqlCommand selectCommand = new("SELECT COUNT(*) FROM Documents WHERE Url = @Url", connection);
        selectCommand.Parameters.AddWithValue("@Url", page.Url);
        var exists = (int)await selectCommand.ExecuteScalarAsync() > 0;

        if (exists)
        {
            // Update existing document
            await using SqlCommand updateCommand = new(
                "UPDATE Documents SET Uid = @Uid, Title = @Title, Breadcrumb = @Breadcrumb, DocHtml = @DocHtml, NormalizedMarkdown = @NormalizedMarkdown, Hash = @Hash, LastFetched = @LastFetched WHERE Url = @Url",
                connection);
            AddDocumentParameters(updateCommand, page);
            await updateCommand.ExecuteNonQueryAsync();
        }
        else
        {
            // Insert new document
            await using SqlCommand insertCommand = new(
                "INSERT INTO Documents (Url, Uid, Title, ContentRaw, Breadcrumb, DocHtml, NormalizedMarkdown, Hash, LastFetched) VALUES (@Url, @Uid, @Title, @ContentRaw, @Breadcrumb, @DocHtml, @NormalizedMarkdown, @Hash, @LastFetched)",
                connection);
            AddDocumentParameters(insertCommand, page);
            insertCommand.Parameters.Add("@ContentRaw", SqlDbType.NVarChar, -1).Value = (object?)page.Html ?? DBNull.Value;
            await insertCommand.ExecuteNonQueryAsync();
        }
    }








    private static void AddDocumentParameters(SqlCommand command, DocsPage page)
    {
        command.Parameters.Add("@Url", SqlDbType.NVarChar, 2048).Value = page.Url;

        // Keep Uid as NVARCHAR to match DocsPage.Uid (string) and avoid implicit conversions.
        command.Parameters.Add("@Uid", SqlDbType.NVarChar, 128).Value = (object?)page.Uid ?? DBNull.Value;

        command.Parameters.Add("@Title", SqlDbType.NVarChar, 512).Value = (object?)page.Title ?? DBNull.Value;

        var breadcrumbJson = page.Breadcrumb is null ? null : JsonSerializer.Serialize(page.Breadcrumb);
        command.Parameters.Add("@Breadcrumb", SqlDbType.NVarChar, -1).Value = (object?)breadcrumbJson ?? DBNull.Value;

        command.Parameters.Add("@DocHtml", SqlDbType.NVarChar, -1).Value = (object?)page.Html ?? DBNull.Value;
        command.Parameters.Add("@NormalizedMarkdown", SqlDbType.NVarChar, -1).Value = (object?)page.NormalizedMarkdown ?? DBNull.Value;
        command.Parameters.Add("@Hash", SqlDbType.NVarChar, 128).Value = (object?)page.Hash ?? DBNull.Value;
        command.Parameters.Add("@LastFetched", SqlDbType.DateTimeOffset).Value = page.LastFetched;
    }
}