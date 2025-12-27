namespace SkAgentGroup.AgentFramework;

public sealed class SupervisorAgent
{
    private readonly PlanningAgent _planner;
    private readonly CodingAgent _coder;
    private readonly CriticAgent _critic;

    public SupervisorAgent(PlanningAgent planner, CodingAgent coder, CriticAgent critic)
    {
        _planner = planner;
        _coder = coder;
        _critic = critic;
    }

    public async Task<string> ExecuteGoalAsync(string goal, CancellationToken cancellationToken = default)
    {
        var plan = await _planner.RunAsync(goal, cancellationToken);

        // Simple heuristic: send whole plan to coder, then to critic.
        var code = await _coder.RunAsync(
            $"Here is the plan:\n{plan}\n\nWrite the code to implement this.",
            cancellationToken);

        var review = await _critic.RunAsync(
            $"Here is the code:\n{code}\n\nReview this code for correctness and improvements.",
            cancellationToken);

        return $"PLAN:\n{plan}\n\nCODE:\n{code}\n\nREVIEW:\n{review}";
    }
}