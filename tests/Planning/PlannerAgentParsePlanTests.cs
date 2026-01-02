using System;

using ITCompanionAI.AgentFramework.Planning;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanionTests.Planning;

[TestClass]
public sealed class PlannerAgentParsePlanTests
{
    [TestMethod]
    public void ParsePlan_ValidJson_ReturnsTargets()
    {
        var raw = "{\"targets\":[{\"uri\":\"https://learn.microsoft.com/en-us/semantic-kernel/\",\"sourceLabel\":\"Web\",\"category\":\"Planner\",\"version\":\"v1\"}]}";

        var plan = PlannerAgent.ParsePlanForTests("goal", raw);

        Assert.AreEqual("goal", plan.Goal);
        Assert.AreEqual(1, plan.Targets.Count);
        Assert.AreEqual(new Uri("https://learn.microsoft.com/en-us/semantic-kernel/"), plan.Targets[0].Uri);
        Assert.AreEqual("Web", plan.Targets[0].SourceLabel);
        Assert.AreEqual("Planner", plan.Targets[0].Category);
        Assert.AreEqual("v1", plan.Targets[0].Version);
    }

    [TestMethod]
    public void ParsePlan_JsonPrefixedByProse_ExtractsJsonAndParses()
    {
        var raw = "Remember, each URI should be unique.\n{\n  \"targets\": [\n    {\n      \"uri\": \"https://learn.microsoft.com/en-us/semantic-kernel/\"\n    }\n  ]\n}";

        var plan = PlannerAgent.ParsePlanForTests("goal", raw);

        Assert.AreEqual(1, plan.Targets.Count);
        Assert.AreEqual(new Uri("https://learn.microsoft.com/en-us/semantic-kernel/"), plan.Targets[0].Uri);
        Assert.AreEqual("Web", plan.Targets[0].SourceLabel);
    }

    [TestMethod]
    public void ParsePlan_JsonWrappedInCodeFence_ExtractsJsonAndParses()
    {
        var raw = "```json\n{\"targets\":[{\"uri\":\"https://learn.microsoft.com/en-us/semantic-kernel/\"}]}\n```";

        var plan = PlannerAgent.ParsePlanForTests("goal", raw);

        Assert.AreEqual(1, plan.Targets.Count);
        Assert.AreEqual(new Uri("https://learn.microsoft.com/en-us/semantic-kernel/"), plan.Targets[0].Uri);
    }

    [TestMethod]
    public void ParsePlan_InvalidJson_ThrowsInvalidOperationException()
    {
        var raw = "Not JSON at all";

        try
        {
            _ = PlannerAgent.ParsePlanForTests("goal", raw);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Expected InvalidOperationException.");
        }
        catch (InvalidOperationException ex)
        {
            StringAssert.Contains(ex.Message, "Planner returned invalid JSON");
            StringAssert.Contains(ex.Message, raw);
        }
    }

    [TestMethod]
    public void ParsePlan_EmptyTargets_ThrowsInvalidOperationException()
    {
        var raw = "{\"targets\":[]}";

        try
        {
            _ = PlannerAgent.ParsePlanForTests("goal", raw);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Expected InvalidOperationException.");
        }
        catch (InvalidOperationException ex)
        {
            StringAssert.Contains(ex.Message, "Planner returned no targets");
        }
    }
}
