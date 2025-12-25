namespace SKAgentOrchestrator.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; } = new();
    private Window MyWindow { get; set; } 
    public MainPage()
    {
        InitializeComponent();
        DataContext = ViewModel;
        MyWindow = App.AppWindow!;
        
        MyWindow.Activated   += OnWindowActivated;
    }

    private async void OnWindowActivated(object sender, WindowActivatedEventArgs e)
    {
        await ViewModel.InitializeAsync();
    }
}
