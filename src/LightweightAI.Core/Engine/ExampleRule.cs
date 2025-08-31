// Project Name: LightweightAI.Core
// File Name: ExampleRule.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


/// <summary>
///     Demonstration rule: triggers on Windows failed logon Event ID 4625 assigning a higher
///     score when matched. Serves as a template for additional static rules.
/// </summary>
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