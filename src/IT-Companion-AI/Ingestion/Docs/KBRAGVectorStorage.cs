using Microsoft.Extensions.DataIngestion;
using Microsoft.Extensions.Logging;




namespace ITCompanionAI.Ingestion.Docs;





internal partial class MyIngestionChunkWriter : IngestionChunkWriter<string>
{
    private readonly ILogger<MyIngestionChunkWriter> _logger;








    internal MyIngestionChunkWriter()
    {
        _logger = App.GetService<ILogger<MyIngestionChunkWriter>>();
    }








    public override async Task WriteAsync(IAsyncEnumerable<IngestionChunk<string>> chunks, CancellationToken cancellationToken = new())
    {
        await foreach (var chunk in chunks.WithCancellation(cancellationToken))
        {
            // Process each chunk
            foreach (var item in chunk.Content)
                    // Perform necessary operations with each item
                    // Example: Store or log the item
                _logger.LogDebug("Writing chunk item: {Item}", item);
        }
    }
}