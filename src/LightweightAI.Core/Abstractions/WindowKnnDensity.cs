using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightAI.Core.Abstractions;

public sealed class WindowedKnnDensity : IDetector
{
    public string Name => "KNN-Density";
    private readonly int _k;
    private readonly float _radius;
    private readonly int _capacity;
    private readonly Queue<float> _values = new();

    public WindowedKnnDensity(int k = 10, float radius = 0.5f, int capacity = 2048)
    {
        _k = Math.Max(1, k);
        _radius = Math.Max(1e-3f, radius);
        _capacity = Math.Max(_k * 2, capacity);
    }

    public float UpdateAndScore(in EncodedEvent example, DateTimeOffset nowUtc)
    {
        var x = example.Dense.Span[0];

        // Count neighbors in radius
        int neighbors = 0;
        foreach (var v in _values)
            if (MathF.Abs(v - x) <= _radius) neighbors++;

        var score = 1f - MathF.Min(1f, neighbors / (float)_k);

        // Update buffer
        _values.Enqueue(x);
        if (_values.Count > _capacity) _values.Dequeue();

        return score;
    }
}
