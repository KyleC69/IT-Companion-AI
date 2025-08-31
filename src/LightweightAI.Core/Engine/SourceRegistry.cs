// Project Name: LightweightAI.Core
// File Name: SourceRegistry.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


internal sealed class SourceRegistry : ISourceRegistry
{
    private readonly List<SourceExecutionPlan> _plans = new();





    public void Register(string sourceKey, ISourceLoader loader, IReadOnlyDictionary<string, string>? parameters = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceKey);
        this._plans.Add(new SourceExecutionPlan(sourceKey, loader, parameters));
    }





    public IEnumerable<SourceExecutionPlan> Plans => this._plans;
}