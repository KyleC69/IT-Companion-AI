// Project Name: SKAgent
// File Name: PlannerAgent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;


#pragma warning disable SKEXP0110


namespace ITCompanionAI.AgentFramework.Planning;


public interface IPlannerAgent
{
    Task<IngestionPlan> CreatePlanAsync(string goal, CancellationToken cancellationToken = default);







    IAsyncEnumerable<AgentResponseItem<ChatMessageContent>> InvokeAsync(ICollection<ChatMessageContent> messages,
        AgentThread? thread = null, AgentInvokeOptions? options = null, CancellationToken cancellationToken = default);







    IAsyncEnumerable<AgentResponseItem<StreamingChatMessageContent>> InvokeStreamingAsync(
        ICollection<ChatMessageContent> messages, AgentThread? thread = null, AgentInvokeOptions? options = null,
        CancellationToken cancellationToken = default);
}




public sealed class PlannerAgent : Agent, IPlannerAgent
{
    private readonly ILLMClient _llmClient;
    private readonly ChatHistory messagehistory = [];







    public PlannerAgent(ILLMClient llmClient)
    {
        _llmClient = llmClient;
    }







    public async Task<IngestionPlan> CreatePlanAsync(string goal, CancellationToken cancellationToken = default)
    {

        var prompt = BuildPlannerPrompt(goal);
        var rawResponse = await _llmClient.CompleteAsync(prompt, cancellationToken).ConfigureAwait(false);
        IngestionPlan plan = ParsePlan(goal, rawResponse);
        return plan;

    }







    public override async IAsyncEnumerable<AgentResponseItem<ChatMessageContent>> InvokeAsync(
        ICollection<ChatMessageContent> messages,
        AgentThread? thread = null,
        AgentInvokeOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        messagehistory.AddSystemMessage(BuildPlannerPrompt(
            "Produce a list of links to the API Definition web pages for the Microsoft Semantic Kernel."));

        var msg = BuildPlannerPrompt(
            "Produce a list of links to the API Definition web pages for the Microsoft Semantic Kernel.");
        var rawResponse = await _llmClient.CompleteAsync(msg, cancellationToken).ConfigureAwait(false);

        IngestionPlan plan = ParsePlan(msg, rawResponse);

        var responseText = JsonSerializer.Serialize(plan);
        yield return thread is null
            ? throw new ArgumentNullException(nameof(thread))
            : new AgentResponseItem<ChatMessageContent>(
                new ChatMessageContent(AuthorRole.Assistant, responseText),
                thread);
    }







    public override IAsyncEnumerable<AgentResponseItem<StreamingChatMessageContent>> InvokeStreamingAsync(
        ICollection<ChatMessageContent> messages, AgentThread? thread = null, AgentInvokeOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }







    public static IngestionPlan ParsePlanForTests(string goal, string rawResponse)
    {
        return ParsePlan(goal, rawResponse);
    }







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

        ReadOnlyCollection<IngestionTarget> targets = parsed.Targets
            .Where(t => !string.IsNullOrWhiteSpace(t.Uri))
            .Select(t => new IngestionTarget(
                new Uri(t.Uri, UriKind.Absolute),
                string.IsNullOrWhiteSpace(t.SourceLabel) ? "Web" : t.SourceLabel,
                t.Category,
                t.Version
            ))
            .ToList()
            .AsReadOnly();

        return new IngestionPlan(
            goal,
            targets
        );
    }







    private static string BuildPlannerPrompt(string goal)
    {
        var sb = new StringBuilder();

        sb.AppendLine("You are a planning assistant for building a Semantic Kernel knowledge base.");
        sb.AppendLine(
            "Your job is to select the most relevant online documentation URLs that are specific to your goal.");
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
        sb.AppendLine("Do NOT include explanations or any text outside the JSON.");

        return sb.ToString();
    }







    /// <summary>
    /// </summary>
    /// <param name="rawResponse"></param>
    /// <returns></returns>
    private static string ExtractFirstJsonObject(string rawResponse)
    {
        if (string.IsNullOrWhiteSpace(rawResponse))
        {
            return rawResponse;
        }

        var start = rawResponse.IndexOf('{');
        if (start < 0)
        {
            return rawResponse.Trim();
        }

        var inString = false;
        var escape = false;
        var depth = 0;

        for (var i = start; i < rawResponse.Length; i++)
        {
            var c = rawResponse[i];

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







    protected override IEnumerable<string> GetChannelKeys()
    {
        throw new NotImplementedException();
    }







    protected override Task<AgentChannel> CreateChannelAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }







    protected override Task<AgentChannel> RestoreChannelAsync(string channelState, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }







    private sealed class PlannerResponse
    {
        [JsonPropertyName("targets")] public List<PlannerTarget> Targets { get; set; } = [];
    }




    /// <summary>
    /// </summary>
    private sealed class PlannerTarget
    {
        [JsonPropertyName("uri")] public string Uri { get; } = string.Empty;

        [JsonPropertyName("sourceLabel")] public string? SourceLabel { get; [UsedImplicitly] set; }

        [JsonPropertyName("category")] public string? Category { get; set; }

        [JsonPropertyName("version")] public string? Version { get; set; }
    }
}




internal class IngestionTarget
{
}




public interface IngestionPlan
{
}