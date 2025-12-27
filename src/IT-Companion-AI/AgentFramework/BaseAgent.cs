using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

using SkAgentGroup.AgentFramework.Memory;

using System.Buffers;
using System.Text;
using System.Text.Json.Nodes;

namespace SkAgentGroup.AgentFramework;





public abstract class BaseAgent
{
    public string AgentId { get; }
    public string AgentName { get; }

    // Tool registry
    private readonly Dictionary<string, Func<Dictionary<string, object>, Task<string>>> _tools =
        new(StringComparer.OrdinalIgnoreCase);

    protected readonly IAgentMemory _memory;
    protected readonly IChatClient _llm;

    protected BaseAgent(string agentId, string agentName, IAgentMemory memory, IChatClient llm)
    {
        AgentId = agentId;
        AgentName = agentName;
        _memory = memory;
        _llm = llm;
    }

    /// <summary>
    /// The agent’s internal rules/rails.
    /// Each agent overrides this to define its personality and constraints.
    /// </summary>
    protected virtual string SystemPrompt { get; }

    public void AddTool(string name, Func<Dictionary<string, object>, Task<string>> handler)
    {
        _tools[name] = handler;
    }

    /// <summary>
    /// Main entry point for agent tasks.
    /// Now supports tool-calling.
    /// </summary>
    public virtual async Task<string> HandleTaskAsync(
        string userInput,
        CancellationToken cancellationToken = default)
    {
        // 1. Recall relevant memories
        var memories = await _memory.SearchAsync(
            AgentId,
            userInput,
            topK: 5,
            cancellationToken);

        // 2. Build context
        string context = BuildContext(memories);

        // 3. Build initial prompt
        string prompt = BuildPrompt(context, userInput);

        // 4. Enter LLM + tool loop
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, SystemPrompt),
            new(ChatRole.User, prompt)
        };

        while (true)
        {
            var response = await _llm.CompleteAsync(messages, cancellationToken: cancellationToken);
            var content = response.Message.Text ?? "";

            // 5. Check for tool call
            if (TryParseToolCall(content, out var toolName, out var args))
            {
                if (!_tools.TryGetValue(toolName, out var handler))
                {
                    messages.Add(new(ChatRole.Assistant, $"Unknown tool: {toolName}"));
                    continue;
                }

                // Execute tool
                var result = await handler(args);

                // Feed result back to LLM
                messages.Add(new(ChatRole.Assistant, $"Tool result:\n{result}"));
                continue;
            }

            // 6. No tool call → final answer
            await StoreMemoryAsync(userInput, content, cancellationToken);
            return content;
        }
    }

    private string BuildContext(IReadOnlyList<AgentMemoryRecord> memories)
    {
        if (memories.Count == 0)
            return "No relevant memory found.";

        var sb = new StringBuilder();
        sb.AppendLine("Relevant past memories:");

        foreach (var m in memories)
        {
            sb.AppendLine($"- [{m.CreatedAtUtc:u}] {m.Text}");
        }

        return sb.ToString();
    }

    private string BuildPrompt(string context, string userInput)
    {
        return $@"
CONTEXT:
{context}

USER INPUT:
{userInput}

AGENT RESPONSE:";
    }

    private async Task StoreMemoryAsync(
        string userInput,
        string output,
        CancellationToken cancellationToken)
    {
        var record = new AgentMemoryRecord
        {
            AgentId = AgentId,
            AgentName = AgentName,
            Text = $"{userInput}\nResponse: {output}",
            MemoryType = "episodic"
        };

        await _memory.StoreAsync(record, cancellationToken);
    }

    private bool TryParseToolCall(
        string content,
        out string toolName,
        out Dictionary<string, object> args)
    {
        toolName = "";
        args = new();

        try
        {
            var json = JsonNode.Parse(content)?.AsObject();
            if (json == null) return false;

            if (!json.TryGetPropertyValue("tool", out var toolNode)) return false;
            if (!json.TryGetPropertyValue("arguments", out var argsNode)) return false;

            toolName = toolNode!.ToString();

            foreach (var kvp in argsNode!.AsObject())
            {
                args[kvp.Key] = kvp.Value!.GetValue<object>();
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}





//###############################################
//###############################################
//              T  O  O  L  S
//###############################################
//###############################################



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
    } }