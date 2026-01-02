using System;
using System.Collections.Generic;
using System.Text;

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

