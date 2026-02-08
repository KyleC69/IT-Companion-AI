using ITCompanionAI.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;




namespace ITCompanionAI.Views;





public partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }








    public MainViewModel ViewModel { get; } = new();
    private Window MyWindow { get; }
}