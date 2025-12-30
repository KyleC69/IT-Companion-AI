using System;
using System.Collections.Generic;
using System.Text;
    using SkKnowledgeBase.Llm;
    using SkKnowledgeBase.Models;
    using SkKnowledgeBase.Storage;



// ============================================================================
// QUERY SERVICE (for your web chat)
// ============================================================================

namespace SkKnowledgeBase.Query
{

    public sealed record QueryResult(
        string Answer,
        IReadOnlyList<ReconciledChunkRecord> SupportingChunks
    );

    public sealed class KnowledgeQueryService
    {
        private readonly IEmbeddingClient _embeddingClient;
        private readonly IVectorStore _vectorStore;
        private readonly ILLMClient _llmClient;

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

            var reconciled = await _vectorStore.SearchReconciledAsync(queryEmbedding, topK: 10, cancellationToken)
                .ConfigureAwait(false);

            var sb = new StringBuilder();
            sb.AppendLine("You are an assistant that answers questions about the Semantic Kernel API.");
            sb.AppendLine("Use the following information as authoritative context:");
            sb.AppendLine();
            foreach (var chunk in reconciled)
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
                Answer: answer.Trim(),
                SupportingChunks: reconciled
            );
        }
    }
}

