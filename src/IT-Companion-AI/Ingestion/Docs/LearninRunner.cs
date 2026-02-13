using ITCompanionAI.Ingestion.Services;

using Markdig;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Microsoft.Extensions.AI;
using Microsoft.Extensions.DataIngestion;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.ML.Tokenizers;

using OllamaSharp;




namespace ITCompanionAI.Ingestion.Docs;





public sealed class LearninRunner
{



    public enum sourceMode
    {
        Web, FileSystem

    }





    private const string startingPath = "e:\\ingestionsource\\dotnet\\docs\\docs\\docs";
    private const string testFile = @"E:\\IngestionSource\\dotnet\\docs\\docs\\docs\\ai\\ichatclient.md";





    private static readonly System.Diagnostics.TraceSource Log = new("DocsIngestion", System.Diagnostics.SourceLevels.All);

    private static readonly Lazy<Task<Tokenizer>> TokenizerFactory = new(CreateTokenizerAsync);
    private readonly IChatClient _chatClient;
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embed;
    private readonly ILogger<LearninRunner> _logger = App.GetService<ILogger<LearninRunner>>();

    private readonly Lazy<LearnPageParser> _parser = new();
    private readonly Lazy<DocRepository> _repository = new();
    private readonly Lazy<KBContext> dbContext = new();








    public LearninRunner()
    {
        _ = App.GetService<IOptions<IngestionSettings>>().Value;
        _embed = new OllamaApiClient(new Uri("http://127.0.0.1:11434"), "mxbai-embed-large-v1");
        _chatClient = new OllamaApiClient(new Uri("http://127.0.0.1:11434"), "mxbai-embed-large-v1");
        _logger.LogDebug("LearninRunner initialized with OllamaApiClient for embeddings and chat.");
    }








    public static sourceMode SourceMode { get; set; }
    public bool IsRunning { get; private set; }








    private static async Task<Tokenizer> CreateTokenizerAsync()
    {
        return await BertTokenizer.CreateAsync(@"f:\\ai-models\\mxbai-embed-large-v1\\vocab.txt");
    }








    public async Task IngestDocumentAsync(string filePath, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        ArgumentException.ThrowIfNullOrEmpty(filePath);
        MarkdownParserContext context = new();




        var markdown = await File.ReadAllTextAsync(testFile, token);


        var textdoc = Markdown.ToPlainText(markdown, null, context);



        _logger.LogDebug("Document read successfully. Starting chunking process...");

        IngestionDocumentReader reader = new MarkdownReader();

        //  IngestionDocument dic = await reader.ReadAsync(stream, filePath, "text/markdown", token);

        IngestionChunkerOptions options = new(await TokenizerFactory.Value)
        {
                MaxTokensPerChunk = 1000
        };

        IngestionChunkWriter<string> writer = new MyIngestionChunkWriter
        {
                // Implementation of the writer that persists the enriched chunks to the database using KBContext and stored procedures.
        };

        HeaderChunker chunker = new(new IngestionChunkerOptions(await TokenizerFactory.Value)
        {
                MaxTokensPerChunk = 1000
        });

        SummaryEnricher summaryEnricher = new(new EnricherOptions(_chatClient)
        {
                LoggerFactory = LoggerFactory.Create(builder => builder.AddConsole())
        }, 150);


        KeywordEnricher keywordEnricher = new(new EnricherOptions(_chatClient)
        {
                LoggerFactory = LoggerFactory.Create(builder => builder.AddConsole())
        }, null, 5, 0.7f);

        using var pipeline = new IngestionPipeline<string>(reader, chunker, writer, loggerFactory: App.GetService<ILoggerFactory>())
        {
                ChunkProcessors = { summaryEnricher, keywordEnricher }
        };

        var mdFilesEnumerable = Directory.EnumerateFiles(startingPath, "*.md", SearchOption.AllDirectories);

        //   await foreach (IngestionResult result in pipeline.ProcessAsync(Directory.EnumerateFiles(testFile, "*.md",SearchOption.AllDirectories),token)) 
        {
            //     _logger.LogDebug($"Completed processing '{result.DocumentId}'. Succeeded: '{result.Succeeded}'.");
        }



        _logger.LogTrace("Ingestion pipeline processing completed.");

    }








    private (IEnumerable<string> mdFilesEnumerable, IEnumerable<string> csFilesEnumerable) BuildTocAsync(string startingPath, CancellationToken token)
    {
        try
        {

            var mdFilesEnumerable = Directory.EnumerateFiles(startingPath, "*.md", SearchOption.AllDirectories);
            var csFilesEnumerable = Directory.EnumerateFiles(startingPath, "*.cs", SearchOption.AllDirectories);

            return (mdFilesEnumerable, csFilesEnumerable);
        }
        catch (Exception ex)
        {
            Log.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0, "Error building TOC from Url={0} Error={1}", startingPath, ex.Message);
            Console.WriteLine(ex.Message);
        }

