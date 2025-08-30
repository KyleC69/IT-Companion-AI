// Project Name: LightweightAI.Core
// File Name: SourceOrdering.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Analyzers;


public sealed class SourceOrdering
{
    private readonly Dictionary<string, long> _order = new(StringComparer.OrdinalIgnoreCase);





    public SourceOrdering(IEnumerable<string> orderedSources)
    {
        long i = 0;
        foreach (var s in orderedSources) this._order[s] = i++;
    }





    public long OrderOf(string sourceId)
    {
        return this._order.TryGetValue(sourceId, out var n) ? n : long.MaxValue;
    }
}