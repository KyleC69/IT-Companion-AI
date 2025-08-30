// Project Name: LightweightAI.Core
// File Name: ISourceLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Abstractions;


public interface ISourceLoader
{
    /// <summary>Stream normalized raw events for a given source.</summary>
    IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request, CancellationToken ct = default);
}