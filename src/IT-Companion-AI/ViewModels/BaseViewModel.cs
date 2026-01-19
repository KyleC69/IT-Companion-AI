using CommunityToolkit.Mvvm.ComponentModel;



namespace ITCompanionAI.ViewModels;


public class BaseViewModel : ObservableObject, INotifyPropertyChanged
{
    private string _title = string.Empty;





    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
}