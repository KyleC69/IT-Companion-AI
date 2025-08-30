// Project Name: LightweightAI.Core
// File Name: WindowedKnnDensity.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Abstractions;
using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Analyzers;


public sealed class WindowedKnnDensity : IDetector
{
    private readonly int _capacity;
    private readonly int _k;
    private readonly float _radius;
    private readonly Queue<float> _values = new();





    public WindowedKnnDensity(int k = 10, float radius = 0.5f, int capacity = 2048)
    {
        this._k = Math.Max(1, k);
        this._radius = Math.Max(1e-3f, radius);
        this._capacity = Math.Max(this._k * 2, capacity);
    }





    public string Name => "KNN-Density";





    public float UpdateAndScore(in EncodedEvent example, DateTimeOffset nowUtc)
    {
        var x = example.Dense.Span[0];

        // Count neighbors in radius
        var neighbors = 0;
        foreach (var v in this._values)
            if (MathF.Abs(v - x) <= this._radius)
                neighbors++;

        var score = 1f - MathF.Min(1f, neighbors / (float)this._k);

        // Update buffer
        this._values.Enqueue(x);
        if (this._values.Count > this._capacity) this._values.Dequeue();

        return score;
    }
}