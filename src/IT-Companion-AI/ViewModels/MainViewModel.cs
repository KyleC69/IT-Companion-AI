using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using ITCompanionAI.AgentFramework;

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using SkKnowledgeBase.Ingestion;
using SkKnowledgeBase.Agents;
using SkKnowledgeBase.Agents.Planning;
using SkKnowledgeBase.AgentFramework.Planning;


namespace ITCompanionAI.ViewModels;

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


    public ObservableCollection<ChatMessageViewModel> Messages { get; } = new();

    private bool _isBusy;
    private object _loop;

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
        _cts = new CancellationTokenSource();
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
        /*
        if (_loop is not null)
        {
            return;
        }

        IsBusy = true;
        try
        {
            await Task.CompletedTask;
        }
        finally
        {
            IsBusy = false;
        }

        */
    }












    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task SendAsync()
    {
        _cts = new CancellationTokenSource();

        // Example usage (e.g., from a hosted service or controller):
        var orchestrator = App.GetService<KnowledgeIngestionOrchestrator>();




        IsBusy = true;
        OnIsBusyChanged(true);
        OnPropertyChanged(nameof(CanCancel));

        try
        {


            var plan = await orchestrator.BuildOrUpdateKnowledgeBaseAsync(
         goal: "Ingest the Microsoft Semantic Kernel documentation focussing on orchestration capabilities and usage.",_cts.Token);



            await Task.CompletedTask;
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(CanSend));
            OnPropertyChanged(nameof(CanCancel));
        }









        //         var ingestion = App.GetService<IngestionAgent>();
        //    await ingestion.IngestAsync(new IngestionRequest(Url: "https://learn.microsoft.com/en-us/semantic-kernel/agents"));
        //     //    await verifier.VerifySymbolAsync("Microsoft.SemanticKernel.Agents.Agent");
        //        var recon = App.GetService<ReconciliationAgent>();
        //     await recon.ReconcileSymbolAsync("Microsoft.SemanticKernel.Agents.Agent");


        /*
        if (_loop is null || IsBusy || string.IsNullOrWhiteSpace(_userInput))
        {
            return;
        }

        IsBusy = true;
        var submittedInput = _userInput;
        UserInput = string.Empty;
        _cts = new CancellationTokenSource();
        OnPropertyChanged(nameof(CanCancel));

        try
        {
            Messages.Add(new ChatMessageViewModel("You", submittedInput));

            void LogTranscript(AgentTranscript transcript)
            {
                // Avoid duplicating the user entry; we already added it.
                if (string.Equals(transcript.Role, "user", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                Messages.Add(new ChatMessageViewModel(transcript.AgentName, transcript.Content));
            }

            var final = ""; //await _loop.RunAsync(submittedInput, LogTranscript, _cts.Token).ConfigureAwait(false);
            Messages.Add(new ChatMessageViewModel("Assistant", final));
        }
        catch (OperationCanceledException)
        {
            Messages.Add(new ChatMessageViewModel("System", "Request cancelled."));
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(CanSend));
            OnPropertyChanged(nameof(CanCancel));
        }
        */
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
//// Represents a chat message shown in the UI.
//// </summary>
public sealed record ChatMessageViewModel(string Author, string Content);
