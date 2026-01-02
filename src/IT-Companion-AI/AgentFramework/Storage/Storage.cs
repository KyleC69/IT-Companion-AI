using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;

using ITCompanionAI.AgentFramework;


// ============================================================================
// STORAGE: SqlServer + vector storage
// ============================================================================
// NOTE: Implementation now targets a local SQL Server instance.

namespace ITCompanionAI.AgentFramework.Storage;


public interface IVectorStore
{
    Task EnsureSchemaAsync(CancellationToken cancellationToken = default);

    Task<DocumentRecord> UpsertDocumentAsync(
        DocumentRecord document,
        CancellationToken cancellationToken = default);

    Task UpsertChunksAsync(
        Guid documentId,
        IReadOnlyList<ChunkRecord> chunks,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ChunkRecord>> GetChunksBySymbolAsync(
        string symbol,
        CancellationToken cancellationToken = default);

    Task UpsertReconciledChunkAsync(
        ReconciledChunkRecord chunk,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReconciledChunkRecord>> SearchReconciledAsync(
        float[] embedding,
        int topK,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ChunkRecord>> SearchRawChunksAsync(
        float[] embedding,
        int topK,
        CancellationToken cancellationToken = default);
}





/// <summary>
/// 
/// </summary>
public sealed class PgVectorStore : IVectorStore
{
    private readonly string _connectionString;
    private readonly int _embeddingDim;

    public PgVectorStore(string connectionString, int embeddingDim)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _embeddingDim = embeddingDim;
    }

    private async Task<SqlConnection> OpenConnectionAsync(CancellationToken ct)
    {
        var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync(ct).ConfigureAwait(false);
        return conn;
    }



    /// <summary>
    /// Ensures that the necessary database schema (tables and indexes) exists.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task EnsureSchemaAsync(CancellationToken cancellationToken = default)
    {
        const string documentsSql = @"
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'documents')
BEGIN
    CREATE TABLE dbo.documents (
        id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        external_id NVARCHAR(512) NOT NULL,
        source NVARCHAR(256) NOT NULL,
        title NVARCHAR(512) NOT NULL,
        version NVARCHAR(128) NULL,
        category NVARCHAR(128) NULL,
        status NVARCHAR(64) NOT NULL,
        created_at DATETIMEOFFSET NOT NULL,
        updated_at DATETIMEOFFSET NOT NULL,
        last_error NVARCHAR(MAX) NULL,
        CONSTRAINT UQ_documents_external_id UNIQUE (external_id)
    );
END";

        const string chunksSql = @"
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'chunks')
BEGIN
    CREATE TABLE dbo.chunks (
        id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        document_id UNIQUEIDENTIFIER NOT NULL,
        chunk_index INT NOT NULL,
        text NVARCHAR(MAX) NOT NULL,
        token_count INT NOT NULL,
        embedding VECTOR(384) NOT NULL,
        section NVARCHAR(256) NULL,
        symbol NVARCHAR(256) NULL,
        kind NVARCHAR(128) NULL,
        category NVARCHAR(128) NULL,
        verified BIT NOT NULL,
        confidence FLOAT NOT NULL,
        deprecated BIT NOT NULL,
        CONSTRAINT FK_chunks_documents FOREIGN KEY (document_id) REFERENCES dbo.documents(id),
        CONSTRAINT UQ_chunks_doc_chunk UNIQUE (document_id, chunk_index)
    );
END";

        const string chunksSymbolIndex = @"
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes WHERE name = 'IX_chunks_symbol' AND object_id = OBJECT_ID('dbo.chunks'))
BEGIN
    CREATE INDEX IX_chunks_symbol ON dbo.chunks(symbol);
END";

        const string reconciledSql = @"
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'reconciled_chunks')
BEGIN
    CREATE TABLE dbo.reconciled_chunks (
        id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        symbol NVARCHAR(256) NOT NULL,
        namespace NVARCHAR(256) NULL,
        version NVARCHAR(128) NULL,
        category NVARCHAR(128) NULL,
        summary NVARCHAR(MAX) NOT NULL,
        embedding VECTOR(384) NOT NULL,
        confidence FLOAT NOT NULL,
        source_count INT NOT NULL
    );
