// Project Name: LightweightAI.Core
// File Name: ProcessConnectionCorrelatorConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Refinery;


public sealed class ProcessConnectionCorrelatorConfig
{
    public bool DeltaOnly { get; init; } = true;
    public bool AuditLog { get; init; } = true;
}