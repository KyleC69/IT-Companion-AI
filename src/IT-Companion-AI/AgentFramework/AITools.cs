using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using SkAgentGroup.AgentFramework.Agents;
using SkAgentGroup.AgentFramework.Memory;

namespace SkAgentGroup.AgentFramework.Tools;

// ============================================================
// FILE TOOLS
// ============================================================
public sealed class FileTools
{
    public async Task<string> ReadFileAsync(string path)
    {
        return await File.ReadAllTextAsync(path);
    }

    public async Task WriteFileAsync(string path, string content)
    {
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        await File.WriteAllTextAsync(path, content);
    }
}

// ============================================================
// WEB TOOLS
// ============================================================
public sealed class WebTools
{
    private readonly HttpClient _http = new();

    public async Task<string> GetAsync(string url)
    {
        return await _http.GetStringAsync(url);
    }
}

// ============================================================
// MEMORY TOOLS
// ============================================================
public sealed class MemoryTools
{
    private readonly IAgentMemory _memory;

    public MemoryTools(IAgentMemory memory)
    {
        _memory = memory;
    }

    public async Task StoreAsync(string agentId, string text, string type = "note")
    {
        var record = new AgentMemoryRecord
        {
            AgentId = agentId,
            AgentName = agentId,
            Text = text,
            MemoryType = type
        };

        await _memory.StoreAsync(record);
    }

    public async Task<IReadOnlyList<AgentMemoryRecord>> SearchAsync(string agentId, string query)
    {
        return await _memory.SearchAsync(agentId, query);
    }
}

// ============================================================
// AGENT-TO-AGENT TOOLS
// ============================================================
public sealed class AgentTools
{
    private readonly IServiceProvider _services;

    public AgentTools(IServiceProvider services)
    {
        _services = services;
    }

    public async Task<string> AskAgentAsync(string agentName, string input)
    {
        return agentName switch
        {
            "planner" => await _services.GetRequiredService<PlanningAgent>().HandleTaskAsync(input),
            "coder"   => await _services.GetRequiredService<CodingAgent>().HandleTaskAsync(input),
            "critic"  => await _services.GetRequiredService<CriticAgent>().HandleTaskAsync(input),
            _         => $"Unknown agent '{agentName}'"
        };
    }
}

// ============================================================
// TOOL REGISTRATION EXTENSION
// ============================================================
public static class AgentToolExtensions
{
    public static void AddMinimalTools(this BaseAgent agent, IServiceProvider sp)
    {
        var files = sp.GetRequiredService<FileTools>();
        var web = sp.GetRequiredService<WebTools>();
        var memory = sp.GetRequiredService<MemoryTools>();
        var agents = sp.GetRequiredService<AgentTools>();

        agent.AddTool("read_file", async args =>
        {
            var path = args["path"].ToString()!;
            return await files.ReadFileAsync(path);
        });

        agent.AddTool("write_file", async args =>
        {
            var path = args["path"].ToString()!;
            var content = args["content"].ToString()!;
            await files.WriteFileAsync(path, content);
            return "ok";
        });

        agent.AddTool("http_get", async args =>
        {
            var url = args["url"].ToString()!;
            return await web.GetAsync(url);
        });

        agent.AddTool("memory_store", async args =>
        {
            var text = args["text"].ToString()!;
            var type = args.ContainsKey("type") ? args["type"].ToString()! : "note";
            await memory.StoreAsync(agent.AgentId, text, type);
            return "stored";
        });

        agent.AddTool("memory_search", async args =>
        {
            var query = args["query"].ToString()!;
            var results = await memory.SearchAsync(agent.AgentId, query);
            return string.Join("\n---\n", results.Select(r => r.Text));
        });

        agent.AddTool("ask_agent", async args =>
        {
            var target = args["agent"].ToString()!;
            var input = args["input"].ToString()!;
            return await agents.AskAgentAsync(target, input);
        });
    }
}