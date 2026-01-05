using System;
using System.Threading;
using System.Threading.Tasks;

using ITCompanionAI.AgentFramework.Ingestion;
using ITCompanionAI.Helpers;

namespace ITCompanionAI.AgentFramework;

internal class APIKnowledgebaseOrchestration
{
    public async Task InitiateIngestionAsync(CancellationToken cancellationToken = default)
    {
        // Starts Ingestion Process
        try
        {
          
            // Create authenticated client via DI so config/user-secrets are available.
            var ghFactory = App.GetService<IGitHubClientFactory>();
            _ = ghFactory.CreateClient();
            var ingest= App.GetService<ApiHarvester>();

        }
        catch (Exception ex)
        {
            // Log the exception (replace with actual logging mechanism)
            Console.WriteLine($"Error during ingestion process: {ex.Message}");
            throw;
        }
    }

  
}
