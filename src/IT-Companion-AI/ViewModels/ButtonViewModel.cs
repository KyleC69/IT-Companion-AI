// Project Name: SKAgent
// File Name: ButtonViewModel.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.ViewModels;


public partial class ButtonViewModel : BaseViewModel
{
    /// <summary>
    ///     Executes the action behind the "Harvest Api" button.
    /// </summary>
    [RelayCommand]
    private async Task HarvestApiAsync()
    {
        //  var orchestrator = new IngestionOrchestrator(new AiagentRagContext(), new ApiHarvester(path),new ApiDocHarvester(), new ApiHelpers.MarkdownDocParser());
        //    await orchestrator.RunAsync(new IngestionRequest(path)).ConfigureAwait(false);

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