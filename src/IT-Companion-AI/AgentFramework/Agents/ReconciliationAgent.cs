using System.Text;

using ITCompanionAI.AgentFramework;
using ITCompanionAI.AgentFramework.Storage;



// ============================================================================
// AGENTS: Verification + Reconciliation
// ============================================================================

namespace ITCompanionAI.AgentFramework.Agents;

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

