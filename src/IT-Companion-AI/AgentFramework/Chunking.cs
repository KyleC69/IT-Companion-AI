using System;
using System.Collections.Generic;
using System.Text;

    using Microsoft.ML.Tokenizers;

// ============================================================================
// CHUNKING (Microsoft.ML.Tokenizers-based)
// ============================================================================

namespace SkKnowledgeBase.Chunking;


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
        _tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer));
        _maxTokens = maxTokens;
    }

    public IReadOnlyList<Chunk> Chunk(string text, string? section = null)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Array.Empty<Chunk>();
        }

        // Encode to token IDs (no out parameter)
        IReadOnlyList<int> ids = _tokenizer.EncodeToIds(text);

        if (ids.Count == 0)
        {
            return Array.Empty<Chunk>();
        }

        var chunks = new List<Chunk>();
        int index = 0;

        for (int start = 0; start < ids.Count; start += _maxTokens)
        {
            int count = Math.Min(_maxTokens, ids.Count - start);

            // Slice the token ID range
            var subIds = ids.Skip(start).Take(count).ToList();

            // Decode IDs back into text
            string subText = _tokenizer.Decode(subIds);

            chunks.Add(new Chunk(
                Index: index++,
                Text: subText,
                TokenCount: subIds.Count,
                Section: section
            ));
        }

        return chunks;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vocabPath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    internal static Func<IServiceProvider, Tokenizer> CreateTokenizer(string vocabPath,string mergesPath)
    {
        return _ =>
        {
            if (vocabPath is not string vocabFile)
            {
                throw new ArgumentException("vocabPath and mergesPath must be strings.");
            }

            if (!File.Exists(vocabFile))
            {
                throw new FileNotFoundException("Tokenizer vocab file not found", vocabFile);
            }

            return BpeTokenizer.Create(vocabFile, mergesPath);
        }
        ;
    }




}


