// Project Name: SKAgent
// File Name: MainPage.xaml.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using ITCompanionAI.ViewModels;


namespace ITCompanionAI.Views;


public sealed class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        DataContext = ViewModel;
        MyWindow = App.AppWindow!;

        MyWindow.Activated += OnWindowActivated;
    }







    public MainViewModel ViewModel { get; } = new();
    private Window MyWindow { get; }







    private async void OnWindowActivated(object sender, WindowActivatedEventArgs e)
    {


    }
}