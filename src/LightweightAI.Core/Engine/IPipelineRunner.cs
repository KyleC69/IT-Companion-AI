// Project Name: LightweightAI.Core
// File Name: IPipelineRunner.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public interface IPipelineRunner
{
    void Initialize();
    void Run();
    void Teardown();
}