namespace ITCompanionAI.Ingestion.Docs;


public sealed class LearnIngestionRunner
{
    private readonly LearnPageParser _parser;
    private readonly DocRepository _repository;








    public LearnIngestionRunner(LearnPageParser parser, DocRepository repository)
    {
        _parser = parser;
        _repository = repository;
    }








    public async Task IngestAsync(IEnumerable<string> urls, Guid sourceSnapshotId, Guid ingestionRunId)
    {
        foreach (var url in urls)
        {
            var trimmed = url?.Trim();
            if (string.IsNullOrWhiteSpace(trimmed)) continue;

            var result = _parser.Parse(trimmed);

            //await _repository.InsertPageAsync(result.Page, result.Sections, result.CodeBlocks);
        }
    }








    public async Task IngestAsync(string url)
    {
        var trimmed = url?.Trim();


        if (trimmed != null)
        {
            var result = _parser.Parse(trimmed);
        }
    }
}