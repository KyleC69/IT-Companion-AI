// Project Name: LightweightAI.Core
// File Name: ExampleRule.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public class ExampleRule : IRule
{
    public RuleResult Evaluate(EventContext context)
    {
        var match = context.EventId == 4625;
        return new RuleResult(
            nameof(ExampleRule),
            match,
            match ? 0.8 : 0.1
        );
    }
}