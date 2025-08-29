using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightAI.Core.Abstractions;

public sealed class EwmaDetector : IDetector
{
    public string Name => "EWMA";
    private readonly float _alpha;
    private readonly float _k;
    private float _mean;
    private float _var;

    public EwmaDetector(float alpha = 0.3f, float k = 3f)
    {
        _alpha = Math.Clamp(alpha, 0.01f, 0.99f);
        _k = Math.Max(0.1f, k);
        _mean = 0f;
        _var = 1f;
    }

    public float UpdateAndScore(in EncodedEvent example, DateTimeOffset nowUtc)
    {
        // Example signal: dense[0] severity, dense[1] hour -> combine
        var x = example.Dense.Span[0];

        var prevMean = _mean;
        _mean = _alpha * x + (1f - _alpha) * _mean;

        var diff = x - prevMean;
        _var = _alpha * (diff * diff) + (1f - _alpha) * _var;
        var sigma = MathF.Sqrt(Math.Max(_var, 1e-6f));

        var score = MathF.Min(1f, MathF.Abs(x - _mean) / (MathF.Max(1e-5f, _k * sigma)));
        return score;
    }
}
