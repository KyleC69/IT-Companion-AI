// Project Name: LightweightAI.Core
// File Name: MemoryRegionLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using LightweightAI.Core.Interfaces;


namespace LightweightAI.Core.Loaders.Windows;


public sealed class MemoryRegionLoader(MemoryRegionLoaderConfig config, IMemoryRegionSink sink, ILoggerSeverity log)
    : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "VirtualQueryEx + GetMappedFileNameW";
    private const string SourceId = "memregions";
    private const string Loader = nameof(MemoryRegionLoader);

    private readonly MemoryRegionLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, MemoryRegionRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IMemoryRegionSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
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
                Dictionary<string, MemoryRegionRecord> snapshot = CollectSnapshot();

                IReadOnlyList<MemoryRegionRecord> toEmit;
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
                foreach (KeyValuePair<string, MemoryRegionRecord> kvp in snapshot)
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





    private Dictionary<string, MemoryRegionRecord> CollectSnapshot()
    {
        Dictionary<string, MemoryRegionRecord> result = new(StringComparer.OrdinalIgnoreCase);

        foreach (Process proc in Process.GetProcesses())
        {
            if (ShouldSkipProcess(proc.ProcessName)) continue;

            var hProcess = IntPtr.Zero;
            try
            {
                hProcess = OpenForQuery(proc.Id);
                if (hProcess == IntPtr.Zero) continue;

                var emittedForProc = 0;
                EnumerateRegions(proc, hProcess, rec =>
                {
                    var key = rec.RecordId;
                    result[key] = rec;

                    if (this._config.AuditLog)
                        this._log.Debug(
                            $"{Loader} audit PID={rec.Pid} Proc='{rec.ProcessName}' Base=0x{rec.BaseAddress} Size={rec.RegionSize} Prot='{rec.Protect}' Type='{rec.Type}' Path='{rec.MappedPath}'");

                    emittedForProc++;
                    return this._config.MaxRegionsPerProcess <= 0 || emittedForProc < this._config.MaxRegionsPerProcess;
                });
            }
            catch (Win32Exception)
            {
                // Access denied or process exited; skip.
            }
            catch (InvalidOperationException)
            {
                // Process exited mid-enumeration; skip.
            }
            catch (Exception ex)
            {
                this._log.Warn($"{Loader} failed for PID={proc.Id}: {ex.Message}");
            }
            finally
            {
                if (hProcess != IntPtr.Zero) Kernel32.CloseHandle(hProcess);
            }
        }

        return result;
    }





    private bool ShouldSkipProcess(string name)
    {
        if (this._config.IncludeOnlyProcesses?.Count > 0)
            return !this._config.IncludeOnlyProcesses.Contains(name, StringComparer.OrdinalIgnoreCase);
        if (this._config.ExcludeProcesses?.Count > 0)
            return this._config.ExcludeProcesses.Contains(name, StringComparer.OrdinalIgnoreCase);
        return false;
    }





    private static IntPtr OpenForQuery(int pid)
    {
        const uint QUERY = 0x0400; // PROCESS_QUERY_INFORMATION
        const uint QUERY_LIMITED = 0x1000; // PROCESS_QUERY_LIMITED_INFORMATION
        const uint VM_READ = 0x0010; // PROCESS_VM_READ

        // Try full query + VM_READ first
        var h = Kernel32.OpenProcess(QUERY | VM_READ, false, (uint)pid);
        if (h != IntPtr.Zero) return h;

        // Fall back to limited query (may still allow VirtualQueryEx for some regions)
        h = Kernel32.OpenProcess(QUERY_LIMITED | VM_READ, false, (uint)pid);
        return h;
    }





    private void EnumerateRegions(Process proc, IntPtr hProcess, Func<MemoryRegionRecord, bool> onRecord)
    {
        var address = UIntPtr.Zero;
        var mbiSize = IntPtr.Size == 8
            ? Marshal.SizeOf<MEMORY_BASIC_INFORMATION64>()
            : Marshal.SizeOf<MEMORY_BASIC_INFORMATION32>();

        while (true)
        {
            int bytesReturned;
            if (IntPtr.Size == 8)
            {
                var mbi = new MEMORY_BASIC_INFORMATION64();
                bytesReturned = Kernel32.VirtualQueryEx(hProcess, address, out mbi, (UIntPtr)mbiSize);
                if (bytesReturned == 0) break;

                // Advance address before any continues to avoid infinite loops
                var regionSize = mbi.RegionSize;
                var baseAddr = mbi.BaseAddress;
                var next = baseAddr + Math.Max(regionSize, 0x1000UL);
                address = (UIntPtr)next;

                if (mbi.State == MEM_STATE.MEM_FREE) continue;
                if (!this._config.IncludeReservedRegions && mbi.State == MEM_STATE.MEM_RESERVE) continue;

                MemoryRegionRecord rec = BuildRecord(proc, mbi);
                if (!onRecord(rec)) break;
            }
            else
            {
                var mbi = new MEMORY_BASIC_INFORMATION32();
                bytesReturned = Kernel32.VirtualQueryEx(hProcess, address, out mbi, (UIntPtr)mbiSize);
                if (bytesReturned == 0) break;

                // Advance
                var regionSize = mbi.RegionSize;
                var baseAddr = mbi.BaseAddress;
                var next = baseAddr + Math.Max(regionSize, 0x1000U);
                address = next;

                if (mbi.State == MEM_STATE.MEM_FREE) continue;
                if (!this._config.IncludeReservedRegions && mbi.State == MEM_STATE.MEM_RESERVE) continue;

                MemoryRegionRecord rec = BuildRecord(proc, mbi);
                if (!onRecord(rec)) break;
            }
        }
    }





    private MemoryRegionRecord BuildRecord(Process proc, MEMORY_BASIC_INFORMATION64 mbi)
    {
        var prot = ProtectionToString(mbi.Protect);
        var allocProt = ProtectionToString(mbi.AllocationProtect);
        var type = TypeToString(mbi.Type);

        var mappedPath = TryGetMappedPath(proc, (UIntPtr)mbi.BaseAddress);

        var rec = new MemoryRegionRecord
        {
            Pid = proc.Id,
            ProcessName = proc.ProcessName,
            BaseAddress = $"0x{mbi.BaseAddress:X}",
            AllocationBase = $"0x{mbi.AllocationBase:X}",
            RegionSize = (long)mbi.RegionSize,
            State = StateToString(mbi.State),
            Protect = prot,
            AllocationProtect = allocProt,
            Type = type,
            MappedPath = mappedPath,
            Host = Environment.MachineName,
            SourceId = SourceId,
            LoaderName = Loader,
            SchemaVersion = SchemaVersion,
            CollectionMethod = CollectionMethod,
            RecordId = $"{proc.Id}:0x{mbi.BaseAddress:X}",
            ChangeType = "Unchanged"
        };
        return rec;
    }





    private MemoryRegionRecord BuildRecord(Process proc, MEMORY_BASIC_INFORMATION32 mbi)
    {
        var prot = ProtectionToString(mbi.Protect);
        var allocProt = ProtectionToString(mbi.AllocationProtect);
        var type = TypeToString(mbi.Type);

        var mappedPath = TryGetMappedPath(proc, mbi.BaseAddress);

        var rec = new MemoryRegionRecord
        {
            Pid = proc.Id,
            ProcessName = proc.ProcessName,
            BaseAddress = $"0x{mbi.BaseAddress:X}",
            AllocationBase = $"0x{mbi.AllocationBase:X}",
            RegionSize = mbi.RegionSize,
            State = StateToString(mbi.State),
            Protect = prot,
            AllocationProtect = allocProt,
            Type = type,
            MappedPath = mappedPath,
            Host = Environment.MachineName,
            SourceId = SourceId,
            LoaderName = Loader,
            SchemaVersion = SchemaVersion,
            CollectionMethod = CollectionMethod,
            RecordId = $"{proc.Id}:0x{mbi.BaseAddress:X}",
            ChangeType = "Unchanged"
        };
        return rec;
    }





    private string TryGetMappedPath(Process proc, UIntPtr baseAddr)
    {
        try
        {
            var hProcess = proc.Handle; // May throw; alternative is using our opened handle.
        }
        catch
        {
            // Use our previously opened handle (more reliable)
        }

        // We need the explicit handle we opened for VirtualQueryEx, so prefer OpenForQuery again.
        var path = string.Empty;
        var hProcess = OpenForQuery(proc.Id);
        if (hProcess == IntPtr.Zero) return path;

        try
        {
            var sb = new StringBuilder(1024);
            var len = PsApi.GetMappedFileNameW(hProcess, baseAddr, sb, (uint)sb.Capacity);
            if (len > 0) path = NormalizeDevicePath(sb.ToString());
        }
        catch
        {
            /* ignore */
        }
        finally
        {
            Kernel32.CloseHandle(hProcess);
        }

        return path;
    }





    private static string NormalizeDevicePath(string devicePath)
    {
        // Best-effort path normalization: translate \Device\HarddiskVolumeX to drive letter if possible.
        // Lightweight map for common drives.
        try
        {
            for (var drive = 'A'; drive <= 'Z'; drive++)
            {
                var driveStr = drive + ":";
                var sb = new StringBuilder(256);
                var res = Kernel32.QueryDosDevice(driveStr, sb, (uint)sb.Capacity);
                if (res == 0) continue;

                var target = sb.ToString().TrimEnd('\0');
                if (!string.IsNullOrEmpty(target) && devicePath.StartsWith(target, StringComparison.OrdinalIgnoreCase))
                    return devicePath.Replace(target, driveStr);
            }
        }
        catch
        {
            /* ignore */
        }

        return devicePath;
    }





    private static List<MemoryRegionRecord> DiffSnapshots(
        Dictionary<string, MemoryRegionRecord> oldSnap,
        Dictionary<string, MemoryRegionRecord> newSnap)
    {
        List<MemoryRegionRecord> changes = new();

        foreach ((var id, MemoryRegionRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(id, out MemoryRegionRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (HasChanged(old, cur)) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var id, MemoryRegionRecord old) in oldSnap)
            if (!newSnap.ContainsKey(id))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(MemoryRegionRecord a, MemoryRegionRecord b)
    {
        return !StringEquals(a.Protect, b.Protect) ||
               !StringEquals(a.AllocationProtect, b.AllocationProtect) ||
               !StringEquals(a.Type, b.Type) ||
               !StringEquals(a.State, b.State) ||
               !StringEquals(a.MappedPath, b.MappedPath) ||
               a.RegionSize != b.RegionSize;
    }





    private static bool StringEquals(string? x, string? y)
    {
        return string.Equals(x ?? string.Empty, y ?? string.Empty, StringComparison.OrdinalIgnoreCase);
    }





    private static MemoryRegionRecord CloneWithChange(MemoryRegionRecord src, string changeType)
    {
        return new MemoryRegionRecord
        {
            Pid = src.Pid,
            ProcessName = src.ProcessName,
            BaseAddress = src.BaseAddress,
            AllocationBase = src.AllocationBase,
            RegionSize = src.RegionSize,
            State = src.State,
            Protect = src.Protect,
            AllocationProtect = src.AllocationProtect,
            Type = src.Type,
            MappedPath = src.MappedPath,
            Host = src.Host,
            SourceId = src.SourceId,
            LoaderName = src.LoaderName,
            SchemaVersion = src.SchemaVersion,
            CollectionMethod = src.CollectionMethod,
            RecordId = src.RecordId,
            ChangeType = changeType
        };
    }





    private static string StateToString(uint state)
    {
        return state switch
        {
            0x1000 => "MEM_COMMIT",
            0x2000 => "MEM_RESERVE",
            0x10000 => "MEM_FREE",
            _ => $"0x{state:X}"
        };
    }





    private static string TypeToString(uint type)
    {
        return type switch
        {
            0x1000000 => "MEM_IMAGE",
            0x40000 => "MEM_MAPPED",
            0x20000 => "MEM_PRIVATE",
            _ => $"0x{type:X}"
        };
    }





    private static string ProtectionToString(uint protect)
    {
        // Normalize common flag combos
        List<string> sb = new();
        if ((protect & 0x10) != 0) sb.Add("PAGE_EXECUTE");
        if ((protect & 0x20) != 0) sb.Add("PAGE_EXECUTE_READ");
        if ((protect & 0x40) != 0) sb.Add("PAGE_EXECUTE_READWRITE");
        if ((protect & 0x80) != 0) sb.Add("PAGE_EXECUTE_WRITECOPY");
        if ((protect & 0x02) != 0) sb.Add("PAGE_READONLY");
        if ((protect & 0x04) != 0) sb.Add("PAGE_READWRITE");
        if ((protect & 0x08) != 0) sb.Add("PAGE_WRITECOPY");
        if ((protect & 0x01) != 0) sb.Add("PAGE_NOACCESS");
        if ((protect & 0x100) != 0) sb.Add("PAGE_GUARD");
        if ((protect & 0x200) != 0) sb.Add("PAGE_NOCACHE");
        if ((protect & 0x400) != 0) sb.Add("PAGE_WRITECOMBINE");
        return sb.Count > 0 ? string.Join("|", sb) : $"0x{protect:X}";
    }
}



// -------------------------
// DTOs and config
// -------------------------



public interface AILogger
{
    void Debug(string message);
    void Info(string message);
    void Warn(string message);
    void Error(string message);
}



// -------------------------
// Native interop
// -------------------------