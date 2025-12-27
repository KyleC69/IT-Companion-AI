#pragma warning disable SKEXP0001, SKEXP0101, SKEXP0010

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.PgVector;

namespace SkAgentGroup.AgentFramework;

public static class KernelSetup
{

    public static IServiceCollection AddAiKernel(this IServiceCollection services)
    {
        var githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN")
            ?? throw new InvalidOperationException("Missing GitHub API key in configuration.");
        var postgresConnectionString = """Host=127.0.0.1;Database=postgres;Username=postgres;Password=Agent1234;Persist Security Info=True"""
            ?? throw new InvalidOperationException("Missing Postgres connection string in environment variable 'POSTGRES_CONNECTIONSTRING'.");

        var phiModel = "Phi-4-mini-instruct";
        var openAiEndpoint = new Uri("https://models.github.ai/inference");

        var loggingConfiguration = new Action<ILoggingBuilder>(c => c.AddConsole().SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace));

        var builder = services.AddKernel();
        builder.Services.AddOpenAIChatCompletion(modelId: phiModel, endpoint: openAiEndpoint, apiKey: githubToken);
        builder.Services.AddOpenAIEmbeddingGenerator("text-embedding-3-small", apiKey: githubToken);
        builder.Services.AddLogging(loggingConfiguration);

        services.AddSingleton<PostgresVectorStore>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger>();

            //TASK: Complete PostgresVectorStore initialization
            var store = new Microsoft.SemanticKernel.Connectors.PgVector.PostgresVectorStore();
            
            store
            return store;
        });

        services.AddSingleton<GeneralAgent>();
        services.AddSingleton<AgentLoop>();

        return services;
    }

}