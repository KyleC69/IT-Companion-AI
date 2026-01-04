// Project Name: SKAgent
// File Name: BaseAgent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


#pragma warning disable SKEXP0110 // Agent base class is experimental

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;


namespace ITCompanionAI.AgentFramework.Agents;


internal class BaseAgent : Agent
{
    public override IAsyncEnumerable<AgentResponseItem<ChatMessageContent>> InvokeAsync(
        ICollection<ChatMessageContent> messages, AgentThread? thread = null, AgentInvokeOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }





    public override IAsyncEnumerable<AgentResponseItem<StreamingChatMessageContent>> InvokeStreamingAsync(
        ICollection<ChatMessageContent> messages, AgentThread? thread = null, AgentInvokeOptions? options = null,
        CancellationToken cancellationToken = default)
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