// Project Name: LightweightAI.Core
// File Name: IRule.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine;



namespace LightweightAI.Core.Interfaces;


public interface IRule
{
    RuleResult Evaluate(EventContext context);
}