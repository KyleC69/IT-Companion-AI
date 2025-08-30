// Project Name: LightweightAI.Core
// File Name: DriverLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Interfaces;
using LightweightAI.Core.Loaders.Services;

using Microsoft.Win32;


namespace LightweightAI.Core.Loaders.Drivers;


public sealed class DriverLoader(DriverLoaderConfig config, IDriverSink sink, ILoggerSeverity log) : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "ServiceController API (Driver Mode)";
    private const string SourceId = "drivers";
    private const string Loader = nameof(DriverLoader);

    private readonly DriverLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, DriverRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IDriverSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
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
                Dictionary<string, DriverRecord> snapshot = CollectSnapshot();

                IReadOnlyList<DriverRecord> toEmit;
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
                foreach (KeyValuePair<string, DriverRecord> kvp in snapshot)
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




    /// <summary>
    /// Collects a snapshot of the current state of device drivers on the system.
    /// </summary>
    /// <remarks>
    /// This method retrieves detailed information about each device driver, including its service name, display name, 
    /// status, start type, type, binary path, signed status, and other metadata. It applies filtering based on the 
    /// configuration provided in <see cref="DriverLoaderConfig"/> to include or exclude specific drivers.
    /// </remarks>
    /// <returns>
    /// A dictionary where the keys are driver service names and the values are <see cref="DriverRecord"/> objects 
    /// containing detailed information about each driver.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown if an error occurs while querying driver information or accessing registry values.
    /// </exception>
    private Dictionary<string, DriverRecord> CollectSnapshot()
    {
        Dictionary<string, DriverRecord> result = new(StringComparer.OrdinalIgnoreCase);

        foreach (var svc in ServiceController.GetDevices())
            try
            {
                if (this._config.IncludeNames?.Count > 0 && !this._config.IncludeNames.Contains(svc.ServiceName))
                    continue;
                if (this._config.ExcludeNames?.Contains(svc.ServiceName) == true)
                    continue;

                var rec = new DriverRecord
                {
                    ServiceName = svc.ServiceName,
                    DisplayName = svc.DisplayName,
                    Status = svc.Status.ToString(),
                    StartType = GetRegistryValue(svc.ServiceName, "Start"),
                    Type = GetRegistryValue(svc.ServiceName, "Type"),
                    BinaryPath = GetRegistryValue(svc.ServiceName, "ImagePath"),
                    Signed = CheckSignedStatus(GetRegistryValue(svc.ServiceName, "ImagePath")),
                    Host = Environment.MachineName,
                    SourceId = SourceId,
                    LoaderName = Loader,
                    SchemaVersion = SchemaVersion,
                    CollectionMethod = CollectionMethod,
                    RecordId = $"{svc.ServiceName}:{Environment.MachineName}",
                    ChangeType = "Unchanged"
                };

                result[svc.ServiceName] = rec;

                if (this._config.AuditLog)
                    this._log.Debug(
                        $"{Loader} audit Driver='{rec.ServiceName}' Status='{rec.Status}' Signed={rec.Signed} Schema='{SchemaVersion}'");
            }
            catch (Exception ex)
            {
                this._log.Warn($"{Loader} failed to query driver '{svc.ServiceName}': {ex.Message}");
            }

        return result;
    }




    /// <summary>
    /// Compares two snapshots of driver records and identifies the differences between them.
    /// </summary>
    /// <param name="oldSnap">The previous snapshot of driver records.</param>
    /// <param name="newSnap">The current snapshot of driver records.</param>
    /// <returns>A list of <see cref="DriverRecord"/> objects representing the changes. 
    /// Each record is marked with a change type indicating whether it was added, modified, or removed.</returns>
    private static List<DriverRecord> DiffSnapshots(Dictionary<string, DriverRecord> oldSnap,
        Dictionary<string, DriverRecord> newSnap)
    {
        List<DriverRecord> changes = new();

        foreach ((var name, DriverRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(name, out DriverRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (HasChanged(old, cur)) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var name, DriverRecord old) in oldSnap)
            if (!newSnap.ContainsKey(name))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(DriverRecord oldRec, DriverRecord newRec)
    {
        return oldRec.Status != newRec.Status ||
               oldRec.StartType != newRec.StartType ||
               oldRec.Type != newRec.Type ||
               oldRec.BinaryPath != newRec.BinaryPath ||
               oldRec.Signed != newRec.Signed;
    }





    private static DriverRecord CloneWithChange(DriverRecord src, string changeType)
    {
        return new DriverRecord
        {
            ServiceName = src.ServiceName,
            DisplayName = src.DisplayName,
            Status = src.Status,
            StartType = src.StartType,
            Type = src.Type,
            BinaryPath = src.BinaryPath,
            Signed = src.Signed,
            Host = src.Host,
            SourceId = src.SourceId,
            LoaderName = src.LoaderName,
            SchemaVersion = src.SchemaVersion,
            CollectionMethod = src.CollectionMethod,
            RecordId = src.RecordId,
            ChangeType = changeType
        };
    }





    // -------------------------
    // Helpers
    // -------------------------





    private static string GetRegistryValue(string serviceName, string valueName)
    {
        try
        {
            using RegistryKey? key =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}");
            return key?.GetValue(valueName)?.ToString() ?? "";
        }
        catch
        {
            return "";
        }
    }





    private static bool CheckSignedStatus(string binaryPath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(binaryPath)) return false;
            var expandedPath = Environment.ExpandEnvironmentVariables(binaryPath.Trim('"'));
            if (!File.Exists(expandedPath)) return false;

            var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(expandedPath);
            return cert.Subject.Length > 0; // crude check; can be replaced with full chain validation
        }
        catch
        {
            return false;
        }
    }
}


// -------------------------
// DTOs and config
// -------------------------