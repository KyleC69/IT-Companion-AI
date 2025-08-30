// Project Name: LightweightAI.Core
// File Name: ProcessLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Diagnostics;
using System.Management;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using LightweightAI.Core.Interfaces;


namespace LightweightAI.Core.Loaders.Windows;


public sealed class ProcessLoader(ProcessLoaderConfig config, Services.IInventorySink sink, ILoggerSeverity log)
    : IDisposable
{
    private const string SchemaVersion = "1.1";
    private const string CollectionMethod = "System.Diagnostics.Process + WMI Win32_Process";
    private const string SourceId = "processes";
    private const string Loader = nameof(ProcessLoader);

    private readonly ProcessLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<int, ProcessRecord> _lastSnapshot = new();
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly Services.IInventorySink _sink = sink ?? throw new ArgumentNullException(nameof(sink));

    private bool _disposed;





    public void Dispose()
    {
        if (this._disposed) return;
        this._disposed = true;
    }





    public async Task StartAsync(CancellationToken ct)
    {
        this._log.Info(
            $"{Loader} starting. Interval={this._config.SampleInterval}, ChangeOnly={this._config.ChangeOnly}, IncludeCmd={this._config.IncludeCommandLine}, IncludeUser={this._config.IncludeUser}");

        while (!ct.IsCancellationRequested)
            try
            {
                Dictionary<int, ProcessRecord> snapshot = CollectSnapshot();

                IReadOnlyList<ProcessRecord> toEmit;
                if (this._config.ChangeOnly)
                {
                    toEmit = DiffSnapshots(this._lastSnapshot, snapshot);
                }
                else
                {
                    toEmit = snapshot.Values.ToList();
                    foreach (ProcessRecord r in toEmit) r.ChangeType = "Unchanged";
                }

                if (toEmit.Count > 0)
                    await this._sink.EmitBatchAsync(toEmit, ct).ConfigureAwait(false);

                this._lastSnapshot.Clear();
                foreach (KeyValuePair<int, ProcessRecord> kvp in snapshot)
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





    // -------------------------
    // Collection
    // -------------------------





    private Dictionary<int, ProcessRecord> CollectSnapshot()
    {
        Dictionary<int, ProcessRecord> result = new();

        Process[] processes;
        try
        {
            processes = Process.GetProcesses();
        }
        catch (Exception ex)
        {
            this._log.Error($"{Loader} failed to enumerate processes: {ex.Message}");
            return result;
        }

        // Preload WMI map if command line or user requested
        Dictionary<int, WmiProcInfo>? wmiInfo = null;
        if (this._config.IncludeCommandLine || this._config.IncludeUser || this._config.IncludeSessionId)
            wmiInfo = QueryWmiProcesses();

        foreach (Process proc in processes)
            try
            {
                var pid = proc.Id;

                string? exePath = null;
                DateTime? startTime = null;
                try
                {
                    exePath = proc.MainModule?.FileName;
                }
                catch
                {
                    /* access denied */
                }

                try
                {
                    startTime = proc.StartTime.ToUniversalTime();
                }
                catch
                {
                    /* access denied */
                }

                // Fallback to WMI ExecutablePath when MainModule is blocked
                if (string.IsNullOrWhiteSpace(exePath) && wmiInfo != null &&
                    wmiInfo.TryGetValue(pid, out WmiProcInfo? wmi))
                    exePath = wmi.ExecutablePath;

                // Parent, CommandLine, User, Session
                int? ppid = null;
                string? cmd = null;
                string? userSid = null;
                string? userName = null;
                int? sessionId = null;

                if (wmiInfo != null && wmiInfo.TryGetValue(pid, out WmiProcInfo? wi))
                {
                    ppid = wi.ParentPid;
                    if (this._config.IncludeCommandLine) cmd = wi.CommandLine;
                    if (this._config.IncludeUser)
                    {
                        userSid = wi.UserSid;
                        userName = wi.UserName;
                    }

                    if (this._config.IncludeSessionId) sessionId = wi.SessionId;
                }
                else
                {
                    // Minimal fallback parent via WMI single query
                    ppid = GetParentPid(pid);
                }

                string? hash = null;
                string? signer = null;
                bool? sigValid = null;

                if (!string.IsNullOrWhiteSpace(exePath) && File.Exists(exePath))
                {
                    try
                    {
                        hash = ComputeSha256(exePath);
                    }
                    catch
                    {
                        /* ignore */
                    }

                    try
                    {
                        (signer, sigValid) = TryGetSignatureInfo(exePath, this._config.VerifyOnline);
                    }
                    catch
                    {
                        /* ignore */
                    }
                }

                var record = new ProcessRecord
                {
                    ProcessId = pid,
                    ParentProcessId = ppid,
                    ProcessName = Safe(() => proc.ProcessName) ?? "",
                    MainWindowTitle = Safe(() => proc.MainWindowTitle),
                    ExecutablePath = exePath,
                    BinaryHash = hash,
                    DigitalSignature = signer,
                    SignatureValid = sigValid,
                    StartTimeUtc = startTime,
                    CommandLine = cmd,
                    UserSid = userSid,
                    UserName = userName,
                    SessionId = sessionId,

                    Host = Environment.MachineName,
                    SourceId = SourceId,
                    LoaderName = Loader,
                    SchemaVersion = SchemaVersion,
                    CollectionMethod = CollectionMethod,
                    RecordId = Guid.NewGuid().ToString(),
                    ChangeType = "Unchanged"
                };

                if (this._config.AuditLog)
                    this._log.Debug(
                        $"{Loader} audit PID={record.ProcessId} Name='{record.ProcessName}' PPID={record.ParentProcessId} Hash='{Trunc(record.BinaryHash, 16)}' Signer='{record.DigitalSignature}' User='{record.UserName ?? record.UserSid}'");

                result[pid] = record;
            }
            catch (Exception ex)
            {
                this._log.Warn($"{Loader} failed to process PID={proc.Id}: {ex.Message}");
            }
            finally
            {
                proc.Dispose();
            }

        return result;
    }





    private static Dictionary<int, WmiProcInfo> QueryWmiProcesses()
    {
        Dictionary<int, WmiProcInfo> map = new();
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT ProcessId, ParentProcessId, ExecutablePath, CommandLine, SessionId FROM Win32_Process");
            foreach (ManagementObject mo in searcher.Get())
                try
                {
                    var pid = Convert.ToInt32(mo["ProcessId"]);
                    var info = new WmiProcInfo
                    {
                        ParentPid = TryToInt(mo["ParentProcessId"]),
                        ExecutablePath = mo["ExecutablePath"] as string,
                        CommandLine = mo["CommandLine"] as string,
                        SessionId = TryToInt(mo["SessionId"])
                    };

                    // User SID via method call GetOwnerSid (faster than token handles in managed code)
                    try
                    {
                        var outParams = mo.InvokeMethod("GetOwnerSid", null, null);
                        if (outParams != null) info.UserSid = outParams["Sid"] as string;

                        // User name/domain via GetOwner (optional)
                        var outOwner = mo.InvokeMethod("GetOwner", null, null);
                        if (outOwner != null)
                        {
                            var user = outOwner["User"] as string;
                            var domain = outOwner["Domain"] as string;
                            info.UserName = string.IsNullOrWhiteSpace(domain) ? user : $"{domain}\\{user}";
                        }
                    }
                    catch
                    {
                        /* ignored */
                    }

                    map[pid] = info;
                }
                catch
                {
                    /* ignore malformed rows */
                }
        }
        catch
        {
            /* WMI may be disabled */
        }

        return map;
    }





    private static int? GetParentPid(int pid)
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher($"SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {pid}");
            foreach (var obj in searcher.Get())
                return Convert.ToInt32(((ManagementObject)obj)["ParentProcessId"]);
        }
        catch
        {
            /* ignore */
        }

        return null;
    }





    // -------------------------
    // Diffing
    // -------------------------





    private static List<ProcessRecord> DiffSnapshots(Dictionary<int, ProcessRecord> oldSnap,
        Dictionary<int, ProcessRecord> newSnap)
    {
        List<ProcessRecord> changes = new();

        foreach ((var pid, ProcessRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(pid, out ProcessRecord? old))
            {
                changes.Add(CloneWithChange(cur, "Added"));
            }
            else if (HasChanged(old, cur))
            {
                ProcessRecord mod = CloneWithChange(cur, "Modified");
                mod.PreviousBinaryHash = old.BinaryHash;
                mod.PreviousSignatureValid = old.SignatureValid;
                changes.Add(mod);
            }

        foreach ((var pid, ProcessRecord old) in oldSnap)
            if (!newSnap.ContainsKey(pid))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(ProcessRecord oldRec, ProcessRecord newRec)
    {
        return !StringEquals(oldRec.BinaryHash, newRec.BinaryHash)
               || oldRec.SignatureValid != newRec.SignatureValid
               || !StringEquals(oldRec.ExecutablePath, newRec.ExecutablePath)
               || oldRec.ParentProcessId != newRec.ParentProcessId;
    }





    private static bool StringEquals(string? a, string? b)
    {
        return string.Equals(a ?? string.Empty, b ?? string.Empty, StringComparison.OrdinalIgnoreCase);
    }





    private static ProcessRecord CloneWithChange(ProcessRecord src, string changeType)
    {
        return new ProcessRecord
        {
            ProcessId = src.ProcessId,
            ParentProcessId = src.ParentProcessId,
            ProcessName = src.ProcessName,
            MainWindowTitle = src.MainWindowTitle,
            ExecutablePath = src.ExecutablePath,
            BinaryHash = src.BinaryHash,
            DigitalSignature = src.DigitalSignature,
            SignatureValid = src.SignatureValid,
            StartTimeUtc = src.StartTimeUtc,
            CommandLine = src.CommandLine,
            UserSid = src.UserSid,
            UserName = src.UserName,
            SessionId = src.SessionId,

            Host = src.Host,
            SourceId = src.SourceId,
            LoaderName = src.LoaderName,
            SchemaVersion = src.SchemaVersion,
            CollectionMethod = src.CollectionMethod,
            RecordId = Guid.NewGuid().ToString(),
            ChangeType = changeType,

            PreviousBinaryHash = null,
            PreviousSignatureValid = null
        };
    }





    // -------------------------
    // Helpers
    // -------------------------





    private static string ComputeSha256(string filePath)
    {
        using var sha = SHA256.Create();
        using FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var hash = sha.ComputeHash(stream);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }





    [Obsolete("TryGetSignatureInfo is obsolete due to unreliable certificate validation. Use platform-specific APIs or a dedicated security library for signature verification instead.")]
    private static (string? signer, bool? valid) TryGetSignatureInfo(string filePath, bool onlineRevocation)
    {
        try
        {
            using var cert = new X509Certificate2(filePath);
            if (cert == null || cert.Handle == IntPtr.Zero)
                return (null, null);

            var signer = TryGetCommonName(cert) ?? cert.Subject;

            using var chain = new X509Chain
            {
                ChainPolicy =
                {
                    RevocationMode = onlineRevocation ? X509RevocationMode.Online : X509RevocationMode.Offline,
                    RevocationFlag = X509RevocationFlag.ExcludeRoot,
                    VerificationFlags = X509VerificationFlags.NoFlag,
                    VerificationTime = DateTime.UtcNow
                }
            };

            var valid = chain.Build(cert);
            return (signer, valid);
        }
        catch
        {
            return (null, null);
        }
    }





    private static string? TryGetCommonName(X509Certificate2 cert)
    {
        try
        {
            const string cnPrefix = "CN=";
            foreach (var part in cert.Subject.Split(',',
                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                if (part.StartsWith(cnPrefix, StringComparison.OrdinalIgnoreCase))
                    return part.Substring(cnPrefix.Length);
        }
        catch
        {
            /* ignore */
        }

        return null;
    }





    private static T? Safe<T>(Func<T> f)
    {
        try
        {
            return f();
        }
        catch
        {
            return default;
        }
    }





    private static int? TryToInt(object? o)
    {
        try
        {
            return o == null ? null : Convert.ToInt32(o);
        }
        catch
        {
            return null;
        }
    }





    private static string Trunc(string? s, int n)
    {
        return string.IsNullOrEmpty(s) ? "" : s.Length <= n ? s : s.Substring(0, n);
    }





    private sealed class WmiProcInfo
    {
        public int? ParentPid { get; set; }
        public string? ExecutablePath { get; set; }
        public string? CommandLine { get; set; }
        public string? UserSid { get; set; }
        public string? UserName { get; set; }
        public int? SessionId { get; set; }
    }
}



// -------------------------
// DTOs and config
// -------------------------