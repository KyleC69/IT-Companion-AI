// Project Name: LightweightAI.Core
// File Name: IncidentTimelineBuilder.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Security.Cryptography;
using System.Text;

using LightweightAI.Core.Builders;
using LightweightAI.Core.Interfaces;
using LightweightAI.Core.Loaders.Windows;
using LightweightAI.Core.Models;

using ThreadRecord = LightweightAI.Core.Loaders.Windows.ThreadRecord;


namespace LightweightAI.Core.Analyzers;


public sealed class IncidentTimelineBuilder(ITimelineStore store, ILoggerSeverity log, SourceOrdering ordering)
{
    private readonly ILoggerSeverity _log = log;

    private readonly List<TimelineEvent> _events = new();




    // Fusion incident lifecycle
    public async Task AppendIncidentLifecycleAsync(FusionIncident incident, string changeType, CancellationToken ct)
    {
        DateTime now = DateTime.UtcNow;

        Dictionary<string, string> evidence = new()
        {
            ["Severity"] = incident.Severity,
            ["Confidence"] = incident.Confidence.ToString("0.###"),
            ["Description"] = incident.Description
        };

        TimelineEvent evt = await MakeEventAsync(
            incident.IncidentId,
            "fusion",
            "FusionAnalyzer",
            "1.0",
            incident.IncidentId,
            changeType,
            incident.LastSeen,
            now,
            $"Incident {changeType}: {incident.Description}",
            evidence,
            MinimalJson(incident),
            ct);

        await store.AppendAsync(evt, ct).ConfigureAwait(false);
    }





    // Adapters for various records (add more as needed)
    public async Task AppendThreadEventsAsync(string incidentId, IEnumerable<ThreadRecord> threads,
        CancellationToken ct)
    {
        foreach (ThreadRecord t in threads)
        {
            TimelineEvent ev = await MakeEventAsync(
                incidentId,
                "threads",
                "ThreadLoader",
                t.SchemaVersion,
                $"{t.Pid}:{t.Tid}",
                MapChange(t.ChangeType),
                t.StartTime ?? DateTime.UtcNow,
                DateTime.UtcNow,
                $"Thread {t.Tid} in {t.ProcessName} is {t.ThreadState} ({t.WaitReason})",
                new Dictionary<string, string>
                {
                    ["Pid"] = t.Pid.ToString(),
                    ["Tid"] = t.Tid.ToString(),
                    ["State"] = t.ThreadState,
                    ["Priority"] = t.PriorityLevel,
                    ["WaitReason"] = t.WaitReason ?? ""
                },
                MinimalJson(t),
                ct);
            await store.AppendAsync(ev, ct).ConfigureAwait(false);
        }
    }





    public async Task AppendMemoryAnomalyEventsAsync(string incidentId, IEnumerable<MemoryAnomaly> anoms,
        CancellationToken ct)
    {
        foreach (MemoryAnomaly a in anoms)
        {
            TimelineEvent ev = await MakeEventAsync(
                incidentId,
                "memanomaly",
                "MemoryAnomalyDetector",
                "1.0",
                $"{a.Pid}:{a.BaseAddress}",
                "AlertUpdated",
                a.LastSeen,
                DateTime.UtcNow,
                $"Memory anomaly in {a.ProcessName} at {a.BaseAddress}: {string.Join((string?)", ", a.RulesTriggered)}",
                new Dictionary<string, string>
                {
                    ["Pid"] = a.Pid.ToString(),
                    ["BaseAddress"] = a.BaseAddress,
                    ["RegionSize"] = a.RegionSize.ToString(),
                    ["Protect"] = a.Protect,
                    ["Type"] = a.Type,
                    ["MappedPath"] = a.MappedPath,
                    ["Rules"] = string.Join((string?)"|", a.RulesTriggered)
                },
                MinimalJson(a),
                ct);
            await store.AppendAsync(ev, ct).ConfigureAwait(false);
        }
    }





