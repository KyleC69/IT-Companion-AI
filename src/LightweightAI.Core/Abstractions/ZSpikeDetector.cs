using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightAI.Core.Abstractions;

public sealed class ZScoreDetector : IDetector
{
    public string Name => "ZScore";
    private long _n = 0;
    private double _mean = 0;
    private double _m2 = 0;
    private readonly float _k;

    public ZScoreDetector(float k = 3f) => _k = Math.Max(0.1f, k);

    public float UpdateAndScore(in EncodedEvent example, DateTimeOffset nowUtc)
    {
        var x = example.Dense.Span[0];

        _n++;
        var delta = x - _mean;
        _mean += delta / _n;
        _m2 += delta * (x - _mean);

        var variance = _n > 1 ? _m2 / (_n - 1) : 1e-6;
        var sigma = Math.Sqrt(Math.Max(variance, 1e-6));
        var score = (float)Math.Min(1.0, Math.Abs((x - _mean) / (_k * sigma + 1e-6)));
        return score;
    }
}
