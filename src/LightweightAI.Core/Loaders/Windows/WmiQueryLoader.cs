// Project Name: LightweightAI.Core
// File Name: WmiQueryLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Management;



namespace LightweightAI.Core.Loaders.Windows;


public sealed class WmiQueryLoader : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "System.Management WMI Query";
    private const string Loader = nameof(WmiQueryLoader);

    private readonly WmiQueryLoaderConfig _config;

    private readonly Dictionary<string, WmiRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILogger _log;
    private readonly IWmiQuerySink _sink;
    private bool _disposed;





    public WmiQueryLoader(WmiQueryLoaderConfig config, IWmiQuerySink sink, ILogger log)
    {
        this._config = config ?? throw new ArgumentNullException(nameof(config));
        this._sink = sink ?? throw new ArgumentNullException(nameof(sink));
        this._log = log ?? throw new ArgumentNullException(nameof(log));
    }





    public void Dispose()
    {
        if (this._disposed) return;
        this._disposed = true;
    }





    public async Task StartAsync(CancellationToken ct)
    {
        this._log.Info(
            $"{Loader} starting. Query='{this._config.Query}' Interval={this._config.SampleInterval}, DeltaOnly={this._config.DeltaOnly}");

        while (!ct.IsCancellationRequested)
            try
            {
                Dictionary<string, WmiRecord> snapshot = CollectSnapshot();

                IReadOnlyList<WmiRecord> toEmit;
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
                foreach (KeyValuePair<string, WmiRecord> kvp in snapshot) this._lastSnapshot[kvp.Key] = kvp.Value;

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





    private Dictionary<string, WmiRecord> CollectSnapshot()
    {
        Dictionary<string, WmiRecord> result = new(StringComparer.OrdinalIgnoreCase);

        try
        {
            using var searcher = new ManagementObjectSearcher(this._config.Scope, this._config.Query);
            foreach (ManagementObject obj in searcher.Get())
            {
                var rec = new WmiRecord
                {
                    ClassName = obj.ClassPath.ClassName,
                    Properties = ExtractProperties(obj),
                    Host = Environment.MachineName,
                    SourceId = this._config.SourceId,
                    LoaderName = Loader,
                    SchemaVersion = SchemaVersion,
                    CollectionMethod = CollectionMethod,
                    RecordId = $"{obj.ClassPath.ClassName}:{obj["Name"] ?? obj["Id"] ?? Guid.NewGuid()}",
                    ChangeType = "Unchanged"
                };

                result[rec.RecordId] = rec;

                if (this._config.AuditLog)
                    this._log.Debug(
                        $"{Loader} audit Class='{rec.ClassName}' RecordId='{rec.RecordId}' Schema='{SchemaVersion}'");
            }
        }
        catch (Exception ex)
        {
            this._log.Warn($"{Loader} failed to execute query '{this._config.Query}': {ex.Message}");
        }

        return result;
    }





    private static Dictionary<string, string> ExtractProperties(ManagementObject obj)
    {
        Dictionary<string, string> dict = new(StringComparer.OrdinalIgnoreCase);
        foreach (var prop in obj.Properties) dict[prop.Name] = prop.Value?.ToString() ?? "";
        return dict;
    }





    private static List<WmiRecord> DiffSnapshots(Dictionary<string, WmiRecord> oldSnap,
        Dictionary<string, WmiRecord> newSnap)
    {
        List<WmiRecord> changes = new();

        foreach ((var id, WmiRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(id, out WmiRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (HasChanged(old, cur)) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var id, WmiRecord old) in oldSnap)
            if (!newSnap.ContainsKey(id))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(WmiRecord oldRec, WmiRecord newRec)
    {
        if (oldRec.Properties.Count != newRec.Properties.Count)
            return true;

        foreach (KeyValuePair<string, string> kvp in newRec.Properties)
            if (!oldRec.Properties.TryGetValue(kvp.Key, out var oldVal) || oldVal != kvp.Value)
                return true;
        return false;
    }





    private static WmiRecord CloneWithChange(WmiRecord src, string changeType)
    {
        return new WmiRecord
        {
            ClassName = src.ClassName,
            Properties = new Dictionary<string, string>(src.Properties, StringComparer.OrdinalIgnoreCase),
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



public sealed class WmiRecord
{
    public string ClassName { get; set; } = "";
    public Dictionary<string, string> Properties { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";
    public string ChangeType { get; set; } = "";
}



public sealed class WmiQueryLoaderConfig
{
    public string Scope { get; init; } = @"\\.\root\cimv2";
    public string Query { get; init; } = "";
    public string SourceId { get; init; } = "wmi";
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromMinutes(5);
    public bool DeltaOnly { get; init; } = true;
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
}



public interface IWmiQuerySink
{
    Task EmitBatchAsync(IReadOnlyList<WmiRecord> batch, CancellationToken ct);
}



public interface ILogger
{
    void Debug(string message);
    void Info(string message);
    void Warn(string message);
    void Error(string message);
}