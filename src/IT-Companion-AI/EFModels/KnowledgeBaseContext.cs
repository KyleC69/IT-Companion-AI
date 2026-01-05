// Project Name: SKAgent
// File Name: KnowledgeBaseContext.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Data;
using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace ITCompanionAI.KnowledgeBase;


public partial class KnowledgeBaseContext : DbContext
{
    public KnowledgeBaseContext()
    {
        OnCreated();
    }







    public KnowledgeBaseContext(DbContextOptions<KnowledgeBaseContext> options) :
        base(options)
    {
        OnCreated();
    }







    public virtual DbSet<ApiFeature> ApiFeatures { get; set; }

    public virtual DbSet<ApiMember> ApiMembers { get; set; }

    public virtual DbSet<ApiMemberDiff> ApiMemberDiffs { get; set; }

    public virtual DbSet<ApiParameter> ApiParameters { get; set; }

    public virtual DbSet<ApiType> ApiTypes { get; set; }

    public virtual DbSet<ApiTypeDiff> ApiTypeDiffs { get; set; }

    public virtual DbSet<CodeBlock> CodeBlocks { get; set; }

    public virtual DbSet<DocPage> DocPages { get; set; }

    public virtual DbSet<DocPageDiff> DocPageDiffs { get; set; }

    public virtual DbSet<DocSection> DocSections { get; set; }

    public virtual DbSet<ExecutionResult> ExecutionResults { get; set; }

    public virtual DbSet<ExecutionRun> ExecutionRuns { get; set; }

    public virtual DbSet<FeatureDocLink> FeatureDocLinks { get; set; }

    public virtual DbSet<FeatureMemberLink> FeatureMemberLinks { get; set; }

    public virtual DbSet<FeatureTypeLink> FeatureTypeLinks { get; set; }

    public virtual DbSet<IngestionRun> IngestionRuns { get; set; }

    public virtual DbSet<RagChunk> RagChunks { get; set; }

    public virtual DbSet<RagRun> RagRuns { get; set; }

    public virtual DbSet<ReviewIssue> ReviewIssues { get; set; }

    public virtual DbSet<ReviewItem> ReviewItems { get; set; }

    public virtual DbSet<ReviewRun> ReviewRuns { get; set; }

    public virtual DbSet<Sample> Samples { get; set; }

    public virtual DbSet<SampleApiMemberLink> SampleApiMemberLinks { get; set; }

    public virtual DbSet<SampleRun> SampleRuns { get; set; }

    public virtual DbSet<SemanticIdentity> SemanticIdentities { get; set; }

    public virtual DbSet<SnapshotDiff> SnapshotDiffs { get; set; }

    public virtual DbSet<SourceSnapshot> SourceSnapshots { get; set; }

    public virtual DbSet<TruthRun> TruthRuns { get; set; }

    public virtual DbSet<VApiFeatureCurrent> VApiFeatureCurrents { get; set; }

    public virtual DbSet<VApiMemberCurrent> VApiMemberCurrents { get; set; }

    public virtual DbSet<VApiTypeCurrent> VApiTypeCurrents { get; set; }

