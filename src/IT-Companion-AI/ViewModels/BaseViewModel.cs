using CommunityToolkit.Mvvm.ComponentModel;




namespace ITCompanionAI.ViewModels;





public class BaseViewModel : ObservableObject
{
    public string Title
    {
        get => field ?? string.Empty;
        set => SetProperty(ref field, value);
    }
}