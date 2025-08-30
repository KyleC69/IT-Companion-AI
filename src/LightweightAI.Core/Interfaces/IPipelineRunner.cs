// Project Name: LightweightAI.Core
// File Name: IPipelineRunner.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine;


namespace LightweightAI.Core.Interfaces;


/// <summary>
///     Contract for driving the end‑to‑end intake pipeline (source load -> normalize -> encode -> detect -> fuse ->
///     dispatch).
/// </summary>
public interface IPipelineRunner
{
    Task RunAsync(IEnumerable<SourceExecutionPlan> plans, CancellationToken ct = default);
}