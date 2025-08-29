using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Microsoft.Extensions.Logging;

using System.Text.Json;

using LightweightAI.Core.Abstractions;


namespace LightweightAI.Core.Loaders.Generic;

public interface IFeedParser<T>
{
    bool TryParse(ReadOnlySpan<char> line, out T? obj);
    RawEvent MapToRaw(string sourceKey, in T obj);
}

public sealed class GenericFeedLoader<T> : ISourceLoader
{
    private readonly ILogger<GenericFeedLoader<T>> _log;
    private readonly IFeedParser<T> _parser;

    public GenericFeedLoader(ILogger<GenericFeedLoader<T>> log, IFeedParser<T> parser)
    {
        _log = log;
        _parser = parser;
    }

    public async IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        // Expect Parameters["path"] to be local file path or FIFO
        if (request.Parameters is null || !request.Parameters.TryGetValue("path", out var path))
            throw new ArgumentException("GenericFeedLoader requires Parameters['path'].");

        using var file = File.OpenText(path);
        for (string? line = await file.ReadLineAsync(); line is not null; line = await file.ReadLineAsync())
        {
            ct.ThrowIfCancellationRequested();
            if (_parser.TryParse(line.AsSpan(), out var obj) && obj is { })
                yield return _parser.MapToRaw(request.SourceKey, obj);
        }
    }
}

// Example JSON parser
public sealed class JsonFeedParser<T> : IFeedParser<T>
{
    private readonly JsonSerializerOptions _opts = new(JsonSerializerDefaults.Web);

    public bool TryParse(ReadOnlySpan<char> line, out T? obj)
    {
        try { obj = JsonSerializer.Deserialize<T>(line, _opts); return obj is not null; }
        catch { obj = default; return false; }
    }

    public RawEvent MapToRaw(string sourceKey, in T obj)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object?>>(JsonSerializer.Serialize(obj))!;
        var ts = dict.TryGetValue("timestamp", out var v) && v is string s && DateTimeOffset.TryParse(s, out var dto) ? dto.UtcDateTime : DateTime.UtcNow;
        var host = dict.TryGetValue("host", out var h) ? h?.ToString() ?? "unknown" : "unknown";
        var eventId = dict.TryGetValue("eventId", out var e) && int.TryParse(e?.ToString(), out var id) ? id : 0;

        return new RawEvent(sourceKey, eventId, ts, host, dict.GetValueOrDefault("user")?.ToString(), "Info", dict, $"json:{eventId}");
    }
}