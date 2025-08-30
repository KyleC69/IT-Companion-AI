// Project Name: LightweightAI.Core
// File Name: HandleLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;
using System.Diagnostics;

using LightweightAI.Core.Interfaces;


namespace LightweightAI.Core.Loaders.Windows;


public sealed class HandleLoader(HandleLoaderConfig config, IHandleSink sink, ILoggerSeverity log) : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "NtQuerySystemInformation(SystemHandleInformation)";
    private const string SourceId = "handles";
    private const string Loader = nameof(HandleLoader);

    private readonly HandleLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, HandleRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IHandleSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
    private bool _disposed;





    public void Dispose()
    {
        if (this._disposed) return;
        this._disposed = true;
    }





    public async Task StartAsync(CancellationToken ct)
    {
        this._log.Info(
            $"{Loader} starting. Interval={this._config.SampleInterval}, DeltaOnly={this._config.DeltaOnly}");

        while (!ct.IsCancellationRequested)
            try
            {
                Dictionary<string, HandleRecord> snapshot = CollectSnapshot();

                IReadOnlyList<HandleRecord> toEmit;
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
                foreach (KeyValuePair<string, HandleRecord> kvp in snapshot)
                    this._lastSnapshot[kvp.Key] = kvp.Value;

                await Task.Delay(this._config.SampleInterval, ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                this._log.Error($"{Loader} loop error: {ex.Message}");
                if (this._config.FailFast) throw;
                await Task.Delay(TimeSpan.FromSeconds(2), ct).ConfigureAwait(false);
            }

        this._log.Info($"{Loader} stopped.");
    }





    private Dictionary<string, HandleRecord> CollectSnapshot()
    {
        Dictionary<string, HandleRecord> result = new(StringComparer.OrdinalIgnoreCase);

        foreach (Process proc in Process.GetProcesses())
            try
            {
                IEnumerable<(string TypeName, string ObjectName, uint AccessMask)> handles =
                    NativeHandleEnumerator.GetHandles(proc.Id);
                foreach ((string TypeName, string ObjectName, uint AccessMask) h in handles)
                {
                    var rec = new HandleRecord
                    {
                        Pid = proc.Id,
                        ProcessName = proc.ProcessName,
                        HandleType = h.TypeName,
                        ObjectName = h.ObjectName,
                        AccessMask = h.AccessMask,
                        Host = Environment.MachineName,
                        SourceId = SourceId,
                        LoaderName = Loader,
                        SchemaVersion = SchemaVersion,
                        CollectionMethod = CollectionMethod,
                        RecordId = $"{proc.Id}:{h.TypeName}:{h.ObjectName}",
                        ChangeType = "Unchanged"
                    };

                    result[rec.RecordId] = rec;

                    if (this._config.AuditLog)
                        this._log.Debug(
                            $"{Loader} audit PID={rec.Pid} Proc='{rec.ProcessName}' Type='{rec.HandleType}' Obj='{rec.ObjectName}' Schema={SchemaVersion}");
                }
            }
            catch (Win32Exception)
            {
                // Access denied to process handles — skip
            }
            catch (InvalidOperationException)
            {
                // Process exited — skip
            }
            catch (Exception ex)
            {
                this._log.Warn($"{Loader} failed to enumerate handles for PID={proc.Id}: {ex.Message}");
            }

        return result;
    }





    private static List<HandleRecord> DiffSnapshots(Dictionary<string, HandleRecord> oldSnap,
        Dictionary<string, HandleRecord> newSnap)
    {
        List<HandleRecord> changes = new();

        foreach ((var id, HandleRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(id, out HandleRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (HasChanged(old, cur)) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var id, HandleRecord old) in oldSnap)
            if (!newSnap.ContainsKey(id))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(HandleRecord oldRec, HandleRecord newRec)
    {
        return oldRec.AccessMask != newRec.AccessMask;
    }





    private static HandleRecord CloneWithChange(HandleRecord src, string changeType)
    {
        return new HandleRecord
        {
            Pid = src.Pid,
            ProcessName = src.ProcessName,
            HandleType = src.HandleType,
            ObjectName = src.ObjectName,
            AccessMask = src.AccessMask,
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

// -------------------------
// Native handle enumeration helper
// -------------------------