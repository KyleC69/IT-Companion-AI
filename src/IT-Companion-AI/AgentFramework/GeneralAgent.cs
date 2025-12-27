#pragma warning disable SKEXP0001, SKEXP0101

using Microsoft.Extensions.AI;

using SkAgentGroup.AgentFramework.Memory;

namespace SkAgentGroup.AgentFramework;

public sealed class GeneralAgent : BaseAgent
{
    private const string AgentIdConst = "general";
    private const string AgentNameConst = "GeneralAgent";

    protected override string SystemPrompt => """
        You are a helpful, precise, technical assistant.
        Keep answers clear, structured, and correct. Your name is GeneralAgent.
        """;

    public GeneralAgent(IAgentMemory memory, IChatClient llm)
        : base(AgentIdConst, AgentNameConst, memory, llm)
    {
    }
}
