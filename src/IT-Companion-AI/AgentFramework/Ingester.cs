// ============================================================================
// COMPLETE, SELF-CONTAINED C# LIBRARY
// Deterministic ingestion + ONNX embeddings + ONNX LLM + pgvector + tokenizers
// Only configuration: paths to ONNX models and tokenizer files, and DB string.
// ============================================================================
//
// NuGet packages you will need:
//
// - Npgsql
// - NpgsqlVector
// - HtmlAgilityPack
// - Microsoft.ML.OnnxRuntime
// - Microsoft.ML.Tokenizers
// - System.Text.Json
//
// Postgres extension:
//
//   CREATE EXTENSION IF NOT EXISTS vector;
//
// ============================================================================
// MODELS
// ============================================================================



using SkKnowledgeBase.Chunking;
using SkKnowledgeBase.Llm;
using SkKnowledgeBase.Models;
using SkKnowledgeBase.Parsing;
using HFTokenizer=Tokenizers.HuggingFace.Tokenizer;


using Microsoft.ML.Tokenizers;

using Microsoft.Extensions.DependencyInjection;
using SkKnowledgeBase.Ingestion;
using SkKnowledgeBase.Agents;
using SkKnowledgeBase.Query;

using Microsoft.Extensions.Hosting;


namespace SkKnowledgeBase.Models
{
    public sealed class DocumentRecord
    {
        public Guid Id { get; init; }
        public string ExternalId { get; init; } = default!;
        public string Source { get; init; } = default!;
        public string Title { get; init; } = default!;
        public string? Version { get; init; }
        public string Status { get; set; } = "Pending"; // Pending, Processing, Complete, Failed
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string? LastError { get; set; }
        public string Category { get; internal set; }
    }

    public sealed class ChunkRecord
    {
        public Guid Id { get; init; }
        public Guid DocumentId { get; init; }
        public int ChunkIndex { get; init; }
        public string Text { get; init; } = default!;
        public int TokenCount { get; init; }
        public float[] Embedding { get; init; } = default!;
        public string? Section { get; init; }
        public string? Symbol { get; init; }
        public string? Kind { get; init; }
        public bool Verified { get; set; }
        public double Confidence { get; set; }
        public bool Deprecated { get; set; }
        public string? Category { get; internal set; }
    }

    public sealed class ReconciledChunkRecord
    {
        public Guid Id { get; init; }
        public string Symbol { get; init; } = default!;
        public string? Namespace { get; init; }
        public string? Version { get; init; }
        public string Summary { get; init; } = default!;
        public float[] Embedding { get; init; } = default!;
        public double Confidence { get; init; }
        public int SourceCount { get; init; }
    }
}




public class Ingester
{
    public IServiceCollection ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
       // var connectionString = "(localdb)\\MSSqlLocalDB;Database=AIAgentRag";
        var embeddingModelPath = """D:\\Solutions\\SolHack\\RepoRoot\\src\\IT-Companion-AI\\AIModels\\bge-small\\model.onnx""";
        var vocabPath = """D:\\Solutions\\SolHack\\RepoRoot\\src\\IT-Companion-AI\\AIModels\\bge-small\\tokenizer.json""";
        //var mergesPath = """D:\\Solutions\\SolHack\\RepoRoot\\src\\IT-Companion-AI\\AIModels\\bge-small\\merges.txt""";

        var llmModelPath = """D:\\cpu-int4-rtn-block-32\\phi3-mini-4k-instruct-cpu-int4-rtn-block-32.onnx""";
        var llmTokenizerJson = """D:\\Solutions\\SolHack\\RepoRoot\\src\\IT-Companion-AI\\AIModels\\Phi3\\tokenizer.json""";



  

        services.AddKeyedSingleton<HFTokenizer.Tokenizer>("embedding", (sp, _) =>
        {
            return HFTokenizer.Tokenizer.FromFile(vocabPath);
        });

        services.AddSingleton<IChunker>(sp =>
            new TokenizerChunker(sp.GetRequiredKeyedService<HFTokenizer.Tokenizer>("embedding"), maxTokens: 512));

            services.AddKeyedSingleton<Tokenizer>("llm", (sp, _) =>
        {
            using (var modelStream = File.OpenRead(@"d:\cpu-int4-rtn-block-32\tokenizer.model"))
            {
                return LlamaTokenizer.Create(modelStream);
            }
        });

        services.AddSingleton<IContentParser, HtmlMarkdownContentParser>();




        services.AddSingleton<IEmbeddingClient>(sp =>
            new OnnxEmbeddingClient(
                modelPath: embeddingModelPath,
                tokenizer: sp.GetRequiredKeyedService<HFTokenizer.Tokenizer>("embedding"),
                maxTokens: 512,
                inputIdsName: "input_ids",
                attentionMaskName: "attention_mask",
                outputName: "last_hidden_state",
                embeddingDim: 384));

        services.AddSingleton<ILLMClient>(sp =>
            new OnnxLLMClient(
                modelPath: llmModelPath,
                tokenizer: sp.GetRequiredKeyedService<Tokenizer>("llm"),
                maxNewTokens: 256,
                inputIdsName: "input_ids",
                outputName: "logits"));

        /*
        services.AddSingleton<IVectorStore>(sp =>
        {
            var store = new PgVectorStore(connectionString, embeddingDim: 384);
            store.EnsureSchemaAsync().GetAwaiter().GetResult();
            return store;
        });
        */
        services.AddSingleton<IngestionAgent>();
        services.AddSingleton<VerificationAgent>();
        services.AddSingleton<ReconciliationAgent>();
        services.AddSingleton<KnowledgeQueryService>();

        return services;
    }
}




/*
var host = builder.Build();

// Example usage (e.g., from a hosted service or controller):
// var ingestion = host.Services.GetRequiredService<IngestionAgent>();
// await ingestion.IngestAsync(new IngestionRequest(Url: "https://learn.microsoft.com/en-us/semantic-kernel/agents"));
// var verifier = host.Services.GetRequiredService<VerificationAgent>();
// await verifier.VerifySymbolAsync("Microsoft.SemanticKernel.Agents.Agent");
// var recon = host.Services.GetRequiredService<ReconciliationAgent>();
// await recon.ReconcileSymbolAsync("Microsoft.SemanticKernel.Agents.Agent");
// var queryService = host.Services.GetRequiredService<KnowledgeQueryService>();
// var result = await queryService.QueryAsync("How do SK agents work?");
// Console.WriteLine(result.Answer);

await host.RunAsync();
*/// var queryService = host.Services.GetRequiredService<KnowledgeQueryService>();
// var result = await queryService.QueryAsync("How do SK agents work?");
// Console.WriteLine(result.Answer);

