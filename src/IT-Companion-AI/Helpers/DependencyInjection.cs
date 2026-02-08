namespace ITCompanionAI.Helpers;





public class DependencyInjection
{
    public void RegisterServices()
    {
        _ = Path.Combine(@"d:\skApiRepo\semantic-kernel\dotnet", "src");

        //    var orchestrator = new IngestionOrchestrator(new AiagentRagContext(), new ApiHarvester(path), new XmlDocExtractor(), new MarkdownDocParser());
    }
}