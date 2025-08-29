// Project Name: LightweightAI.Core
// File Name: TrainingContext.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine.models;


public record TrainingContext(
    string DatasetRevision,
    string PreprocessingHash,
    string ModelVersion,
    DateTime TrainingDateUtc,
    string[] Hyperparameters,
    string[] Metrics
);