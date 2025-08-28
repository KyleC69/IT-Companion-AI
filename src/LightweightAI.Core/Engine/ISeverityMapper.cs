// Project Name: LightweightAI.Core
// File Name: ISeverityMapper.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using AICompanion.Tests;


namespace LightweightAI.Core.Engine;


public interface ISeverityMapper
{
    int MapSeverity(object modelOutput, FusionBroker.ConfigSnapshot config);
}