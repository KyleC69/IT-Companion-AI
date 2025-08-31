// Project Name: LightweightAI.Core
// File Name: StopWordFilter.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Conversational;


public static class StopwordFilter
{
    private static readonly HashSet<string> Stopwords = new()
    {
        "the", "is", "at", "which", "on", "and", "a", "an", "of", "to", "in", "for", "with", "by", "as", "that"
    };





    public static List<string> Apply(List<string>? tokens)
    {
        if (tokens == null) return new List<string>();
        return tokens.Where(t => !Stopwords.Contains(t)).ToList();
    }
}