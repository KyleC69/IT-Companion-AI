// Project Name: LightweightAI.Core
// File Name: RulesEngine.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public class RulesEngine
{
    private readonly List<IRule> _rules = new();





    public void AddRule(IRule rule)
    {
        this._rules.Add(rule);
    }





    public IEnumerable<RuleResult> Evaluate(EventContext context)
    {
        foreach (IRule rule in this._rules)
            yield return rule.Evaluate(context);
    }
}