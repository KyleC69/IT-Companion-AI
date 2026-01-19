namespace ITCompanionAI.Ingestion.Docs;


public sealed class LearnIngestionRunner
{
    private static readonly System.Diagnostics.TraceSource Log = new("DocsIngestion", System.Diagnostics.SourceLevels.All);
    private readonly LearnPageParser _parser;
    private readonly DocRepository _repository;








    public LearnIngestionRunner(LearnPageParser parser, DocRepository repository)
    {
        _parser = parser;
        _repository = repository;
    }








    public async Task IngestAsync(IEnumerable<string> urls, Guid sourceSnapshotId, Guid ingestionRunId)
    {
        LearnPageParseResult result = new();
        foreach (var url in urls)
        {
            var trimmed = url?.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                continue;
            }




            try
            {
                result = _parser.Parse(trimmed, Guid.NewGuid(), Guid.NewGuid());
            }
            catch (Exception e)
            {
                Log.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0, "Fetch failed Url={0} Error={1}", url, e.Message);
                Console.WriteLine(e);
                throw;
            }


            await _repository.InsertPageAsync(result.Page, result.Sections, result.CodeBlocks);
        }
    }








    public async Task IngestAsync(string url)
    {
        var trimmed = url?.Trim();


        if (trimmed != null)
        {
            //     var result = _parser.Parse(url);
        }
    }
}