using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;

namespace ITCompanionAI.AgentFramework.Planning;

#pragma warning disable SKEXP0110
internal class ApprovalTerminationStrategy : TerminationStrategy
#pragma warning restore SKEXP0110
{
    protected override Task<bool> ShouldAgentTerminateAsync(Agent agent, IReadOnlyList<ChatMessageContent> history, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(false);
    }
}