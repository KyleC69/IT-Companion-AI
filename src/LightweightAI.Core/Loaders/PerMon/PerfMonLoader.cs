using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightweightAI.Core.Abstractions;

using Microsoft.Extensions.Logging;

namespace LightweightAI.Core.Loaders.PerMon;


public sealed class PerfmonLoader : ISourceLoader
{
    private readonly ILogger<PerfmonLoader> _log;

    public PerfmonLoader(ILogger<PerfmonLoader> log) => _log = log;

    public async IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        if (request.Parameters is null || !request.Parameters.TryGetValue("path", out var path))
            throw new ArgumentException("PerfmonLoader requires Parameters['path'].");

        using var reader = new StreamReader(File.OpenRead(path));
        string? header = await reader.ReadLineAsync();
        if (header is null) yield break;
        var columns = header.Split(',');

        for (string? line = await reader.ReadLineAsync(); line is not null; line = await reader.ReadLineAsync())
        {
            ct.ThrowIfCancellationRequested();
            var cells = line.Split(',');
            var map = new Dictionary<string, object?>(columns.Length);
            for (int i = 0; i < columns.Length && i < cells.Length; i++)
                map[columns[i].Trim()] = float.TryParse(cells[i], out var f) ? f : cells[i];

            var ts = map.TryGetValue("Time", out var t) && DateTimeOffset.TryParse(t?.ToString(), out var dto) ? dto.UtcDateTime : DateTime.UtcNow;
            var host = map.TryGetValue("Host", out var h) ? h?.ToString() ?? "unknown" : "unknown";

            yield return new RawEvent(request.SourceKey, EventId: 0, ts, host, null, "Info", map, "perfmon:csv");
        }
    }
}