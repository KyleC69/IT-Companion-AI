// Project Name: AICompanion.Tests
// File Name: ProvenanceAssertions.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace AICompanion.Tests.Assertions;


public static class ProvenanceAssertions
{
    public static void AssertBase(ProvenancedDecision actual)
    {
        Assert.NotNull(actual);
        Assert.NotEqual(0f, actual.EventId);
        Assert.InRange(actual.Timestamp, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }





    public static void AssertModelInfo(ProvenancedDecision actual, string expectedFusionSig, string expectedModelId,
        string expectedModelVersion)
    {
        Assert.Equal(expectedFusionSig, actual.FusionSignature);
        Assert.Equal(expectedModelId, actual.ModelId);
        Assert.Equal(expectedModelVersion, actual.ModelVersion);
    }
}