// Project Name: LightweightAI.Core
// File Name: SeverityMapper.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Fusion;
using LightweightAI.Core.Interfaces;


namespace LightweightAI.Core.Engine;


// === SeverityMapper.cs ===
/// <summary>
///     Maps arbitrary model output objects (converted to a string key) into an integer severity score
///     using a provided lookup dictionary. Acts as an indirection layer so model/raw detector outputs
///     can be decoupled from downstream fusion / alerting severity scales and easily reconfigured.
/// </summary>
public class SeverityMapper(Dictionary<string, int> severityMap) : ISeverityMapper
{
    public int MapSeverity(object? modelOutput, FusionBroker.ConfigSnapshot config)
    {
        // Placeholder logic — replace with modelOutput interpretation
        var key = modelOutput?.ToString() ?? "UNKNOWN";
        return severityMap.TryGetValue(key, out var score) ? score : 0;
    }
}