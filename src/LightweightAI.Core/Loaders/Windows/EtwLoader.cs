// Project Name: LightweightAI.Core
// File Name: EtwLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

using LightweightAI.Core.Config;
using LightweightAI.Core.Engine.Compat;

using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;



// TraceEvent packages


namespace LightweightAI.Core.Loaders.Windows;


public sealed class EtwLoader : IDisposable
{
    private const string SchemaVersion = "1.1";
    private const string CollectionMethod = "ETW TraceEventSession";
    private const string SourceId = "etw";
    private const string Loader = nameof(EtwLoader);

    private static readonly Regex RxGuid =
        new(@"[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}", RegexOptions.Compiled);

    private static readonly Regex RxHex = new(@"\b0x[0-9a-fA-F]+\b", RegexOptions.Compiled);
    private static readonly Regex RxInt = new(@"\b\d{4,}\b", RegexOptions.Compiled);
    private static readonly Regex RxWs = new(@"\s+", RegexOptions.Compiled);

    private readonly EtwLoaderConfig _config;
    private readonly CancellationTokenSource _cts = new();
    private readonly DedupIndex _dedup;
    private readonly ILoggerSeverity _log;

    private readonly ConcurrentQueue<EtwEventEnvelope> _queue = new();
    private readonly IEtwSink _sink;
    private readonly List<IDisposable> _subscriptions = new();
    private bool _disposed;
    private bool _kernelEnabled;

    private Task? _pumpTask;

    private TraceEventSession? _session;






