// Project Name: LightweightAI.Core
// File Name: ModuleLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

using LightweightAI.Core.Config;



namespace LightweightAI.Core.Loaders.Modules;


public sealed class ModuleLoader(ModuleLoaderConfig config, IModuleSink sink, ILoggerSeverity log) : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "Process.Modules API";
    private const string SourceId = "modules";
    private const string Loader = nameof(ModuleLoader);

    private readonly ModuleLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, ModuleRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IModuleSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
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
                Dictionary<string, ModuleRecord> snapshot = CollectSnapshot();

                IReadOnlyList<ModuleRecord> toEmit;
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
                foreach (KeyValuePair<string, ModuleRecord> kvp in snapshot)
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





    private Dictionary<string, ModuleRecord> CollectSnapshot()
    {
        Dictionary<string, ModuleRecord> result = new(StringComparer.OrdinalIgnoreCase);

        foreach (Process proc in Process.GetProcesses())
            try
            {
                foreach (ProcessModule mod in proc.Modules)
                {
                    var rec = new ModuleRecord
                    {
                        Pid = proc.Id,
                        ProcessName = proc.ProcessName,
                        ModuleName = mod.ModuleName,
                        FilePath = mod.FileName ?? "",
                        BaseAddress = mod.BaseAddress.ToInt64(),
                        ModuleSize = mod.ModuleMemorySize,
                        Signed = CheckSignedStatus(mod.FileName),
                        Host = Environment.MachineName,
                        SourceId = SourceId,
                        LoaderName = Loader,
                        SchemaVersion = SchemaVersion,
                        CollectionMethod = CollectionMethod,
                        RecordId = $"{proc.Id}:{mod.FileName}",
                        ChangeType = "Unchanged"
                    };

                    result[rec.RecordId] = rec;

                    if (this._config.AuditLog)
                        this._log.Debug(
                            $"{Loader} audit PID={rec.Pid} Proc='{rec.ProcessName}' Module='{rec.ModuleName}' Signed={rec.Signed} Schema={SchemaVersion}");
                }
            }
            catch (Win32Exception)
            {
                // Access denied to process modules — skip
            }
            catch (InvalidOperationException)
            {
                // Process exited — skip
            }
            catch (Exception ex)
            {
                this._log.Warn($"{Loader} failed to enumerate modules for PID={proc.Id}: {ex.Message}");
            }

        return result;
    }





    private static bool CheckSignedStatus(string? filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;

            var cert = new X509Certificate2(filePath);
            return cert.Subject.Length > 0;
        }
        catch
        {
            return false;
        }
    }





    private static List<ModuleRecord> DiffSnapshots(Dictionary<string, ModuleRecord> oldSnap,
        Dictionary<string, ModuleRecord> newSnap)
    {
        List<ModuleRecord> changes = new();

        foreach ((var id, ModuleRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(id, out ModuleRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (HasChanged(old, cur)) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var id, ModuleRecord old) in oldSnap)
            if (!newSnap.ContainsKey(id))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(ModuleRecord oldRec, ModuleRecord newRec)
    {
        return oldRec.ModuleSize != newRec.ModuleSize || oldRec.Signed != newRec.Signed;
    }





    private static ModuleRecord CloneWithChange(ModuleRecord src, string changeType)
    {
        return new ModuleRecord
        {
            Pid = src.Pid,
            ProcessName = src.ProcessName,
            ModuleName = src.ModuleName,
            FilePath = src.FilePath,
            BaseAddress = src.BaseAddress,
            ModuleSize = src.ModuleSize,
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
}


// -------------------------
// DTOs and config
// -------------------------