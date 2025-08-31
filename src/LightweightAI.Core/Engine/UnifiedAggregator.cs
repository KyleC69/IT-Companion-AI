// Project Name: LightweightAI.Core
// File Name: UnifiedAggregator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections.Immutable;

namespace LightweightAI.Core.Engine;


/// <summary>
///     Aggregates individual rule evaluation results into a coarse composite event signal,
///     computing an average score and a triggered flag (any rule matched). This provides
///     a lightweight summary fed into higher level statistical / fusion stages.
/// </summary>
public  class UnifiedAggregator : IUnifiedAggregator
{
    public AggregatedEvent Aggregate(IEnumerable<RuleResult>? ruleResults)
    {
        // Materialize once to avoid multiple enumeration and ensure correct LINQ extension resolution
        List<RuleResult> list = ruleResults?.ToList() ?? new List<RuleResult>();
        var score = list.Count != 0 ? list.Average(r => r.Score) : 0d;
        var triggered = list.Any(r => r.IsMatch);
        return new AggregatedEvent(score, triggered);
    }



    private readonly ImmutableArray<IAggregator> _aggregators;





    public UnifiedAggregator(IEnumerable<IAggregator> aggregators)
    {
        this._aggregators = aggregators.ToImmutableArray();
    }





    public IEnumerable<AggregatedMetric> AggregateUnified(IEnumerable<EventRecord> events, AggregatorConfig cfg)
    {
        var results = new List<AggregatedMetric>(this._aggregators.Length * 8);
        foreach (var agg in this._aggregators) results.AddRange(agg.Aggregate(events, cfg));

        foreach (var m in results
                     .OrderBy(m => m.WindowStart)
                     .ThenBy(m => m.Key, StringComparer.Ordinal))
            yield return m;
    }
}