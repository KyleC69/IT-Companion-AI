// Project Name: SKAgent
// File Name: ApiSourceLocation.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.AgentFramework.Models;


public class ApiSourceLocation
{
    public string? FilePath { get; set; }
    public int StartLine { get; set; }
    public int EndLine { get; set; }
}