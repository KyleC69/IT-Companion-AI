using SkAgentGroup.AgentFramework;

namespace SkAgentGroup;

public class AgentLoop
{
    private readonly GeneralAgent _agent;

    public AgentLoop()
    {
        _agent = App.GetService<GeneralAgent>();
    }

    public Task<string> RunAsync(string userInput)
    {
        return _agent.HandleTaskAsync(userInput);
    }
}






