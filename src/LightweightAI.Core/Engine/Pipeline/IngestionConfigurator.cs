// Project Name: LightweightAI.Core
// File Name: IngestionConfigurator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

using LightweightAI.Core.Loaders.Generic;
using LightweightAI.Core.Loaders.Perfmon;
using LightweightAI.Core.Loaders.Sysmon;
using LightweightAI.Core.Loaders.Windows;

using Microsoft.Extensions.DependencyInjection;
using LightweightAI.Core.Loaders.Windows;
using Microsoft.Extensions.Logging; // retain for adapter


namespace LightweightAI.Core.Engine.Pipeline;


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
                // Wrap EtwLoader in an adapter to satisfy ISourceLoader polymorphically
                var etwConfig = sp.GetRequiredService<EtwLoaderConfig>();
                var etwSink = sp.GetRequiredService<IEtwSink>();
                var etwLog = sp.GetRequiredService<ILoggerSeverity>();
                var etwInner = new EtwLoader(etwConfig, etwSink, etwLog);
                return (new EtwLoaderSourceAdapter(etwInner), dict);

            case "wmi":
                // Derive WQL and add to parameter map for provenance
                var wql = def.Params is not null && def.Params.Value.TryGetProperty("wql", out JsonElement wqlEl)
                    ? wqlEl.GetString() ?? "SELECT * FROM Win32_OperatingSystem"
                    : "SELECT * FROM Win32_OperatingSystem";
                dict["wql"] = wql;

                // Base config (may be registered with defaults) then create an effective config overriding Query
                var baseConfig = sp.GetService<WmiQueryLoaderConfig>() ?? new WmiQueryLoaderConfig();
                var effectiveConfig = new WmiQueryLoaderConfig
                {
                    Scope = baseConfig.Scope,
                    Query = string.IsNullOrWhiteSpace(baseConfig.Query) ? wql : baseConfig.Query,
                    SourceId = baseConfig.SourceId,
                    SampleInterval = baseConfig.SampleInterval,
                    DeltaOnly = baseConfig.DeltaOnly,
                    FailFast = baseConfig.FailFast,
                    AuditLog = baseConfig.AuditLog
                };

                var wmiSink = sp.GetRequiredService<IWmiQuerySink>();

                // Attempt to obtain a Microsoft logger and adapt it; fall back to a custom ILogger registration if present
                LightweightAI.Core.Loaders.Windows.ILogger wmiLogger;
                var msLogger = sp.GetService<Microsoft.Extensions.Logging.ILogger<WmiQueryLoader>>();
                if (msLogger is not null)
                {
                    wmiLogger = new MsLoggerAdapter(msLogger);
                }
                else
                {
                    wmiLogger = sp.GetRequiredService<LightweightAI.Core.Loaders.Windows.ILogger>();
                }

                var wmiInner = new WmiQueryLoader(effectiveConfig, wmiSink, wmiLogger);
                return (new WmiQuerySourceAdapter(wmiInner), dict);

            case "generic":
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

// Adapter to expose WmiQueryLoader as an ISourceLoader (placeholder implementation)
internal sealed class WmiQuerySourceAdapter : ISourceLoader, IDisposable
{
    private readonly WmiQueryLoader _inner;
    public WmiQuerySourceAdapter(WmiQueryLoader inner) => _inner = inner;

    public async IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request, [EnumeratorCancellation] CancellationToken ct = default)
    {
        await _inner.StartAsync(ct).ConfigureAwait(false);
        yield break;
    }

    public void Dispose() => _inner.Dispose();
}

// Adapter to expose EtwLoader via ISourceLoader
internal sealed class EtwLoaderSourceAdapter : ISourceLoader, IDisposable
{
    private readonly EtwLoader _inner;
    public EtwLoaderSourceAdapter(EtwLoader inner) => _inner = inner;

    public async IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request, [EnumeratorCancellation] CancellationToken ct = default)
    {
        await _inner.StartAsync(ct).ConfigureAwait(false);
        yield break;
    }

    public void Dispose() => _inner.Dispose();
}

// Polymorphic adapter bridging Microsoft.Extensions.Logging.ILogger to the custom ILogger interface
internal sealed class MsLoggerAdapter : LightweightAI.Core.Loaders.Windows.ILogger
{
    private readonly Microsoft.Extensions.Logging.ILogger _inner;
    public MsLoggerAdapter(Microsoft.Extensions.Logging.ILogger inner) => _inner = inner;
    public void Debug(string message) => _inner.LogDebug(message);
    public void Info(string message) => _inner.LogInformation(message);
    public void Warn(string message) => _inner.LogWarning(message);
    public void Error(string message) => _inner.LogError(message);
}