using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Agents.Runtime.Core;

using SkAgentGroup.AgentFramework.Memory;

namespace SkAgentGroup.AgentFramework;

public sealed class CriticAgent : BaseAgent
{
    public CriticAgent(IAgentMemory memory, IChatClient llm)
        : base("critic", "CriticAgent", memory, llm) { }

    protected override string SystemPrompt =>
        """
        You are the CriticAgent. You review plans and code for correctness,
        safety, and clarity. You point out issues and suggest improvements.
        """;
}
