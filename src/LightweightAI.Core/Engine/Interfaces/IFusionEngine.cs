// Project Name: LightweightAI.Core
// File Name: IFusionEngine.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


// Namespace adjusted to match project/directory pattern (LightweightAI.Core.Engine).
// Previously in AICompanion.Tests causing cross-project namespace inconsistency.


namespace LightweightAI.Core.Engine;


public interface IFusionEngine
{
    DecisionOutput Fuse(DecisionInput input, FusionConfig cfg);
}