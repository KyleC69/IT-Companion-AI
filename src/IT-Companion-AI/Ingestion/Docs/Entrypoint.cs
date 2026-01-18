using ITCompanionAI.Ingestion;





public static class Entrypoint
{
    public static async Task Main(string args)
    {
        var baseUrl = "https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview";


        LearnIngestionPipeline pipeline = new();

        await pipeline.IngestAsync(args);

        Console.WriteLine("Ingestion complete.");
    }
}