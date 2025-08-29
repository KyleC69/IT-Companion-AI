// Project Name: LightweightAI.Core
// File Name: ConfigRule.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;

/// <summary>
/// Declarative rule definition loaded from JSON configuration. Matches events by EventId
/// and contributes a fixed score when matched. Enables quick, non-compiled rule updates.
/// </summary>
public class ConfigRule : IRule
{
    /// <summary>Logical name for identification / reporting.</summary>
    public string RuleName { get; init; } = string.Empty;
    /// <summary>Target event identifier to match.</summary>
    public int EventId { get; init; }
    /// <summary>Score contributed when the rule matches.</summary>
    public double Score { get; init; } = 0.0;





    /// <inheritdoc />
    public RuleResult Evaluate(EventContext context)
    {
        var match = context.EventId == this.EventId;
        return new RuleResult(this.RuleName, match, match ? this.Score : 0.0);
    }





    /// <summary>
    /// Deserializes a JSON array of rule objects into <see cref="IRule"/> instances.
    /// </summary>
    public static IEnumerable<IRule> LoadFromJson(string json)
    {
        List<ConfigRule>? rules = System.Text.Json.JsonSerializer.Deserialize<List<ConfigRule>>(json,
            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return rules as IEnumerable<IRule> ?? [];
    }
}