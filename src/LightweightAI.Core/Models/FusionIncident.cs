// Project Name: LightweightAI.Core
// File Name: FusionIncident.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed class FusionIncident
{
    public string IncidentId { get; set; } = "";
    public string Severity { get; set; } = "";
    public double Confidence { get; set; }
    public DateTime FirstSeen { get; set; }
    public DateTime LastSeen { get; set; }
    public string Description { get; set; } = "";
    public Dictionary<string, object> ContributingRecords { get; set; } = new();
}