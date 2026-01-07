// Project Name: SKAgent
// File Name: BooleanToVisibilityConverter.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Converters;


public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is bool boolValue ? boolValue ? Visibility.Visible : Visibility.Collapsed : Visibility.Collapsed;
    }







    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is Visibility visibility && visibility == Visibility.Visible;
    }
}