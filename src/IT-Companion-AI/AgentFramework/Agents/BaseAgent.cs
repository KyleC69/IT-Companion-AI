using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
#pragma warning disable SKEXP0110

namespace ITCompanionAI.AgentFramework.Agents;

internal class BaseAgent : Agent
{
    public override IAsyncEnumerable<AgentResponseItem<ChatMessageContent>> InvokeAsync(ICollection<ChatMessageContent> messages, AgentThread? thread = null, AgentInvokeOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override IAsyncEnumerable<AgentResponseItem<StreamingChatMessageContent>> InvokeStreamingAsync(ICollection<ChatMessageContent> messages, AgentThread? thread = null, AgentInvokeOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    protected override Task<AgentChannel> CreateChannelAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<string> GetChannelKeys()
    {
        throw new NotImplementedException();
    }

    protected override Task<AgentChannel> RestoreChannelAsync(string channelState, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

   



}
