// Project Name: LightweightAI.Core
// File Name: HealthLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Diagnostics;
using System.Net.NetworkInformation;

using LightweightAI.Core.Config;



namespace LightweightAI.Core.Loaders.Windows;


public sealed class HealthLoader(HealthLoaderConfig config, IHealthSink sink, ILoggerSeverity log) : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "System Health Poll";
    private const string SourceId = "health";
    private const string Loader = nameof(HealthLoader);

    private readonly HealthLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, HealthRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IHealthSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
    private bool _disposed;





    public void Dispose()
    {
        if (this._disposed) return;
        this._disposed = true;
    }





    public async Task StartAsync(CancellationToken ct)
    {
        this._log.Info(
            $"{Loader} starting. Metrics={string.Join(",", this._config.Metrics)}, Interval={this._config.SampleInterval}, DeltaOnly={this._config.DeltaOnly}");

        while (!ct.IsCancellationRequested)
            try
            {
                Dictionary<string, HealthRecord> snapshot = CollectSnapshot();

                IReadOnlyList<HealthRecord> toEmit;
                if (this._config.DeltaOnly)
                    toEmit = DiffSnapshots(this._lastSnapshot, snapshot, this._config.ChangeThreshold);
                else
                    toEmit = snapshot.Values.Select(r =>
                    {
                        r.ChangeType = "Unchanged";
                        return r;
                    }).ToList();

                if (toEmit.Count > 0)
                    await this._sink.EmitBatchAsync(toEmit, ct).ConfigureAwait(false);

                this._lastSnapshot.Clear();
                foreach (KeyValuePair<string, HealthRecord> kvp in snapshot)
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





    private Dictionary<string, HealthRecord> CollectSnapshot()
    {
        Dictionary<string, HealthRecord> result = new(StringComparer.OrdinalIgnoreCase);
        DateTimeOffset now = DateTimeOffset.UtcNow;

        foreach (var metric in this._config.Metrics)
            try
            {
                var value = metric switch
                {
                    "cpu" => GetCpuUsage(),
                    "memory" => GetMemoryUsage(),
                    "disk" => GetDiskUsage(),
                    "net" => GetNetworkUsage(),
                    _ => double.NaN
                };

                var rec = new HealthRecord
                {
                    Metric = metric,
                    Value = value,
                    Timestamp = now,
                    Host = Environment.MachineName,
                    SourceId = SourceId,
                    LoaderName = Loader,
                    SchemaVersion = SchemaVersion,
                    CollectionMethod = CollectionMethod,
                    RecordId = Guid.NewGuid().ToString(),
                    ChangeType = "Unchanged"
                };

                result[metric] = rec;

                if (this._config.AuditLog)
                    this._log.Debug($"{Loader} audit Metric='{metric}' Value={value:F2} Schema='{SchemaVersion}'");
            }
            catch (Exception ex)
            {
                this._log.Warn($"{Loader} metric '{metric}' failed: {ex.Message}");
            }

        return result;
    }





    private static List<HealthRecord> DiffSnapshots(Dictionary<string, HealthRecord> oldSnap,
        Dictionary<string, HealthRecord> newSnap, double threshold)
    {
        List<HealthRecord> changes = new();

        foreach ((var metric, HealthRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(metric, out HealthRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (Math.Abs(cur.Value - old.Value) >= threshold) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var metric, HealthRecord old) in oldSnap)
            if (!newSnap.ContainsKey(metric))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static HealthRecord CloneWithChange(HealthRecord src, string changeType)
    {
        return new HealthRecord
        {
            Metric = src.Metric,
            Value = src.Value,
            Timestamp = src.Timestamp,
            Host = src.Host,
            SourceId = src.SourceId,
            LoaderName = src.LoaderName,
            SchemaVersion = src.SchemaVersion,
            CollectionMethod = src.CollectionMethod,
            RecordId = Guid.NewGuid().ToString(),
            ChangeType = changeType
        };
    }





    // -------------------------
    // Metric collectors
    // -------------------------





    private static double GetCpuUsage()
    {
        using var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        _ = cpuCounter.NextValue();
        Thread.Sleep(250);
        return Math.Round(cpuCounter.NextValue(), 2);
    }





    private static double GetMemoryUsage()
    {
        GCMemoryInfo info = GC.GetGCMemoryInfo();
        var total = info.TotalAvailableMemoryBytes;
        var used = total - info.HighMemoryLoadThresholdBytes;
        return Math.Round((double)used / total * 100, 2);
    }





    private static double GetDiskUsage()
    {
        IEnumerable<DriveInfo> drives = DriveInfo.GetDrives().Where(d => d.IsReady);
        var total = drives.Sum(d => (double)d.TotalSize);
        var free = drives.Sum(d => (double)d.TotalFreeSpace);
        return Math.Round((total - free) / total * 100, 2);
    }





    private static double GetNetworkUsage()
    {
        IEnumerable<NetworkInterface> interfaces = NetworkInterface.GetAllNetworkInterfaces()
            .Where(ni => ni.OperationalStatus == OperationalStatus.Up);
        return interfaces.Sum(ni => (double)ni.Speed) / 1_000_000; // Mbps
    }
}



// -------------------------
// DTOs and config
// -------------------------