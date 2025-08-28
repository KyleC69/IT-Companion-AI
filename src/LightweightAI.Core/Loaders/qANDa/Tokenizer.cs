// Project Name: LightweightAI.Core
// File Name: Tokenizer.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.qANDa;


public static class Tokenizer
{
    public static List<string> Tokenize(string input)
    {
        return input
            .ToLowerInvariant()
            .Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }
}