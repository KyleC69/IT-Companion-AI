// Project Name: AICompanion.Tests
// File Name: BufferGaurds.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace AICompanion.Tests.Engine;


public static class BufferGuards
{
    public static void AssertHealthy<T>(
        IReadOnlyCollection<T> buffer,
        int minExpectedCount,
        int maxExpectedCount)
    {
        Assert.NotNull(buffer);
        Assert.True(buffer.Count >= minExpectedCount,
            $"Buffer underflow: expected at least {minExpectedCount}, found {buffer.Count}");
        Assert.True(buffer.Count <= maxExpectedCount,
            $"Buffer overflow: expected at most {maxExpectedCount}, found {buffer.Count}");
    }
}