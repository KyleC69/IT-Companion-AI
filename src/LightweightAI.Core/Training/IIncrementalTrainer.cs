// Project Name: LightweightAI.Core
// File Name: IIncrementalTrainer.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Training;


public interface IIncrementalTrainer
{
    void Ingest(TrainingSample sample);
    void TrainNextBatch();
    IReadOnlyList<TrainingSample> GetWindowSnapshot();
}