    public async Task<IncidentSnapshot> GetSnapshotAsync(string incidentId, CancellationToken ct)
    {
        var events = await store.GetEventsAsync(incidentId, ct).ConfigureAwait(false);
        if (events.Count == 0)
            return new IncidentSnapshot
            {
                IncidentId = incidentId, FirstSeenUtc = DateTime.MinValue, LastSeenUtc = DateTime.MinValue,
                OrderedEvents = events
            };

        return new IncidentSnapshot
        {
            IncidentId = incidentId,
            FirstSeenUtc = events.Min(e => e.ObservedAtUtc),
            LastSeenUtc = events.Max(e => e.ObservedAtUtc),
            OrderedEvents = events
        };
    }





    // --------------- helpers ---------------





    private async Task<TimelineEvent> MakeEventAsync(
        string incidentId,
        string sourceId,
        string loaderName,
        string schemaVersion,
        string recordId,
        string changeType,
        DateTime observedAtUtc,
        DateTime reportedAtUtc,
        string summary,
        Dictionary<string, string> evidence,
        string provenanceJson,
        CancellationToken ct)
    {
        var seq = await store.NextSequenceAsync(ct).ConfigureAwait(false);
        var srcOrder = ordering.OrderOf(sourceId);
        var contentHash =
            Sha256Hex(string.Join("|", evidence.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}={kv.Value}")));
        var eventId = Sha256Hex($"{incidentId}|{sourceId}|{recordId}|{changeType}|{observedAtUtc:O}|{contentHash}");

        return new TimelineEvent
        {
            IncidentId = incidentId,
            EventId = eventId,
            SourceId = sourceId,
            LoaderName = loaderName,
            SchemaVersion = schemaVersion,
            RecordId = recordId,
            ChangeType = changeType,
            ObservedAtUtc = observedAtUtc,
            ReportedAtUtc = reportedAtUtc,
            MonotonicSeq = seq,
            SourceOrder = srcOrder,
            Summary = summary,
            Evidence = evidence,
            ContentHash = contentHash,
            ProvenanceJson = provenanceJson
        };
    }





    private static string MapChange(string changeType)
    {
        return string.IsNullOrWhiteSpace(changeType) ? "Unchanged" : changeType;
    }





    private static string MinimalJson(object obj)
    {
        // Minimal, order-stable JSON without external deps (not a full serializer).
        // Use only for provenance crumbs; keep it compact.
        IEnumerable<string> pairs = obj.GetType().GetProperties()
            .Where(p => p.CanRead)
            .Select(p => $"\"{p.Name}\":\"{(p.GetValue(obj)?.ToString() ?? "").Replace("\"", "\\\"")}\"");
        return "{" + string.Join(",", pairs) + "}";
    }





    private static string Sha256Hex(string s)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(s));
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes) sb.Append(b.ToString("x2"));
        return sb.ToString();
    }





    public async Task AppendHandleEventsAsync(string incidentIncidentId, IEnumerable<HandleRecord> handles,
        CancellationToken ct)
    {
        throw new NotImplementedException();
        //TODO: Implement if needed
    }





    public async Task AppendConnectionEventsAsync(string incidentIncidentId, IEnumerable<NetworkRecord> conns,
        CancellationToken ct)
    {
        throw new NotImplementedException();
        //TODO: Implement if needed
    }





    public IReadOnlyList<TimelineEvent> Build(IEventLoader<MemoryAnomalyEvent> loader)
    {
        foreach (var anomaly in loader.LoadEvents())
        {
            _events.Add(new TimelineEvent
            {
                TimestampUtc = anomaly.TimestampUtc,
                Category = "MemoryAnomaly",
                Details = $"{anomaly.AnomalyType} (Severity: {anomaly.Severity})",
                Provenance = anomaly.Provenance
            });
        }

        return _events;
    }
}