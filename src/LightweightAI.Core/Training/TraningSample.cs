// Project Name: LightweightAI.Core
// File Name: TraningSample.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Training;


public record TrainingSample(
    Guid EventId,
    DateTime Timestamp,
    string SourceId,
    float ConfidenceScore,
    string Label,
    object Payload // Can be vector, token list, etc.
);



public interface IIncrementalTrainer
{
    void Ingest(TrainingSample sample);
    void TrainNextBatch();
    IReadOnlyList<TrainingSample> GetWindowSnapshot();
}