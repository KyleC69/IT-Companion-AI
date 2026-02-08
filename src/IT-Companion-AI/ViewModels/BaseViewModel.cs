using CommunityToolkit.Mvvm.ComponentModel;




namespace ITCompanionAI.ViewModels;





public class BaseViewModel : ObservableObject
{
    private string _title;





    public string Title
    {
        get => _title ?? string.Empty;
        set => this.SetProperty(ref _title, value);
    }
}