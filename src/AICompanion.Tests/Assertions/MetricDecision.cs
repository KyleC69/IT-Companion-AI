// Project Name: AICompanion.Tests
// File Name: MetricDecision.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace AICompanion.Tests.Assertions;


// ======= SAMPLE DOMAIN TYPES (replace with your real ones) =======
public class MetricDecision
{
    public string MetricKey { get; set; }
    public DateTimeOffset MetricWindowStart { get; set; }
    public DateTimeOffset MetricWindowEnd { get; set; }
    public double Score { get; set; }
    public bool IsAlert { get; set; }
    public object Payload { get; set; }
}