// Project Name: LightweightAI.Core
// File Name: RuleResult.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Interfaces;


namespace LightweightAI.Core.Engine;


/// <summary>
///     Result of evaluating a single <see cref="IRule" /> against an <see cref="EventContext" />,
///     capturing its name, whether it matched, and the score contribution (often weight or risk).
/// </summary>
public record RuleResult(string RuleName, bool IsMatch, double Score);