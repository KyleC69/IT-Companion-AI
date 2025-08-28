// Project Name: AICompanion.Tests
// File Name: BufferGuards.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace AICompanion.Tests.Assertions;


public static class BufferGuards
{
    /// <summary>
    ///     Validates that the provided buffer meets the expected size constraints.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the buffer.</typeparam>
    /// <param name="buffer">The buffer to validate. Must not be <c>null</c>.</param>
    /// <param name="minExpectedCount">The minimum expected number of elements in the buffer.</param>
    /// <param name="maxExpectedCount">The maximum expected number of elements in the buffer.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="buffer" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the number of elements in <paramref name="buffer" /> is less than <paramref name="minExpectedCount" />
    ///     or greater than <paramref name="maxExpectedCount" />.
    /// </exception>
    public static void AssertHealthy<T>(
        IReadOnlyCollection<T> buffer,
        int minExpectedCount,
        int maxExpectedCount)
    {
        Assert.NotNull(buffer);
        Assert.True(buffer.Count >= minExpectedCount,
            $"Buffer underflow: expected ≥ {minExpectedCount}, found {buffer.Count}");
        Assert.True(buffer.Count <= maxExpectedCount,
            $"Buffer overflow: expected ≤ {maxExpectedCount}, found {buffer.Count}");
    }
}