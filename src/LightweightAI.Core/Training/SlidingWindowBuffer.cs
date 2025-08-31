// Project Name: LightweightAI.Core
// File Name: SlidingWindowBuffer.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Training;


public class SlidingWindowBuffer(TimeSpan retentionPeriod, int maxSamples)
{
    private readonly LinkedList<TrainingSample> _buffer = new();





    public void Add(TrainingSample sample)
    {
        this._buffer.AddLast(sample);
        Prune();
    }





    private void Prune()
    {
        DateTime cutoff = DateTime.UtcNow - retentionPeriod;
        while (this._buffer.Count > 0 &&
               (this._buffer.First!.Value.Timestamp < cutoff || this._buffer.Count > maxSamples))
            this._buffer.RemoveFirst();
    }





    public IReadOnlyList<TrainingSample> GetSnapshot()
    {
        return this._buffer.ToList();
    }
}