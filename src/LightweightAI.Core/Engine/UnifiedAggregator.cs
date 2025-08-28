// Project Name: LightweightAI.Core
// File Name: UnifiedAggregator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public class UnifiedAggregator
{
    public AggregatedEvent Aggregate(IEnumerable<RuleResult> ruleResults)
    {
        var score = ruleResults.Average(r => r.Score);
        var triggered = ruleResults.Any(r => r.IsMatch);
        return new AggregatedEvent(score, triggered);
    }
}