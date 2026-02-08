using Microsoft.UI.Xaml;



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.




namespace ITCompanionAI;





/// <summary>
///     An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // ↓ Add this. ↓
        // Hide the default system title bar.
        ExtendsContentIntoTitleBar = true;
        // Replace system title bar with the WinUI TitleBar.
        this.SetTitleBar(AppTitleBar);
        // ↑ Add this. ↑
    }
}