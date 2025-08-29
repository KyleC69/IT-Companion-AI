// Project Name: LightweightAI.Core
// File Name: RulesEngine.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;

/// <summary>
/// Hosts and evaluates a collection of <see cref="IRule"/> instances against an incoming
/// <see cref="EventContext"/>. Provides a simple, in‑memory rule evaluation pipeline whose
/// results (match flag + score) are later aggregated and potentially fused with statistical
/// / model driven signals. Adds optional precedence / hard‑floor override semantics.
/// </summary>
public class RulesEngine
{
    private readonly List<(IRule rule, int priority, double? hardFloor)> _rules = new();

    /// <summary>
    /// Adds a rule with optional priority (higher evaluated first) and severity hard floor (minimum fused score if matched).
    /// </summary>
    public void AddRule(IRule rule, int priority = 0, double? hardFloor = null)
    {
        _rules.Add((rule, priority, hardFloor));
        _rules.Sort(static (a, b) => b.priority.CompareTo(a.priority));
    }

    /// <summary>
    /// Evaluates all registered rules against the supplied event context yielding individual
    /// <see cref="RuleResult"/> records on demand to minimize allocations.
    /// </summary>
    public IEnumerable<RuleResult> Evaluate(EventContext context)
    {
        foreach (var entry in _rules)
            yield return entry.rule.Evaluate(context);
    }

    /// <summary>
    /// Applies precedence / hard floor overrides to a fused decision score returning the adjusted score.
    /// If a matching rule with a higher hard floor is found, the score is elevated to that floor.
    /// </summary>
    public double ApplyOverrides(EventContext ctx, double fusedScore)
    {
        foreach (var (rule, _, hardFloor) in _rules)
        {
            if (hardFloor is null) continue;
            var res = rule.Evaluate(ctx);
            if (res.IsMatch && fusedScore < hardFloor.Value)
                fusedScore = hardFloor.Value;
        }
        return fusedScore;
    }
}