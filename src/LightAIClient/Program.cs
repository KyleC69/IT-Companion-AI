// Project Name: LightAIClient
// File Name: Program.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Loaders.Conversational;



namespace LightAIClient;


internal class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Pipeline Trainer & Response Tester";
        Console.WriteLine("=== Modular AI Pipeline Console ===");
        Console.WriteLine("Type 'exit' to quit, 'batch' to run test suite, or 'train' to simulate training mode.\n");

        var runner = new QPipelineRunner();

        while (true)
        {
            Console.Write("Input > ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) continue;
            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

            if (input.Equals("batch", StringComparison.OrdinalIgnoreCase))
                //  QPipelineTest.RunTests();
                continue;

            if (input.Equals("train", StringComparison.OrdinalIgnoreCase))
            {
                RunTrainingMode(runner);
                continue;
            }

            AnswerEnvelope result = runner.Run(input, "console-user");
            Console.WriteLine(result.ToJson());
            Console.WriteLine(new string('-', 80));
        }
    }





    private static void RunTrainingMode(QPipelineRunner runner)
    {
        Console.WriteLine("\n--- Training Mode Activated ---");
        var trainingInputs = new[]
        {
            "Define retention policy for critical logs.",
            "How does the system detect anomalies?",
            "Explain audit trail generation.",
            "What is the max buffer size?",
            "Describe provenance mapping strategy."
        };

        foreach (var input in trainingInputs)
        {
            AnswerEnvelope result = runner.Run(input, "training-mode");
            Console.WriteLine($"[TRAINING] {result.Answer}");
        }

        Console.WriteLine("--- Training Complete ---\n");
    }
}