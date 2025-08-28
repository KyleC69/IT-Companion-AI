// Project Name: AICompanion.Tests
// File Name: FusionBrokerTest.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections.Immutable;

using AICompanion.Tests.Assertions;

using LightweightAI.Core.Engine;

using Moq;

using MetricDecision = AICompanion.Tests.Assertions.MetricDecision;


namespace AICompanion.Tests.Engine;


public class FusionBrokerTest
{
    [Fact]
    public void Constructor_ShouldInitializeDependencies()
    {
        // Arrange
        Mock<ISeverityMapper> severityMapperMock = new();
        Mock<IProvenanceLogger> provenanceLoggerMock = new();
        Mock<IRunner> runnerMock = new();

        // Act
        var fusionBroker = new FusionBroker(severityMapperMock.Object, provenanceLoggerMock.Object, runnerMock.Object);

        // Assert
        Assert.NotNull(fusionBroker);
    }





    [Fact]
    public void Pipeline_Should_Produce_Valid_Decisions()
    {
        List<ProvenancedDecision> buffer = new();
        {
            ImmutableDictionary<string, double> metricPayload = ImmutableDictionary<string, double>.Empty
                .Add("dim1", 123)
                .Add("dim2", 456);

            var provenancedDecision = new ProvenancedDecision
            {
                Metrics = new LightweightAI.Core.Engine.MetricDecision(
                    "CPU.Load",
                    DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow,
                    0.92,
                    true,
                    metricPayload
                ),
                ModelId = "model123",
                ModelVersion = "1.0",
                Timestamp = DateTime.UtcNow,
                FusionSignature = "hash123"
            };
            buffer.Add(provenancedDecision);
        }
        ;

        buffer.ShouldBeHealthy(1, 100);

        var metric = new MetricDecision
        {
            MetricKey = "CPU.Load",
            MetricWindowStart = DateTimeOffset.UtcNow,
            MetricWindowEnd = DateTimeOffset.UtcNow,
            Score = 0.92,
            IsAlert = true,
            Payload = ImmutableDictionary<string, double>.Empty
                .Add("dim1", 123)
                .Add("dim2", 456)
        }; // from your pipeline
        metric.ShouldBe()
            .MetricKey("CPU.Load")
            .Window(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
            .Scoring(0.92, true);

        ProvenancedDecision decision = buffer.First();
        decision.ShouldBe()
            .BaseChecks()
            .ModelInfo("hash123", "ModelA", "1.0");
    }





    [Theory]
    [InlineData("output1", 1)]
    [InlineData("output2", 2)]
    [InlineData("UNKNOWN", 0)] // Edge case: Unknown key
    [InlineData(null, 0)] // Edge case: Null model output
    public void SeverityMapper_MapSeverity_ShouldReturnExpectedSeverity(string modelOutput, int expectedSeverity)
    {
        // Arrange
        Dictionary<string, int> severityMap = new()
        {
            { "output1", 1 },
            { "output2", 2 }
        };
        var config = new FusionBroker.ConfigSnapshot { SeverityMap = severityMap };
        var severityMapper = new FusionBroker.SeverityMapper();

        // Act
        var result = severityMapper.MapSeverity(modelOutput, config);

        // Assert
        Assert.Equal(expectedSeverity, result);
    }
}