using System.Data;

using ITCompanionAI.Ingestion.Docs;

using Microsoft.Data.SqlClient;





/// <summary>
///     Provides methods for inserting documentation pages, sections, and code blocks into the database as part of a
///     documentation ingestion process.
/// </summary>
/// <remarks>
///     This class is intended for use in scenarios where documentation content needs to be persisted in a
///     SQL Server database. It encapsulates transactional operations to ensure that related entities (pages, sections, and
///     code blocks) are inserted atomically. Instances of this class are immutable and thread-safe for concurrent use,
///     provided that the supplied connection string is valid and the database is accessible.
/// </remarks>
public sealed class DocRepository
{
    private readonly string _connectionString;








    public DocRepository(string connectionString)
    {
        _connectionString = connectionString;
    }








    /// <summary>
    ///     Asynchronously inserts a documentation page along with its associated sections and code blocks into the database
    ///     as a single transaction.
    /// </summary>
    /// <remarks>
    ///     If any part of the insert operation fails, all changes are rolled back to maintain data
    ///     consistency. This method is not thread-safe and should not be called concurrently on the same
    ///     instance.
    /// </remarks>
    /// <param name="page">The documentation page to insert. Cannot be null.</param>
    /// <param name="sections">
    ///     The collection of sections to associate with the page. Each section will be inserted as part of the transaction.
    ///     Cannot be null.
    /// </param>
    /// <param name="codeBlocks">
    ///     The collection of code blocks to associate with the page. Each code block will be inserted as part of the
    ///     transaction. Cannot be null.
    /// </param>
    /// <returns>A task that represents the asynchronous insert operation.</returns>
    public async Task InsertPageAsync(DocPage page, IEnumerable<DocSection> sections, IEnumerable<CodeBlock> codeBlocks)
    {
        using SqlConnection conn = new(_connectionString);
        await conn.OpenAsync();

        using SqlTransaction tx = conn.BeginTransaction();

        try
        {
            await InsertDocPageAsync(conn, tx, page);

            foreach (DocSection section in sections) await InsertDocSectionAsync(conn, tx, section);

            foreach (CodeBlock block in codeBlocks) await InsertCodeBlockAsync(conn, tx, block);

            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }








    private static async Task InsertDocPageAsync(SqlConnection conn, SqlTransaction tx, DocPage page)
    {
        const string sql = @"
insert into dbo.doc_page (
    id,
    semantic_uid,
    source_snapshot_id,
    source_path,
    title,
    language,
    url,
    raw_markdown,
    version_number,
    created_ingestion_run_id,
    updated_ingestion_run_id,
    removed_ingestion_run_id,
    valid_from_utc,
    valid_to_utc,
    is_active,
    content_hash
) values (
    @id,
    @semantic_uid,
    @source_snapshot_id,
    @source_path,
    @title,
    @language,
    @url,
    @raw_markdown,
    @version_number,
    @created_ingestion_run_id,
    @updated_ingestion_run_id,
    @removed_ingestion_run_id,
    @valid_from_utc,
    @valid_to_utc,
    @is_active,
    @content_hash
);";

        using SqlCommand cmd = new(sql, conn, tx);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = page.Id;
        cmd.Parameters.Add("@semantic_uid", SqlDbType.NVarChar, 1000).Value = (object)page.SemanticUid ?? DBNull.Value;
        cmd.Parameters.Add("@source_snapshot_id", SqlDbType.UniqueIdentifier).Value = page.SourceSnapshotId;
        cmd.Parameters.Add("@source_path", SqlDbType.NVarChar, -1).Value = (object)page.SourcePath ?? DBNull.Value;
        cmd.Parameters.Add("@title", SqlDbType.NVarChar, 400).Value = (object)page.Title ?? DBNull.Value;
        cmd.Parameters.Add("@language", SqlDbType.NVarChar, 200).Value = (object)page.Language ?? DBNull.Value;
        cmd.Parameters.Add("@url", SqlDbType.NVarChar, -1).Value = page.Url;
        cmd.Parameters.Add("@raw_markdown", SqlDbType.NVarChar, -1).Value = (object)page.RawMarkdown ?? DBNull.Value;
        cmd.Parameters.Add("@version_number", SqlDbType.Int).Value = page.VersionNumber;
        cmd.Parameters.Add("@created_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = page.CreatedIngestionRunId;
        cmd.Parameters.Add("@updated_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)page.UpdatedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@removed_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)page.RemovedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@valid_from_utc", SqlDbType.DateTime2).Value = page.ValidFromUtc;
        cmd.Parameters.Add("@valid_to_utc", SqlDbType.DateTime2).Value = (object)page.ValidToUtc ?? DBNull.Value;
        cmd.Parameters.Add("@is_active", SqlDbType.Bit).Value = page.IsActive;
        cmd.Parameters.Add("@content_hash", SqlDbType.Binary, 32).Value = (object)page.ContentHash ?? DBNull.Value;

        await cmd.ExecuteNonQueryAsync();
    }








    private static async Task InsertDocSectionAsync(SqlConnection conn, SqlTransaction tx, DocSection section)
    {
        const string sql = @"
insert into dbo.doc_section (
    id,
    doc_page_id,
    semantic_uid,
    heading,
    level,
    content_markdown,
    order_index,
    version_number,
    created_ingestion_run_id,
    updated_ingestion_run_id,
    removed_ingestion_run_id,
    valid_from_utc,
    valid_to_utc,
    is_active,
    content_hash
) values (
    @id,
    @doc_page_id,
    @semantic_uid,
    @heading,
    @level,
    @content_markdown,
    @order_index,
    @version_number,
    @created_ingestion_run_id,
    @updated_ingestion_run_id,
    @removed_ingestion_run_id,
    @valid_from_utc,
    @valid_to_utc,
    @is_active,
    @content_hash
);";

        using SqlCommand cmd = new(sql, conn, tx);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = section.Id;
        cmd.Parameters.Add("@doc_page_id", SqlDbType.UniqueIdentifier).Value = section.DocPageId;
        cmd.Parameters.Add("@semantic_uid", SqlDbType.NVarChar, 1000).Value = section.SemanticUid;
        cmd.Parameters.Add("@heading", SqlDbType.NVarChar, 400).Value = (object)section.Heading ?? DBNull.Value;
        cmd.Parameters.Add("@level", SqlDbType.Int).Value = (object)section.Level ?? DBNull.Value;
        cmd.Parameters.Add("@content_markdown", SqlDbType.NVarChar, -1).Value = (object)section.ContentMarkdown ?? DBNull.Value;
        cmd.Parameters.Add("@order_index", SqlDbType.Int).Value = (object)section.OrderIndex ?? DBNull.Value;
        cmd.Parameters.Add("@version_number", SqlDbType.Int).Value = section.VersionNumber;
        cmd.Parameters.Add("@created_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = section.CreatedIngestionRunId;
        cmd.Parameters.Add("@updated_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)section.UpdatedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@removed_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)section.RemovedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@valid_from_utc", SqlDbType.DateTime2).Value = section.ValidFromUtc;
        cmd.Parameters.Add("@valid_to_utc", SqlDbType.DateTime2).Value = (object)section.ValidToUtc ?? DBNull.Value;
        cmd.Parameters.Add("@is_active", SqlDbType.Bit).Value = section.IsActive;
        cmd.Parameters.Add("@content_hash", SqlDbType.Binary, 32).Value = (object)section.ContentHash ?? DBNull.Value;

        await cmd.ExecuteNonQueryAsync();
    }








    private static async Task InsertCodeBlockAsync(SqlConnection conn, SqlTransaction tx, CodeBlock block)
    {
        const string sql = @"
insert into dbo.code_block (
    id,
    doc_section_id,
    semantic_uid,
    language,
    content,
    declared_packages,
    tags,
    inline_comments,
    version_number,
    created_ingestion_run_id,
    updated_ingestion_run_id,
    removed_ingestion_run_id,
    valid_from_utc,
    valid_to_utc,
    is_active,
    content_hash
) values (
    @id,
    @doc_section_id,
    @semantic_uid,
    @language,
    @content,
    @declared_packages,
    @tags,
    @inline_comments,
    @version_number,
    @created_ingestion_run_id,
    @updated_ingestion_run_id,
    @removed_ingestion_run_id,
    @valid_from_utc,
    @valid_to_utc,
    @is_active,
    @content_hash
);";

        using SqlCommand cmd = new(sql, conn, tx);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = block.Id;
        cmd.Parameters.Add("@doc_section_id", SqlDbType.UniqueIdentifier).Value = block.DocSectionId;
        cmd.Parameters.Add("@semantic_uid", SqlDbType.NVarChar, 1000).Value = (object)block.SemanticUid ?? DBNull.Value;
        cmd.Parameters.Add("@language", SqlDbType.NVarChar, 200).Value = (object)block.Language ?? DBNull.Value;
        cmd.Parameters.Add("@content", SqlDbType.NVarChar, -1).Value = (object)block.Content ?? DBNull.Value;
        cmd.Parameters.Add("@declared_packages", SqlDbType.NVarChar, -1).Value = (object)block.DeclaredPackages ?? DBNull.Value;
        cmd.Parameters.Add("@tags", SqlDbType.NVarChar, -1).Value = (object)block.Tags ?? DBNull.Value;
        cmd.Parameters.Add("@inline_comments", SqlDbType.NVarChar, -1).Value = (object)block.InlineComments ?? DBNull.Value;
        cmd.Parameters.Add("@version_number", SqlDbType.Int).Value = block.VersionNumber;
        cmd.Parameters.Add("@created_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = block.CreatedIngestionRunId;
        cmd.Parameters.Add("@updated_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)block.UpdatedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@removed_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)block.RemovedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@valid_from_utc", SqlDbType.DateTime2).Value = block.ValidFromUtc;
        cmd.Parameters.Add("@valid_to_utc", SqlDbType.DateTime2).Value = (object)block.ValidToUtc ?? DBNull.Value;
        cmd.Parameters.Add("@is_active", SqlDbType.Bit).Value = block.IsActive;
        cmd.Parameters.Add("@content_hash", SqlDbType.Binary, 32).Value = (object)block.ContentHash ?? DBNull.Value;

        await cmd.ExecuteNonQueryAsync();
    }
}