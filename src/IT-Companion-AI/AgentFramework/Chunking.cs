// ============================================================================
// CHUNKING (Microsoft.ML.Tokenizers-based)
// ============================================================================



namespace ITCompanionAI.AgentFramework;


public sealed record Chunk(
    int Index,
    string Text,
    int TokenCount,
    string Section = null,
    string Symbol = null,
    string Kind = null
);





public interface IChunker
{
    IReadOnlyList<Chunk> Chunk(string text, string section = null);
}





/// <summary>
///     Tokenizer-aware chunker using a BPE tokenizer compatible with bge-small-en and phi-2 style models.
/// </summary>
public sealed class TokenizerChunker : IChunker
{
    private readonly int _maxTokens;
    private readonly HFTokenizer.Tokenizer _tokenizer;








    public TokenizerChunker(HFTokenizer.Tokenizer tokenizer, int maxTokens = 512)
    {
        _tokenizer = tokenizer;
        if (maxTokens <= 0) throw new ArgumentOutOfRangeException(nameof(maxTokens));

        _maxTokens = maxTokens;
    }








    public IReadOnlyList<Chunk> Chunk(string text, string section = null)
    {
        if (string.IsNullOrWhiteSpace(text)) return Array.Empty<Chunk>();

        // Encode the text using the tokenizer
        var ids = _tokenizer.Encode(text, false) as IReadOnlyList<int> ?? Array.Empty<int>();
        // If no tokens are generated, return an empty list
        if (ids.Count == 0) return Array.Empty<Chunk>();

        var total = ids.Count;
        var chunkCount = (total + _maxTokens - 1) / _maxTokens;
        List<Chunk> chunks = new(chunkCount);
        for (int index = 0, start = 0; start < total; index++, start += _maxTokens)
        {
            var count = Math.Min(_maxTokens, total - start);
            // Extract the subset of token IDs for the current chunk
            var subIds = ids.Skip(start).Take(count).Select(id => (uint)id).ToList();
            // Decode the subset of token IDs back into text
            var subText = _tokenizer.Decode(subIds, true);
            // Add the chunk to the list
            chunks.Add(new Chunk(
                index,
                subText,
                count,
                section
            ));
        }

        return chunks;
    }
}