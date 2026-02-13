using CommunityToolkit.Mvvm.Input;

using ITCompanionAI.Ingestion;
using ITCompanionAI.Ingestion.Docs;
using ITCompanionAI.Services;

using Markdig;
using Markdig.Syntax;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;




namespace ITCompanionAI.ViewModels;





public partial class ButtonViewModel : BaseViewModel
{
    private readonly BusyState _busyState;
    private readonly ILogger<ButtonViewModel> logger;
    private CancellationTokenSource _cts;








    public ButtonViewModel()
    {
        _busyState = App.Services.GetRequiredService<BusyState>();
        logger = App.Services.GetRequiredService<ILogger<ButtonViewModel>>();
        logger.LogInformation("ButtonViewModel initialized.");
        HarvestApiCommand = new RelayCommand(async () => await HarvestApi());
        GetMSLearnCommand = new RelayCommand(async () => await GetMSLearn());
        IngestLocalCodeCommand = new RelayCommand(async () => IngestLocalCode());
        HarvestDocsCommand = new RelayCommand(async () => await HarvestDocAsync());
    }








    public bool CanCancel => IsBusy && _cts is { IsCancellationRequested: false };

    public RelayCommand IngestLocalCodeCommand { get; set; }
    public RelayCommand HarvestDocsCommand { get; set; }
    public RelayCommand HarvestApiCommand { get; set; }
    public RelayCommand GetMSLearnCommand { get; set; }





    public string IngestTarget
    {
        get => field;
        set
        {
            if (value == field)
            {
                return;
            }

            field = value;
            this.OnPropertyChanged();
            this.OnPropertyChanged(nameof(CanSend));
        }
    }





    public bool IsBusy { get; set; }
    public bool CanClick { set; get; }

    public bool CanSend => !IsBusy && !string.IsNullOrWhiteSpace(IngestTarget);








    /// Executes the action behind the "Harvest Api" button.
    /// </summary>
    public async Task HarvestApi()
    {
        await RunWithBusyAsync(async () =>
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

            await Task.CompletedTask.ConfigureAwait(false);
            logger.LogInformation("Ingestion Complete");
        }).ConfigureAwait(false);
    }








    /// <summary>
    ///     Harvests MS Learn content, either from a local repository or by scraping the web, depending on the configuration.
    /// </summary>
    private async Task HarvestDocAsync()
    {
        await RunWithBusyAsync(async () =>
        {
            logger.LogInformation("Ingestion of MS Learn docs has started from {0} ", IngestTarget);

            LearninRunner runner = new();
            //_ = runner.RunIngestionAsync(IngestTarget).ConfigureAwait(false);


        }).ConfigureAwait(false);
    }








    /// <summary>
    ///     Executes the action behind the "Get Doc Blocs" button.
    ///     Triggers the ingestion of code and documents in the MS Learn format. Can be local repo clones or live web scrape.
    /// </summary>
    private async Task GetMSLearn()
    {
        await RunWithBusyAsync(async () =>
        {
            LearninRunner runnner = new();
            await runnner.RunAsync(IngestTarget, CancellationToken.None).ConfigureAwait(false);


            logger.LogInformation("Ingestion Complete");
        }).ConfigureAwait(false);
    }








    /// <summary>
    ///     Executes the action behind the "Harvest Docs" button.
    /// </summary>
    private async Task HarvestDocsAsync()
    {
        await RunWithBusyAsync(async () =>
        {
            await Task.CompletedTask.ConfigureAwait(false);
            logger.LogInformation("Ingestion Complete");
        }).ConfigureAwait(false);
    }








    /// <summary>
    ///     Executes the action behind the "Action 2" button.
    /// </summary>
    private void IngestLocalCode()
    {
        logger.LogInformation("IngestLocalCode started-- Starting to translate files to text for rag ingestion");
        ConvertMdToText();

    }








    private void ConvertMdToText()
    {
        this.OnPropertyChanged(nameof(IsBusy));
        var Source = @"E:\IngestionSource\dotnet\docs\docs\docs\ai";
        var filepaths = Directory.EnumerateFiles(Source, "*.md", SearchOption.AllDirectories);

        logger.LogWarning("Starting markdown to text conversion for {0} files.", filepaths.Count());

        foreach (var file in filepaths)
        {
            var markdown = File.ReadAllText(file);
            var plainText = ExtractHeadersAndPlainText(markdown);
            var outputFilePath = Path.ChangeExtension(file, ".mdtxt");
            logger.LogInformation("Writing plain text to {0}", outputFilePath);
            File.WriteAllText(outputFilePath, plainText);
        }




        logger.LogTrace("Markdown to text conversion completed for {0} files.", filepaths.Count());






    }








    public static string ExtractHeadersAndPlainText(string markdown)
    {
        MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

        MarkdownDocument doc = Markdown.Parse(markdown, pipeline);




        using StringWriter writer = new();
        HeaderPreservingPlainTextRenderer renderer = new(writer);
        pipeline.Setup(renderer);

        renderer.Render(doc);
        return writer.ToString();
    }








    private async Task RunWithBusyAsync(Func<Task> action)
    {
        _busyState.IsBusy = true;
        try
        {
            await action().ConfigureAwait(false);
        }
        finally
        {
            _busyState.IsBusy = false;
        }
    }








    private void RunWithBusy(Action action)
    {
        _busyState.IsBusy = true;
        try
        {
            action();
        }
        finally
        {
            _busyState.IsBusy = false;
        }
    }








    /// <summary>
    ///     Executes the action behind the "Action 3" button.
    /// </summary>
    [RelayCommand]
    private void Action3()
    {
    }
}