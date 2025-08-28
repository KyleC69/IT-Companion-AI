// Project Name: LightweightAI.Core
// File Name: QueryCollector.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.qANDa;


public class QueryCollector
{
    public string RawInput { get; set; }
    public string SourceId { get; set; }
    public string EventId { get; set; }
    public DateTime Timestamp { get; set; }
    public Dictionary<string, string>? ClientMetadata { get; set; }





    public static QueryCollector FromInput(string input, string sourceId, Dictionary<string, string>? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("Input cannot be empty.");
        return new QueryCollector
        {
            RawInput = input.Trim(),
            SourceId = sourceId,
            EventId = Guid.NewGuid().ToString(),
            Timestamp = DateTime.UtcNow,
            ClientMetadata = metadata
        };
    }
}