// Project Name: LightweightAI.Core
// File Name: UnifiedAggregator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


/// <summary>
///     Aggregates individual rule evaluation results into a coarse composite event signal,
///     computing an average score and a triggered flag (any rule matched). This provides
///     a lightweight summary fed into higher level statistical / fusion stages.
/// </summary>
public class UnifiedAggregator
{
    public AggregatedEvent Aggregate(IEnumerable<RuleResult>? ruleResults)
    {
        // Materialize once to avoid multiple enumeration and ensure correct LINQ extension resolution
        List<RuleResult> list = ruleResults?.ToList() ?? new List<RuleResult>();
        var score = list.Count != 0 ? list.Average(r => r.Score) : 0d;
        var triggered = list.Any(r => r.IsMatch);
        return new AggregatedEvent(score, triggered);
    }
}