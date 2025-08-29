// Project Name: LightweightAI.Core
// File Name: AuditLogger.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.qANDa;


public static class AuditLogger
{
    public static void Log(object obj, string level = "Info")
    {
        var json = System.Text.Json.JsonSerializer.Serialize(obj);
        Console.WriteLine($"[{level}] {DateTime.UtcNow:o} {json}");
    }





    public static void LogEnvelope(AnswerEnvelope envelope)
    {
        Log(envelope, "Answer");
    }





    public static void LogCollector(QueryCollector collector)
    {
        Log(collector, "Collector");
    }





    public static void LogRefinery(QueryRefinery refinery)
    {
        Log(refinery, "Refinery");
    }
}