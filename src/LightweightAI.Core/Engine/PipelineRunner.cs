// Project Name: LightweightAI.Core
// File Name: PipelineRunner.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Config;
using LightweightAI.Core.Engine.FastDetectors;
using LightweightAI.Core.Refinery;



namespace LightweightAI.Core.Engine;


/// <summary>
///     Modern asynchronous pipeline runner: Source -> Normalize -> Encode -> Reduce -> Fast Detectors
///     -> Rolling Aggregate -> (optional) Fusion + Hysteresis + Rule Overrides -> (optional) Alert Dispatch + Drift
///     Detection.
/// </summary>
public sealed class PipelineRunner(
    INormalizer normalizer,
    FeatureEncoder encoder,
    FeatureReducer reducer,
    Ewma ewma,
    ZScore z,
    IncrementalKnn knn,
    IFusionEngine? fusion = null,
    FusionConfig? fusionConfig = null,
    HysteresisDecider? hysteresis = null,
    IAlertDispatcher? alerts = null,
    IDriftDetector? drift = null,
    RulesEngine? rules = null)
    : IPipelineRunner
{
    private readonly RollingWindowAggregator _agg = new(TimeSpan.FromMinutes(5));

    // optional for precedence overrides





    public async Task RunAsync(IEnumerable<SourceExecutionPlan> plans, CancellationToken ct = default)
    {
        foreach (SourceExecutionPlan plan in plans)
        {
            var request = new SourceRequest(plan.SourceKey, Parameters: plan.Parameters);
            await foreach (RawEvent raw in plan.Loader.LoadAsync(request, ct))
            {
                RawEvent norm = normalizer.Normalize(raw);
                var encoded = encoder.Encode(norm);
                var reduced = reducer.Reduce(encoded);

                DateTimeOffset now = DateTimeOffset.UtcNow;
                var s1 = ewma.UpdateAndScore(reduced, now);
                var s2 = z.UpdateAndScore(reduced, now);
                var s3 = knn.UpdateAndScore(reduced, now);
                var composite = (s1 + s2 + s3) / 3f;

                var driftEvent = false;
                double driftScore = 0;
                if (drift is not null)
                {
                    driftScore = drift.Update(composite, now, out var isDrift);
                    driftEvent = isDrift;
                }

                (AggregatedMetric metric, Snapshot snapshot) =
                    this._agg.Add(plan.SourceKey, norm.TimestampUtc, composite);

                var anomaly = new AnomalySignal(
                    Reading: new MetricReading(norm.TimestampUtc.UtcDateTime, plan.SourceKey, composite,
                        new ProvenanceKey(Guid.Empty, plan.SourceKey, norm.EventId)),
                    IsAnomaly: composite > 0.8f,
                    Score: composite,
                    PValue: 1.0 - composite,
                    Detector: "ensemble_fast",
                    Notes: string.Empty,
                    ZScore: s2
                );

                var decisionInput = new DecisionInput(metric, snapshot, anomaly);
                DecisionOutput? fused = null;

                if (fusion is not null && fusionConfig is not null)
                {
                    fused = fusion.Fuse(decisionInput, fusionConfig);

                    // Apply rule precedence hard floor overrides if rules provided
                    if (rules is not null)
                    {
                        var ctx = new EventContext(
                            norm.Host,
                            norm.SourceKey,
                            norm.EventId,
                            norm.TimestampUtc.UtcDateTime,
                            Enumerable.ToDictionary<KeyValuePair<string, object>, string, object>(norm.Fields, k => k.Key,
                                v => v.Value ?? new object() // Replace nulls with a non-null object
                            )
                        );
                        var adjusted = rules.ApplyOverrides(ctx, fused.Score);
                        if (Math.Abs(adjusted - fused.Score) > 1e-9)
                            fused = fused with { Score = adjusted, IsAlert = adjusted >= fusionConfig.AlertThreshold };
                    }

                    if (hysteresis is not null)
                    {
                        var latched = hysteresis.Decide(fused.Score);
                        fused = fused with { IsAlert = latched };
                    }

                    if (fused.IsAlert && alerts is not null)
                    {
                        
                        // Replace the construction of ProvenancedDecision with property initialization
                        var external = new ProvenancedDecision
                        {
                            Metrics = new MetricDecision
                            {
                                MetricKey= fused.MetricKey,
                                MetricWindowStart = fused.MetricWindowStart,
                                MetricWindowEnd = fused.MetricWindowEnd,
                                Score = fused.Score,
                                IsAlert = fused.IsAlert,
                                Payload = fused.Payload
                                
                                // Add other MetricDecision properties as needed
                            },
                            EventId = fused.EventId.GetHashCode(),
                            FusionSignature = fused.FusionSignature,
                            ModelId = fused.ModelId,
                            ModelVersion = fused.ModelVersion,
                            Severity = fused.Severity,
                            Risk = (float)Math.Clamp((double)fused.Score, 0, 1),
                            SeverityScaleRef = fused.SeverityScaleRef,
                            Timestamp = DateTime.UtcNow,
                            TimestampUtc = DateTimeOffset.UtcNow,
                            CorrelationId = string.Empty,
                            Summary = fused.IsAlert ? "ALERT" : "OK",
                            Tags = null // Set tags as needed
                        };
                        
                        
                        try
                        {
                            await alerts.DispatchAsync(external, ct);
                        }
                        catch
                        {
                        }
                    }
                }

#pragma warning disable CA1848
                if (fused is null)
                    Console.WriteLine(
                        $"[{plan.SourceKey}] {norm.TimestampUtc:O} comp={composite:F2} z={s2:F2} ema={s1:F2} knn={s3:F2} drift={driftScore:F2}{(driftEvent ? "*" : "")} count={metric.Count}");
                else
                    Console.WriteLine(
                        $"[{plan.SourceKey}] fused={fused.Score:F3} alert={fused.IsAlert} drift={driftScore:F2}{(driftEvent ? "*" : "")} window={fused.MetricWindowStart:t}-{fused.MetricWindowEnd:t}");
#pragma warning restore CA1848
            }
        }
    }
}