// Project Name: AICompanion.Tests
// File Name: DecisionAssertions.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace AICompanion.Tests.Assertions;


public static class DecisionAssertions
{
    public static void AssertMetricDecision(
        MetricDecision actual,
        string expectedKey,
        DateTimeOffset expectedStart,
        DateTimeOffset expectedEnd,
        double expectedScore,
        bool expectedAlert)
    {
        Assert.NotNull(actual);
        Assert.Equal(expectedKey, actual.MetricKey);
        Assert.Equal(expectedStart, actual.MetricWindowStart);
        Assert.Equal(expectedEnd, actual.MetricWindowEnd);
        Assert.Equal(expectedScore, actual.Score);
        Assert.Equal(expectedAlert, actual.IsAlert);
        Assert.NotNull(actual.Payload);
    }





    public static void AssertProvenancedDecision(
        ProvenancedDecision actual,
        string expectedFusionSig,
        string expectedModelId,
        string expectedModelVersion)
    {
        Assert.NotNull(actual);
        Assert.Equal(expectedFusionSig, actual.FusionSignature);
        Assert.Equal(expectedModelId, actual.ModelId);
        Assert.Equal(expectedModelVersion, actual.ModelVersion);
        Assert.NotEqual(000, actual.EventId);
        Assert.True(actual.Timestamp <= DateTime.UtcNow &&
                    actual.Timestamp > DateTime.UtcNow.AddMinutes(-1));
    }
}