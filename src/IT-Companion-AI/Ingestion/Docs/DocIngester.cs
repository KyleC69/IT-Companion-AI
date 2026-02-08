using System.Text.RegularExpressions;

using ITCompanionAI.Services;

using Markdig;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Microsoft.Extensions.DataIngestion;
using Microsoft.Extensions.Logging;
using Microsoft.ML.Tokenizers;

using OllamaSharp;




namespace ITCompanionAI.Ingestion.Docs;





public class DocIngester
{

    private const string _onnx = "F:\\AI-Models\\distilbert-base-uncased-finetuned-sst-2-english\\onnx\\model.onnx";
    private const string _token = "F:\\AI-Models\\distilbert-base-uncased-finetuned-sst-2-english\\onnx\\tokenizer.json";
    private const string _vocab = "F:\\AI-Models\\distilbert-base-uncased-finetuned-sst-2-english\\onnx\\vocab.txt";
    private static readonly System.Diagnostics.TraceSource Log = new("DocsIngestion", System.Diagnostics.SourceLevels.All);
    private readonly OllamaApiClient _chatClient;
    private readonly HttpClientService _http;
    private readonly string[] _labels;
    private readonly ILogger<DocIngester> _logger;
    private readonly TocFetcher _tocFetcher = new();

    private KBContext dbContext;
    //   private readonly InferenceSession _session;
    // private HFT.Tokenizer _tokenizer;








    public DocIngester()
    {
        _http = App.GetService<HttpClientService>();
        _logger = App.GetService<ILogger<DocIngester>>();
        //  _chatClient = App.GetService<OllamaApiClient>();
        dbContext = new KBContext();
        _chatClient = new OllamaApiClient("http://localhost:11434", "llama3.2:1b");
        // HF tokenizer 
        // HFTokenizer.Tokenizer tk = HFTokenizer.Tokenizer.FromFile(_token);
    }








    public async Task RunIngestion(string startingUrl, CancellationToken token = default)
    {
        var connectionString = "Data Source=Desktop-NC01091;Initial Catalog=KnowledgeBase;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        var ingestionRunId = new OutputParameter<Guid?>();
        var snapshotID = new OutputParameter<Guid?>();
        KBContext dbContext = new();
#if !DEBUG
            await dbContext.Procedures.sp_BeginIngestionRunAsync("1.2", "run doc ingestion", ingestionRunId, cancellationToken: token);
            await dbContext.Procedures.sp_CreateSourceSnapshotAsync(ingestionRunId.Value, "UNK", startingUrl, "main", "", "", "", "", "", snapshotID, cancellationToken: token);
        if (snapshotID.Value == Guid.Empty || ingestionRunId.Value == Guid.Empty)
        {
            throw new ArgumentNullException("snapshot source or run ID is null Aborting....");
        }


#endif
        var parserLogger = App.GetService<ILogger<LearnPageParser>>();
        LearnPageParser parser = new();
        DocRepository repo = new(connectionString);
        LearnIngestionRunner runner = new(parser, repo);

        /*


        _logger.LogTrace("Starting Document ingestion run");
        // 1. Fetch TOC JSON

        var toc = await _tocFetcher.FetchAsync(startingUrl);
        if (toc == null)
        {
            throw new ArgumentNullException(nameof(toc));
        }
        */
        //    Log.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "TOC root items: {0}", toc?.Count ?? 0);
        IngestionDocument doc = new("dfdff");
        IngestionDocumentReader reader = new MarkdownReader();

        MarkdownPipeline pipe = new MarkdownPipelineBuilder()
                .UseReferralLinks("nofollow")
                .UsePipeTables()
                .UseTaskLists()
                .UseCustomContainers()
                .Build();


        HttpResponseMessage response = await _http.GetAsync("https://raw.githubusercontent.com/dotnet/docs/refs/heads/live/docs/ai/dotnet-ai-ecosystem.md", token);

        var markdown = await response.Content.ReadAsStringAsync(token);


        //Remove any links in the markdown to avoid confusion for the chunker and enrichers. We will keep the link text but remove the actual links.
        MarkdownDocument rem = Markdown.Parse(markdown, pipe);
        foreach (AutolinkInline item in rem.Descendants<AutolinkInline>()) item.Remove();

        // Convert the modified markdown document back to text
        await using StringWriter writer = new();
        NormalizeRenderer renderer = new(writer);
        pipe.Setup(renderer);
        renderer.Render(rem);
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
        IList<IngestionChunk<string>> chunkList = new List<IngestionChunk<string>>();
        await foreach (var chunk in chunks) chunkList.Add(chunk);


        //##################################################
        //HFTokenizer.Tokenizer tokenizer = HFTokenizer.Tokenizer.FromFile("tokenizer.json");

        //   IngestionPipeline<string> pipeline = new();







        //###########################################
        //##
        //##    Enrichers from Data Ingestion namespace

        //##   Keyword Enricher
        List<IngestionChunk<string>> wordlist = new();
        KeywordEnricher keywordEnricher = new(new EnricherOptions(_chatClient), null, 5, 0.7f);
        var keywords = keywordEnricher.ProcessAsync(chunks, token);
        await foreach (var item in keywords) wordlist.Add(item);

        List<IngestionChunk<string>> summaries = new();
        SummaryEnricher summaryEnricher = new(new EnricherOptions(_chatClient), 50);
        var gist = summaryEnricher.ProcessAsync(chunks, token);
        await foreach (var g in gist) summaries.Add(g);




        //############################################
        //##
        //##    Starting page ingestion here
        try
        {
            if (snapshotID?.Value != null)
            {
                if (ingestionRunId.Value != null)
                {
                    //  await runner.IngestAsync(toc, (Guid)snapshotID?.Value, (Guid)ingestionRunId.Value, token);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"[ERROR] Fetching URL: {ex.Message}");
            //       Log.TraceEvent(System.Diagnostics.TraceEventType.Warning, 0, "Fetch failed Url={0} Error={1}", entry.Url, ex.Message);
        }

        _logger.LogInformation("Doc ingestion is complete...");
        await dbContext.Procedures.sp_EndIngestionRunAsync(ingestionRunId.Value, cancellationToken: token);
    }








    private static async Task<Stream> NormalizeAutolinksAsync(Stream input)
    {
        using StreamReader reader = new(input, leaveOpen: true);
        var text = await reader.ReadToEndAsync();
        text = Regex.Replace(text, @"<((https?|mailto):[^>]+)>", "[$1]($1)");
        return new MemoryStream(Encoding.UTF8.GetBytes(text));
    }
}