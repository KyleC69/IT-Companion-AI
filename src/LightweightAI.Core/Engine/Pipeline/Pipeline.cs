// Project Name: LightweightAI.Core
// File Name: Pipeline.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers

namespace LightweightAI.Core.Engine;

/// <summary>
/// High‑level orchestration facade that runs the configured ingestion <see cref="ISourceRegistry"/>
/// through the provided <see cref="IPipelineRunner"/>. Legacy Initialize/Run/Teardown lifecycle
/// collapsed into a single async execution for the simplified runner design.
/// </summary>
public sealed class Pipeline
{
    private readonly IPipelineRunner _runner;
    private readonly ISourceRegistry _sources;

    public Pipeline(IPipelineRunner runner, ISourceRegistry sources)
    {
        _runner = runner ?? throw new ArgumentNullException(nameof(runner));
        _sources = sources ?? throw new ArgumentNullException(nameof(sources));
    }

    /// <summary>
    /// Executes the pipeline over all registered source execution plans.
    /// </summary>
    public Task ExecuteAsync(CancellationToken ct = default)
        => _runner.RunAsync(_sources.Plans, ct);
}