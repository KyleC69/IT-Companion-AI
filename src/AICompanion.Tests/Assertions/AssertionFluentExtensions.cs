// Project Name: AICompanion.Tests
// File Name: AssertionFluentExtensions.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace AICompanion.Tests.Assertions;


public static class AssertionFluentExtensions
{
    public static MetricDecisionFluent ShouldBe(this MetricDecision actual)
    {
        return new MetricDecisionFluent(actual);
    }





    public static ProvenancedDecisionFluent ShouldBe(this ProvenancedDecision actual)
    {
        return new ProvenancedDecisionFluent(actual);
    }





    public static IReadOnlyCollection<T> ShouldBeHealthy<T>(this IReadOnlyCollection<T> buffer, int min, int max)
    {
        BufferGuards.AssertHealthy(buffer, min, max);
        return buffer;
    }
}