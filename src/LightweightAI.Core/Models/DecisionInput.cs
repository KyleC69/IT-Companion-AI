// Project Name: LightweightAI.Core
// File Name: DecisionInput.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine;



namespace LightweightAI.Core.Models;


/// <summary>
///     Data contract passed into the fusion engine containing the aggregated metric
///     envelope, latest statistical snapshot, and anomaly signal for unified scoring.
/// </summary>
public readonly record struct DecisionInput(
    AggregatedMetric Metric,
    Snapshot Trend,
    Engine.AnomalySignal Anomaly
);