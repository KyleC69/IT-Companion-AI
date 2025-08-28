// Project Name: LightweightAI.Core
// File Name: Program.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine;


namespace LightweightAI.Core;


internal class Program
{
    private static void Main(string[] args)
    {
        var pipeline = new Pipeline(new PipelineRunner());
        pipeline.Execute();
    }
}