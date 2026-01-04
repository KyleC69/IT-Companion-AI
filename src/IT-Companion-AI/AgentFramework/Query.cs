// Project Name: SKAgent
// File Name: Query.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Text;

using ITCompanionAI.AgentFramework.Storage;


// ============================================================================
// QUERY SERVICE (for your web chat)
// ============================================================================


namespace ITCompanionAI.AgentFramework;


public sealed record QueryResult(
    string Answer,
    IReadOnlyList<ReconciledChunkRecord> SupportingChunks
);



public sealed class KnowledgeQueryService
{
    private readonly IEmbeddingClient _embeddingClient;
    private readonly ILLMClient _llmClient;
    private readonly IVectorStore _vectorStore;





    public KnowledgeQueryService(
        IEmbeddingClient embeddingClient,
        IVectorStore vectorStore,
        ILLMClient llmClient)
    {
        _embeddingClient = embeddingClient;
        _vectorStore = vectorStore;
        _llmClient = llmClient;
    }





    public async Task<QueryResult> QueryAsync(
        string question,
        CancellationToken cancellationToken = default)
    {
        var queryEmbedding = await _embeddingClient.EmbedAsync(question, cancellationToken)
            .ConfigureAwait(false);

        IReadOnlyList<ReconciledChunkRecord> reconciled = await _vectorStore
            .SearchReconciledAsync(queryEmbedding, 10, cancellationToken)
            .ConfigureAwait(false);

        var sb = new StringBuilder();
        sb.AppendLine("You are an assistant that answers questions about the Semantic Kernel API.");
        sb.AppendLine("Use the following information as authoritative context:");
        sb.AppendLine();
        foreach (ReconciledChunkRecord chunk in reconciled)
        {
            sb.AppendLine($"[Symbol={chunk.Symbol}, Confidence={chunk.Confidence:F2}]");
            sb.AppendLine(chunk.Summary);
            sb.AppendLine();
        }

        sb.AppendLine("Question:");
        sb.AppendLine(question);
        sb.AppendLine();
        sb.AppendLine("Answer clearly and concisely, citing relevant symbols and namespaces where helpful.");

        var answer = await _llmClient.CompleteAsync(sb.ToString(), cancellationToken)
            .ConfigureAwait(false);

        return new QueryResult(
            answer.Trim(),
            reconciled
        );
    }
}