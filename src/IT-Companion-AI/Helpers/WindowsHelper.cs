// Project Name: SKAgent
// File Name: WindowsHelper.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Helpers;


public static class WindowHelper
{
    public static Window GetWindowForElement(UIElement element)
    {
        return element.XamlRoot?.GetType().GetProperty("Window")?.GetValue(element.XamlRoot) as Window
               ?? throw new InvalidOperationException("Unable to retrieve the Window for the specified element.");
    }
}