// Project Name: LightweightAI.Core
// File Name: SlidingWindowBuffer.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers

using System.Linq;

public class SlidingWindowBuffer
{
    private readonly LinkedList<TrainingSample> _buffer = new();
    private readonly int _maxSamples;
    private readonly TimeSpan _retentionPeriod;

    public SlidingWindowBuffer(TimeSpan retentionPeriod, int maxSamples)
    {
        this._retentionPeriod = retentionPeriod;
        this._maxSamples = maxSamples;
    }

    public void Add(TrainingSample sample)
    {
        this._buffer.AddLast(sample);
        Prune();
    }

    private void Prune()
    {
        DateTime cutoff = DateTime.UtcNow - this._retentionPeriod;
        while (this._buffer.Count > 0 &&
               (this._buffer.First!.Value.Timestamp < cutoff || this._buffer.Count > this._maxSamples))
            this._buffer.RemoveFirst();
    }

    public IReadOnlyList<TrainingSample> GetSnapshot()
    {
        return this._buffer.ToList();
    }
}