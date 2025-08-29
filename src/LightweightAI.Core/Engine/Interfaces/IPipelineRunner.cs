// Project Name: LightweightAI.Core
// File Name: IPipelineRunner.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers

namespace LightweightAI.Core.Engine;

/// <summary>
/// Contract for driving the end‑to‑end intake pipeline (source load -> normalize -> encode -> detect -> fuse -> dispatch).
/// </summary>
public interface IPipelineRunner
{
    Task RunAsync(IEnumerable<SourceExecutionPlan> plans, CancellationToken ct = default);
}

/// <summary>
/// Describes a source execution request tying an <see cref="Abstractions.ISourceLoader"/> implementation
/// to a logical source key and optional parameter map.
/// </summary>
public sealed record SourceExecutionPlan(string SourceKey, Abstractions.ISourceLoader Loader, IReadOnlyDictionary<string,string>? Parameters = null);