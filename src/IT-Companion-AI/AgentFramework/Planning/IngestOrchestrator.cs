using System;
using System.Collections.Generic;
using System.Text;

using ITCompanionAI;

using SkKnowledgeBase.Agents.Planning;
using SkKnowledgeBase.Ingestion;
using SkKnowledgeBase.Models;
using SkKnowledgeBase.Parsing;

namespace SkKnowledgeBase.AgentFramework.Planning;

public sealed class KnowledgeIngestionOrchestrator
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
            await ingestion.IngestAsync(new IngestionRequest(target.Uri.ToString(),SourceLabel: target.SourceLabel,Version: target.Version,Category: target.Category), cancellationToken);
            var html = await _fetcher.GetStringAsync (target.Uri, cancellationToken)
                .ConfigureAwait(false);

            // For now, use your existing “skeleton” parser
            var text = _parser.ParseHtml(html);

            var doc =  CreateDocumentRecord(
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
}