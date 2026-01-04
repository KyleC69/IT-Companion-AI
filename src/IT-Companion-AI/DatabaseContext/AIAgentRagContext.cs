// Project Name: SKAgent
// File Name: AIAgentRagContext.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using ITCompanionAI.AIVectorDb;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.DatabaseContext;


public partial class AIAgentRagContext : DbContext
{
    public AIAgentRagContext()
    {
    }





    public AIAgentRagContext(DbContextOptions<AIAgentRagContext> options)
        : base(options)
    {
    }





    public virtual DbSet<api_member> api_members { get; set; }

    public virtual DbSet<api_member_diff> api_member_diffs { get; set; }

    public virtual DbSet<api_member_doc_link> api_member_doc_links { get; set; }

    public virtual DbSet<api_parameter> api_parameters { get; set; }

    public virtual DbSet<api_type> api_types { get; set; }

    public virtual DbSet<api_type_diff> api_type_diffs { get; set; }

    public virtual DbSet<code_block> code_blocks { get; set; }

    public virtual DbSet<doc_page> doc_pages { get; set; }

    public virtual DbSet<doc_page_diff> doc_page_diffs { get; set; }

    public virtual DbSet<doc_section> doc_sections { get; set; }

    public virtual DbSet<execution_result> execution_results { get; set; }

    public virtual DbSet<execution_run> execution_runs { get; set; }

    public virtual DbSet<api_feature> features { get; set; }

    public virtual DbSet<feature_doc_link> feature_doc_links { get; set; }

    public virtual DbSet<feature_member_link> feature_member_links { get; set; }

    public virtual DbSet<feature_type_link> feature_type_links { get; set; }

    public virtual DbSet<ingestion_run> ingestion_runs { get; set; }

    public virtual DbSet<rag_chunk> rag_chunks { get; set; }

    public virtual DbSet<rag_run> rag_runs { get; set; }

    public virtual DbSet<review_issue> review_issues { get; set; }

    public virtual DbSet<review_item> review_items { get; set; }

    public virtual DbSet<review_run> review_runs { get; set; }

    public virtual DbSet<code_sample> samples { get; set; }

    public virtual DbSet<sample_api_member_link> sample_api_member_links { get; set; }

    public virtual DbSet<sample_run> sample_runs { get; set; }

    public virtual DbSet<snapshot_diff> snapshot_diffs { get; set; }

    public virtual DbSet<source_snapshot> source_snapshots { get; set; }

