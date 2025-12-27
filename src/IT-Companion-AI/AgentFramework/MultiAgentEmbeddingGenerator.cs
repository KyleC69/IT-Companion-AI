using Microsoft.Extensions.AI;

namespace SkAgentGroup.AgentFramework;




public sealed class MultiAgentEmbeddingGenerator
{
    private readonly IRawEmbeddingBackend _backend;

    public MultiAgentEmbeddingGenerator(IRawEmbeddingBackend backend)
    {
        _backend = backend;
    }

    public async Task<Embedding<float>> GenerateAsync(
        AgentEmbeddingRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Options?.LogEmbeddingRequests == true)
        {
            // Plug in your logging here if desired
            // e.g. ILogger, telemetry, etc.
        }

        var vector = await _backend.GenerateAsync(request, cancellationToken);

        return new Embedding<float>(vector);
    }
}
