using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Orchestration;
using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;

using System;
#pragma warning disable SKEXP0110




namespace ITCompanionAI.AgentFramework;

public static class KernelFactory
{
    private static readonly object SyncRoot = new();
    private static Kernel? _sharedKernel;

    /// <summary>
    /// Creates or returns the shared <see cref="Kernel"/> configured for all model definitions.
    /// </summary>
    /// <param name="loggerFactory">Logger factory used to propagate structured logging into the kernel.</param>
    /// <returns>Configured shared kernel instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when parameters are null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when required environment variables are missing or connection is unsupported.</exception>
    public static Kernel GetKernel(ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);

        if (App.TheKernel != null)
        {
            return App.TheKernel;
        }

        lock (SyncRoot)
        {
            App.TheKernel ??= BuildKernel(loggerFactory);
        }

        return App.TheKernel;
    }

    /// <summary>
    /// Creates a default kernel using all configured models for convenience.
    /// </summary>
    /// <param name="loggerFactory">Logger factory used to propagate structured logging into the kernel.</param>
    /// <returns>Configured shared kernel instance.</returns>
    public static Kernel CreateDefaultKernel(ILoggerFactory loggerFactory) => GetKernel(loggerFactory);





    /// <summary>
    /// Builds the shared <see cref="Kernel"/> with registered model definitions and logging configured in the service container.
    /// </summary>
    /// <param name="loggerFactory">Logger factory to propagate structured logging into the kernel and downstream components.</param>
    /// <returns>A fully built <see cref="Kernel"/> configured with all cataloged models.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the <c>GITHUB_TOKEN</c> environment variable is missing or empty, or when a model registration specifies an unsupported connection type.
    /// </exception>
    private static Kernel BuildKernel(ILoggerFactory loggerFactory)
    {
        var builder = Kernel.CreateBuilder();
        builder.Services.AddSingleton(loggerFactory);
        builder.Services.AddLogging();

        var apiKey = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("GITHUB_TOKEN environment variable is required to construct the kernel.");
        }

        foreach (var definition in ModelCatalog.AllModels)
        {
            RegisterModel(builder, definition, apiKey);
        }

        return builder.Build();
    }




    /// <summary>
    /// Registers a model definition with the kernel builder, enabling chat completion against the configured provider.
    /// </summary>
    /// <param name="builder">Kernel builder used to register model services.</param>
    /// <param name="definition">Model definition containing connection, identifier, and API details.</param>
    /// <param name="apiKey">API key used to authenticate with the configured model provider.</param>
    /// <exception cref="InvalidOperationException">Thrown when the model connection type is not supported.</exception>
    private static void RegisterModel(IKernelBuilder builder, ModelDefinition definition, string apiKey)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(definition);


        /*

        switch (definition.Connection.ToLowerInvariant())
        {
            case "openai":
                builder.AddOpenAIChatCompletion(
                    modelId: definition.Api,
                    apiKey: apiKey,
                    endpoint: new Uri("https://models.github.ai/inference"),
                    serviceId: definition.Id);
                break;
            default:
                throw new InvalidOperationException($"Unsupported connection type '{definition.Connection}'.");
        }




        */
    }
}

