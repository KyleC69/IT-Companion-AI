using ITCompanionAI.EFModels;
using ITCompanionAI.Services;

using Microsoft.Extensions.Logging;



namespace ITCompanionAI.Ingestion.Docs;


public class DocIngester
{
    private static readonly System.Diagnostics.TraceSource Log = new("DocsIngestion", System.Diagnostics.SourceLevels.All);
    private readonly HttpClient _http = App.GetService<HttpClientService>();
    private readonly TocFetcher _tocFetcher = new();
    private ILogger<DocIngester> _logger;
    private KBContext dbContext;








    public async Task RunIngestion(string startingUrl)
    {
        var connectionString = """server=MSSQLServer;Database=KnowledgeBase;Trusted_Connection=True;""";
        dbContext = new KBContext();
        Guid? ingestionRunId = Guid.NewGuid(); // Change Guid to Guid?
        Guid sourceSnapshotId = Guid.NewGuid();
        dbContext.SpBeginIngestionRun("1111", "rerun doc ingestion", ref ingestionRunId); // Pass as ref Guid?


        LearnPageParser parser = new(App.GetService<HttpClientService>());
        DocRepository repo = new(connectionString);
        LearnIngestionRunner runner = new(parser, repo);



        _logger.LogTrace("Starting Document ingestion run");
        // 1. Fetch TOC JSON

        var toc = await _tocFetcher.FetchAsync(startingUrl);
        Log.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "TOC root items: {0}", toc?.Count ?? 0);


        // 2. Normalize TOC hrefs to full URLs
        TocFetcher.TocNormalizer.Normalize(toc, startingUrl);



        // 3. Flatten TOC
        var flat = TocFlattener.Flatten(toc).ToList();
        Log.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "Flattened TOC entries: {0}", flat.Count);


        //############################################
        //##
        //##    Starting page ingestion here
        // 4.
        var list = flat.Select(entry => entry.Url).ToList();

        try
        {
            await runner.IngestAsync(list, sourceSnapshotId, (Guid)ingestionRunId!);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"[ERROR] Fetching URL: {ex.Message}");
            //       Log.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0, "Fetch failed Url={0} Error={1}", entry.Url, ex.Message);
        }
    }
}