using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI;

public partial class AiagentRagContext : DbContext
{
    public AiagentRagContext()
    {
    }

    public AiagentRagContext(DbContextOptions<AiagentRagContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApiMember> ApiMembers { get; set; }

    public virtual DbSet<ApiMemberDiff> ApiMemberDiffs { get; set; }

    public virtual DbSet<ApiMemberDocLink> ApiMemberDocLinks { get; set; }

    public virtual DbSet<ApiParameter> ApiParameters { get; set; }

    public virtual DbSet<ApiType> ApiTypes { get; set; }

    public virtual DbSet<ApiTypeDiff> ApiTypeDiffs { get; set; }

    public virtual DbSet<Chunk> Chunks { get; set; }

    public virtual DbSet<CodeBlock> CodeBlocks { get; set; }

    public virtual DbSet<DocPage> DocPages { get; set; }

    public virtual DbSet<DocPageDiff> DocPageDiffs { get; set; }

    public virtual DbSet<DocSection> DocSections { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<ExecutionResult> ExecutionResults { get; set; }

    public virtual DbSet<ExecutionRun> ExecutionRuns { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<FeatureDocLink> FeatureDocLinks { get; set; }

    public virtual DbSet<FeatureMemberLink> FeatureMemberLinks { get; set; }

    public virtual DbSet<FeatureTypeLink> FeatureTypeLinks { get; set; }

    public virtual DbSet<IngestionRun> IngestionRuns { get; set; }

    public virtual DbSet<RagChunk> RagChunks { get; set; }

    public virtual DbSet<RagRun> RagRuns { get; set; }

    public virtual DbSet<ReconciledChunk> ReconciledChunks { get; set; }

    public virtual DbSet<ReviewIssue> ReviewIssues { get; set; }

    public virtual DbSet<ReviewItem> ReviewItems { get; set; }

    public virtual DbSet<ReviewRun> ReviewRuns { get; set; }

    public virtual DbSet<Sample> Samples { get; set; }

    public virtual DbSet<SampleApiMemberLink> SampleApiMemberLinks { get; set; }

    public virtual DbSet<SampleRun> SampleRuns { get; set; }

    public virtual DbSet<SnapshotDiff> SnapshotDiffs { get; set; }

    public virtual DbSet<SourceSnapshot> SourceSnapshots { get; set; }

    public virtual DbSet<TruthRun> TruthRuns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=AIAgentRag");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__api_memb__3213E83F7176B5EE");

            entity.ToTable("api_member");

            entity.HasIndex(e => e.ApiTypeId, "idx_api_member_api_type_id");

            entity.HasIndex(e => new { e.ApiTypeId, e.MemberUid }, "uq_api_member_uid_per_type").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Accessibility)
                .HasMaxLength(200)
                .HasColumnName("accessibility");
            entity.Property(e => e.ApiTypeId).HasColumnName("api_type_id");
            entity.Property(e => e.Attributes).HasColumnName("attributes");
            entity.Property(e => e.GenericParameters).HasColumnName("generic_parameters");
            entity.Property(e => e.IsAsync).HasColumnName("is_async");
            entity.Property(e => e.IsExtensionMethod).HasColumnName("is_extension_method");
            entity.Property(e => e.IsStatic).HasColumnName("is_static");
            entity.Property(e => e.Kind)
                .HasMaxLength(200)
                .HasColumnName("kind");
            entity.Property(e => e.MemberUid)
                .HasMaxLength(200)
                .HasColumnName("member_uid");
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .HasColumnName("name");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.ReturnType)
                .HasMaxLength(400)
                .HasColumnName("return_type");
            entity.Property(e => e.SourceEndLine).HasColumnName("source_end_line");
            entity.Property(e => e.SourceFilePath).HasColumnName("source_file_path");
            entity.Property(e => e.SourceStartLine).HasColumnName("source_start_line");
            entity.Property(e => e.Summary).HasColumnName("summary");

