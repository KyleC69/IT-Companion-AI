// Project Name: LightweightAI.Core
// File Name: KsStreamingDrift.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Interfaces;


namespace LightweightAI.Core.Engine.SlowDetectors;


/// <summary>
///     Simple streaming Kolmogorov-Smirnov style drift estimator maintaining two fixed-size windows
///     (reference and recent). Periodically (every CompareInterval events) computes an approximate KS
///     statistic using binned empirical CDFs. Stub thresholding logic sets drift when statistic exceeds
///     <see cref="threshold" />.
/// </summary>
public sealed class KsStreamingDrift(
    int referenceSize = 2048,
    int currentSize = 512,
    int bins = 32,
    double threshold = 0.35,
    int compareInterval = 256)
    : IDriftDetector
{
    private readonly Queue<double> _current = new();

    private readonly Queue<double> _reference = new();
    private long _seen;
    public string Name => "ks_stream";





    public double Update(double value, DateTimeOffset timestampUtc, out bool isDrift)
    {
        this._seen++;
        this._current.Enqueue(value);
        if (this._current.Count > currentSize) this._current.Dequeue();

        if (this._reference.Count < referenceSize)
        {
            this._reference.Enqueue(value);
        }
        else if (this._seen % 17 == 0) // slow decay sampling
        {
            this._reference.Dequeue();
            this._reference.Enqueue(value);
        }

        var stat = 0.0;
        isDrift = false;

        if (this._reference.Count > bins && this._current.Count > bins && this._seen % compareInterval == 0)
        {
            // Build histograms
            var min = Math.Min(this._reference.Min(), this._current.Min());
            var max = Math.Max(this._reference.Max(), this._current.Max());
            if (max <= min) return 0;
            var refHist = new double[bins];
            var curHist = new double[bins];
            var width = (max - min) / bins;
            foreach (var v in this._reference)
            {
                var idx = (int)Math.Clamp((v - min) / width, 0, bins - 1);
                refHist[idx]++;
            }

            foreach (var v in this._current)
            {
                var idx = (int)Math.Clamp((v - min) / width, 0, bins - 1);
                curHist[idx]++;
            }

            // Convert to cumulative probability
            double refCum = 0, curCum = 0;
            for (var i = 0; i < bins; i++)
            {
                refCum += refHist[i] / this._reference.Count;
                curCum += curHist[i] / this._current.Count;
                var diff = Math.Abs(refCum - curCum);
                if (diff > stat) stat = diff;
            }

            if (stat >= threshold) isDrift = true;
        }

        return stat;
    }
}