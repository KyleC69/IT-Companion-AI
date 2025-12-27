using Microsoft.Extensions.AI;

using SkAgentGroup.AgentFramework.Memory;

namespace SkAgentGroup.AgentFramework;


public sealed class PlanningAgent : BaseAgent
{
    public PlanningAgent(
        IAgentMemory memory,
        IChatClient llm)
        : base(
            agentId: "planner",
            agentName: "PlanningAgent",
            memory: memory,
            llm: llm)
    {
    }

    protected override string SystemPrompt =>
        """
        You are the PlanningAgent. Your job is to break down tasks into clear,
        actionable steps. You think methodically, avoid assumptions, and produce
        structured plans. You do not write code. You do not execute tasks.
        You only plan.

        Always:
        - Identify the goal
        - Break it into steps
        - Highlight dependencies
        - Suggest which agent should handle each step
        """;
}
