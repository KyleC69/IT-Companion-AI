// Project Name: LightweightAI.Core
// File Name: DataSampler.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Training;


/// <summary>
///     Placeholder balanced sampling utility that can weight minority labels higher to
///     address class imbalance prior to training or incremental updates.
/// </summary>
public static class DataSampler
{
    public static IEnumerable<T> WeightedSample<T>(IEnumerable<T> items, Func<T, string> labelSelector, int take,
        float minorityBoost = 2f)
    {
        Dictionary<string, List<T>> groups = items.GroupBy(labelSelector).ToDictionary(g => g.Key, g => g.ToList());
        if (groups.Count == 0) yield break;
        var max = groups.Max(g => g.Value.Count);
        var rnd = new Random();
        foreach (KeyValuePair<string, List<T>> kv in groups)
        {
            var weight = kv.Value.Count == 0 ? 0 : (float)max / kv.Value.Count;
            var effective = (int)Math.Ceiling(take * Math.Min(minorityBoost, weight) / groups.Count);
            for (var i = 0; i < effective && kv.Value.Count > 0; i++)
                yield return kv.Value[rnd.Next(kv.Value.Count)];
        }
    }
}