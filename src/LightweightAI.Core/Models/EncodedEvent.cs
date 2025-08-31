// Project Name: LightweightAI.Core
// File Name: EncodedEvent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed record EncodedEvent(
    string SourceKey,
    DateTimeOffset TimestampUtc,
    string Host,
    ReadOnlyMemory<float> Dense,
    IReadOnlyDictionary<string, int> SparseIndex // one-hot: key -> 1 index
);