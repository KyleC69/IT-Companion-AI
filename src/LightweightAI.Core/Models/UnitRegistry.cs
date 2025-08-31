// Project Name: LightweightAI.Core
// File Name: UnitRegistry.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


internal static class UnitRegistry
{
    // Priority: (Category, Counter) -> Counter -> default
    private static readonly Dictionary<(string category, string counter), string> MapByCatCtr =
        new(StringTupleComparer.OrdinalIgnoreCase)
        {
            { ("Network Interface", "Bytes Total/sec"), "bytes/sec" },
            { ("Network Interface", "Packets/sec"), "packets/sec" },
            { ("Processor", "% Processor Time"), "%" },
            { ("Processor Information", "% Processor Time"), "%" },
            { ("LogicalDisk", "Disk Transfers/sec"), "ops/sec" },
            { ("LogicalDisk", "Disk Bytes/sec"), "bytes/sec" },
            { ("PhysicalDisk", "Disk Transfers/sec"), "ops/sec" },
            { ("PhysicalDisk", "Disk Bytes/sec"), "bytes/sec" },
            { ("Memory", "Available MBytes"), "MB" },
            { ("Process", "Private Bytes"), "bytes" },
            { ("Process", "Working Set"), "bytes" },
            { ("Process", "IO Read Bytes/sec"), "bytes/sec" },
            { ("Process", "IO Write Bytes/sec"), "bytes/sec" }
        };

    private static readonly Dictionary<string, string> MapByCounter =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "% Processor Time", "%" },
            { "% Privileged Time", "%" },
            { "% User Time", "%" },
            { "Disk Transfers/sec", "ops/sec" },
            { "Disk Reads/sec", "ops/sec" },
            { "Disk Writes/sec", "ops/sec" },
            { "Disk Bytes/sec", "bytes/sec" },
            { "Disk Read Bytes/sec", "bytes/sec" },
            { "Disk Write Bytes/sec", "bytes/sec" },
            { "Current Disk Queue Length", "count" },
            { "Avg. Disk sec/Transfer", "sec/op" },
            { "Available MBytes", "MB" },
            { "Bytes Total/sec", "bytes/sec" },
            { "Packets/sec", "packets/sec" },
            { "Private Bytes", "bytes" },
            { "Working Set", "bytes" },
            { "IO Read Bytes/sec", "bytes/sec" },
            { "IO Write Bytes/sec", "bytes/sec" }
        };





    public static string? ResolveUnit(string category, string counter)
    {
        if (MapByCatCtr.TryGetValue((category, counter), out var u))
            return u;
        if (MapByCounter.TryGetValue(counter, out var u2))
            return u2;
        return null;
    }





    private sealed class StringTupleComparer : IEqualityComparer<(string a, string b)>
    {
        public static readonly StringTupleComparer OrdinalIgnoreCase = new();





        public bool Equals((string a, string b) x, (string a, string b) y)
        {
            return string.Equals(x.a, y.a, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(x.b, y.b, StringComparison.OrdinalIgnoreCase);
        }





        public int GetHashCode((string a, string b) obj)
        {
            unchecked
            {
                var h = 17;
                h = h * 23 + StringComparer.OrdinalIgnoreCase.GetHashCode(obj.a);
                h = h * 23 + StringComparer.OrdinalIgnoreCase.GetHashCode(obj.b);
                return h;
            }
        }
    }
}