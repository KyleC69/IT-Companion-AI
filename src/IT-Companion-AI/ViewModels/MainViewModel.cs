using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SkAgentGroup.AgentFramework;

using System.Collections.ObjectModel;

using static DIRegisterExtensions;

namespace SkAgentGroup.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private CancellationTokenSource? _cts;
    private readonly ILoggerFactory _loggerFactory;
    private AgentLoop? _loop;
    private string _userInput = string.Empty;
    public string UserInput
    {
        get => _userInput;
        set
        {
            if (_userInput != value)
            {
                _userInput = value;
                OnPropertyChanged(nameof(UserInput));
                OnPropertyChanged(nameof(CanSend));
                OnUserInputChanged(value);
            }
        }
    }


    public ObservableCollection<ChatMessageViewModel> Messages { get; } = new();

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy != value)
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(CanSend));
                OnPropertyChanged(nameof(CanCancel));
                OnIsBusyChanged(value);
            }
        }
    }

    public bool CanSend => !IsBusy && !string.IsNullOrWhiteSpace(UserInput);

    public bool CanCancel => IsBusy && _cts is { IsCancellationRequested: false };



    public MainViewModel()
    {
        Title = "Agentic AI";
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddDebug();
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Trace);

        });
    }









    /// <summary>
    /// Initializes the agent orchestrator and restores any persisted chat history,
    /// guarding against repeated initialization while updating the busy state.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous initialization operation.</returns>
    public async Task InitializeAsync()
    {
        var args = "";
        var services = new ServiceCollection();
        services.AddAgentSystem();

        var provider = services.BuildServiceProvider();

        // Ensure DB schema is correct at startup
        var connString = Environment.GetEnvironmentVariable("PGVECTOR_CONNECTION")!;
        await AgentMemorySchemaManager.EnsureAgentMemoryTableAsync(connString);

        var supervisor = provider.GetRequiredService<SupervisorAgent>();

        var goal = args.Length > 0
            ? string.Join(" ", args)
            : "Design a small multi-agent architecture in C# that uses Postgres for memory.";

        var result = await supervisor.ExecuteGoalAsync(goal);


    }










    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task SendAsync()
    {
        if (_loop is null || IsBusy || string.IsNullOrWhiteSpace(_userInput))
        {
            return;
        }

        IsBusy = true;
        var submittedInput = _userInput;

        try
        {
            Messages.Add(new ChatMessageViewModel("You", submittedInput));

            var response = await _loop.RunAsync(submittedInput);
            Messages.Add(new ChatMessageViewModel("Agent", response));
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private void Cancel()
    {
        if (_cts is { IsCancellationRequested: false })
        {
            _cts.Cancel();
            OnPropertyChanged(nameof(CanCancel));
        }
    }

 

    



    partial void OnUserInputChanged(string value);
    partial void OnIsBusyChanged(bool value);


}

/// <summary>
/// Represents a chat message shown in the UI.
/// </summary>
public sealed record ChatMessageViewModel(string Author, string Content);
