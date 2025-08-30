// Project Name: LightweightAI.Core
// File Name: EwmaDetector.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Abstractions;
using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Engine.FastDetectors;


public sealed class EwmaDetector(float alpha = 0.3f, float k = 3f) : IDetector
{
    private readonly float _alpha = Math.Clamp(alpha, 0.01f, 0.99f);
    private readonly float _k = Math.Max(0.1f, k);
    private float _mean;
    private float _var = 1f;
    public string Name => "EWMA";





    public float UpdateAndScore(in EncodedEvent example, DateTimeOffset nowUtc)
    {
        // Example signal: dense[0] severity, dense[1] hour -> combine
        var x = example.Dense.Span[0];

        var prevMean = this._mean;
        this._mean = this._alpha * x + (1f - this._alpha) * this._mean;

        var diff = x - prevMean;
        this._var = this._alpha * (diff * diff) + (1f - this._alpha) * this._var;
        var sigma = MathF.Sqrt(Math.Max(this._var, 1e-6f));

        var score = MathF.Min(1f, MathF.Abs(x - this._mean) / MathF.Max(1e-5f, this._k * sigma));
        return score;
    }
}