    public EtwLoader(EtwLoaderConfig config, IEtwSink sink, ILoggerSeverity log)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _sink = sink ?? throw new ArgumentNullException(nameof(sink));
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _dedup = new DedupIndex(_config.DedupWindow, _config.DedupMaxPerWindow,
            _config.DedupCapacity, log);
    }






    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        StopAsync().GetAwaiter().GetResult();
    }






    public async Task StartAsync(CancellationToken external)
    {
        _log.Info(
            $"{Loader} starting. Session='{_config.SessionName}' BatchSize={_config.BatchSize} FlushInterval={_config.FlushInterval} Providers={_config.Providers.Count} Kernel={_config.EnableKernel}");

        // Merge external and internal tokens
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(external, _cts.Token);
        CancellationToken ct = linkedCts.Token;

        InitializeSession();

        // Subscribe providers
        SubscribeProviders();
        if (_config.EnableKernel)
        {
            EnableKernelProviders();
        }

        // Start background pump
        _pumpTask = Task.Run(() => PumpLoopAsync(ct), ct);

        // The TraceEventSession runs on its own native thread; just await cancellation
        try
        {
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(200), ct).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            /* normal */
        }

        await StopAsync().ConfigureAwait(false);
        _log.Info($"{Loader} stopped.");
    }






    public async Task StopAsync()
    {
        if (_disposed)
        {
            return;
        }

        try
        {
            _cts.Cancel();

            try
            {
                if (_pumpTask != null)
                {
                    await _pumpTask.ConfigureAwait(false);
                }
            }
            catch
            {
                /* ignore */
            }

            foreach (IDisposable d in _subscriptions)
            {
                try
                {
                    d.Dispose();
                }
                catch
                {
                    /* ignore */
                }
            }

            _subscriptions.Clear();

            if (_kernelEnabled && _session != null)
            {
                try
                {
                    _session.DisableKernelProvider((KernelTraceEventParser.Keywords)_config.KernelKeywords);
                }
                catch
                {
                    /* ignore */
                }
            }

            if (_session != null)
            {
                try
                {
                    _session.Stop();
                }
                catch
                {
                    /* ignore */
                }

                try
                {
                    _session.Dispose();
                }
                catch
                {
                    /* ignore */
                }

                _session = null;
            }
        }
        finally
        {
            _disposed = true;
        }
    }






    // -------------------------
    // Initialization
    // -------------------------






    private void InitializeSession()
    {
        // Best effort to clean up an existing session with the same name
        if (TraceEventSession.GetActiveSession(_config.SessionName) != null)
        {
            if (_config.TakeoverExistingSession)
            {
                _log.Warn($"{Loader} taking over existing ETW session '{_config.SessionName}'.");
                try
                {
                    using var s = new TraceEventSession(_config.SessionName);
                    s.Stop();
                }
                catch
                {
                    /* ignore */
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"ETW session '{_config.SessionName}' already exists. Set TakeoverExistingSession=true to reuse.");
            }
        }

        _session = new TraceEventSession(_config.SessionName, null)
        {
            BufferSizeMB = _config.BufferSizeMB
        };

        if (_config.RealTimeSession)
        {
            _session.Source?.RegisterUnhandledEvent(delegate { });
        }

        // Ensure disposal on CTRL+C scenarios
        _session.StopOnDispose = true;

        _log.Info(
            $"{Loader} session created. RealTime={_config.RealTimeSession} BufferMB={_config.BufferSizeMB} Circular={_config.Circular}");
    }






    private void SubscribeProviders()
    {
        if (_session == null)
        {
            throw new InvalidOperationException("Session not initialized.");
        }

        if (_session.Source == null)
        {
            throw new InvalidOperationException("Session source not available.");
        }

        foreach (EtwProviderSpec p in _config.Providers)
        {
            try
            {
                var level = (TracingLevel)Clamp(p.Level, 0, 5);
                // Keywords expected as ulong by TraceEvent API when using numeric overloads
                var keywords = unchecked((ulong)p.Keywords);

                if (p.ProviderGuid != Guid.Empty)
                {
                    _session.EnableProvider(p.ProviderGuid, (TraceEventLevel)level, keywords);
                    _log.Info(
                        $"{Loader} enabled provider GUID={p.ProviderGuid} Level={(int)level} Keywords=0x{keywords:X}");
                }
                else if (!string.IsNullOrWhiteSpace(p.ProviderName))
                {
                    _session.EnableProvider(p.ProviderName!, (TraceEventLevel)level, keywords);
                    _log.Info(
                        $"{Loader} enabled provider Name='{p.ProviderName}' Level={(int)level} Keywords=0x{keywords:X}");
                }
                else
                {
                    _log.Warn($"{Loader} skipped provider with neither name nor GUID.");
                    continue;
                }

                // Attach a generic callback that fires for all events from all providers
                _session.Source.Dynamic.All += data =>
                {
                    try
                    {
                        OnEvent(data);
                    }
                    catch (Exception ex)
                    {
                        if (_config.AuditLog)
                        {
                            _log.Warn($"{Loader} event handler error: {ex.Message}");
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                _log.Error(
                    $"{Loader} failed to enable provider '{p.ProviderName ?? p.ProviderGuid.ToString()}': {ex.Message}");
            }
        }

        // Start processing on a background thread
        Task.Run(() =>
        {
            try
            {
                _session!.Source.Process();
            }
            catch (Exception ex)
            {
                _log.Error($"{Loader} session processing error: {ex.Message}");
            }
        });
    }






    private void EnableKernelProviders()
    {
        if (_session == null)
        {
            throw new InvalidOperationException("Session not initialized.");
        }

        try
        {
            var kws = (KernelTraceEventParser.Keywords)_config.KernelKeywords;
            _session.EnableKernelProvider(kws);
            _kernelEnabled = true;
            _log.Info($"{Loader} kernel providers enabled Keywords=0x{_config.KernelKeywords:X}");
        }
        catch (Exception ex)
        {
            _log.Error($"{Loader} failed to enable kernel providers: {ex.Message}");
        }
    }






    // -------------------------
    // Event handling
    // -------------------------






    private void OnEvent(TraceEvent data)
    {
        // Timestamp normalization
        DateTime ts = data.TimeStamp.ToUniversalTime();

        // Basic identity
        var providerName = data.ProviderName ?? "Unknown";
        Guid providerGuid = data.ProviderGuid;
        var eventName = data.EventName ?? "";
        var taskName = Safe(() => data.TaskName) ?? "";
        var opcodeName = Safe(() => data.OpcodeName) ?? "";
        var task = SafeNullable(() => (int)data.Task) ?? 0;
        var opcode = SafeNullable(() => (int)data.Opcode) ?? 0;
        var level = SafeNullable(() => (int)data.Level) ?? 0;
        var keywords = (ulong)SafeNullable(() => (ulong)data.Keywords);

        // Identity and context
        var pid = data.ProcessID;
        var tid = data.ThreadID;
        string? procName = null;
        if (_config.IncludeProcessName && pid > 0)
        {
            try
            {
                using var p = Process.GetProcessById(pid);
                procName = p.ProcessName;
            }
            catch
            {
                /* process may have exited */
            }
        }

        // Activity correlation
        var activityId = data.ActivityID == Guid.Empty ? null : data.ActivityID.ToString();
        var relatedId = data.RelatedActivityID == Guid.Empty ? null : data.RelatedActivityID.ToString();

        // Payload extraction
        Dictionary<string, string>? payload = null;
        if (_config.IncludePayload)
        {
            try
            {
                if (data.PayloadNames is { Length: > 0 })
                {
                    payload = new Dictionary<string, string>(data.PayloadNames.Length,
                        StringComparer.OrdinalIgnoreCase);
                    for (var i = 0; i < data.PayloadNames.Length; i++)
                    {
                        var name = data.PayloadNames[i] ?? $"field{i}";
                        string val;
                        try
                        {
                            val = data.PayloadValue(i)?.ToString() ?? "";
                        }
                        catch
                        {
                            val = "";
                        }

                        payload[name] = val;
                    }
                }
            }
            catch
            {
                /* ignore payload failures */
            }
        }

        // Render a simple message when asked (best-effort)
        string? message = null;
        if (_config.IncludeRenderedMessage)
        {
            message = BuildRendered(providerName, eventName, payload);
        }

        // Normalization & hash
        var normalized = Normalize(message ?? eventName);
        var hash = Hash(providerName, eventName, task, opcode, normalized, payload);

        var env = new EtwEventEnvelope
        {
            ProviderName = providerName,
            ProviderGuid = providerGuid,
            EventName = eventName,
            Task = task,
            TaskName = taskName,
            Opcode = opcode,
            OpcodeName = opcodeName,
            Level = level,
            Keywords = keywords,

            ProcessId = pid,
            ThreadId = tid,
            ProcessName = procName,

            ActivityId = activityId,
            RelatedActivityId = relatedId,

            UtcTimestamp = ts,

            RenderedMessage = _config.IncludeRenderedMessage ? message : null,
            Payload = _config.IncludePayload ? payload : null,

            NormalizedMessage = normalized,
            EventHash = hash,

            Host = Environment.MachineName,
            SourceId = SourceId,
            LoaderName = Loader,
            SchemaVersion = SchemaVersion,
            CollectionMethod = CollectionMethod,
            RecordGuid = Guid.NewGuid().ToString()
        };

        // Dedup guard
        var allow = _config.DedupMaxPerWindow <= 0 || _dedup.Allow(env.EventHash, env.UtcTimestamp);
        if (!allow)
        {
            if (_config.AuditLog)
            {
                _log.Debug(
                    $"{Loader} dedup suppressed: {providerName}:{eventName} Hash={Trunc(env.EventHash, 12)}");
            }

            return;
        }

        if (_config.AuditLog)
        {
            _log.Debug(
                $"{Loader} audit Provider='{env.ProviderName}' Event='{env.EventName}' Level={env.Level} PID={env.ProcessId} Hash='{Trunc(env.EventHash, 12)}' Schema='{SchemaVersion}'");
        }

        _queue.Enqueue(env);
    }






    // -------------------------
    // Pump and batching
    // -------------------------






    private async Task PumpLoopAsync(CancellationToken ct)
    {
        List<EtwEventEnvelope> batch = new(_config.BatchSize);
        DateTime nextFlush = DateTime.UtcNow + _config.FlushInterval;

        while (!ct.IsCancellationRequested)
        {
            try
            {
                // Drain queue up to batch size
                while (batch.Count < _config.BatchSize && _queue.TryDequeue(out EtwEventEnvelope? item))
                {
                    batch.Add(item);
                }

                DateTime now = DateTime.UtcNow;
                var timeToFlush = now >= nextFlush;

                if (batch.Count >= _config.BatchSize || (timeToFlush && batch.Count > 0))
                {
                    await _sink.EmitBatchAsync(batch, ct).ConfigureAwait(false);
                    batch.Clear();
                    nextFlush = now + _config.FlushInterval;
                    _dedup.SweepExpired();
                }

                if (batch.Count == 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(10), ct).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _log.Error($"{Loader} pump error: {ex.Message}");
                if (_config.FailFast)
                {
                    throw;
                }

                await Task.Delay(TimeSpan.FromMilliseconds(50), ct).ConfigureAwait(false);
            }
        }

        // Final flush
        if (batch.Count > 0)
        {
            try
            {
                await _sink.EmitBatchAsync(batch, CancellationToken.None).ConfigureAwait(false);
            }
            catch
            {
                /* ignore */
            }
        }
    }






    // -------------------------
    // Helpers and utilities
    // -------------------------






    private static string? BuildRendered(string provider, string eventName, Dictionary<string, string>? payload)
    {
        if (payload is null || payload.Count == 0)
        {
            return $"{provider}:{eventName}";
        }

        var sb = new StringBuilder(provider.Length + eventName.Length + 32);
        sb.Append(provider).Append(':').Append(eventName).Append(' ');
        var first = true;
        foreach (KeyValuePair<string, string> kv in payload)
        {
            if (!first)
            {
                sb.Append(' ');
            }

            sb.Append(kv.Key).Append('=').Append(kv.Value);
            first = false;
        }

        return sb.ToString();
    }






    private static int Clamp(int v, int min, int max)
    {
        return v < min ? min : v > max ? max : v;
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






    private static T? SafeNullable<T>(Func<T> f) where T : struct
    {
        try
        {
            return f();
        }
        catch
        {
            return null;
        }
    }






    private static string Normalize(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return "";
        }

        s = RxGuid.Replace(s, "{GUID}");
        s = RxHex.Replace(s, "{HEX}");
        s = RxInt.Replace(s, "{N}");
        s = RxWs.Replace(s, " ").Trim();
        return s;
    }






    private static string Hash(string provider, string eventName, int task, int opcode, string normalized,
        Dictionary<string, string>? payload)
    {
        var sb = new StringBuilder(256);
        sb.Append(provider).Append('|')
            .Append(eventName).Append('|')
            .Append(task).Append('|')
            .Append(opcode).Append('|')
            .Append(normalized);

        if (payload is { Count: > 0 })
            // Deterministic order by key
        {
            foreach (KeyValuePair<string, string> kv in payload.OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
            {
                sb.Append('|').Append(kv.Key).Append('=').Append(kv.Value);
            }
        }

        // FNV-1a 64
        var hash = 1469598103934665603UL;
        foreach (var c in sb.ToString())
        {
            hash ^= c;
            hash *= 1099511628211UL;
        }

        return hash.ToString("x16");
    }






    private static string Trunc(string? s, int n)
    {
        return string.IsNullOrEmpty(s) ? "" : s.Length <= n ? s : s.Substring(0, n);
    }






    // -------------------------
    // Dedup index
    // -------------------------



    private sealed class DedupIndex(TimeSpan window, int maxPerWindow, int capacity, ILoggerSeverity log)
    {
        private readonly int _capacity = Math.Max(4096, capacity);
        private readonly ILoggerSeverity _log = log;

        private readonly Dictionary<string, DedupEntry> _map = new(StringComparer.Ordinal);
        private readonly TimeSpan _window = window <= TimeSpan.Zero ? TimeSpan.FromSeconds(1) : window;
        private DateTime _nextSweep = DateTime.UtcNow;






        public bool Allow(string key, DateTime when)
        {
            if (maxPerWindow <= 0)
            {
                return true;
            }

            if (!_map.TryGetValue(key, out DedupEntry? e))
            {
                e = new DedupEntry { First = when, Last = when, Count = 0 };
                _map[key] = e;
            }

            if (when - e.First > _window)
            {
                e.First = when;
                e.Count = 0;
            }

            e.Last = when;

            if (e.Count < maxPerWindow)
            {
                e.Count++;
                return true;
            }

            return false;
        }






        public void SweepExpired()
        {
            DateTime now = DateTime.UtcNow;
            if (now < _nextSweep)
            {
                return;
            }

            // Remove stale keys
            foreach (var k in _map.Where(p => now - p.Value.Last > _window).Select(p => p.Key).ToList())
            {
                _map.Remove(k);
            }

            // Enforce capacity
            if (_map.Count > _capacity)
            {
                foreach (var k in _map.OrderBy(p => p.Value.Last).Take(_map.Count - _capacity)
                             .Select(p => p.Key).ToList())
                {
                    _map.Remove(k);
                }

                _log.Warn($"{Loader} dedup map truncated to capacity={_capacity}");
            }

            _nextSweep = now + TimeSpan.FromSeconds(5);
        }






        private sealed class DedupEntry
        {
            public int Count;
            public DateTime First;
            public DateTime Last;
        }
    }
}



// -------------------------
// DTOs, config, sink, logger
// -------------------------