// Project Name: LightweightAI.Core
// File Name: IngestionConfigurator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Text.Json;
using System.Text.Json.Serialization;

using LightweightAI.Core.Abstractions;
using LightweightAI.Core.Engine.Models;
using LightweightAI.Core.Interfaces;
using LightweightAI.Core.Loaders.Generic;
using LightweightAI.Core.Loaders.PerMon;
using LightweightAI.Core.Loaders.Sysmon;
using LightweightAI.Core.Loaders.Windows;

using Microsoft.Extensions.DependencyInjection;


namespace LightweightAI.Core.Engine.Intake;


internal sealed class IngestionConfigurator : IIngestionConfigurator
{
    public void Configure(ISourceRegistry registry, IServiceProvider services, string jsonConfig)
    {
        Root? root = JsonSerializer.Deserialize<Root>(jsonConfig,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (root == null) return;
        foreach (SourceDef src in root.Sources)
            try
            {
                (ISourceLoader? loader, IReadOnlyDictionary<string, string>? parameters) = CreateLoader(src, services);
                if (loader is not null)
                    registry.Register(src.Key, loader, parameters);
            }
            catch
            {
                // Swallow individual source failures to keep startup resilient
            }
    }





    private static (ISourceLoader? loader, IReadOnlyDictionary<string, string>? parameters) CreateLoader(SourceDef def,
        IServiceProvider sp)
    {
        var type = def.Type.ToLowerInvariant();
        Dictionary<string, string> dict = new(StringComparer.OrdinalIgnoreCase);
        if (def.Params is { } p && p.ValueKind == JsonValueKind.Object)
            foreach (JsonProperty prop in p.EnumerateObject())
                dict[prop.Name] = prop.Value.ToString();

        switch (type)
        {
            case "sysmon":
                return (ActivatorUtilities.CreateInstance<SysmonLoader>(sp), dict);


            case "eventlog":
            case "wevt":
                var channels = def.Params is not null &&
                               def.Params.Value.TryGetProperty("channels", out JsonElement chEl) &&
                               chEl.ValueKind == JsonValueKind.Array
                    ? chEl.EnumerateArray().Select(e => e.GetString() ?? string.Empty)
                        .Where(s => !string.IsNullOrWhiteSpace(s)).ToArray()
                    : new[] { "System", "Application" };
                var xpath = def.Params is not null && def.Params.Value.TryGetProperty("xpath", out JsonElement xEl)
                    ? xEl.GetString()
                    : null;
                return (new EventLogMultiChannelLoader(channels, xpath), dict);


            case "perfmon":
                return (ActivatorUtilities.CreateInstance<PerfmonLoader>(sp), dict);


            case "health":
                return (new HealthStatusLoader(), dict);


            case "trace":
            case "etw":
                IEnumerable<string> providers =
                    def.Params is not null && def.Params.Value.TryGetProperty("providers", out JsonElement provEl) &&
                    provEl.ValueKind == JsonValueKind.Array
                        ? provEl.EnumerateArray().Select(e => e.GetString() ?? string.Empty)
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                        : new[] { "Microsoft-Windows-Kernel-Process" };
                return (new EtwRealTimeLoader("MiniAI-RT", providers), dict);


            case "wmi":
                var wql = def.Params is not null && def.Params.Value.TryGetProperty("wql", out JsonElement wqlEl)
                    ? wqlEl.GetString() ?? "SELECT * FROM Win32_OperatingSystem"
                    : "SELECT * FROM Win32_OperatingSystem";
                return (new WmiQueryLoader(wql), dict);


            case "generic":
                // generic JSON feed: require path parameter; parser resolves at runtime
                return (ActivatorUtilities.CreateInstance<GenericFeedLoader<Dictionary<string, object?>>>(sp), dict);


            default:
                return (null, null);
        }
    }





    private sealed record Root(
        [property: JsonPropertyName("sources")]
        List<SourceDef> Sources);



    private sealed record SourceDef(
        [property: JsonPropertyName("key")] string Key,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("params")] JsonElement? Params);
}



internal class HealthStatusLoader : ISourceLoader
{
    public IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}