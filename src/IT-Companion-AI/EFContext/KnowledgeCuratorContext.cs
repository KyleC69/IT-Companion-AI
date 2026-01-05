using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

public partial class KnowledgeCuratorContext : DbContext
{
    public KnowledgeCuratorContext()
    {
    }

    public KnowledgeCuratorContext(DbContextOptions<KnowledgeCuratorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<api_feature> api_features { get; set; }

    public virtual DbSet<api_member> api_members { get; set; }

    public virtual DbSet<api_member_diff> api_member_diffs { get; set; }

    public virtual DbSet<api_parameter> api_parameters { get; set; }

    public virtual DbSet<api_type> api_types { get; set; }

    public virtual DbSet<api_type_diff> api_type_diffs { get; set; }

    public virtual DbSet<code_block> code_blocks { get; set; }

    public virtual DbSet<doc_page> doc_pages { get; set; }

    public virtual DbSet<doc_page_diff> doc_page_diffs { get; set; }

    public virtual DbSet<doc_section> doc_sections { get; set; }

    public virtual DbSet<execution_result> execution_results { get; set; }

    public virtual DbSet<execution_run> execution_runs { get; set; }

    public virtual DbSet<feature_doc_link> feature_doc_links { get; set; }

    public virtual DbSet<feature_member_link> feature_member_links { get; set; }

    public virtual DbSet<feature_type_link> feature_type_links { get; set; }

    public virtual DbSet<ingestion_run> ingestion_runs { get; set; }

    public virtual DbSet<rag_chunk> rag_chunks { get; set; }

    public virtual DbSet<rag_run> rag_runs { get; set; }

    public virtual DbSet<review_issue> review_issues { get; set; }

    public virtual DbSet<review_item> review_items { get; set; }

    public virtual DbSet<review_run> review_runs { get; set; }

    public virtual DbSet<sample> samples { get; set; }

    public virtual DbSet<sample_api_member_link> sample_api_member_links { get; set; }

    public virtual DbSet<sample_run> sample_runs { get; set; }

    public virtual DbSet<semantic_identity> semantic_identities { get; set; }

    public virtual DbSet<snapshot_diff> snapshot_diffs { get; set; }

    public virtual DbSet<source_snapshot> source_snapshots { get; set; }

    public virtual DbSet<truth_run> truth_runs { get; set; }

    public virtual DbSet<v_api_feature_current> v_api_feature_currents { get; set; }

    public virtual DbSet<v_api_member_current> v_api_member_currents { get; set; }

    public virtual DbSet<v_api_type_current> v_api_type_currents { get; set; }

    public virtual DbSet<v_doc_page_current> v_doc_page_currents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=KnowledgeCurator");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<api_feature>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_api_feature");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_api_feature_id");
            entity.Property(e => e.content_hash).IsFixedLength();
            entity.Property(e => e.is_active).HasDefaultValue(true, "df_api_feature_is_active");
            entity.Property(e => e.semantic_uid_hash)
                .HasComputedColumnSql("(CONVERT([binary](32),hashbytes('SHA2_256',[semantic_uid])))", true)
                .IsFixedLength();

            entity.HasOne(d => d.created_ingestion_run).WithMany(p => p.api_featurecreated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_feature_created_ingestion_run");

            entity.HasOne(d => d.removed_ingestion_run).WithMany(p => p.api_featureremoved_ingestion_runs)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_api_feature_removed_ingestion_run");

            entity.HasOne(d => d.semantic_uid_hashNavigation).WithMany(p => p.api_features).HasConstraintName("fk_api_feature_semantic_identity");

            entity.HasOne(d => d.truth_run).WithMany(p => p.api_features).HasConstraintName("fk_api_feature_truth_run");

            entity.HasOne(d => d.updated_ingestion_run).WithMany(p => p.api_featureupdated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_feature_updated_ingestion_run");
        });

