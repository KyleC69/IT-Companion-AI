using System;
using System.Collections.Generic;
using System.Text;



using System.Text.Json;
using System.Text.Json.Serialization;

using SkKnowledgeBase.Llm;

using SkKnowledgeBase.Query; // for ILLMClient

namespace SkKnowledgeBase.Agents.Planning;


public interface IPlannerAgent
{
    Task<IngestionPlan> CreatePlanAsync(string goal, CancellationToken cancellationToken = default);
}

public sealed class PlannerAgent : IPlannerAgent
{
    private readonly ILLMClient _llmClient;

    public PlannerAgent(ILLMClient llmClient)
    {
        _llmClient = llmClient;
    }

    public async Task<IngestionPlan> CreatePlanAsync(string goal, CancellationToken cancellationToken = default)
    {
        var prompt = BuildPlannerPrompt(goal);

        var rawResponse = await _llmClient
            .CompleteAsync(prompt, cancellationToken)
            .ConfigureAwait(false);

        var plan = ParsePlan(goal, rawResponse);

        return plan;
    }

    internal static IngestionPlan ParsePlanForTests(string goal, string rawResponse) => ParsePlan(goal, rawResponse);

    private static IngestionPlan ParsePlan(string goal, string rawResponse)
    {
        // Very small helper DTO for JSON parsing
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var json = ExtractFirstJsonObject(rawResponse);

        PlannerResponse? parsed;
        try
        {
            parsed = JsonSerializer.Deserialize<PlannerResponse>(json, options);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"Planner returned invalid JSON. Raw response:\n{rawResponse}", ex);
        }

        if (parsed?.Targets is null || parsed.Targets.Count == 0)
        {
            throw new InvalidOperationException(
                $"Planner returned no targets. Raw response:\n{rawResponse}");
        }

        var targets = parsed.Targets
            .Where(t => !string.IsNullOrWhiteSpace(t.Uri))
            .Select(t => new IngestionTarget(
                Uri: new Uri(t.Uri, UriKind.Absolute),
                SourceLabel: string.IsNullOrWhiteSpace(t.SourceLabel) ? "Web" : t.SourceLabel,
                Category: t.Category,
                Version: t.Version
            ))
            .ToList()
            .AsReadOnly();

        return new IngestionPlan(
            Goal: goal,
            Targets: targets
        );
    }

    private static string BuildPlannerPrompt(string goal)
    {
        var sb = new StringBuilder();

        sb.AppendLine("You are a planning assistant for building a Semantic Kernel knowledge base.");
        sb.AppendLine("Your job is to select the most relevant online documentation URLs to ingest.");
        sb.AppendLine();
        sb.AppendLine("Goal:");
        sb.AppendLine(goal);
        sb.AppendLine();
        sb.AppendLine("Only consider official or authoritative sources like:");
        sb.AppendLine("- https://learn.microsoft.com/");
        sb.AppendLine("- https://github.com/microsoft/semantic-kernel/");
        sb.AppendLine();
        sb.AppendLine("Respond ONLY with compact JSON in the following format:");
        sb.AppendLine();
        sb.AppendLine("{");
        sb.AppendLine("  \"targets\": [");
        sb.AppendLine("    {");
        sb.AppendLine("      \"uri\": \"https://...\",");
        sb.AppendLine("      \"sourceLabel\": \"Web\",");
        sb.AppendLine("      \"category\": \"Planner\",");
        sb.AppendLine("      \"version\": \"v1.30+\"");
        sb.AppendLine("    }");
        sb.AppendLine("  ]");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("Do not include explanations or any text outside the JSON.");

        return sb.ToString();
    }

    private static string ExtractFirstJsonObject(string rawResponse)
    {
        if (string.IsNullOrWhiteSpace(rawResponse))
        {
            return rawResponse;
        }

        int start = rawResponse.IndexOf('{');
        if (start < 0)
        {
            return rawResponse.Trim();
        }

        bool inString = false;
        bool escape = false;
        int depth = 0;

        for (int i = start; i < rawResponse.Length; i++)
        {
            char c = rawResponse[i];

            if (inString)
            {
                if (escape)
                {
                    escape = false;
                    continue;
                }

                if (c == '\\')
                {
                    escape = true;
                    continue;
                }

                if (c == '"')
                {
                    inString = false;
                }

                continue;
            }

            if (c == '"')
            {
                inString = true;
                continue;
            }

            if (c == '{')
            {
                depth++;
            }
            else if (c == '}')
            {
                depth--;
                if (depth == 0)
                {
                    return rawResponse.Substring(start, i - start + 1);
                }
            }
        }

        return rawResponse.Substring(start).Trim();
    }

    private sealed class PlannerResponse
    {
        [JsonPropertyName("targets")]
        public List<PlannerTarget> Targets { get; set; } = new();
    }

    private sealed class PlannerTarget
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; } = string.Empty;

        [JsonPropertyName("sourceLabel")]
        public string? SourceLabel { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }
    }
}