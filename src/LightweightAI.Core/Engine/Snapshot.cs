// Project Name: LightweightAI.Core
// File Name: Snapshot.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public readonly record struct Snapshot(
    DateTimeOffset At,
    long Count,
    double Sum,
    double Mean,
    double StdDev,
    double Ema,
    double TrendDelta
);