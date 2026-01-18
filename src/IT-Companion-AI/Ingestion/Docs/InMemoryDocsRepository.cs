using ITCompanionAI.Ingestion;





public sealed class InMemoryDocsRepository : IDocsRepository
{
    private readonly Dictionary<string, DocsPage> _store = new(StringComparer.OrdinalIgnoreCase);








    public Task<DocsPage> GetByUrlAsync(string url)
    {
        _store.TryGetValue(url, out DocsPage page);
        return Task.FromResult(page);
    }








    public Task SaveAsync(DocsPage page)
    {
        _store[page.Url] = page;
        return Task.CompletedTask;
    }
}