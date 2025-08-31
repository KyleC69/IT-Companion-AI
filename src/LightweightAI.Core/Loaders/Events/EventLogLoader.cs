// Project Name: LightweightAI.Core
// File Name: EventLogLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers

using System.Diagnostics.Eventing.Reader;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;




namespace LightweightAI.Core.Loaders.Events;

public sealed class EventLogLoader : IDisposable
{
    private const string SchemaVersion = "1.1";
    private const string CollectionMethod = "EventLogReader API";
    private const string SourceId = "windows-eventlog";
    private const string Loader = nameof(EventLogLoader);

    private static readonly Regex RxGuid =
        new(@"[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}", RegexOptions.Compiled);
    private static readonly Regex RxHex = new(@"\b0x[0-9a-fA-F]+\b", RegexOptions.Compiled);
    private static readonly Regex RxIp = new(@"\b\d{1,3}(\.\d{1,3}){3}\b", RegexOptions.Compiled);
    private static readonly Regex RxInt = new(@"\b\d{2,}\b", RegexOptions.Compiled);
    private static readonly Regex RxWs = new(@"\s+", RegexOptions.Compiled);

    private readonly Dictionary<string, ChannelState> _channels = new(StringComparer.OrdinalIgnoreCase);
    private readonly EventLogLoaderConfig _config;
    private readonly DedupIndex _dedup;
    private readonly ILoggerSeverity _log;
    private readonly IEventSink _sink;
    private bool _disposed;

