namespace ITCompanionAI.AgentFramework.Models;


/// <summary>
///     Represents a chat model configuration used to build a Semantic Kernel instance.
/// </summary>
public sealed record CustomModelDefinition(
    string Id,
    string Connection,
    string Api,
    IDictionary<string, object> Options)
{
    /// <summary>
    ///     Validates the model definition and throws when required fields are missing or invalid.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when any required field is missing or invalid.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            throw new ArgumentException("Id is required.", nameof(Id));
        }

        if (string.IsNullOrWhiteSpace(Connection))
        {
            throw new ArgumentException("Connection is required.", nameof(Connection));
        }

        if (string.IsNullOrWhiteSpace(Api))
        {
            throw new ArgumentException("Api (model identifier) is required.", nameof(Api));
        }

        ArgumentNullException.ThrowIfNull(Options);
    }








    /// <summary>
    ///     Retrieves a typed option value or the provided default when the option is absent or cannot be cast.
    /// </summary>
    public T GetOptionOrDefault<T>(string key, T defaultValue)
    {
        if (Options.TryGetValue(key, out var value) && value is T typed)
        {
            return typed;
        }

        try
        {
            if (Options.TryGetValue(key, out value) && value is not null)
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }
        catch
        {
            // Ignore conversion failures and fall back to default.
        }

        return defaultValue;
    }
}





/// <summary>
///     Represents a routed agent message for logging or UI display.
/// </summary>
public sealed record AgentTranscript(string AgentName, string Role, string Content, DateTimeOffset TimestampUtc)
{
    /// <summary>
    ///     Creates a transcript entry for a user message.
    /// </summary>
    public static AgentTranscript FromUser(string content)
    {
        return new AgentTranscript("user", "user", content, DateTimeOffset.UtcNow);
    }








    /// <summary>
    ///     Creates a transcript entry for a system or agent message.
    /// </summary>
    public static AgentTranscript FromAgent(string agentName, string role, string content)
    {
        return new AgentTranscript(agentName, role, content, DateTimeOffset.UtcNow);
    }
}





internal interface ISpecialistAgent
{
    string SpecialistName { get; }








    Task<SpecialistResult> ExecuteAsync(string userMessage,
        string task,
        CancellationToken cancellationToken,
        Action<AgentTranscript> logCallback);
}





/// <summary>
///     Represents the output from a specialist model.
/// </summary>
internal sealed record SpecialistResult(string SpecialistName, string Task, string Findings);