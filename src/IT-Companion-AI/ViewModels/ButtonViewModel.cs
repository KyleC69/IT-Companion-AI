// Project Name: SKAgent
// File Name: ButtonViewModel.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;

using ITCompanionAI.AgentFramework.Ingestion;
using ITCompanionAI.KCCurator;


namespace ITCompanionAI.ViewModels;


public class ButtonViewModel : BaseViewModel, INotifyPropertyChanged
{
    private readonly ILogger<ButtonViewModel> logger;







    public ButtonViewModel()
    {

        logger = App.Services.GetRequiredService<ILogger<ButtonViewModel>>();


        logger.LogInformation("ButtonViewModel initialized.");

        logger.LogInformation("^^^^^^^^^^^^^^^^^      Ingestion Finished    ************.");
    }







    public event PropertyChangedEventHandler PropertyChanged;







    /// Executes the action behind the "Harvest Api" button.
    /// </summary>
    [RelayCommand]
    public async Task HarvestApiAsync()
    {
        var path = "d:\\SKAPIRepo\\semantic-kernel\\dotnet\\src";
        logger.LogInformation("Application Starting Up");

        var ingester = new APIIngestion(new KBContext());
        await ingester.StartIngestionAsync().ConfigureAwait(false);
        logger.LogInformation("Ingestion Complete");



    }







    /// <summary>
    ///     Executes the action behind the "Harvest xml docx" button.
    /// </summary>
    [RelayCommand]
    private async Task HarvestXmlDocxAsync()
    {
        await Task.CompletedTask.ConfigureAwait(false);
    }







    /// <summary>
    ///     Executes the action behind the "Get Doc Blocs" button.
    /// </summary>
    [RelayCommand]
    private async Task GetDocBlocsAsync()
    {
        await Task.CompletedTask.ConfigureAwait(false);
    }







    /// <summary>
    ///     Executes the action behind the "Harvest Docs" button.
    /// </summary>
    [RelayCommand]
    private async Task HarvestDocsAsync()
    {
        await Task.CompletedTask.ConfigureAwait(false);
    }







    /// <summary>
    ///     Executes the action behind the "Action 2" button.
    /// </summary>
    [RelayCommand]
    private void Action2()
    {
    }







    /// <summary>
    ///     Executes the action behind the "Action 3" button.
    /// </summary>
    [RelayCommand]
    private void Action3()
    {
    }
}