// Project Name: LightweightAI.Core
// File Name: SourceRequest.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed record SourceRequest(
    string SourceKey,
    DateTimeOffset? SinceUtc = null,
    DateTimeOffset? UntilUtc = null,
    IReadOnlyDictionary<string, string>? Parameters = null);