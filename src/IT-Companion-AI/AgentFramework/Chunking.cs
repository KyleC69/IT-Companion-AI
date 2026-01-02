using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.ML.Tokenizers;

using Tokenizers.HuggingFace.Tokenizer;

using Tokenizer = Tokenizers.HuggingFace.Tokenizer.Tokenizer;

// ============================================================================
// CHUNKING (Microsoft.ML.Tokenizers-based)
// ============================================================================

namespace ITCompanionAI.AgentFramework;

public sealed record Chunk(
    int Index,
    string Text,
    int TokenCount,
    string? Section = null,
    string? Symbol = null,
    string? Kind = null
);

public interface IChunker
{
    IReadOnlyList<Chunk> Chunk(string text, string? section = null);
}

/// <summary>
/// Tokenizer-aware chunker using a BPE tokenizer compatible with bge-small-en and phi-2 style models.
/// </summary>
public sealed class TokenizerChunker : IChunker
{
    private readonly Tokenizer _tokenizer;
    private readonly int _maxTokens;

    public TokenizerChunker(Tokenizer tokenizer, int maxTokens = 512)
    {
        _tokenizer = tokenizer;
        if (maxTokens <= 0) throw new ArgumentOutOfRangeException(nameof(maxTokens));
        _maxTokens = maxTokens;
    }

    public IReadOnlyList<Chunk> Chunk(string text, string? section = null)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Array.Empty<Chunk>();
        }

        IReadOnlyList<int> ids = (IReadOnlyList<int>)_tokenizer.Encode(text,false);
        if (ids.Count == 0)
        {
            return Array.Empty<Chunk>();
        }

        int total = ids.Count;
        int chunkCount = (total + _maxTokens - 1) / _maxTokens;
        var chunks = new List<Chunk>(chunkCount);

        for (int index = 0, start = 0; start < total; index++, start += _maxTokens)
        {
            int count = Math.Min(_maxTokens, total - start);

            var subIds = new List<uint>(count);
            for (int i = 0; i < count; i++)
            {
                subIds.Add((uint)ids[start + i]);
            }

            string subText = _tokenizer.Decode(subIds,true);

            chunks.Add(new Chunk(
                Index: index,
                Text: subText,
                TokenCount: count,
                Section: section
            ));
        }

        return chunks;
    }


}


