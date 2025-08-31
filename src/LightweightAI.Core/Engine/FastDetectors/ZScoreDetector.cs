// Project Name: LightweightAI.Core
// File Name: ZScoreDetector.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine.FastDetectors;


public sealed class ZScoreDetector(float k = 3f) : IDetector
{
    private readonly float _k = Math.Max(0.1f, k);
    private double _m2;
    private double _mean;
    private long _n;
    public string Name => "ZScore";





    public float UpdateAndScore(in EncodedEvent example, DateTimeOffset nowUtc)
    {
        var x = example.Dense.Span[0];

        this._n++;
        var delta = x - this._mean;
        this._mean += delta / this._n;
        this._m2 += delta * (x - this._mean);

        var variance = this._n > 1 ? this._m2 / (this._n - 1) : 1e-6;
        var sigma = Math.Sqrt(Math.Max(variance, 1e-6));
        var score = (float)Math.Min(1.0, Math.Abs((x - this._mean) / (this._k * sigma + 1e-6)));
        return score;
    }
}