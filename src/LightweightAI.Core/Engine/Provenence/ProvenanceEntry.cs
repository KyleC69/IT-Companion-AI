// Project Name: LightweightAI.Core
// File Name: ProvenanceEntry.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine.Provenence;


public record ProvenanceEntry(
    string Stage,
    DateTime TimestampUtc,
    string Detail,
    ProvImportance Importance = ProvImportance.Important,
    object? Parameters = null
);