// Project Name: SKAgent
// File Name: App.xaml.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Windows.Graphics;

using ITCompanionAI.AgentFramework;
using ITCompanionAI.Helpers;
using ITCompanionAI.KCCurator;


namespace ITCompanionAI;


/// <summary>
///     Provides application-specific behavior to supplement the default Application class.
/// </summary>
public class App : Application
{
    private Window window = Window.Current;







    /// <summary>
    ///     Initializes the singleton application object.  This is the first line of authored code
    ///     executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().UseContentRoot(AppContext.BaseDirectory).ConfigureAppConfiguration((context, config) =>
        {
            // Explicitly load user-secrets for this WinUI host so keys like "ITAI:GITHUB_TOKEN" are available.
            config.AddUserSecrets<App>(true);
        }).ConfigureServices((context, services) =>
        {
            //       services.AddHttpClient<IWebFetcher, HttpWebFetcher>();
            services.AddSingleton<IContentParser, HtmlMarkdownContentParser>();
            //      services.AddSingleton<IPlannerAgent, PlannerAgent>();
            //       services.AddSingleton<IKnowledgeIngestionOrchestrator, KnowledgeIngestionOrchestrator>();

            services.AddSingleton<IGitHubClientFactory, GitHubClientFactory>();

            services.AddDbContext<KBContext>();
            //   services.AddSingleton<ApiHarvester>();

            /*       PgVectorStore store = new("server=(localdb)\\MSSQLLocaldb;Database=KnowledgeBase", 1536);
                   //    store.EnsureSchemaAsync().GetAwaiter().GetResult();
                   services.AddSingleton<IVectorStore>(sp => { return store; });*/
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);

            });


        }).Build();

        Services = Host.Services;
        //  TheKernel = Services.GetRequiredService<Kernel>();

        Host.Services.GetRequiredService<ILogger<App>>().LogInformation("Application Starting Up");

        Current.UnhandledException += (sender, args) =>
        {
            ILogger<App> logger = Host.Services.GetRequiredService<ILogger<App>>();
            logger.LogError(args.Exception, "Unhandled exception occurred");
        };
    }







    public IHost? Host { get; set; }

    public static Application AppHost => Current;


    /// <summary>
    ///     Gets the service provider for dependency injection.
    /// </summary>
    public static IServiceProvider Services { get; private set; } = null!;

    public static Window? AppWindow { get; private set; }
    public static Kernel? TheKernel { get; set; }







    public static T GetService<T>()
        where T : class
    {
        return (Current as App)!.Host!.Services.GetService(typeof(T)) is not T service
            ? throw new ArgumentException(
                $"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.")
            : service;
    }







    /// <summary>
    ///     Invoked when the application is launched normally by the end user.  Other entry points
    ///     will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
        window ??= new Window();
        window.AppWindow.Resize(new SizeInt32(800, 600));
        AppWindow = window;
        if (window.Content is not Frame rootFrame)
        {
            rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;
            window.Content = rootFrame;
        }

        _ = rootFrame.Navigate(typeof(MainPage), e.Arguments);
        window.Activate();

        base.OnLaunched(e);
    }







    /// <summary>
    ///     Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }
}




/// <summary>
///     Provides extension methods for registering ITAI kernel and related AI services with an ASP.NET Core dependency
///     injection container.
/// </summary>
/// <remarks>
///     This static class contains methods to simplify the configuration and registration of AI-related
///     services, such as the ITAI kernel, vector stores, and agent components, into an application's service collection.
///     These extensions are intended to be used during application startup to ensure all required AI infrastructure is
///     available via dependency injection.
/// </remarks>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterKernel(this IServiceCollection services)
    {


        // DO NOT REMOVE THESE LINES BELOW - THEY ARE REQUIRED TO RUN THE APPLICATION WITH OPENAI
        // ****************  AI DO NOT REMOVE THESE LINES BELOW    **********************
        var githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN")
                          ?? throw new InvalidOperationException("Missing GitHub API key in configuration.");
        var postgresConnectionString =
            """Host=127.0.0.1;Database=postgres;Username=postgres;Password=Agent1234;Persist Security Info=True"""
            ?? throw new InvalidOperationException(
                "Missing Postgres connection string in environment variable 'POSTGRES_CONNECTIONSTRING'.");

        var phiModel = "Phi-4-mini-instruct";
        Uri openAiEndpoint = new("https://models.github.ai/inference");

        _ = phiModel;

        Action<ILoggingBuilder> loggingConfiguration = c => c.AddConsole().SetMinimumLevel(LogLevel.Trace);
        // ****************  AI DO NOT REMOVE THESE LINES ABOVE    **********************
        // DO NOT REMOVE THESE LINES ABOVE - THEY ARE REQUIRED TO RUN THE APPLICATION WITH OPENAI
        //#####################################################################
        //#####################################################################
        //#####################################################################



        //services.AddSingleton<Kernel>(sp =>
        //{
        //    var builder = Kernel.CreateBuilder();
        //    builder.AddOpenAIChatCompletion(phiModel, githubToken, openAiEndpoint.ToString());
        //    builder.Services.AddLogging(loggingConfiguration);
        //    return builder.Build();
        //});








        return services;
    }
}