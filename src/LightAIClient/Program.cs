// Project Name: LightAIClient
// File Name: Program.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.CommandLine;

using LightweightAI.Core.Loaders.Conversational;
using LightweightAI.Core.Models;



namespace LightAIClient;


internal class Program
{
    private static int Main(string[] args)
    {
        Console.Title = "Pipeline Trainer & Response Tester";
        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("=== Modular AI Pipeline Console ===");
        Console.WriteLine("Type 'exit' to quit, 'batch' to run test suite, or 'train' to simulate training mode.\n");


        #region "Train command and sub options"

        Command trainCommand = new("train", "Train a model with specified parameters.");
        Command statusCommand = new("status", "Check the status of the training process.");
        Command promptCommand = new("prompt", "Prompt the IT Q&A model with a test input during training.");
        Command purgeCommand = new("purge", "Purge all training data and reset the model.");

        // Model Type from LightweightAI.Core.Models
        Option<ModelType> modelOption = new("--model")
        {
            Description = "Choose the model you wish to train."
        };

        Option<DataLoaders> loaderArguments = new("--loaders")
        {
            Description = "Choose the combination of data loaders to use during this phase",
            AllowMultipleArgumentsPerToken = true
        };

        #endregion

        // Set the action for the train command.
        trainCommand.SetAction(parseResult =>
        {
            ModelType modelType = parseResult.GetValue(modelOption);
            DataLoaders loaders = parseResult.GetValue(loaderArguments);
            Console.WriteLine($"Training model: {modelType} with loaders: {loaders}");
        });






        RootCommand rootCommand = new("IT AI Companion training system");
        rootCommand.Subcommands.Add(trainCommand);
        rootCommand.Subcommands.Add(statusCommand);
        rootCommand.Subcommands.Add(promptCommand);
        rootCommand.Subcommands.Add(purgeCommand);

        // Add options to the train command.
        trainCommand.Options.Add(modelOption);
        trainCommand.Options.Add(loaderArguments);




        return rootCommand.Parse(args).Invoke();



    }






    internal static void BuildTrainerInputs(QPipelineRunner pipe)
    {
        
        
        
    }






    private static void StartPipeline()
    {
        
        
        
    }


    internal static void ReadFile(FileInfo file, int delay, ConsoleColor fgColor, bool lightMode)
    {
        Console.BackgroundColor = lightMode ? ConsoleColor.White : ConsoleColor.Black;
        Console.ForegroundColor = fgColor;
        foreach (var line in File.ReadLines(file.FullName))
        {
            Console.WriteLine(line);
            Thread.Sleep(TimeSpan.FromMilliseconds(delay * line.Length));
        }
    }








    /*

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



    */
}