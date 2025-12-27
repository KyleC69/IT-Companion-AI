namespace SkAgentGroup.AgentFramework;

public interface IRawEmbeddingBackend
    {
        Task<ReadOnlyMemory<float>> GenerateAsync(
            AgentEmbeddingRequest request,
            CancellationToken cancellationToken = default);
    }












