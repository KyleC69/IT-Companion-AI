using CommunityToolkit.Mvvm.Input;

using ITCompanionAI.Ingestion.API;
using ITCompanionAI.Ingestion.Docs;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using KBContext = ITCompanionAI.EFModels.KBContext;



namespace ITCompanionAI.ViewModels;


public partial class ButtonViewModel : BaseViewModel
{
    private readonly ILogger<ButtonViewModel> logger;








    public ButtonViewModel()
    {
        logger = App.Services.GetRequiredService<ILogger<ButtonViewModel>>();
        logger.LogInformation("ButtonViewModel initialized.");
        HarvestApiCommand = new RelayCommand(async () => await HarvestApi());
        GetMSLearnCommand = new RelayCommand(async () => await GetMSLearn());
        IngestLocalCodeCommand = new RelayCommand(IngestLocalCode);
        HarvestDocCommand = new RelayCommand(async () => await HarvestDocAsync());
    }








    public RelayCommand IngestLocalCodeCommand { get; set; }
    public RelayCommand HarvestDocCommand { get; set; }
    public RelayCommand HarvestApiCommand { get; set; }
    public RelayCommand GetMSLearnCommand { get; set; }

    public string IngestTarget { get; set; }


    public event PropertyChangedEventHandler PropertyChanged;








    /// Executes the action behind the "Harvest Api" button.
    /// </summary>
    public async Task HarvestApi()
    {
        logger.LogInformation("Application Starting Up");


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
    private async Task HarvestDocAsync()
    {
        logger.LogInformation("Ingestion Complete");
        await Task.CompletedTask.ConfigureAwait(false);
    }








    /// <summary>
    ///     Executes the action behind the "Get Doc Blocs" button.
    /// </summary>
    private async Task GetMSLearn()
    {
        DocIngester ingester = new();
        await ingester.RunIngestion(IngestTarget).ConfigureAwait(false);


        logger.LogInformation("Ingestion Complete");
    }








    /// <summary>
    ///     Executes the action behind the "Harvest Docs" button.
    /// </summary>
    private async Task HarvestDocsAsync()
    {
        await Task.CompletedTask.ConfigureAwait(false);
        logger.LogInformation("Ingestion Complete");
    }








    /// <summary>
    ///     Executes the action behind the "Action 2" button.
    /// </summary>
    private void IngestLocalCode()
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