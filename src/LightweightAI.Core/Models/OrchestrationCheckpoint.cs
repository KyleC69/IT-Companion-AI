// Project Name: LightweightAI.Core
// File Name: OrchestrationCheckpoint.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public record OrchestrationCheckpoint(
    string Name,
    string ConfigHash,
    TrainingContext TrainingContext,
    DateTime CreatedUtc
);