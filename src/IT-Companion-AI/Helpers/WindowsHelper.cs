namespace SkAgentGroup.Helpers;

public static class WindowHelper
{
    public static Window GetWindowForElement(UIElement element)
    {
   return element.XamlRoot?.GetType().GetProperty("Window")?.GetValue(element.XamlRoot) as Window
               ?? throw new InvalidOperationException("Unable to retrieve the Window for the specified element.");
    }
}
