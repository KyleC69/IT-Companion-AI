using CommunityToolkit.Mvvm.ComponentModel;




namespace ITCompanionAI.Services;




public class BusyState : ObservableObject
{
    public bool IsBusy
    {
        get;
        set => SetProperty(ref field, value);
    }
}
