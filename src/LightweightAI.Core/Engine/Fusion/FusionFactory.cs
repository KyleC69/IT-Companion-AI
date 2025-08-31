// Project Name: LightweightAI.Core
// File Name: FusionFactory.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine.Fusion;


public static class FusionFactory
{
    public static IFusionEngine CreateEngine()
    {
        return new FusionEngine();
    }





    public static IFusionPipeline CreatePipeline()
    {
        return new FusionPipeline(CreateEngine());
    }
}