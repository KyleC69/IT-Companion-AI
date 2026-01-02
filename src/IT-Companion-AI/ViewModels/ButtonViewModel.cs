using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Threading.Tasks;

using ITCompanionAI.AgentFramework.Ingestion;


namespace ITCompanionAI.ViewModels;

public partial class ButtonViewModel : BaseViewModel
{



    public ButtonViewModel()
    {
    }
    /// <summary>
    /// Executes the action behind the "Harvest Api" button.
    /// </summary>
    [RelayCommand]
    private async Task HarvestApiAsync()
    {
        var path = @"D:\SkApiRepo\semantic-kernel\dotnet\src";
        var orchestrator = new IngestionOrchestrator(new AiagentRagContext(), new ApiHarvester(path),new ApiDocHarvester(), new ApiHelpers.MarkdownDocParser());
        await orchestrator.RunAsync(new IngestionRequest(path)).ConfigureAwait(false);

    }

    /// <summary>
    /// Executes the action behind the "Harvest xml docx" button.
    /// </summary>
    [RelayCommand]
    private async Task HarvestXmlDocxAsync()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// Executes the action behind the "Get Doc Blocs" button.
    /// </summary>
    [RelayCommand]
    private async Task GetDocBlocsAsync()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// Executes the action behind the "Harvest Docs" button.
    /// </summary>
    [RelayCommand]
    private async Task HarvestDocsAsync()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// Executes the action behind the "Action 2" button.
    /// </summary>
    [RelayCommand]
    private void Action2()
    {
    }

    /// <summary>
    /// Executes the action behind the "Action 3" button.
    /// </summary>
    [RelayCommand]
    private void Action3()
    {
    }
}




