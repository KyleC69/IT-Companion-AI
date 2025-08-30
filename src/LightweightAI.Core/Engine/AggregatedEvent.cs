// Project Name: LightweightAI.Core
// File Name: AggregatedEvent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


/// <summary>
///     Minimal representation of combined rule evaluation state used as an input signal
///     for subsequent metric aggregation or anomaly analysis.
/// </summary>
public record AggregatedEvent(double Score, bool Triggered);