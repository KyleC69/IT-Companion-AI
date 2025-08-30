// Project Name: LightweightAI.Core
// File Name: ThreadLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;
using System.Diagnostics;

using LightweightAI.Core.Interfaces;

using ThreadState = System.Threading.ThreadState;


namespace LightweightAI.Core.Loaders.Windows;


public sealed class ThreadLoader(ThreadLoaderConfig config, IThreadSink sink, ILoggerSeverity log) : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD)";
    private const string SourceId = "threads";
    private const string Loader = nameof(ThreadLoader);

    private readonly ThreadLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, ThreadRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IThreadSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
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
                Dictionary<string, ThreadRecord> snapshot = CollectSnapshot();

                IReadOnlyList<ThreadRecord> toEmit;
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
                foreach (KeyValuePair<string, ThreadRecord> kvp in snapshot)
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





    private Dictionary<string, ThreadRecord> CollectSnapshot()
    {
        Dictionary<string, ThreadRecord> result = new(StringComparer.OrdinalIgnoreCase);

        foreach (Process proc in Process.GetProcesses())
            try
            {
                foreach (ProcessThread t in proc.Threads)
                {
                    var rec = new ThreadRecord
                    {
                        Pid = proc.Id,
                        Tid = t.Id,
                        ProcessName = proc.ProcessName,
                        StartTime = SafeGetStartTime(t),
                        PriorityLevel = t.PriorityLevel.ToString(),
                        ThreadState = t.ThreadState.ToString(),
                        WaitReason = t.ThreadState == (System.Diagnostics.ThreadState)ThreadState.WaitSleepJoin ? t.WaitReason.ToString() : null,
                        Host = Environment.MachineName,
                        SourceId = SourceId,
                        LoaderName = Loader,
                        SchemaVersion = SchemaVersion,
                        CollectionMethod = CollectionMethod,
                        RecordId = $"{proc.Id}:{t.Id}",
                        ChangeType = "Unchanged"
                    };

                    result[rec.RecordId] = rec;

                    if (this._config.AuditLog)
                        this._log.Debug(
                            $"{Loader} audit PID={rec.Pid} TID={rec.Tid} State={rec.ThreadState} Wait={rec.WaitReason}");
                }
            }
            catch (Win32Exception)
            {
                // Access denied — skip
            }
            catch (InvalidOperationException)
            {
                // Process exited — skip
            }
            catch (Exception ex)
            {
                this._log.Warn($"{Loader} failed to enumerate threads for PID={proc.Id}: {ex.Message}");
            }

        return result;
    }





    private static DateTime? SafeGetStartTime(ProcessThread t)
    {
        try
        {
            return t.StartTime;
        }
        catch
        {
            return null;
        }
    }





    private static List<ThreadRecord> DiffSnapshots(Dictionary<string, ThreadRecord> oldSnap,
        Dictionary<string, ThreadRecord> newSnap)
    {
        List<ThreadRecord> changes = new();

        foreach ((var id, ThreadRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(id, out ThreadRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (HasChanged(old, cur)) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var id, ThreadRecord old) in oldSnap)
            if (!newSnap.ContainsKey(id))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(ThreadRecord oldRec, ThreadRecord newRec)
    {
        return oldRec.ThreadState != newRec.ThreadState ||
               oldRec.PriorityLevel != newRec.PriorityLevel ||
               oldRec.WaitReason != newRec.WaitReason;
    }





    private static ThreadRecord CloneWithChange(ThreadRecord src, string changeType)
    {
        return new ThreadRecord
        {
            Pid = src.Pid,
            Tid = src.Tid,
            ProcessName = src.ProcessName,
            StartTime = src.StartTime,
            PriorityLevel = src.PriorityLevel,
            ThreadState = src.ThreadState,
            WaitReason = src.WaitReason,
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