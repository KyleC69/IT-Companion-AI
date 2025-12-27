using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

using SkAgentGroup.AgentFramework.Tools;
using SkAgentGroup.AgentFramework.Memory;

using System.Buffers;
namespace SkAgentGroup.AgentFramework;

public static class AgentServiceCollectionExtensions
{
    public static IServiceCollection AddAgentFramework(this IServiceCollection services)
    {
        //
        // 1. Register LLM clients
        //
        services.AddSingleton(sp =>
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")!;
            return new OpenAI.OpenAIClient(apiKey);
        });

        services.AddKeyedSingleton<IChatClient>("planner-llm", (sp, _) =>
        {
            var client = sp.GetRequiredKeyedService<OpenAI.OpenAIClient>("gpt-4.1-mini");
            return OpenAIChatClient(client, "gpt-4.1-mini");
        });

        services.AddKeyedSingleton<IChatClient>("coder-llm", (sp, _) =>
        {
            var client = sp.GetRequiredService<OpenAI.OpenAIClient>();
            return new OpenAIChatClient(client, "gpt-4.1");
        });

        services.AddKeyedSingleton<IChatClient>("critic-llm", (sp, _) =>
        {
            var client = sp.GetRequiredService<OpenAI.OpenAIClient>();
            return new OpenAIChatClient(client, "gpt-4.1-mini");
        });

        //
        // 2. Register memory system (your PgVectorAgentMemory)
        //
        services.AddSingleton<IAgentMemory>(sp =>
        {
            // You already have PgVectorAgentMemory wired elsewhere.
            // Just resolve it here.
            return sp.GetRequiredService<PgVectorAgentMemory>();
        });

        //
        // 3. Register tools
        //
        services.AddSingleton<FileTools>();
        services.AddSingleton<WebTools>();
        services.AddSingleton<MemoryTools>();
        services.AddSingleton<AgentTools>();

        //
        // 4. Register agents (attach tools inside the factory)
        //
        services.AddSingleton<PlanningAgent>(sp =>
        {
            var memory = sp.GetRequiredService<IAgentMemory>();
            var llm = sp.GetRequiredService<IChatClient>("planner-llm");

            var agent = new PlanningAgent(memory, llm);
            agent.AddMinimalTools(sp);
            return agent;
        });

        services.AddSingleton<CodingAgent>(sp =>
        {
            var memory = sp.GetRequiredService<IAgentMemory>();
            var llm = sp.GetRequiredService<IChatClient>("coder-llm");

            // Provide required parameters: agentId, agentRuntime, description, logger (optional)
            var agentId = "coding-agent";
            var agentRuntime = sp.GetRequiredService<IAgentRuntime>();
            var description = "Agent responsible for code generation and related tasks.";
            var logger = sp.GetService<ILogger<CodingAgent>>();

            var agent = new CodingAgent(agentId, agentRuntime, description, logger);
            agent.AddMinimalTools(sp);
            return agent;
        });

        services.AddSingleton<CriticAgent>(sp =>
        {
            var memory = sp.GetRequiredService<IAgentMemory>();
            var llm = sp.GetRequiredService<IChatClient>("critic-llm");

            var agent = new CriticAgent(memory, llm);
            agent.AddMinimalTools(sp);
            return agent;
        });

        //
        // 5. Orchestrator / Supervisor
        //
        services.AddSingleton<SupervisorAgent>();
        services.AddSingleton<AgentOrchestrator>();

        return services;
    }
}