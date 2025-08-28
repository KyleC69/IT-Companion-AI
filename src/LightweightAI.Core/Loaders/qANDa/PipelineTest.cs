// Project Name: LightweightAI.Core
// File Name: PipelineTest.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.qANDa;


public static class PipelineTest
{
    public static void RunTests()
    {
        var runner = new Engine.PipelineRunner();

        var inputs = new[]
        {
            "What is the retention policy?",
            "How is audit logging handled?",
            "Tell me about the anomaly detection module.",
            "What happens if the buffer explodes?",
            "Explain provenance mapping."
        };

        foreach (var input in inputs)
        {
            //AnswerEnvelope result = runner.Run(input, "test-suite");
            // Console.WriteLine(result.ToJson());
            //  Console.WriteLine(new string('-', 80));
        }
    }
}