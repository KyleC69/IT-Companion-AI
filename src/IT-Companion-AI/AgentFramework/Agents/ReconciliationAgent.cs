// Project Name: SKAgent
// File Name: ReconciliationAgent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Text;

using ITCompanionAI.AgentFramework.Storage;


// ============================================================================
// AGENTS: Verification + Reconciliation
// ============================================================================


namespace ITCompanionAI.AgentFramework.Agents;


public sealed class ReconciliationAgent
{
    private readonly IEmbeddingClient _embeddingClient;
    private readonly ILLMClient _llmClient;
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
        IReadOnlyList<ChunkRecord> chunks =
            await _vectorStore.GetChunksBySymbolAsync(symbol, cancellationToken).ConfigureAwait(false);
        if (chunks.Count == 0)
        {
            return;
        }

        StringBuilder sb = new();
        sb.AppendLine("You are reconciling Semantic Kernel API documentation.");
        sb.AppendLine($"Symbol: {symbol}");
        sb.AppendLine();
        sb.AppendLine("Here are verified or unverified fragments:");
        foreach (ChunkRecord chunk in chunks)
        {
            sb.AppendLine(
                $"[Verified={chunk.Verified}, Confidence={chunk.Confidence:F2}, Deprecated={chunk.Deprecated}]");
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

        ReconciledChunkRecord reconciled = new()
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