// Project Name: LightweightAI.Core
// File Name: FusionPipeline.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Config;



namespace LightweightAI.Core.Engine.Fusion;


public sealed class FusionPipeline(IFusionEngine engine) : IFusionPipeline
{
    public IEnumerable<DecisionOutput> Process(IEnumerable<DecisionInput> inputs, FusionConfig cfg)
    {
        foreach (DecisionInput i in inputs)
            yield return engine.Fuse(i, cfg);
    }
}