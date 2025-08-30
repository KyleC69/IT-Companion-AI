// Project Name: LightweightAI.Core
// File Name: GenericFeedLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Abstractions;
using LightweightAI.Core.Engine;
using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Loaders.Generic;


public sealed class GenericFeedLoader<T>(ILoggerSeverity<GenericFeedLoader<T>> log, IFeedParser<T> parser)
    : ISourceLoader
{
    private readonly ILoggerSeverity<GenericFeedLoader<T>> _log = log;





    public async IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        // Expect Parameters["path"] to be local file path or FIFO
        if (request.Parameters is null || !request.Parameters.TryGetValue("path", out var path))
            throw new ArgumentException("GenericFeedLoader requires Parameters['path'].");

        using StreamReader file = File.OpenText(path);
        for (var line = await file.ReadLineAsync(); line is not null; line = await file.ReadLineAsync())
        {
            ct.ThrowIfCancellationRequested();
            if (parser.TryParse(line.AsSpan(), out T? obj) && obj is not null)
                yield return parser.MapToRaw(request.SourceKey, obj);
        }
    }
}


// Example JSON parser