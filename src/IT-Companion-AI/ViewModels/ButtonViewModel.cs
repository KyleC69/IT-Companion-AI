using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ITCompanionAI.Ingestion.API;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using KBContext = ITCompanionAI.EFModels.KBContext;



namespace ITCompanionAI.ViewModels;


public partial class ButtonViewModel : BaseViewModel, INotifyPropertyChanged
{
    private readonly ILogger<ButtonViewModel> logger;


    [ObservableProperty] private string _ingestTarget;








    public ButtonViewModel()
    {
        logger = App.Services.GetRequiredService<ILogger<ButtonViewModel>>();


        logger.LogInformation("ButtonViewModel initialized.");
    }








    public event PropertyChangedEventHandler PropertyChanged;








    /// Executes the action behind the "Harvest Api" button.
    /// </summary>
    [RelayCommand]
    public async Task HarvestApiAsync()
    {
        logger.LogInformation("Application Starting Up");

        //   var ingester = new APIIngestion(new KBContext());
        //    await ingester.StartIngestionAsync().ConfigureAwait(false);
        var filePath = """f:\SKApiRepo\semantic-kernel\dotnet\src""";


        //  var results =  await harvester.ExtractAsync(filePath, CancellationToken.None);


        //   var ingester = new APIIngestion(new KBContext());
        //  var sourceSnapshotId =  await ingester.StartIngestionAsync();
        Guid sourceSnapshotId = new("997BE774-96B5-44D8-B6AD-14FC2EBDDABB");


        IngestionVerifier verifier = new(new KBContext());
        await verifier.VerifyApiTypesAsync(sourceSnapshotId, CancellationToken.None);
        await verifier.VerifyMembersAndParametersAsync(sourceSnapshotId, CancellationToken.None);


        logger.LogInformation("Ingestion Complete");
    }








    /// <summary>
    ///     Executes the action behind the "Harvest xml docx" button.
    /// </summary>
    [RelayCommand]
    private async Task HarvestXmlDocxAsync()
    {
        logger.LogInformation("Ingestion Complete");
        await Task.CompletedTask.ConfigureAwait(false);
    }








    /// <summary>
    ///     Executes the action behind the "Get Doc Blocs" button.
    /// </summary>
    [RelayCommand]
    private async Task GetDocBlocsAsync()
    {
        await Entrypoint.Main(_ingestTarget);


        logger.LogInformation("Ingestion Complete");
    }








    /// <summary>
    ///     Executes the action behind the "Harvest Docs" button.
    /// </summary>
    [RelayCommand]
    private async Task HarvestDocsAsync()
    {
        await Task.CompletedTask.ConfigureAwait(false);
        logger.LogInformation("Ingestion Complete");
    }








    /// <summary>
    ///     Executes the action behind the "Action 2" button.
    /// </summary>
    [RelayCommand]
    private void Action2()
    {
        logger.LogInformation("Ingestion Complete");
    }








    /// <summary>
    ///     Executes the action behind the "Action 3" button.
    /// </summary>
    [RelayCommand]
    private void Action3()
    {
    }
}