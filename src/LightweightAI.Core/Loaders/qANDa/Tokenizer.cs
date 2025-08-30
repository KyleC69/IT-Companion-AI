// Project Name: LightweightAI.Core
// File Name: Tokenizer.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Text.RegularExpressions;


namespace LightweightAI.Core.Loaders.qANDa;


public static class Tokenizer
{
    private static readonly Regex TokenRegex = new("[A-Za-z0-9_]+", RegexOptions.Compiled);





    public static List<string> Tokenize(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return new List<string>();
        return TokenRegex.Matches(input)
            .Select(m => m.Value.ToLowerInvariant())
            .ToList();
    }
}