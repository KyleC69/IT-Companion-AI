// Project Name: SKAgent
// File Name: ButtonOverlay.xaml.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using ITCompanionAI.ViewModels;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.


namespace ITCompanionAI.Views;


public sealed partial class ButtonOverlay : UserControl
{
    public ButtonViewModel Viewmodel;





    public ButtonOverlay()
    {
        Viewmodel = new ButtonViewModel();
        DataContext = Viewmodel;

        InitializeComponent();

    }
}