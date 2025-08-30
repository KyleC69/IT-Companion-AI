// Project Name: LightweightAI.Core
// File Name: AuditFriendlyTrainer.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Training;


public class AuditFriendlyTrainer(TimeSpan retentionPeriod, int maxSamples) : IIncrementalTrainer
{
    private readonly List<TrainingSample> _pendingBatch = new();
    private readonly SlidingWindowBuffer _window = new(retentionPeriod, maxSamples);





    public void Ingest(TrainingSample sample)
    {
        this._window.Add(sample);
        this._pendingBatch.Add(sample);
    }





    public void TrainNextBatch()
    {
        if (this._pendingBatch.Count == 0) return;

        Console.WriteLine($"Training on {this._pendingBatch.Count} samples...");
        foreach (TrainingSample sample in this._pendingBatch)
            Console.WriteLine($"→ {sample.EventId} | {sample.Label} | {sample.ConfidenceScore}");

        this._pendingBatch.Clear();
    }





    public IReadOnlyList<TrainingSample> GetWindowSnapshot()
    {
        return this._window.GetSnapshot();
    }
}