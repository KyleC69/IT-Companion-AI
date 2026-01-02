using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;

namespace ITCompanionAI.AgentFramework.Planning;

internal class ApprovalTerminationStrategy : TerminationStrategy
{
    public object Agents { get; set; }
    public int MaximumIterations { get; set; }

    protected override Task<bool> ShouldAgentTerminateAsync(Agent agent, IReadOnlyList<ChatMessageContent> history, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}