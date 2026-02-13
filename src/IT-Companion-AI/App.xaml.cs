using ITCompanionAI.Ingestion;
using ITCompanionAI.Ingestion.Docs;
using ITCompanionAI.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

using OllamaSharp;




namespace ITCompanionAI;





/// <summary>
///     Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private Window _window;








    public App()
    {
        InitializeComponent();
        Host = CreateHost();
        Services = Host.Services;
    }








    public IHost Host { get; }


    public static Application AppHost => Current;


    /// <summary>
    ///     Gets the service provider for dependency injection.
    /// </summary>
    public static IServiceProvider Services { get; private set; }

    public static Window AppWindow { get; private set; }








    public static T GetService<T>() where T : class
    {
        return Services.GetRequiredService<T>();
    }








    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window ??= new MainWindow();
        AppWindow = _window;

        // Root UI composition via DI.
        _ = GetService<MainWindow>();
        //  mainWindow.RequestedTheme = ElementTheme.Default;


        _window.Activate();
        base.OnLaunched(args);
    }








    private static IHost CreateHost()
    {
        return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Typical desktop configuration sources
                    _ = config.AddJsonFile("appsettings.json", true, true);
                    _ = config.AddUserSecrets<App>(true);
                    _ = config.AddEnvironmentVariables();
                    _ = config.SetBasePath(AppContext.BaseDirectory);
                })
                .ConfigureLogging((context, logging) =>
                {
                    _ = logging.ClearProviders();
                    _ = logging.AddDebug();
                    _ = logging.AddConsole();
                    _ = logging.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureServices((context, services) => { ConfigureServices(context.Configuration, services); })
                .Build();
    }








    private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Views / composition root
        _ = services.AddSingleton<MainWindow>();
        _ = services.AddSingleton<HttpClientService>();
        _ = services.AddSingleton<BusyState>();
        _ = services.Configure<IngestionSettings>(configuration.GetSection("Ingestion"));
        _ = services.AddSingleton(sp =>
        {
            IngestionSettings settings = sp.GetRequiredService<IOptions<IngestionSettings>>().Value;
            return string.IsNullOrWhiteSpace(settings.OllamaBaseUrl) || string.IsNullOrWhiteSpace(settings.OllamaModel)
                    ? throw new InvalidOperationException("Missing Ingestion:OllamaBaseUrl or Ingestion:OllamaModel configuration.")
                    : new OllamaApiClient(new Uri(settings.OllamaBaseUrl), settings.OllamaModel);
        });

        //services.AddDbContext<KBContext>(options => { options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); });
        _ = services.AddSingleton<LocalFileParser>();
        services.AddSingleton<LearnPageParser>();







        // Example registrations (remove if unused):
        // services.AddSingleton<IMyService, MyService>();
        // services.AddTransient<MyViewModel>();
        //
        // If you use options:
        // services.AddOptions<MyOptions>().Bind(configuration.GetSection("MyOptions"));
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