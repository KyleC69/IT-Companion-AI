// Project Name: LightweightAI.Core
// File Name: RuleResult.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public record RuleResult(string RuleName, bool IsMatch, double Score);