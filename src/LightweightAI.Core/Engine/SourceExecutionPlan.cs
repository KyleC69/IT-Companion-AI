// Project Name: LightweightAI.Core
// File Name: SourceExecutionPlan.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


/// <summary>
///     Describes a source execution request tying an <see cref="ISourceLoader" /> implementation
///     to a logical source key and optional parameter map.
/// </summary>
public sealed record SourceExecutionPlan(
    string SourceKey,
    ISourceLoader Loader,
    IReadOnlyDictionary<string, string>? Parameters = null);