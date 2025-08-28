// Project Name: LightweightAI.Core
// File Name: IFusionPipeline.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public interface IFusionPipeline
{
    IEnumerable<DecisionOutput> Process(IEnumerable<DecisionInput> inputs, FusionConfig cfg);
}