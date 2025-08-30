// Project Name: LightweightAI.Core
// File Name: ProvenanceEntry.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Loaders.qANDa;


namespace LightweightAI.Core.Engine.Provenance;


public record ProvenanceEntry(
    string Stage,
    DateTime TimestampUtc,
    string Detail,
    ProvImportance Importance = ProvImportance.Important,
    object? Parameters = null
);