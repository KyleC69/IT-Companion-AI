using Microsoft.Extensions.DataIngestion;




namespace ITCompanionAI.Ingestion.Docs;





internal class LinkProcessor : IngestionDocumentProcessor
{
    public override Task<IngestionDocument> ProcessAsync(IngestionDocument document, CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }
}