END";

        const string reconciledSymbolIndex = @"
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes WHERE name = 'IX_reconciled_symbol' AND object_id = OBJECT_ID('dbo.reconciled_chunks'))
BEGIN
    CREATE INDEX IX_reconciled_symbol ON dbo.reconciled_chunks(symbol);
END";

        await using var conn = await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        foreach (var statement in new[] { documentsSql, chunksSql, chunksSymbolIndex, reconciledSql, reconciledSymbolIndex })
        {
            await using var cmd = new SqlCommand(statement, conn);
            await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<DocumentRecord> UpsertDocumentAsync(
        DocumentRecord document,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
MERGE dbo.documents WITH (HOLDLOCK) AS target
USING (VALUES (@id, @external_id, @source, @title, @version, @status, @created_at, @updated_at, @last_error,@category))
    AS src (id, external_id, source_value, title, version, status, created_at, updated_at, last_error,category)
ON target.external_id = src.external_id
WHEN MATCHED THEN
    UPDATE SET
        source = src.source_value,
        title = src.title,
        version = src.version,
        status = src.status,
        updated_at = src.updated_at,
        last_error = src.last_error,
        category = src.category
WHEN NOT MATCHED THEN
    INSERT (id, external_id, source, title, version, status, created_at, updated_at, last_error, category)
    VALUES (src.id, src.external_id, src.source_value, src.title, src.version, src.status, src.created_at, src.updated_at, src.last_error, src.category)
OUTPUT inserted.id, inserted.external_id, inserted.source, inserted.title, inserted.version, inserted.status, inserted.created_at, inserted.updated_at, inserted.last_error, inserted.category;";

        await using var conn = await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = document.Id;
        cmd.Parameters.Add("@external_id", SqlDbType.NVarChar, 512).Value = document.ExternalId;
        cmd.Parameters.Add("@source", SqlDbType.NVarChar, 256).Value = document.Source;
        cmd.Parameters.Add("@title", SqlDbType.NVarChar, 512).Value = document.Title;
        cmd.Parameters.Add("@version", SqlDbType.NVarChar, 128).Value = (object?)document.Version ?? DBNull.Value;
        cmd.Parameters.Add("@status", SqlDbType.NVarChar, 64).Value = document.Status;
        cmd.Parameters.Add("@created_at", SqlDbType.DateTimeOffset).Value = document.CreatedAt;
        cmd.Parameters.Add("@updated_at", SqlDbType.DateTimeOffset).Value = document.UpdatedAt;
        cmd.Parameters.Add("@last_error", SqlDbType.NVarChar, -1).Value = (object?)document.LastError ?? DBNull.Value;
        cmd.Parameters.Add("@category", SqlDbType.NVarChar, 128).Value = (object?)document.Category ?? DBNull.Value;

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        return await reader.ReadAsync(cancellationToken).ConfigureAwait(false)
            ? new DocumentRecord
            {
                Id = reader.GetGuid(0),
                ExternalId = reader.GetString(1),
                Source = reader.GetString(2),
                Title = reader.GetString(3),
                Version = reader.IsDBNull(4) ? null : reader.GetString(4),
                Status = reader.GetString(5),
                CreatedAt = reader.GetFieldValue<DateTimeOffset>(6),
                UpdatedAt = reader.GetFieldValue<DateTimeOffset>(7),
                LastError = reader.IsDBNull(8) ? null : reader.GetString(8),
                Category = reader.IsDBNull(9) ? null : reader.GetString(9)
            }
            : throw new DataException("Failed to upsert document.");
    }

    public async Task UpsertChunksAsync(
        Guid documentId,
        IReadOnlyList<ChunkRecord> chunks,
        CancellationToken cancellationToken = default)
    {
        if (chunks.Count == 0)
        {
            return;
        }

        const string sql = @"
MERGE dbo.chunks WITH (HOLDLOCK) AS target
USING (VALUES (@id, @document_id, @chunk_index, @text, @token_count, @embedding, @section, @symbol, @kind, @verified, @confidence, @deprecated, @category))
    AS src (id, document_id, chunk_index, text, token_count, embedding, section, symbol, kind, verified, confidence, deprecated, category)
ON target.document_id = src.document_id AND target.chunk_index = src.chunk_index
WHEN MATCHED THEN
    UPDATE SET
        text = src.text,
        token_count = src.token_count,
        embedding = src.embedding,
        section = src.section,
        symbol = src.symbol,
        kind = src.kind,
        verified = src.verified,
        confidence = src.confidence,
        deprecated = src.deprecated,
        category = src.category
WHEN NOT MATCHED THEN
    INSERT (id, document_id, chunk_index, text, token_count, embedding, section, symbol, kind, verified, confidence, deprecated, category)
    VALUES (src.id, src.document_id, src.chunk_index, src.text, src.token_count, src.embedding, src.section, src.symbol, src.kind, src.verified, src.confidence, src.deprecated, src.category);";

        await using var conn = await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        using var tx = conn.BeginTransaction();

        foreach (var chunk in chunks)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var cmd = new SqlCommand(sql, conn, tx);
            cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = chunk.Id;
            cmd.Parameters.Add("@document_id", SqlDbType.UniqueIdentifier).Value = documentId;
            cmd.Parameters.Add("@chunk_index", SqlDbType.Int).Value = chunk.ChunkIndex;
            cmd.Parameters.Add("@text", SqlDbType.NVarChar, -1).Value = chunk.Text;
            cmd.Parameters.Add("@token_count", SqlDbType.Int).Value = chunk.TokenCount;
            cmd.Parameters.Add("@embedding", SqlDbType.Vector).Value = ToSqlVector(chunk.Embedding);
            cmd.Parameters.Add("@section", SqlDbType.NVarChar, 256).Value = (object?)chunk.Section ?? DBNull.Value;
            cmd.Parameters.Add("@symbol", SqlDbType.NVarChar, 256).Value = (object?)chunk.Symbol ?? DBNull.Value;
            cmd.Parameters.Add("@kind", SqlDbType.NVarChar, 128).Value = (object?)chunk.Kind ?? DBNull.Value;
            cmd.Parameters.Add("@verified", SqlDbType.Bit).Value = chunk.Verified;
            cmd.Parameters.Add("@confidence", SqlDbType.Float).Value = chunk.Confidence;
            cmd.Parameters.Add("@deprecated", SqlDbType.Bit).Value = chunk.Deprecated;
            cmd.Parameters.Add("@category", SqlDbType.NVarChar, 128).Value = (object?)chunk.Category ?? DBNull.Value;

            await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }

        await tx.CommitAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<ChunkRecord>> GetChunksBySymbolAsync(
        string symbol,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT id, document_id, chunk_index, text, token_count, embedding, section, symbol, kind, verified, confidence, deprecated, category
FROM dbo.chunks
WHERE symbol = @symbol;";

        var result = new List<ChunkRecord>();

        await using var conn = await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@symbol", SqlDbType.NVarChar, 256).Value = symbol;

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ChunkRecord
            {
                Id = reader.GetGuid(0),
                DocumentId = reader.GetGuid(1),
                ChunkIndex = reader.GetInt32(2),
                Text = reader.GetString(3),
                TokenCount = reader.GetInt32(4),
                Embedding = FromSqlVector(reader.GetSqlVector<float>(5)),
                Section = reader.IsDBNull(6) ? null : reader.GetString(6),
                Symbol = reader.IsDBNull(7) ? null : reader.GetString(7),
                Kind = reader.IsDBNull(8) ? null : reader.GetString(8),
                Verified = reader.GetBoolean(9),
                Confidence = reader.GetDouble(10),
                Deprecated = reader.GetBoolean(11),
                Category = reader.IsDBNull(12) ? null : reader.GetString(12)
            });
        }

        return result;
    }

    public async Task UpsertReconciledChunkAsync(
        ReconciledChunkRecord chunk,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
MERGE dbo.reconciled_chunks WITH (HOLDLOCK) AS target
USING (VALUES (@id, @symbol, @namespace, @version, @summary, @embedding, @confidence, @source_count))
    AS src (id, symbol, namespace, version, summary, embedding, confidence, source_count)
ON target.id = src.id
WHEN MATCHED THEN
    UPDATE SET
        symbol = src.symbol,
        namespace = src.namespace,
        version = src.version,
        summary = src.summary,
        embedding = src.embedding,
        confidence = src.confidence,
        source_count = src.source_count
WHEN NOT MATCHED THEN
    INSERT (id, symbol, namespace, version, summary, embedding, confidence, source_count)
    VALUES (src.id, src.symbol, src.namespace, src.version, src.summary, src.embedding, src.confidence, src.source_count);";

        await using var conn = await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = chunk.Id;
        cmd.Parameters.Add("@symbol", SqlDbType.NVarChar, 256).Value = chunk.Symbol;
        cmd.Parameters.Add("@namespace", SqlDbType.NVarChar, 256).Value = (object?)chunk.Namespace ?? DBNull.Value;
        cmd.Parameters.Add("@version", SqlDbType.NVarChar, 128).Value = (object?)chunk.Version ?? DBNull.Value;
        cmd.Parameters.Add("@summary", SqlDbType.NVarChar, -1).Value = chunk.Summary;
        cmd.Parameters.Add("@embedding", SqlDbType.Vector).Value = ToSqlVector(chunk.Embedding);
        cmd.Parameters.Add("@confidence", SqlDbType.Float).Value = chunk.Confidence;
        cmd.Parameters.Add("@source_count", SqlDbType.Int).Value = chunk.SourceCount;

        await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<ReconciledChunkRecord>> SearchReconciledAsync(
        float[] embedding,
        int topK,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT id, symbol, namespace, version, summary, embedding, confidence, source_count
FROM dbo.reconciled_chunks;";

        var allChunks = new List<ReconciledChunkRecord>();

        await using var conn = await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var cmd = new SqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            allChunks.Add(new ReconciledChunkRecord
            {
                Id = reader.GetGuid(0),
                Symbol = reader.GetString(1),
                Namespace = reader.IsDBNull(2) ? null : reader.GetString(2),
                Version = reader.IsDBNull(3) ? null : reader.GetString(3),
                Summary = reader.GetString(4),
                Embedding = FromSqlVector(reader.GetSqlVector<float>(5)),
                Confidence = reader.GetDouble(6),
                SourceCount = reader.GetInt32(7)
            });
        }

        return allChunks
            .OrderByDescending(c => CosineSimilarity(embedding, c.Embedding))
            .Take(topK <= 0 ? 0 : topK)
            .ToList();
    }

    public async Task<IReadOnlyList<ChunkRecord>> SearchRawChunksAsync(
        float[] embedding,
        int topK,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT id, document_id, chunk_index, text, token_count, embedding, section, symbol, kind, verified, confidence, deprecated
FROM dbo.chunks;";

        var chunks = new List<ChunkRecord>();

        await using var conn = await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var cmd = new SqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            chunks.Add(new ChunkRecord
            {
                Id = reader.GetGuid(0),
                DocumentId = reader.GetGuid(1),
                ChunkIndex = reader.GetInt32(2),
                Text = reader.GetString(3),
                TokenCount = reader.GetInt32(4),
                Embedding = FromSqlVector(reader.GetSqlVector<float>(5)),
                Section = reader.IsDBNull(6) ? null : reader.GetString(6),
                Symbol = reader.IsDBNull(7) ? null : reader.GetString(7),
                Kind = reader.IsDBNull(8) ? null : reader.GetString(8),
                Verified = reader.GetBoolean(9),
                Confidence = reader.GetDouble(10),
                Deprecated = reader.GetBoolean(11)
            });
        }

        return chunks
            .OrderByDescending(c => CosineSimilarity(embedding, c.Embedding))
            .Take(topK <= 0 ? 0 : topK)
            .ToList();
    }

    private SqlVector<float> ToSqlVector(float[] embedding)
    {
        return embedding.Length != _embeddingDim
            ? throw new InvalidOperationException($"Embedding dimension mismatch. Expected {_embeddingDim}, received {embedding.Length}.")
            : new SqlVector<float>(embedding);
    }

    private float[] FromSqlVector(SqlVector<float> vector)
    {
        return vector.IsNull
            ? throw new InvalidOperationException("Invalid embedding payload.")
            : vector.Length != _embeddingDim
            ? throw new InvalidOperationException($"Embedding dimension mismatch. Expected {_embeddingDim}, received {vector.Length}.")
            : vector.Memory.ToArray();
    }

    private static double CosineSimilarity(float[] a, float[] b)
    {
        if (a.Length == 0 || b.Length == 0)
        {
            return 0;
        }

        var length = Math.Min(a.Length, b.Length);
        double dot = 0;
        double normA = 0;
        double normB = 0;

        for (var i = 0; i < length; i++)
        {
            dot += a[i] * b[i];
            normA += a[i] * a[i];
            normB += b[i] * b[i];
        }

        return normA == 0 || normB == 0 ? 0 : dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
    }
}