        modelBuilder.Entity<api_member>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_api_member");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_api_member_id");
            entity.Property(e => e.content_hash).IsFixedLength();
            entity.Property(e => e.is_active).HasDefaultValue(true, "df_api_member_is_active");
            entity.Property(e => e.member_uid_hash).IsFixedLength();
            entity.Property(e => e.semantic_uid_hash)
                .HasComputedColumnSql("(CONVERT([binary](32),hashbytes('SHA2_256',[semantic_uid])))", true)
                .IsFixedLength();

            entity.HasOne(d => d.api_type).WithMany(p => p.api_members)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_member_type");

            entity.HasOne(d => d.created_ingestion_run).WithMany(p => p.api_membercreated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_member_created_ingestion_run");

            entity.HasOne(d => d.removed_ingestion_run).WithMany(p => p.api_memberremoved_ingestion_runs)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_api_member_removed_ingestion_run");

            entity.HasOne(d => d.semantic_uid_hashNavigation).WithMany(p => p.api_members).HasConstraintName("fk_api_member_semantic_identity");

            entity.HasOne(d => d.updated_ingestion_run).WithMany(p => p.api_memberupdated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_member_updated_ingestion_run");
        });

        modelBuilder.Entity<api_member_diff>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_api_member_diff");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_api_member_diff_id");

            entity.HasOne(d => d.snapshot_diff).WithMany(p => p.api_member_diffs).HasConstraintName("fk_api_member_diff_snapshot");
        });

        modelBuilder.Entity<api_parameter>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_api_parameter");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_api_parameter_id");
            entity.Property(e => e.content_hash).IsFixedLength();
            entity.Property(e => e.is_active).HasDefaultValue(true, "df_api_parameter_is_active");

            entity.HasOne(d => d.api_member).WithMany(p => p.api_parameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_parameter_member");

            entity.HasOne(d => d.created_ingestion_run).WithMany(p => p.api_parametercreated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_parameter_created_ingestion_run");

            entity.HasOne(d => d.removed_ingestion_run).WithMany(p => p.api_parameterremoved_ingestion_runs).HasConstraintName("fk_api_parameter_removed_ingestion_run");

            entity.HasOne(d => d.updated_ingestion_run).WithMany(p => p.api_parameterupdated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_parameter_updated_ingestion_run");
        });

        modelBuilder.Entity<api_type>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_api_type");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_api_type_id");
            entity.Property(e => e.content_hash).IsFixedLength();
            entity.Property(e => e.is_active).HasDefaultValue(true, "df_api_type_is_active");
            entity.Property(e => e.semantic_uid_hash)
                .HasComputedColumnSql("(CONVERT([binary](32),hashbytes('SHA2_256',[semantic_uid])))", true)
                .IsFixedLength();

            entity.HasOne(d => d.created_ingestion_run).WithMany(p => p.api_typecreated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_type_created_ingestion_run");

            entity.HasOne(d => d.removed_ingestion_run).WithMany(p => p.api_typeremoved_ingestion_runs).HasConstraintName("fk_api_type_removed_ingestion_run");

            entity.HasOne(d => d.semantic_uid_hashNavigation).WithMany(p => p.api_types).HasConstraintName("fk_api_type_semantic_identity");

            entity.HasOne(d => d.source_snapshot).WithMany(p => p.api_types)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_type_snapshot");

            entity.HasOne(d => d.updated_ingestion_run).WithMany(p => p.api_typeupdated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_type_updated_ingestion_run");
        });

        modelBuilder.Entity<api_type_diff>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_api_type_diff");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_api_type_diff_id");

            entity.HasOne(d => d.snapshot_diff).WithMany(p => p.api_type_diffs).HasConstraintName("fk_api_type_diff_snapshot");
        });

        modelBuilder.Entity<code_block>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_code_block");

            entity.HasIndex(e => new { e.semantic_uid, e.version_number }, "ix_code_block_semantic_version")
                .IsUnique()
                .HasFilter("([semantic_uid] IS NOT NULL)");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_code_block_id");
            entity.Property(e => e.content_hash).IsFixedLength();
            entity.Property(e => e.is_active).HasDefaultValue(true, "df_code_block_is_active");

            entity.HasOne(d => d.created_ingestion_run).WithMany(p => p.code_blockcreated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_code_block_created_ingestion_run");

            entity.HasOne(d => d.doc_section).WithMany(p => p.code_blocks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_code_block_section");

            entity.HasOne(d => d.removed_ingestion_run).WithMany(p => p.code_blockremoved_ingestion_runs).HasConstraintName("fk_code_block_removed_ingestion_run");

            entity.HasOne(d => d.updated_ingestion_run).WithMany(p => p.code_blockupdated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_code_block_updated_ingestion_run");
        });

        modelBuilder.Entity<doc_page>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_doc_page");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_doc_page_id");
            entity.Property(e => e.content_hash).IsFixedLength();
            entity.Property(e => e.is_active).HasDefaultValue(true, "df_doc_page_is_active");
            entity.Property(e => e.semantic_uid_hash)
                .HasComputedColumnSql("(CONVERT([binary](32),hashbytes('SHA2_256',[semantic_uid])))", true)
                .IsFixedLength();

            entity.HasOne(d => d.created_ingestion_run).WithMany(p => p.doc_pagecreated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doc_page_created_ingestion_run");

            entity.HasOne(d => d.removed_ingestion_run).WithMany(p => p.doc_pageremoved_ingestion_runs).HasConstraintName("fk_doc_page_removed_ingestion_run");

            entity.HasOne(d => d.semantic_uid_hashNavigation).WithMany(p => p.doc_pages).HasConstraintName("fk_doc_page_semantic_identity");

            entity.HasOne(d => d.source_snapshot).WithMany(p => p.doc_pages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doc_page_snapshot");

            entity.HasOne(d => d.updated_ingestion_run).WithMany(p => p.doc_pageupdated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doc_page_updated_ingestion_run");
        });

        modelBuilder.Entity<doc_page_diff>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_doc_page_diff");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_doc_page_diff_id");

            entity.HasOne(d => d.snapshot_diff).WithMany(p => p.doc_page_diffs).HasConstraintName("fk_doc_page_diff_snapshot");
        });

        modelBuilder.Entity<doc_section>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_doc_section");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_doc_section_id");
            entity.Property(e => e.content_hash).IsFixedLength();
            entity.Property(e => e.is_active).HasDefaultValue(true, "df_doc_section_is_active");
            entity.Property(e => e.semantic_uid_hash)
                .HasComputedColumnSql("(CONVERT([binary](32),hashbytes('SHA2_256',[semantic_uid])))", true)
                .IsFixedLength();

            entity.HasOne(d => d.created_ingestion_run).WithMany(p => p.doc_sectioncreated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doc_section_created_ingestion_run");

            entity.HasOne(d => d.doc_page).WithMany(p => p.doc_sections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doc_section_page");

            entity.HasOne(d => d.removed_ingestion_run).WithMany(p => p.doc_sectionremoved_ingestion_runs).HasConstraintName("fk_doc_section_removed_ingestion_run");

            entity.HasOne(d => d.semantic_uid_hashNavigation).WithMany(p => p.doc_sections).HasConstraintName("fk_doc_section_semantic_identity");

            entity.HasOne(d => d.updated_ingestion_run).WithMany(p => p.doc_sectionupdated_ingestion_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doc_section_updated_ingestion_run");
        });

        modelBuilder.Entity<execution_result>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_execution_result");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_execution_result_id");

            entity.HasOne(d => d.execution_run).WithMany(p => p.execution_results).HasConstraintName("fk_execution_result_run");
        });

        modelBuilder.Entity<execution_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_execution_run");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_execution_run_id");

            entity.HasOne(d => d.sample_run).WithMany(p => p.execution_runs).HasConstraintName("fk_execution_run_sample_run");

            entity.HasOne(d => d.snapshot).WithMany(p => p.execution_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_execution_run_snapshot");
        });

        modelBuilder.Entity<feature_doc_link>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_feature_doc_link");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_feature_doc_link_id");

            entity.HasOne(d => d.feature).WithMany(p => p.feature_doc_links).HasConstraintName("fk_feature_doc_link_feature");
        });

        modelBuilder.Entity<feature_member_link>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_feature_member_link");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_feature_member_link_id");

            entity.HasOne(d => d.feature).WithMany(p => p.feature_member_links).HasConstraintName("fk_feature_member_link_feature");
        });

        modelBuilder.Entity<feature_type_link>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_feature_type_link");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_feature_type_link_id");

            entity.HasOne(d => d.feature).WithMany(p => p.feature_type_links).HasConstraintName("fk_feature_type_link_feature");
        });

        modelBuilder.Entity<ingestion_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_ingestion_run");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_ingestion_run_id");
        });

        modelBuilder.Entity<rag_chunk>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_rag_chunk");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_rag_chunk_id");

            entity.HasOne(d => d.rag_run).WithMany(p => p.rag_chunks).HasConstraintName("fk_rag_chunk_run");
        });

        modelBuilder.Entity<rag_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_rag_run");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_rag_run_id");

            entity.HasOne(d => d.snapshot).WithMany(p => p.rag_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rag_run_snapshot");
        });

        modelBuilder.Entity<review_issue>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_review_issue");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_review_issue_id");

            entity.HasOne(d => d.review_item).WithMany(p => p.review_issues).HasConstraintName("fk_review_issue_item");
        });

        modelBuilder.Entity<review_item>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_review_item");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_review_item_id");

            entity.HasOne(d => d.review_run).WithMany(p => p.review_items).HasConstraintName("fk_review_item_run");
        });

        modelBuilder.Entity<review_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_review_run");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_review_run_id");

            entity.HasOne(d => d.snapshot).WithMany(p => p.review_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_run_snapshot");
        });

        modelBuilder.Entity<sample>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_sample");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_sample_id");

            entity.HasOne(d => d.sample_run).WithMany(p => p.samples).HasConstraintName("fk_sample_sample_run");
        });

        modelBuilder.Entity<sample_api_member_link>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_sample_api_member_link");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_sample_api_member_link_id");

            entity.HasOne(d => d.sample).WithMany(p => p.sample_api_member_links).HasConstraintName("fk_sample_api_member_link_sample");
        });

        modelBuilder.Entity<sample_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_sample_run");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_sample_run_id");

            entity.HasOne(d => d.snapshot).WithMany(p => p.sample_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sample_run_snapshot");
        });

        modelBuilder.Entity<semantic_identity>(entity =>
        {
            entity.HasKey(e => e.uid_hash).HasName("pk_semantic_identity");

            entity.Property(e => e.uid_hash)
                .HasComputedColumnSql("(CONVERT([binary](32),hashbytes('SHA2_256',[uid])))", true)
                .IsFixedLength();
        });

        modelBuilder.Entity<snapshot_diff>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_snapshot_diff");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_snapshot_diff_id");

            entity.HasOne(d => d.new_snapshot).WithMany(p => p.snapshot_diffnew_snapshots).HasConstraintName("fk_snapshot_diff_new");

            entity.HasOne(d => d.old_snapshot).WithMany(p => p.snapshot_diffold_snapshots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_snapshot_diff_old");
        });

        modelBuilder.Entity<source_snapshot>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_source_snapshot");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_source_snapshot_id");
            entity.Property(e => e.snapshot_uid_hash)
                .HasComputedColumnSql("(CONVERT([binary](32),hashbytes('SHA2_256',[snapshot_uid])))", true)
                .IsFixedLength();

            entity.HasOne(d => d.ingestion_run).WithMany(p => p.source_snapshots).HasConstraintName("fk_source_snapshot_ingestion_run");
        });

        modelBuilder.Entity<truth_run>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_truth_run");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())", "df_truth_run_id");

            entity.HasOne(d => d.snapshot).WithMany(p => p.truth_runs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_truth_run_snapshot");
        });

        modelBuilder.Entity<v_api_feature_current>(entity =>
        {
            entity.ToView("v_api_feature_current");

            entity.Property(e => e.content_hash).IsFixedLength();
            entity.Property(e => e.semantic_uid_hash).IsFixedLength();
        });

        modelBuilder.Entity<v_api_member_current>(entity =>
        {
            entity.ToView("v_api_member_current");

            entity.Property(e => e.content_hash).IsFixedLength();
            entity.Property(e => e.member_uid_hash).IsFixedLength();
            entity.Property(e => e.semantic_uid_hash).IsFixedLength();
        });

        modelBuilder.Entity<v_api_type_current>(entity =>
        {
            entity.ToView("v_api_type_current");

            entity.Property(e => e.content_hash).IsFixedLength();
            entity.Property(e => e.semantic_uid_hash).IsFixedLength();
        });

        modelBuilder.Entity<v_doc_page_current>(entity =>
        {
            entity.ToView("v_doc_page_current");

            entity.Property(e => e.content_hash).IsFixedLength();
            entity.Property(e => e.semantic_uid_hash).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
