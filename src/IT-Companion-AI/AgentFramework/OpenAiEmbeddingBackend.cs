using Azure;

using OpenAI;
using OpenAI.Embeddings;

namespace SkAgentGroup.AgentFramework;

public sealed class OpenAiEmbeddingBackend : IRawEmbeddingBackend
{
    private readonly OpenAIClient _client;
    private readonly string _defaultModelId;

    public OpenAiEmbeddingBackend(OpenAI.OpenAIClient client, string defaultModelId)
    {
        _client = client;
        _defaultModelId = defaultModelId;
    }

    public async Task<ReadOnlyMemory<float>> GenerateAsync(
        AgentEmbeddingRequest request,
        CancellationToken cancellationToken = default)
    {
        var modelId = request.Options?.ModelId ?? _defaultModelId;

        EmbeddingClient embeddingClient = _client.GetEmbeddingClient(modelId);

        // Generate embeddings
        var response =
            await embeddingClient.GenerateEmbeddingsAsync(
                new[] { request.Text },
                cancellationToken: cancellationToken
            );

        // Safely get the first embedding and handle possible nulls
        var embedding = response.Value.FirstOrDefault();
        if (embedding == null)
        {
            throw new InvalidOperationException("No embeddings were returned from the OpenAI service.");
        }

        // ToFloats() returns ReadOnlyMemory<float>, so assign directly
        ReadOnlyMemory<float> vector = embedding.ToFloats();

        return vector;
    }
}
