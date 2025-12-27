using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace SkAgentGroup.AgentFramework;

public class SimpleAgent
{
    private readonly Kernel _kernel;
    private ChatHistory _history;
    private const string SystemPrompt = """
        You are a helpful, precise, technical assistant.
        Keep answers clear, structured, and correct.
        """;
    private readonly string _agentName = "SimpleAgent";


    public SimpleAgent(Kernel kernel)
    {
        _kernel = kernel;
        _history = new ChatHistory();
        _history.AddSystemMessage(SystemPrompt);
    }

    public async Task<string> RespondAsync(string input, CancellationToken cancellationToken = default)
    {
        var chat = _kernel.GetRequiredService<IChatCompletionService>();
        _history.AddUserMessage(input);


        var result = await chat.GetChatMessageContentAsync(
            _history,
            kernel: _kernel,
            cancellationToken: cancellationToken);

        var response = result?.Content ?? string.Empty;

        _history.AddAssistantMessage(response);

        return response;

    }
}