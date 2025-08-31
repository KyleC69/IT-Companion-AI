// Project Name: LightweightAI.Core
// File Name: ISeverityMapper.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


// namespace LightweightAI.Core.adjusted to match project/directory pattern (LightweightAI.Core.Engine).
// Previously referenced AICompanion.Tests types creating inconsistency.


using LightweightAI.Core.Engine.Fusion;



namespace LightweightAI.Core.Interfaces;


public interface ISeverityMapper
{
    int MapSeverity(object modelOutput, FusionBroker.ConfigSnapshot config);
}