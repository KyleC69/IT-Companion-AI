using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SemanticKernel;
using SKAgentOrchestrator.AgentFramework;
using System.Text;

#nullable enable
using AiChatMessage = Microsoft.Extensions.AI.ChatMessageContent;
using AiChatResponse = Microsoft.Extensions.AI.ChatResponse;
using AiChatResponseUpdate = Microsoft.Extensions.AI.ChatResponseUpdate;
using AiChatRole = Microsoft.Extensions.AI.ChatRole;
using System.Runtime.CompilerServices;

namespace CompanionTests;

[TestClass]
public class AgentIntegrationTests
{
    public TestContext TestContext { get; set; } = null!;

    private static LiveTestContext RequireContext(TestContext testContext)
    {
        var ctx = LiveTestContext.TryCreate(testContext);
        if (ctx == null)
        {
            Assert.Inconclusive("GITHUB_TOKEN (and optional endpoint/model) must be set for live integration tests.");
        }
        return ctx!;
    }

    [TestMethod]
    public async Task EchoText_UsesToolAndReturnsEcho()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Echo back the exact text 'ping' using the echo.text tool.";
        LogPrompt(prompt);

        var result = await ctx.Orchestrator.RunAsync(new AgentChatHistory(), prompt, CancellationToken.None);
        LogModel(ctx, result);

