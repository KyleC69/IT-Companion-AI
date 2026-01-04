// Project Name: SKAgent
// File Name: KnowledgeIngestionOrchestrator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using ITCompanionAI.AgentFramework.Ingestion;

using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;


namespace ITCompanionAI.AgentFramework.Planning;


public interface IKnowledgeIngestionOrchestrator
{
    Task<IngestionPlan> BuildOrUpdateKnowledgeBaseAsync(string goal, CancellationToken cancellationToken = default);
}



public sealed class KnowledgeIngestionOrchestrator : IKnowledgeIngestionOrchestrator
{
    private readonly IWebFetcher _fetcher;
    private readonly IIngestionAgent _ingester;
    private readonly IContentParser _parser;
    private readonly IPlannerAgent _planner;





    public KnowledgeIngestionOrchestrator(IPlannerAgent planner, IWebFetcher fetcher, IContentParser parser,
        IIngestionAgent ingester)
    {
        _planner = planner;
        _fetcher = fetcher;
        _parser = parser;
        _ingester = ingester;
    }





    public async Task<IngestionPlan> BuildOrUpdateKnowledgeBaseAsync(string goal,
        CancellationToken cancellationToken = default)
    {
        IngestionPlan plan = await _planner.CreatePlanAsync(goal, cancellationToken)
            .ConfigureAwait(false);
        IngestionAgent ingestion = App.GetService<IngestionAgent>();


        foreach (IngestionTarget target in plan.Targets)
        {
            await ingestion.IngestAsync(
                new IngestionRequest(target.Uri.ToString(), SourceLabel: target.SourceLabel, Version: target.Version,
                    Category: target.Category), cancellationToken);
            var html = await _fetcher.GetStringAsync(target.Uri, cancellationToken)
                .ConfigureAwait(false);

            // For now, use your existing “skeleton” parser
            var text = _parser.ParseHtml(html);

            DocumentRecord doc = CreateDocumentRecord(
                target.Uri.ToString(),
                target.SourceLabel,
                target.Uri.ToString(),
                target.Version);

            await _ingester.IngestTextIntoDocumentAsync(doc, text, cancellationToken)
                .ConfigureAwait(false);
        }

        return plan;
    }





    private DocumentRecord CreateDocumentRecord(string externalId, string source, string title, string? version)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        return new DocumentRecord
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            Source = source,
            Title = title,
            Version = version,
            Status = "Processing",
            CreatedAt = now,
            UpdatedAt = now
        };
    }





#pragma warning disable SKEXP0110
    private AgentGroupChat BuildChatGroup()
    {
        // Define agents
        AgentDefinition agentReviewer = new()
        {
            Name = "Art-Director",
            Description = "An experienced art director who reviews and approves content plans.",
            Type = ""
        };

        // Create a chat for agent interaction.
        AgentGroupChat chat =
            new(new PlannerAgent(App.GetService<OnnxLLMClient>()))
            {
                ExecutionSettings =
                    new AgentGroupChatSettings
                    {
                        // Here a TerminationStrategy subclass is used that will terminate when
                        // an assistant message contains the term "approve".
                        TerminationStrategy = new ApprovalTerminationStrategy()

                    }
            };

        return chat;
    }
}