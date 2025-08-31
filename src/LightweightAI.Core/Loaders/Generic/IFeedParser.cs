// Project Name: LightweightAI.Core
// File Name: IFeedParser.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Generic;


public interface IFeedParser<T>
{
    bool TryParse(ReadOnlySpan<char> line, out T? obj);
    RawEvent MapToRaw(string sourceKey, in T obj);
}