// Project Name: AICompanion.Tests
// File Name: ProvenancedDecisionFluent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace AICompanion.Tests.Assertions;


public class ProvenancedDecisionFluent
{
    private readonly ProvenancedDecision _actual;





    public ProvenancedDecisionFluent(ProvenancedDecision actual)
    {
        this._actual = actual;
    }





    public ProvenancedDecisionFluent BaseChecks()
    {
        ProvenanceAssertions.AssertBase(this._actual);
        return this;
    }





    public ProvenancedDecisionFluent ModelInfo(string fusionSig, string modelId, string modelVersion)
    {
        ProvenanceAssertions.AssertModelInfo(this._actual, fusionSig, modelId, modelVersion);
        return this;
    }
}