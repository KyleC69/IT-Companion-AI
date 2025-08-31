// Project Name: LightweightAI.Core
// File Name: IFusionEngine.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


// namespace LightweightAI.Core.adjusted to match project/directory pattern (LightweightAI.Core.Engine).
// Previously in AICompanion.Tests causing cross-project namespace LightweightAI.Core.inconsistency.


using LightweightAI.Core.Config;



namespace LightweightAI.Core.Interfaces;


public interface IFusionEngine
{
    DecisionOutput Fuse(DecisionInput input, FusionConfig cfg);
}