// Project Name: LightweightAI.Core
// File Name: ISourceLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Interfaces;


public interface ISourceLoader
{
    /// <summary>Stream normalized raw events for a given source.</summary>
    IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request, CancellationToken ct = default);
}