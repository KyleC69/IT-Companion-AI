// Project Name: LightweightAI.Core
// File Name: IFusionPipeline.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine;
using LightweightAI.Core.Engine.Fusion;


namespace LightweightAI.Core.Interfaces;


public interface IFusionPipeline
{
    IEnumerable<DecisionOutput> Process(IEnumerable<DecisionInput> inputs, FusionConfig cfg);
}