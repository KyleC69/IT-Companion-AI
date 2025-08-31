// Project Name: LightweightAI.Core
// File Name: ISourceRegistry.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine;



namespace LightweightAI.Core.Interfaces;


/// <summary>
///     Registry used to collect and enumerate configured source loaders. Provides a single
///     indirection layer so pipeline construction can remain config driven.
/// </summary>
public interface ISourceRegistry
{
    IEnumerable<SourceExecutionPlan> Plans { get; }
    void Register(string sourceKey, ISourceLoader loader, IReadOnlyDictionary<string, string>? parameters = null);
}