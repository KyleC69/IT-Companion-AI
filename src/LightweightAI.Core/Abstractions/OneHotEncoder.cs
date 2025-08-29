using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightAI.Core.Abstractions;

public sealed class OneHotEncoder : IFeatureEncoder
{
    private readonly IReadOnlyDictionary<int, int> _eventIdIndex;
    private readonly IReadOnlyDictionary<string, int> _sourceIndex;
    private readonly int _denseDim;

    public OneHotEncoder(IReadOnlyDictionary<int, int> eventIdIndex, IReadOnlyDictionary<string, int> sourceIndex, int denseDim = 0)
    {
        _eventIdIndex = eventIdIndex;
        _sourceIndex = sourceIndex;
        _denseDim = denseDim;
    }

    public EncodedEvent Encode(RawEvent e)
    {
        var sparse = new Dictionary<string, int>(capacity: 8);

        if (_eventIdIndex.TryGetValue(e.EventId, out var ei)) sparse[$"evt:{e.EventId}"] = ei;
        if (_sourceIndex.TryGetValue(e.SourceKey, out var si)) sparse[$"src:{e.SourceKey}"] = si;

        if (e.Fields.TryGetValue("ProcessId", out var pidObj) && pidObj is int pid)
            sparse[$"pid:{pid % 1024}"] = (pid % 1024); // simple hash bucket

        // Severity buckets
        var sevOrdinal = e.Severity switch { "Critical" => 4, "Error" => 3, "Warn" => 2, "Info" => 1, _ => 0 };
        sparse[$"sev:{sevOrdinal}"] = sevOrdinal;

        // Dense features: severity ordinal, hour-of-day, composite hash of (SourceKey, EventId)
        var hod = (float)e.TimestampUtc.Hour;
        var hash = (float)(HashCode.Combine(e.SourceKey, e.EventId) & 0x7FFFFFFF) / int.MaxValue; // 0..1
        var denseLength = Math.Max(_denseDim, 3);
        var dense = new float[denseLength];
        dense[0] = sevOrdinal;
        dense[1] = hod;
        dense[2] = hash;

        return new EncodedEvent(e.SourceKey, e.TimestampUtc, e.Host, dense, sparse);
    }
}

