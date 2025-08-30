// Project Name: LightweightAI.Core
// File Name: AnomalyType.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Builders;


/// <summary>
///     Classification of anomaly types detected in memory or event streams.
/// </summary>
public enum AnomalyType
{
    None = 0,
    MemoryLeak,
    UnexpectedGrowth,
    AccessViolation,
    DataCorruption,
    PerformanceDegradation,
    Unknown
}