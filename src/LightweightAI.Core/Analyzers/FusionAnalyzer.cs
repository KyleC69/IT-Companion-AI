// Project Name: LightweightAI.Core
// File Name: FusionAnalyzer.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Builders;
using LightweightAI.Core.Config;
using LightweightAI.Core.Loaders.Windows;



namespace LightweightAI.Core.Analyzers;


/// <summary>
///     FusionAnalyzer detects potential threats by correlating injected threads,
///     RWX memory regions, and active network connections in processes.
///     It raises incidents when a process exhibits all three behaviors.
/// </summary>
public sealed class FusionAnalyzer(
    FusionConfig config,
    IFusionSink sink,
    IncidentTimelineBuilder timeline,
    ILoggerSeverity log)
{
    private readonly FusionConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, IncidentState> _incidents = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IFusionSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
    private readonly IncidentTimelineBuilder _timeline = timeline ?? throw new ArgumentNullException(nameof(timeline));





    public async Task ProcessAsync(
        IReadOnlyList<ProcessRecord> processes,
        IReadOnlyList<ThreadRecord> threads,
        IReadOnlyList<HandleRecord> handles,
        IReadOnlyList<NetworkRecord> conns,
        IReadOnlyList<MemoryRegionRecord> memRegions,
        IReadOnlyList<MemoryAnomaly> memAnoms,
        CancellationToken ct)
    {
        DateTime now = DateTime.UtcNow;
        List<FusionIncident> newIncidents = new();

        ILookup<int, MemoryAnomaly> rwxRegions = memAnoms
            .Where(a => a.RulesTriggered.Contains("RWX region", StringComparer.OrdinalIgnoreCase))
            .ToLookup(a => a.Pid);

        foreach (var pid in rwxRegions.Select(g => g.Key))
        {
            ProcessRecord? proc = processes.FirstOrDefault(p => p.Pid == pid);
            if (proc == null) continue;

            var hasInjectedThread =
                threads.Any(t => t.Pid == pid && t.StartTime != null && t.StartTime > proc.StartTime);
            var hasNetConn = conns.Any(c => c.Pid == pid);

            if (hasInjectedThread && hasNetConn)
            {
                var id = $"InjectionC2:{pid}";
                if (!this._incidents.TryGetValue(id, out IncidentState? st))
                    st = this._incidents[id] = new IncidentState { FirstSeen = now };

                st.LastSeen = now;
                st.TriggerCount++;

                if (st.TriggerCount == this._config.DwellThreshold)
                {
                    FusionIncident incident = BuildIncident(id, proc, pid, threads, conns, rwxRegions[pid]);
                    newIncidents.Add(incident);

                    await this._timeline.AppendIncidentLifecycleAsync(incident, "AlertOpened", ct);
                    await AppendContextToTimeline(incident, threads, handles, conns, memAnoms, ct);
                }
                else if (st.TriggerCount > this._config.DwellThreshold)
                {
                    FusionIncident incident = BuildIncident(id, proc, pid, threads, conns, rwxRegions[pid]);
                    await this._timeline.AppendIncidentLifecycleAsync(incident, "AlertUpdated", ct);
                    await AppendContextToTimeline(incident, threads, handles, conns, memAnoms, ct);
                }
            }
        }

        // Clear stale incidents
        List<string> toRemove = this._incidents
            .Where(kvp => (now - kvp.Value.LastSeen).TotalSeconds > this._config.ClearSeconds)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var id in toRemove)
        {
            var incident = new FusionIncident { IncidentId = id, Description = "Incident cleared" };
            await this._timeline.AppendIncidentLifecycleAsync(incident, "AlertClosed", ct);
            this._incidents.Remove(id);
        }

        if (newIncidents.Count > 0)
            await this._sink.EmitIncidentsAsync(newIncidents, ct).ConfigureAwait(false);
    }





    private FusionIncident BuildIncident(string id, ProcessRecord proc, int pid,
        IEnumerable<ThreadRecord> threads, IEnumerable<NetworkRecord> conns, IEnumerable<MemoryAnomaly> memAnoms)
    {
        return new FusionIncident
        {
            IncidentId = id,
            Severity = "High",
            Confidence = 0.95,
            FirstSeen = DateTime.UtcNow,
            LastSeen = DateTime.UtcNow,
            Description =
                $"Process {proc.ProcessName} ({pid}) shows injected thread + RWX memory + active network connection.",
            ContributingRecords = new Dictionary<string, object>
            {
                { "Process", new[] { proc } },
                { "Threads", threads.Where(t => t.Pid == pid).ToArray() },
                { "MemoryAnomalies", memAnoms.ToArray() },
                { "Connections", conns.Where(c => c.Pid == pid).ToArray() }
            }
        };
    }





    private async Task AppendContextToTimeline(FusionIncident incident,
        IEnumerable<ThreadRecord> threads,
        IEnumerable<HandleRecord> handles,
        IEnumerable<NetworkRecord> conns,
        IEnumerable<MemoryAnomaly> memAnoms,
        CancellationToken ct)
    {
        await this._timeline.AppendThreadEventsAsync(incident.IncidentId,
            threads.Where(t => incident.ContributingRecords["Threads"].Cast<ThreadRecord>().Contains(t)), ct);
        await this._timeline.AppendMemoryAnomalyEventsAsync(incident.IncidentId, memAnoms, ct);
        await this._timeline.AppendHandleEventsAsync(incident.IncidentId, handles, ct);
        await this._timeline.AppendConnectionEventsAsync(incident.IncidentId, conns, ct);
    }





    private sealed class IncidentState
    {
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public int TriggerCount { get; set; }
    }
}



public interface IFusionSink
{
//TODO: Implement these record types properly
Task EmitIncidentsAsync(List<FusionIncident> NewIncidents, CancellationToken Ct);
}


//TODO: Implement these record types properly