// Project Name: LightweightAI.Core
// File Name: Normalizer.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.qANDa;


public class Normalizer
{
    public string Normalize(string input, out int removedCount)
    {
        HashSet<string> stopwords = new() { "is", "the", "a" };
        var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        List<string> filtered = new();
        removedCount = 0;
        foreach (var t in tokens)
            if (stopwords.Contains(t.ToLower())) removedCount++;
            else filtered.Add(t);
        return string.Join(' ', filtered);
    }
}