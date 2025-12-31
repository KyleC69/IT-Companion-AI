using System;
using System.Collections.Generic;
using System.Text;

using SkKnowledgeBase.Chunking;
using SkKnowledgeBase.Llm;
using SkKnowledgeBase.Models;
using SkKnowledgeBase.Parsing;
using SkKnowledgeBase.Storage;

// ============================================================================
// INGESTION: deterministic tools (URL/file/directory)
// ============================================================================

namespace SkKnowledgeBase.Ingestion;



public sealed record IngestionRequest(
    string? Url = null,
    string? FilePath = null,
    string? DirectoryPath = null,
    string? SourceLabel = null,
    string?  Category = null,
    string? Version = null
);

public interface IWebFetcher
{
    Task<string> GetStringAsync(Uri uri, CancellationToken cancellationToken = default);
}

public sealed class HttpWebFetcher : IWebFetcher
{
    private readonly HttpClient _httpClient;

    public HttpWebFetcher(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public Task<string> GetStringAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        if (uri is null)
        {
            throw new ArgumentNullException(nameof(uri));
        }

        return _httpClient.GetStringAsync(uri, cancellationToken);
    }
}

public interface IIngestionAgent
{
    Task IngestAsync(IngestionRequest request, CancellationToken cancellationToken = default);

    Task IngestTextIntoDocumentAsync(
        DocumentRecord document,
        string text,
        CancellationToken cancellationToken);
}

public sealed class IngestionAgent : IIngestionAgent
{
    private readonly IWebFetcher _webFetcher;
    private readonly IContentParser _parser;
    private readonly IChunker _chunker;
    private readonly IEmbeddingClient _embeddingClient;
    private readonly IVectorStore _vectorStore;

    public IngestionAgent(
        IWebFetcher webFetcher,
        IContentParser parser,
        IChunker chunker,
        IEmbeddingClient embeddingClient,
        IVectorStore vectorStore)
    {
        _webFetcher = webFetcher;
        _parser = parser;
        _chunker = chunker;
        _embeddingClient = embeddingClient;
        _vectorStore = vectorStore;
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task IngestAsync(IngestionRequest request, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(request.Url))
        {
            await IngestUrlAsync(request, cancellationToken).ConfigureAwait(false);
        }
        else if (!string.IsNullOrWhiteSpace(request.FilePath))
        {
            await IngestFileAsync(request, cancellationToken).ConfigureAwait(false);
        }
        else if (!string.IsNullOrWhiteSpace(request.DirectoryPath))
        {
            await IngestDirectoryAsync(request, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            throw new ArgumentException("IngestionRequest must have Url, FilePath, or DirectoryPath set.");
        }
    }





    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task IngestUrlAsync(IngestionRequest request, CancellationToken cancellationToken)
    {
        var uri = new Uri(request.Url!, UriKind.Absolute);
        var html = await _webFetcher.GetStringAsync(uri, cancellationToken).ConfigureAwait(false);
        var text = _parser.ParseHtml(html);

        var doc = CreateDocumentRecord(
            externalId: uri.ToString(),
            source: request.SourceLabel ?? "Web",
            title: uri.ToString(),
            category: request.Category ?? string.Empty,
            version: request.Version
        );

        await IngestTextIntoDocumentAsync(doc, text, cancellationToken).ConfigureAwait(false);
    }




    /// <summary>
    ///  Asynchronously ingests a single text or markup file specified in the <paramref name="request"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    private async Task IngestFileAsync(IngestionRequest request, CancellationToken cancellationToken)
    {
        var path = request.FilePath!;
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("File not found", path);
        }

        var text = File.ReadAllText(path);
        if (path.EndsWith(".html", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".htm", StringComparison.OrdinalIgnoreCase))
        {
            text = _parser.ParseHtml(text);
        }
        else if (path.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            text = _parser.ParseMarkdown(text);
        }

        var doc = CreateDocumentRecord(
            externalId: Path.GetFullPath(path),
            source: request.SourceLabel ?? "File",
            title: Path.GetFileName(path),
            category: request.Category ?? string.Empty,
            version: request.Version
        );

        await IngestTextIntoDocumentAsync(doc, text, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>
    /// Asynchronously ingests all supported text and markup files from the specified directory and its
    /// subdirectories.
    /// </summary>
    /// <remarks>Only files with the extensions .html, .htm, .md, or .txt are ingested. The operation
    /// processes files recursively in all subdirectories. The method supports cancellation via the provided
    /// <paramref name="cancellationToken"/>.</remarks>
    /// <param name="request">An object containing the ingestion parameters, including the path of the directory to process.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous ingestion operation.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown if the directory specified in <paramref name="request"/> does not exist.</exception>
    private async Task IngestDirectoryAsync(IngestionRequest request, CancellationToken cancellationToken)
    {
        var dir = request.DirectoryPath!;
        if (!Directory.Exists(dir))
        {
            throw new DirectoryNotFoundException(dir);
        }

        var files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
            .Where(f => f.EndsWith(".html", StringComparison.OrdinalIgnoreCase)
                     || f.EndsWith(".htm", StringComparison.OrdinalIgnoreCase)
                     || f.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
                     || f.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await IngestFileAsync(request with { FilePath = file, DirectoryPath = null }, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private DocumentRecord CreateDocumentRecord(string externalId, string source, string title, string category,string? version)
    {
        var now = DateTimeOffset.UtcNow;
        return new DocumentRecord
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            Source = source,
            Title = title,
            Category = category,
            Version = version,
            Status = "Processing",
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public async Task IngestTextIntoDocumentAsync(
        DocumentRecord document,
        string text,
        CancellationToken cancellationToken)
    {
        try
        {
            document = await _vectorStore.UpsertDocumentAsync(document, cancellationToken).ConfigureAwait(false);

            var chunks = _chunker.Chunk(text);
            var records = new List<ChunkRecord>(chunks.Count);

            foreach (var chunk in chunks)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var embedding = await _embeddingClient.EmbedAsync(chunk.Text, cancellationToken).ConfigureAwait(false);

                records.Add(new ChunkRecord
                {
                    Id = Guid.NewGuid(),
                    DocumentId = document.Id,
                    ChunkIndex = chunk.Index,
                    Text = chunk.Text,
                    TokenCount = chunk.TokenCount,
                    Embedding = embedding,
                    Section = chunk.Section,
                    Symbol = chunk.Symbol,
                    Kind = chunk.Kind,
                    Verified = false,
                    Confidence = 0,
                    Deprecated = false
                });
            }

            await _vectorStore.UpsertChunksAsync(document.Id, records, cancellationToken).ConfigureAwait(false);

            document.Status = "Complete";
            document.UpdatedAt = DateTimeOffset.UtcNow;
            document.LastError = null;
            await _vectorStore.UpsertDocumentAsync(document, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            document.Status = "Failed";
            document.UpdatedAt = DateTimeOffset.UtcNow;
            document.LastError = ex.ToString();
            await _vectorStore.UpsertDocumentAsync(document, cancellationToken).ConfigureAwait(false);
            throw;
        }
    }
}


