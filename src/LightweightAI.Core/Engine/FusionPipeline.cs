// Project Name: LightweightAI.Core
// File Name: FusionPipeline.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using AICompanion.Tests;


namespace LightweightAI.Core.Engine;


public sealed class FusionPipeline : IFusionPipeline
{
    private readonly IFusionEngine _engine;





    public FusionPipeline(IFusionEngine engine)
    {
        this._engine = engine;
    }





    public IEnumerable<DecisionOutput> Process(IEnumerable<DecisionInput> inputs, FusionConfig cfg)
    {
        foreach (DecisionInput i in inputs)
            yield return this._engine.Fuse(i, cfg);
    }
}