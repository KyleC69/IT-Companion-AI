// Project Name: LightweightAI.Core
// File Name: PerfCounterKey.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Windows;


public readonly struct PerfCounterKey(string category, string? instance, string counterName)
    : IEquatable<PerfCounterKey>
{
    public string Category { get; } = category;
    public string Instance { get; } = instance ?? string.Empty;
    public string CounterName { get; } = counterName;





    public override string ToString()
    {
        return
            $@"\{this.Category}{(string.IsNullOrEmpty(this.Instance) ? "" : $"({this.Instance})")}\{this.CounterName}";
    }





    public bool Equals(PerfCounterKey other)
    {
        return string.Equals(this.Category, other.Category, StringComparison.OrdinalIgnoreCase)
               && string.Equals(this.Instance, other.Instance, StringComparison.Ordinal)
               && string.Equals(this.CounterName, other.CounterName, StringComparison.OrdinalIgnoreCase);
    }





    public override bool Equals(object? obj)
    {
        return obj is PerfCounterKey k && Equals(k);
    }





    public override int GetHashCode()
    {
        unchecked
        {
            var h = 17;
            h = h * 23 + StringComparer.OrdinalIgnoreCase.GetHashCode(this.Category);
            h = h * 23 + this.Instance.GetHashCode();
            h = h * 23 + StringComparer.OrdinalIgnoreCase.GetHashCode(this.CounterName);
            return h;
        }
    }
}