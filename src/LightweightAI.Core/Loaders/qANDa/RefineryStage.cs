// Project Name: LightweightAI.Core
// File Name: RefineryStage.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.qANDa;


public class QueryRefinery
{
    public string NormalizedQuery { get; set; }
    public List<string> Tokens { get; set; }
    public string SourceEventId { get; set; }
    public string LanguageCode { get; set; }





    public static QueryRefinery Refine(QueryCollector collector, bool filterStopwords = true)
    {
        List<string> tokens = Tokenizer.Tokenize(collector.RawInput);
        if (filterStopwords) tokens = StopwordFilter.Apply(tokens);

        return new QueryRefinery
        {
            NormalizedQuery = string.Join(" ", tokens),
            Tokens = tokens,
            SourceEventId = collector.EventId,
            LanguageCode = "en" // stub for future multilingual support
        };
    }
}