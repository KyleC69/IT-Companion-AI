using System;
using System.Collections.Generic;
using System.Text;

using ITCompanionAI;
using ITCompanionAI.AgentFramework;
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
    private readonly IPlannerAgent _planner;
    private readonly IWebFetcher _fetcher;
    private readonly IContentParser _parser;
    private readonly IIngestionAgent _ingester;

    public KnowledgeIngestionOrchestrator(IPlannerAgent planner, IWebFetcher fetcher, IContentParser parser, IIngestionAgent ingester)
    {
        _planner = planner;
        _fetcher = fetcher;
        _parser = parser;
        _ingester = ingester;
    }

    public async Task<IngestionPlan> BuildOrUpdateKnowledgeBaseAsync(string goal, CancellationToken cancellationToken = default)
    {
        var plan = await _planner.CreatePlanAsync(goal, cancellationToken)
            .ConfigureAwait(false);
        var ingestion = App.GetService<IngestionAgent>();
      
        
        foreach (var target in plan.Targets)
        {
            await ingestion.IngestAsync(new IngestionRequest(target.Uri.ToString(), SourceLabel: target.SourceLabel, Version: target.Version, Category: target.Category), cancellationToken);
            var html = await _fetcher.GetStringAsync(target.Uri, cancellationToken)
                .ConfigureAwait(false);

            // For now, use your existing “skeleton” parser
            var text = _parser.ParseHtml(html);

            var doc = CreateDocumentRecord(
                externalId: target.Uri.ToString(),
                source: target.SourceLabel,
                title: target.Uri.ToString(),
                version: target.Version);

            await _ingester.IngestTextIntoDocumentAsync(doc, text, cancellationToken)
                .ConfigureAwait(false);
        }

        return plan;
    }



    private DocumentRecord CreateDocumentRecord(string externalId, string source, string title, string? version)
    {
        var now = DateTimeOffset.UtcNow;
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

    private AgentGroupChat BuildChatGroup()
    {
        // Define agents
        AgentDefinition agentReviewer = new()
        {
            Name = "Art-Director",
            Description = "An experienced art director who reviews and approves content plans.",
            Type= "",
        };

        // Create a chat for agent interaction.
        AgentGroupChat chat =
            new(new PlannerAgent(App.GetService<OnnxLLMClient>()))
            {
                ExecutionSettings =
                    new()
                    {
                        // Here a TerminationStrategy subclass is used that will terminate when
                        // an assistant message contains the term "approve".
                        TerminationStrategy =  new ApprovalTerminationStrategy()

                    }
            };

        return chat;
    }





































}