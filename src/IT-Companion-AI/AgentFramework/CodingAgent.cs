using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Agents.Runtime.Core;
using Microsoft.SemanticKernel.Agents.Runtime;
using SkAgentGroup.AgentFramework.Memory;
using Microsoft.Extensions.Logging;

namespace SkAgentGroup.AgentFramework;

public sealed class CodingAgent : BaseAgent
{
    public CodingAgent(AgentId agentId, IAgentRuntime agentRuntime, string description, ILogger? logger = null)
        : base(agentId, agentRuntime, description, logger) { }

    protected string SystemPrompt { get; } = """You are the CodingAgent. You write code to implement plans. You produce minimal, correct, well-structured code with explanations when needed.""";

    // Factory method to create CodingAgent with memory and llm
    public static CodingAgent Create(IAgentMemory memory, IChatClient llm, ILogger? logger = null)
    {
        var agentId = new AgentId("coder");
        var agentRuntime = new CodingAgentRuntime(memory, llm);
        return new CodingAgent(agentId, agentRuntime, "CodingAgent", logger);
    }
}

// Example implementation of IAgentRuntime for CodingAgent
internal sealed class CodingAgentRuntime : IAgentRuntime
{
    private readonly IAgentMemory _memory;
    private readonly IChatClient _llm;

    public CodingAgentRuntime(IAgentMemory memory, IChatClient llm)
    {
        _memory = memory;
        _llm = llm;
    }

    // Implement required members of IAgentRuntime here
}