        var obs = ReadObservation(result);
        Assert.AreEqual("echo.text", obs.ToolName);
        Assert.AreEqual("ok", obs.Status);
        Assert.AreEqual("ping", obs.Data.GetProperty("text").GetString());
        StringAssert.Contains(result.AssistantMessage.Text, "ping");
    }

    [TestMethod]
    public async Task SystemInfo_UsesToolAndIncludesDetail()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Get extended system information using the system.info tool.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "system.info");
        Assert.AreEqual("extended", obs.Data.GetProperty("detail").GetString());
        Assert.IsTrue((obs.Data.GetProperty("machine").GetString() ?? string.Empty).Length <= 16);
        Assert.IsTrue(result.AssistantMessage.Text.IndexOf("system", StringComparison.OrdinalIgnoreCase) >= 0);
    }

    [TestMethod]
    public async Task WindowsIntrospection_UsesTool()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Inspect Windows environment with extended detail using windows.introspect tool.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "windows.introspect");
        Assert.IsTrue(obs.Data.GetProperty("processCount").GetInt32() >= 0);
        Assert.AreEqual("extended", obs.Data.GetProperty("detail").GetString());
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.AssistantMessage.Text));
    }

    [TestMethod]
    public async Task WeatherTool_ReturnsForecast()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Get deterministic weather for Paris using get_weather tool.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "get_weather");
        Assert.AreEqual("Paris", obs.Data.GetProperty("city").GetString());
        Assert.AreEqual("Sunny", obs.Data.GetProperty("forecast").GetString());
        StringAssert.Contains(result.AssistantMessage.Text, "Sunny");
    }

    [TestMethod]
    public async Task JsonValidator_InvalidJson()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Validate this malformed json using json.validate: not-json";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "json.validate");
        Assert.AreEqual(1, obs.ExitCode);
        Assert.AreEqual("invalid", obs.Status);
        StringAssert.Contains(result.AssistantMessage.Text, "invalid", StringComparison.OrdinalIgnoreCase);
    }

    [TestMethod]
    public async Task HttpGet_InvalidScheme()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Use http.get to fetch ftp://example.com";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "http.get");
        Assert.AreEqual(1, obs.ExitCode);
        Assert.AreEqual("error", obs.Status);
        StringAssert.Contains(result.AssistantMessage.Text, "error", StringComparison.OrdinalIgnoreCase);
    }

    [TestMethod]
    public async Task HttpPost_InvalidScheme()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Use http.post to send to ftp://example.com";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "http.post");
        Assert.AreEqual(1, obs.ExitCode);
        Assert.AreEqual("error", obs.Status);
        StringAssert.Contains(result.AssistantMessage.Text, "error", StringComparison.OrdinalIgnoreCase);
    }

    [TestMethod]
    public async Task VectorSearch_FindsMatch()
    {
        using var ctx = RequireContext(TestContext);
        ctx.VectorStore.Upsert(new VectorStoreRecord
        {
            Id = "1",
            Text = "alpha text",
            Source = "test",
            Embedding = ctx.EmbeddingService.GenerateEmbedding("alpha text")
        });
        var prompt = "Search for similar text 'alpha text' using vector.search and return top result.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "vector.search");
        var results = obs.Data.GetProperty("results").EnumerateArray().ToArray();
        Assert.AreEqual(1, results.Length);
        Assert.AreEqual("alpha text", results[0].GetProperty("Text").GetString());
        StringAssert.Contains(result.AssistantMessage.Text, "alpha text");
    }

    [TestMethod]
    public async Task MemoryStore_StoresContent()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Store the note 'Remember this' with type note using memory.store.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "memory.store");
        Assert.AreEqual("ok", obs.Status);
        var recent = await ctx.MemoryStore.QueryAsync("Remember", 5, CancellationToken.None);
        Assert.IsTrue(recent.Any(r => r.Text == "Remember this" && r.Type == "note"));
        StringAssert.Contains(result.AssistantMessage.Text, "Remember");
    }

    [TestMethod]
    public async Task PowerShellRun_AllowsSafeScript()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Run PowerShell command Write-Output 'hi' using powershell.run tool.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "powershell.run");
        Assert.AreEqual(0, obs.ExitCode);
        Assert.IsTrue(obs.Data.GetProperty("stdout").GetString()?.Contains("hi", StringComparison.OrdinalIgnoreCase) == true);
        StringAssert.Contains(result.AssistantMessage.Text, "hi");
    }

    [TestMethod]
    public async Task FileWrite_WritesToSandbox()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Write text 'hello world' to data/sample.txt using file.write tool.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "file.write");
        var fullPath = Path.Combine(ctx.SandboxRoot, "data", "sample.txt");
        Assert.IsTrue(File.Exists(fullPath));
        Assert.AreEqual("hello world", File.ReadAllText(fullPath));
        StringAssert.Contains(result.AssistantMessage.Text, "hello world");
    }

    [TestMethod]
    public async Task FileRead_ReadsFromSandbox()
    {
        using var ctx = RequireContext(TestContext);
        Directory.CreateDirectory(ctx.SandboxRoot);
        File.WriteAllText(Path.Combine(ctx.SandboxRoot, "readme.txt"), "file content");
        var prompt = "Read file readme.txt using file.read tool.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "file.read");
        Assert.AreEqual("file content", obs.Data.GetProperty("content").GetString());
        StringAssert.Contains(result.AssistantMessage.Text, "file content");
    }

    [TestMethod]
    public async Task FileList_ListsDirectory()
    {
        using var ctx = RequireContext(TestContext);
        var dir = Path.Combine(ctx.SandboxRoot, "listdir");
        Directory.CreateDirectory(dir);
        File.WriteAllText(Path.Combine(dir, "a.txt"), "a");
        File.WriteAllText(Path.Combine(dir, "b.txt"), "b");
        var prompt = "List files in listdir using file.list tool.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "file.list");
        var files = obs.Data.GetProperty("files").EnumerateArray().Select(f => f.GetString()).ToArray();
        CollectionAssert.Contains(files, "a.txt");
        CollectionAssert.Contains(files, "b.txt");
        StringAssert.Contains(result.AssistantMessage.Text, "a.txt");
    }

    [TestMethod]
    public async Task RegistryRead_ReturnsPath()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Read registry path HKLM\\SOFTWARE\\Test using registry.read tool.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "registry.read");
        Assert.AreEqual("HKLM\\SOFTWARE\\Test", obs.Data.GetProperty("path").GetString());
        StringAssert.Contains(result.AssistantMessage.Text, "HKLM");
    }

    [TestMethod]
    public async Task EventLog_ReturnsLimitedEntries()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Get 2 entries from System event log using eventlog.read tool.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "eventlog.read");
        var entries = obs.Data.GetProperty("entries").EnumerateArray().ToArray();
        Assert.AreEqual(2, entries.Length);
        Assert.AreEqual("System", entries[0].GetProperty("source").GetString());
        StringAssert.Contains(result.AssistantMessage.Text, "System");
    }

    [TestMethod]
    public async Task RegexExtract_FindsNumbers()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Find numbers in 'Value 42 and 99' using regex.extract with pattern \\d+ and IgnoreCase.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "regex.extract");
        var matches = obs.Data.GetProperty("matches").EnumerateArray().ToArray();
        Assert.AreEqual(2, matches.Length);
        Assert.AreEqual("42", matches[0].GetProperty("Value").GetString());
        StringAssert.Contains(result.AssistantMessage.Text, "42");
    }

    [TestMethod]
    public async Task CsvParse_ParsesRows()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Parse CSV with header using csv.parse tool: name,age\\nAnn,30\\nBob,25";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "csv.parse");
        var header = obs.Data.GetProperty("header").EnumerateArray().Select(e => e.GetString()).ToArray();
        var rows = obs.Data.GetProperty("rows").EnumerateArray().ToArray();
        CollectionAssert.AreEquivalent(new[] { "name", "age" }, header);
        Assert.AreEqual(2, rows.Length);
        StringAssert.Contains(result.AssistantMessage.Text, "Ann");
    }

    [TestMethod]
    public async Task Stats_Calculates()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Calculate stats for values 1,2,3 using stats.calculate.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "stats.calculate");
        Assert.AreEqual(1, obs.Data.GetProperty("min").GetDouble());
        Assert.AreEqual(3, obs.Data.GetProperty("max").GetDouble());
        Assert.AreEqual(3, obs.Data.GetProperty("count").GetInt32());
        StringAssert.Contains(result.AssistantMessage.Text, "mean", StringComparison.OrdinalIgnoreCase);
    }

    [TestMethod]
    public async Task Embedding_GeneratesVector()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Generate embedding for text 'sample' using embedding.generate tool.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "embedding.generate");
        Assert.AreEqual(128, obs.Data.GetProperty("length").GetInt32());
        StringAssert.Contains(result.AssistantMessage.Text, "128");
    }

    [TestMethod]
    public async Task Classifier_InfersIntent()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Classify intent of 'Please install the update' using classify.intent tool.";

        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "classify.intent");
        Assert.AreEqual("setup", obs.Data.GetProperty("intent").GetString());
        StringAssert.Contains(result.AssistantMessage.Text, "setup");
    }

    [TestMethod]
    public async Task PowerShell_ReadOnly_GetProcess()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Use powershell.run to execute 'Get-Process | Select-Object -First 1 -Property ProcessName'. Include stdout in your answer.";
        LogPrompt(prompt);

        var result = await ctx.Orchestrator.RunAsync(new AgentChatHistory(), prompt, CancellationToken.None);
        LogModel(ctx, result);

        var obs = ReadObservation(result);
        Assert.AreEqual("powershell.run", obs.ToolName);
        Assert.AreEqual(0, obs.ExitCode);
        var stdout = obs.Data.GetProperty("stdout").GetString() ?? string.Empty;
        StringAssert.Contains(stdout, "ProcessName", "Expected read-only Get-Process output.");
        StringAssert.Contains(result.AssistantMessage.Text, "ProcessName", "Final response should reflect tool output.");
    }

    [TestMethod]
    public async Task HttpGet_BingSearch_ReadOnly()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Use http.get to fetch https://www.bing.com/search?q=bing and summarize the status code only.";
        LogPrompt(prompt);

        var result = await ctx.Orchestrator.RunAsync(new AgentChatHistory(), prompt, CancellationToken.None);
        LogModel(ctx, result);

        var obs = ReadObservation(result);
        Assert.AreEqual("http.get", obs.ToolName);
        Assert.AreEqual(0, obs.ExitCode);
        var status = obs.Data.GetProperty("status").GetInt32();
        Assert.AreEqual(200, status);
        StringAssert.Contains(result.AssistantMessage.Text, "200");
    }

    [TestMethod]
    public async Task HttpPost_Echo()
    {
        string prompt = string.Empty;
        using var ctx = RequireContext(TestContext);
      //  var prompt = "Use http.post to send JSON {\\"hello\\":\\"world\\"} to https://postman-echo.com/post and report the status code.";
        LogPrompt(prompt);

        var result = await ctx.Orchestrator.RunAsync(new AgentChatHistory(), prompt, CancellationToken.None);
        LogModel(ctx, result);

        var obs = ReadObservation(result);
        Assert.AreEqual("http.post", obs.ToolName);
        Assert.AreEqual(0, obs.ExitCode);
        var status = obs.Data.GetProperty("status").GetInt32();
        Assert.AreEqual(200, status);
        StringAssert.Contains(result.AssistantMessage.Text, "200");
    }

    [TestMethod]
    public async Task FileList_ReadOnlySandbox()
    {
        using var ctx = RequireContext(TestContext);
        Directory.CreateDirectory(Path.Combine(ctx.SandboxRoot, "readonly"));
        File.WriteAllText(Path.Combine(ctx.SandboxRoot, "readonly", "a.txt"), "alpha");
        File.WriteAllText(Path.Combine(ctx.SandboxRoot, "readonly", "b.txt"), "beta");
        var prompt = "List files in readonly using file.list and echo the filenames only.";
        LogPrompt(prompt);

        var result = await ctx.Orchestrator.RunAsync(new AgentChatHistory(), prompt, CancellationToken.None);
        LogModel(ctx, result);

        var obs = ReadObservation(result);
        Assert.AreEqual("file.list", obs.ToolName);
        var files = obs.Data.GetProperty("files").EnumerateArray().Select(f => f.GetString()).ToArray();
        CollectionAssert.Contains(files, "a.txt");
        CollectionAssert.Contains(files, "b.txt");
        StringAssert.Contains(result.AssistantMessage.Text, "a.txt");
    }

    [TestMethod]
    public async Task RegistryRead_ReadOnly()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Use registry.read to read HKLM\\\\SOFTWARE\\\\Test and report the path.";
        LogPrompt(prompt);

        var result = await ctx.Orchestrator.RunAsync(new AgentChatHistory(), prompt, CancellationToken.None);
        LogModel(ctx, result);

        var obs = ReadObservation(result);
        Assert.AreEqual("registry.read", obs.ToolName);
        Assert.AreEqual("HKLM\\SOFTWARE\\Test", obs.Data.GetProperty("path").GetString());
        StringAssert.Contains(result.AssistantMessage.Text, "HKLM");
    }

    [TestMethod]
    public async Task EventLog_ReadOnly()
    {
        using var ctx = RequireContext(TestContext);
        var prompt = "Use eventlog.read to get 2 entries from System log and summarize the source names.";
        LogPrompt(prompt);

        var result = await ctx.Orchestrator.RunAsync(new AgentChatHistory(), prompt, CancellationToken.None);
        LogModel(ctx, result);

        var obs = ReadObservation(result);
        Assert.AreEqual("eventlog.read", obs.ToolName);
        var entries = obs.Data.GetProperty("entries").EnumerateArray().ToArray();
        Assert.IsTrue(entries.Length >= 1);
        StringAssert.Contains(result.AssistantMessage.Text, "System");
    }

    [TestMethod]
    public async Task MemoryQuery_ReturnsStoredContent()
    {
        using var ctx = RequireContext(TestContext);
        // Seed memory directly
        await ctx.MemoryStore.AddAsync(new MemoryRecord
        {
            Content = "Stored reminder text",
            Tags = new Dictionary<string, string> { { "type", "note" } },
            Type = "note"
        }, CancellationToken.None);

        var prompt = "Use memory.query to find the note containing 'reminder' and share the content.";
        var (obs, result) = await ExecuteToolAsync(ctx, prompt, expectedTool: "memory.query");
        var items = obs.Data.GetProperty("items").EnumerateArray().ToArray();
        Assert.IsTrue(items.Length >= 1);
        StringAssert.Contains(items[0].GetProperty("Content").GetString() ?? string.Empty, "Stored reminder text");
        StringAssert.Contains(result.AssistantMessage.Text, "reminder");
    }

    // Helpers
    private void LogPrompt(string prompt) => TestContext.WriteLine("Prompt:\n{0}", prompt);

    private void LogModel(LiveTestContext ctx, AgentTurnResult result)
    {
        if (!string.IsNullOrWhiteSpace(ctx.ChatLogger.LastPrompt))
        {
            TestContext.WriteLine("Model Prompt Sent:\n{0}", ctx.ChatLogger.LastPrompt);
        }
        if (result.ToolMessage != null)
        {
            TestContext.WriteLine("Tool Observation:\n{0}", result.ToolMessage.Text);
        }
        TestContext.WriteLine("Assistant Response:\n{0}", result.AssistantMessage.Text);
    }

    private static ToolObservation ReadObservation(AgentTurnResult result)
    {
        Assert.IsNotNull(result.ToolMessage, "Tool message missing");
        var observation = JsonSerializer.Deserialize<ToolObservation>(result.ToolMessage!.Text);
        Assert.IsNotNull(observation, "Tool observation parse failed");
        return observation!;
    }

    private static async Task<(ToolObservation observation, AgentTurnResult result)> ExecuteToolAsync(LiveTestContext ctx, string prompt, string expectedTool)
    {
        ctx.TestContext.WriteLine("Prompt:\n{0}", prompt);
        var result = await ctx.Orchestrator.RunAsync(new AgentChatHistory(), prompt, CancellationToken.None);
        ctx.TestContext.WriteLine("Model Prompt Sent:\n{0}", ctx.ChatLogger.LastPrompt ?? string.Empty);
        ctx.TestContext.WriteLine("Assistant Response:\n{0}", result.AssistantMessage.Text);
        var obs = ReadObservation(result);
        Assert.AreEqual(expectedTool, obs.ToolName, "Unexpected tool invoked");
        return (obs, result);
    }

    public sealed class LiveTestContext : IDisposable
    {
        private LiveTestContext(AgentOrchestrator orchestrator, EmbeddingService embeddingService, InMemoryVectorStore vectorStore, IMemoryStore memoryStore, string sandboxRoot, LoggingChatClient chatLogger, TestContext testContext)
        {
            Orchestrator = orchestrator;
            EmbeddingService = embeddingService;
            VectorStore = vectorStore;
            MemoryStore = memoryStore;
            SandboxRoot = sandboxRoot;
            ChatLogger = chatLogger;
            TestContext = testContext;
        }

        public AgentOrchestrator Orchestrator { get; }
        public EmbeddingService EmbeddingService { get; }
        public InMemoryVectorStore VectorStore { get; }
        public IMemoryStore MemoryStore { get; }
        public string SandboxRoot { get; }
        public LoggingChatClient ChatLogger { get; }
        public TestContext TestContext { get; }

        public static LiveTestContext? TryCreate(TestContext testContext)
        {
            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var endpoint = Environment.GetEnvironmentVariable("GITHUB_MODELS_ENDPOINT") ?? AgentOrchestratorDefaults.DefaultEndpoint;
            var modelName = Environment.GetEnvironmentVariable("GITHUB_MODEL_NAME") ?? AgentOrchestratorDefaults.DefaultModelName;

            var client = new ChatCompletionsClient(new Uri(endpoint), new AzureKeyCredential(token));
            var embeddingService = new EmbeddingService();
            var vectorStore = new InMemoryVectorStore();
            SeedVectorStore(vectorStore, embeddingService);
            var sandbox = Path.Combine(Path.GetTempPath(), "AgentIntegrationTests_" + Guid.NewGuid().ToString("N"));
            var memoryPath = Path.Combine(sandbox, "memory.json");
            var memoryStore = new FileMemoryStore(memoryPath);
            var safetyGuard = new SafetyGuard();
            var toolRegistry = new ToolRegistry();
            toolRegistry.RegisterDefaults(embeddingService, vectorStore, memoryStore, sandbox);
            var azureChatClient = new AzureInferenceChatClient(client, modelName, NullLogger<AzureInferenceChatClient>.Instance);
            var loggingClient = new LoggingChatClient(azureChatClient, testContext);
            var kernelFactory = new SemanticKernelFactory(loggingClient, toolRegistry, NullLoggerFactory.Instance);

            var orchestrator = new AgentOrchestrator(kernelFactory, modelName, embeddingService, vectorStore, memoryStore, safetyGuard, toolRegistry, NullLogger<AgentOrchestrator>.Instance);
            return new LiveTestContext(orchestrator, embeddingService, vectorStore, memoryStore, sandbox, loggingClient, testContext);
        }

        public void Dispose()
        {
            Orchestrator.Dispose();
            if (Directory.Exists(SandboxRoot))
            {
                try { Directory.Delete(SandboxRoot, recursive: true); } catch { }
            }
        }

        private static void SeedVectorStore(InMemoryVectorStore store, EmbeddingService embeddingService)
        {
            var samples = new[]
            {
                "Azure App Service: Host web applications and APIs in a fully managed Azure service.",
                "Azure Blob Storage: Store and retrieve files in the cloud, highly scalable and redundant.",
                "Azure Key Vault: Store application secrets in an encrypted vault with restricted access."
            };
            foreach (var text in samples)
            {
                store.Upsert(new VectorStoreRecord
                {
                    Text = text,
                    Embedding = embeddingService.GenerateEmbedding(text),
                    Source = "bootstrap"
                });
            }
        }
    }

    private static class AgentOrchestratorDefaults
    {
        public const string DefaultModelName = "microsoft/phi-4-mini-instruct";
        public const string DefaultEndpoint = "https://models.github.ai/inference";
    }

    public sealed class LoggingChatClient : IChatClient, IDisposable
    {
        private readonly IChatClient _inner;
        private readonly TestContext _testContext;

        public LoggingChatClient(IChatClient inner, TestContext testContext)
        {
            _inner = inner;
            _testContext = testContext;
        }

        public string? LastPrompt { get; private set; }

        public async Task<AiChatResponse> GetResponseAsync(IEnumerable<AiChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            LastPrompt = BuildPrompt(messages);
            _testContext.WriteLine("Chat prompt sent to model:\n{0}", LastPrompt);
            var response = await _inner.GetResponseAsync(messages, options, cancellationToken).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(response.Text))
            {
                _testContext.WriteLine("Model raw response:\n{0}", response.Text);
            }
            return response;
        }

        public async IAsyncEnumerable<AiChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<AiChatMessage> messages, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            LastPrompt = BuildPrompt(messages);
            _testContext.WriteLine("Chat prompt sent to model (streaming):\n{0}", LastPrompt);
            await foreach (var update in _inner.GetStreamingResponseAsync(messages, options, cancellationToken).ConfigureAwait(false))
            {
                if (!string.IsNullOrWhiteSpace(update.Text))
                {
                    _testContext.WriteLine("Model streaming update:\n{0}", update.Text);
                }
                yield return update;
            }
        }

        public object? GetService(Type serviceType, object? serviceKey) => _inner.GetService(serviceType, serviceKey);

        public void Dispose()
        {
            if (_inner is IDisposable d)
            {
                d.Dispose();
            }
        }

        private static string BuildPrompt(IEnumerable<AiChatMessage> messages)
        {
            var sb = new StringBuilder();
            foreach (var message in messages)
            {
                sb.AppendLine($"[{message.Role}] {message.Text}");
            }
            return sb.ToString();
        }
    }
}
