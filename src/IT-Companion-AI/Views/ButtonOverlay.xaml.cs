using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;




namespace ITCompanionAI.Views;





public partial class ButtonOverlay : UserControl
{
    public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
        nameof(IsBusy),
        typeof(bool),
        typeof(ButtonOverlay),
        new PropertyMetadata(false));

    public ButtonOverlay()
    {
        InitializeComponent();
    }

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }
}