        return (Enumerable.Empty<string>(), Enumerable.Empty<string>());
    }








    /// <summary>
    ///     Processes and ingests a collection of URLs by parsing their content, persisting the parsed results,
    ///     and performing additional operations such as chunking and embedding generation.
    /// </summary>
    /// <param name="urls">An array of URLs to be ingested.</param>
    /// <param name="sourceSnapshotId">The unique identifier of the source snapshot associated with the ingestion.</param>
    /// <param name="ingestionRunId">The unique identifier of the ingestion run.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="urls" /> parameter is null.</exception>
    /// <remarks>
    ///     This method handles errors gracefully by logging warnings and continuing with the next URL in case of failures.
    ///     It also includes throttling mechanisms to avoid overwhelming the server.
    /// </remarks>
    public async Task IngestDocumentAsync(string Startingpath, Guid? sourceSnapshotId, Guid? ingestionRunId, CancellationToken cancellationToken = default)
    {
        LearnPageParser _FileParser = App.GetService<LearnPageParser>();

        if (Startingpath == null)
        {
            throw new ArgumentNullException(nameof(Startingpath));
        }


        cancellationToken.ThrowIfCancellationRequested();

        LearnPageParseResult result = new();
        try
        {
            {
                result = await _parser.Value.ParseAsync(Startingpath, ingestionRunId, sourceSnapshotId, cancellationToken);
            }
        }
        catch (HttpRequestException e)
        {
            Log.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0, "Fetch failed Url={0} Error={1}", Startingpath, e.Message);
            Console.WriteLine(e.Message);
            // continue; //Prevent premature exit from ingestion run on single url fetch failure
        }

        //##############################
        //persist the parsed result
        try
        {
            await _repository.Value.InsertPageAsync(result);
        }
        catch (Exception ex)
        {
            Log.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0, "Fetch failed Url={0} Error={1}", Startingpath, ex.Message);
            Console.WriteLine(ex.Message);
        }




    }








    public async Task RunAsync(string startingPath, CancellationToken token = default)
    {
#if DEBUG
        var testFile = @"E:\\IngestionSource\\dotnet\\docs\\docs\\docs\\ai\\ichatclient.md";
        Guid ingestionRunId = Guid.Empty;
        Guid snapshotID = Guid.Empty;
        await IngestDocumentAsync(testFile, snapshotID, ingestionRunId, token);
#else
        var (ingestionRunId, snapshotID) = await InitiateDataRunAsync(startingPath, token);
        await IngestDocumentAsync(startingPath, snapshotID, ingestionRunId, token);
        await CompleteIngestionRunAsync(dbContext.Value, ingestionRunId, token);
#endif

    }








    /*


    /// <summary>
    ///     Initiates and manages the document ingestion process, including fetching, parsing,
    ///     chunking, and enriching documents from a specified starting URL.
    /// </summary>
    /// <param name="startingPath">
    ///     The file system path or web URL from which the ingestion process begins. This is used to fetch the initial
    ///     document or table of contents for processing.
    /// </param>
    /// <param name="token">
    ///     A <see cref="CancellationToken" /> that can be used to cancel the ingestion process.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when critical parameters such as the snapshot source or run ID are null,
    ///     or when the table of contents (TOC) cannot be fetched.
    /// </exception>
    public async Task RunIngestionAsync(string startingPath, CancellationToken token = default)
    {
        Guid? ingestionRunId = Guid.NewGuid();
        Guid? snapshotID = Guid.NewGuid();

        SourceMode = startingPath.StartsWith("http") ? sourceMode.Web : sourceMode.FileSystem;

        //         await dbContext.Procedures.sp_BeginIngestionRunAsync("1.2", "run doc ingestion", ingestionRunId, cancellationToken: token);
        //           await dbContext.Procedures.sp_CreateSourceSnapshotAsync(ingestionRunId.Value, "UNK", startingUrl, "main", "", "", "", "", "", snapshotID, cancellationToken: token);
        //       if (snapshotID.Value == Guid.Empty || ingestionRunId.Value == Guid.Empty)
        //          throw new ArgumentNullException("snapshot source or run ID is null Aborting....");

        _ = new
                LearnPageParser();
        _ = App.GetService<ILogger<LearnPageParser>>();
        _ = new DocRepository();
        LearninRunner runner = new();


        await runner.IngestAsync(new[] { startingPath }, snapshotID, ingestionRunId, token);

        //  _logger.LogInformation("Doc ingestion is complete...");
        _ = await dbContext.Value.Procedures.sp_EndIngestionRunAsync(ingestionRunId.Value, cancellationToken: token);
    }


    */








    //Final step to mark the ingestion run as complete in the database by calling the stored procedure sp_EndIngestionRunAsync with the ingestion run ID. This will update the status of the run in the database and allow for proper tracking and auditing of the ingestion process.
    private async Task CompleteIngestionRunAsync(KBContext runContext, Guid? ingestionRunId, CancellationToken token)
    {
        _logger.LogInformation("Doc ingestion is complete...");
        _ = await runContext.Procedures.sp_EndIngestionRunAsync(ingestionRunId.Value, cancellationToken: token);
    }








    /// <summary>
    ///     Initiates a data ingestion run by creating a source snapshot and starting an ingestion process.
    ///     This step is crucial for setting up the necessary context and identifiers for the identity semantics in the
    ///     temporal storage db.  This allows subsequent operations to be properly tracked and associated with the correct run
    ///     and snapshot.
    ///     This tagging also enables better traceability and management of the ingested data, ensuring that all related
    ///     operations can be correlated back to the original source and run, which is essential for maintaining data integrity
    ///     and facilitating debugging or auditing processes in the future.
    /// </summary>
    /// <param name="startingUrl">The starting URL Or local path for the data ingestion process.</param>
    /// <param name="token">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A tuple containing:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>An <see cref="OutputParameter{TValue}" /> representing the ingestion run ID.</description>
    ///         </item>
    ///         <item>
    ///             <description>An <see cref="OutputParameter{TValue}" /> representing the snapshot ID.</description>
    ///         </item>
    ///         <item>
    ///             <description>An instance of <see cref="KBContext" /> for database operations.</description>
    ///         </item>
    ///     </list>
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when either the snapshot ID or the ingestion run ID is null or empty.
    /// </exception>
    private async Task<(OutputParameter<Guid?> IngestionRunId, OutputParameter<Guid?> SnapshotId)> InitiateDataRunAsync(string startingUrl, CancellationToken token)
    {
        OutputParameter<Guid?> ingestionRunId = new();
        OutputParameter<Guid?> snapshotID = new();
        KBContext runContext = new();

        _ = await runContext.Procedures.sp_BeginIngestionRunAsync("1.2", "DBug run doc ingestion", ingestionRunId, cancellationToken: token);
        _ = await runContext.Procedures.sp_CreateSourceSnapshotAsync(ingestionRunId.Value, "UNK", startingUrl, "main", "N/A", "en-US", "", "", "", snapshotID, cancellationToken: token);

        return ingestionRunId.Value == Guid.Empty || snapshotID.Value == Guid.Empty ? throw new ArgumentNullException("snapshot source or run ID is null Aborting....") : (ingestionRunId, snapshotID);

    }








    /*


    private async Task<IngestionDocument> ParseMarkdownAsync(string markdown, CancellationToken token)
    {
        MarkdownPipeline pipe;
        using StringWriter writer = new();
        NormalizeRenderer renderer = new(writer: writer);
        pipe.Setup(new NormalizeRenderer(new StringWriter()));
          var modifiedMarkdown = NormalizeMarkdown(markdown: markdown, pipe: pipe);
            MemoryStream ms = new(buffer: Encoding.UTF8.GetBytes(s: modifiedMarkdown));
        IngestionDocumentReader reader = new MarkdownReader();
        return await reader.ReadAsync(source: ms, identifier: "doc1", mediaType: "md", cancellationToken: token);
    }

    */








    //This needs to be turned into a processor in the pipeline and should be called before chunking to clean up the markdown content and
    //remove any unwanted elements such as autolinks that may interfere with the chunking and embedding generation process.
    //The NormalizeMarkdown method uses the Markdig library to parse the markdown content, remove any autolink inline elements,
    //and then render the cleaned markdown back to a string format that can be further processed in the ingestion pipeline.
    private static Stream NormalizeMarkdown(string markdown)
    {

        MarkdownPipeline pipe = new MarkdownPipelineBuilder().DisableHtml().UseAdvancedExtensions().Build();

        pipe.Setup(new NormalizeRenderer(new StringWriter()));
        MarkdownDocument MarkDoc = Markdown.Parse(markdown, pipe);
        MarkDoc.Descendants<AutolinkInline>().ToList().ForEach(node => { node.Url = string.Empty; });

        // 1. Render AST back to Markdown text
        StringWriter writer = new();
        NormalizeRenderer renderer = new(writer);
        renderer.Render(MarkDoc);
        writer.Flush();
        var mark = writer.ToString();

        // 2. Convert to UTF8 bytes
        var bytes = Encoding.UTF8.GetBytes(mark);

        // 3. Wrap in MemoryStream (rewound)
        return new MemoryStream(bytes, false);


    }








    private async Task<List<IngestionChunk<string>>> CreateChunksAsync(IngestionDocument doc, CancellationToken token)
    {
        if (doc == null)
        {
            throw new ArgumentNullException(nameof(doc), "The document cannot be null.");
        }

        try
        {
            Tokenizer tok = await TokenizerFactory.Value.ConfigureAwait(false);
            HeaderChunker chunker = new(new IngestionChunkerOptions(tok));
            var chunks = chunker.ProcessAsync(doc, token);
            List<IngestionChunk<string>> chunkList = [];
            await foreach (var chunk in chunks.WithCancellation(token)) chunkList.Add(chunk);

            return chunkList;
        }
        catch (Exception ex)
        {
            // Log the exception and rethrow or handle it as necessary
            throw new InvalidOperationException("An error occurred while creating chunks.", ex);
        }
    }
}