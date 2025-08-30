// Project Name: LightweightAI.Core
// File Name: RegistryLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Interfaces;

using Microsoft.Win32;


namespace LightweightAI.Core.Loaders.Registry;


public sealed class RegistryLoader(RegistryLoaderConfig config, IRegistrySink sink, ILoggerSeverity log)
    : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "Registry API";
    private const string SourceId = "registry";
    private const string Loader = nameof(RegistryLoader);

    private readonly RegistryLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, RegistryRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IRegistrySink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
    private bool _disposed;





    public void Dispose()
    {
        if (this._disposed) return;
        this._disposed = true;
    }





    public async Task StartAsync(CancellationToken ct)
    {
        this._log.Info(
            $"{Loader} starting. Keys={this._config.Keys.Count}, Interval={this._config.SampleInterval}, DeltaOnly={this._config.DeltaOnly}");

        while (!ct.IsCancellationRequested)
            try
            {
                Dictionary<string, RegistryRecord> snapshot = CollectSnapshot();

                IReadOnlyList<RegistryRecord> toEmit;
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
                foreach (KeyValuePair<string, RegistryRecord> kvp in snapshot)
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





    private Dictionary<string, RegistryRecord> CollectSnapshot()
    {
        Dictionary<string, RegistryRecord> result = new(StringComparer.OrdinalIgnoreCase);

        foreach (RegistryKeySpec keySpec in this._config.Keys)
            try
            {
                using RegistryKey? baseKey = OpenHive(keySpec.Hive);
                if (baseKey == null)
                {
                    this._log.Warn($"{Loader} unknown hive '{keySpec.Hive}'");
                    continue;
                }

                using RegistryKey? subKey = baseKey.OpenSubKey(keySpec.Path);
                if (subKey == null)
                {
                    this._log.Warn($"{Loader} missing key '{keySpec.Hive}\\{keySpec.Path}'");
                    continue;
                }

                EnumerateKey(subKey, $"{keySpec.Hive}\\{keySpec.Path}", keySpec.Depth, result);
            }
            catch (Exception ex)
            {
                this._log.Warn($"{Loader} failed to read '{keySpec.Hive}\\{keySpec.Path}': {ex.Message}");
            }

        return result;
    }





    private void EnumerateKey(RegistryKey key, string fullPath, int depth,
        Dictionary<string, RegistryRecord> result)
    {
        try
        {
            foreach (var valueName in key.GetValueNames())
            {
                var rec = new RegistryRecord
                {
                    KeyPath = fullPath,
                    ValueName = valueName,
                    ValueData = key.GetValue(valueName)?.ToString() ?? "",
                    ValueKind = key.GetValueKind(valueName).ToString(),
                    Host = Environment.MachineName,
                    SourceId = SourceId,
                    LoaderName = Loader,
                    SchemaVersion = SchemaVersion,
                    CollectionMethod = CollectionMethod,
                    RecordId = $"{fullPath}:{valueName}:{Environment.MachineName}",
                    ChangeType = "Unchanged"
                };

                result[rec.RecordId] = rec;

                if (this._config.AuditLog)
                    this._log.Debug(
                        $"{Loader} audit Key='{rec.KeyPath}' Value='{rec.ValueName}' Kind='{rec.ValueKind}' Schema='{SchemaVersion}'");
            }

            if (depth > 0)
                foreach (var subName in key.GetSubKeyNames())
                {
                    using RegistryKey? subKey = key.OpenSubKey(subName);
                    if (subKey != null)
                        EnumerateKey(subKey, $"{fullPath}\\{subName}", depth - 1, result);
                }
        }
        catch (Exception ex)
        {
            this._log.Warn($"{Loader} failed to enumerate '{fullPath}': {ex.Message}");
        }
    }





    private static RegistryKey? OpenHive(string hiveName)
    {
        return hiveName.ToUpperInvariant() switch
        {
            "HKLM" or "HKEY_LOCAL_MACHINE" => Microsoft.Win32.Registry.LocalMachine,
            "HKCU" or "HKEY_CURRENT_USER" => Microsoft.Win32.Registry.CurrentUser,
            "HKU" or "HKEY_USERS" => Microsoft.Win32.Registry.Users,
            "HKCR" or "HKEY_CLASSES_ROOT" => Microsoft.Win32.Registry.ClassesRoot,
            _ => null
        };
    }





    private static List<RegistryRecord> DiffSnapshots(Dictionary<string, RegistryRecord> oldSnap,
        Dictionary<string, RegistryRecord> newSnap)
    {
        List<RegistryRecord> changes = new();

        foreach ((var id, RegistryRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(id, out RegistryRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (HasChanged(old, cur)) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var id, RegistryRecord old) in oldSnap)
            if (!newSnap.ContainsKey(id))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(RegistryRecord oldRec, RegistryRecord newRec)
    {
        return oldRec.ValueData != newRec.ValueData || oldRec.ValueKind != newRec.ValueKind;
    }





    private static RegistryRecord CloneWithChange(RegistryRecord src, string changeType)
    {
        return new RegistryRecord
        {
            KeyPath = src.KeyPath,
            ValueName = src.ValueName,
            ValueData = src.ValueData,
            ValueKind = src.ValueKind,
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