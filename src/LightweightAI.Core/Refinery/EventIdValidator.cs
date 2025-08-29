// Project Name: LightweightAI.Core
// File Name: EventIdValidator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.DataRefineries;


public static class EventIdValidator
{
    // <machine>.<log/event source>.<rawID> — enforce this format
    private static readonly System.Text.RegularExpressions.Regex Pattern =
        new(@"^[^.]+\.[^.]+\.[^.\s]+$", System.Text.RegularExpressions.RegexOptions.Compiled);





    public static void Enforce(string eventId)
    {
        if (string.IsNullOrWhiteSpace(eventId))
            throw new ArgumentException("EventId is required");

        if (!Pattern.IsMatch(eventId))
            throw new ArgumentException(
                $"EventId '{eventId}' does not match <machine>.<source>.<rawId> format");
    }





    public static string Compose(string machine, string source, string rawId)
    {
        return $"{machine}.{source}.{rawId}";
    }
}