// Project Name: LightweightAI.Core
// File Name: Pipeline.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public class Pipeline
{
    private readonly IPipelineRunner _runner;





    public Pipeline(IPipelineRunner runner)
    {
        this._runner = runner ?? throw new ArgumentNullException(nameof(runner));
    }





    public void Execute()
    {
        this._runner.Initialize();
        this._runner.Run();
        this._runner.Teardown();
    }
}