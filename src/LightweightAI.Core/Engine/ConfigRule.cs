// Project Name: LightweightAI.Core
// File Name: ConfigRule.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Text.Json;


namespace LightweightAI.Core.Engine;


public class ConfigRule : IRule
{
    public string RuleName { get; init; } = string.Empty;
    public int EventId { get; init; }
    public double Score { get; init; } = 0.0;





    public RuleResult Evaluate(EventContext context)
    {
        var match = context.EventId == this.EventId;
        return new RuleResult(this.RuleName, match, match ? this.Score : 0.0);
    }





    public static IEnumerable<IRule> LoadFromJson(string json)
    {
        List<ConfigRule>? rules = JsonSerializer.Deserialize<List<ConfigRule>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return rules as IEnumerable<IRule> ?? [];
    }
}