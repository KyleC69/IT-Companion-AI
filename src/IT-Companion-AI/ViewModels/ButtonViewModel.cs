// Project Name: SKAgent
// File Name: ButtonViewModel.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using ITCompanionAI.AgentFramework;
using ITCompanionAI.KnowledgeBase;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace ITCompanionAI.ViewModels;


public partial class ButtonViewModel : BaseViewModel
{
    /// <summary>
    ///     Executes the action behind the "Harvest Api" button.
    /// </summary>
    [RelayCommand]
    private async Task HarvestApiAsync()
    {
        var path = "d:\\SKAPIRepo\\semantic-kernel\\dotnet\\src";
        App.Services.GetRequiredService<ILogger<App>>().LogInformation("Application Starting Up");

        var ingester = new APIIngestion(new KnowledgeBaseContext());
        //await ingester.StartIngestionAsync(""CancellationToken.None).ConfigureAwait(false);

        App.Services.GetRequiredService<ILogger<App>>().LogInformation("Ingestion Complete");
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