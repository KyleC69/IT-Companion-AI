namespace SkAgentGroup.AgentFramework;

/// <summary>
/// Represents one logical user → agent interaction, including metadata.
/// </summary>
public sealed class AgentTurnContext
{
    public required string UserInput { get; init; }
    public string? UserId { get; init; }
    public string? CorrelationId { get; init; }
    public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>();

    public CancellationToken CancellationToken { get; init; }

    public AgentTurnContext(
        string userInput,
        string? userId,
        string? correlationId,
        CancellationToken cancellationToken = default)
    {
        UserInput = userInput;
        UserId = userId;
        CorrelationId = correlationId ?? Guid.NewGuid().ToString("N");
        CancellationToken = cancellationToken;
    }

}