    public virtual DbSet<VDocPageCurrent> VDocPageCurrents { get; set; }







    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured ||
            (!optionsBuilder.Options.Extensions.OfType<RelationalOptionsExtension>().Any(ext => !string.IsNullOrEmpty(ext.ConnectionString) || ext.Connection != null) &&
             !optionsBuilder.Options.Extensions.Any(ext => !(ext is RelationalOptionsExtension) && !(ext is CoreOptionsExtension))))
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=KnowledgeCurator;Integrated Security=True;Persist Security Info=True");
        }

        CustomizeConfiguration(ref optionsBuilder);
        base.OnConfiguring(optionsBuilder);
    }







    partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);







    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ApiFeatureMapping(modelBuilder);
        CustomizeApiFeatureMapping(modelBuilder);

        ApiMemberMapping(modelBuilder);
        CustomizeApiMemberMapping(modelBuilder);

        ApiMemberDiffMapping(modelBuilder);
        CustomizeApiMemberDiffMapping(modelBuilder);

        ApiParameterMapping(modelBuilder);
        CustomizeApiParameterMapping(modelBuilder);

        ApiTypeMapping(modelBuilder);
        CustomizeApiTypeMapping(modelBuilder);

        ApiTypeDiffMapping(modelBuilder);
        CustomizeApiTypeDiffMapping(modelBuilder);

        CodeBlockMapping(modelBuilder);
        CustomizeCodeBlockMapping(modelBuilder);

        DocPageMapping(modelBuilder);
        CustomizeDocPageMapping(modelBuilder);

        DocPageDiffMapping(modelBuilder);
        CustomizeDocPageDiffMapping(modelBuilder);

        DocSectionMapping(modelBuilder);
        CustomizeDocSectionMapping(modelBuilder);

        ExecutionResultMapping(modelBuilder);
        CustomizeExecutionResultMapping(modelBuilder);

        ExecutionRunMapping(modelBuilder);
        CustomizeExecutionRunMapping(modelBuilder);

        FeatureDocLinkMapping(modelBuilder);
        CustomizeFeatureDocLinkMapping(modelBuilder);

        FeatureMemberLinkMapping(modelBuilder);
        CustomizeFeatureMemberLinkMapping(modelBuilder);

        FeatureTypeLinkMapping(modelBuilder);
        CustomizeFeatureTypeLinkMapping(modelBuilder);

        IngestionRunMapping(modelBuilder);
        CustomizeIngestionRunMapping(modelBuilder);

        RagChunkMapping(modelBuilder);
        CustomizeRagChunkMapping(modelBuilder);

        RagRunMapping(modelBuilder);
        CustomizeRagRunMapping(modelBuilder);

        ReviewIssueMapping(modelBuilder);
        CustomizeReviewIssueMapping(modelBuilder);

        ReviewItemMapping(modelBuilder);
        CustomizeReviewItemMapping(modelBuilder);

        ReviewRunMapping(modelBuilder);
        CustomizeReviewRunMapping(modelBuilder);

        SampleMapping(modelBuilder);
        CustomizeSampleMapping(modelBuilder);

        SampleApiMemberLinkMapping(modelBuilder);
        CustomizeSampleApiMemberLinkMapping(modelBuilder);

        SampleRunMapping(modelBuilder);
        CustomizeSampleRunMapping(modelBuilder);

        SemanticIdentityMapping(modelBuilder);
        CustomizeSemanticIdentityMapping(modelBuilder);

        SnapshotDiffMapping(modelBuilder);
        CustomizeSnapshotDiffMapping(modelBuilder);

        SourceSnapshotMapping(modelBuilder);
        CustomizeSourceSnapshotMapping(modelBuilder);

        TruthRunMapping(modelBuilder);
        CustomizeTruthRunMapping(modelBuilder);

        VApiFeatureCurrentMapping(modelBuilder);
        CustomizeVApiFeatureCurrentMapping(modelBuilder);

        VApiMemberCurrentMapping(modelBuilder);
        CustomizeVApiMemberCurrentMapping(modelBuilder);

        VApiTypeCurrentMapping(modelBuilder);
        CustomizeVApiTypeCurrentMapping(modelBuilder);

        VDocPageCurrentMapping(modelBuilder);
        CustomizeVDocPageCurrentMapping(modelBuilder);

        RelationshipsMapping(modelBuilder);
        CustomizeMapping(ref modelBuilder);
    }







    private void RelationshipsMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiFeature>().HasOne(x => x.TruthRun_TruthRunId).WithMany(op => op.ApiFeatures_TruthRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"TruthRunId").IsRequired();
        modelBuilder.Entity<ApiFeature>().HasOne(x => x.IngestionRun_CreatedIngestionRunId).WithMany(op => op.ApiFeatures_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<ApiFeature>().HasOne(x => x.IngestionRun_UpdatedIngestionRunId).WithMany(op => op.ApiFeatures_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<ApiFeature>().HasOne(x => x.IngestionRun_RemovedIngestionRunId).WithMany(op => op.ApiFeatures_RemovedIngestionRunId).OnDelete(DeleteBehavior.SetNull).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);
        modelBuilder.Entity<ApiFeature>().HasOne(x => x.SemanticIdentity_SemanticUidHash).WithMany(op => op.ApiFeatures_SemanticUidHash).HasForeignKey(@"SemanticUidHash").IsRequired(false);
        modelBuilder.Entity<ApiFeature>().HasMany(x => x.FeatureDocLinks_FeatureId).WithOne(op => op.ApiFeature_FeatureId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"FeatureId").IsRequired();
        modelBuilder.Entity<ApiFeature>().HasMany(x => x.FeatureMemberLinks_FeatureId).WithOne(op => op.ApiFeature_FeatureId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"FeatureId").IsRequired();
        modelBuilder.Entity<ApiFeature>().HasMany(x => x.FeatureTypeLinks_FeatureId).WithOne(op => op.ApiFeature_FeatureId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"FeatureId").IsRequired();

        modelBuilder.Entity<ApiMember>().HasOne(x => x.IngestionRun_CreatedIngestionRunId).WithMany(op => op.ApiMembers_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<ApiMember>().HasOne(x => x.IngestionRun_UpdatedIngestionRunId).WithMany(op => op.ApiMembers_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<ApiMember>().HasOne(x => x.IngestionRun_RemovedIngestionRunId).WithMany(op => op.ApiMembers_RemovedIngestionRunId).OnDelete(DeleteBehavior.SetNull).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);

        modelBuilder.Entity<ApiMemberDiff>().HasOne(x => x.SnapshotDiff_SnapshotDiffId).WithMany(op => op.ApiMemberDiffs_SnapshotDiffId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SnapshotDiffId").IsRequired();

        modelBuilder.Entity<ApiType>().HasOne(x => x.SourceSnapshot_SourceSnapshotId).WithMany(op => op.ApiTypes_SourceSnapshotId).HasForeignKey(@"SourceSnapshotId").IsRequired();
        modelBuilder.Entity<ApiType>().HasOne(x => x.IngestionRun_CreatedIngestionRunId).WithMany(op => op.ApiTypes_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<ApiType>().HasOne(x => x.IngestionRun_UpdatedIngestionRunId).WithMany(op => op.ApiTypes_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<ApiType>().HasOne(x => x.IngestionRun_RemovedIngestionRunId).WithMany(op => op.ApiTypes_RemovedIngestionRunId).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);

        modelBuilder.Entity<ApiTypeDiff>().HasOne(x => x.SnapshotDiff_SnapshotDiffId).WithMany(op => op.ApiTypeDiffs_SnapshotDiffId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SnapshotDiffId").IsRequired();

        modelBuilder.Entity<CodeBlock>().HasOne(x => x.DocSection_DocSectionId).WithMany(op => op.CodeBlocks_DocSectionId).HasForeignKey(@"DocSectionId").IsRequired();
        modelBuilder.Entity<CodeBlock>().HasOne(x => x.IngestionRun_CreatedIngestionRunId).WithMany(op => op.CodeBlocks_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<CodeBlock>().HasOne(x => x.IngestionRun_UpdatedIngestionRunId).WithMany(op => op.CodeBlocks_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<CodeBlock>().HasOne(x => x.IngestionRun_RemovedIngestionRunId).WithMany(op => op.CodeBlocks_RemovedIngestionRunId).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);

        modelBuilder.Entity<DocPage>().HasOne(x => x.SourceSnapshot_SourceSnapshotId).WithMany(op => op.DocPages_SourceSnapshotId).HasForeignKey(@"SourceSnapshotId").IsRequired();
        modelBuilder.Entity<DocPage>().HasOne(x => x.IngestionRun_CreatedIngestionRunId).WithMany(op => op.DocPages_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<DocPage>().HasOne(x => x.IngestionRun_UpdatedIngestionRunId).WithMany(op => op.DocPages_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<DocPage>().HasOne(x => x.IngestionRun_RemovedIngestionRunId).WithMany(op => op.DocPages_RemovedIngestionRunId).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);
        modelBuilder.Entity<DocPage>().HasOne(x => x.SemanticIdentity_SemanticUidHash).WithMany(op => op.DocPages_SemanticUidHash).HasForeignKey(@"SemanticUidHash").IsRequired(false);
        modelBuilder.Entity<DocPage>().HasMany(x => x.DocSections_DocPageId).WithOne(op => op.DocPage_DocPageId).HasForeignKey(@"DocPageId").IsRequired();

        modelBuilder.Entity<DocPageDiff>().HasOne(x => x.SnapshotDiff_SnapshotDiffId).WithMany(op => op.DocPageDiffs_SnapshotDiffId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SnapshotDiffId").IsRequired();

        modelBuilder.Entity<DocSection>().HasMany(x => x.CodeBlocks_DocSectionId).WithOne(op => op.DocSection_DocSectionId).HasForeignKey(@"DocSectionId").IsRequired();
        modelBuilder.Entity<DocSection>().HasOne(x => x.DocPage_DocPageId).WithMany(op => op.DocSections_DocPageId).HasForeignKey(@"DocPageId").IsRequired();
        modelBuilder.Entity<DocSection>().HasOne(x => x.IngestionRun_CreatedIngestionRunId).WithMany(op => op.DocSections_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<DocSection>().HasOne(x => x.IngestionRun_UpdatedIngestionRunId).WithMany(op => op.DocSections_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<DocSection>().HasOne(x => x.IngestionRun_RemovedIngestionRunId).WithMany(op => op.DocSections_RemovedIngestionRunId).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);
        modelBuilder.Entity<DocSection>().HasOne(x => x.SemanticIdentity_SemanticUidHash).WithMany(op => op.DocSections_SemanticUidHash).HasForeignKey(@"SemanticUidHash").IsRequired(false);

        modelBuilder.Entity<ExecutionResult>().HasOne(x => x.ExecutionRun_ExecutionRunId).WithMany(op => op.ExecutionResults_ExecutionRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"ExecutionRunId").IsRequired();

        modelBuilder.Entity<ExecutionRun>().HasMany(x => x.ExecutionResults_ExecutionRunId).WithOne(op => op.ExecutionRun_ExecutionRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"ExecutionRunId").IsRequired();
        modelBuilder.Entity<ExecutionRun>().HasOne(x => x.SourceSnapshot_SnapshotId).WithMany(op => op.ExecutionRuns_SnapshotId).HasForeignKey(@"SnapshotId").IsRequired();
        modelBuilder.Entity<ExecutionRun>().HasOne(x => x.SampleRun_SampleRunId).WithMany(op => op.ExecutionRuns_SampleRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SampleRunId").IsRequired();

        modelBuilder.Entity<FeatureDocLink>().HasOne(x => x.ApiFeature_FeatureId).WithMany(op => op.FeatureDocLinks_FeatureId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"FeatureId").IsRequired();

        modelBuilder.Entity<FeatureMemberLink>().HasOne(x => x.ApiFeature_FeatureId).WithMany(op => op.FeatureMemberLinks_FeatureId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"FeatureId").IsRequired();

        modelBuilder.Entity<FeatureTypeLink>().HasOne(x => x.ApiFeature_FeatureId).WithMany(op => op.FeatureTypeLinks_FeatureId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"FeatureId").IsRequired();

        modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiFeatures_CreatedIngestionRunId).WithOne(op => op.IngestionRun_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiFeatures_UpdatedIngestionRunId).WithOne(op => op.IngestionRun_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiFeatures_RemovedIngestionRunId).WithOne(op => op.IngestionRun_RemovedIngestionRunId).OnDelete(DeleteBehavior.SetNull).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiMembers_CreatedIngestionRunId).WithOne(op => op.IngestionRun_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiMembers_UpdatedIngestionRunId).WithOne(op => op.IngestionRun_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiMembers_RemovedIngestionRunId).WithOne(op => op.IngestionRun_RemovedIngestionRunId).OnDelete(DeleteBehavior.SetNull).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiTypes_CreatedIngestionRunId).WithOne(op => op.IngestionRun_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiTypes_UpdatedIngestionRunId).WithOne(op => op.IngestionRun_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiTypes_RemovedIngestionRunId).WithOne(op => op.IngestionRun_RemovedIngestionRunId).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.CodeBlocks_CreatedIngestionRunId).WithOne(op => op.IngestionRun_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.CodeBlocks_UpdatedIngestionRunId).WithOne(op => op.IngestionRun_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.CodeBlocks_RemovedIngestionRunId).WithOne(op => op.IngestionRun_RemovedIngestionRunId).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.DocPages_CreatedIngestionRunId).WithOne(op => op.IngestionRun_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.DocPages_UpdatedIngestionRunId).WithOne(op => op.IngestionRun_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.DocPages_RemovedIngestionRunId).WithOne(op => op.IngestionRun_RemovedIngestionRunId).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.DocSections_CreatedIngestionRunId).WithOne(op => op.IngestionRun_CreatedIngestionRunId).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.DocSections_UpdatedIngestionRunId).WithOne(op => op.IngestionRun_UpdatedIngestionRunId).HasForeignKey(@"UpdatedIngestionRunId").IsRequired();
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.DocSections_RemovedIngestionRunId).WithOne(op => op.IngestionRun_RemovedIngestionRunId).HasForeignKey(@"RemovedIngestionRunId").IsRequired(false);
        modelBuilder.Entity<IngestionRun>().HasMany(x => x.SourceSnapshots_IngestionRunId).WithOne(op => op.IngestionRun_IngestionRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"IngestionRunId").IsRequired();

        modelBuilder.Entity<RagChunk>().HasOne(x => x.RagRun_RagRunId).WithMany(op => op.RagChunks_RagRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"RagRunId").IsRequired();

        modelBuilder.Entity<RagRun>().HasMany(x => x.RagChunks_RagRunId).WithOne(op => op.RagRun_RagRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"RagRunId").IsRequired();
        modelBuilder.Entity<RagRun>().HasOne(x => x.SourceSnapshot_SnapshotId).WithMany(op => op.RagRuns_SnapshotId).HasForeignKey(@"SnapshotId").IsRequired();

        modelBuilder.Entity<ReviewIssue>().HasOne(x => x.ReviewItem_ReviewItemId).WithMany(op => op.ReviewIssues_ReviewItemId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"ReviewItemId").IsRequired();

        modelBuilder.Entity<ReviewItem>().HasMany(x => x.ReviewIssues_ReviewItemId).WithOne(op => op.ReviewItem_ReviewItemId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"ReviewItemId").IsRequired();
        modelBuilder.Entity<ReviewItem>().HasOne(x => x.ReviewRun_ReviewRunId).WithMany(op => op.ReviewItems_ReviewRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"ReviewRunId").IsRequired();

        modelBuilder.Entity<ReviewRun>().HasMany(x => x.ReviewItems_ReviewRunId).WithOne(op => op.ReviewRun_ReviewRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"ReviewRunId").IsRequired();
        modelBuilder.Entity<ReviewRun>().HasOne(x => x.SourceSnapshot_SnapshotId).WithMany(op => op.ReviewRuns_SnapshotId).HasForeignKey(@"SnapshotId").IsRequired();

        modelBuilder.Entity<Sample>().HasOne(x => x.SampleRun_SampleRunId).WithMany(op => op.Samples_SampleRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SampleRunId").IsRequired();
        modelBuilder.Entity<Sample>().HasMany(x => x.SampleApiMemberLinks_SampleId).WithOne(op => op.Sample_SampleId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SampleId").IsRequired();

        modelBuilder.Entity<SampleApiMemberLink>().HasOne(x => x.Sample_SampleId).WithMany(op => op.SampleApiMemberLinks_SampleId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SampleId").IsRequired();

        modelBuilder.Entity<SampleRun>().HasMany(x => x.ExecutionRuns_SampleRunId).WithOne(op => op.SampleRun_SampleRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SampleRunId").IsRequired();
        modelBuilder.Entity<SampleRun>().HasMany(x => x.Samples_SampleRunId).WithOne(op => op.SampleRun_SampleRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SampleRunId").IsRequired();
        modelBuilder.Entity<SampleRun>().HasOne(x => x.SourceSnapshot_SnapshotId).WithMany(op => op.SampleRuns_SnapshotId).HasForeignKey(@"SnapshotId").IsRequired();

        modelBuilder.Entity<SemanticIdentity>().HasMany(x => x.ApiFeatures_SemanticUidHash).WithOne(op => op.SemanticIdentity_SemanticUidHash).HasForeignKey(@"SemanticUidHash").IsRequired(false);
        modelBuilder.Entity<SemanticIdentity>().HasMany(x => x.DocPages_SemanticUidHash).WithOne(op => op.SemanticIdentity_SemanticUidHash).HasForeignKey(@"SemanticUidHash").IsRequired(false);
        modelBuilder.Entity<SemanticIdentity>().HasMany(x => x.DocSections_SemanticUidHash).WithOne(op => op.SemanticIdentity_SemanticUidHash).HasForeignKey(@"SemanticUidHash").IsRequired(false);

        modelBuilder.Entity<SnapshotDiff>().HasMany(x => x.ApiMemberDiffs_SnapshotDiffId).WithOne(op => op.SnapshotDiff_SnapshotDiffId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SnapshotDiffId").IsRequired();
        modelBuilder.Entity<SnapshotDiff>().HasMany(x => x.ApiTypeDiffs_SnapshotDiffId).WithOne(op => op.SnapshotDiff_SnapshotDiffId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SnapshotDiffId").IsRequired();
        modelBuilder.Entity<SnapshotDiff>().HasMany(x => x.DocPageDiffs_SnapshotDiffId).WithOne(op => op.SnapshotDiff_SnapshotDiffId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"SnapshotDiffId").IsRequired();
        modelBuilder.Entity<SnapshotDiff>().HasOne(x => x.SourceSnapshot_OldSnapshotId).WithMany(op => op.SnapshotDiffs_OldSnapshotId).HasForeignKey(@"OldSnapshotId").IsRequired();
        modelBuilder.Entity<SnapshotDiff>().HasOne(x => x.SourceSnapshot_NewSnapshotId).WithMany(op => op.SnapshotDiffs_NewSnapshotId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"NewSnapshotId").IsRequired();

        modelBuilder.Entity<SourceSnapshot>().HasMany(x => x.ApiTypes_SourceSnapshotId).WithOne(op => op.SourceSnapshot_SourceSnapshotId).HasForeignKey(@"SourceSnapshotId").IsRequired();
        modelBuilder.Entity<SourceSnapshot>().HasMany(x => x.DocPages_SourceSnapshotId).WithOne(op => op.SourceSnapshot_SourceSnapshotId).HasForeignKey(@"SourceSnapshotId").IsRequired();
        modelBuilder.Entity<SourceSnapshot>().HasMany(x => x.ExecutionRuns_SnapshotId).WithOne(op => op.SourceSnapshot_SnapshotId).HasForeignKey(@"SnapshotId").IsRequired();
        modelBuilder.Entity<SourceSnapshot>().HasMany(x => x.RagRuns_SnapshotId).WithOne(op => op.SourceSnapshot_SnapshotId).HasForeignKey(@"SnapshotId").IsRequired();
        modelBuilder.Entity<SourceSnapshot>().HasMany(x => x.ReviewRuns_SnapshotId).WithOne(op => op.SourceSnapshot_SnapshotId).HasForeignKey(@"SnapshotId").IsRequired();
        modelBuilder.Entity<SourceSnapshot>().HasMany(x => x.SampleRuns_SnapshotId).WithOne(op => op.SourceSnapshot_SnapshotId).HasForeignKey(@"SnapshotId").IsRequired();
        modelBuilder.Entity<SourceSnapshot>().HasMany(x => x.SnapshotDiffs_OldSnapshotId).WithOne(op => op.SourceSnapshot_OldSnapshotId).HasForeignKey(@"OldSnapshotId").IsRequired();
        modelBuilder.Entity<SourceSnapshot>().HasMany(x => x.SnapshotDiffs_NewSnapshotId).WithOne(op => op.SourceSnapshot_NewSnapshotId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"NewSnapshotId").IsRequired();
        modelBuilder.Entity<SourceSnapshot>().HasOne(x => x.IngestionRun_IngestionRunId).WithMany(op => op.SourceSnapshots_IngestionRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"IngestionRunId").IsRequired();
        modelBuilder.Entity<SourceSnapshot>().HasMany(x => x.TruthRuns_SnapshotId).WithOne(op => op.SourceSnapshot_SnapshotId).HasForeignKey(@"SnapshotId").IsRequired();

        modelBuilder.Entity<TruthRun>().HasMany(x => x.ApiFeatures_TruthRunId).WithOne(op => op.TruthRun_TruthRunId).OnDelete(DeleteBehavior.Cascade).HasForeignKey(@"TruthRunId").IsRequired();
        modelBuilder.Entity<TruthRun>().HasOne(x => x.SourceSnapshot_SnapshotId).WithMany(op => op.TruthRuns_SnapshotId).HasForeignKey(@"SnapshotId").IsRequired();
    }







    partial void CustomizeMapping(ref ModelBuilder modelBuilder);







    public bool HasChanges()
    {
        return ChangeTracker.Entries().Any(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
    }







    partial void OnCreated();

    #region Methods

    public void BeginIngestionRun(string SchemaVersion, string Notes, ref Guid? IngestionRunId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.BeginIngestionRun";

                DbParameter SchemaVersionParameter = cmd.CreateParameter();
                SchemaVersionParameter.ParameterName = "SchemaVersion";
                SchemaVersionParameter.Direction = ParameterDirection.Input;
                SchemaVersionParameter.DbType = DbType.String;
                SchemaVersionParameter.Size = 200;
                if (SchemaVersion != null)
                {
                    SchemaVersionParameter.Value = SchemaVersion;
                }
                else
                {
                    SchemaVersionParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SchemaVersionParameter);

                DbParameter NotesParameter = cmd.CreateParameter();
                NotesParameter.ParameterName = "Notes";
                NotesParameter.Direction = ParameterDirection.Input;
                NotesParameter.DbType = DbType.String;
                if (Notes != null)
                {
                    NotesParameter.Value = Notes;
                }
                else
                {
                    NotesParameter.Size = -1;
                    NotesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NotesParameter);

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.InputOutput;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);
                cmd.ExecuteNonQuery();

                if (cmd.Parameters["IngestionRunId"].Value != null && !(cmd.Parameters["IngestionRunId"].Value is DBNull))
                {
                    IngestionRunId = (Guid)Convert.ChangeType(cmd.Parameters["IngestionRunId"].Value,
                        typeof(Guid));
                }
                else
                {
                    IngestionRunId = default(Guid);
                }
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task<Tuple<Guid?>> BeginIngestionRunAsync(string SchemaVersion, string Notes, Guid? IngestionRunId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.BeginIngestionRun";

                DbParameter SchemaVersionParameter = cmd.CreateParameter();
                SchemaVersionParameter.ParameterName = "SchemaVersion";
                SchemaVersionParameter.Direction = ParameterDirection.Input;
                SchemaVersionParameter.DbType = DbType.String;
                SchemaVersionParameter.Size = 200;
                if (SchemaVersion != null)
                {
                    SchemaVersionParameter.Value = SchemaVersion;
                }
                else
                {
                    SchemaVersionParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SchemaVersionParameter);

                DbParameter NotesParameter = cmd.CreateParameter();
                NotesParameter.ParameterName = "Notes";
                NotesParameter.Direction = ParameterDirection.Input;
                NotesParameter.DbType = DbType.String;
                if (Notes != null)
                {
                    NotesParameter.Value = Notes;
                }
                else
                {
                    NotesParameter.Size = -1;
                    NotesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NotesParameter);

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.InputOutput;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);
                await cmd.ExecuteNonQueryAsync();

                if (cmd.Parameters["IngestionRunId"].Value != null && !(cmd.Parameters["IngestionRunId"].Value is DBNull))
                {
                    IngestionRunId = (Guid)Convert.ChangeType(cmd.Parameters["IngestionRunId"].Value,
                        typeof(Guid));
                }
                else
                {
                    IngestionRunId = default(Guid);
                }
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }

        return new Tuple<Guid?>(IngestionRunId);
    }







    public void CheckTemporalConsistency()
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.CheckTemporalConsistency";
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task CheckTemporalConsistencyAsync()
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.CheckTemporalConsistency";
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void CompactTypeHistory(string SemanticUid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.CompactTypeHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task CompactTypeHistoryAsync(string SemanticUid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.CompactTypeHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void CreateSourceSnapshot(Guid? IngestionRunId, string SnapshotUid, string RepoUrl, string Branch, string RepoCommit, string Language, string PackageName, string PackageVersion, string ConfigJson, ref Guid? SnapshotId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.CreateSourceSnapshot";

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter SnapshotUidParameter = cmd.CreateParameter();
                SnapshotUidParameter.ParameterName = "SnapshotUid";
                SnapshotUidParameter.Direction = ParameterDirection.Input;
                SnapshotUidParameter.DbType = DbType.String;
                SnapshotUidParameter.Size = 1000;
                if (SnapshotUid != null)
                {
                    SnapshotUidParameter.Value = SnapshotUid;
                }
                else
                {
                    SnapshotUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SnapshotUidParameter);

                DbParameter RepoUrlParameter = cmd.CreateParameter();
                RepoUrlParameter.ParameterName = "RepoUrl";
                RepoUrlParameter.Direction = ParameterDirection.Input;
                RepoUrlParameter.DbType = DbType.String;
                if (RepoUrl != null)
                {
                    RepoUrlParameter.Value = RepoUrl;
                }
                else
                {
                    RepoUrlParameter.Size = -1;
                    RepoUrlParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(RepoUrlParameter);

                DbParameter BranchParameter = cmd.CreateParameter();
                BranchParameter.ParameterName = "Branch";
                BranchParameter.Direction = ParameterDirection.Input;
                BranchParameter.DbType = DbType.String;
                BranchParameter.Size = 200;
                if (Branch != null)
                {
                    BranchParameter.Value = Branch;
                }
                else
                {
                    BranchParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(BranchParameter);

                DbParameter RepoCommitParameter = cmd.CreateParameter();
                RepoCommitParameter.ParameterName = "RepoCommit";
                RepoCommitParameter.Direction = ParameterDirection.Input;
                RepoCommitParameter.DbType = DbType.String;
                RepoCommitParameter.Size = 200;
                if (RepoCommit != null)
                {
                    RepoCommitParameter.Value = RepoCommit;
                }
                else
                {
                    RepoCommitParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(RepoCommitParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                if (Language != null)
                {
                    LanguageParameter.Value = Language;
                }
                else
                {
                    LanguageParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(LanguageParameter);

                DbParameter PackageNameParameter = cmd.CreateParameter();
                PackageNameParameter.ParameterName = "PackageName";
                PackageNameParameter.Direction = ParameterDirection.Input;
                PackageNameParameter.DbType = DbType.String;
                PackageNameParameter.Size = 200;
                if (PackageName != null)
                {
                    PackageNameParameter.Value = PackageName;
                }
                else
                {
                    PackageNameParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(PackageNameParameter);

                DbParameter PackageVersionParameter = cmd.CreateParameter();
                PackageVersionParameter.ParameterName = "PackageVersion";
                PackageVersionParameter.Direction = ParameterDirection.Input;
                PackageVersionParameter.DbType = DbType.String;
                PackageVersionParameter.Size = 200;
                if (PackageVersion != null)
                {
                    PackageVersionParameter.Value = PackageVersion;
                }
                else
                {
                    PackageVersionParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(PackageVersionParameter);

                DbParameter ConfigJsonParameter = cmd.CreateParameter();
                ConfigJsonParameter.ParameterName = "ConfigJson";
                ConfigJsonParameter.Direction = ParameterDirection.Input;
                ConfigJsonParameter.DbType = DbType.String;
                if (ConfigJson != null)
                {
                    ConfigJsonParameter.Value = ConfigJson;
                }
                else
                {
                    ConfigJsonParameter.Size = -1;
                    ConfigJsonParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ConfigJsonParameter);

                DbParameter SnapshotIdParameter = cmd.CreateParameter();
                SnapshotIdParameter.ParameterName = "SnapshotId";
                SnapshotIdParameter.Direction = ParameterDirection.InputOutput;
                SnapshotIdParameter.DbType = DbType.Guid;
                if (SnapshotId.HasValue)
                {
                    SnapshotIdParameter.Value = SnapshotId.Value;
                }
                else
                {
                    SnapshotIdParameter.Size = -1;
                    SnapshotIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SnapshotIdParameter);
                cmd.ExecuteNonQuery();

                if (cmd.Parameters["SnapshotId"].Value != null && !(cmd.Parameters["SnapshotId"].Value is DBNull))
                {
                    SnapshotId = (Guid)Convert.ChangeType(cmd.Parameters["SnapshotId"].Value,
                        typeof(Guid));
                }
                else
                {
                    SnapshotId = default(Guid);
                }
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task<Tuple<Guid?>> CreateSourceSnapshotAsync(Guid? IngestionRunId, string SnapshotUid, string RepoUrl, string Branch, string RepoCommit, string Language, string PackageName, string PackageVersion, string ConfigJson, Guid? SnapshotId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.CreateSourceSnapshot";

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter SnapshotUidParameter = cmd.CreateParameter();
                SnapshotUidParameter.ParameterName = "SnapshotUid";
                SnapshotUidParameter.Direction = ParameterDirection.Input;
                SnapshotUidParameter.DbType = DbType.String;
                SnapshotUidParameter.Size = 1000;
                if (SnapshotUid != null)
                {
                    SnapshotUidParameter.Value = SnapshotUid;
                }
                else
                {
                    SnapshotUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SnapshotUidParameter);

                DbParameter RepoUrlParameter = cmd.CreateParameter();
                RepoUrlParameter.ParameterName = "RepoUrl";
                RepoUrlParameter.Direction = ParameterDirection.Input;
                RepoUrlParameter.DbType = DbType.String;
                if (RepoUrl != null)
                {
                    RepoUrlParameter.Value = RepoUrl;
                }
                else
                {
                    RepoUrlParameter.Size = -1;
                    RepoUrlParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(RepoUrlParameter);

                DbParameter BranchParameter = cmd.CreateParameter();
                BranchParameter.ParameterName = "Branch";
                BranchParameter.Direction = ParameterDirection.Input;
                BranchParameter.DbType = DbType.String;
                BranchParameter.Size = 200;
                if (Branch != null)
                {
                    BranchParameter.Value = Branch;
                }
                else
                {
                    BranchParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(BranchParameter);

                DbParameter RepoCommitParameter = cmd.CreateParameter();
                RepoCommitParameter.ParameterName = "RepoCommit";
                RepoCommitParameter.Direction = ParameterDirection.Input;
                RepoCommitParameter.DbType = DbType.String;
                RepoCommitParameter.Size = 200;
                if (RepoCommit != null)
                {
                    RepoCommitParameter.Value = RepoCommit;
                }
                else
                {
                    RepoCommitParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(RepoCommitParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                if (Language != null)
                {
                    LanguageParameter.Value = Language;
                }
                else
                {
                    LanguageParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(LanguageParameter);

                DbParameter PackageNameParameter = cmd.CreateParameter();
                PackageNameParameter.ParameterName = "PackageName";
                PackageNameParameter.Direction = ParameterDirection.Input;
                PackageNameParameter.DbType = DbType.String;
                PackageNameParameter.Size = 200;
                if (PackageName != null)
                {
                    PackageNameParameter.Value = PackageName;
                }
                else
                {
                    PackageNameParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(PackageNameParameter);

                DbParameter PackageVersionParameter = cmd.CreateParameter();
                PackageVersionParameter.ParameterName = "PackageVersion";
                PackageVersionParameter.Direction = ParameterDirection.Input;
                PackageVersionParameter.DbType = DbType.String;
                PackageVersionParameter.Size = 200;
                if (PackageVersion != null)
                {
                    PackageVersionParameter.Value = PackageVersion;
                }
                else
                {
                    PackageVersionParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(PackageVersionParameter);

                DbParameter ConfigJsonParameter = cmd.CreateParameter();
                ConfigJsonParameter.ParameterName = "ConfigJson";
                ConfigJsonParameter.Direction = ParameterDirection.Input;
                ConfigJsonParameter.DbType = DbType.String;
                if (ConfigJson != null)
                {
                    ConfigJsonParameter.Value = ConfigJson;
                }
                else
                {
                    ConfigJsonParameter.Size = -1;
                    ConfigJsonParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ConfigJsonParameter);

                DbParameter SnapshotIdParameter = cmd.CreateParameter();
                SnapshotIdParameter.ParameterName = "SnapshotId";
                SnapshotIdParameter.Direction = ParameterDirection.InputOutput;
                SnapshotIdParameter.DbType = DbType.Guid;
                if (SnapshotId.HasValue)
                {
                    SnapshotIdParameter.Value = SnapshotId.Value;
                }
                else
                {
                    SnapshotIdParameter.Size = -1;
                    SnapshotIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SnapshotIdParameter);
                await cmd.ExecuteNonQueryAsync();

                if (cmd.Parameters["SnapshotId"].Value != null && !(cmd.Parameters["SnapshotId"].Value is DBNull))
                {
                    SnapshotId = (Guid)Convert.ChangeType(cmd.Parameters["SnapshotId"].Value,
                        typeof(Guid));
                }
                else
                {
                    SnapshotId = default(Guid);
                }
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }

        return new Tuple<Guid?>(SnapshotId);
    }







    public void CreateTruthRun(Guid? SnapshotId, string SchemaVersion, ref Guid? TruthRunId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.CreateTruthRun";

                DbParameter SnapshotIdParameter = cmd.CreateParameter();
                SnapshotIdParameter.ParameterName = "SnapshotId";
                SnapshotIdParameter.Direction = ParameterDirection.Input;
                SnapshotIdParameter.DbType = DbType.Guid;
                if (SnapshotId.HasValue)
                {
                    SnapshotIdParameter.Value = SnapshotId.Value;
                }
                else
                {
                    SnapshotIdParameter.Size = -1;
                    SnapshotIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SnapshotIdParameter);

                DbParameter SchemaVersionParameter = cmd.CreateParameter();
                SchemaVersionParameter.ParameterName = "SchemaVersion";
                SchemaVersionParameter.Direction = ParameterDirection.Input;
                SchemaVersionParameter.DbType = DbType.String;
                SchemaVersionParameter.Size = 200;
                if (SchemaVersion != null)
                {
                    SchemaVersionParameter.Value = SchemaVersion;
                }
                else
                {
                    SchemaVersionParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SchemaVersionParameter);

                DbParameter TruthRunIdParameter = cmd.CreateParameter();
                TruthRunIdParameter.ParameterName = "TruthRunId";
                TruthRunIdParameter.Direction = ParameterDirection.InputOutput;
                TruthRunIdParameter.DbType = DbType.Guid;
                if (TruthRunId.HasValue)
                {
                    TruthRunIdParameter.Value = TruthRunId.Value;
                }
                else
                {
                    TruthRunIdParameter.Size = -1;
                    TruthRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(TruthRunIdParameter);
                cmd.ExecuteNonQuery();

                if (cmd.Parameters["TruthRunId"].Value != null && !(cmd.Parameters["TruthRunId"].Value is DBNull))
                {
                    TruthRunId = (Guid)Convert.ChangeType(cmd.Parameters["TruthRunId"].Value,
                        typeof(Guid));
                }
                else
                {
                    TruthRunId = default(Guid);
                }
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task<Tuple<Guid?>> CreateTruthRunAsync(Guid? SnapshotId, string SchemaVersion, Guid? TruthRunId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.CreateTruthRun";

                DbParameter SnapshotIdParameter = cmd.CreateParameter();
                SnapshotIdParameter.ParameterName = "SnapshotId";
                SnapshotIdParameter.Direction = ParameterDirection.Input;
                SnapshotIdParameter.DbType = DbType.Guid;
                if (SnapshotId.HasValue)
                {
                    SnapshotIdParameter.Value = SnapshotId.Value;
                }
                else
                {
                    SnapshotIdParameter.Size = -1;
                    SnapshotIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SnapshotIdParameter);

                DbParameter SchemaVersionParameter = cmd.CreateParameter();
                SchemaVersionParameter.ParameterName = "SchemaVersion";
                SchemaVersionParameter.Direction = ParameterDirection.Input;
                SchemaVersionParameter.DbType = DbType.String;
                SchemaVersionParameter.Size = 200;
                if (SchemaVersion != null)
                {
                    SchemaVersionParameter.Value = SchemaVersion;
                }
                else
                {
                    SchemaVersionParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SchemaVersionParameter);

                DbParameter TruthRunIdParameter = cmd.CreateParameter();
                TruthRunIdParameter.ParameterName = "TruthRunId";
                TruthRunIdParameter.Direction = ParameterDirection.InputOutput;
                TruthRunIdParameter.DbType = DbType.Guid;
                if (TruthRunId.HasValue)
                {
                    TruthRunIdParameter.Value = TruthRunId.Value;
                }
                else
                {
                    TruthRunIdParameter.Size = -1;
                    TruthRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(TruthRunIdParameter);
                await cmd.ExecuteNonQueryAsync();

                if (cmd.Parameters["TruthRunId"].Value != null && !(cmd.Parameters["TruthRunId"].Value is DBNull))
                {
                    TruthRunId = (Guid)Convert.ChangeType(cmd.Parameters["TruthRunId"].Value,
                        typeof(Guid));
                }
                else
                {
                    TruthRunId = default(Guid);
                }
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }

        return new Tuple<Guid?>(TruthRunId);
    }







    public void DeleteSemanticIdentity(string Uid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.DeleteSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                if (Uid != null)
                {
                    UidParameter.Value = Uid;
                }
                else
                {
                    UidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(UidParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task DeleteSemanticIdentityAsync(string Uid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.DeleteSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                if (Uid != null)
                {
                    UidParameter.Value = Uid;
                }
                else
                {
                    UidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(UidParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void EndIngestionRun(Guid? IngestionRunId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.EndIngestionRun";

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task EndIngestionRunAsync(Guid? IngestionRunId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.EndIngestionRun";

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void EnsureSemanticIdentity(string Uid, string Kind, string Notes)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.EnsureSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                if (Uid != null)
                {
                    UidParameter.Value = Uid;
                }
                else
                {
                    UidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(UidParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 50;
                if (Kind != null)
                {
                    KindParameter.Value = Kind;
                }
                else
                {
                    KindParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(KindParameter);

                DbParameter NotesParameter = cmd.CreateParameter();
                NotesParameter.ParameterName = "Notes";
                NotesParameter.Direction = ParameterDirection.Input;
                NotesParameter.DbType = DbType.String;
                if (Notes != null)
                {
                    NotesParameter.Value = Notes;
                }
                else
                {
                    NotesParameter.Size = -1;
                    NotesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NotesParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task EnsureSemanticIdentityAsync(string Uid, string Kind, string Notes)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.EnsureSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                if (Uid != null)
                {
                    UidParameter.Value = Uid;
                }
                else
                {
                    UidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(UidParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 50;
                if (Kind != null)
                {
                    KindParameter.Value = Kind;
                }
                else
                {
                    KindParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(KindParameter);

                DbParameter NotesParameter = cmd.CreateParameter();
                NotesParameter.ParameterName = "Notes";
                NotesParameter.Direction = ParameterDirection.Input;
                NotesParameter.DbType = DbType.String;
                if (Notes != null)
                {
                    NotesParameter.Value = Notes;
                }
                else
                {
                    NotesParameter.Size = -1;
                    NotesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NotesParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void GetChangesInIngestionRun(Guid? IngestionRunId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.GetChangesInIngestionRun";

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task GetChangesInIngestionRunAsync(Guid? IngestionRunId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.GetChangesInIngestionRun";

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void GetDocPageHistory(string SemanticUid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.GetDocPageHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task GetDocPageHistoryAsync(string SemanticUid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.GetDocPageHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void GetFeatureHistory(string SemanticUid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.GetFeatureHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task GetFeatureHistoryAsync(string SemanticUid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.GetFeatureHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void GetMemberHistory(string SemanticUid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.GetMemberHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task GetMemberHistoryAsync(string SemanticUid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.GetMemberHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void GetTypeHistory(string SemanticUid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.GetTypeHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task GetTypeHistoryAsync(string SemanticUid)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.GetTypeHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void RegisterSemanticIdentity(string Uid, string Kind, string Notes)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.RegisterSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                if (Uid != null)
                {
                    UidParameter.Value = Uid;
                }
                else
                {
                    UidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(UidParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 50;
                if (Kind != null)
                {
                    KindParameter.Value = Kind;
                }
                else
                {
                    KindParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(KindParameter);

                DbParameter NotesParameter = cmd.CreateParameter();
                NotesParameter.ParameterName = "Notes";
                NotesParameter.Direction = ParameterDirection.Input;
                NotesParameter.DbType = DbType.String;
                if (Notes != null)
                {
                    NotesParameter.Value = Notes;
                }
                else
                {
                    NotesParameter.Size = -1;
                    NotesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NotesParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task RegisterSemanticIdentityAsync(string Uid, string Kind, string Notes)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.RegisterSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                if (Uid != null)
                {
                    UidParameter.Value = Uid;
                }
                else
                {
                    UidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(UidParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 50;
                if (Kind != null)
                {
                    KindParameter.Value = Kind;
                }
                else
                {
                    KindParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(KindParameter);

                DbParameter NotesParameter = cmd.CreateParameter();
                NotesParameter.ParameterName = "Notes";
                NotesParameter.Direction = ParameterDirection.Input;
                NotesParameter.DbType = DbType.String;
                if (Notes != null)
                {
                    NotesParameter.Value = Notes;
                }
                else
                {
                    NotesParameter.Size = -1;
                    NotesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NotesParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void UpsertApiFeature(string SemanticUid, Guid? TruthRunId, Guid? IngestionRunId, string Name, string Language, string Description, string Tags)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.UpsertApiFeature";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter TruthRunIdParameter = cmd.CreateParameter();
                TruthRunIdParameter.ParameterName = "TruthRunId";
                TruthRunIdParameter.Direction = ParameterDirection.Input;
                TruthRunIdParameter.DbType = DbType.Guid;
                if (TruthRunId.HasValue)
                {
                    TruthRunIdParameter.Value = TruthRunId.Value;
                }
                else
                {
                    TruthRunIdParameter.Size = -1;
                    TruthRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(TruthRunIdParameter);

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                if (Name != null)
                {
                    NameParameter.Value = Name;
                }
                else
                {
                    NameParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NameParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                if (Language != null)
                {
                    LanguageParameter.Value = Language;
                }
                else
                {
                    LanguageParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(LanguageParameter);

                DbParameter DescriptionParameter = cmd.CreateParameter();
                DescriptionParameter.ParameterName = "Description";
                DescriptionParameter.Direction = ParameterDirection.Input;
                DescriptionParameter.DbType = DbType.String;
                if (Description != null)
                {
                    DescriptionParameter.Value = Description;
                }
                else
                {
                    DescriptionParameter.Size = -1;
                    DescriptionParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(DescriptionParameter);

                DbParameter TagsParameter = cmd.CreateParameter();
                TagsParameter.ParameterName = "Tags";
                TagsParameter.Direction = ParameterDirection.Input;
                TagsParameter.DbType = DbType.String;
                if (Tags != null)
                {
                    TagsParameter.Value = Tags;
                }
                else
                {
                    TagsParameter.Size = -1;
                    TagsParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(TagsParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task UpsertApiFeatureAsync(string SemanticUid, Guid? TruthRunId, Guid? IngestionRunId, string Name, string Language, string Description, string Tags)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.UpsertApiFeature";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter TruthRunIdParameter = cmd.CreateParameter();
                TruthRunIdParameter.ParameterName = "TruthRunId";
                TruthRunIdParameter.Direction = ParameterDirection.Input;
                TruthRunIdParameter.DbType = DbType.Guid;
                if (TruthRunId.HasValue)
                {
                    TruthRunIdParameter.Value = TruthRunId.Value;
                }
                else
                {
                    TruthRunIdParameter.Size = -1;
                    TruthRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(TruthRunIdParameter);

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                if (Name != null)
                {
                    NameParameter.Value = Name;
                }
                else
                {
                    NameParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NameParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                if (Language != null)
                {
                    LanguageParameter.Value = Language;
                }
                else
                {
                    LanguageParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(LanguageParameter);

                DbParameter DescriptionParameter = cmd.CreateParameter();
                DescriptionParameter.ParameterName = "Description";
                DescriptionParameter.Direction = ParameterDirection.Input;
                DescriptionParameter.DbType = DbType.String;
                if (Description != null)
                {
                    DescriptionParameter.Value = Description;
                }
                else
                {
                    DescriptionParameter.Size = -1;
                    DescriptionParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(DescriptionParameter);

                DbParameter TagsParameter = cmd.CreateParameter();
                TagsParameter.ParameterName = "Tags";
                TagsParameter.Direction = ParameterDirection.Input;
                TagsParameter.DbType = DbType.String;
                if (Tags != null)
                {
                    TagsParameter.Value = Tags;
                }
                else
                {
                    TagsParameter.Size = -1;
                    TagsParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(TagsParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void UpsertApiMember(string SemanticUid, Guid? ApiTypeId, Guid? IngestionRunId, string Name, string Kind, string MethodKind, string Accessibility, bool? IsStatic, bool? IsExtensionMethod, bool? IsAsync, bool? IsVirtual, bool? IsOverride, bool? IsAbstract, bool? IsSealed, bool? IsReadOnly, bool? IsConst, bool? IsUnsafe, string ReturnTypeUid, string ReturnNullable, string GenericParameters, string GenericConstraints, string Summary, string Remarks, string Attributes, string SourceFilePath, int? SourceStartLine, int? SourceEndLine)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.UpsertApiMember";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter ApiTypeIdParameter = cmd.CreateParameter();
                ApiTypeIdParameter.ParameterName = "ApiTypeId";
                ApiTypeIdParameter.Direction = ParameterDirection.Input;
                ApiTypeIdParameter.DbType = DbType.Guid;
                if (ApiTypeId.HasValue)
                {
                    ApiTypeIdParameter.Value = ApiTypeId.Value;
                }
                else
                {
                    ApiTypeIdParameter.Size = -1;
                    ApiTypeIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ApiTypeIdParameter);

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                if (Name != null)
                {
                    NameParameter.Value = Name;
                }
                else
                {
                    NameParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NameParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 200;
                if (Kind != null)
                {
                    KindParameter.Value = Kind;
                }
                else
                {
                    KindParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(KindParameter);

                DbParameter MethodKindParameter = cmd.CreateParameter();
                MethodKindParameter.ParameterName = "MethodKind";
                MethodKindParameter.Direction = ParameterDirection.Input;
                MethodKindParameter.DbType = DbType.String;
                MethodKindParameter.Size = 200;
                if (MethodKind != null)
                {
                    MethodKindParameter.Value = MethodKind;
                }
                else
                {
                    MethodKindParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(MethodKindParameter);

                DbParameter AccessibilityParameter = cmd.CreateParameter();
                AccessibilityParameter.ParameterName = "Accessibility";
                AccessibilityParameter.Direction = ParameterDirection.Input;
                AccessibilityParameter.DbType = DbType.String;
                AccessibilityParameter.Size = 200;
                if (Accessibility != null)
                {
                    AccessibilityParameter.Value = Accessibility;
                }
                else
                {
                    AccessibilityParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AccessibilityParameter);

                DbParameter IsStaticParameter = cmd.CreateParameter();
                IsStaticParameter.ParameterName = "IsStatic";
                IsStaticParameter.Direction = ParameterDirection.Input;
                IsStaticParameter.DbType = DbType.Boolean;
                if (IsStatic.HasValue)
                {
                    IsStaticParameter.Value = IsStatic.Value;
                }
                else
                {
                    IsStaticParameter.Size = -1;
                    IsStaticParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsStaticParameter);

                DbParameter IsExtensionMethodParameter = cmd.CreateParameter();
                IsExtensionMethodParameter.ParameterName = "IsExtensionMethod";
                IsExtensionMethodParameter.Direction = ParameterDirection.Input;
                IsExtensionMethodParameter.DbType = DbType.Boolean;
                if (IsExtensionMethod.HasValue)
                {
                    IsExtensionMethodParameter.Value = IsExtensionMethod.Value;
                }
                else
                {
                    IsExtensionMethodParameter.Size = -1;
                    IsExtensionMethodParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsExtensionMethodParameter);

                DbParameter IsAsyncParameter = cmd.CreateParameter();
                IsAsyncParameter.ParameterName = "IsAsync";
                IsAsyncParameter.Direction = ParameterDirection.Input;
                IsAsyncParameter.DbType = DbType.Boolean;
                if (IsAsync.HasValue)
                {
                    IsAsyncParameter.Value = IsAsync.Value;
                }
                else
                {
                    IsAsyncParameter.Size = -1;
                    IsAsyncParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsAsyncParameter);

                DbParameter IsVirtualParameter = cmd.CreateParameter();
                IsVirtualParameter.ParameterName = "IsVirtual";
                IsVirtualParameter.Direction = ParameterDirection.Input;
                IsVirtualParameter.DbType = DbType.Boolean;
                if (IsVirtual.HasValue)
                {
                    IsVirtualParameter.Value = IsVirtual.Value;
                }
                else
                {
                    IsVirtualParameter.Size = -1;
                    IsVirtualParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsVirtualParameter);

                DbParameter IsOverrideParameter = cmd.CreateParameter();
                IsOverrideParameter.ParameterName = "IsOverride";
                IsOverrideParameter.Direction = ParameterDirection.Input;
                IsOverrideParameter.DbType = DbType.Boolean;
                if (IsOverride.HasValue)
                {
                    IsOverrideParameter.Value = IsOverride.Value;
                }
                else
                {
                    IsOverrideParameter.Size = -1;
                    IsOverrideParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsOverrideParameter);

                DbParameter IsAbstractParameter = cmd.CreateParameter();
                IsAbstractParameter.ParameterName = "IsAbstract";
                IsAbstractParameter.Direction = ParameterDirection.Input;
                IsAbstractParameter.DbType = DbType.Boolean;
                if (IsAbstract.HasValue)
                {
                    IsAbstractParameter.Value = IsAbstract.Value;
                }
                else
                {
                    IsAbstractParameter.Size = -1;
                    IsAbstractParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsAbstractParameter);

                DbParameter IsSealedParameter = cmd.CreateParameter();
                IsSealedParameter.ParameterName = "IsSealed";
                IsSealedParameter.Direction = ParameterDirection.Input;
                IsSealedParameter.DbType = DbType.Boolean;
                if (IsSealed.HasValue)
                {
                    IsSealedParameter.Value = IsSealed.Value;
                }
                else
                {
                    IsSealedParameter.Size = -1;
                    IsSealedParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsSealedParameter);

                DbParameter IsReadOnlyParameter = cmd.CreateParameter();
                IsReadOnlyParameter.ParameterName = "IsReadOnly";
                IsReadOnlyParameter.Direction = ParameterDirection.Input;
                IsReadOnlyParameter.DbType = DbType.Boolean;
                if (IsReadOnly.HasValue)
                {
                    IsReadOnlyParameter.Value = IsReadOnly.Value;
                }
                else
                {
                    IsReadOnlyParameter.Size = -1;
                    IsReadOnlyParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsReadOnlyParameter);

                DbParameter IsConstParameter = cmd.CreateParameter();
                IsConstParameter.ParameterName = "IsConst";
                IsConstParameter.Direction = ParameterDirection.Input;
                IsConstParameter.DbType = DbType.Boolean;
                if (IsConst.HasValue)
                {
                    IsConstParameter.Value = IsConst.Value;
                }
                else
                {
                    IsConstParameter.Size = -1;
                    IsConstParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsConstParameter);

                DbParameter IsUnsafeParameter = cmd.CreateParameter();
                IsUnsafeParameter.ParameterName = "IsUnsafe";
                IsUnsafeParameter.Direction = ParameterDirection.Input;
                IsUnsafeParameter.DbType = DbType.Boolean;
                if (IsUnsafe.HasValue)
                {
                    IsUnsafeParameter.Value = IsUnsafe.Value;
                }
                else
                {
                    IsUnsafeParameter.Size = -1;
                    IsUnsafeParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsUnsafeParameter);

                DbParameter ReturnTypeUidParameter = cmd.CreateParameter();
                ReturnTypeUidParameter.ParameterName = "ReturnTypeUid";
                ReturnTypeUidParameter.Direction = ParameterDirection.Input;
                ReturnTypeUidParameter.DbType = DbType.String;
                ReturnTypeUidParameter.Size = 1000;
                if (ReturnTypeUid != null)
                {
                    ReturnTypeUidParameter.Value = ReturnTypeUid;
                }
                else
                {
                    ReturnTypeUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ReturnTypeUidParameter);

                DbParameter ReturnNullableParameter = cmd.CreateParameter();
                ReturnNullableParameter.ParameterName = "ReturnNullable";
                ReturnNullableParameter.Direction = ParameterDirection.Input;
                ReturnNullableParameter.DbType = DbType.String;
                ReturnNullableParameter.Size = 50;
                if (ReturnNullable != null)
                {
                    ReturnNullableParameter.Value = ReturnNullable;
                }
                else
                {
                    ReturnNullableParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ReturnNullableParameter);

                DbParameter GenericParametersParameter = cmd.CreateParameter();
                GenericParametersParameter.ParameterName = "GenericParameters";
                GenericParametersParameter.Direction = ParameterDirection.Input;
                GenericParametersParameter.DbType = DbType.String;
                if (GenericParameters != null)
                {
                    GenericParametersParameter.Value = GenericParameters;
                }
                else
                {
                    GenericParametersParameter.Size = -1;
                    GenericParametersParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(GenericParametersParameter);

                DbParameter GenericConstraintsParameter = cmd.CreateParameter();
                GenericConstraintsParameter.ParameterName = "GenericConstraints";
                GenericConstraintsParameter.Direction = ParameterDirection.Input;
                GenericConstraintsParameter.DbType = DbType.String;
                if (GenericConstraints != null)
                {
                    GenericConstraintsParameter.Value = GenericConstraints;
                }
                else
                {
                    GenericConstraintsParameter.Size = -1;
                    GenericConstraintsParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(GenericConstraintsParameter);

                DbParameter SummaryParameter = cmd.CreateParameter();
                SummaryParameter.ParameterName = "Summary";
                SummaryParameter.Direction = ParameterDirection.Input;
                SummaryParameter.DbType = DbType.String;
                if (Summary != null)
                {
                    SummaryParameter.Value = Summary;
                }
                else
                {
                    SummaryParameter.Size = -1;
                    SummaryParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SummaryParameter);

                DbParameter RemarksParameter = cmd.CreateParameter();
                RemarksParameter.ParameterName = "Remarks";
                RemarksParameter.Direction = ParameterDirection.Input;
                RemarksParameter.DbType = DbType.String;
                if (Remarks != null)
                {
                    RemarksParameter.Value = Remarks;
                }
                else
                {
                    RemarksParameter.Size = -1;
                    RemarksParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(RemarksParameter);

                DbParameter AttributesParameter = cmd.CreateParameter();
                AttributesParameter.ParameterName = "Attributes";
                AttributesParameter.Direction = ParameterDirection.Input;
                AttributesParameter.DbType = DbType.String;
                if (Attributes != null)
                {
                    AttributesParameter.Value = Attributes;
                }
                else
                {
                    AttributesParameter.Size = -1;
                    AttributesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AttributesParameter);

                DbParameter SourceFilePathParameter = cmd.CreateParameter();
                SourceFilePathParameter.ParameterName = "SourceFilePath";
                SourceFilePathParameter.Direction = ParameterDirection.Input;
                SourceFilePathParameter.DbType = DbType.String;
                if (SourceFilePath != null)
                {
                    SourceFilePathParameter.Value = SourceFilePath;
                }
                else
                {
                    SourceFilePathParameter.Size = -1;
                    SourceFilePathParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceFilePathParameter);

                DbParameter SourceStartLineParameter = cmd.CreateParameter();
                SourceStartLineParameter.ParameterName = "SourceStartLine";
                SourceStartLineParameter.Direction = ParameterDirection.Input;
                SourceStartLineParameter.DbType = DbType.Int32;
                SourceStartLineParameter.Precision = 10;
                SourceStartLineParameter.Scale = 0;
                if (SourceStartLine.HasValue)
                {
                    SourceStartLineParameter.Value = SourceStartLine.Value;
                }
                else
                {
                    SourceStartLineParameter.Size = -1;
                    SourceStartLineParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceStartLineParameter);

                DbParameter SourceEndLineParameter = cmd.CreateParameter();
                SourceEndLineParameter.ParameterName = "SourceEndLine";
                SourceEndLineParameter.Direction = ParameterDirection.Input;
                SourceEndLineParameter.DbType = DbType.Int32;
                SourceEndLineParameter.Precision = 10;
                SourceEndLineParameter.Scale = 0;
                if (SourceEndLine.HasValue)
                {
                    SourceEndLineParameter.Value = SourceEndLine.Value;
                }
                else
                {
                    SourceEndLineParameter.Size = -1;
                    SourceEndLineParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceEndLineParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task UpsertApiMemberAsync(string SemanticUid, Guid? ApiTypeId, Guid? IngestionRunId, string Name, string Kind, string MethodKind, string Accessibility, bool? IsStatic, bool? IsExtensionMethod, bool? IsAsync, bool? IsVirtual, bool? IsOverride, bool? IsAbstract, bool? IsSealed, bool? IsReadOnly, bool? IsConst, bool? IsUnsafe, string ReturnTypeUid, string ReturnNullable, string GenericParameters, string GenericConstraints, string Summary, string Remarks, string Attributes, string SourceFilePath, int? SourceStartLine, int? SourceEndLine)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.UpsertApiMember";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter ApiTypeIdParameter = cmd.CreateParameter();
                ApiTypeIdParameter.ParameterName = "ApiTypeId";
                ApiTypeIdParameter.Direction = ParameterDirection.Input;
                ApiTypeIdParameter.DbType = DbType.Guid;
                if (ApiTypeId.HasValue)
                {
                    ApiTypeIdParameter.Value = ApiTypeId.Value;
                }
                else
                {
                    ApiTypeIdParameter.Size = -1;
                    ApiTypeIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ApiTypeIdParameter);

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                if (Name != null)
                {
                    NameParameter.Value = Name;
                }
                else
                {
                    NameParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NameParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 200;
                if (Kind != null)
                {
                    KindParameter.Value = Kind;
                }
                else
                {
                    KindParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(KindParameter);

                DbParameter MethodKindParameter = cmd.CreateParameter();
                MethodKindParameter.ParameterName = "MethodKind";
                MethodKindParameter.Direction = ParameterDirection.Input;
                MethodKindParameter.DbType = DbType.String;
                MethodKindParameter.Size = 200;
                if (MethodKind != null)
                {
                    MethodKindParameter.Value = MethodKind;
                }
                else
                {
                    MethodKindParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(MethodKindParameter);

                DbParameter AccessibilityParameter = cmd.CreateParameter();
                AccessibilityParameter.ParameterName = "Accessibility";
                AccessibilityParameter.Direction = ParameterDirection.Input;
                AccessibilityParameter.DbType = DbType.String;
                AccessibilityParameter.Size = 200;
                if (Accessibility != null)
                {
                    AccessibilityParameter.Value = Accessibility;
                }
                else
                {
                    AccessibilityParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AccessibilityParameter);

                DbParameter IsStaticParameter = cmd.CreateParameter();
                IsStaticParameter.ParameterName = "IsStatic";
                IsStaticParameter.Direction = ParameterDirection.Input;
                IsStaticParameter.DbType = DbType.Boolean;
                if (IsStatic.HasValue)
                {
                    IsStaticParameter.Value = IsStatic.Value;
                }
                else
                {
                    IsStaticParameter.Size = -1;
                    IsStaticParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsStaticParameter);

                DbParameter IsExtensionMethodParameter = cmd.CreateParameter();
                IsExtensionMethodParameter.ParameterName = "IsExtensionMethod";
                IsExtensionMethodParameter.Direction = ParameterDirection.Input;
                IsExtensionMethodParameter.DbType = DbType.Boolean;
                if (IsExtensionMethod.HasValue)
                {
                    IsExtensionMethodParameter.Value = IsExtensionMethod.Value;
                }
                else
                {
                    IsExtensionMethodParameter.Size = -1;
                    IsExtensionMethodParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsExtensionMethodParameter);

                DbParameter IsAsyncParameter = cmd.CreateParameter();
                IsAsyncParameter.ParameterName = "IsAsync";
                IsAsyncParameter.Direction = ParameterDirection.Input;
                IsAsyncParameter.DbType = DbType.Boolean;
                if (IsAsync.HasValue)
                {
                    IsAsyncParameter.Value = IsAsync.Value;
                }
                else
                {
                    IsAsyncParameter.Size = -1;
                    IsAsyncParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsAsyncParameter);

                DbParameter IsVirtualParameter = cmd.CreateParameter();
                IsVirtualParameter.ParameterName = "IsVirtual";
                IsVirtualParameter.Direction = ParameterDirection.Input;
                IsVirtualParameter.DbType = DbType.Boolean;
                if (IsVirtual.HasValue)
                {
                    IsVirtualParameter.Value = IsVirtual.Value;
                }
                else
                {
                    IsVirtualParameter.Size = -1;
                    IsVirtualParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsVirtualParameter);

                DbParameter IsOverrideParameter = cmd.CreateParameter();
                IsOverrideParameter.ParameterName = "IsOverride";
                IsOverrideParameter.Direction = ParameterDirection.Input;
                IsOverrideParameter.DbType = DbType.Boolean;
                if (IsOverride.HasValue)
                {
                    IsOverrideParameter.Value = IsOverride.Value;
                }
                else
                {
                    IsOverrideParameter.Size = -1;
                    IsOverrideParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsOverrideParameter);

                DbParameter IsAbstractParameter = cmd.CreateParameter();
                IsAbstractParameter.ParameterName = "IsAbstract";
                IsAbstractParameter.Direction = ParameterDirection.Input;
                IsAbstractParameter.DbType = DbType.Boolean;
                if (IsAbstract.HasValue)
                {
                    IsAbstractParameter.Value = IsAbstract.Value;
                }
                else
                {
                    IsAbstractParameter.Size = -1;
                    IsAbstractParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsAbstractParameter);

                DbParameter IsSealedParameter = cmd.CreateParameter();
                IsSealedParameter.ParameterName = "IsSealed";
                IsSealedParameter.Direction = ParameterDirection.Input;
                IsSealedParameter.DbType = DbType.Boolean;
                if (IsSealed.HasValue)
                {
                    IsSealedParameter.Value = IsSealed.Value;
                }
                else
                {
                    IsSealedParameter.Size = -1;
                    IsSealedParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsSealedParameter);

                DbParameter IsReadOnlyParameter = cmd.CreateParameter();
                IsReadOnlyParameter.ParameterName = "IsReadOnly";
                IsReadOnlyParameter.Direction = ParameterDirection.Input;
                IsReadOnlyParameter.DbType = DbType.Boolean;
                if (IsReadOnly.HasValue)
                {
                    IsReadOnlyParameter.Value = IsReadOnly.Value;
                }
                else
                {
                    IsReadOnlyParameter.Size = -1;
                    IsReadOnlyParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsReadOnlyParameter);

                DbParameter IsConstParameter = cmd.CreateParameter();
                IsConstParameter.ParameterName = "IsConst";
                IsConstParameter.Direction = ParameterDirection.Input;
                IsConstParameter.DbType = DbType.Boolean;
                if (IsConst.HasValue)
                {
                    IsConstParameter.Value = IsConst.Value;
                }
                else
                {
                    IsConstParameter.Size = -1;
                    IsConstParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsConstParameter);

                DbParameter IsUnsafeParameter = cmd.CreateParameter();
                IsUnsafeParameter.ParameterName = "IsUnsafe";
                IsUnsafeParameter.Direction = ParameterDirection.Input;
                IsUnsafeParameter.DbType = DbType.Boolean;
                if (IsUnsafe.HasValue)
                {
                    IsUnsafeParameter.Value = IsUnsafe.Value;
                }
                else
                {
                    IsUnsafeParameter.Size = -1;
                    IsUnsafeParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsUnsafeParameter);

                DbParameter ReturnTypeUidParameter = cmd.CreateParameter();
                ReturnTypeUidParameter.ParameterName = "ReturnTypeUid";
                ReturnTypeUidParameter.Direction = ParameterDirection.Input;
                ReturnTypeUidParameter.DbType = DbType.String;
                ReturnTypeUidParameter.Size = 1000;
                if (ReturnTypeUid != null)
                {
                    ReturnTypeUidParameter.Value = ReturnTypeUid;
                }
                else
                {
                    ReturnTypeUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ReturnTypeUidParameter);

                DbParameter ReturnNullableParameter = cmd.CreateParameter();
                ReturnNullableParameter.ParameterName = "ReturnNullable";
                ReturnNullableParameter.Direction = ParameterDirection.Input;
                ReturnNullableParameter.DbType = DbType.String;
                ReturnNullableParameter.Size = 50;
                if (ReturnNullable != null)
                {
                    ReturnNullableParameter.Value = ReturnNullable;
                }
                else
                {
                    ReturnNullableParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ReturnNullableParameter);

                DbParameter GenericParametersParameter = cmd.CreateParameter();
                GenericParametersParameter.ParameterName = "GenericParameters";
                GenericParametersParameter.Direction = ParameterDirection.Input;
                GenericParametersParameter.DbType = DbType.String;
                if (GenericParameters != null)
                {
                    GenericParametersParameter.Value = GenericParameters;
                }
                else
                {
                    GenericParametersParameter.Size = -1;
                    GenericParametersParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(GenericParametersParameter);

                DbParameter GenericConstraintsParameter = cmd.CreateParameter();
                GenericConstraintsParameter.ParameterName = "GenericConstraints";
                GenericConstraintsParameter.Direction = ParameterDirection.Input;
                GenericConstraintsParameter.DbType = DbType.String;
                if (GenericConstraints != null)
                {
                    GenericConstraintsParameter.Value = GenericConstraints;
                }
                else
                {
                    GenericConstraintsParameter.Size = -1;
                    GenericConstraintsParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(GenericConstraintsParameter);

                DbParameter SummaryParameter = cmd.CreateParameter();
                SummaryParameter.ParameterName = "Summary";
                SummaryParameter.Direction = ParameterDirection.Input;
                SummaryParameter.DbType = DbType.String;
                if (Summary != null)
                {
                    SummaryParameter.Value = Summary;
                }
                else
                {
                    SummaryParameter.Size = -1;
                    SummaryParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SummaryParameter);

                DbParameter RemarksParameter = cmd.CreateParameter();
                RemarksParameter.ParameterName = "Remarks";
                RemarksParameter.Direction = ParameterDirection.Input;
                RemarksParameter.DbType = DbType.String;
                if (Remarks != null)
                {
                    RemarksParameter.Value = Remarks;
                }
                else
                {
                    RemarksParameter.Size = -1;
                    RemarksParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(RemarksParameter);

                DbParameter AttributesParameter = cmd.CreateParameter();
                AttributesParameter.ParameterName = "Attributes";
                AttributesParameter.Direction = ParameterDirection.Input;
                AttributesParameter.DbType = DbType.String;
                if (Attributes != null)
                {
                    AttributesParameter.Value = Attributes;
                }
                else
                {
                    AttributesParameter.Size = -1;
                    AttributesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AttributesParameter);

                DbParameter SourceFilePathParameter = cmd.CreateParameter();
                SourceFilePathParameter.ParameterName = "SourceFilePath";
                SourceFilePathParameter.Direction = ParameterDirection.Input;
                SourceFilePathParameter.DbType = DbType.String;
                if (SourceFilePath != null)
                {
                    SourceFilePathParameter.Value = SourceFilePath;
                }
                else
                {
                    SourceFilePathParameter.Size = -1;
                    SourceFilePathParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceFilePathParameter);

                DbParameter SourceStartLineParameter = cmd.CreateParameter();
                SourceStartLineParameter.ParameterName = "SourceStartLine";
                SourceStartLineParameter.Direction = ParameterDirection.Input;
                SourceStartLineParameter.DbType = DbType.Int32;
                SourceStartLineParameter.Precision = 10;
                SourceStartLineParameter.Scale = 0;
                if (SourceStartLine.HasValue)
                {
                    SourceStartLineParameter.Value = SourceStartLine.Value;
                }
                else
                {
                    SourceStartLineParameter.Size = -1;
                    SourceStartLineParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceStartLineParameter);

                DbParameter SourceEndLineParameter = cmd.CreateParameter();
                SourceEndLineParameter.ParameterName = "SourceEndLine";
                SourceEndLineParameter.Direction = ParameterDirection.Input;
                SourceEndLineParameter.DbType = DbType.Int32;
                SourceEndLineParameter.Precision = 10;
                SourceEndLineParameter.Scale = 0;
                if (SourceEndLine.HasValue)
                {
                    SourceEndLineParameter.Value = SourceEndLine.Value;
                }
                else
                {
                    SourceEndLineParameter.Size = -1;
                    SourceEndLineParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceEndLineParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void UpsertApiParameter(Guid? api_member_id, string name, string type_uid, string nullable_annotation, int? position, string modifier, bool? has_default_value, string default_value_literal, Guid? ingestion_run_id)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.UpsertApiParameter";

                DbParameter api_member_idParameter = cmd.CreateParameter();
                api_member_idParameter.ParameterName = "api_member_id";
                api_member_idParameter.Direction = ParameterDirection.Input;
                api_member_idParameter.DbType = DbType.Guid;
                if (api_member_id.HasValue)
                {
                    api_member_idParameter.Value = api_member_id.Value;
                }
                else
                {
                    api_member_idParameter.Size = -1;
                    api_member_idParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(api_member_idParameter);

                DbParameter nameParameter = cmd.CreateParameter();
                nameParameter.ParameterName = "name";
                nameParameter.Direction = ParameterDirection.Input;
                nameParameter.DbType = DbType.String;
                nameParameter.Size = 200;
                if (name != null)
                {
                    nameParameter.Value = name;
                }
                else
                {
                    nameParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(nameParameter);

                DbParameter type_uidParameter = cmd.CreateParameter();
                type_uidParameter.ParameterName = "type_uid";
                type_uidParameter.Direction = ParameterDirection.Input;
                type_uidParameter.DbType = DbType.String;
                type_uidParameter.Size = 1000;
                if (type_uid != null)
                {
                    type_uidParameter.Value = type_uid;
                }
                else
                {
                    type_uidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(type_uidParameter);

                DbParameter nullable_annotationParameter = cmd.CreateParameter();
                nullable_annotationParameter.ParameterName = "nullable_annotation";
                nullable_annotationParameter.Direction = ParameterDirection.Input;
                nullable_annotationParameter.DbType = DbType.String;
                nullable_annotationParameter.Size = 50;
                if (nullable_annotation != null)
                {
                    nullable_annotationParameter.Value = nullable_annotation;
                }
                else
                {
                    nullable_annotationParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(nullable_annotationParameter);

                DbParameter positionParameter = cmd.CreateParameter();
                positionParameter.ParameterName = "position";
                positionParameter.Direction = ParameterDirection.Input;
                positionParameter.DbType = DbType.Int32;
                positionParameter.Precision = 10;
                positionParameter.Scale = 0;
                if (position.HasValue)
                {
                    positionParameter.Value = position.Value;
                }
                else
                {
                    positionParameter.Size = -1;
                    positionParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(positionParameter);

                DbParameter modifierParameter = cmd.CreateParameter();
                modifierParameter.ParameterName = "modifier";
                modifierParameter.Direction = ParameterDirection.Input;
                modifierParameter.DbType = DbType.String;
                modifierParameter.Size = 50;
                if (modifier != null)
                {
                    modifierParameter.Value = modifier;
                }
                else
                {
                    modifierParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(modifierParameter);

                DbParameter has_default_valueParameter = cmd.CreateParameter();
                has_default_valueParameter.ParameterName = "has_default_value";
                has_default_valueParameter.Direction = ParameterDirection.Input;
                has_default_valueParameter.DbType = DbType.Boolean;
                if (has_default_value.HasValue)
                {
                    has_default_valueParameter.Value = has_default_value.Value;
                }
                else
                {
                    has_default_valueParameter.Size = -1;
                    has_default_valueParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(has_default_valueParameter);

                DbParameter default_value_literalParameter = cmd.CreateParameter();
                default_value_literalParameter.ParameterName = "default_value_literal";
                default_value_literalParameter.Direction = ParameterDirection.Input;
                default_value_literalParameter.DbType = DbType.String;
                if (default_value_literal != null)
                {
                    default_value_literalParameter.Value = default_value_literal;
                }
                else
                {
                    default_value_literalParameter.Size = -1;
                    default_value_literalParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(default_value_literalParameter);

                DbParameter ingestion_run_idParameter = cmd.CreateParameter();
                ingestion_run_idParameter.ParameterName = "ingestion_run_id";
                ingestion_run_idParameter.Direction = ParameterDirection.Input;
                ingestion_run_idParameter.DbType = DbType.Guid;
                if (ingestion_run_id.HasValue)
                {
                    ingestion_run_idParameter.Value = ingestion_run_id.Value;
                }
                else
                {
                    ingestion_run_idParameter.Size = -1;
                    ingestion_run_idParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ingestion_run_idParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task UpsertApiParameterAsync(Guid? api_member_id, string name, string type_uid, string nullable_annotation, int? position, string modifier, bool? has_default_value, string default_value_literal, Guid? ingestion_run_id)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.UpsertApiParameter";

                DbParameter api_member_idParameter = cmd.CreateParameter();
                api_member_idParameter.ParameterName = "api_member_id";
                api_member_idParameter.Direction = ParameterDirection.Input;
                api_member_idParameter.DbType = DbType.Guid;
                if (api_member_id.HasValue)
                {
                    api_member_idParameter.Value = api_member_id.Value;
                }
                else
                {
                    api_member_idParameter.Size = -1;
                    api_member_idParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(api_member_idParameter);

                DbParameter nameParameter = cmd.CreateParameter();
                nameParameter.ParameterName = "name";
                nameParameter.Direction = ParameterDirection.Input;
                nameParameter.DbType = DbType.String;
                nameParameter.Size = 200;
                if (name != null)
                {
                    nameParameter.Value = name;
                }
                else
                {
                    nameParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(nameParameter);

                DbParameter type_uidParameter = cmd.CreateParameter();
                type_uidParameter.ParameterName = "type_uid";
                type_uidParameter.Direction = ParameterDirection.Input;
                type_uidParameter.DbType = DbType.String;
                type_uidParameter.Size = 1000;
                if (type_uid != null)
                {
                    type_uidParameter.Value = type_uid;
                }
                else
                {
                    type_uidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(type_uidParameter);

                DbParameter nullable_annotationParameter = cmd.CreateParameter();
                nullable_annotationParameter.ParameterName = "nullable_annotation";
                nullable_annotationParameter.Direction = ParameterDirection.Input;
                nullable_annotationParameter.DbType = DbType.String;
                nullable_annotationParameter.Size = 50;
                if (nullable_annotation != null)
                {
                    nullable_annotationParameter.Value = nullable_annotation;
                }
                else
                {
                    nullable_annotationParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(nullable_annotationParameter);

                DbParameter positionParameter = cmd.CreateParameter();
                positionParameter.ParameterName = "position";
                positionParameter.Direction = ParameterDirection.Input;
                positionParameter.DbType = DbType.Int32;
                positionParameter.Precision = 10;
                positionParameter.Scale = 0;
                if (position.HasValue)
                {
                    positionParameter.Value = position.Value;
                }
                else
                {
                    positionParameter.Size = -1;
                    positionParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(positionParameter);

                DbParameter modifierParameter = cmd.CreateParameter();
                modifierParameter.ParameterName = "modifier";
                modifierParameter.Direction = ParameterDirection.Input;
                modifierParameter.DbType = DbType.String;
                modifierParameter.Size = 50;
                if (modifier != null)
                {
                    modifierParameter.Value = modifier;
                }
                else
                {
                    modifierParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(modifierParameter);

                DbParameter has_default_valueParameter = cmd.CreateParameter();
                has_default_valueParameter.ParameterName = "has_default_value";
                has_default_valueParameter.Direction = ParameterDirection.Input;
                has_default_valueParameter.DbType = DbType.Boolean;
                if (has_default_value.HasValue)
                {
                    has_default_valueParameter.Value = has_default_value.Value;
                }
                else
                {
                    has_default_valueParameter.Size = -1;
                    has_default_valueParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(has_default_valueParameter);

                DbParameter default_value_literalParameter = cmd.CreateParameter();
                default_value_literalParameter.ParameterName = "default_value_literal";
                default_value_literalParameter.Direction = ParameterDirection.Input;
                default_value_literalParameter.DbType = DbType.String;
                if (default_value_literal != null)
                {
                    default_value_literalParameter.Value = default_value_literal;
                }
                else
                {
                    default_value_literalParameter.Size = -1;
                    default_value_literalParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(default_value_literalParameter);

                DbParameter ingestion_run_idParameter = cmd.CreateParameter();
                ingestion_run_idParameter.ParameterName = "ingestion_run_id";
                ingestion_run_idParameter.Direction = ParameterDirection.Input;
                ingestion_run_idParameter.DbType = DbType.Guid;
                if (ingestion_run_id.HasValue)
                {
                    ingestion_run_idParameter.Value = ingestion_run_id.Value;
                }
                else
                {
                    ingestion_run_idParameter.Size = -1;
                    ingestion_run_idParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ingestion_run_idParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void UpsertApiType(string SemanticUid, Guid? SourceSnapshotId, Guid? IngestionRunId, string Name, string NamespacePath, string Kind, string Accessibility, bool? IsStatic, bool? IsGeneric, bool? IsAbstract, bool? IsSealed, bool? IsRecord, bool? IsRefLike, string BaseTypeUid, string Interfaces, string ContainingTypeUid, string GenericParameters, string GenericConstraints, string Summary, string Remarks, string Attributes, string SourceFilePath, int? SourceStartLine, int? SourceEndLine)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.UpsertApiType";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter SourceSnapshotIdParameter = cmd.CreateParameter();
                SourceSnapshotIdParameter.ParameterName = "SourceSnapshotId";
                SourceSnapshotIdParameter.Direction = ParameterDirection.Input;
                SourceSnapshotIdParameter.DbType = DbType.Guid;
                if (SourceSnapshotId.HasValue)
                {
                    SourceSnapshotIdParameter.Value = SourceSnapshotId.Value;
                }
                else
                {
                    SourceSnapshotIdParameter.Size = -1;
                    SourceSnapshotIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceSnapshotIdParameter);

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                if (Name != null)
                {
                    NameParameter.Value = Name;
                }
                else
                {
                    NameParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NameParameter);

                DbParameter NamespacePathParameter = cmd.CreateParameter();
                NamespacePathParameter.ParameterName = "NamespacePath";
                NamespacePathParameter.Direction = ParameterDirection.Input;
                NamespacePathParameter.DbType = DbType.String;
                NamespacePathParameter.Size = 1000;
                if (NamespacePath != null)
                {
                    NamespacePathParameter.Value = NamespacePath;
                }
                else
                {
                    NamespacePathParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NamespacePathParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 200;
                if (Kind != null)
                {
                    KindParameter.Value = Kind;
                }
                else
                {
                    KindParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(KindParameter);

                DbParameter AccessibilityParameter = cmd.CreateParameter();
                AccessibilityParameter.ParameterName = "Accessibility";
                AccessibilityParameter.Direction = ParameterDirection.Input;
                AccessibilityParameter.DbType = DbType.String;
                AccessibilityParameter.Size = 200;
                if (Accessibility != null)
                {
                    AccessibilityParameter.Value = Accessibility;
                }
                else
                {
                    AccessibilityParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AccessibilityParameter);

                DbParameter IsStaticParameter = cmd.CreateParameter();
                IsStaticParameter.ParameterName = "IsStatic";
                IsStaticParameter.Direction = ParameterDirection.Input;
                IsStaticParameter.DbType = DbType.Boolean;
                if (IsStatic.HasValue)
                {
                    IsStaticParameter.Value = IsStatic.Value;
                }
                else
                {
                    IsStaticParameter.Size = -1;
                    IsStaticParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsStaticParameter);

                DbParameter IsGenericParameter = cmd.CreateParameter();
                IsGenericParameter.ParameterName = "IsGeneric";
                IsGenericParameter.Direction = ParameterDirection.Input;
                IsGenericParameter.DbType = DbType.Boolean;
                if (IsGeneric.HasValue)
                {
                    IsGenericParameter.Value = IsGeneric.Value;
                }
                else
                {
                    IsGenericParameter.Size = -1;
                    IsGenericParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsGenericParameter);

                DbParameter IsAbstractParameter = cmd.CreateParameter();
                IsAbstractParameter.ParameterName = "IsAbstract";
                IsAbstractParameter.Direction = ParameterDirection.Input;
                IsAbstractParameter.DbType = DbType.Boolean;
                if (IsAbstract.HasValue)
                {
                    IsAbstractParameter.Value = IsAbstract.Value;
                }
                else
                {
                    IsAbstractParameter.Size = -1;
                    IsAbstractParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsAbstractParameter);

                DbParameter IsSealedParameter = cmd.CreateParameter();
                IsSealedParameter.ParameterName = "IsSealed";
                IsSealedParameter.Direction = ParameterDirection.Input;
                IsSealedParameter.DbType = DbType.Boolean;
                if (IsSealed.HasValue)
                {
                    IsSealedParameter.Value = IsSealed.Value;
                }
                else
                {
                    IsSealedParameter.Size = -1;
                    IsSealedParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsSealedParameter);

                DbParameter IsRecordParameter = cmd.CreateParameter();
                IsRecordParameter.ParameterName = "IsRecord";
                IsRecordParameter.Direction = ParameterDirection.Input;
                IsRecordParameter.DbType = DbType.Boolean;
                if (IsRecord.HasValue)
                {
                    IsRecordParameter.Value = IsRecord.Value;
                }
                else
                {
                    IsRecordParameter.Size = -1;
                    IsRecordParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsRecordParameter);

                DbParameter IsRefLikeParameter = cmd.CreateParameter();
                IsRefLikeParameter.ParameterName = "IsRefLike";
                IsRefLikeParameter.Direction = ParameterDirection.Input;
                IsRefLikeParameter.DbType = DbType.Boolean;
                if (IsRefLike.HasValue)
                {
                    IsRefLikeParameter.Value = IsRefLike.Value;
                }
                else
                {
                    IsRefLikeParameter.Size = -1;
                    IsRefLikeParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsRefLikeParameter);

                DbParameter BaseTypeUidParameter = cmd.CreateParameter();
                BaseTypeUidParameter.ParameterName = "BaseTypeUid";
                BaseTypeUidParameter.Direction = ParameterDirection.Input;
                BaseTypeUidParameter.DbType = DbType.String;
                BaseTypeUidParameter.Size = 1000;
                if (BaseTypeUid != null)
                {
                    BaseTypeUidParameter.Value = BaseTypeUid;
                }
                else
                {
                    BaseTypeUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(BaseTypeUidParameter);

                DbParameter InterfacesParameter = cmd.CreateParameter();
                InterfacesParameter.ParameterName = "Interfaces";
                InterfacesParameter.Direction = ParameterDirection.Input;
                InterfacesParameter.DbType = DbType.String;
                if (Interfaces != null)
                {
                    InterfacesParameter.Value = Interfaces;
                }
                else
                {
                    InterfacesParameter.Size = -1;
                    InterfacesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(InterfacesParameter);

                DbParameter ContainingTypeUidParameter = cmd.CreateParameter();
                ContainingTypeUidParameter.ParameterName = "ContainingTypeUid";
                ContainingTypeUidParameter.Direction = ParameterDirection.Input;
                ContainingTypeUidParameter.DbType = DbType.String;
                ContainingTypeUidParameter.Size = 1000;
                if (ContainingTypeUid != null)
                {
                    ContainingTypeUidParameter.Value = ContainingTypeUid;
                }
                else
                {
                    ContainingTypeUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ContainingTypeUidParameter);

                DbParameter GenericParametersParameter = cmd.CreateParameter();
                GenericParametersParameter.ParameterName = "GenericParameters";
                GenericParametersParameter.Direction = ParameterDirection.Input;
                GenericParametersParameter.DbType = DbType.String;
                if (GenericParameters != null)
                {
                    GenericParametersParameter.Value = GenericParameters;
                }
                else
                {
                    GenericParametersParameter.Size = -1;
                    GenericParametersParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(GenericParametersParameter);

                DbParameter GenericConstraintsParameter = cmd.CreateParameter();
                GenericConstraintsParameter.ParameterName = "GenericConstraints";
                GenericConstraintsParameter.Direction = ParameterDirection.Input;
                GenericConstraintsParameter.DbType = DbType.String;
                if (GenericConstraints != null)
                {
                    GenericConstraintsParameter.Value = GenericConstraints;
                }
                else
                {
                    GenericConstraintsParameter.Size = -1;
                    GenericConstraintsParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(GenericConstraintsParameter);

                DbParameter SummaryParameter = cmd.CreateParameter();
                SummaryParameter.ParameterName = "Summary";
                SummaryParameter.Direction = ParameterDirection.Input;
                SummaryParameter.DbType = DbType.String;
                if (Summary != null)
                {
                    SummaryParameter.Value = Summary;
                }
                else
                {
                    SummaryParameter.Size = -1;
                    SummaryParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SummaryParameter);

                DbParameter RemarksParameter = cmd.CreateParameter();
                RemarksParameter.ParameterName = "Remarks";
                RemarksParameter.Direction = ParameterDirection.Input;
                RemarksParameter.DbType = DbType.String;
                if (Remarks != null)
                {
                    RemarksParameter.Value = Remarks;
                }
                else
                {
                    RemarksParameter.Size = -1;
                    RemarksParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(RemarksParameter);

                DbParameter AttributesParameter = cmd.CreateParameter();
                AttributesParameter.ParameterName = "Attributes";
                AttributesParameter.Direction = ParameterDirection.Input;
                AttributesParameter.DbType = DbType.String;
                if (Attributes != null)
                {
                    AttributesParameter.Value = Attributes;
                }
                else
                {
                    AttributesParameter.Size = -1;
                    AttributesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AttributesParameter);

                DbParameter SourceFilePathParameter = cmd.CreateParameter();
                SourceFilePathParameter.ParameterName = "SourceFilePath";
                SourceFilePathParameter.Direction = ParameterDirection.Input;
                SourceFilePathParameter.DbType = DbType.String;
                if (SourceFilePath != null)
                {
                    SourceFilePathParameter.Value = SourceFilePath;
                }
                else
                {
                    SourceFilePathParameter.Size = -1;
                    SourceFilePathParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceFilePathParameter);

                DbParameter SourceStartLineParameter = cmd.CreateParameter();
                SourceStartLineParameter.ParameterName = "SourceStartLine";
                SourceStartLineParameter.Direction = ParameterDirection.Input;
                SourceStartLineParameter.DbType = DbType.Int32;
                SourceStartLineParameter.Precision = 10;
                SourceStartLineParameter.Scale = 0;
                if (SourceStartLine.HasValue)
                {
                    SourceStartLineParameter.Value = SourceStartLine.Value;
                }
                else
                {
                    SourceStartLineParameter.Size = -1;
                    SourceStartLineParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceStartLineParameter);

                DbParameter SourceEndLineParameter = cmd.CreateParameter();
                SourceEndLineParameter.ParameterName = "SourceEndLine";
                SourceEndLineParameter.Direction = ParameterDirection.Input;
                SourceEndLineParameter.DbType = DbType.Int32;
                SourceEndLineParameter.Precision = 10;
                SourceEndLineParameter.Scale = 0;
                if (SourceEndLine.HasValue)
                {
                    SourceEndLineParameter.Value = SourceEndLine.Value;
                }
                else
                {
                    SourceEndLineParameter.Size = -1;
                    SourceEndLineParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceEndLineParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task UpsertApiTypeAsync(string SemanticUid, Guid? SourceSnapshotId, Guid? IngestionRunId, string Name, string NamespacePath, string Kind, string Accessibility, bool? IsStatic, bool? IsGeneric, bool? IsAbstract, bool? IsSealed, bool? IsRecord, bool? IsRefLike, string BaseTypeUid, string Interfaces, string ContainingTypeUid, string GenericParameters, string GenericConstraints, string Summary, string Remarks, string Attributes, string SourceFilePath, int? SourceStartLine, int? SourceEndLine)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.UpsertApiType";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter SourceSnapshotIdParameter = cmd.CreateParameter();
                SourceSnapshotIdParameter.ParameterName = "SourceSnapshotId";
                SourceSnapshotIdParameter.Direction = ParameterDirection.Input;
                SourceSnapshotIdParameter.DbType = DbType.Guid;
                if (SourceSnapshotId.HasValue)
                {
                    SourceSnapshotIdParameter.Value = SourceSnapshotId.Value;
                }
                else
                {
                    SourceSnapshotIdParameter.Size = -1;
                    SourceSnapshotIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceSnapshotIdParameter);

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                if (Name != null)
                {
                    NameParameter.Value = Name;
                }
                else
                {
                    NameParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NameParameter);

                DbParameter NamespacePathParameter = cmd.CreateParameter();
                NamespacePathParameter.ParameterName = "NamespacePath";
                NamespacePathParameter.Direction = ParameterDirection.Input;
                NamespacePathParameter.DbType = DbType.String;
                NamespacePathParameter.Size = 1000;
                if (NamespacePath != null)
                {
                    NamespacePathParameter.Value = NamespacePath;
                }
                else
                {
                    NamespacePathParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(NamespacePathParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 200;
                if (Kind != null)
                {
                    KindParameter.Value = Kind;
                }
                else
                {
                    KindParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(KindParameter);

                DbParameter AccessibilityParameter = cmd.CreateParameter();
                AccessibilityParameter.ParameterName = "Accessibility";
                AccessibilityParameter.Direction = ParameterDirection.Input;
                AccessibilityParameter.DbType = DbType.String;
                AccessibilityParameter.Size = 200;
                if (Accessibility != null)
                {
                    AccessibilityParameter.Value = Accessibility;
                }
                else
                {
                    AccessibilityParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AccessibilityParameter);

                DbParameter IsStaticParameter = cmd.CreateParameter();
                IsStaticParameter.ParameterName = "IsStatic";
                IsStaticParameter.Direction = ParameterDirection.Input;
                IsStaticParameter.DbType = DbType.Boolean;
                if (IsStatic.HasValue)
                {
                    IsStaticParameter.Value = IsStatic.Value;
                }
                else
                {
                    IsStaticParameter.Size = -1;
                    IsStaticParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsStaticParameter);

                DbParameter IsGenericParameter = cmd.CreateParameter();
                IsGenericParameter.ParameterName = "IsGeneric";
                IsGenericParameter.Direction = ParameterDirection.Input;
                IsGenericParameter.DbType = DbType.Boolean;
                if (IsGeneric.HasValue)
                {
                    IsGenericParameter.Value = IsGeneric.Value;
                }
                else
                {
                    IsGenericParameter.Size = -1;
                    IsGenericParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsGenericParameter);

                DbParameter IsAbstractParameter = cmd.CreateParameter();
                IsAbstractParameter.ParameterName = "IsAbstract";
                IsAbstractParameter.Direction = ParameterDirection.Input;
                IsAbstractParameter.DbType = DbType.Boolean;
                if (IsAbstract.HasValue)
                {
                    IsAbstractParameter.Value = IsAbstract.Value;
                }
                else
                {
                    IsAbstractParameter.Size = -1;
                    IsAbstractParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsAbstractParameter);

                DbParameter IsSealedParameter = cmd.CreateParameter();
                IsSealedParameter.ParameterName = "IsSealed";
                IsSealedParameter.Direction = ParameterDirection.Input;
                IsSealedParameter.DbType = DbType.Boolean;
                if (IsSealed.HasValue)
                {
                    IsSealedParameter.Value = IsSealed.Value;
                }
                else
                {
                    IsSealedParameter.Size = -1;
                    IsSealedParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsSealedParameter);

                DbParameter IsRecordParameter = cmd.CreateParameter();
                IsRecordParameter.ParameterName = "IsRecord";
                IsRecordParameter.Direction = ParameterDirection.Input;
                IsRecordParameter.DbType = DbType.Boolean;
                if (IsRecord.HasValue)
                {
                    IsRecordParameter.Value = IsRecord.Value;
                }
                else
                {
                    IsRecordParameter.Size = -1;
                    IsRecordParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsRecordParameter);

                DbParameter IsRefLikeParameter = cmd.CreateParameter();
                IsRefLikeParameter.ParameterName = "IsRefLike";
                IsRefLikeParameter.Direction = ParameterDirection.Input;
                IsRefLikeParameter.DbType = DbType.Boolean;
                if (IsRefLike.HasValue)
                {
                    IsRefLikeParameter.Value = IsRefLike.Value;
                }
                else
                {
                    IsRefLikeParameter.Size = -1;
                    IsRefLikeParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IsRefLikeParameter);

                DbParameter BaseTypeUidParameter = cmd.CreateParameter();
                BaseTypeUidParameter.ParameterName = "BaseTypeUid";
                BaseTypeUidParameter.Direction = ParameterDirection.Input;
                BaseTypeUidParameter.DbType = DbType.String;
                BaseTypeUidParameter.Size = 1000;
                if (BaseTypeUid != null)
                {
                    BaseTypeUidParameter.Value = BaseTypeUid;
                }
                else
                {
                    BaseTypeUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(BaseTypeUidParameter);

                DbParameter InterfacesParameter = cmd.CreateParameter();
                InterfacesParameter.ParameterName = "Interfaces";
                InterfacesParameter.Direction = ParameterDirection.Input;
                InterfacesParameter.DbType = DbType.String;
                if (Interfaces != null)
                {
                    InterfacesParameter.Value = Interfaces;
                }
                else
                {
                    InterfacesParameter.Size = -1;
                    InterfacesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(InterfacesParameter);

                DbParameter ContainingTypeUidParameter = cmd.CreateParameter();
                ContainingTypeUidParameter.ParameterName = "ContainingTypeUid";
                ContainingTypeUidParameter.Direction = ParameterDirection.Input;
                ContainingTypeUidParameter.DbType = DbType.String;
                ContainingTypeUidParameter.Size = 1000;
                if (ContainingTypeUid != null)
                {
                    ContainingTypeUidParameter.Value = ContainingTypeUid;
                }
                else
                {
                    ContainingTypeUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(ContainingTypeUidParameter);

                DbParameter GenericParametersParameter = cmd.CreateParameter();
                GenericParametersParameter.ParameterName = "GenericParameters";
                GenericParametersParameter.Direction = ParameterDirection.Input;
                GenericParametersParameter.DbType = DbType.String;
                if (GenericParameters != null)
                {
                    GenericParametersParameter.Value = GenericParameters;
                }
                else
                {
                    GenericParametersParameter.Size = -1;
                    GenericParametersParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(GenericParametersParameter);

                DbParameter GenericConstraintsParameter = cmd.CreateParameter();
                GenericConstraintsParameter.ParameterName = "GenericConstraints";
                GenericConstraintsParameter.Direction = ParameterDirection.Input;
                GenericConstraintsParameter.DbType = DbType.String;
                if (GenericConstraints != null)
                {
                    GenericConstraintsParameter.Value = GenericConstraints;
                }
                else
                {
                    GenericConstraintsParameter.Size = -1;
                    GenericConstraintsParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(GenericConstraintsParameter);

                DbParameter SummaryParameter = cmd.CreateParameter();
                SummaryParameter.ParameterName = "Summary";
                SummaryParameter.Direction = ParameterDirection.Input;
                SummaryParameter.DbType = DbType.String;
                if (Summary != null)
                {
                    SummaryParameter.Value = Summary;
                }
                else
                {
                    SummaryParameter.Size = -1;
                    SummaryParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SummaryParameter);

                DbParameter RemarksParameter = cmd.CreateParameter();
                RemarksParameter.ParameterName = "Remarks";
                RemarksParameter.Direction = ParameterDirection.Input;
                RemarksParameter.DbType = DbType.String;
                if (Remarks != null)
                {
                    RemarksParameter.Value = Remarks;
                }
                else
                {
                    RemarksParameter.Size = -1;
                    RemarksParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(RemarksParameter);

                DbParameter AttributesParameter = cmd.CreateParameter();
                AttributesParameter.ParameterName = "Attributes";
                AttributesParameter.Direction = ParameterDirection.Input;
                AttributesParameter.DbType = DbType.String;
                if (Attributes != null)
                {
                    AttributesParameter.Value = Attributes;
                }
                else
                {
                    AttributesParameter.Size = -1;
                    AttributesParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AttributesParameter);

                DbParameter SourceFilePathParameter = cmd.CreateParameter();
                SourceFilePathParameter.ParameterName = "SourceFilePath";
                SourceFilePathParameter.Direction = ParameterDirection.Input;
                SourceFilePathParameter.DbType = DbType.String;
                if (SourceFilePath != null)
                {
                    SourceFilePathParameter.Value = SourceFilePath;
                }
                else
                {
                    SourceFilePathParameter.Size = -1;
                    SourceFilePathParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceFilePathParameter);

                DbParameter SourceStartLineParameter = cmd.CreateParameter();
                SourceStartLineParameter.ParameterName = "SourceStartLine";
                SourceStartLineParameter.Direction = ParameterDirection.Input;
                SourceStartLineParameter.DbType = DbType.Int32;
                SourceStartLineParameter.Precision = 10;
                SourceStartLineParameter.Scale = 0;
                if (SourceStartLine.HasValue)
                {
                    SourceStartLineParameter.Value = SourceStartLine.Value;
                }
                else
                {
                    SourceStartLineParameter.Size = -1;
                    SourceStartLineParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceStartLineParameter);

                DbParameter SourceEndLineParameter = cmd.CreateParameter();
                SourceEndLineParameter.ParameterName = "SourceEndLine";
                SourceEndLineParameter.Direction = ParameterDirection.Input;
                SourceEndLineParameter.DbType = DbType.Int32;
                SourceEndLineParameter.Precision = 10;
                SourceEndLineParameter.Scale = 0;
                if (SourceEndLine.HasValue)
                {
                    SourceEndLineParameter.Value = SourceEndLine.Value;
                }
                else
                {
                    SourceEndLineParameter.Size = -1;
                    SourceEndLineParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceEndLineParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void UpsertDocPage(string SemanticUid, Guid? SourceSnapshotId, Guid? IngestionRunId, string SourcePath, string Title, string Language, string Url, string RawMarkdown)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.UpsertDocPage";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter SourceSnapshotIdParameter = cmd.CreateParameter();
                SourceSnapshotIdParameter.ParameterName = "SourceSnapshotId";
                SourceSnapshotIdParameter.Direction = ParameterDirection.Input;
                SourceSnapshotIdParameter.DbType = DbType.Guid;
                if (SourceSnapshotId.HasValue)
                {
                    SourceSnapshotIdParameter.Value = SourceSnapshotId.Value;
                }
                else
                {
                    SourceSnapshotIdParameter.Size = -1;
                    SourceSnapshotIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceSnapshotIdParameter);

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter SourcePathParameter = cmd.CreateParameter();
                SourcePathParameter.ParameterName = "SourcePath";
                SourcePathParameter.Direction = ParameterDirection.Input;
                SourcePathParameter.DbType = DbType.String;
                if (SourcePath != null)
                {
                    SourcePathParameter.Value = SourcePath;
                }
                else
                {
                    SourcePathParameter.Size = -1;
                    SourcePathParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourcePathParameter);

                DbParameter TitleParameter = cmd.CreateParameter();
                TitleParameter.ParameterName = "Title";
                TitleParameter.Direction = ParameterDirection.Input;
                TitleParameter.DbType = DbType.String;
                TitleParameter.Size = 400;
                if (Title != null)
                {
                    TitleParameter.Value = Title;
                }
                else
                {
                    TitleParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(TitleParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                if (Language != null)
                {
                    LanguageParameter.Value = Language;
                }
                else
                {
                    LanguageParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(LanguageParameter);

                DbParameter UrlParameter = cmd.CreateParameter();
                UrlParameter.ParameterName = "Url";
                UrlParameter.Direction = ParameterDirection.Input;
                UrlParameter.DbType = DbType.String;
                if (Url != null)
                {
                    UrlParameter.Value = Url;
                }
                else
                {
                    UrlParameter.Size = -1;
                    UrlParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(UrlParameter);

                DbParameter RawMarkdownParameter = cmd.CreateParameter();
                RawMarkdownParameter.ParameterName = "RawMarkdown";
                RawMarkdownParameter.Direction = ParameterDirection.Input;
                RawMarkdownParameter.DbType = DbType.String;
                if (RawMarkdown != null)
                {
                    RawMarkdownParameter.Value = RawMarkdown;
                }
                else
                {
                    RawMarkdownParameter.Size = -1;
                    RawMarkdownParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(RawMarkdownParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task UpsertDocPageAsync(string SemanticUid, Guid? SourceSnapshotId, Guid? IngestionRunId, string SourcePath, string Title, string Language, string Url, string RawMarkdown)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.UpsertDocPage";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter SourceSnapshotIdParameter = cmd.CreateParameter();
                SourceSnapshotIdParameter.ParameterName = "SourceSnapshotId";
                SourceSnapshotIdParameter.Direction = ParameterDirection.Input;
                SourceSnapshotIdParameter.DbType = DbType.Guid;
                if (SourceSnapshotId.HasValue)
                {
                    SourceSnapshotIdParameter.Value = SourceSnapshotId.Value;
                }
                else
                {
                    SourceSnapshotIdParameter.Size = -1;
                    SourceSnapshotIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourceSnapshotIdParameter);

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter SourcePathParameter = cmd.CreateParameter();
                SourcePathParameter.ParameterName = "SourcePath";
                SourcePathParameter.Direction = ParameterDirection.Input;
                SourcePathParameter.DbType = DbType.String;
                if (SourcePath != null)
                {
                    SourcePathParameter.Value = SourcePath;
                }
                else
                {
                    SourcePathParameter.Size = -1;
                    SourcePathParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SourcePathParameter);

                DbParameter TitleParameter = cmd.CreateParameter();
                TitleParameter.ParameterName = "Title";
                TitleParameter.Direction = ParameterDirection.Input;
                TitleParameter.DbType = DbType.String;
                TitleParameter.Size = 400;
                if (Title != null)
                {
                    TitleParameter.Value = Title;
                }
                else
                {
                    TitleParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(TitleParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                if (Language != null)
                {
                    LanguageParameter.Value = Language;
                }
                else
                {
                    LanguageParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(LanguageParameter);

                DbParameter UrlParameter = cmd.CreateParameter();
                UrlParameter.ParameterName = "Url";
                UrlParameter.Direction = ParameterDirection.Input;
                UrlParameter.DbType = DbType.String;
                if (Url != null)
                {
                    UrlParameter.Value = Url;
                }
                else
                {
                    UrlParameter.Size = -1;
                    UrlParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(UrlParameter);

                DbParameter RawMarkdownParameter = cmd.CreateParameter();
                RawMarkdownParameter.ParameterName = "RawMarkdown";
                RawMarkdownParameter.Direction = ParameterDirection.Input;
                RawMarkdownParameter.DbType = DbType.String;
                if (RawMarkdown != null)
                {
                    RawMarkdownParameter.Value = RawMarkdown;
                }
                else
                {
                    RawMarkdownParameter.Size = -1;
                    RawMarkdownParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(RawMarkdownParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void VerifyIngestionRun(Guid? IngestionRunId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.VerifyIngestionRun";

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task VerifyIngestionRunAsync(Guid? IngestionRunId)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.VerifyIngestionRun";

                DbParameter IngestionRunIdParameter = cmd.CreateParameter();
                IngestionRunIdParameter.ParameterName = "IngestionRunId";
                IngestionRunIdParameter.Direction = ParameterDirection.Input;
                IngestionRunIdParameter.DbType = DbType.Guid;
                if (IngestionRunId.HasValue)
                {
                    IngestionRunIdParameter.Value = IngestionRunId.Value;
                }
                else
                {
                    IngestionRunIdParameter.Size = -1;
                    IngestionRunIdParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(IngestionRunIdParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public byte[] FnComputeContentHash256(string Input)
    {

        byte[] result;
        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.fn_ComputeContentHash256";

                DbParameter InputParameter = cmd.CreateParameter();
                InputParameter.ParameterName = "Input";
                InputParameter.Direction = ParameterDirection.Input;
                InputParameter.DbType = DbType.String;
                if (Input != null)
                {
                    InputParameter.Value = Input;
                }
                else
                {
                    InputParameter.Size = -1;
                    InputParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(InputParameter);

                DbParameter returnValueParameter = cmd.CreateParameter();
                returnValueParameter.Direction = ParameterDirection.ReturnValue;
                returnValueParameter.DbType = DbType.Binary;
                returnValueParameter.Size = -1;
                cmd.Parameters.Add(returnValueParameter);
                cmd.ExecuteNonQuery();
                if (returnValueParameter.Value != null && !(returnValueParameter.Value is DBNull))
                {
                    result = (byte[])Convert.ChangeType(returnValueParameter.Value,
                        typeof(byte[]));
                }
                else
                {
                    result = default;
                }
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }

        return result;
    }







    public async Task<byte[]> FnComputeContentHash256Async(string Input)
    {

        byte[] result;
        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = @"dbo.fn_ComputeContentHash256";

                DbParameter InputParameter = cmd.CreateParameter();
                InputParameter.ParameterName = "Input";
                InputParameter.Direction = ParameterDirection.Input;
                InputParameter.DbType = DbType.String;
                if (Input != null)
                {
                    InputParameter.Value = Input;
                }
                else
                {
                    InputParameter.Size = -1;
                    InputParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(InputParameter);

                DbParameter returnValueParameter = cmd.CreateParameter();
                returnValueParameter.Direction = ParameterDirection.ReturnValue;
                returnValueParameter.DbType = DbType.Binary;
                returnValueParameter.Size = -1;
                cmd.Parameters.Add(returnValueParameter);
                await cmd.ExecuteNonQueryAsync();
                if (returnValueParameter.Value != null && !(returnValueParameter.Value is DBNull))
                {
                    result = (byte[])Convert.ChangeType(returnValueParameter.Value,
                        typeof(byte[]));
                }
                else
                {
                    result = default;
                }
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }

        return result;
    }







    public void FnGetDocPageAsOf(string SemanticUid, DateTime? AsOfUtc)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select * from dbo.fn_GetDocPageAsOf(@SemanticUid, @AsOfUtc)";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter AsOfUtcParameter = cmd.CreateParameter();
                AsOfUtcParameter.ParameterName = "AsOfUtc";
                AsOfUtcParameter.Direction = ParameterDirection.Input;
                AsOfUtcParameter.DbType = DbType.DateTime;
                if (AsOfUtc.HasValue)
                {
                    AsOfUtcParameter.Value = AsOfUtc.Value;
                }
                else
                {
                    AsOfUtcParameter.Size = -1;
                    AsOfUtcParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AsOfUtcParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task FnGetDocPageAsOfAsync(string SemanticUid, DateTime? AsOfUtc)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select * from dbo.fn_GetDocPageAsOf(@SemanticUid, @AsOfUtc)";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter AsOfUtcParameter = cmd.CreateParameter();
                AsOfUtcParameter.ParameterName = "AsOfUtc";
                AsOfUtcParameter.Direction = ParameterDirection.Input;
                AsOfUtcParameter.DbType = DbType.DateTime;
                if (AsOfUtc.HasValue)
                {
                    AsOfUtcParameter.Value = AsOfUtc.Value;
                }
                else
                {
                    AsOfUtcParameter.Size = -1;
                    AsOfUtcParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AsOfUtcParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void FnGetFeatureAsOf(string SemanticUid, DateTime? AsOfUtc)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select * from dbo.fn_GetFeatureAsOf(@SemanticUid, @AsOfUtc)";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter AsOfUtcParameter = cmd.CreateParameter();
                AsOfUtcParameter.ParameterName = "AsOfUtc";
                AsOfUtcParameter.Direction = ParameterDirection.Input;
                AsOfUtcParameter.DbType = DbType.DateTime;
                if (AsOfUtc.HasValue)
                {
                    AsOfUtcParameter.Value = AsOfUtc.Value;
                }
                else
                {
                    AsOfUtcParameter.Size = -1;
                    AsOfUtcParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AsOfUtcParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task FnGetFeatureAsOfAsync(string SemanticUid, DateTime? AsOfUtc)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select * from dbo.fn_GetFeatureAsOf(@SemanticUid, @AsOfUtc)";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter AsOfUtcParameter = cmd.CreateParameter();
                AsOfUtcParameter.ParameterName = "AsOfUtc";
                AsOfUtcParameter.Direction = ParameterDirection.Input;
                AsOfUtcParameter.DbType = DbType.DateTime;
                if (AsOfUtc.HasValue)
                {
                    AsOfUtcParameter.Value = AsOfUtc.Value;
                }
                else
                {
                    AsOfUtcParameter.Size = -1;
                    AsOfUtcParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AsOfUtcParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void FnGetMemberAsOf(string SemanticUid, DateTime? AsOfUtc)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select * from dbo.fn_GetMemberAsOf(@SemanticUid, @AsOfUtc)";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter AsOfUtcParameter = cmd.CreateParameter();
                AsOfUtcParameter.ParameterName = "AsOfUtc";
                AsOfUtcParameter.Direction = ParameterDirection.Input;
                AsOfUtcParameter.DbType = DbType.DateTime;
                if (AsOfUtc.HasValue)
                {
                    AsOfUtcParameter.Value = AsOfUtc.Value;
                }
                else
                {
                    AsOfUtcParameter.Size = -1;
                    AsOfUtcParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AsOfUtcParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task FnGetMemberAsOfAsync(string SemanticUid, DateTime? AsOfUtc)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select * from dbo.fn_GetMemberAsOf(@SemanticUid, @AsOfUtc)";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter AsOfUtcParameter = cmd.CreateParameter();
                AsOfUtcParameter.ParameterName = "AsOfUtc";
                AsOfUtcParameter.Direction = ParameterDirection.Input;
                AsOfUtcParameter.DbType = DbType.DateTime;
                if (AsOfUtc.HasValue)
                {
                    AsOfUtcParameter.Value = AsOfUtc.Value;
                }
                else
                {
                    AsOfUtcParameter.Size = -1;
                    AsOfUtcParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AsOfUtcParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public void FnGetTypeAsOf(string SemanticUid, DateTime? AsOfUtc)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select * from dbo.fn_GetTypeAsOf(@SemanticUid, @AsOfUtc)";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter AsOfUtcParameter = cmd.CreateParameter();
                AsOfUtcParameter.ParameterName = "AsOfUtc";
                AsOfUtcParameter.Direction = ParameterDirection.Input;
                AsOfUtcParameter.DbType = DbType.DateTime;
                if (AsOfUtc.HasValue)
                {
                    AsOfUtcParameter.Value = AsOfUtc.Value;
                }
                else
                {
                    AsOfUtcParameter.Size = -1;
                    AsOfUtcParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AsOfUtcParameter);
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }







    public async Task FnGetTypeAsOfAsync(string SemanticUid, DateTime? AsOfUtc)
    {

        DbConnection connection = Database.GetDbConnection();
        var needClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
            needClose = true;
        }

        try
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                if (Database.GetCommandTimeout().HasValue)
                {
                    cmd.CommandTimeout = Database.GetCommandTimeout().Value;
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select * from dbo.fn_GetTypeAsOf(@SemanticUid, @AsOfUtc)";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                if (SemanticUid != null)
                {
                    SemanticUidParameter.Value = SemanticUid;
                }
                else
                {
                    SemanticUidParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(SemanticUidParameter);

                DbParameter AsOfUtcParameter = cmd.CreateParameter();
                AsOfUtcParameter.ParameterName = "AsOfUtc";
                AsOfUtcParameter.Direction = ParameterDirection.Input;
                AsOfUtcParameter.DbType = DbType.DateTime;
                if (AsOfUtc.HasValue)
                {
                    AsOfUtcParameter.Value = AsOfUtc.Value;
                }
                else
                {
                    AsOfUtcParameter.Size = -1;
                    AsOfUtcParameter.Value = DBNull.Value;
                }

                cmd.Parameters.Add(AsOfUtcParameter);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        finally
        {
            if (needClose)
            {
                connection.Close();
            }
        }
    }

    #endregion

    #region ApiFeature Mapping

    private void ApiFeatureMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiFeature>().ToTable(@"ApiFeatures");
        modelBuilder.Entity<ApiFeature>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<ApiFeature>().Property(x => x.SemanticUid).HasColumnName(@"SemanticUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ApiFeature>().Property(x => x.TruthRunId).HasColumnName(@"TruthRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiFeature>().Property(x => x.Name).HasColumnName(@"Name").ValueGeneratedNever().HasMaxLength(400);
        modelBuilder.Entity<ApiFeature>().Property(x => x.Language).HasColumnName(@"Language").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ApiFeature>().Property(x => x.Description).HasColumnName(@"Description").ValueGeneratedNever();
        modelBuilder.Entity<ApiFeature>().Property(x => x.Tags).HasColumnName(@"Tags").ValueGeneratedNever();
        modelBuilder.Entity<ApiFeature>().Property(x => x.VersionNumber).HasColumnName(@"VersionNumber").IsRequired().ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<ApiFeature>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"CreatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiFeature>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"UpdatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiFeature>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"RemovedIngestionRunId").ValueGeneratedNever();
        modelBuilder.Entity<ApiFeature>().Property(x => x.ValidFromUtc).HasColumnName(@"ValidFromUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiFeature>().Property(x => x.ValidToUtc).HasColumnName(@"ValidToUtc").ValueGeneratedNever();
        modelBuilder.Entity<ApiFeature>().Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        modelBuilder.Entity<ApiFeature>().Property(x => x.ContentHash).HasColumnName(@"ContentHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<ApiFeature>().Property(x => x.SemanticUidHash).HasColumnName(@"SemanticUidHash").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        modelBuilder.Entity<ApiFeature>().HasKey(@"Id");
        modelBuilder.Entity<ApiFeature>().HasIndex(@"SemanticUid",
            @"VersionNumber").IsUnique().HasDatabaseName(@"uq_api_feature_semantic_version");
    }







    partial void CustomizeApiFeatureMapping(ModelBuilder modelBuilder);

    #endregion

    #region ApiMember Mapping

    private void ApiMemberMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiMember>().ToTable(@"ApiMembers");
        modelBuilder.Entity<ApiMember>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<ApiMember>().Property(x => x.SemanticUid).HasColumnName(@"SemanticUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ApiMember>().Property(x => x.ApiTypeId).HasColumnName(@"ApiTypeId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.Name).HasColumnName(@"Name").ValueGeneratedNever().HasMaxLength(400);
        modelBuilder.Entity<ApiMember>().Property(x => x.Kind).HasColumnName(@"Kind").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ApiMember>().Property(x => x.MethodKind).HasColumnName(@"MethodKind").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ApiMember>().Property(x => x.Accessibility).HasColumnName(@"Accessibility").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ApiMember>().Property(x => x.IsStatic).HasColumnName(@"IsStatic").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.IsExtensionMethod).HasColumnName(@"IsExtensionMethod").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.IsAsync).HasColumnName(@"IsAsync").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.IsVirtual).HasColumnName(@"IsVirtual").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.IsOverride).HasColumnName(@"IsOverride").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.IsAbstract).HasColumnName(@"IsAbstract").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.IsSealed).HasColumnName(@"IsSealed").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.IsReadonly).HasColumnName(@"IsReadonly").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.IsConst).HasColumnName(@"IsConst").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.IsUnsafe).HasColumnName(@"IsUnsafe").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.ReturnTypeUid).HasColumnName(@"ReturnTypeUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ApiMember>().Property(x => x.ReturnNullable).HasColumnName(@"ReturnNullable").ValueGeneratedNever().HasMaxLength(50);
        modelBuilder.Entity<ApiMember>().Property(x => x.GenericParameters).HasColumnName(@"GenericParameters").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.GenericConstraints).HasColumnName(@"GenericConstraints").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.Summary).HasColumnName(@"Summary").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.Remarks).HasColumnName(@"Remarks").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.Attributes).HasColumnName(@"Attributes").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.SourceFilePath).HasColumnName(@"SourceFilePath").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.SourceStartLine).HasColumnName(@"SourceStartLine").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<ApiMember>().Property(x => x.SourceEndLine).HasColumnName(@"SourceEndLine").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<ApiMember>().Property(x => x.MemberUidHash).HasColumnName(@"MemberUidHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<ApiMember>().Property(x => x.VersionNumber).HasColumnName(@"VersionNumber").IsRequired().ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<ApiMember>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"CreatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"UpdatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"RemovedIngestionRunId").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.ValidFromUtc).HasColumnName(@"ValidFromUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.ValidToUtc).HasColumnName(@"ValidToUtc").ValueGeneratedNever();
        modelBuilder.Entity<ApiMember>().Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        modelBuilder.Entity<ApiMember>().Property(x => x.ContentHash).HasColumnName(@"ContentHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<ApiMember>().Property(x => x.SemanticUidHash).HasColumnName(@"SemanticUidHash").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        modelBuilder.Entity<ApiMember>().HasKey(@"Id");
        modelBuilder.Entity<ApiMember>().HasIndex(@"ApiTypeId",
            @"MemberUidHash").IsUnique().HasDatabaseName(@"ix_api_member_type_hash");
        modelBuilder.Entity<ApiMember>().HasIndex(@"VersionNumber",
            @"SemanticUidHash").IsUnique().HasDatabaseName(@"uq_api_member_semantic_version");
    }







    partial void CustomizeApiMemberMapping(ModelBuilder modelBuilder);

    #endregion

    #region ApiMemberDiff Mapping

    private void ApiMemberDiffMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiMemberDiff>().ToTable(@"ApiMemberDiffs");
        modelBuilder.Entity<ApiMemberDiff>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<ApiMemberDiff>().Property(x => x.SnapshotDiffId).HasColumnName(@"SnapshotDiffId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiMemberDiff>().Property(x => x.MemberUid).HasColumnName(@"MemberUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ApiMemberDiff>().Property(x => x.ChangeKind).HasColumnName(@"ChangeKind").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ApiMemberDiff>().Property(x => x.OldSignature).HasColumnName(@"OldSignature").ValueGeneratedNever();
        modelBuilder.Entity<ApiMemberDiff>().Property(x => x.NewSignature).HasColumnName(@"NewSignature").ValueGeneratedNever();
        modelBuilder.Entity<ApiMemberDiff>().Property(x => x.Breaking).HasColumnName(@"Breaking").ValueGeneratedNever();
        modelBuilder.Entity<ApiMemberDiff>().Property(x => x.DetailJson).HasColumnName(@"DetailJson").ValueGeneratedNever();
        modelBuilder.Entity<ApiMemberDiff>().HasKey(@"Id");
    }







    partial void CustomizeApiMemberDiffMapping(ModelBuilder modelBuilder);

    #endregion

    #region ApiParameter Mapping

    private void ApiParameterMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiParameter>().ToTable(@"ApiParameters");
        modelBuilder.Entity<ApiParameter>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<ApiParameter>().Property(x => x.ApiMemberId).HasColumnName(@"ApiMemberId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiParameter>().Property(x => x.Name).HasColumnName(@"Name").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ApiParameter>().Property(x => x.TypeUid).HasColumnName(@"TypeUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ApiParameter>().Property(x => x.NullableAnnotation).HasColumnName(@"NullableAnnotation").ValueGeneratedNever().HasMaxLength(50);
        modelBuilder.Entity<ApiParameter>().Property(x => x.Position).HasColumnName(@"Position").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<ApiParameter>().Property(x => x.Modifier).HasColumnName(@"Modifier").ValueGeneratedNever().HasMaxLength(50);
        modelBuilder.Entity<ApiParameter>().Property(x => x.HasDefaultValue).HasColumnName(@"HasDefaultValue").ValueGeneratedNever();
        modelBuilder.Entity<ApiParameter>().Property(x => x.DefaultValueLiteral).HasColumnName(@"DefaultValueLiteral").ValueGeneratedNever();
        modelBuilder.Entity<ApiParameter>().Property(x => x.VersionNumber).HasColumnName(@"VersionNumber").IsRequired().ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<ApiParameter>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"CreatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiParameter>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"UpdatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiParameter>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"RemovedIngestionRunId").ValueGeneratedNever();
        modelBuilder.Entity<ApiParameter>().Property(x => x.ValidFromUtc).HasColumnName(@"ValidFromUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiParameter>().Property(x => x.ValidToUtc).HasColumnName(@"ValidToUtc").ValueGeneratedNever();
        modelBuilder.Entity<ApiParameter>().Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        modelBuilder.Entity<ApiParameter>().Property(x => x.ContentHash).HasColumnName(@"ContentHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<ApiParameter>().HasKey(@"Id");
        modelBuilder.Entity<ApiParameter>().HasIndex(@"ApiMemberId",
            @"Position",
            @"VersionNumber").IsUnique().HasDatabaseName(@"uq_api_parameter_member_position_version");
    }







    partial void CustomizeApiParameterMapping(ModelBuilder modelBuilder);

    #endregion

    #region ApiType Mapping

    private void ApiTypeMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiType>().ToTable(@"ApiTypes");
        modelBuilder.Entity<ApiType>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<ApiType>().Property(x => x.SemanticUid).HasColumnName(@"SemanticUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ApiType>().Property(x => x.SourceSnapshotId).HasColumnName(@"SourceSnapshotId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.Name).HasColumnName(@"Name").ValueGeneratedNever().HasMaxLength(400);
        modelBuilder.Entity<ApiType>().Property(x => x.NamespacePath).HasColumnName(@"NamespacePath").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ApiType>().Property(x => x.Kind).HasColumnName(@"Kind").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ApiType>().Property(x => x.Accessibility).HasColumnName(@"Accessibility").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ApiType>().Property(x => x.IsStatic).HasColumnName(@"IsStatic").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.IsGeneric).HasColumnName(@"IsGeneric").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.IsAbstract).HasColumnName(@"IsAbstract").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.IsSealed).HasColumnName(@"IsSealed").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.IsRecord).HasColumnName(@"IsRecord").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.IsRefLike).HasColumnName(@"IsRefLike").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.BaseTypeUid).HasColumnName(@"BaseTypeUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ApiType>().Property(x => x.Interfaces).HasColumnName(@"Interfaces").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.ContainingTypeUid).HasColumnName(@"ContainingTypeUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ApiType>().Property(x => x.GenericParameters).HasColumnName(@"GenericParameters").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.GenericConstraints).HasColumnName(@"GenericConstraints").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.Summary).HasColumnName(@"Summary").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.Remarks).HasColumnName(@"Remarks").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.Attributes).HasColumnName(@"Attributes").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.SourceFilePath).HasColumnName(@"SourceFilePath").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.SourceStartLine).HasColumnName(@"SourceStartLine").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<ApiType>().Property(x => x.SourceEndLine).HasColumnName(@"SourceEndLine").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<ApiType>().Property(x => x.VersionNumber).HasColumnName(@"VersionNumber").IsRequired().ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<ApiType>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"CreatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"UpdatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"RemovedIngestionRunId").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.ValidFromUtc).HasColumnName(@"ValidFromUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.ValidToUtc).HasColumnName(@"ValidToUtc").ValueGeneratedNever();
        modelBuilder.Entity<ApiType>().Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        modelBuilder.Entity<ApiType>().Property(x => x.ContentHash).HasColumnName(@"ContentHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<ApiType>().Property(x => x.SemanticUidHash).HasColumnName(@"SemanticUidHash").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        modelBuilder.Entity<ApiType>().HasKey(@"Id");
        modelBuilder.Entity<ApiType>().HasIndex(@"VersionNumber",
            @"SemanticUidHash").IsUnique().HasDatabaseName(@"uq_api_type_semantic_version");
    }







    partial void CustomizeApiTypeMapping(ModelBuilder modelBuilder);

    #endregion

    #region ApiTypeDiff Mapping

    private void ApiTypeDiffMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiTypeDiff>().ToTable(@"ApiTypeDiffs");
        modelBuilder.Entity<ApiTypeDiff>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<ApiTypeDiff>().Property(x => x.SnapshotDiffId).HasColumnName(@"SnapshotDiffId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ApiTypeDiff>().Property(x => x.TypeUid).HasColumnName(@"TypeUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ApiTypeDiff>().Property(x => x.ChangeKind).HasColumnName(@"ChangeKind").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ApiTypeDiff>().Property(x => x.DetailJson).HasColumnName(@"DetailJson").ValueGeneratedNever();
        modelBuilder.Entity<ApiTypeDiff>().HasKey(@"Id");
    }







    partial void CustomizeApiTypeDiffMapping(ModelBuilder modelBuilder);

    #endregion

    #region CodeBlock Mapping

    private void CodeBlockMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CodeBlock>().ToTable(@"CodeBlocks");
        modelBuilder.Entity<CodeBlock>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<CodeBlock>().Property(x => x.DocSectionId).HasColumnName(@"DocSectionId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<CodeBlock>().Property(x => x.SemanticUid).HasColumnName(@"SemanticUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<CodeBlock>().Property(x => x.Language).HasColumnName(@"Language").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<CodeBlock>().Property(x => x.Content).HasColumnName(@"Content").ValueGeneratedNever();
        modelBuilder.Entity<CodeBlock>().Property(x => x.DeclaredPackages).HasColumnName(@"DeclaredPackages").ValueGeneratedNever();
        modelBuilder.Entity<CodeBlock>().Property(x => x.Tags).HasColumnName(@"Tags").ValueGeneratedNever();
        modelBuilder.Entity<CodeBlock>().Property(x => x.InlineComments).HasColumnName(@"InlineComments").ValueGeneratedNever();
        modelBuilder.Entity<CodeBlock>().Property(x => x.VersionNumber).HasColumnName(@"VersionNumber").IsRequired().ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<CodeBlock>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"CreatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<CodeBlock>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"UpdatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<CodeBlock>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"RemovedIngestionRunId").ValueGeneratedNever();
        modelBuilder.Entity<CodeBlock>().Property(x => x.ValidFromUtc).HasColumnName(@"ValidFromUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<CodeBlock>().Property(x => x.ValidToUtc).HasColumnName(@"ValidToUtc").ValueGeneratedNever();
        modelBuilder.Entity<CodeBlock>().Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        modelBuilder.Entity<CodeBlock>().Property(x => x.ContentHash).HasColumnName(@"ContentHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<CodeBlock>().HasKey(@"Id");
        modelBuilder.Entity<CodeBlock>().HasIndex(@"SemanticUid",
            @"VersionNumber").IsUnique().HasDatabaseName(@"ix_code_block_semantic_version");
    }







    partial void CustomizeCodeBlockMapping(ModelBuilder modelBuilder);

    #endregion

    #region DocPage Mapping

    private void DocPageMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocPage>().ToTable(@"DocPages");
        modelBuilder.Entity<DocPage>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<DocPage>().Property(x => x.SemanticUid).HasColumnName(@"SemanticUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<DocPage>().Property(x => x.SourceSnapshotId).HasColumnName(@"SourceSnapshotId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<DocPage>().Property(x => x.SourcePath).HasColumnName(@"SourcePath").ValueGeneratedNever();
        modelBuilder.Entity<DocPage>().Property(x => x.Title).HasColumnName(@"Title").ValueGeneratedNever().HasMaxLength(400);
        modelBuilder.Entity<DocPage>().Property(x => x.Language).HasColumnName(@"Language").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<DocPage>().Property(x => x.Url).HasColumnName(@"Url").ValueGeneratedNever();
        modelBuilder.Entity<DocPage>().Property(x => x.RawMarkdown).HasColumnName(@"RawMarkdown").ValueGeneratedNever();
        modelBuilder.Entity<DocPage>().Property(x => x.VersionNumber).HasColumnName(@"VersionNumber").IsRequired().ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<DocPage>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"CreatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<DocPage>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"UpdatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<DocPage>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"RemovedIngestionRunId").ValueGeneratedNever();
        modelBuilder.Entity<DocPage>().Property(x => x.ValidFromUtc).HasColumnName(@"ValidFromUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<DocPage>().Property(x => x.ValidToUtc).HasColumnName(@"ValidToUtc").ValueGeneratedNever();
        modelBuilder.Entity<DocPage>().Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        modelBuilder.Entity<DocPage>().Property(x => x.ContentHash).HasColumnName(@"ContentHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<DocPage>().Property(x => x.SemanticUidHash).HasColumnName(@"SemanticUidHash").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        modelBuilder.Entity<DocPage>().HasKey(@"Id");
        modelBuilder.Entity<DocPage>().HasIndex(@"SemanticUid",
            @"VersionNumber").IsUnique().HasDatabaseName(@"uq_doc_page_semantic_version");
    }







    partial void CustomizeDocPageMapping(ModelBuilder modelBuilder);

    #endregion

    #region DocPageDiff Mapping

    private void DocPageDiffMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocPageDiff>().ToTable(@"DocPageDiffs");
        modelBuilder.Entity<DocPageDiff>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<DocPageDiff>().Property(x => x.SnapshotDiffId).HasColumnName(@"SnapshotDiffId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<DocPageDiff>().Property(x => x.DocUid).HasColumnName(@"DocUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<DocPageDiff>().Property(x => x.ChangeKind).HasColumnName(@"ChangeKind").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<DocPageDiff>().Property(x => x.DetailJson).HasColumnName(@"DetailJson").ValueGeneratedNever();
        modelBuilder.Entity<DocPageDiff>().HasKey(@"Id");
    }







    partial void CustomizeDocPageDiffMapping(ModelBuilder modelBuilder);

    #endregion

    #region DocSection Mapping

    private void DocSectionMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocSection>().ToTable(@"DocSections");
        modelBuilder.Entity<DocSection>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<DocSection>().Property(x => x.DocPageId).HasColumnName(@"DocPageId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<DocSection>().Property(x => x.SemanticUid).HasColumnName(@"SemanticUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<DocSection>().Property(x => x.Heading).HasColumnName(@"Heading").ValueGeneratedNever().HasMaxLength(400);
        modelBuilder.Entity<DocSection>().Property(x => x.Level).HasColumnName(@"Level").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<DocSection>().Property(x => x.ContentMarkdown).HasColumnName(@"ContentMarkdown").ValueGeneratedNever();
        modelBuilder.Entity<DocSection>().Property(x => x.OrderIndex).HasColumnName(@"OrderIndex").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<DocSection>().Property(x => x.VersionNumber).HasColumnName(@"VersionNumber").IsRequired().ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<DocSection>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"CreatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<DocSection>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"UpdatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<DocSection>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"RemovedIngestionRunId").ValueGeneratedNever();
        modelBuilder.Entity<DocSection>().Property(x => x.ValidFromUtc).HasColumnName(@"ValidFromUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<DocSection>().Property(x => x.ValidToUtc).HasColumnName(@"ValidToUtc").ValueGeneratedNever();
        modelBuilder.Entity<DocSection>().Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        modelBuilder.Entity<DocSection>().Property(x => x.ContentHash).HasColumnName(@"ContentHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<DocSection>().Property(x => x.SemanticUidHash).HasColumnName(@"SemanticUidHash").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        modelBuilder.Entity<DocSection>().HasKey(@"Id");
        modelBuilder.Entity<DocSection>().HasIndex(@"SemanticUid",
            @"VersionNumber").IsUnique().HasDatabaseName(@"uq_doc_section_semantic_version");
    }







    partial void CustomizeDocSectionMapping(ModelBuilder modelBuilder);

    #endregion

    #region ExecutionResult Mapping

    private void ExecutionResultMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExecutionResult>().ToTable(@"ExecutionResults");
        modelBuilder.Entity<ExecutionResult>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<ExecutionResult>().Property(x => x.ExecutionRunId).HasColumnName(@"ExecutionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ExecutionResult>().Property(x => x.SampleUid).HasColumnName(@"SampleUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ExecutionResult>().Property(x => x.Status).HasColumnName(@"Status").ValueGeneratedNever().HasMaxLength(100);
        modelBuilder.Entity<ExecutionResult>().Property(x => x.BuildLog).HasColumnName(@"BuildLog").ValueGeneratedNever();
        modelBuilder.Entity<ExecutionResult>().Property(x => x.RunLog).HasColumnName(@"RunLog").ValueGeneratedNever();
        modelBuilder.Entity<ExecutionResult>().Property(x => x.ExceptionJson).HasColumnName(@"ExceptionJson").ValueGeneratedNever();
        modelBuilder.Entity<ExecutionResult>().Property(x => x.DurationMs).HasColumnName(@"DurationMs").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<ExecutionResult>().HasKey(@"Id");
    }







    partial void CustomizeExecutionResultMapping(ModelBuilder modelBuilder);

    #endregion

    #region ExecutionRun Mapping

    private void ExecutionRunMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExecutionRun>().ToTable(@"ExecutionRuns");
        modelBuilder.Entity<ExecutionRun>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<ExecutionRun>().Property(x => x.SnapshotId).HasColumnName(@"SnapshotId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ExecutionRun>().Property(x => x.SampleRunId).HasColumnName(@"SampleRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ExecutionRun>().Property(x => x.TimestampUtc).HasColumnName(@"TimestampUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ExecutionRun>().Property(x => x.EnvironmentJson).HasColumnName(@"EnvironmentJson").ValueGeneratedNever();
        modelBuilder.Entity<ExecutionRun>().Property(x => x.SchemaVersion).HasColumnName(@"SchemaVersion").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ExecutionRun>().HasKey(@"Id");
    }







    partial void CustomizeExecutionRunMapping(ModelBuilder modelBuilder);

    #endregion

    #region FeatureDocLink Mapping

    private void FeatureDocLinkMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeatureDocLink>().ToTable(@"FeatureDocLinks");
        modelBuilder.Entity<FeatureDocLink>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<FeatureDocLink>().Property(x => x.FeatureId).HasColumnName(@"FeatureId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<FeatureDocLink>().Property(x => x.DocUid).HasColumnName(@"DocUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<FeatureDocLink>().Property(x => x.SectionUid).HasColumnName(@"SectionUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<FeatureDocLink>().HasKey(@"Id");
    }







    partial void CustomizeFeatureDocLinkMapping(ModelBuilder modelBuilder);

    #endregion

    #region FeatureMemberLink Mapping

    private void FeatureMemberLinkMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeatureMemberLink>().ToTable(@"FeatureMemberLinks");
        modelBuilder.Entity<FeatureMemberLink>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<FeatureMemberLink>().Property(x => x.FeatureId).HasColumnName(@"FeatureId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<FeatureMemberLink>().Property(x => x.MemberUid).HasColumnName(@"MemberUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<FeatureMemberLink>().Property(x => x.Role).HasColumnName(@"Role").ValueGeneratedNever().HasMaxLength(50);
        modelBuilder.Entity<FeatureMemberLink>().HasKey(@"Id");
    }







    partial void CustomizeFeatureMemberLinkMapping(ModelBuilder modelBuilder);

    #endregion

    #region FeatureTypeLink Mapping

    private void FeatureTypeLinkMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeatureTypeLink>().ToTable(@"FeatureTypeLinks");
        modelBuilder.Entity<FeatureTypeLink>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<FeatureTypeLink>().Property(x => x.FeatureId).HasColumnName(@"FeatureId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<FeatureTypeLink>().Property(x => x.TypeUid).HasColumnName(@"TypeUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<FeatureTypeLink>().Property(x => x.Role).HasColumnName(@"Role").ValueGeneratedNever().HasMaxLength(50);
        modelBuilder.Entity<FeatureTypeLink>().HasKey(@"Id");
    }







    partial void CustomizeFeatureTypeLinkMapping(ModelBuilder modelBuilder);

    #endregion

    #region IngestionRun Mapping

    private void IngestionRunMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IngestionRun>().ToTable(@"IngestionRuns");
        modelBuilder.Entity<IngestionRun>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<IngestionRun>().Property(x => x.TimestampUtc).HasColumnName(@"TimestampUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<IngestionRun>().Property(x => x.SchemaVersion).HasColumnName(@"SchemaVersion").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<IngestionRun>().Property(x => x.Notes).HasColumnName(@"Notes").ValueGeneratedNever();
        modelBuilder.Entity<IngestionRun>().HasKey(@"Id");
    }







    partial void CustomizeIngestionRunMapping(ModelBuilder modelBuilder);

    #endregion

    #region RagChunk Mapping

    private void RagChunkMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RagChunk>().ToTable(@"RagChunks");
        modelBuilder.Entity<RagChunk>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<RagChunk>().Property(x => x.RagRunId).HasColumnName(@"RagRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<RagChunk>().Property(x => x.ChunkUid).HasColumnName(@"ChunkUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<RagChunk>().Property(x => x.Kind).HasColumnName(@"Kind").ValueGeneratedNever().HasMaxLength(100);
        modelBuilder.Entity<RagChunk>().Property(x => x.Text).HasColumnName(@"Text").ValueGeneratedNever();
        modelBuilder.Entity<RagChunk>().Property(x => x.MetadataJson).HasColumnName(@"MetadataJson").ValueGeneratedNever();
        modelBuilder.Entity<RagChunk>().Property(x => x.EmbeddingVector).HasColumnName(@"EmbeddingVector").ValueGeneratedNever().HasMaxLength(1536);
        modelBuilder.Entity<RagChunk>().HasKey(@"Id");
        modelBuilder.Entity<RagChunk>().HasIndex(@"ChunkUid").IsUnique();
    }







    partial void CustomizeRagChunkMapping(ModelBuilder modelBuilder);

    #endregion

    #region RagRun Mapping

    private void RagRunMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RagRun>().ToTable(@"RagRuns");
        modelBuilder.Entity<RagRun>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<RagRun>().Property(x => x.SnapshotId).HasColumnName(@"SnapshotId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<RagRun>().Property(x => x.TimestampUtc).HasColumnName(@"TimestampUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<RagRun>().Property(x => x.SchemaVersion).HasColumnName(@"SchemaVersion").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<RagRun>().HasKey(@"Id");
    }







    partial void CustomizeRagRunMapping(ModelBuilder modelBuilder);

    #endregion

    #region ReviewIssue Mapping

    private void ReviewIssueMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewIssue>().ToTable(@"ReviewIssues");
        modelBuilder.Entity<ReviewIssue>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<ReviewIssue>().Property(x => x.ReviewItemId).HasColumnName(@"ReviewItemId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ReviewIssue>().Property(x => x.Code).HasColumnName(@"Code").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ReviewIssue>().Property(x => x.Severity).HasColumnName(@"Severity").ValueGeneratedNever().HasMaxLength(50);
        modelBuilder.Entity<ReviewIssue>().Property(x => x.RelatedMemberUid).HasColumnName(@"RelatedMemberUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ReviewIssue>().Property(x => x.Details).HasColumnName(@"Details").ValueGeneratedNever();
        modelBuilder.Entity<ReviewIssue>().HasKey(@"Id");
    }







    partial void CustomizeReviewIssueMapping(ModelBuilder modelBuilder);

    #endregion

    #region ReviewItem Mapping

    private void ReviewItemMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewItem>().ToTable(@"ReviewItems");
        modelBuilder.Entity<ReviewItem>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<ReviewItem>().Property(x => x.ReviewRunId).HasColumnName(@"ReviewRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ReviewItem>().Property(x => x.TargetKind).HasColumnName(@"TargetKind").IsRequired().ValueGeneratedNever().HasMaxLength(50);
        modelBuilder.Entity<ReviewItem>().Property(x => x.TargetUid).HasColumnName(@"TargetUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<ReviewItem>().Property(x => x.Status).HasColumnName(@"Status").ValueGeneratedNever().HasMaxLength(50);
        modelBuilder.Entity<ReviewItem>().Property(x => x.Summary).HasColumnName(@"Summary").ValueGeneratedNever();
        modelBuilder.Entity<ReviewItem>().HasKey(@"Id");
    }







    partial void CustomizeReviewItemMapping(ModelBuilder modelBuilder);

    #endregion

    #region ReviewRun Mapping

    private void ReviewRunMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewRun>().ToTable(@"ReviewRuns");
        modelBuilder.Entity<ReviewRun>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<ReviewRun>().Property(x => x.SnapshotId).HasColumnName(@"SnapshotId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ReviewRun>().Property(x => x.TimestampUtc).HasColumnName(@"TimestampUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<ReviewRun>().Property(x => x.SchemaVersion).HasColumnName(@"SchemaVersion").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<ReviewRun>().HasKey(@"Id");
    }







    partial void CustomizeReviewRunMapping(ModelBuilder modelBuilder);

    #endregion

    #region Sample Mapping

    private void SampleMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sample>().ToTable(@"Samples");
        modelBuilder.Entity<Sample>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<Sample>().Property(x => x.SampleRunId).HasColumnName(@"SampleRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<Sample>().Property(x => x.SampleUid).HasColumnName(@"SampleUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<Sample>().Property(x => x.FeatureUid).HasColumnName(@"FeatureUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<Sample>().Property(x => x.Language).HasColumnName(@"Language").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<Sample>().Property(x => x.Code).HasColumnName(@"Code").ValueGeneratedNever();
        modelBuilder.Entity<Sample>().Property(x => x.EntryPoint).HasColumnName(@"EntryPoint").ValueGeneratedNever().HasMaxLength(400);
        modelBuilder.Entity<Sample>().Property(x => x.TargetFramework).HasColumnName(@"TargetFramework").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<Sample>().Property(x => x.PackageReferences).HasColumnName(@"PackageReferences").ValueGeneratedNever();
        modelBuilder.Entity<Sample>().Property(x => x.DerivedFromCodeUid).HasColumnName(@"DerivedFromCodeUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<Sample>().Property(x => x.Tags).HasColumnName(@"Tags").ValueGeneratedNever();
        modelBuilder.Entity<Sample>().HasKey(@"Id");
        modelBuilder.Entity<Sample>().HasIndex(@"SampleUid").IsUnique();
    }







    partial void CustomizeSampleMapping(ModelBuilder modelBuilder);

    #endregion

    #region SampleApiMemberLink Mapping

    private void SampleApiMemberLinkMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SampleApiMemberLink>().ToTable(@"SampleApiMemberLinks");
        modelBuilder.Entity<SampleApiMemberLink>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<SampleApiMemberLink>().Property(x => x.SampleId).HasColumnName(@"SampleId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<SampleApiMemberLink>().Property(x => x.MemberUid).HasColumnName(@"MemberUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<SampleApiMemberLink>().HasKey(@"Id");
    }







    partial void CustomizeSampleApiMemberLinkMapping(ModelBuilder modelBuilder);

    #endregion

    #region SampleRun Mapping

    private void SampleRunMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SampleRun>().ToTable(@"SampleRuns");
        modelBuilder.Entity<SampleRun>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<SampleRun>().Property(x => x.SnapshotId).HasColumnName(@"SnapshotId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<SampleRun>().Property(x => x.TimestampUtc).HasColumnName(@"TimestampUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<SampleRun>().Property(x => x.SchemaVersion).HasColumnName(@"SchemaVersion").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<SampleRun>().HasKey(@"Id");
    }







    partial void CustomizeSampleRunMapping(ModelBuilder modelBuilder);

    #endregion

    #region SemanticIdentity Mapping

    private void SemanticIdentityMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SemanticIdentity>().ToTable(@"SemanticIdentities");
        modelBuilder.Entity<SemanticIdentity>().Property(x => x.UidHash).HasColumnName(@"UidHash").IsRequired().ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        modelBuilder.Entity<SemanticIdentity>().Property(x => x.Uid).HasColumnName(@"Uid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<SemanticIdentity>().Property(x => x.Kind).HasColumnName(@"Kind").IsRequired().ValueGeneratedNever().HasMaxLength(50);
        modelBuilder.Entity<SemanticIdentity>().Property(x => x.CreatedUtc).HasColumnName(@"CreatedUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<SemanticIdentity>().Property(x => x.Notes).HasColumnName(@"Notes").ValueGeneratedNever();
        modelBuilder.Entity<SemanticIdentity>().HasKey(@"UidHash");
    }







    partial void CustomizeSemanticIdentityMapping(ModelBuilder modelBuilder);

    #endregion

    #region SnapshotDiff Mapping

    private void SnapshotDiffMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SnapshotDiff>().ToTable(@"SnapshotDiffs");
        modelBuilder.Entity<SnapshotDiff>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<SnapshotDiff>().Property(x => x.OldSnapshotId).HasColumnName(@"OldSnapshotId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<SnapshotDiff>().Property(x => x.NewSnapshotId).HasColumnName(@"NewSnapshotId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<SnapshotDiff>().Property(x => x.TimestampUtc).HasColumnName(@"TimestampUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<SnapshotDiff>().Property(x => x.SchemaVersion).HasColumnName(@"SchemaVersion").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<SnapshotDiff>().HasKey(@"Id");
    }







    partial void CustomizeSnapshotDiffMapping(ModelBuilder modelBuilder);

    #endregion

    #region SourceSnapshot Mapping

    private void SourceSnapshotMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SourceSnapshot>().ToTable(@"SourceSnapshots");
        modelBuilder.Entity<SourceSnapshot>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<SourceSnapshot>().Property(x => x.IngestionRunId).HasColumnName(@"IngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<SourceSnapshot>().Property(x => x.SnapshotUid).HasColumnName(@"SnapshotUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<SourceSnapshot>().Property(x => x.RepoUrl).HasColumnName(@"RepoUrl").ValueGeneratedNever();
        modelBuilder.Entity<SourceSnapshot>().Property(x => x.Branch).HasColumnName(@"Branch").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<SourceSnapshot>().Property(x => x.RepoCommit).HasColumnName(@"RepoCommit").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<SourceSnapshot>().Property(x => x.Language).HasColumnName(@"Language").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<SourceSnapshot>().Property(x => x.PackageName).HasColumnName(@"PackageName").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<SourceSnapshot>().Property(x => x.PackageVersion).HasColumnName(@"PackageVersion").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<SourceSnapshot>().Property(x => x.ConfigJson).HasColumnName(@"ConfigJson").ValueGeneratedNever();
        modelBuilder.Entity<SourceSnapshot>().Property(x => x.SnapshotUidHash).HasColumnName(@"SnapshotUidHash").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        modelBuilder.Entity<SourceSnapshot>().HasKey(@"Id");
        modelBuilder.Entity<SourceSnapshot>().HasIndex(@"SnapshotUidHash").IsUnique();
    }







    partial void CustomizeSourceSnapshotMapping(ModelBuilder modelBuilder);

    #endregion

    #region TruthRun Mapping

    private void TruthRunMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TruthRun>().ToTable(@"TruthRuns");
        modelBuilder.Entity<TruthRun>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        modelBuilder.Entity<TruthRun>().Property(x => x.SnapshotId).HasColumnName(@"SnapshotId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<TruthRun>().Property(x => x.TimestampUtc).HasColumnName(@"TimestampUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<TruthRun>().Property(x => x.SchemaVersion).HasColumnName(@"SchemaVersion").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<TruthRun>().HasKey(@"Id");
    }







    partial void CustomizeTruthRunMapping(ModelBuilder modelBuilder);

    #endregion

    #region VApiFeatureCurrent Mapping

    private void VApiFeatureCurrentMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VApiFeatureCurrent>().HasNoKey();
        modelBuilder.Entity<VApiFeatureCurrent>().ToView(@"VApiFeatureCurrents");
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.SemanticUid).HasColumnName(@"SemanticUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.TruthRunId).HasColumnName(@"TruthRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.Name).HasColumnName(@"Name").ValueGeneratedNever().HasMaxLength(400);
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.Language).HasColumnName(@"Language").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.Description).HasColumnName(@"Description").ValueGeneratedNever();
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.Tags).HasColumnName(@"Tags").ValueGeneratedNever();
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.VersionNumber).HasColumnName(@"VersionNumber").IsRequired().ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"CreatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"UpdatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"RemovedIngestionRunId").ValueGeneratedNever();
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.ValidFromUtc).HasColumnName(@"ValidFromUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.ValidToUtc).HasColumnName(@"ValidToUtc").ValueGeneratedNever();
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.ContentHash).HasColumnName(@"ContentHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.SemanticUidHash).HasColumnName(@"SemanticUidHash").ValueGeneratedNever().HasMaxLength(32);
    }







    partial void CustomizeVApiFeatureCurrentMapping(ModelBuilder modelBuilder);

    #endregion

    #region VApiMemberCurrent Mapping

    private void VApiMemberCurrentMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VApiMemberCurrent>().HasNoKey();
        modelBuilder.Entity<VApiMemberCurrent>().ToView(@"VApiMemberCurrents");
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.SemanticUid).HasColumnName(@"SemanticUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ApiTypeId).HasColumnName(@"ApiTypeId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Name).HasColumnName(@"Name").ValueGeneratedNever().HasMaxLength(400);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Kind).HasColumnName(@"Kind").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.MethodKind).HasColumnName(@"MethodKind").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Accessibility).HasColumnName(@"Accessibility").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsStatic).HasColumnName(@"IsStatic").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsExtensionMethod).HasColumnName(@"IsExtensionMethod").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsAsync).HasColumnName(@"IsAsync").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsVirtual).HasColumnName(@"IsVirtual").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsOverride).HasColumnName(@"IsOverride").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsAbstract).HasColumnName(@"IsAbstract").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsSealed).HasColumnName(@"IsSealed").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsReadonly).HasColumnName(@"IsReadonly").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsConst).HasColumnName(@"IsConst").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsUnsafe).HasColumnName(@"IsUnsafe").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ReturnTypeUid).HasColumnName(@"ReturnTypeUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ReturnNullable).HasColumnName(@"ReturnNullable").ValueGeneratedNever().HasMaxLength(50);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.GenericParameters).HasColumnName(@"GenericParameters").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.GenericConstraints).HasColumnName(@"GenericConstraints").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Summary).HasColumnName(@"Summary").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Remarks).HasColumnName(@"Remarks").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Attributes).HasColumnName(@"Attributes").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.SourceFilePath).HasColumnName(@"SourceFilePath").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.SourceStartLine).HasColumnName(@"SourceStartLine").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.SourceEndLine).HasColumnName(@"SourceEndLine").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.MemberUidHash).HasColumnName(@"MemberUidHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.VersionNumber).HasColumnName(@"VersionNumber").IsRequired().ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"CreatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"UpdatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"RemovedIngestionRunId").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ValidFromUtc).HasColumnName(@"ValidFromUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ValidToUtc).HasColumnName(@"ValidToUtc").ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ContentHash).HasColumnName(@"ContentHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.SemanticUidHash).HasColumnName(@"SemanticUidHash").ValueGeneratedNever().HasMaxLength(32);
    }







    partial void CustomizeVApiMemberCurrentMapping(ModelBuilder modelBuilder);

    #endregion

    #region VApiTypeCurrent Mapping

    private void VApiTypeCurrentMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VApiTypeCurrent>().HasNoKey();
        modelBuilder.Entity<VApiTypeCurrent>().ToView(@"VApiTypeCurrents");
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SemanticUid).HasColumnName(@"SemanticUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SourceSnapshotId).HasColumnName(@"SourceSnapshotId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Name).HasColumnName(@"Name").ValueGeneratedNever().HasMaxLength(400);
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.NamespacePath).HasColumnName(@"NamespacePath").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Kind).HasColumnName(@"Kind").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Accessibility).HasColumnName(@"Accessibility").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsStatic).HasColumnName(@"IsStatic").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsGeneric).HasColumnName(@"IsGeneric").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsAbstract).HasColumnName(@"IsAbstract").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsSealed).HasColumnName(@"IsSealed").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsRecord).HasColumnName(@"IsRecord").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsRefLike).HasColumnName(@"IsRefLike").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.BaseTypeUid).HasColumnName(@"BaseTypeUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Interfaces).HasColumnName(@"Interfaces").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.ContainingTypeUid).HasColumnName(@"ContainingTypeUid").ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.GenericParameters).HasColumnName(@"GenericParameters").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.GenericConstraints).HasColumnName(@"GenericConstraints").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Summary).HasColumnName(@"Summary").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Remarks).HasColumnName(@"Remarks").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Attributes).HasColumnName(@"Attributes").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SourceFilePath).HasColumnName(@"SourceFilePath").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SourceStartLine).HasColumnName(@"SourceStartLine").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SourceEndLine).HasColumnName(@"SourceEndLine").ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.VersionNumber).HasColumnName(@"VersionNumber").IsRequired().ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"CreatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"UpdatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"RemovedIngestionRunId").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.ValidFromUtc).HasColumnName(@"ValidFromUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.ValidToUtc).HasColumnName(@"ValidToUtc").ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.ContentHash).HasColumnName(@"ContentHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SemanticUidHash).HasColumnName(@"SemanticUidHash").ValueGeneratedNever().HasMaxLength(32);
    }







    partial void CustomizeVApiTypeCurrentMapping(ModelBuilder modelBuilder);

    #endregion

    #region VDocPageCurrent Mapping

    private void VDocPageCurrentMapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VDocPageCurrent>().HasNoKey();
        modelBuilder.Entity<VDocPageCurrent>().ToView(@"VDocPageCurrents");
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.SemanticUid).HasColumnName(@"SemanticUid").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.SourceSnapshotId).HasColumnName(@"SourceSnapshotId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.SourcePath).HasColumnName(@"SourcePath").ValueGeneratedNever();
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.Title).HasColumnName(@"Title").ValueGeneratedNever().HasMaxLength(400);
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.Language).HasColumnName(@"Language").ValueGeneratedNever().HasMaxLength(200);
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.Url).HasColumnName(@"Url").ValueGeneratedNever();
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.RawMarkdown).HasColumnName(@"RawMarkdown").ValueGeneratedNever();
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.VersionNumber).HasColumnName(@"VersionNumber").IsRequired().ValueGeneratedNever().HasPrecision(10,
            0);
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"CreatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"UpdatedIngestionRunId").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"RemovedIngestionRunId").ValueGeneratedNever();
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.ValidFromUtc).HasColumnName(@"ValidFromUtc").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.ValidToUtc).HasColumnName(@"ValidToUtc").ValueGeneratedNever();
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().ValueGeneratedNever();
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.ContentHash).HasColumnName(@"ContentHash").ValueGeneratedNever().HasMaxLength(32);
        modelBuilder.Entity<VDocPageCurrent>().Property(x => x.SemanticUidHash).HasColumnName(@"SemanticUidHash").ValueGeneratedNever().HasMaxLength(32);
    }







    partial void CustomizeVDocPageCurrentMapping(ModelBuilder modelBuilder);

    #endregion







   public async Task<Guid> CreateSourceSnapshotAsync(SourceSnapshot snapshot)
{
    if (snapshot == null)
    {
        throw new ArgumentNullException(nameof(snapshot), "Snapshot cannot be null.");
    }
    try
    {
        Guid? SessionidEmpty = Guid.Empty;
        var result = await CreateSourceSnapshotAsync(
            snapshot.IngestionRunId,
            snapshot.SnapshotUid,
            snapshot.RepoUrl,
            snapshot.Branch,
            snapshot.RepoCommit,
            snapshot.Language,
            snapshot.PackageName,
            snapshot.PackageVersion,
            snapshot.ConfigJson,
            SnapshotId: SessionidEmpty
        );
        return result?.Item1 ?? throw new InvalidOperationException("Snapshot creation failed.");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error creating source snapshot: {ex.Message}");
        throw;
    }
}

}