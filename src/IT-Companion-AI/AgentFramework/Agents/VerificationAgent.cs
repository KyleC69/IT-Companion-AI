// Project Name: SKAgent
// File Name: VerificationAgent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Text;
using System.Text.Json;

using ITCompanionAI.AgentFramework.Storage;


// ============================================================================
// AGENTS: Verification + Reconciliation
// ============================================================================


namespace ITCompanionAI.AgentFramework.Agents;


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
        IReadOnlyList<ChunkRecord> chunks =
            await _vectorStore.GetChunksBySymbolAsync(symbol, cancellationToken).ConfigureAwait(false);
        if (chunks.Count == 0)
        {
            return;
        }

        StringBuilder sb = new();
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

        foreach (ChunkRecord chunk in chunks)
        {
            chunk.Verified = result.Verified;
            chunk.Confidence = result.Confidence;
            chunk.Deprecated = result.Deprecated;
        }

        IEnumerable<IGrouping<Guid, ChunkRecord>> byDoc = chunks.GroupBy(c => c.DocumentId);
        foreach (IGrouping<Guid, ChunkRecord> group in byDoc)
            await _vectorStore.UpsertChunksAsync(group.Key, group.ToList(), cancellationToken)
                .ConfigureAwait(false);
    }
}