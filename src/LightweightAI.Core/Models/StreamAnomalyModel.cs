// Project Name: LightweightAI.Core
// File Name: StreamAnomalyModel.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


// Consolidated from duplicate locations:
//   - Engine/StreamAnomalyModel.cs (empty stub)
//   - Engine/models/StreamAnomalyModel.cs (original implementation)
// All logic preserved. Empty duplicate removed.


using System.Runtime.CompilerServices;



namespace LightweightAI.Core.Models;


public sealed class StreamAnomalyModel : IStreamAnomalyModel
{
    private readonly AnomalyConfig _cfg;
    private double _ema;
    private double _emaSq;
    private bool _init;






    public StreamAnomalyModel(AnomalyConfig cfg)
    {
        _cfg = cfg;
        _ema = 0d;
        _emaSq = 0d;
        _init = false;
    }






    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AnomalySignal Update(in TrendPoint p)
    {
        if (!_init)
        {
            _ema = p.Value;
            _emaSq = p.Value * p.Value;
            _init = true;
            var reading = new MetricReading(
                p.At.UtcDateTime, // Assuming `p.At` is DateTimeOffset
                "SignalName", // Replace with the appropriate signal name
                p.Value,
                new ProvenanceKey(Guid.NewGuid(), "SourceId", 0) // Replace with actual ProvenanceKey values
            );
            return new AnomalySignal(
                reading,
                false, // IsAnomaly
                0, // Score
                0, // PValue
                "DetectorName", // Replace with actual detector name
                "Initialization", // Notes
                0 // ZScore
            );
        }

        _ema = _cfg.Alpha * p.Value + (1 - _cfg.Alpha) * _ema;
        _emaSq = _cfg.Alpha * p.Value * p.Value + (1 - _cfg.Alpha) * _emaSq;
        var variance = Math.Max(_cfg.MinVariance, _emaSq - _ema * _ema);
        var std = Math.Sqrt(variance);
        var z = std > 0 ? (p.Value - _ema) / std : 0d;
        var isAnom = Math.Abs(z) >= _cfg.ZThreshold;
        var updatedReading = new MetricReading(
            p.At.UtcDateTime,
            "SignalName", // Replace with the appropriate signal name
            p.Value,
            new ProvenanceKey(Guid.NewGuid(), "SourceId", 0) // Replace with actual ProvenanceKey values
        );
        return new AnomalySignal(
            updatedReading,
            isAnom,
            z, // Score
            0, // PValue (replace with actual calculation if needed)
            "DetectorName", // Replace with actual detector name
            isAnom ? "Anomaly detected" : "Normal", // Notes
            z // ZScore
        );
    }
}