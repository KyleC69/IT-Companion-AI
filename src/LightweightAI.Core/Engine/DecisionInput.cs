// Project Name: LightweightAI.Core
// File Name: DecisionInput.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public readonly record struct DecisionInput(
    AggregatedMetric Metric,
    Snapshot Trend,
    AnomalySignal Anomaly
);