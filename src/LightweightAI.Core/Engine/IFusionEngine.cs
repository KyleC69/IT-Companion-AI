// Project Name: LightweightAI.Core
// File Name: IFusionEngine.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine;


namespace AICompanion.Tests;


public interface IFusionEngine
{
    DecisionOutput Fuse(DecisionInput input, FusionConfig cfg);
}