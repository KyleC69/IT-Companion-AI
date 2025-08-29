// Project Name: LightweightAI.Core
// File Name: PipelineRunner.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers

using LightweightAI.Core.Abstractions;
using LightweightAI.Core.Refinery;
using LightweightAI.Core.Engine.FastDetectors;
using LightweightAI.Core.Engine.Intake;
using LightweightAI.Core.Engine.SlowDetectors;

namespace LightweightAI.Core.Engine;

/// <summary>
/// Modern asynchronous pipeline runner: Source -> Normalize -> Encode -> Reduce -> Fast Detectors
/// -> Rolling Aggregate -> (optional) Fusion + Hysteresis + Rule Overrides -> (optional) Alert Dispatch + Drift Detection.
/// </summary>
public sealed class PipelineRunner : IPipelineRunner
{
    private readonly INormalizer _normalizer;
    private readonly FeatureEncoder _encoder;
    private readonly FeatureReducer _reducer;
    private readonly Ewma _ewma;
    private readonly ZScore _z;
    private readonly IncrementalKnn _knn;
    private readonly RollingWindowAggregator _agg = new(TimeSpan.FromMinutes(5));
    private readonly IFusionEngine? _fusion;
    private readonly FusionConfig? _fusionConfig;
    private readonly HysteresisDecider? _hysteresis;
    private readonly IAlertDispatcher? _alerts;
    private readonly IDriftDetector? _drift;
    private readonly RulesEngine? _rules; // optional for precedence overrides

    public PipelineRunner(
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
    {
        _normalizer = normalizer;
        _encoder = encoder;
        _reducer = reducer;
        _ewma = ewma;
        _z = z;
        _knn = knn;
        _fusion = fusion;
        _fusionConfig = fusionConfig;
        _hysteresis = hysteresis;
        _alerts = alerts;
        _drift = drift;
        _rules = rules;
    }

    public async Task RunAsync(IEnumerable<SourceExecutionPlan> plans, CancellationToken ct = default)
    {
        foreach (var plan in plans)
        {
            var request = new SourceRequest(plan.SourceKey, Parameters: plan.Parameters);
            await foreach (var raw in plan.Loader.LoadAsync(request, ct))
            {
                var norm = _normalizer.Normalize(raw);
                var encoded = _encoder.Encode(norm);
                var reduced = _reducer.Reduce(encoded);

                var now = DateTimeOffset.UtcNow;
                var s1 = _ewma.UpdateAndScore(reduced, now);
                var s2 = _z.UpdateAndScore(reduced, now);
                var s3 = _knn.UpdateAndScore(reduced, now);
                var composite = (s1 + s2 + s3) / 3f;

                bool driftEvent = false;
                double driftScore = 0;
                if (_drift is not null)
                {
                    driftScore = _drift.Update(composite, now, out var isDrift);
                    driftEvent = isDrift;
                }

                var (metric, snapshot) = _agg.Add(plan.SourceKey, norm.TimestampUtc, composite);

                var anomaly = new AnomalySignal(
                    Reading: new MetricReading(norm.TimestampUtc.UtcDateTime, plan.SourceKey, composite, new ProvenanceKey(Guid.Empty, plan.SourceKey, norm.EventId)),
                    IsAnomaly: composite > 0.8f,
                    Score: composite,
                    PValue: 1.0 - composite,
                    Detector: "ensemble_fast",
                    Notes: string.Empty,
                    ZScore: s2
                );

                var decisionInput = new DecisionInput(metric, snapshot, anomaly);
                DecisionOutput? fused = null;

                if (_fusion is not null && _fusionConfig is not null)
                {
                    fused = _fusion.Fuse(decisionInput, _fusionConfig);

                    // Apply rule precedence hard floor overrides if rules provided
                    if (_rules is not null)
                    {
                        var ctx = new EventContext(norm.Host, norm.SourceKey, norm.EventId, norm.TimestampUtc.UtcDateTime, norm.Fields.ToDictionary(k => k.Key, v => (object?)v.Value));
                        var adjusted = _rules.ApplyOverrides(ctx, fused.Score);
                        if (Math.Abs(adjusted - fused.Score) > 1e-9)
                            fused = fused with { Score = adjusted, IsAlert = adjusted >= _fusionConfig.AlertThreshold };
                    }

                    if (_hysteresis is not null)
                    {
                        var latched = _hysteresis.Decide(fused.Score);
                        fused = fused with { IsAlert = latched };
                    }

                    if (fused.IsAlert && _alerts is not null)
                    {
                        var external = new Abstractions.ProvenancedDecision(
                            TimestampUtc: DateTimeOffset.UtcNow,
                            CorrelationId: fused.MetricKey,
                            Risk: (float)Math.Clamp(fused.Score, 0, 1),
                            Severity: fused.Score.ToString("F2"),
                            Summary: fused.IsAlert ? "ALERT" : "OK",
                            DetailJson: string.Empty,
                            ProvenanceChainHash: string.Empty);
                        try { await _alerts.DispatchAsync(external, ct); } catch { }
                    }
                }

#pragma warning disable CA1848
                if (fused is null)
                    Console.WriteLine($"[{plan.SourceKey}] {norm.TimestampUtc:O} comp={composite:F2} z={s2:F2} ema={s1:F2} knn={s3:F2} drift={driftScore:F2}{(driftEvent ? "*" : "")} count={metric.Count}");
                else
                    Console.WriteLine($"[{plan.SourceKey}] fused={fused.Score:F3} alert={fused.IsAlert} drift={driftScore:F2}{(driftEvent ? "*" : "")} window={fused.MetricWindowStart:t}-{fused.MetricWindowEnd:t}");
#pragma warning restore CA1848
            }
        }
    }
}