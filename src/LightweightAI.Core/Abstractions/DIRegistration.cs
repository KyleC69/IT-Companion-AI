using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LightweightAI.Core.Abstractions;

/// <summary>
/// Extension methods for registering core LightweightAI abstraction/services into a dependency injection
/// container. Currently a placeholder that wires option support; intended to expand as concrete services
/// (ingestion, refinement, anomaly detectors, fusion orchestrators) are formalized.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers baseline framework services required by the core library plus core intake pipeline pieces.
    /// </summary>
    public static IServiceCollection AddCoreContracts(this IServiceCollection services)
    {
        services.AddOptions();

        // Normalization + encoding
        services.AddSingleton<INormalizer, DefaultNormalizer>();
        services.AddSingleton<OneHotEncoder>(sp => new OneHotEncoder(new Dictionary<int,int>(), new Dictionary<string,int>()));
        services.AddSingleton<Refinery.FeatureEncoder>();
        services.AddSingleton<Refinery.FeatureReducer>();

        // Detectors (fast)
        services.AddSingleton<EwmaDetector>();
        services.AddSingleton<ZScoreDetector>();
        services.AddSingleton<WindowedKnnDensity>();
        services.AddSingleton<Engine.FastDetectors.Ewma>();
        services.AddSingleton<Engine.FastDetectors.ZScore>();
        services.AddSingleton<Engine.FastDetectors.IncrementalKnn>();

        // Drift detector (slow)
        services.AddSingleton<Engine.SlowDetectors.KsStreamingDrift>();
        services.AddSingleton<IDriftDetector>(sp => sp.GetRequiredService<Engine.SlowDetectors.KsStreamingDrift>());

        // Alerts
        services.AddSingleton<Engine.AlertDispatcher>();
        services.AddSingleton<IAlertDispatcher>(sp => sp.GetRequiredService<Engine.AlertDispatcher>());
        services.AddSingleton<Engine.Alerts.IAlertSink>(sp => new Engine.Alerts.SyslogAlertSink("127.0.0.1"));
        services.AddSingleton<Engine.Alerts.IAlertSink>(sp => new Engine.Alerts.WebhookAlertSink("http://localhost:5001/alert"));
        // Smtp optional (example values)
        services.AddSingleton<Engine.Alerts.IAlertSink>(sp => new Engine.Alerts.SmtpAlertSink("localhost",25,"alerts@local","ops@local"));
        services.PostConfigure<Engine.AlertDispatcher>(d =>
        {
            var serviceProvider = services.BuildServiceProvider();
            foreach (var sink in serviceProvider.GetServices<Engine.Alerts.IAlertSink>())
                d.AddSink(sink);
        });

        // Source registry + configurator
        services.AddSingleton<Engine.ISourceRegistry, Engine.SourceRegistry>();
        services.AddSingleton<Engine.Intake.IIngestionConfigurator, Engine.Intake.IngestionConfigurator>();

        // Pipeline runner
        services.AddSingleton<Engine.IPipelineRunner, Engine.PipelineRunner>();

        return services;
    }
}