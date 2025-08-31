// Project Name: LightweightAI.Core
// File Name: JsonFeedParser.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Text.Json;



namespace LightweightAI.Core.Loaders.Generic;


public sealed class JsonFeedParser<T> : IFeedParser<T>
{
    private readonly JsonSerializerOptions _opts = new(JsonSerializerDefaults.Web);





    public bool TryParse(ReadOnlySpan<char> line, out T? obj)
    {
        try
        {
            obj = JsonSerializer.Deserialize<T>(line, this._opts);
            return obj is not null;
        }
        catch
        {
            obj = default;
            return false;
        }
    }





    public RawEvent MapToRaw(string sourceKey, in T obj)
    {
        Dictionary<string, object?>? dict =
            JsonSerializer.Deserialize<Dictionary<string, object?>>(JsonSerializer.Serialize(obj))!;
        DateTime ts =
            dict.TryGetValue("timestamp", out var v) && v is string s &&
            DateTimeOffset.TryParse(s, out DateTimeOffset dto)
                ? dto.UtcDateTime
                : DateTime.UtcNow;
        var host = dict.TryGetValue("host", out var h) ? h?.ToString() ?? "unknown" : "unknown";
        var eventId = dict.TryGetValue("eventId", out var e) && int.TryParse(e?.ToString(), out var id) ? id : 0;

        return new RawEvent(sourceKey, eventId, ts, host, dict.GetValueOrDefault("user")?.ToString(), "Info", dict,
            $"json:{eventId}");
    }
}