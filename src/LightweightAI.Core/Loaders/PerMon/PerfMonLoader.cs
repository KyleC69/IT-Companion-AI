// Project Name: LightweightAI.Core
// File Name: PerfMonLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Abstractions;
using LightweightAI.Core.Engine;
using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Loaders.PerMon;


public sealed class PerfmonLoader(ILoggerSeverity<PerfmonLoader> log) : ISourceLoader
{
    private readonly ILoggerSeverity<PerfmonLoader> _log = log;





    public async IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        if (request.Parameters is null || !request.Parameters.TryGetValue("path", out var path))
            throw new ArgumentException("PerfmonLoader requires Parameters['path'].");

        using var reader = new StreamReader(File.OpenRead(path));
        var header = await reader.ReadLineAsync();
        if (header is null) yield break;
        var columns = header.Split(',');

        for (var line = await reader.ReadLineAsync(); line is not null; line = await reader.ReadLineAsync())
        {
            ct.ThrowIfCancellationRequested();
            var cells = line.Split(',');
            Dictionary<string, object?> map = new(columns.Length);
            for (var i = 0; i < columns.Length && i < cells.Length; i++)
                map[columns[i].Trim()] = float.TryParse(cells[i], out var f) ? f : cells[i];

            DateTime ts =
                map.TryGetValue("Time", out var t) && DateTimeOffset.TryParse(t?.ToString(), out DateTimeOffset dto)
                    ? dto.UtcDateTime
                    : DateTime.UtcNow;
            var host = map.TryGetValue("Host", out var h) ? h?.ToString() ?? "unknown" : "unknown";

            yield return new RawEvent(request.SourceKey, EventId: 0, ts, host, null, "Info", map, "perfmon:csv");
        }
    }
}