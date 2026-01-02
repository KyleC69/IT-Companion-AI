using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.SemanticKernel.Agents;

namespace ITCompanionAI.AgentFramework;

/// <summary>
/// Provides model definitions used by the multi-agent chat orchestration layer.
/// </summary>
public static class ModelCatalog
{

    

#pragma warning disable SKEXP0110
    public static ModelDefinition Conversationalist  => new() { Api="gpt-4o", Id="conversationalist" };
#pragma warning restore SKEXP0110



    /*/// <summary>
    /// Lightweight planner and router that coordinates the specialist agents.
    /// </summary>
    public static ModelDefinition ManagerModel => new(
      Id: "Manager-1",
      Connection: "openai",
      Api: "microsoft/Phi-3",
      Options: new Dictionary<string, object?>
      {
          ["temperature"] = 0.15,
          ["max_tokens"] = 2048
      });








/// <summary>
/// Specialist focused on infrastructure and operations topics.
/// </summary>
public static ModelDefinition InfraSpecialistModel => new(
        Id: "InfraSpecialist",
        Connection: "openai",
        Api: "deepseek/DeepSeek-R1",
        Options: new Dictionary<string, object?>
        {
            ["temperature"] = 0.15,
            ["max_tokens"] = 2048
        });

    /// <summary>
    /// Specialist focused on coding and scripting guidance.
    /// </summary>
    public static ModelDefinition CodingSpecialistModel => new(
        Id: "CodingSpecialist",
        Connection: "openai",
        Api: "deepseek/DeepSeek-R1",
        Options: new Dictionary<string, object?>
        {
            ["temperature"] = 0.25,
            ["max_tokens"] = 2048
        });

    /// <summary>
    /// Specialist focused on general knowledge and troubleshooting steps.
    /// </summary>
    public static ModelDefinition GeneralSpecialistModel => new(
        Id: "GeneralSpecialist",
        Connection: "openai",
        Api: "deepseek/DeepSeek-R1",
        Options: new Dictionary<string, object?>
        {
            ["temperature"] = 0.3,
            ["max_tokens"] = 2048
        });
    */
    /// <summary>
    /// All model definitions used across the agent system.
    /// </summary>
#pragma warning disable SKEXP0110
    public static IReadOnlyList<ModelDefinition> AllModels => new[]
#pragma warning restore SKEXP0110
    {
        Conversationalist
    };

    
}