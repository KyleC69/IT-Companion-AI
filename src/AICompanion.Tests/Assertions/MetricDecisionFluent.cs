// Project Name: AICompanion.Tests
// File Name: MetricDecisionFluent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace AICompanion.Tests.Assertions;


public class MetricDecisionFluent
{
    private readonly MetricDecision _actual;





    public MetricDecisionFluent(MetricDecision actual)
    {
        this._actual = actual;
    }





    public MetricDecisionFluent MetricKey(string expected)
    {
        MetricDecisionAssertions.AssertCore(this._actual, expected);
        return this;
    }





    public MetricDecisionFluent Window(DateTimeOffset start, DateTimeOffset end)
    {
        MetricDecisionAssertions.AssertWindow(this._actual, start, end);
        return this;
    }





    public MetricDecisionFluent Scoring(double score, bool alert)
    {
        MetricDecisionAssertions.AssertScoring(this._actual, score, alert);
        return this;
    }
}