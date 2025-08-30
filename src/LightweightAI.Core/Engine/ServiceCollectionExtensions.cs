// Project Name: LightweightAI.Core
// File Name: ServiceCollectionExtensions.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Abstractions;
using LightweightAI.Core.Analyzers;
using LightweightAI.Core.Engine.Alerts;
using LightweightAI.Core.Engine.FastDetectors;
using LightweightAI.Core.Engine.Intake;
using LightweightAI.Core.Interfaces;

using Microsoft.Extensions.DependencyInjection;


namespace LightweightAI.Core.Engine
{
    /// <summary>
    ///     Extension methods for registering core LightweightAI abstraction/services into a dependency injection
    ///     container. Currently a placeholder that wires option support; intended to expand as concrete services
    ///     (ingestion, refinement, anomaly detectors, fusion orchestrators) are formalized.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Registers baseline framework services required by the core library plus core intake pipeline pieces.
        /// </summary>
        public static IServiceCollection AddCoreContracts(this IServiceCollection services)
        {
            services.AddOptions();

            // Normalization + encoding
            services.AddSingleton<INormalizer, DefaultNormalizer>();
            services.AddSingleton<OneHotEncoder>(sp =>
                new OneHotEncoder(new Dictionary<int, int>(), new Dictionary<string, int>()));
            services.AddSingleton<Refinery.FeatureEncoder>();
            services.AddSingleton<Refinery.FeatureReducer>();

            // Detectors (fast)
            services.AddSingleton<EwmaDetector>();
            services.AddSingleton<ZScoreDetector>();
            services.AddSingleton<WindowedKnnDensity>();
            services.AddSingleton<Ewma>();
            services.AddSingleton<ZScore>();
            services.AddSingleton<IncrementalKnn>();

            // Drift detector (slow)
            services.AddSingleton<SlowDetectors.KsStreamingDrift>();
            services.AddSingleton<IDriftDetector>(sp => sp.GetRequiredService<SlowDetectors.KsStreamingDrift>());

            // Alerts
            services.AddSingleton<AlertDispatcher>();
            services.AddSingleton<IAlertDispatcher>(sp => sp.GetRequiredService<AlertDispatcher>());
            services.AddSingleton<Alerts.IAlertSink>(sp => new Alerts.SyslogAlertSink("127.0.0.1"));
            services.AddSingleton<Alerts.IAlertSink>(sp => new Alerts.WebhookAlertSink("http://localhost:5001/alert"));
            // Smtp optional (example values)
            services.AddSingleton<Alerts.IAlertSink>(sp =>
                new Alerts.SmtpAlertSink("localhost", 25, "alerts@local", "ops@local"));
            services.PostConfigure<AlertDispatcher>(d =>
            {
                ServiceProvider serviceProvider = services.BuildServiceProvider();
                foreach (IAlertSink sink in serviceProvider.GetServices<Alerts.IAlertSink>())
                    d.AddSink(sink);
            });

            // Source registry + configurator
            services.AddSingleton<ISourceRegistry, SourceRegistry>();
            services.AddSingleton<IIngestionConfigurator, IngestionConfigurator>();

            // Pipeline runner
            services.AddSingleton<IPipelineRunner, PipelineRunner>();

            return services;
        }
    }
}


namespace LightweightAI.Core.Engine
{
    // TODO: Missing implementation
    public class IIngestionConfigurator
    {
    }
}