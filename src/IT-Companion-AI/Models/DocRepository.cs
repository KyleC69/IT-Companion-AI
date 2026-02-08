using System.Data;

using ITCompanionAI;

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
set nocount on;

-- If an active row exists for this semantic uid and the content hash is identical,
-- treat this as idempotent and avoid creating a new version.
if exists (
    select 1
    from dbo.doc_page p
    where p.semantic_uid = @semantic_uid
      and p.is_active = 1
      and p.content_hash = @content_hash
)
begin
    return;
end;

declare @next_version int;

select @next_version = isnull(max(p.version_number), 0) + 1
from dbo.doc_page p with (updlock, holdlock)
where p.semantic_uid = @semantic_uid;

-- Close any active row for this semantic uid.
update dbo.doc_page
set valid_to_utc = @valid_from_utc,
    is_active = 0,
    updated_ingestion_run_id = @created_ingestion_run_id
where semantic_uid = @semantic_uid
  and is_active = 1;

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
    @next_version,
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
        cmd.Parameters.Add("@raw_page_source", SqlDbType.NVarChar, -1).Value = (object)page.RawPageSource ?? DBNull.Value;
        cmd.Parameters.Add("@created_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = page.CreatedIngestionRunId;
        cmd.Parameters.Add("@updated_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)page.UpdatedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@removed_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)page.RemovedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@valid_from_utc", SqlDbType.DateTime2).Value = page.ValidFromUtc;
        cmd.Parameters.Add("@valid_to_utc", SqlDbType.DateTime2).Value = (object)page.ValidToUtc ?? DBNull.Value;
        cmd.Parameters.Add("@is_active", SqlDbType.Bit).Value = page.IsActive;
        cmd.Parameters.Add("@content_hash", SqlDbType.Binary, 32).Value = (object)page.ContentHash ?? DBNull.Value;

        _ = await cmd.ExecuteNonQueryAsync();
    }








    private static async Task InsertDocSectionAsync(SqlConnection conn, SqlTransaction tx, DocSection section)
    {
        const string sql = @"
set nocount on;

-- If an active row exists for this semantic uid and the content hash is identical,
-- treat this as idempotent and avoid creating a new version.
if exists (
    select 1
    from dbo.doc_section s
    where s.semantic_uid = @semantic_uid
      and s.is_active = 1
      and s.content_hash = @content_hash
)
begin
    return;
end;

declare @next_version int;

select @next_version = isnull(max(s.version_number), 0) + 1
from dbo.doc_section s with (updlock, holdlock)
where s.semantic_uid = @semantic_uid;

-- Close any active row for this semantic uid.
update dbo.doc_section
set valid_to_utc = @valid_from_utc,
    is_active = 0,
    updated_ingestion_run_id = @created_ingestion_run_id
where semantic_uid = @semantic_uid
  and is_active = 1;

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
    @next_version,
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
        cmd.Parameters.Add("@created_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = section.CreatedIngestionRunId;
        cmd.Parameters.Add("@updated_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)section.UpdatedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@removed_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)section.RemovedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@valid_from_utc", SqlDbType.DateTime2).Value = section.ValidFromUtc;
        cmd.Parameters.Add("@valid_to_utc", SqlDbType.DateTime2).Value = (object)section.ValidToUtc ?? DBNull.Value;
        cmd.Parameters.Add("@is_active", SqlDbType.Bit).Value = section.IsActive;
        cmd.Parameters.Add("@content_hash", SqlDbType.Binary, 32).Value = (object)section.ContentHash ?? DBNull.Value;

        _ = await cmd.ExecuteNonQueryAsync();
    }








    private static async Task InsertCodeBlockAsync(SqlConnection conn, SqlTransaction tx, CodeBlock block)
    {
        const string sql = @"
set nocount on;

-- If an active row exists for this semantic uid and the content hash is identical,
-- treat this as idempotent and avoid creating a new version.
if exists (
    select 1
    from dbo.code_block cb
    where cb.semantic_uid = @semantic_uid
      and cb.is_active = 1
      and cb.content_hash = @content_hash
)
begin
    return;
end;

declare @next_version int;

select @next_version = isnull(max(cb.version_number), 0) + 1
from dbo.code_block cb with (updlock, holdlock)
where cb.semantic_uid = @semantic_uid;

-- Close any active row for this semantic uid.
update dbo.code_block
set valid_to_utc = @valid_from_utc,
    is_active = 0,
    updated_ingestion_run_id = @created_ingestion_run_id
where semantic_uid = @semantic_uid
  and is_active = 1;

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
    @next_version,
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
        cmd.Parameters.Add("@created_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = block.CreatedIngestionRunId;
        cmd.Parameters.Add("@updated_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)block.UpdatedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@removed_ingestion_run_id", SqlDbType.UniqueIdentifier).Value = (object)block.RemovedIngestionRunId ?? DBNull.Value;
        cmd.Parameters.Add("@valid_from_utc", SqlDbType.DateTime2).Value = block.ValidFromUtc;
        cmd.Parameters.Add("@valid_to_utc", SqlDbType.DateTime2).Value = (object)block.ValidToUtc ?? DBNull.Value;
        cmd.Parameters.Add("@is_active", SqlDbType.Bit).Value = block.IsActive;
        cmd.Parameters.Add("@content_hash", SqlDbType.Binary, 32).Value = (object)block.ContentHash ?? DBNull.Value;

        _ = await cmd.ExecuteNonQueryAsync();
    }








    public async Task InsertChunksAsync(List<RagChunk> chunks, CancellationToken cancellationToken)
    {
        KBContext context = new();
        context.RagChunks.AddRange(chunks);
        await context.SaveChangesAsync(cancellationToken);
    }
}