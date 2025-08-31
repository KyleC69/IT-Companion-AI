// Project Name: LightweightAI.Core
// File Name: Pipeline.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine.Pipeline;


/// <summary>
///     High‑level orchestration facade that runs the configured ingestion <see cref="ISourceRegistry" />
///     through the provided <see cref="IPipelineRunner" />. Legacy Initialize/Run/Teardown lifecycle
///     collapsed into a single async execution for the simplified runner design.
/// </summary>
public sealed class Pipeline(IPipelineRunner runner, ISourceRegistry sources)
{
    private readonly IPipelineRunner _runner = runner ?? throw new ArgumentNullException(nameof(runner));
    private readonly ISourceRegistry _sources = sources ?? throw new ArgumentNullException(nameof(sources));





    /// <summary>
    ///     Executes the pipeline over all registered source execution plans.
    /// </summary>
    public Task ExecuteAsync(CancellationToken ct = default)
    {
        return this._runner.RunAsync(this._sources.Plans, ct);
    }
}