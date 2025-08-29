// Project Name: LightweightAI.Core
// File Name: UnifiedAggregator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers

using System.Linq;

namespace LightweightAI.Core.Engine;

public class UnifiedAggregator
{
    public AggregatedEvent Aggregate(IEnumerable<RuleResult> ruleResults)
    {
        // Materialize once to avoid multiple enumeration and ensure correct LINQ extension resolution
        var list = ruleResults?.ToList() ?? new List<RuleResult>();
        double score = list.Count != 0 ? list.Average(r => r.Score) : 0d;
        bool triggered = list.Any(r => r.IsMatch);
        return new AggregatedEvent(score, triggered);
    }
}