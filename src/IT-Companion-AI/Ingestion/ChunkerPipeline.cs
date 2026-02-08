using System.Security.Cryptography;

using Microsoft.Data.SqlTypes;

using Newtonsoft.Json;

using OllamaSharp;
using OllamaSharp.Models;




namespace ITCompanionAI.Ingestion;





public sealed class ChunkerPipeline
{
    private readonly OllamaApiClient _embedding;
    private readonly DocRepository _ragRepo;








    public ChunkerPipeline(OllamaApiClient embedding, DocRepository ragRepo)
    {
        _embedding = embedding;
        _ragRepo = ragRepo;

    }








    public async Task ProcessPageAsync(
            Guid runId,
            Guid snapshotId,
            LearnPageParseResult result,
            CancellationToken cancellationToken = default)
    {
        var codeBlocks = result.CodeBlocks;
        var sections = result.Sections;

        var codeBySection = codeBlocks
                .GroupBy(cb => cb.DocSectionId)
                .ToDictionary(g => g.Key, g => g.ToList());

        var chunks = new List<RagChunk>();

        foreach (DocSection section in sections.OrderBy(s => s.OrderIndex))
        {
            var mergedText = BuildSectionMarkdown(result.Page, section, codeBySection);

            foreach (var chunk in SplitOverflow(mergedText, 4000)
                             .Select((text, index) => new { text, index }))
            {
                var hash = ComputeHash(chunk.text);

                EmbedResponse embedding = await _embedding.EmbedAsync(chunk.text, cancellationToken);

                chunks.Add(new RagChunk
                {

                        RagRunId = runId,
                        ChunkUid = section.SemanticUid, // your stable identity
                        Kind = "learn.section",
                        Text = chunk.text,
                        MetadataJson = BuildMetadataJson(result.Page, section),
                        ContentType = "markdown",
                        ChunkHash = hash,
                        Embedding = embedding.Embeddings?.Count > 0
                                ? new SqlVector<float>(embedding.Embeddings[0])
                                : null
                });
            }
        }

        await _ragRepo.InsertChunksAsync(chunks, cancellationToken);
    }








    private static string BuildSectionMarkdown(
            DocPage p,
            DocSection section,
            IReadOnlyDictionary<Guid, List<CodeBlock>> codeBySection)
    {
        StringBuilder sb = new();

        if (!string.IsNullOrEmpty(section.Heading))
        {
            var level = section.Level ?? 2;
            sb.Append(new string('#', level))
                    .Append(' ')
                    .AppendLine(section.Heading)
                    .AppendLine();
        }

        if (!string.IsNullOrEmpty(section.ContentMarkdown))
        {
            sb.AppendLine(section.ContentMarkdown);
        }

        if (codeBySection.TryGetValue(section.Id, out var blocks) && blocks.Count > 0)
        {
            sb.AppendLine();
            foreach (CodeBlock cb in blocks)
                sb.Append("```")
                        .AppendLine(cb.Language ?? string.Empty)
                        .AppendLine(cb.Content ?? string.Empty)
                        .AppendLine("```")
                        .AppendLine();
        }

        return sb.ToString().Trim();
    }








    private static IEnumerable<string> SplitOverflow(string text, int maxCharsPerChunk)
    {
        if (text.Length <= maxCharsPerChunk)
        {
            yield return text;
            yield break;
        }

        var paragraphs = text.Split(
                new[] { "\r\n\r\n", "\n\n" },
                StringSplitOptions.RemoveEmptyEntries);

        StringBuilder sb = new();

        foreach (var para in paragraphs)
        {
            if (sb.Length + para.Length + 4 > maxCharsPerChunk && sb.Length > 0)
            {
                yield return sb.ToString();
                sb.Clear();
            }

            if (sb.Length > 0)
            {
                sb.AppendLine().AppendLine();
            }

            sb.Append(para);
        }

        if (sb.Length > 0)
        {
            yield return sb.ToString();
        }
    }








    private static string ComputeHash(string text)
    {
        using SHA256 sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(text);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }








    private static string BuildMetadataJson(DocPage page, DocSection section)
    {
        var meta = new
        {
                source = "microsoft-learn",
                pageSemanticUid = page.SemanticUid,
                pageTitle = page.Title,
                pageUrl = page.Url,
                sectionHeading = section.Heading,
                sectionLevel = section.Level,
                sectionOrder = section.OrderIndex
        };

        return JsonConvert.SerializeObject(meta);
    }
}