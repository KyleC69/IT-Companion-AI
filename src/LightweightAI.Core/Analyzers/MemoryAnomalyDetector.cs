// Project Name: LightweightAI.Core
// File Name: MemoryAnomalyDetector.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Config;
using LightweightAI.Core.Loaders.Windows;



namespace LightweightAI.Core.Analyzers;


public sealed class MemoryAnomalyDetector(MemoryAnomalyConfig config, IMemoryAnomalySink sink, ILoggerSeverity log)
{
    private readonly MemoryAnomalyConfig _config = config ?? throw new ArgumentNullException(nameof(config));
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IMemoryAnomalySink _sink = sink ?? throw new ArgumentNullException(nameof(sink));

    // Key: PID:BaseAddress
    private readonly Dictionary<string, AnomalyState> _state = new(StringComparer.OrdinalIgnoreCase);





    public async Task ProcessBatchAsync(IReadOnlyList<MemoryRegionRecord> regions, CancellationToken ct)
    {
        DateTime now = DateTime.UtcNow;
        List<MemoryAnomaly> anomalies = new();

        foreach (MemoryRegionRecord rec in regions)
        {
            var key = rec.RecordId;
            List<string> triggeredRules = EvaluateRules(rec);

            if (triggeredRules.Count > 0)
            {
                if (!this._state.TryGetValue(key, out AnomalyState? st))
                    st = this._state[key] = new AnomalyState { FirstSeen = now };

                st.LastSeen = now;
                st.TriggerCount++;

                if (st.TriggerCount >= this._config.DwellThreshold)
                    anomalies.Add(new MemoryAnomaly
                    {
                        Pid = rec.Pid,
                        ProcessName = rec.ProcessName,
                        BaseAddress = rec.BaseAddress,
                        RegionSize = rec.RegionSize,
                        Protect = rec.Protect,
                        Type = rec.Type,
                        MappedPath = rec.MappedPath,
                        RulesTriggered = triggeredRules,
                        FirstSeen = st.FirstSeen,
                        LastSeen = st.LastSeen
                    });
            }
            else
            {
                // No rules triggered — decay state
                if (this._state.TryGetValue(key, out AnomalyState? st))
                {
                    st.ClearCount++;
                    if (st.ClearCount >= this._config.ClearThreshold)
                        this._state.Remove(key);
                }
            }
        }

        if (anomalies.Count > 0)
            await this._sink.EmitAnomaliesAsync(anomalies, ct).ConfigureAwait(false);
    }





    private List<string> EvaluateRules(MemoryRegionRecord rec)
    {
        List<string> rules = new();

        if (this._config.FlagRWX && rec.Protect.Contains("PAGE_EXECUTE_READWRITE", StringComparison.OrdinalIgnoreCase))
            rules.Add("RWX region");

        if (this._config.FlagPrivateExec &&
            rec.Type.Equals("MEM_PRIVATE", StringComparison.OrdinalIgnoreCase) &&
            rec.Protect.StartsWith("PAGE_EXECUTE", StringComparison.OrdinalIgnoreCase) &&
            string.IsNullOrEmpty(rec.MappedPath))
            rules.Add("Private executable memory");

        if (this._config.FlagImageProtMismatch &&
            rec.Type.Equals("MEM_IMAGE", StringComparison.OrdinalIgnoreCase) &&
            rec.Protect.Contains("WRITE", StringComparison.OrdinalIgnoreCase))
            rules.Add("Writable image mapping");

        if (this._config.FlagGuardPages &&
            rec.Protect.Contains("PAGE_GUARD", StringComparison.OrdinalIgnoreCase))
            rules.Add("Guard page");

        if (this._config.FlagLargeRegion &&
            rec.RegionSize >= this._config.LargeRegionBytes)
            rules.Add($"Large region >= {this._config.LargeRegionBytes} bytes");

        return rules;
    }





    private sealed class AnomalyState
    {
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public int TriggerCount { get; set; }
        public int ClearCount { get; set; }
    }
}


// -------------------------
// DTOs and config
// -------------------------