    public virtual DbSet<truth_run> truth_runs { get; set; }





    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=AIAgentRag");
    }





    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<api_member>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__api_memb__3213E83FC0360325");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.member_uid_hash).IsFixedLength();

            entity.HasOne(d => d.api_type).WithMany(p => p.api_members).HasConstraintName("fk_api_member_type");
        });

        modelBuilder.Entity<api_member_diff>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__api_memb__3213E83FE1BC3A11");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.snapshot_diff).WithMany(p => p.api_member_diffs)
                .HasConstraintName("fk_api_member_diff_snapshot");
        });

        modelBuilder.Entity<api_member_doc_link>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__api_memb__3213E83F4D70D527");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.api_member).WithMany(p => p.api_member_doc_links)
                .HasConstraintName("fk_api_member_doc_link_member");
        });

        modelBuilder.Entity<api_parameter>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__api_para__3213E83FD564D384");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.api_member).WithMany(p => p.api_parameters)
                .HasConstraintName("fk_api_parameter_member");
        });

        modelBuilder.Entity<api_type>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__api_type__3213E83F6AA70338");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.type_uid_hash).IsFixedLength();

            entity.HasOne(d => d.source_snapshot).WithMany(p => p.api_types).HasConstraintName("fk_api_type_snapshot");
        });

        modelBuilder.Entity<api_type_diff>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__api_type__3213E83FAC92C6E2");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.snapshot_diff).WithMany(p => p.api_type_diffs)
                .HasConstraintName("fk_api_type_diff_snapshot");
        });

        modelBuilder.Entity<code_block>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__code_blo__3213E83F46B4AFE4");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.doc_section).WithMany(p => p.code_blocks).HasConstraintName("fk_code_block_section");
        });

        modelBuilder.Entity<doc_page>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__doc_page__3213E83FE0C9B7E7");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.source_snapshot).WithMany(p => p.doc_pages).HasConstraintName("fk_doc_page_snapshot");
        });

        modelBuilder.Entity<doc_page_diff>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__doc_page__3213E83FE0C2C243");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.snapshot_diff).WithMany(p => p.doc_page_diffs)
                .HasConstraintName("fk_doc_page_diff_snapshot");
        });

        modelBuilder.Entity<doc_section>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__doc_sect__3213E83FD263E919");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.doc_page).WithMany(p => p.doc_sections).HasConstraintName("fk_doc_section_page");
        });

        modelBuilder.Entity<execution_result>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__executio__3213E83F46EC27DF");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.execution_run).WithMany(p => p.execution_results)
                .HasConstraintName("fk_execution_result_run");
        });

        modelBuilder.Entity<execution_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__executio__3213E83FF0E8A78F");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.sample_run).WithMany(p => p.execution_runs)
                .HasConstraintName("fk_execution_run_sample_run");

            entity.HasOne(d => d.snapshot).WithMany(p => p.execution_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_execution_run_snapshot");
        });

        modelBuilder.Entity<api_feature>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__feature__3213E83FFA46D494");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.truth_run).WithMany(p => p.features).HasConstraintName("fk_feature_truth_run");
        });

        modelBuilder.Entity<feature_doc_link>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__feature___3213E83FD0E43EE8");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.feature).WithMany(p => p.feature_doc_links)
                .HasConstraintName("fk_feature_doc_link_feature");
        });

        modelBuilder.Entity<feature_member_link>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__feature___3213E83F6F14730F");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.feature).WithMany(p => p.feature_member_links)
                .HasConstraintName("fk_feature_member_link_feature");
        });

        modelBuilder.Entity<feature_type_link>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__feature___3213E83FAA699823");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.feature).WithMany(p => p.feature_type_links)
                .HasConstraintName("fk_feature_type_link_feature");
        });

        modelBuilder.Entity<ingestion_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__ingestio__3213E83F14318750");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<rag_chunk>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__rag_chun__3213E83F4B205D76");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.rag_run).WithMany(p => p.rag_chunks).HasConstraintName("fk_rag_chunk_run");
        });

        modelBuilder.Entity<rag_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__rag_run__3213E83F5E821252");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.snapshot).WithMany(p => p.rag_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rag_run_snapshot");
        });

        modelBuilder.Entity<review_issue>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__review_i__3213E83FCC425009");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.review_item).WithMany(p => p.review_issues).HasConstraintName("fk_review_issue_item");
        });

        modelBuilder.Entity<review_item>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__review_i__3213E83FCC2BE863");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.review_run).WithMany(p => p.review_items).HasConstraintName("fk_review_item_run");
        });

        modelBuilder.Entity<review_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__review_r__3213E83F19EC2413");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.snapshot).WithMany(p => p.review_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_run_snapshot");
        });

        modelBuilder.Entity<code_sample>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__sample__3213E83F0191AD1B");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.sample_run).WithMany(p => p.samples).HasConstraintName("fk_sample_sample_run");
        });

        modelBuilder.Entity<sample_api_member_link>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__sample_a__3213E83F8A74C56D");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.sample).WithMany(p => p.sample_api_member_links)
                .HasConstraintName("fk_sample_api_member_link_sample");
        });

        modelBuilder.Entity<sample_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__sample_r__3213E83FF2EB5FE7");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.snapshot).WithMany(p => p.sample_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sample_run_snapshot");
        });

        modelBuilder.Entity<snapshot_diff>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__snapshot__3213E83F182DB545");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.new_snapshot).WithMany(p => p.snapshot_diffnew_snapshots)
                .HasConstraintName("fk_snapshot_diff_new");

            entity.HasOne(d => d.old_snapshot).WithMany(p => p.snapshot_diffold_snapshots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_snapshot_diff_old");
        });

        modelBuilder.Entity<source_snapshot>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__source_s__3213E83F054CCFE8");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.ingestion_run).WithMany(p => p.source_snapshots)
                .HasConstraintName("fk_source_snapshot_ingestion_run");
        });

        modelBuilder.Entity<truth_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__truth_ru__3213E83F384479AF");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.snapshot).WithMany(p => p.truth_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_truth_run_snapshot");
        });

        OnModelCreatingPartial(modelBuilder);
    }





    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}