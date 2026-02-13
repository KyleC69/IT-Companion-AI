using Markdig;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Microsoft.Extensions.AI;
using Microsoft.Extensions.DataIngestion;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.ML.Tokenizers;




namespace ITCompanionAI.Ingestion.Docs;





internal class LocalFileParser(ILogger<LocalFileParser> logger)
{
    private static readonly IngestionSettings settings = App.GetService<IOptions<IngestionSettings>>().Value;

    private readonly ILogger<LocalFileParser> _logger = App.GetService<ILogger<LocalFileParser>>();
    private readonly string _vocab = settings.VocabPath ?? throw new InvalidOperationException("Missing Ingestion:VocabPath configuration.");
    private IChatClient _chatClient = App.GetService<IChatClient>();








    internal async Task<LearnPageParseResult> ParseAsync(string path, Guid ingestionRunId, Guid sourceSnapshotId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        using IDisposable scope = logger.BeginScope(new Dictionary<string, object>
        {
                ["Url"] = path,
                ["IngestionRunId"] = ingestionRunId,
                ["SourceSnapshotId"] = sourceSnapshotId
        });

        try
        {
            using (StreamReader sreader = new(path))
            {
                var rawFile = await sreader.ReadToEndAsync(token);

                IngestionDocument doc = new("newdoc1");
                IngestionDocumentReader reader = new MarkdownReader();

                MarkdownPipeline pipe = new MarkdownPipelineBuilder()
                        .UsePipeTables()
                        .UseTaskLists()
                        .UseEmojiAndSmiley()
                        .UseCustomContainers()
                        .UseAdvancedExtensions()
                        .Build();


                //Remove any links in the markdown to avoid confusion for the chunker and enrichers. We will keep the link text but remove the actual links.
                MarkdownDocument rem = Markdown.Parse(rawFile, pipe);
                foreach (AutolinkInline item in rem.Descendants<AutolinkInline>()) item.Remove();


                // Convert the modified markdown document back to text
                await using StringWriter writer = new();
                NormalizeRenderer renderer = new(writer);
                pipe.Setup(renderer);
                _ = renderer.Render(rem);
                var modifiedMarkdown = writer.ToString();


                //Convert the Markdown Document back to
                MemoryStream ms = new(Encoding.UTF8.GetBytes(modifiedMarkdown));
                doc = await reader.ReadAsync(ms, "doc1", "md", token);

                //########################################################
                // This should be Microsoft.ML.Tokenizers. for the Chunker below.
                Tokenizer tok = BertTokenizer.Create(_vocab, new BertOptions());
                // Testing the Header Chunker.
                HeaderChunker chunker = new(new IngestionChunkerOptions(tok));
                // Process the document and get the chunks as an async stream.
                var chunks = chunker.ProcessAsync(doc, token);
                IList<IngestionChunk<string>> chunkList = [];
                await foreach (var chunk in chunks) chunkList.Add(chunk);


                //###########################################
                //##
                //##    Enrichers from Data Ingestion namespace

                //##   Keyword Enricher
                List<IngestionChunk<string>> wordlist = [];
                KeywordEnricher keywordEnricher = new(new EnricherOptions(_chatClient), null, 5, 0.7f);
                var keywords = keywordEnricher.ProcessAsync(chunks, token);
                await foreach (var item in keywords) wordlist.Add(item);

                List<IngestionChunk<string>> summaries = [];
                SummaryEnricher summaryEnricher = new(new EnricherOptions(_chatClient), 50);
                var gist = summaryEnricher.ProcessAsync(chunks, token);
                await foreach (var g in gist) summaries.Add(g);
            }


        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            logger.LogError($"Error reading file: {ex.Message}");
        }



        return new LearnPageParseResult();

    }
}