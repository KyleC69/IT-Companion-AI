// Project Name: LightweightAI.Core
// File Name: MetricRefinery.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.DataRefineries;


public sealed class MetricRefinery
{
    public IReadOnlyList<MetricSample> Process(IReadOnlyList<MetricSample> raw)
    {
        if (raw == null || raw.Count == 0)
            throw new ArgumentException("Metric window is empty");

        // Validate EventId for *each* sample
        foreach (MetricSample sample in raw) EventIdValidator.Enforce(sample.EventId);

        // Optional: normalize timestamps, scrub NaNs, clamp values, etc.
        return raw;
    }
}