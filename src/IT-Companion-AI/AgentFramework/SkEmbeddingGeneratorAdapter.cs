using Microsoft.Extensions.AI;

namespace SkAgentGroup.AgentFramework;

public sealed class SkEmbeddingGeneratorAdapter
    : IEmbeddingGenerator<string, Embedding<float>>
{
    private readonly MultiAgentEmbeddingGenerator _inner;
    private readonly string _agentId;
    private readonly string _agentName;
    private readonly AgentEmbeddingOptions _defaultOptions;

    public SkEmbeddingGeneratorAdapter(
        MultiAgentEmbeddingGenerator inner,
        string agentId,
        string agentName,
        AgentEmbeddingOptions? defaultOptions = null)
    {
        _inner = inner;
        _agentId = agentId;
        _agentName = agentName;
        _defaultOptions = defaultOptions ?? new AgentEmbeddingOptions
        {
            AgentId = agentId,
            AgentName = agentName
        };
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(IEnumerable<string> values, EmbeddingGenerationOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Embedding<float>> GenerateEmbeddingAsync(
        string text,
        EmbeddingGenerationOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        // Bridge from SK’s generic options to your richer agent options if needed
        var agentRequest = new AgentEmbeddingRequest
        {
            Text = text,
            AgentId = _agentId,
            AgentName = _agentName,
            Options = _defaultOptions
        };

        return await _inner.GenerateAsync(agentRequest, cancellationToken);
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        throw new NotImplementedException();
    }
}