            entity.HasOne(d => d.ApiType).WithMany(p => p.ApiMembers)
                .HasForeignKey(d => d.ApiTypeId)
                .HasConstraintName("fk_api_member_type");
        });

        modelBuilder.Entity<ApiMemberDiff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__api_memb__3213E83F54D61428");

            entity.ToTable("api_member_diff");

            entity.HasIndex(e => e.SnapshotDiffId, "idx_api_member_diff_snapshot_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Breaking).HasColumnName("breaking");
            entity.Property(e => e.ChangeKind)
                .HasMaxLength(200)
                .HasColumnName("change_kind");
            entity.Property(e => e.DetailJson).HasColumnName("detail_json");
            entity.Property(e => e.MemberUid)
                .HasMaxLength(200)
                .HasColumnName("member_uid");
            entity.Property(e => e.NewSignature).HasColumnName("new_signature");
            entity.Property(e => e.OldSignature).HasColumnName("old_signature");
            entity.Property(e => e.SnapshotDiffId).HasColumnName("snapshot_diff_id");

            entity.HasOne(d => d.SnapshotDiff).WithMany(p => p.ApiMemberDiffs)
                .HasForeignKey(d => d.SnapshotDiffId)
                .HasConstraintName("fk_api_member_diff_snapshot");
        });

        modelBuilder.Entity<ApiMemberDocLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__api_memb__3213E83F23EA70F7");

            entity.ToTable("api_member_doc_link");

            entity.HasIndex(e => e.ApiMemberId, "idx_api_member_doc_link_member_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ApiMemberId).HasColumnName("api_member_id");
            entity.Property(e => e.DocUid)
                .HasMaxLength(200)
                .HasColumnName("doc_uid");
            entity.Property(e => e.SectionUid)
                .HasMaxLength(200)
                .HasColumnName("section_uid");

            entity.HasOne(d => d.ApiMember).WithMany(p => p.ApiMemberDocLinks)
                .HasForeignKey(d => d.ApiMemberId)
                .HasConstraintName("fk_api_member_doc_link_member");
        });

        modelBuilder.Entity<ApiParameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__api_para__3213E83F85AED3FD");

            entity.ToTable("api_parameter");

            entity.HasIndex(e => e.ApiMemberId, "idx_api_parameter_member_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ApiMemberId).HasColumnName("api_member_id");
            entity.Property(e => e.DefaultValueLiteral).HasColumnName("default_value_literal");
            entity.Property(e => e.HasDefaultValue).HasColumnName("has_default_value");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Type)
                .HasMaxLength(400)
                .HasColumnName("type");

            entity.HasOne(d => d.ApiMember).WithMany(p => p.ApiParameters)
                .HasForeignKey(d => d.ApiMemberId)
                .HasConstraintName("fk_api_parameter_member");
        });

        modelBuilder.Entity<ApiType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__api_type__3213E83FFA572C86");

            entity.ToTable("api_type");

            entity.HasIndex(e => e.SourceSnapshotId, "idx_api_type_snapshot_id");

            entity.HasIndex(e => new { e.SourceSnapshotId, e.TypeUid }, "uq_api_type_uid_per_snapshot").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Accessibility)
                .HasMaxLength(200)
                .HasColumnName("accessibility");
            entity.Property(e => e.Attributes).HasColumnName("attributes");
            entity.Property(e => e.GenericParameters).HasColumnName("generic_parameters");
            entity.Property(e => e.IsGeneric).HasColumnName("is_generic");
            entity.Property(e => e.IsStatic).HasColumnName("is_static");
            entity.Property(e => e.Kind)
                .HasMaxLength(200)
                .HasColumnName("kind");
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .HasColumnName("name");
            entity.Property(e => e.Namespace)
                .HasMaxLength(400)
                .HasColumnName("namespace");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.SourceSnapshotId).HasColumnName("source_snapshot_id");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.TypeUid)
                .HasMaxLength(200)
                .HasColumnName("type_uid");

            entity.HasOne(d => d.SourceSnapshot).WithMany(p => p.ApiTypes)
                .HasForeignKey(d => d.SourceSnapshotId)
                .HasConstraintName("fk_api_type_snapshot");
        });

        modelBuilder.Entity<ApiTypeDiff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__api_type__3213E83F46151492");

            entity.ToTable("api_type_diff");

            entity.HasIndex(e => e.SnapshotDiffId, "idx_api_type_diff_snapshot_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ChangeKind)
                .HasMaxLength(200)
                .HasColumnName("change_kind");
            entity.Property(e => e.DetailJson).HasColumnName("detail_json");
            entity.Property(e => e.SnapshotDiffId).HasColumnName("snapshot_diff_id");
            entity.Property(e => e.TypeUid)
                .HasMaxLength(200)
                .HasColumnName("type_uid");

            entity.HasOne(d => d.SnapshotDiff).WithMany(p => p.ApiTypeDiffs)
                .HasForeignKey(d => d.SnapshotDiffId)
                .HasConstraintName("fk_api_type_diff_snapshot");
        });

        modelBuilder.Entity<Chunk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__chunks__3213E83F1AB0955F");

            entity.ToTable("chunks");

            entity.HasIndex(e => e.Symbol, "IX_chunks_symbol");

            entity.HasIndex(e => new { e.DocumentId, e.ChunkIndex }, "UQ_chunks_doc_chunk").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ChunkIndex).HasColumnName("chunk_index");
            entity.Property(e => e.Confidence).HasColumnName("confidence");
            entity.Property(e => e.Deprecated).HasColumnName("deprecated");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.Embedding)
                .HasMaxLength(384)
                .HasColumnName("embedding");
            entity.Property(e => e.Kind)
                .HasMaxLength(128)
                .HasColumnName("kind");
            entity.Property(e => e.Section)
                .HasMaxLength(256)
                .HasColumnName("section");
            entity.Property(e => e.Symbol)
                .HasMaxLength(256)
                .HasColumnName("symbol");
            entity.Property(e => e.Text).HasColumnName("text");
            entity.Property(e => e.TokenCount).HasColumnName("token_count");
            entity.Property(e => e.Verified).HasColumnName("verified");

            entity.HasOne(d => d.Document).WithMany(p => p.Chunks)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_chunks_documents");
        });

        modelBuilder.Entity<CodeBlock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__code_blo__3213E83F62F03902");

            entity.ToTable("code_block");

            entity.HasIndex(e => e.DocSectionId, "idx_code_block_section_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CodeUid)
                .HasMaxLength(200)
                .HasColumnName("code_uid");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.DeclaredPackages).HasColumnName("declared_packages");
            entity.Property(e => e.DocSectionId).HasColumnName("doc_section_id");
            entity.Property(e => e.InlineComments).HasColumnName("inline_comments");
            entity.Property(e => e.Language)
                .HasMaxLength(200)
                .HasColumnName("language");
            entity.Property(e => e.Tags).HasColumnName("tags");

            entity.HasOne(d => d.DocSection).WithMany(p => p.CodeBlocks)
                .HasForeignKey(d => d.DocSectionId)
                .HasConstraintName("fk_code_block_section");
        });

        modelBuilder.Entity<DocPage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__doc_page__3213E83FA881CE41");

            entity.ToTable("doc_page");

            entity.HasIndex(e => e.SourceSnapshotId, "idx_doc_page_snapshot_id");

            entity.HasIndex(e => new { e.SourceSnapshotId, e.DocUid }, "uq_doc_page_uid_per_snapshot").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.DocUid)
                .HasMaxLength(200)
                .HasColumnName("doc_uid");
            entity.Property(e => e.Language)
                .HasMaxLength(200)
                .HasColumnName("language");
            entity.Property(e => e.RawMarkdown).HasColumnName("raw_markdown");
            entity.Property(e => e.SourcePath).HasColumnName("source_path");
            entity.Property(e => e.SourceSnapshotId).HasColumnName("source_snapshot_id");
            entity.Property(e => e.Title)
                .HasMaxLength(400)
                .HasColumnName("title");
            entity.Property(e => e.Url).HasColumnName("url");

            entity.HasOne(d => d.SourceSnapshot).WithMany(p => p.DocPages)
                .HasForeignKey(d => d.SourceSnapshotId)
                .HasConstraintName("fk_doc_page_snapshot");
        });

        modelBuilder.Entity<DocPageDiff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__doc_page__3213E83F371C1220");

            entity.ToTable("doc_page_diff");

            entity.HasIndex(e => e.SnapshotDiffId, "idx_doc_page_diff_snapshot_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ChangeKind)
                .HasMaxLength(200)
                .HasColumnName("change_kind");
            entity.Property(e => e.DetailJson).HasColumnName("detail_json");
            entity.Property(e => e.DocUid)
                .HasMaxLength(200)
                .HasColumnName("doc_uid");
            entity.Property(e => e.SnapshotDiffId).HasColumnName("snapshot_diff_id");

            entity.HasOne(d => d.SnapshotDiff).WithMany(p => p.DocPageDiffs)
                .HasForeignKey(d => d.SnapshotDiffId)
                .HasConstraintName("fk_doc_page_diff_snapshot");
        });

        modelBuilder.Entity<DocSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__doc_sect__3213E83FE4A7935C");

            entity.ToTable("doc_section");

            entity.HasIndex(e => e.DocPageId, "idx_doc_section_page_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ContentMarkdown).HasColumnName("content_markdown");
            entity.Property(e => e.DocPageId).HasColumnName("doc_page_id");
            entity.Property(e => e.Heading)
                .HasMaxLength(400)
                .HasColumnName("heading");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
            entity.Property(e => e.SectionUid)
                .HasMaxLength(200)
                .HasColumnName("section_uid");

            entity.HasOne(d => d.DocPage).WithMany(p => p.DocSections)
                .HasForeignKey(d => d.DocPageId)
                .HasConstraintName("fk_doc_section_page");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__document__3213E83FF9492104");

            entity.ToTable("documents");

            entity.HasIndex(e => e.ExternalId, "UQ_documents_external_id").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExternalId)
                .HasMaxLength(512)
                .HasColumnName("external_id");
            entity.Property(e => e.LastError).HasColumnName("last_error");
            entity.Property(e => e.Source)
                .HasMaxLength(256)
                .HasColumnName("source");
            entity.Property(e => e.Status)
                .HasMaxLength(64)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(512)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Version)
                .HasMaxLength(128)
                .HasColumnName("version");
        });

        modelBuilder.Entity<ExecutionResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__executio__3213E83F2AF526F3");

            entity.ToTable("execution_result");

            entity.HasIndex(e => e.ExecutionRunId, "idx_execution_result_run_id");

            entity.HasIndex(e => e.SampleUid, "idx_execution_result_sample_uid");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.BuildLog).HasColumnName("build_log");
            entity.Property(e => e.DurationMs).HasColumnName("duration_ms");
            entity.Property(e => e.ExceptionJson).HasColumnName("exception_json");
            entity.Property(e => e.ExecutionRunId).HasColumnName("execution_run_id");
            entity.Property(e => e.RunLog).HasColumnName("run_log");
            entity.Property(e => e.SampleUid)
                .HasMaxLength(200)
                .HasColumnName("sample_uid");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasColumnName("status");

            entity.HasOne(d => d.ExecutionRun).WithMany(p => p.ExecutionResults)
                .HasForeignKey(d => d.ExecutionRunId)
                .HasConstraintName("fk_execution_result_run");
        });

        modelBuilder.Entity<ExecutionRun>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__executio__3213E83F95C925B2");

            entity.ToTable("execution_run");

            entity.HasIndex(e => e.SampleRunId, "idx_execution_run_sample_run_id");

            entity.HasIndex(e => e.SnapshotId, "idx_execution_run_snapshot_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.EnvironmentJson).HasColumnName("environment_json");
            entity.Property(e => e.SampleRunId).HasColumnName("sample_run_id");
            entity.Property(e => e.SchemaVersion)
                .HasMaxLength(200)
                .HasColumnName("schema_version");
            entity.Property(e => e.SnapshotId).HasColumnName("snapshot_id");
            entity.Property(e => e.TimestampUtc).HasColumnName("timestamp_utc");

            entity.HasOne(d => d.SampleRun).WithMany(p => p.ExecutionRuns)
                .HasForeignKey(d => d.SampleRunId)
                .HasConstraintName("fk_execution_run_sample_run");

            entity.HasOne(d => d.Snapshot).WithMany(p => p.ExecutionRuns)
                .HasForeignKey(d => d.SnapshotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_execution_run_snapshot");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__feature__3213E83F7545A6AC");

            entity.ToTable("feature");

            entity.HasIndex(e => e.TruthRunId, "idx_feature_truth_run_id");

            entity.HasIndex(e => e.FeatureUid, "uq_feature_uid").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FeatureUid)
                .HasMaxLength(200)
                .HasColumnName("feature_uid");
            entity.Property(e => e.IntroducedInVersion)
                .HasMaxLength(200)
                .HasColumnName("introduced_in_version");
            entity.Property(e => e.Language)
                .HasMaxLength(200)
                .HasColumnName("language");
            entity.Property(e => e.LastSeenVersion)
                .HasMaxLength(200)
                .HasColumnName("last_seen_version");
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .HasColumnName("name");
            entity.Property(e => e.Tags).HasColumnName("tags");
            entity.Property(e => e.TruthRunId).HasColumnName("truth_run_id");

            entity.HasOne(d => d.TruthRun).WithMany(p => p.Features)
                .HasForeignKey(d => d.TruthRunId)
                .HasConstraintName("fk_feature_truth_run");
        });

        modelBuilder.Entity<FeatureDocLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__feature___3213E83FD9CBAAB0");

            entity.ToTable("feature_doc_link");

            entity.HasIndex(e => e.FeatureId, "idx_feature_doc_link_feature_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.DocUid)
                .HasMaxLength(200)
                .HasColumnName("doc_uid");
            entity.Property(e => e.FeatureId).HasColumnName("feature_id");
            entity.Property(e => e.SectionUid)
                .HasMaxLength(200)
                .HasColumnName("section_uid");

            entity.HasOne(d => d.Feature).WithMany(p => p.FeatureDocLinks)
                .HasForeignKey(d => d.FeatureId)
                .HasConstraintName("fk_feature_doc_link_feature");
        });

        modelBuilder.Entity<FeatureMemberLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__feature___3213E83FD36D6782");

            entity.ToTable("feature_member_link");

            entity.HasIndex(e => e.FeatureId, "idx_feature_member_link_feature_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.FeatureId).HasColumnName("feature_id");
            entity.Property(e => e.MemberUid)
                .HasMaxLength(200)
                .HasColumnName("member_uid");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");

            entity.HasOne(d => d.Feature).WithMany(p => p.FeatureMemberLinks)
                .HasForeignKey(d => d.FeatureId)
                .HasConstraintName("fk_feature_member_link_feature");
        });

        modelBuilder.Entity<FeatureTypeLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__feature___3213E83FB16C668D");

            entity.ToTable("feature_type_link");

            entity.HasIndex(e => e.FeatureId, "idx_feature_type_link_feature_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.FeatureId).HasColumnName("feature_id");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.TypeUid)
                .HasMaxLength(200)
                .HasColumnName("type_uid");

            entity.HasOne(d => d.Feature).WithMany(p => p.FeatureTypeLinks)
                .HasForeignKey(d => d.FeatureId)
                .HasConstraintName("fk_feature_type_link_feature");
        });

        modelBuilder.Entity<IngestionRun>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ingestio__3213E83F45B645D4");

            entity.ToTable("ingestion_run");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.SchemaVersion)
                .HasMaxLength(200)
                .HasColumnName("schema_version");
            entity.Property(e => e.TimestampUtc).HasColumnName("timestamp_utc");
        });

        modelBuilder.Entity<RagChunk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rag_chun__3213E83FCAFFCCB7");

            entity.ToTable("rag_chunk");

            entity.HasIndex(e => e.Kind, "idx_rag_chunk_kind");

            entity.HasIndex(e => e.RagRunId, "idx_rag_chunk_run_id");

            entity.HasIndex(e => e.ChunkUid, "uq_rag_chunk_uid").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ChunkUid)
                .HasMaxLength(200)
                .HasColumnName("chunk_uid");
            entity.Property(e => e.EmbeddingVector)
                .HasMaxLength(1536)
                .HasColumnName("embedding_vector");
            entity.Property(e => e.Kind)
                .HasMaxLength(100)
                .HasColumnName("kind");
            entity.Property(e => e.MetadataJson).HasColumnName("metadata_json");
            entity.Property(e => e.RagRunId).HasColumnName("rag_run_id");
            entity.Property(e => e.Text).HasColumnName("text");

            entity.HasOne(d => d.RagRun).WithMany(p => p.RagChunks)
                .HasForeignKey(d => d.RagRunId)
                .HasConstraintName("fk_rag_chunk_run");
        });

        modelBuilder.Entity<RagRun>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rag_run__3213E83F57D7FBF8");

            entity.ToTable("rag_run");

            entity.HasIndex(e => e.SnapshotId, "idx_rag_run_snapshot_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.SchemaVersion)
                .HasMaxLength(200)
                .HasColumnName("schema_version");
            entity.Property(e => e.SnapshotId).HasColumnName("snapshot_id");
            entity.Property(e => e.TimestampUtc).HasColumnName("timestamp_utc");

            entity.HasOne(d => d.Snapshot).WithMany(p => p.RagRuns)
                .HasForeignKey(d => d.SnapshotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rag_run_snapshot");
        });

        modelBuilder.Entity<ReconciledChunk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__reconcil__3213E83F971C3015");

            entity.ToTable("reconciled_chunks");

            entity.HasIndex(e => e.Symbol, "IX_reconciled_symbol");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Confidence).HasColumnName("confidence");
            entity.Property(e => e.Embedding)
                .HasMaxLength(384)
                .HasColumnName("embedding");
            entity.Property(e => e.Namespace)
                .HasMaxLength(256)
                .HasColumnName("namespace");
            entity.Property(e => e.SourceCount).HasColumnName("source_count");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.Symbol)
                .HasMaxLength(256)
                .HasColumnName("symbol");
            entity.Property(e => e.Version)
                .HasMaxLength(128)
                .HasColumnName("version");
        });

        modelBuilder.Entity<ReviewIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__review_i__3213E83F6452AB56");

            entity.ToTable("review_issue");

            entity.HasIndex(e => e.ReviewItemId, "idx_review_issue_item_id");

            entity.HasIndex(e => e.RelatedMemberUid, "idx_review_issue_related_member_uid");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(200)
                .HasColumnName("code");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.RelatedMemberUid)
                .HasMaxLength(200)
                .HasColumnName("related_member_uid");
            entity.Property(e => e.ReviewItemId).HasColumnName("review_item_id");
            entity.Property(e => e.Severity)
                .HasMaxLength(50)
                .HasColumnName("severity");

            entity.HasOne(d => d.ReviewItem).WithMany(p => p.ReviewIssues)
                .HasForeignKey(d => d.ReviewItemId)
                .HasConstraintName("fk_review_issue_item");
        });

        modelBuilder.Entity<ReviewItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__review_i__3213E83FC0D6698F");

            entity.ToTable("review_item");

            entity.HasIndex(e => e.ReviewRunId, "idx_review_item_run_id");

            entity.HasIndex(e => e.TargetUid, "idx_review_item_target_uid");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ReviewRunId).HasColumnName("review_run_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.TargetKind)
                .HasMaxLength(50)
                .HasColumnName("target_kind");
            entity.Property(e => e.TargetUid)
                .HasMaxLength(200)
                .HasColumnName("target_uid");

            entity.HasOne(d => d.ReviewRun).WithMany(p => p.ReviewItems)
                .HasForeignKey(d => d.ReviewRunId)
                .HasConstraintName("fk_review_item_run");
        });

        modelBuilder.Entity<ReviewRun>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__review_r__3213E83FD44872D2");

            entity.ToTable("review_run");

            entity.HasIndex(e => e.SnapshotId, "idx_review_run_snapshot_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.SchemaVersion)
                .HasMaxLength(200)
                .HasColumnName("schema_version");
            entity.Property(e => e.SnapshotId).HasColumnName("snapshot_id");
            entity.Property(e => e.TimestampUtc).HasColumnName("timestamp_utc");

            entity.HasOne(d => d.Snapshot).WithMany(p => p.ReviewRuns)
                .HasForeignKey(d => d.SnapshotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_run_snapshot");
        });

        modelBuilder.Entity<Sample>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sample__3213E83FB36EC2F6");

            entity.ToTable("sample");

            entity.HasIndex(e => e.FeatureUid, "idx_sample_feature_uid");

            entity.HasIndex(e => e.SampleRunId, "idx_sample_sample_run_id");

            entity.HasIndex(e => e.SampleUid, "uq_sample_uid").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.DerivedFromCodeUid)
                .HasMaxLength(200)
                .HasColumnName("derived_from_code_uid");
            entity.Property(e => e.EntryPoint)
                .HasMaxLength(400)
                .HasColumnName("entry_point");
            entity.Property(e => e.FeatureUid)
                .HasMaxLength(200)
                .HasColumnName("feature_uid");
            entity.Property(e => e.Language)
                .HasMaxLength(200)
                .HasColumnName("language");
            entity.Property(e => e.PackageReferences).HasColumnName("package_references");
            entity.Property(e => e.SampleRunId).HasColumnName("sample_run_id");
            entity.Property(e => e.SampleUid)
                .HasMaxLength(200)
                .HasColumnName("sample_uid");
            entity.Property(e => e.Tags).HasColumnName("tags");
            entity.Property(e => e.TargetFramework)
                .HasMaxLength(200)
                .HasColumnName("target_framework");

            entity.HasOne(d => d.SampleRun).WithMany(p => p.Samples)
                .HasForeignKey(d => d.SampleRunId)
                .HasConstraintName("fk_sample_sample_run");
        });

        modelBuilder.Entity<SampleApiMemberLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sample_a__3213E83F498C99B8");

            entity.ToTable("sample_api_member_link");

            entity.HasIndex(e => e.SampleId, "idx_sample_api_member_link_sample_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.MemberUid)
                .HasMaxLength(200)
                .HasColumnName("member_uid");
            entity.Property(e => e.SampleId).HasColumnName("sample_id");

            entity.HasOne(d => d.Sample).WithMany(p => p.SampleApiMemberLinks)
                .HasForeignKey(d => d.SampleId)
                .HasConstraintName("fk_sample_api_member_link_sample");
        });

        modelBuilder.Entity<SampleRun>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sample_r__3213E83FDFCF2090");

            entity.ToTable("sample_run");

            entity.HasIndex(e => e.SnapshotId, "idx_sample_run_snapshot_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.SchemaVersion)
                .HasMaxLength(200)
                .HasColumnName("schema_version");
            entity.Property(e => e.SnapshotId).HasColumnName("snapshot_id");
            entity.Property(e => e.TimestampUtc).HasColumnName("timestamp_utc");

            entity.HasOne(d => d.Snapshot).WithMany(p => p.SampleRuns)
                .HasForeignKey(d => d.SnapshotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sample_run_snapshot");
        });

        modelBuilder.Entity<SnapshotDiff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__snapshot__3213E83FEB70F191");

            entity.ToTable("snapshot_diff");

            entity.HasIndex(e => e.NewSnapshotId, "idx_snapshot_diff_new_snapshot_id");

            entity.HasIndex(e => e.OldSnapshotId, "idx_snapshot_diff_old_snapshot_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.NewSnapshotId).HasColumnName("new_snapshot_id");
            entity.Property(e => e.OldSnapshotId).HasColumnName("old_snapshot_id");
            entity.Property(e => e.SchemaVersion)
                .HasMaxLength(200)
                .HasColumnName("schema_version");
            entity.Property(e => e.TimestampUtc).HasColumnName("timestamp_utc");

            entity.HasOne(d => d.NewSnapshot).WithMany(p => p.SnapshotDiffNewSnapshots)
                .HasForeignKey(d => d.NewSnapshotId)
                .HasConstraintName("fk_snapshot_diff_new");

            entity.HasOne(d => d.OldSnapshot).WithMany(p => p.SnapshotDiffOldSnapshots)
                .HasForeignKey(d => d.OldSnapshotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_snapshot_diff_old");
        });

        modelBuilder.Entity<SourceSnapshot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__source_s__3213E83FBAC27F73");

            entity.ToTable("source_snapshot");

            entity.HasIndex(e => e.IngestionRunId, "idx_source_snapshot_ingestion_run_id");

            entity.HasIndex(e => e.SnapshotUid, "uq_source_snapshot_uid").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Branch)
                .HasMaxLength(200)
                .HasColumnName("branch");
            entity.Property(e => e.ConfigJson).HasColumnName("config_json");
            entity.Property(e => e.IngestionRunId).HasColumnName("ingestion_run_id");
            entity.Property(e => e.Language)
                .HasMaxLength(200)
                .HasColumnName("language");
            entity.Property(e => e.PackageName)
                .HasMaxLength(200)
                .HasColumnName("package_name");
            entity.Property(e => e.PackageVersion)
                .HasMaxLength(200)
                .HasColumnName("package_version");
            entity.Property(e => e.RepoUrl).HasColumnName("repo_url");
            entity.Property(e => e.Repocommit)
                .HasMaxLength(200)
                .HasColumnName("repocommit");
            entity.Property(e => e.SnapshotUid)
                .HasMaxLength(200)
                .HasColumnName("snapshot_uid");

            entity.HasOne(d => d.IngestionRun).WithMany(p => p.SourceSnapshots)
                .HasForeignKey(d => d.IngestionRunId)
                .HasConstraintName("fk_source_snapshot_ingestion_run");
        });

        modelBuilder.Entity<TruthRun>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__truth_ru__3213E83F8416BB1A");

            entity.ToTable("truth_run");

            entity.HasIndex(e => e.SnapshotId, "idx_truth_run_snapshot_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.SchemaVersion)
                .HasMaxLength(200)
                .HasColumnName("schema_version");
            entity.Property(e => e.SnapshotId).HasColumnName("snapshot_id");
            entity.Property(e => e.TimestampUtc).HasColumnName("timestamp_utc");

            entity.HasOne(d => d.Snapshot).WithMany(p => p.TruthRuns)
                .HasForeignKey(d => d.SnapshotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_truth_run_snapshot");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
