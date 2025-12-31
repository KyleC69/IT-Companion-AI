using System;
using System.Collections.Generic;
using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using SkKnowledgeBase.Llm;
    using SkKnowledgeBase.Models;
    using SkKnowledgeBase.Storage;


// ============================================================================
// AGENTS: Verification + Reconciliation
// ============================================================================

namespace SkKnowledgeBase.Agents;


public sealed class VerificationResult
{
    [JsonPropertyName("verified")]
    public bool Verified { get; init; }

    [JsonPropertyName("confidence")]
    public double Confidence { get; init; }

    [JsonPropertyName("deprecated")]
    public bool Deprecated { get; init; }

    [JsonPropertyName("notes")]
    public string Notes { get; init; } = string.Empty;
}

public sealed class VerificationAgent
{
    private readonly ILLMClient _llmClient;
    private readonly IVectorStore _vectorStore;

    public VerificationAgent(ILLMClient llmClient, IVectorStore vectorStore)
    {
        _llmClient = llmClient;
        _vectorStore = vectorStore;
    }

    public async Task VerifySymbolAsync(
        string symbol,
        CancellationToken cancellationToken = default)
    {
        var chunks = await _vectorStore.GetChunksBySymbolAsync(symbol, cancellationToken).ConfigureAwait(false);
        if (chunks.Count == 0)
        {
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine("You are checking consistency of Semantic Kernel API information.");
        sb.AppendLine($"Symbol: {symbol}");
        sb.AppendLine();
        sb.AppendLine("Here are text fragments about this symbol:");
        for (var i = 0; i < chunks.Count; i++)
        {
            sb.AppendLine($"[Fragment {i}]");
            sb.AppendLine(chunks[i].Text);
            sb.AppendLine();
        }

        sb.AppendLine("""
        Task:
        - Determine if fragments agree on the symbol's namespace, type (class/method/etc.), and usage.
        - Identify clearly outdated or conflicting information.
        - Return JSON with fields:
          - verified: bool
          - confidence: number between 0 and 1
          - deprecated: bool
          - notes: string
        """);

        var json = await _llmClient.CompleteAsync(sb.ToString(), cancellationToken).ConfigureAwait(false);

        VerificationResult result;
        try
        {
            result = JsonSerializer.Deserialize<VerificationResult>(json,
                         new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                     ?? new VerificationResult { Verified = false, Confidence = 0, Deprecated = false };
        }
        catch
        {
            result = new VerificationResult
            {
                Verified = false,
                Confidence = 0,
                Deprecated = false,
                Notes = "Failed to parse verification JSON."
            };
        }

        foreach (var chunk in chunks)
        {
            chunk.Verified = result.Verified;
            chunk.Confidence = result.Confidence;
            chunk.Deprecated = result.Deprecated;
        }

        var byDoc = chunks.GroupBy(c => c.DocumentId);
        foreach (var group in byDoc)
        {
            await _vectorStore.UpsertChunksAsync(group.Key, group.ToList(), cancellationToken)
                .ConfigureAwait(false);
        }
    }
}

public sealed class ReconciliationAgent
{
    private readonly ILLMClient _llmClient;
    private readonly IEmbeddingClient _embeddingClient;
    private readonly IVectorStore _vectorStore;

    public ReconciliationAgent(
        ILLMClient llmClient,
        IEmbeddingClient embeddingClient,
        IVectorStore vectorStore)
    {
        _llmClient = llmClient;
        _embeddingClient = embeddingClient;
        _vectorStore = vectorStore;
    }

    public async Task ReconcileSymbolAsync(
        string symbol,
        CancellationToken cancellationToken = default)
    {
        var chunks = await _vectorStore.GetChunksBySymbolAsync(symbol, cancellationToken).ConfigureAwait(false);
        if (chunks.Count == 0)
        {
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine("You are reconciling Semantic Kernel API documentation.");
        sb.AppendLine($"Symbol: {symbol}");
        sb.AppendLine();
        sb.AppendLine("Here are verified or unverified fragments:");
        foreach (var chunk in chunks)
        {
            sb.AppendLine($"[Verified={chunk.Verified}, Confidence={chunk.Confidence:F2}, Deprecated={chunk.Deprecated}]");
            sb.AppendLine(chunk.Text);
            sb.AppendLine();
        }

        sb.AppendLine("""
        Task:
        - Produce a single, accurate summary of this symbol:
          - What it is (class/method/etc.)
          - Which namespace it belongs to
          - Its purpose
          - Important parameters/usage notes
        - Assume newer, verified information is more trustworthy.
        - If you suspect deprecation, clearly note it.
        - Return only the summary text.
        """);

        var summary = await _llmClient.CompleteAsync(sb.ToString(), cancellationToken).ConfigureAwait(false);
        summary = summary.Trim();

        var embedding = await _embeddingClient.EmbedAsync(summary, cancellationToken).ConfigureAwait(false);

        var reconciled = new ReconciledChunkRecord
        {
            Id = Guid.NewGuid(),
            Symbol = symbol,
            Namespace = null,
            Version = null,
            Summary = summary,
            Embedding = embedding,
            Confidence = ComputeAggregateConfidence(chunks),
            SourceCount = chunks.Count
        };

        await _vectorStore.UpsertReconciledChunkAsync(reconciled, cancellationToken).ConfigureAwait(false);
    }

    private static double ComputeAggregateConfidence(IReadOnlyList<ChunkRecord> chunks)
    {
        if (chunks.Count == 0)
        {
            return 0;
        }

        var sum = chunks.Sum(c => c.Confidence);
        return sum / chunks.Count;
    }
}

