// Project Name: LightweightAI.Core
// File Name: WmiQuerySpec.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed class WmiQuerySpec
{
    public string Namespace { get; init; } = @"root\cimv2"; // e.g. root\cimv2
    public string Wql { get; init; } = "SELECT * FROM Win32_OperatingSystem";
    public List<string> Properties { get; init; } = new();
}