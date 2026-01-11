// Project Name: SKAgent
// File Name: BaseViewModel.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using CommunityToolkit.Mvvm.ComponentModel;

namespace ITCompanionAI.ViewModels;


public class BaseViewModel : ObservableObject
{
    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
}