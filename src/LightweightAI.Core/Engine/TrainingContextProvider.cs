// Project Name: LightweightAI.Core
// File Name: TrainingContextProvider.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Engine;


public class TrainingContextProvider
{
    public TrainingContext GetActiveContext()
    {
        return new TrainingContext(
            "2025-07-15",
            "sha256:abcd1234...",
            "qa-model-v2.1",
            new DateTime(2025, 7, 15),
            new[] { "context=5", "threshold=0.85" },
            new[] { "f1=0.92", "precision=0.94" }
        );
    }
}