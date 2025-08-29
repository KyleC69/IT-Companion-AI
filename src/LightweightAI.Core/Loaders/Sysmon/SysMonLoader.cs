using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightweightAI.Core.Abstractions;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Diagnostics.Eventing.Reader;

namespace LightweightAI.Core.Loaders.Sysmon;

public sealed class SysmonOptions
{
    public string Query { get; init; } = "*[System/Provider/@Name='Microsoft-Windows-Sysmon']";
    public string Channel { get; init; } = "Microsoft-Windows-Sysmon/Operational";
    public int BatchSize { get; init; } = 512;
}

public sealed class SysmonLoader : ISourceLoader
{
    private readonly ILogger<SysmonLoader> _log;
    private readonly SysmonOptions _opt;

    public SysmonLoader(ILogger<SysmonLoader> log, IOptions<SysmonOptions> opt)
    {
        _log = log;
        _opt = opt.Value;
    }

    public async IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        var q = new EventLogQuery(_opt.Channel, PathType.LogName, _opt.Query) { TolerateQueryErrors = true, ReverseDirection = false };
        using var reader = new EventLogReader(q);
        for (EventRecord? rec = reader.ReadEvent(); rec != null; rec = reader.ReadEvent())
        {
            ct.ThrowIfCancellationRequested();
            using (rec)
            {
                var ts = rec.TimeCreated?.ToUniversalTime() ?? DateTime.UtcNow;
                var host = rec.MachineName ?? "unknown";
                var evtId = rec.Id;
                var severity = rec.LevelDisplayName ?? "Info";
                var fields = new Dictionary<string, object?>();

                foreach (var prop in rec.Properties.Select((p, i) => (p, i)))
                    fields[$"Prop{prop.i}"] = prop.p.Value;

                yield return new RawEvent(
                    SourceKey: request.SourceKey,
                    EventId: evtId,
                    TimestampUtc: ts,
                    Host: host,
                    User: rec.UserId?.Value,
                    Severity: NormalizeSeverity(severity),
                    Fields: fields,
                    ProvenanceTag: $"sysmon/evtx:{rec.RecordId}"
                );
            }

            // throttle batch to yield control
            await Task.Yield();
        }
    }

    private static string NormalizeSeverity(string level) => level switch
    {
        "Critical" => "Critical",
        "Error" => "Error",
        "Warning" => "Warn",
        _ => "Info"
    };
}
