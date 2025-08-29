// Project Name: LightweightAI.Core
// File Name: TrainingSample.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


public record TrainingSample(
    Guid EventId,
    DateTime Timestamp,
    string SourceId,
    float ConfidenceScore,
    string Label,
    object Payload // Replace with a concrete type if needed
);