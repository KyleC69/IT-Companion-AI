using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKAgentOrchestrator.AgentFramework;

namespace CompanionTests;

[TestClass]
public class AgentToolTests
{
    private static ToolAction CreateAction(string toolName, object parameters, string? actionName = null, string? correlationId = null)
    {
        var payload = JsonSerializer.SerializeToElement(parameters ?? new { });
        return new ToolAction(toolName, actionName ?? "test", correlationId ?? Guid.NewGuid().ToString("N"), payload, null);
    }

    [TestMethod]
    public async Task EchoTextTool_EchoesInput()
    {
        var tool = new EchoTextTool();
        var action = CreateAction(tool.Name, new { text = "hello" });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);

        Assert.AreEqual(0, result.ExitCode);
        Assert.AreEqual("ok", result.Status);
        Assert.AreEqual("hello", result.Data.GetProperty("text").GetString());
    }

    [TestMethod]
    public async Task SystemInfoTool_ReturnsSanitizedValues()
    {
        var tool = new SystemInfoTool();
        var action = CreateAction(tool.Name, new { detail = "extended" });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);
        var data = result.Data;

        Assert.AreEqual("ok", result.Status);
        Assert.IsLessThanOrEqualTo(16, (data.GetProperty("machine").GetString() ?? string.Empty).Length);
        Assert.AreEqual("extended", data.GetProperty("detail").GetString());
    }

    [TestMethod]
    public async Task WindowsIntrospectionTool_ReturnsProcessInfo()
    {
        var tool = new WindowsIntrospectionTool();
        var action = CreateAction(tool.Name, new { detail = "EXTENDED" });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);
        var data = result.Data;

        Assert.AreEqual("ok", result.Status);
        Assert.IsGreaterThanOrEqualTo(0, data.GetProperty("processCount").GetInt32());
        Assert.AreEqual("extended", data.GetProperty("detail").GetString());
    }

    [TestMethod]
    public async Task WeatherTool_ReturnsCityForecast()
    {
        var tool = new WeatherTool();
        var action = CreateAction(tool.Name, new { city = "Paris" });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);
        var data = result.Data;

        Assert.AreEqual("ok", result.Status);
        Assert.AreEqual("Paris", data.GetProperty("city").GetString());
        Assert.AreEqual("Sunny", data.GetProperty("forecast").GetString());
    }

    [TestMethod]
    public async Task JsonValidatorTool_DetectsInvalidJson()
    {
        var tool = new JsonValidatorTool();
        var action = CreateAction(tool.Name, new { json = "{" });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);

        Assert.AreEqual(1, result.ExitCode);
        Assert.AreEqual("invalid", result.Status);
        Assert.IsTrue(result.ErrorMessage?.Length > 0);
    }

    [TestMethod]
    public async Task HttpGetTool_ReturnsErrorForInvalidUrl()
    {
        var tool = new HttpGetTool();
        var action = CreateAction(tool.Name, new { url = "ftp://example.com" });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);

        Assert.AreEqual(1, result.ExitCode);
        Assert.AreEqual("error", result.Status);
    }

    [TestMethod]
    public async Task HttpPostTool_RequiresUrl()
    {
        var tool = new HttpPostTool();
        var action = CreateAction(tool.Name, new { });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);

        Assert.AreEqual(1, result.ExitCode);
        Assert.AreEqual("error", result.Status);
    }

    [TestMethod]
    public async Task VectorSearchTool_ReturnsMatchingRecord()
    {
        var embeddingService = new EmbeddingService();
        var vectorStore = new InMemoryVectorStore();
        var embedding = embeddingService.GenerateEmbedding("alpha text");
        vectorStore.Upsert(new VectorStoreRecord { Id = "1", Text = "alpha text", Source = "test", Embedding = embedding });
        var tool = new VectorSearchTool(vectorStore, embeddingService);
        var action = CreateAction(tool.Name, new { text = "alpha text", topK = 1 });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);
        var results = result.Data.GetProperty("results").EnumerateArray().ToArray();

        Assert.AreEqual(0, result.ExitCode);
        Assert.HasCount(1, results);
        Assert.AreEqual("alpha text", results[0].GetProperty("Text").GetString());
    }

    [TestMethod]
    public async Task MemoryTool_StoresAndQueriesRecords()
    {
        var store = new TestMemoryStore();
        var tool = new MemoryTool(store);
        var storeAction = CreateAction(tool.Name, new { action = "store", content = "Remember this", type = "note" });
        var queryAction = CreateAction(tool.Name, new { action = "query", filter = "remember", take = 3 });

        var storeResult = await tool.ExecuteAsync(storeAction, CancellationToken.None);
        Assert.AreEqual(0, storeResult.ExitCode);
        Assert.HasCount(1, store.Records);

        var queryResult = await tool.ExecuteAsync(queryAction, CancellationToken.None);
        var results = queryResult.Data.GetProperty("results").EnumerateArray().ToArray();

        Assert.AreEqual("ok", queryResult.Status);
        Assert.HasCount(1, results);
        Assert.AreEqual("note", results[0].GetProperty("Type").GetString());
    }

    [TestMethod]
    public async Task PowerShellRunTool_BlocksDangerousScripts()
    {
        var tool = new PowerShellRunTool();
        var action = CreateAction(tool.Name, new { script = "Remove-Item C:/temp" });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);

        Assert.AreEqual(1, result.ExitCode);
        Assert.AreEqual("blocked", result.Status);
    }

    [TestMethod]
    public async Task FileTools_WriteReadAndListWithinSandbox()
    {
        var sandbox = Path.Combine(Path.GetTempPath(), "AgentToolTests_" + Guid.NewGuid().ToString("N"));
        var writeTool = new FileWriteTool(sandbox);
        var readTool = new FileReadTool(sandbox);
        var listTool = new FileListTool(sandbox);

        try
        {
            var writeAction = CreateAction(writeTool.Name, new { path = "data/sample.txt", content = "hello world" });
            var writeResult = await writeTool.ExecuteAsync(writeAction, CancellationToken.None);
            Assert.AreEqual("ok", writeResult.Status);

            var readAction = CreateAction(readTool.Name, new { path = "data/sample.txt" });
            var readResult = await readTool.ExecuteAsync(readAction, CancellationToken.None);
            Assert.AreEqual("hello world", readResult.Data.GetProperty("content").GetString());

            var listAction = CreateAction(listTool.Name, new { path = "data" });
            var listResult = await listTool.ExecuteAsync(listAction, CancellationToken.None);
            var files = listResult.Data.GetProperty("files").EnumerateArray().Select(f => f.GetString()).ToArray();
            Assert.IsTrue(files.Contains("sample.txt"));
        }
        finally
        {
            if (Directory.Exists(sandbox))
            {
                try { Directory.Delete(sandbox, recursive: true); } catch { }
            }
        }
    }

    [TestMethod]
    public async Task RegistryReadTool_ReturnsStubPath()
    {
        var tool = new RegistryReadTool();
        var action = CreateAction(tool.Name, new { path = "HKLM\\SOFTWARE\\Test" });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);

        Assert.AreEqual("ok", result.Status);
        Assert.AreEqual("HKLM\\SOFTWARE\\Test", result.Data.GetProperty("path").GetString());
    }

    [TestMethod]
    public async Task EventLogTool_RespectsLimit()
    {
        var tool = new EventLogTool();
        var action = CreateAction(tool.Name, new { source = "System", limit = 3 });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);
        var entries = result.Data.GetProperty("entries").EnumerateArray().ToArray();

        Assert.HasCount(3, entries);
        Assert.AreEqual("System", entries[0].GetProperty("source").GetString());
    }

    [TestMethod]
    public async Task RegexExtractTool_FindsMatches()
    {
        var tool = new RegexExtractTool();
        var action = CreateAction(tool.Name, new { text = "Value 42 and 99", pattern = "\\d+", options = "IgnoreCase" });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);
        var matches = result.Data.GetProperty("matches").EnumerateArray().ToArray();

        Assert.HasCount(2, matches);
        Assert.AreEqual("42", matches[0].GetProperty("Value").GetString());
    }

    [TestMethod]
    public async Task CsvParseTool_ParsesHeaderAndRows()
    {
        var tool = new CsvParseTool();
        var action = CreateAction(tool.Name, new { csv = "name,age\nAnn,30\nBob,25", hasHeader = true });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);
        var data = result.Data;
        var header = data.GetProperty("header").EnumerateArray().Select(h => h.GetString()).ToArray();
        var rows = data.GetProperty("rows").EnumerateArray().ToArray();

        Assert.IsTrue(header.SequenceEqual(new[] { "name", "age" }));
        Assert.HasCount(2, rows);
        Assert.AreEqual("Ann", rows[0].EnumerateArray().First().GetString());
    }

    [TestMethod]
    public async Task StatsTool_ComputesBasicStatistics()
    {
        var tool = new StatsTool();
        var action = CreateAction(tool.Name, new { values = new[] { 1, 2, 3 } });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);
        var data = result.Data;

        Assert.AreEqual(1, data.GetProperty("min").GetDouble());
        Assert.AreEqual(3, data.GetProperty("max").GetDouble());
        Assert.AreEqual(2, data.GetProperty("mean").GetDouble());
        Assert.AreEqual(3, data.GetProperty("count").GetInt32());
    }

    [TestMethod]
    public async Task EmbeddingTool_GeneratesVector()
    {
        var service = new EmbeddingService();
        var tool = new EmbeddingTool(service);
        var action = CreateAction(tool.Name, new { text = "sample" });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);
        var data = result.Data;

        Assert.AreEqual("ok", result.Status);
        Assert.AreEqual(128, data.GetProperty("length").GetInt32());
    }

    [TestMethod]
    public async Task ClassifierTool_InfersIntent()
    {
        var tool = new ClassifierTool();
        var action = CreateAction(tool.Name, new { text = "Please install the update" });

        var result = await tool.ExecuteAsync(action, CancellationToken.None);

        Assert.AreEqual("setup", result.Data.GetProperty("intent").GetString());
    }

    private sealed class TestMemoryStore : IMemoryStore
    {
        public List<MemoryRecord> Records { get; } = new();

        public Task AddAsync(MemoryRecord record, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            Records.Add(record);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<MemoryRecord>> QueryAsync(string filter, int take, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var matches = Records
                .Where(r => r.Content.Contains(filter ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                .Take(take)
                .ToList();
            return Task.FromResult<IReadOnlyList<MemoryRecord>>(matches);
        }

        public Task PersistAsync(CancellationToken ct) => Task.CompletedTask;
    }
}
