using CommunityToolkit.Mvvm.Input;

using ITCompanionAI.Ingestion.Docs;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;




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
        HarvestDocsCommand = new RelayCommand(async () => await HarvestDocAsync());
    }








    public RelayCommand IngestLocalCodeCommand { get; set; }
    public RelayCommand HarvestDocsCommand { get; set; }
    public RelayCommand HarvestApiCommand { get; set; }
    public RelayCommand GetMSLearnCommand { get; set; }

    public string IngestTarget { get; set; }








    /// Executes the action behind the "Harvest Api" button.
    /// </summary>
    public async Task HarvestApi()
    {
        logger.LogInformation("Application Starting Up");


        //  var results =  await harvester.ExtractAsync(filePath, CancellationToken.None);


        // APIIngestion ingester = new(new KBContext());
        //   Guid sourceSnapshotId = await ingester.StartIngestionAsync();

        /*
        Guid sourceSnapshotId = new("997BE774-96B5-44D8-B6AD-14FC2EBDDABB");
        IngestionVerifier verifier = new(new KBContext());
        await verifier.VerifyApiTypesAsync(sourceSnapshotId, CancellationToken.None);
        await verifier.VerifyMembersAndParametersAsync(sourceSnapshotId, CancellationToken.None);
        */

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
        //  LearnIngestionRunner runner = new(new LearnPageParser(), new DocRepository("Data Source=Desktop-NC01091;Initial Catalog=KnowledgeBase;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"));
        //runner.IngestAsync([IngestTarget], Guid.Empty, Guid.Empty, CancellationToken.None).ConfigureAwait(false);

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