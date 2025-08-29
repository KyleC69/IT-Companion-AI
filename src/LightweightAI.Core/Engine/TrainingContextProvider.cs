// Project Name: LightweightAI.Core
// File Name: TrainingContextProvider.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.models;


namespace LightweightAI.Core.Engine;
public class TrainingContextProvider
{
    public TrainingContext GetActiveContext()
    {
        return new TrainingContext(
            DatasetRevision: "2025-07-15",
            PreprocessingHash: "sha256:abcd1234...",
            ModelVersion: "qa-model-v2.1",
            TrainingDateUtc: new DateTime(2025, 7, 15),
            Hyperparameters: new[] { "context=5", "threshold=0.85" },
            Metrics: new[] { "f1=0.92", "precision=0.94" }
        );
    }
}