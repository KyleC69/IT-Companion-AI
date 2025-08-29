// Project Name: LightweightAI.Core
// File Name: Program.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers

using Microsoft.Extensions.DependencyInjection;
using LightweightAI.Core.Abstractions;
using LightweightAI.Core.Engine;
using LightweightAI.Core.Engine.Intake;

namespace LightweightAI.Core;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // Minimal DI container
        var services = new ServiceCollection();
        services.AddCoreContracts();

        // Register optional fusion components later as needed
        services.AddSingleton<IFusionEngine, FusionEngine>();
        services.AddSingleton(new FusionConfig(System.Collections.Immutable.ImmutableDictionary<string, double>.Empty.Add("count", 0.1).Add("weighted_score", 1.0), 0.75));
        services.AddSingleton(new HysteresisDecider(0.6, 0.8));
        services.AddSingleton<RulesEngine>();

        using var sp = services.BuildServiceProvider();

        // Dynamically configure sources from embedded / sample JSON
        var json = args.Length > 0 && File.Exists(args[0])
            ? await File.ReadAllTextAsync(args[0])
            : "{ \"sources\": [ { \"key\": \"sysmon\", \"type\": \"sysmon\" }, { \"key\": \"eventlog\", \"type\": \"eventlog\", \"params\": { \"channels\": [\"System\", \"Application\"] } }, { \"key\": \"health\", \"type\": \"health\" } ] }";

        var registry = sp.GetRequiredService<ISourceRegistry>();
        var configurator = sp.GetRequiredService<IIngestionConfigurator>();
        configurator.Configure(registry, sp, json);

        var runner = sp.GetRequiredService<IPipelineRunner>();
        var pipeline = new Pipeline(runner, registry);
        Console.WriteLine("[Pipeline] Starting ingestion...");
        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };
        await pipeline.ExecuteAsync(cts.Token);
    }
}