// Project Name: LightweightAI.Core
// File Name: MemoryAnomaly.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed class MemoryAnomaly
{
    public int Pid { get; set; }
    public string ProcessName { get; set; } = "";
    public string BaseAddress { get; set; } = "";
    public long RegionSize { get; set; }
    public string Protect { get; set; } = "";
    public string Type { get; set; } = "";
    public string MappedPath { get; set; } = "";
    public List<string> RulesTriggered { get; set; } = new();
    public DateTime FirstSeen { get; set; }
    public DateTime LastSeen { get; set; }
}