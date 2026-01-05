// Project Name: SKAgent
// File Name: models.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.AgentFramework.Planning;


public sealed record IngestionTarget(
    Uri Uri,
    string SourceLabel,
    string? Category = null,
    string? Version = null
);




public sealed record IngestionPlan(
    string Goal,
    IReadOnlyList<IngestionTarget> Targets
);