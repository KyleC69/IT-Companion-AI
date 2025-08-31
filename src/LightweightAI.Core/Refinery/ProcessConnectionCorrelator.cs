// Project Name: LightweightAI.Core
// File Name: ProcessConnectionCorrelator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Loaders.NetworkConnector;
using LightweightAI.Core.Loaders.Windows;


namespace LightweightAI.Core.Refinery;


public sealed class ProcessConnectionCorrelator(
    ProcessConnectionCorrelatorConfig config,
    IProcessConnectionSink sink,
    ILoggerSeverity log)
    : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "Join(ProcessLoader, NetworkConnectionLoader)";
    private const string SourceId = "proc-net";
    private const string Loader = nameof(ProcessConnectionCorrelator);

    private readonly ProcessConnectionCorrelatorConfig _config =
        config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, ProcessConnectionRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IProcessConnectionSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
    private bool _disposed;





    public void Dispose()
    {
        if (this._disposed) return;
        this._disposed = true;
    }





    public async Task CorrelateAsync(
        IReadOnlyList<ProcessRecord> processes,
        IReadOnlyList<NetworkConnectionRecord> connections,
        CancellationToken ct)
    {
        Dictionary<string, ProcessConnectionRecord> snapshot = new(StringComparer.OrdinalIgnoreCase);

        foreach (NetworkConnectionRecord conn in connections)
        {
            if (conn.OwningPid <= 0) continue;

            ProcessRecord? proc = processes.FirstOrDefault(p => p.Pid == conn.OwningPid);
            if (proc == null) continue;

            var rec = new ProcessConnectionRecord
            {
                Pid = proc.Pid,
                ProcessName = proc.ProcessName,
                ExecutablePath = proc.ExecutablePath,
                Protocol = conn.Protocol,
                LocalAddress = conn.LocalAddress,
                LocalPort = conn.LocalPort,
                RemoteAddress = conn.RemoteAddress,
                RemotePort = conn.RemotePort,
                State = conn.State,
                Host = Environment.MachineName,
                SourceId = SourceId,
                LoaderName = Loader,
                SchemaVersion = SchemaVersion,
                CollectionMethod = CollectionMethod,
                RecordId =
                    $"{proc.Pid}:{conn.Protocol}:{conn.LocalAddress}:{conn.LocalPort}->{conn.RemoteAddress}:{conn.RemotePort}",
                ChangeType = "Unchanged"
            };

            snapshot[rec.RecordId] = rec;

            if (this._config.AuditLog)
                this._log.Debug(
                    $"{Loader} audit PID={rec.Pid} Proc='{rec.ProcessName}' {rec.Protocol} {rec.LocalAddress}:{rec.LocalPort} -> {rec.RemoteAddress}:{rec.RemotePort} Schema={SchemaVersion}");
        }

        IReadOnlyList<ProcessConnectionRecord> toEmit;
        if (this._config.DeltaOnly)
            toEmit = DiffSnapshots(this._lastSnapshot, snapshot);
        else
            toEmit = snapshot.Values.Select(r =>
            {
                r.ChangeType = "Unchanged";
                return r;
            }).ToList();

        if (toEmit.Count > 0)
            await this._sink.EmitBatchAsync(toEmit, ct).ConfigureAwait(false);

        this._lastSnapshot.Clear();
        foreach (KeyValuePair<string, ProcessConnectionRecord> kvp in snapshot)
            this._lastSnapshot[kvp.Key] = kvp.Value;
    }





    private static List<ProcessConnectionRecord> DiffSnapshots(Dictionary<string, ProcessConnectionRecord> oldSnap,
        Dictionary<string, ProcessConnectionRecord> newSnap)
    {
        List<ProcessConnectionRecord> changes = new();

        foreach ((var id, ProcessConnectionRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(id, out ProcessConnectionRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (HasChanged(old, cur)) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var id, ProcessConnectionRecord old) in oldSnap)
            if (!newSnap.ContainsKey(id))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(ProcessConnectionRecord oldRec, ProcessConnectionRecord newRec)
    {
        return oldRec.State != newRec.State || oldRec.ProcessName != newRec.ProcessName ||
               oldRec.ExecutablePath != newRec.ExecutablePath;
    }





    private static ProcessConnectionRecord CloneWithChange(ProcessConnectionRecord src, string changeType)
    {
        return new ProcessConnectionRecord
        {
            Pid = src.Pid,
            ProcessName = src.ProcessName,
            ExecutablePath = src.ExecutablePath,
            Protocol = src.Protocol,
            LocalAddress = src.LocalAddress,
            LocalPort = src.LocalPort,
            RemoteAddress = src.RemoteAddress,
            RemotePort = src.RemotePort,
            State = src.State,
            Host = src.Host,
            SourceId = src.SourceId,
            LoaderName = src.LoaderName,
            SchemaVersion = src.SchemaVersion,
            CollectionMethod = src.CollectionMethod,
            RecordId = src.RecordId,
            ChangeType = changeType
        };
    }
}


// -------------------------
// DTOs and config
// -------------------------