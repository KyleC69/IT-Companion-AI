// Project Name: LightweightAI.Core
// File Name: SysMonLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Diagnostics.Eventing.Reader;

using LightweightAI.Core.Abstractions;
using LightweightAI.Core.Engine;
using LightweightAI.Core.Engine.Models;

using Microsoft.Extensions.Options;


namespace LightweightAI.Core.Loaders.Sysmon;


/// <summary>
///     Sysmon event log loader with enhanced contract:
///     - Bookmark persistence
///     - Schema-aware property naming (where available)
///     - Provenance enrichment (schemaVersion, collectionMethod, providerGuid, recordId)
///     - Batching and robust error handling
/// </summary>
public sealed class SysmonLoader(
    ILoggerSeverity<SysmonLoader> log,
    IOptions<SysmonOptions> opt,
    ISysmonBookmarkStore? bookmarks = null)
    : ISourceLoader
{
    private readonly ISysmonBookmarkStore _bookmarks = bookmarks ?? new InMemorySysmonBookmarkStore();
    private readonly SysmonOptions _opt = opt.Value;





    public async IAsyncEnumerable<RawEvent> LoadAsync(
        SourceRequest request,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken ct = default)
    {
        var query = new EventLogQuery(this._opt.Channel, PathType.LogName, this._opt.Query)
        {
            TolerateQueryErrors = true,
            ReverseDirection = false
        };

        // Resume from bookmark if enabled and available
        EventBookmark? startBookmark = null;
        if (this._opt.EnableBookmarks)
            try
            {
                startBookmark = this._bookmarks.Load(request.SourceKey, this._opt.Channel);
            }
            catch (Exception ex)
            {
                log.LogWarning(ex, "SysmonLoader: failed to load bookmark for {SourceKey} {Channel}", request.SourceKey,
                    this._opt.Channel);
            }

        using EventLogReader reader = startBookmark is not null
            ? new EventLogReader(query, startBookmark)
            : new EventLogReader(query);

        var batchCount = 0;
        EventBookmark? lastBookmark = null;

        for (EventRecord? rec = SafeRead(reader); rec != null; rec = SafeRead(reader))
        {
            ct.ThrowIfCancellationRequested();

            using (rec)
            {
                try
                {
                    DateTime ts = rec.TimeCreated?.ToUniversalTime() ?? DateTime.UtcNow;
                    var host = rec.MachineName ?? "unknown";
                    var evtId = rec.Id;
                    var severityRaw = rec.LevelDisplayName ?? "Info";
                    var severity = NormalizeSeverity(severityRaw);

                    Dictionary<string, object?> fields = new(32)
                    {
                        ["_schemaVersion"] = this._opt.SchemaVersion,
                        ["_collectionMethod"] = "EventLog API",
                        ["_providerName"] = rec.ProviderName,
                        ["_providerGuid"] = rec.ProviderId?.ToString(),
                        ["_channel"] = this._opt.Channel,
                        ["_eventRecordId"] = rec.RecordId?.ToString(),
                        ["_task"] = rec.Task?.ToString(),
                        ["_opcode"] = rec.OpcodeDisplayName,
                        ["_keywords"] = rec.KeywordsDisplayNames is { } kw ? string.Join(';', kw) : null
                    };

                    // Map payload properties
                    if (rec.Properties is { Count: > 0 })
                    {
                        string[]? names = null;
                        if (this._opt.UseSchemaNames && SysmonSchema.TryGetPropertyNames(evtId, out var mapped))
                            names = mapped;

                        foreach ((EventProperty? p, var i) in rec.Properties.Select((p, i) => (p, i)))
                        {
                            var key = names is not null && i < names.Length
                                ? $"Sysmon.{names[i]}"
                                : this._opt.UseSchemaNames
                                    ? $"Sysmon.{i}"
                                    : $"Prop{i}";

                            fields[key] = p.Value;
                        }
                    }

                    // Emit RawEvent
                    yield return new RawEvent(
                        SourceKey: request.SourceKey,
                        EventId: evtId,
                        TimestampUtc: ts,
                        Host: host,
                        User: rec.UserId?.Value,
                        Severity: severity,
                        Fields: fields,
                        ProvenanceTag: $"sysmon/evtx:{rec.RecordId}"
                    );

                    lastBookmark = rec.Bookmark;
                }
                catch (Exception ex)
                {
                    log.LogWarning(ex, "SysmonLoader: failed to process record {RecordId} on {Channel}", rec.RecordId,
                        this._opt.Channel);
                }
            }

            batchCount++;
            if (batchCount >= this._opt.BatchSize)
            {
                // Persist bookmark on batch boundary
                if (this._opt.EnableBookmarks && lastBookmark is not null)
                {
                    TrySaveBookmark(request.SourceKey, this._opt.Channel, lastBookmark);
                    lastBookmark = null;
                }

                batchCount = 0;
                await Task.Yield(); // cooperative scheduling
            }
        }

        // Persist final bookmark if any
        if (this._opt.EnableBookmarks && lastBookmark is not null)
            TrySaveBookmark(request.SourceKey, this._opt.Channel, lastBookmark);
    }





    private static EventRecord? SafeRead(EventLogReader reader)
    {
        try
        {
            return reader.ReadEvent();
        }
        catch
        {
            return null;
        }
    }





    private void TrySaveBookmark(string sourceKey, string channel, EventBookmark bookmark)
    {
        try
        {
            this._bookmarks.Save(sourceKey, channel, bookmark);
        }
        catch (Exception ex)
        {
            log.LogWarning(ex, "SysmonLoader: failed to save bookmark for {SourceKey} {Channel}", sourceKey, channel);
        }
    }





    private static string NormalizeSeverity(string level)
    {
        return level switch
        {
            "Critical" => "Critical",
            "Error" => "Error",
            "Warning" => "Warn",
            "Information" => "Info",
            "Verbose" => "Debug",
            _ => "Info"
        };
    }
}