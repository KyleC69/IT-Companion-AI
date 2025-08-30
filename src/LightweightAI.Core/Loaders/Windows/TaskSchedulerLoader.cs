// Project Name: LightweightAI.Core
// File Name: TaskSchedulerLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Interfaces;


namespace LightweightAI.Core.Loaders.Windows;


public sealed class TaskSchedulerLoader(TaskSchedulerLoaderConfig config, ITaskSink sink, ILoggerSeverity log)
    : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "TaskScheduler API";
    private const string SourceId = "tasks";
    private const string Loader = nameof(TaskSchedulerLoader);

    private readonly TaskSchedulerLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, TaskRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly ITaskSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
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
                Dictionary<string, TaskRecord> snapshot = CollectSnapshot();

                IReadOnlyList<TaskRecord> toEmit;
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
                foreach (KeyValuePair<string, TaskRecord> kvp in snapshot)
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





    private Dictionary<string, TaskRecord> CollectSnapshot()
    {
        Dictionary<string, TaskRecord> result = new(StringComparer.OrdinalIgnoreCase);

        try
        {
            using var ts = new TaskService();
            foreach (var task in ts.AllTasks)
                try
                {
                    if (this._config.IncludePaths?.Count > 0 && !this._config.IncludePaths.Contains(task.Path))
                        continue;
                    if (this._config.ExcludePaths?.Contains(task.Path) == true)
                        continue;

                    var rec = new TaskRecord
                    {
                        Path = task.Path,
                        Name = task.Name,
                        Author = task.Definition.RegistrationInfo.Author ?? "",
                        State = task.State.ToString(),
                        LastRunTime = task.LastRunTime,
                        NextRunTime = task.NextRunTime,
                        Triggers = string.Join(";", task.Definition.Triggers.Select(t => t.ToString())),
                        Actions = string.Join(";", task.Definition.Actions.Select(a => a.ToString())),
                        Host = Environment.MachineName,
                        SourceId = SourceId,
                        LoaderName = Loader,
                        SchemaVersion = SchemaVersion,
                        CollectionMethod = CollectionMethod,
                        RecordId = $"{task.Path}:{Environment.MachineName}",
                        ChangeType = "Unchanged"
                    };

                    result[task.Path] = rec;

                    if (this._config.AuditLog)
                        this._log.Debug(
                            $"{Loader} audit Task='{rec.Path}' State='{rec.State}' Author='{rec.Author}' Schema='{SchemaVersion}'");
                }
                catch (Exception ex)
                {
                    this._log.Warn($"{Loader} failed to query task '{task.Path}': {ex.Message}");
                }
        }
        catch (Exception ex)
        {
            this._log.Error($"{Loader} failed to enumerate tasks: {ex.Message}");
        }

        return result;
    }





    private static List<TaskRecord> DiffSnapshots(Dictionary<string, TaskRecord> oldSnap,
        Dictionary<string, TaskRecord> newSnap)
    {
        List<TaskRecord> changes = new();

        foreach ((var path, TaskRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(path, out TaskRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (HasChanged(old, cur)) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var path, TaskRecord old) in oldSnap)
            if (!newSnap.ContainsKey(path))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(TaskRecord oldRec, TaskRecord newRec)
    {
        return oldRec.State != newRec.State ||
               oldRec.Author != newRec.Author ||
               oldRec.Triggers != newRec.Triggers ||
               oldRec.Actions != newRec.Actions ||
               oldRec.NextRunTime != newRec.NextRunTime;
    }





    private static TaskRecord CloneWithChange(TaskRecord src, string changeType)
    {
        return new TaskRecord
        {
            Path = src.Path,
            Name = src.Name,
            Author = src.Author,
            State = src.State,
            LastRunTime = src.LastRunTime,
            NextRunTime = src.NextRunTime,
            Triggers = src.Triggers,
            Actions = src.Actions,
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