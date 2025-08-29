// Project Name: LightweightAI.Core
// File Name: AnswerEnvelope.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.qANDa;


public class AnswerEnvelope
{
    public string Answer { get; set; }
    public double ConfidenceScore { get; set; }
    public string ModelVersion { get; set; }
    public string EventId { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsFallback { get; set; } = false;
    public List<string>? SourceDocs { get; set; }
    public string? ReasoningTrace { get; set; }





    public string ToJson()
    {
        return System.Text.Json.JsonSerializer.Serialize(this, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}