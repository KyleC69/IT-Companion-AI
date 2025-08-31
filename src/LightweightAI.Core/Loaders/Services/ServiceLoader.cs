// Project Name: LightweightAI.Core
// File Name: ServiceLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;

using LightweightAI.Core.Config;
using LightweightAI.Core.Engine.Compat;

using Microsoft.Win32;


namespace LightweightAI.Core.Loaders.Services;


public sealed class ServiceLoader(ServiceLoaderConfig config, IInventorySink sink, ILoggerSeverity log)
    : IDisposable
{
    private const string SchemaVersion = "1.1";
    private const string CollectionMethod = "ServiceController API + Registry";
    private const string SourceId = "services";
    private const string Loader = nameof(ServiceLoader);

    private readonly ServiceLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    // Last emitted snapshot for diffing
    private readonly Dictionary<string, ServiceRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IInventorySink _sink = sink ?? throw new ArgumentNullException(nameof(sink));

    private bool _disposed;





    public void Dispose()
    {
        if (this._disposed) return;
        this._disposed = true;
    }





    public async Task StartAsync(CancellationToken ct)
    {
        this._log.Info(
            $"{Loader} starting. Interval={this._config.SampleInterval}, ChangeOnly={this._config.ChangeOnly}, IncludeDrivers={this._config.IncludeDrivers}");

        while (!ct.IsCancellationRequested)
            try
            {
                Dictionary<string, ServiceRecord> snapshot = CollectSnapshot();

                IReadOnlyList<ServiceRecord> toEmit;
                if (this._config.ChangeOnly)
                {
                    toEmit = DiffSnapshots(this._lastSnapshot, snapshot);
                }
                else
                {
                    toEmit = snapshot.Values.ToList();
                    // For full snapshots, normalize ChangeType to Unchanged
                    foreach (ServiceRecord r in toEmit) r.ChangeType = "Unchanged";
                }

                if (toEmit.Count > 0)
                    await this._sink.EmitBatchAsync(toEmit, ct).ConfigureAwait(false);

                // Update baseline
                this._lastSnapshot.Clear();
                foreach (KeyValuePair<string, ServiceRecord> kvp in snapshot)
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





    private Dictionary<string, ServiceRecord> CollectSnapshot()
    {
        Dictionary<string, ServiceRecord> result = new(StringComparer.OrdinalIgnoreCase);

        ServiceController[] services = Array.Empty<ServiceController>();
        ServiceController[] drivers = Array.Empty<ServiceController>();

        try
        {
            services = ServiceController.GetServices();
            if (this._config.IncludeDrivers)
                drivers = (ServiceController[])ServiceController.GetDevices();
        }
        catch (Exception ex)
        {
            this._log.Error($"{Loader} enumeration failure: {ex.Message}");
        }

        foreach (var sc in services.Concat(drivers))
            try
            {
                var name = sc.ServiceName;

                var regKeyPath = $@"SYSTEM\CurrentControlSet\Services\{name}";
                using RegistryKey? key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKeyPath);
                var serviceKeyExists = key != null;

                // Start mode
                string? startMode = null;
                try
                {
                    startMode = key?.GetValue("Start") switch
                    {
                        0 => "Boot",
                        1 => "System",
                        2 => "Automatic",
                        3 => "Manual",
                        4 => "Disabled",
                        _ => null
                    };
                }
                catch
                {
                    /* ignore */
                }

                // ImagePath
                string? imagePathRaw = null;
                try
                {
                    imagePathRaw = key?.GetValue("ImagePath") as string;
                }
                catch
                {
                    /* ignore */
                }

                var exePath = NormalizeImagePath(imagePathRaw);

                string? hash = null;
                string? signer = null;
                bool? sigValid = null;
                DateTime? lastChange = null;

                if (!string.IsNullOrWhiteSpace(exePath) && File.Exists(exePath))
                {
                    try
                    {
                        hash = ComputeSha256(exePath);
                    }
                    catch (Exception ex)
                    {
                        this._log.Warn($"{Loader} hash error '{name}': {ex.Message}");
                    }

                    try
                    {
                        (signer, sigValid) = TryGetSignatureInfo(exePath, this._config.VerifyOnline);
                    }
                    catch (Exception ex)
                    {
                        this._log.Warn($"{Loader} signature error '{name}': {ex.Message}");
                    }

                    try
                    {
                        lastChange = File.GetLastWriteTimeUtc(exePath);
                    }
                    catch
                    {
                        /* ignore */
                    }
                }

                var record = new ServiceRecord
                {
                    ServiceName = name,
                    DisplayName = Safe(() => sc.DisplayName),
                    StartMode = startMode,
                    ServiceType = Safe(() => sc.ServiceType.ToString()) ?? "Unknown",
                    Status = Safe(() => sc.Status.ToString()) ?? "Unknown",
                    BinaryPath = exePath,
                    BinaryPathHash = hash,
                    DigitalSignature = signer,
                    SignatureValid = sigValid,
                    LastChangeUtc = lastChange,
                    IsZombie = !serviceKeyExists, // SCM entry without registry key
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
                        $"{Loader} audit Service='{record.ServiceName}' StartMode='{record.StartMode}' Type='{record.ServiceType}' Status='{record.Status}' Hash='{Trunc(record.BinaryPathHash, 16)}' Signer='{record.DigitalSignature}' Zombie={record.IsZombie}");

                result[name] = record;
            }
            catch (Exception ex)
            {
                this._log.Warn($"{Loader} process error for '{sc.ServiceName}': {ex.Message}");
            }

        return result;
    }





    // -------------------------
    // Diffing
    // -------------------------





    private static List<ServiceRecord> DiffSnapshots(
        Dictionary<string, ServiceRecord> oldSnap,
        Dictionary<string, ServiceRecord> newSnap)
    {
        List<ServiceRecord> changes = new();

        // Added or modified
        foreach ((var name, ServiceRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(name, out ServiceRecord? old))
            {
                ServiceRecord added = CloneWithChange(cur, "Added");
                changes.Add(added);
            }
            else
            {
                if (HasChanged(old, cur))
                {
                    ServiceRecord mod = CloneWithChange(cur, "Modified");
                    // attach previous values for audit trail
                    mod.PreviousStartMode = old.StartMode;
                    mod.PreviousStatus = old.Status;
                    mod.PreviousBinaryPathHash = old.BinaryPathHash;
                    mod.PreviousSignatureValid = old.SignatureValid;
                    changes.Add(mod);
                }
                else if (cur.IsZombie && !old.IsZombie)
                {
                    // Registry key vanished while SCM entry persists
                    changes.Add(CloneWithChange(cur, "Zombie"));
                }
            }

        // Removed
        foreach ((var name, ServiceRecord old) in oldSnap)
            if (!newSnap.ContainsKey(name))
            {
                ServiceRecord removed = CloneWithChange(old, "Removed");
                changes.Add(removed);
            }

        return changes;
    }





    private static bool HasChanged(ServiceRecord oldRec, ServiceRecord newRec)
    {
        return !StringEquals(oldRec.StartMode, newRec.StartMode)
               || !StringEquals(oldRec.Status, newRec.Status)
               || !StringEquals(oldRec.BinaryPathHash, newRec.BinaryPathHash)
               || oldRec.SignatureValid != newRec.SignatureValid
               || oldRec.IsZombie != newRec.IsZombie;
    }





    private static bool StringEquals(string? a, string? b)
    {
        return string.Equals(a ?? string.Empty, b ?? string.Empty, StringComparison.OrdinalIgnoreCase);
    }





    private static ServiceRecord CloneWithChange(ServiceRecord src, string changeType)
    {
        return new ServiceRecord
        {
            ServiceName = src.ServiceName,
            DisplayName = src.DisplayName,
            StartMode = src.StartMode,
            ServiceType = src.ServiceType,
            Status = src.Status,
            BinaryPath = src.BinaryPath,
            BinaryPathHash = src.BinaryPathHash,
            DigitalSignature = src.DigitalSignature,
            SignatureValid = src.SignatureValid,
            LastChangeUtc = src.LastChangeUtc,
            IsZombie = src.IsZombie,
            Host = src.Host,
            SourceId = src.SourceId,
            LoaderName = src.LoaderName,
            SchemaVersion = src.SchemaVersion,
            CollectionMethod = src.CollectionMethod,
            RecordId = Guid.NewGuid().ToString(), // new record id per change emission
            ChangeType = changeType,

            // Previous* fields remain null; set by caller if needed
            PreviousStartMode = null,
            PreviousStatus = null,
            PreviousBinaryPathHash = null,
            PreviousSignatureValid = null
        };
    }





    // -------------------------
    // Helpers
    // -------------------------





    private static string? NormalizeImagePath(string? imagePathRaw)
    {
        if (string.IsNullOrWhiteSpace(imagePathRaw))
            return null;

        // Expand REG_EXPAND_SZ environment variables
        var expanded = Environment.ExpandEnvironmentVariables(imagePathRaw.Trim());

        // Remove surrounding quotes, then parse the executable token
        expanded = expanded.Trim();

        // If path starts with quote, take the quoted token
        if (expanded.Length > 0 && expanded[0] == '\"')
        {
            var end = expanded.IndexOf('\"', 1);
            if (end > 1)
            {
                var quoted = expanded.Substring(1, end - 1);
                return CanonicalizePath(quoted);
            }
        }

        // Otherwise, take up to first space as the executable path
        var idx = expanded.IndexOf(' ');
        var exe = idx > 0 ? expanded.Substring(0, idx) : expanded;

        return CanonicalizePath(exe);
    }





    private static string? CanonicalizePath(string path)
    {
        try
        {
            // Strip surrounding quotes just in case
            path = path.Trim().Trim('\"');
            if (string.IsNullOrWhiteSpace(path)) return null;

            // Normalize to full path if possible
            // If the path is not rooted (e.g., system32\svchost.exe), try system root heuristics
            if (!Path.IsPathRooted(path))
            {
                var sysRoot = Environment.GetEnvironmentVariable("SystemRoot");
                if (!string.IsNullOrEmpty(sysRoot))
                {
                    var candidate = Path.Combine(sysRoot, path);
                    if (File.Exists(candidate)) return candidate;
                    var sys32 = Path.Combine(sysRoot, "System32", path);
                    if (File.Exists(sys32)) return sys32;
                }
            }

            // If rooted or resolved, return as-is if exists; otherwise still return the normalized path
            return path;
        }
        catch
        {
            return path;
        }
    }





    private static string ComputeSha256(string filePath)
    {
        using var sha = SHA256.Create();
        using FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var hash = sha.ComputeHash(stream);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }





    private static (string? signer, bool? valid) TryGetSignatureInfo(string filePath, bool onlineRevocation)
    {
        try
        {
            // Load Authenticode certificate if present
            using var cert = new X509Certificate2(filePath);
            if (cert == null || cert.Handle == IntPtr.Zero)
                return (null, null);

            // Extract signer (CN or Subject)
            var signer = TryGetCommonName(cert) ?? cert.Subject;

            // Build chain to validate
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
            // Parse CN from Subject
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





    private static string Trunc(string? s, int n)
    {
        if (string.IsNullOrEmpty(s)) return "";
        return s.Length <= n ? s : s.Substring(0, n);
    }
}




// -------------------------
// DTOs and config
// -------------------------