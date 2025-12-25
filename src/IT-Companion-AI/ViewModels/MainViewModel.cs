using System.Collections.ObjectModel;

using Microsoft.Extensions.Logging;

using SKAgentOrchestrator;

namespace SKAgentOrchestrator.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private CancellationTokenSource? _cts;
    private readonly ILoggerFactory _loggerFactory;


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
        if (_orchestrator != null) return;
        IsBusy = true;
        try
        {
            _orchestrator = AgentOrchestrator.CreateDefault(_loggerFactory);

          //  await LoadHistoryAsync(); // temporarily disable history loading
            Messages.Add(new ChatMessageContent(ChatRole.System, "Agent initialized and ready.", "System"));






        }
        catch (Exception ex)
        {
            Messages.Add(new ChatMessageContent(ChatRole.System, $"Initialization failed: {ex.Message}", "System"));
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(CanSend));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task SendAsync()
    {
      
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
