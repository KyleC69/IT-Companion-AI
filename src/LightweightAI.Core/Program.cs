// Project Name: LightweightAI.Core
// File Name: Program.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core;


internal class Program
{
    private static void Main(string[] args)
    {
        var pipeline = new LightweightAI.Core.Engine.Pipeline(new LightweightAI.Core.Engine.PipelineRunner());
        pipeline.Execute();
    }
}