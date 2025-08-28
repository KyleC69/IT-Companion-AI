// Project Name: AICompanion.Tests
// File Name: MetricDecisionAssertions.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace AICompanion.Tests.Assertions;


public static class MetricDecisionAssertions
{
    public static void AssertCore(MetricDecision actual, string expectedKey)
    {
        Assert.NotNull(actual);
        Assert.Equal(expectedKey, actual.MetricKey);
    }





    public static void AssertWindow(MetricDecision actual, DateTimeOffset expectedStart,
        DateTimeOffset expectedEnd)
    {
        Assert.Equal(expectedStart, actual.MetricWindowStart);
        Assert.Equal(expectedEnd, actual.MetricWindowEnd);
    }





    public static void AssertScoring(MetricDecision actual, double expectedScore,
        bool expectedAlert)
    {
        Assert.Equal(expectedScore, actual.Score);
        Assert.Equal(expectedAlert, actual.IsAlert);
    }
}