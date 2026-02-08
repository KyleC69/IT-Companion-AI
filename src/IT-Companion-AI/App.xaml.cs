using ITCompanionAI.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        MainWindow mainWindow = GetService<MainWindow>();
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
                    config.AddJsonFile("appsettings.json", true, true);
                    config.AddEnvironmentVariables();
                    config.SetBasePath(AppContext.BaseDirectory);
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddDebug();
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureServices((context, services) => { ConfigureServices(context.Configuration, services); })
                .Build();
    }








    private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Views / composition root
        services.AddSingleton<MainWindow>();
        services.AddSingleton<HttpClientService>();
        services.AddSingleton(sp => { return new OllamaApiClient(new Uri("http://localhost:11434"), "bge-large:latest"); });


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