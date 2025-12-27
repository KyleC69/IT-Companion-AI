using SkAgentGroup.AgentFramework.Memory;

using System;
using System.Collections.Generic;
using System.Text;

namespace SkAgentGroup.AgentFramework;


public sealed class AgentOrchestrator
{
    private readonly PlanningAgent _planner;
    private readonly CodingAgent _coder;
    private readonly CriticAgent _critic;
    private readonly IAgentMemory _memory;

    public AgentOrchestrator(
        PlanningAgent planner,
        CodingAgent coder,
        CriticAgent critic,
        IAgentMemory memory)
    {
        _planner = planner;
        _coder = coder;
        _critic = critic;
        _memory = memory;
    }

    public async Task<string> ExecuteGoalAsync(string goal, CancellationToken cancellationToken = default)
    {
        var loopCount = 0;
        var maxLoops = 10;

        string? finalAnswer = null;

        while (loopCount++ < maxLoops)
        {
            // 1. Ask planner for next steps
            var plan = await _planner.RunAsync(goal, cancellationToken);

            await _memory.StoreAsync(
                agentId: "planner",
                agentName: "PlanningAgent",
                text: plan,
                memoryType: "plan",
                tags: new Dictionary<string, string> { ["loop"] = loopCount.ToString() },
                cancellationToken: cancellationToken);

            // 2. Parse plan into steps
            var steps = ExtractSteps(plan);

            if (steps.Count == 0)
                return $"Planner returned no actionable steps:\n{plan}";

            foreach (var step in steps)
            {
                string output;

                // 3. Route step to correct agent
                switch (step.Agent)
                {
                    case "coder":
                        output = await _coder.RunAsync(step.Instruction, cancellationToken);
                        break;

                    case "planner":
                        output = await _planner.RunAsync(step.Instruction, cancellationToken);
                        break;

                    case "critic":
                        output = await _critic.RunAsync(step.Instruction, cancellationToken);
                        break;

                    default:
                        output = $"Unknown agent '{step.Agent}' for step: {step.Instruction}";
                        break;
                }

                // 4. Store memory
                await _memory.StoreAsync(
                    agentId: step.Agent,
                    agentName: step.Agent,
                    text: output,
                    memoryType: "result",
                    tags: new Dictionary<string, string>
                    {
                        ["step"] = step.Number.ToString(),
                        ["loop"] = loopCount.ToString()
                    },
                    cancellationToken: cancellationToken);



                // 5. Critic reviews each step
                var review = await _critic.RunAsync(
                    $"Review this output:\n{output}",
                    cancellationToken);

                await _memory.StoreAsync(
                    agentId: "critic",
                    agentName: "CriticAgent",
                    text: review,
                    memoryType: "review",
                    tags: new Dictionary<string, string>
                    {
                        ["step"] = step.Number.ToString(),
                        ["loop"] = loopCount.ToString()
                    },
                    cancellationToken: cancellationToken);

                // 6. Supervisor checks if done
                if (IsGoalSatisfied(goal, output, review))
                {
                    finalAnswer = output;
                    break;
                }
            }

            if (finalAnswer != null)
                break;
        }

        return finalAnswer ?? "Goal not fully satisfied after max iterations.";
    }

    private bool IsGoalSatisfied(string goal, string output, string review)
    {
        // Very simple heuristic — you can replace with LLM-based evaluation
        return review.Contains("looks good", StringComparison.OrdinalIgnoreCase)
            || review.Contains("complete", StringComparison.OrdinalIgnoreCase);
    }

    private List<PlanStep> ExtractSteps(string plan)
    {
        var steps = new List<PlanStep>();
        var lines = plan.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        int number = 1;
        foreach (var line in lines)
        {
            if (!line.Contains(":")) continue;

            var parts = line.Split(':', 2);
            var agent = parts[0].Trim().ToLowerInvariant();
            var instruction = parts[1].Trim();

            steps.Add(new PlanStep(number++, agent, instruction));
        }

        return steps;
    }

    private record PlanStep(int Number, string Agent, string Instruction);
}