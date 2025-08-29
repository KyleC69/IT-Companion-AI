// Project Name: LightweightAI.Core
// File Name: SeverityMapper.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Fusion;


namespace LightweightAI.Core.Engine;


// === SeverityMapper.cs ===
public class SeverityMapper : ISeverityMapper
{
    private readonly Dictionary<string, int> _severityMap;





    public SeverityMapper(Dictionary<string, int> severityMap)
    {
        this._severityMap = severityMap;
    }





    public int MapSeverity(object modelOutput, FusionBroker.ConfigSnapshot config)
    {
        // Placeholder logic — replace with modelOutput interpretation
        var key = modelOutput?.ToString() ?? "UNKNOWN";
        return this._severityMap.TryGetValue(key, out var score) ? score : 0;
    }





}