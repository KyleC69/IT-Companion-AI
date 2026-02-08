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








    public async Task IngestAsync(IEnumerable<string> urls, Guid sourceSnapshotId, Guid ingestionRunId, CancellationToken cancellationToken = default)
    {
        if (urls == null)
        {
            throw new ArgumentNullException(nameof(urls));
        }

        foreach (var url in urls)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var trimmed = url?.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                continue;
            }

            LearnPageParseResult result;
            try
            {
                result = await _parser.ParseAsync(trimmed, ingestionRunId, sourceSnapshotId, cancellationToken);
                if (result == null)
                {
                    continue;
                }
            }
            catch (HttpRequestException e)
            {
                Log.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0, "Fetch failed Url={0} Error={1}", url, e.Message);
                Console.WriteLine(e.Message);
                await Delay(5, cancellationToken); //short delay on fetch failure
                continue; //Prevent premature exit from ingestion run on single url fetch failure
            }

            //##############################
            //persist the parsed result
            try
            {
                await _repository.InsertPageAsync(result.Page, result.Sections, result.CodeBlocks);
            }
            catch (Exception ex)
            {
                Log.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0, "Fetch failed Url={0} Error={1}", url, ex.Message);
                Console.WriteLine(ex.Message);
                continue;
            }

            //##############################
            // Page chunker and embedding generation
            //
            try
            {


            }
            catch (Exception ex)
            {
                Log.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0, "Chunking/Embedding failed Url={0} Error={1}", url, ex.Message);
                Console.WriteLine(ex.Message);
            }








            //hard throttle to avoid overwhelming the server
            //await Delay(5, cancellationToken);
        }
    }








    public async Task Delay(int howLong, CancellationToken token = default)
    {
        //preconfig for seconds for easy use
        //TODO: Read default delay from config if not provided
        await Task.Delay(TimeSpan.FromSeconds(howLong), token);
    }
}