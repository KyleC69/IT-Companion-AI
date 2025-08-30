// Project Name: LightweightAI.Core
// File Name: PipelineTest.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


// DefaultNormalizer, OneHotEncoder, detectors


using LightweightAI.Core.Analyzers;
using LightweightAI.Core.Engine;
using LightweightAI.Core.Engine.FastDetectors;
using LightweightAI.Core.Refinery;
// PipelineRunner
// Ewma, ZScore, IncrementalKnn wrappers


// FeatureEncoder, FeatureReducer


namespace LightweightAI.Core.Loaders.qANDa;


public static class PipelineTest
{
    public static void RunTests()
    {
        // Explicit construction of dependencies (keeps DI concerns out of this test helper)
        var normalizer = new DefaultNormalizer();
        var oneHot = new OneHotEncoder(new Dictionary<int, int>(), new Dictionary<string, int>());
        var encoder = new FeatureEncoder(oneHot);
        var reducer = new FeatureReducer();
        var ewma = new Ewma(new EwmaDetector());
        var z = new ZScore(new ZScoreDetector());
        var knn = new IncrementalKnn(new WindowedKnnDensity());

        var runner = new PipelineRunner(normalizer, encoder, reducer, ewma, z, knn);

        // NOTE: To actually execute, construct SourceExecutionPlan instances and call:
        // await runner.RunAsync(plans, ct);
    }
}