    public EventLogLoader(EventLogLoaderConfig config, IEventSink sink, ILoggerSeverity log)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _sink = sink ?? throw new ArgumentNullException(nameof(sink));
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _dedup = new DedupIndex(_config.DedupWindow, _config.DedupMaxPerWindow, _config.DedupCapacity, log);
    }

    public void Dispose()
    {
        if (_disposed) return;
        foreach (ChannelState st in _channels.Values) st.Dispose();
        _channels.Clear();
        _disposed = true;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        _log.Info(
            $"{Loader} starting. Channels={string.Join(",", _config.Channels)} Interval={_config.PollInterval} BatchSize={_config.BatchSize} IncludeXml={_config.IncludeXml} IncludeRendered={_config.IncludeRenderedMessage} DedupWindow={_config.DedupWindow} MaxPerWindow={_config.DedupMaxPerWindow}");

        InitializeReaders();

        List<EventEnvelope> batch = new(_config.BatchSize);

        while (!ct.IsCancellationRequested)
            try
            {
                var readCount = 0;

                foreach (ChannelState state in _channels.Values)
                {
                    readCount += ReadChannel(state, batch, ct);
                    if (batch.Count >= _config.BatchSize)
                    {
                        await _sink.EmitBatchAsync(batch, ct).ConfigureAwait(false);
                        batch.Clear();
                    }
                }

                if (batch.Count > 0)
                {
                    await _sink.EmitBatchAsync(batch, ct).ConfigureAwait(false);
                    batch.Clear();
                }

                _dedup.SweepExpired(); // cleanup dedup window

                if (readCount == 0)
                    await Task.Delay(_config.PollInterval, ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _log.Error($"{Loader} loop error: {ex.Message}");
                if (_config.FailFast) throw;
                await Task.Delay(TimeSpan.FromSeconds(1), ct).ConfigureAwait(false);
            }

        _log.Info($"{Loader} stopped.");
    }

    private void InitializeReaders()
    {
        foreach (var channel in _config.Channels)
            try
            {
                var query = new EventLogQuery(channel, PathType.LogName, _config.XPathFilter)
                {
                    TolerateQueryErrors = true,
                    ReverseDirection = false
                };

                var reader = new EventLogReader(query);
                EventBookmark? bookmark = null;

                if (_config.StartAt == EventStartPosition.End)
                {
                    System.Diagnostics.Eventing.Reader.EventRecord? last = null;
                    while (true)
                    {
                        System.Diagnostics.Eventing.Reader.EventRecord? ev = reader.ReadEvent();
                        if (ev is null) break;
                        last?.Dispose();
                        last = ev;
                    }

                    if (last is not null)
                    {
                        bookmark = last.Bookmark;
                        last.Dispose();
                    }
                }

                _channels[channel] = new ChannelState(channel, reader, bookmark);

                // Refactored: clearer ternary without stray quotes
                var xPathLabel = string.IsNullOrWhiteSpace(_config.XPathFilter) ? "(none)" : "custom";
                _log.Info($"{Loader} initialized channel '{channel}' StartAt={_config.StartAt} XPath={xPathLabel}");
            }
            catch (Exception ex)
            {
                _log.Error($"{Loader} failed to initialize channel '{channel}': {ex.Message}");
            }
    }

    private int ReadChannel(ChannelState state, List<EventEnvelope> batch, CancellationToken ct)
    {
        var count = 0;
        for (var i = 0; i < _config.MaxPerChannelPerPoll && !ct.IsCancellationRequested; i++)
        {
            System.Diagnostics.Eventing.Reader.EventRecord? rec = null;
            try
            {
                rec = state.Bookmark == null ? state.Reader.ReadEvent() : state.Reader.ReadEvent(state.Bookmark);
                if (rec is null) break;

                state.Bookmark = rec.Bookmark;

                EventEnvelope? env = Convert(rec);
                if (env == null) continue;

                var key = env.EventHash;
                var okToEmit = _config.DedupMaxPerWindow <= 0 || _dedup.Allow(key, env.UtcTimestamp);

                if (!okToEmit)
                {
                    if (_config.AuditLog)
                        _log.Debug(
                            $"{Loader} dedup suppressed: {env.ProviderName}:{env.EventId} Hash={Trunc(key, 12)}");
                    continue;
                }

                if (_config.AuditLog)
                    _log.Debug(
                        $"{Loader} audit Provider='{env.ProviderName}' Id={env.EventId} Level='{env.LevelName}' Channel='{env.Channel}' Hash='{Trunc(env.EventHash, 12)}' Schema='{SchemaVersion}'");

                batch.Add(env);
                count++;

                if (batch.Count >= _config.BatchSize)
                    break;
            }
            catch (EventLogException ex)
            {
                _log.Warn($"{Loader} channel '{state.Channel}' read error: {ex.Message}");
                break;
            }
            catch (Exception ex)
            {
                _log.Error($"{Loader} unexpected read error on '{state.Channel}': {ex.Message}");
                if (_config.FailFast) throw;
                break;
            }
            finally
            {
                rec?.Dispose();
            }
        }

        return count;
    }

    private EventEnvelope? Convert(System.Diagnostics.Eventing.Reader.EventRecord rec)
    {
        try
        {
            var provider = rec.ProviderName ?? "Unknown";
            var eventId = rec.Id;
            var levelRaw = rec.Level ?? 0;
            var levelName = SafeLevelName(rec);
            var task = rec.Task ?? 0;
            var taskName = SafeTaskName(rec);
            var opcode = rec.Opcode ?? 0;
            var opcodeName = SafeOpcodeName(rec);
            var keywords = rec.Keywords ?? 0;
            var keywordsDisplay = SafeKeywordsDisplay(rec);
            var channel = rec.LogName ?? "Unknown";
            var computer = rec.MachineName ?? Environment.MachineName;
            var activityId = rec.ActivityId?.ToString();
            var relatedId = rec.RelatedActivityId?.ToString();
            var recordId = rec.RecordId ?? 0L;
            DateTime createdUtc = rec.TimeCreated?.ToUniversalTime() ?? DateTime.UtcNow;

            string? userSid = null;
            string? userName = null;
            try
            {
                SecurityIdentifier? sid = rec.UserId;
                if (sid != null)
                {
                    userSid = sid.Value;
                    try { userName = sid.Translate(typeof(NTAccount))?.ToString(); }
                    catch { }
                }
            }
            catch { }

            string? rendered = null;
            if (_config.IncludeRenderedMessage)
                try { rendered = rec.FormatDescription(); } catch { rendered = null; }

            string? xml = null;
            if (_config.IncludeXml)
                try { xml = rec.ToXml(); } catch { xml = null; }

            string[]? props = null;
            if (_config.IncludeProperties && rec.Properties is { Count: > 0 })
            {
                props = new string[rec.Properties.Count];
                for (var i = 0; i < rec.Properties.Count; i++)
                    props[i] = rec.Properties[i]?.Value?.ToString() ?? "";
            }

            var normalized = Normalize(rendered ?? xml ?? "");
            var hash = Hash(provider, eventId, task, opcode, channel, normalized, props);

            return new EventEnvelope
            {
                ProviderName = provider,
                EventId = eventId,
                Level = levelRaw,
                LevelName = levelName,
                Task = task,
                TaskName = taskName,
                Opcode = opcode,
                OpcodeName = opcodeName,
                Keywords = (ulong)keywords,
                KeywordsDisplay = keywordsDisplay,
                Channel = channel,
                Computer = computer,
                RecordId = recordId,
                ActivityId = activityId,
                RelatedActivityId = relatedId,
                UserSid = userSid,
                UserName = userName,
                UtcTimestamp = createdUtc,
                RenderedMessage = _config.IncludeRenderedMessage ? rendered : null,
                EventXml = _config.IncludeXml ? xml : null,
                Properties = _config.IncludeProperties ? props : null,
                NormalizedMessage = normalized,
                EventHash = hash,
                Host = Environment.MachineName,
                SourceId = SourceId,
                LoaderName = Loader,
                SchemaVersion = SchemaVersion,
                CollectionMethod = CollectionMethod,
                RecordGuid = Guid.NewGuid().ToString()
            };
        }
        catch (Exception ex)
        {
            _log.Warn($"{Loader} convert error: {ex.Message}");
            return null;
        }
    }

    private static string SafeLevelName(System.Diagnostics.Eventing.Reader.EventRecord rec)
    {
        try { return rec.LevelDisplayName ?? (rec.Level?.ToString() ?? "Unknown"); }
        catch { return rec.Level?.ToString() ?? "Unknown"; }
    }

    private static string SafeTaskName(System.Diagnostics.Eventing.Reader.EventRecord rec)
    {
        try { return rec.TaskDisplayName ?? (rec.Task?.ToString() ?? ""); }
        catch { return rec.Task?.ToString() ?? ""; }
    }

    private static string SafeOpcodeName(System.Diagnostics.Eventing.Reader.EventRecord rec)
    {
        try { return rec.OpcodeDisplayName ?? (rec.Opcode?.ToString() ?? ""); }
        catch { return rec.Opcode?.ToString() ?? ""; }
    }

    private static string[] SafeKeywordsDisplay(System.Diagnostics.Eventing.Reader.EventRecord rec)
    {
        try
        {
            IEnumerable<string>? list = rec.KeywordsDisplayNames;
            return list == null ? Array.Empty<string>() : list.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        }
        catch { return Array.Empty<string>(); }
    }

    private static string Normalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        s = RxGuid.Replace(s, "{GUID}");
        s = RxHex.Replace(s, "{HEX}");
        s = RxIp.Replace(s, "{IP}");
        s = RxInt.Replace(s, "{N}");
        s = RxWs.Replace(s, " ").Trim();
        return s;
    }

    private static string Hash(string provider, int eventId, int task, int opcode, string channel, string normalized,
        string[]? props)
    {
        var sb = new StringBuilder(256);
        sb.Append(provider).Append('|')
          .Append(eventId).Append('|')
          .Append(task).Append('|')
          .Append(opcode).Append('|')
          .Append(channel).Append('|')
          .Append(normalized);

        if (props is { Length: > 0 })
        {
            sb.Append('|');
            for (var i = 0; i < props.Length; i++)
            {
                if (i > 0) sb.Append(',');
                sb.Append(props[i]);
            }
        }

        var hash = 1469598103934665603UL;
        foreach (var c in sb.ToString())
        {
            hash ^= c;
            hash *= 1099511628211UL;
        }

        return hash.ToString("x16");
    }

    private static string Trunc(string? s, int n) =>
        string.IsNullOrEmpty(s) ? "" : s.Length <= n ? s : s.Substring(0, n);

    private sealed class ChannelState(string channel, EventLogReader reader, EventBookmark? bookmark) : IDisposable
    {
        public string Channel { get; } = channel;
        public EventLogReader Reader { get; } = reader;
        public EventBookmark? Bookmark { get; set; } = bookmark;

        public void Dispose()
        {
            try { Reader?.Dispose(); } catch { }
        }
    }

    private sealed class DedupIndex(TimeSpan window, int maxPerWindow, int capacity, ILoggerSeverity log)
    {
        private readonly int _capacity = Math.Max(1024, capacity);
        private readonly ILoggerSeverity _log = log;
        private readonly Dictionary<string, DedupEntry> _map = new(StringComparer.Ordinal);
        private readonly TimeSpan _window = window <= TimeSpan.Zero ? TimeSpan.FromSeconds(1) : window;
        private DateTime _nextSweep = DateTime.UtcNow;

        public bool Allow(string key, DateTime when)
        {
            if (maxPerWindow <= 0) return true;

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
            if (now < _nextSweep) return;

            List<string> toRemove = new();
            foreach ((var k, DedupEntry v) in _map)
                if (now - v.Last > _window)
                    toRemove.Add(k);

            foreach (var k in toRemove) _map.Remove(k);

            if (_map.Count > _capacity)
            {
                foreach (var k in _map.OrderBy(p => p.Value.Last).Take(_map.Count - _capacity)
                             .Select(p => p.Key).ToList())
                    _map.Remove(k);
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
// DTO/config/sink types defined elsewhere