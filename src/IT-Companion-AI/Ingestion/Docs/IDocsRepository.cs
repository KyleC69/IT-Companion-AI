using ITCompanionAI.Ingestion;





public interface IDocsRepository
{
    Task<DocsPage> GetByUrlAsync(string url);


    Task SaveAsync(DocsPage page);
}