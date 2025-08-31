// Project Name: LightweightAI.Core
// File Name: Snapshot.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


/// <summary>
///     Represents a point-in-time statistical summary over a rolling metric window
///     including basic descriptive statistics and trend indicators used by anomaly
///     and fusion layers.
/// </summary>
public readonly record struct Snapshot(
    DateTimeOffset At,
    long Count,
    double Sum,
    double Mean,
    double StdDev,
    double Ema,
    double TrendDelta
);