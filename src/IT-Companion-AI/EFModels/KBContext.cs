using System.Data;
using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;



namespace ITCompanionAI.EFModels;


public partial class KBContext : DbContext
{

    public KBContext()
    {
        OnCreated();
    }








    public KBContext(DbContextOptions<KBContext> options)
        : base(options)
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
             !optionsBuilder.Options.Extensions.Any(ext => ext is not RelationalOptionsExtension and not CoreOptionsExtension)))
        {
            _ = optionsBuilder.UseSqlServer(@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=KnowledgeCurator;Integrated Security=True;Persist Security Info=True");
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
        _ = modelBuilder.Entity<ApiFeature>().HasOne(x => x.IngestionRun).WithMany(op => op.ApiFeatures).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        _ = modelBuilder.Entity<ApiFeature>().HasOne(x => x.ApiMember).WithOne(op => op.ApiFeature).HasForeignKey(typeof(ApiMember), @"ApiFeatureId").IsRequired(false);

        _ = modelBuilder.Entity<ApiMember>().HasOne(x => x.IngestionRun).WithMany(op => op.ApiMembers).HasForeignKey(@"CreatedIngestionRunId").IsRequired();

        _ = modelBuilder.Entity<ApiParameter>().HasOne(x => x.IngestionRun).WithMany(op => op.ApiParameters).HasForeignKey(@"CreatedIngestionRunId").IsRequired();

        _ = modelBuilder.Entity<ApiType>().HasOne(x => x.IngestionRun).WithMany(op => op.ApiTypes).HasForeignKey(@"CreatedIngestionRunId").IsRequired();

        _ = modelBuilder.Entity<DocPage>().HasOne(x => x.IngestionRun).WithMany(op => op.DocPages).HasForeignKey(@"CreatedIngestionRunId").IsRequired();

        _ = modelBuilder.Entity<DocSection>().HasOne(x => x.IngestionRun).WithMany(op => op.DocSections).HasForeignKey(@"CreatedIngestionRunId").IsRequired();

        _ = modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiFeatures).WithOne(op => op.IngestionRun).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        _ = modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiMembers).WithOne(op => op.IngestionRun).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        _ = modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiParameters).WithOne(op => op.IngestionRun).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        _ = modelBuilder.Entity<IngestionRun>().HasMany(x => x.ApiTypes).WithOne(op => op.IngestionRun).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        _ = modelBuilder.Entity<IngestionRun>().HasMany(x => x.DocPages).WithOne(op => op.IngestionRun).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
        _ = modelBuilder.Entity<IngestionRun>().HasMany(x => x.DocSections).WithOne(op => op.IngestionRun).HasForeignKey(@"CreatedIngestionRunId").IsRequired();
    }








    partial void CustomizeMapping(ref ModelBuilder modelBuilder);








    public bool HasChanges()
    {
        return ChangeTracker.Entries().Any(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
    }








    partial void OnCreated();



    #region Methods

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
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);
                _ = cmd.ExecuteNonQuery();
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
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);
                _ = await cmd.ExecuteNonQueryAsync();
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








    public void SpBeginIngestionRun(string SchemaVersion, string Notes, ref Guid? IngestionRunId)
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
                cmd.CommandText = @"dbo.sp_BeginIngestionRun";

                DbParameter SchemaVersionParameter = cmd.CreateParameter();
                SchemaVersionParameter.ParameterName = "SchemaVersion";
                SchemaVersionParameter.Direction = ParameterDirection.Input;
                SchemaVersionParameter.DbType = DbType.String;
                SchemaVersionParameter.Size = 200;
                SchemaVersionParameter.Value = SchemaVersion != null ? SchemaVersion : DBNull.Value;

                _ = cmd.Parameters.Add(SchemaVersionParameter);

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

                _ = cmd.Parameters.Add(NotesParameter);

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);
                _ = cmd.ExecuteNonQuery();

                IngestionRunId = cmd.Parameters["IngestionRunId"].Value is not null and not DBNull
                    ? (Guid)Convert.ChangeType(cmd.Parameters["IngestionRunId"].Value, typeof(Guid))
                    : default;
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








    public async Task<Tuple<Guid?>> SpBeginIngestionRunAsync(string SchemaVersion, string Notes, Guid? IngestionRunId)
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
                cmd.CommandText = @"dbo.sp_BeginIngestionRun";

                DbParameter SchemaVersionParameter = cmd.CreateParameter();
                SchemaVersionParameter.ParameterName = "SchemaVersion";
                SchemaVersionParameter.Direction = ParameterDirection.Input;
                SchemaVersionParameter.DbType = DbType.String;
                SchemaVersionParameter.Size = 200;
                SchemaVersionParameter.Value = SchemaVersion != null ? SchemaVersion : DBNull.Value;

                _ = cmd.Parameters.Add(SchemaVersionParameter);

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

                _ = cmd.Parameters.Add(NotesParameter);

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);
                _ = await cmd.ExecuteNonQueryAsync();

                IngestionRunId = cmd.Parameters["IngestionRunId"].Value is not null and not DBNull
                    ? (Guid)Convert.ChangeType(cmd.Parameters["IngestionRunId"].Value, typeof(Guid))
                    : default;
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








    public SpCheckTemporalConsistencyMultipleResult SpCheckTemporalConsistency()
    {
        SpCheckTemporalConsistencyMultipleResult result = new();
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
                cmd.CommandText = @"dbo.sp_CheckTemporalConsistency";
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    string[] fieldNames;
                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult1 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            resultRow.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResult1s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult1 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            resultRow.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResult1s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult1 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            resultRow.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResult1s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult1 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            resultRow.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResult1s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult2 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains(@"version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            resultRow.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains(@"valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            resultRow.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        resultRow.ValidToUtc = fieldNames.Contains(@"valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        resultRow.NextFrom = fieldNames.Contains(@"next_from") && !reader.IsDBNull(reader.GetOrdinal(@"next_from"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"next_from")), typeof(DateTime))
                            : null;

                        result.SpCheckTemporalConsistencyResult2s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult2 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains(@"version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            resultRow.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains(@"valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            resultRow.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        resultRow.ValidToUtc = fieldNames.Contains(@"valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        resultRow.NextFrom = fieldNames.Contains(@"next_from") && !reader.IsDBNull(reader.GetOrdinal(@"next_from"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"next_from")), typeof(DateTime))
                            : null;

                        result.SpCheckTemporalConsistencyResult2s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult2 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains(@"version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            resultRow.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains(@"valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            resultRow.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        resultRow.ValidToUtc = fieldNames.Contains(@"valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        resultRow.NextFrom = fieldNames.Contains(@"next_from") && !reader.IsDBNull(reader.GetOrdinal(@"next_from"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"next_from")), typeof(DateTime))
                            : null;

                        result.SpCheckTemporalConsistencyResult2s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult2 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains(@"version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            resultRow.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains(@"valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            resultRow.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        resultRow.ValidToUtc = fieldNames.Contains(@"valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        resultRow.NextFrom = fieldNames.Contains(@"next_from") && !reader.IsDBNull(reader.GetOrdinal(@"next_from"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"next_from")), typeof(DateTime))
                            : null;

                        result.SpCheckTemporalConsistencyResult2s.Add(resultRow);
                    }

                    _ = reader.NextResult();
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








    public async Task<SpCheckTemporalConsistencyMultipleResult> SpCheckTemporalConsistencyAsync()
    {
        SpCheckTemporalConsistencyMultipleResult result = new();
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
                cmd.CommandText = @"dbo.sp_CheckTemporalConsistency";
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    string[] fieldNames;
                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult1 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            resultRow.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResult1s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult1 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            resultRow.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResult1s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult1 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            resultRow.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResult1s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult1 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            resultRow.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        result.SpCheckTemporalConsistencyResult1s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult2 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains(@"version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            resultRow.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains(@"valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            resultRow.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        resultRow.ValidToUtc = fieldNames.Contains(@"valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        resultRow.NextFrom = fieldNames.Contains(@"next_from") && !reader.IsDBNull(reader.GetOrdinal(@"next_from"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"next_from")), typeof(DateTime))
                            : null;

                        result.SpCheckTemporalConsistencyResult2s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult2 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains(@"version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            resultRow.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains(@"valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            resultRow.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        resultRow.ValidToUtc = fieldNames.Contains(@"valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        resultRow.NextFrom = fieldNames.Contains(@"next_from") && !reader.IsDBNull(reader.GetOrdinal(@"next_from"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"next_from")), typeof(DateTime))
                            : null;

                        result.SpCheckTemporalConsistencyResult2s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult2 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains(@"version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            resultRow.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains(@"valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            resultRow.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        resultRow.ValidToUtc = fieldNames.Contains(@"valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        resultRow.NextFrom = fieldNames.Contains(@"next_from") && !reader.IsDBNull(reader.GetOrdinal(@"next_from"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"next_from")), typeof(DateTime))
                            : null;

                        result.SpCheckTemporalConsistencyResult2s.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpCheckTemporalConsistencyResult2 resultRow = new();
                        if (fieldNames.Contains(@"table_name") && !reader.IsDBNull(reader.GetOrdinal(@"table_name")))
                        {
                            resultRow.TableName = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"table_name")), typeof(string));
                        }

                        if (fieldNames.Contains(@"semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            resultRow.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains(@"version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            resultRow.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains(@"valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            resultRow.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        resultRow.ValidToUtc = fieldNames.Contains(@"valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        resultRow.NextFrom = fieldNames.Contains(@"next_from") && !reader.IsDBNull(reader.GetOrdinal(@"next_from"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"next_from")), typeof(DateTime))
                            : null;

                        result.SpCheckTemporalConsistencyResult2s.Add(resultRow);
                    }

                    _ = reader.NextResult();
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








    public void SpCreateSourceSnapshot(Guid? IngestionRunId, string SnapshotUid, string RepoUrl, string Branch, string RepoCommit, string Language, string PackageName, string PackageVersion, string ConfigJson, ref Guid? SnapshotId)
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
                cmd.CommandText = @"dbo.sp_CreateSourceSnapshot";

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter SnapshotUidParameter = cmd.CreateParameter();
                SnapshotUidParameter.ParameterName = "SnapshotUid";
                SnapshotUidParameter.Direction = ParameterDirection.Input;
                SnapshotUidParameter.DbType = DbType.String;
                SnapshotUidParameter.Size = 1000;
                SnapshotUidParameter.Value = SnapshotUid != null ? SnapshotUid : DBNull.Value;

                _ = cmd.Parameters.Add(SnapshotUidParameter);

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

                _ = cmd.Parameters.Add(RepoUrlParameter);

                DbParameter BranchParameter = cmd.CreateParameter();
                BranchParameter.ParameterName = "Branch";
                BranchParameter.Direction = ParameterDirection.Input;
                BranchParameter.DbType = DbType.String;
                BranchParameter.Size = 200;
                BranchParameter.Value = Branch != null ? Branch : DBNull.Value;

                _ = cmd.Parameters.Add(BranchParameter);

                DbParameter RepoCommitParameter = cmd.CreateParameter();
                RepoCommitParameter.ParameterName = "RepoCommit";
                RepoCommitParameter.Direction = ParameterDirection.Input;
                RepoCommitParameter.DbType = DbType.String;
                RepoCommitParameter.Size = 200;
                RepoCommitParameter.Value = RepoCommit != null ? RepoCommit : DBNull.Value;

                _ = cmd.Parameters.Add(RepoCommitParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                LanguageParameter.Value = Language != null ? Language : DBNull.Value;

                _ = cmd.Parameters.Add(LanguageParameter);

                DbParameter PackageNameParameter = cmd.CreateParameter();
                PackageNameParameter.ParameterName = "PackageName";
                PackageNameParameter.Direction = ParameterDirection.Input;
                PackageNameParameter.DbType = DbType.String;
                PackageNameParameter.Size = 200;
                PackageNameParameter.Value = PackageName != null ? PackageName : DBNull.Value;

                _ = cmd.Parameters.Add(PackageNameParameter);

                DbParameter PackageVersionParameter = cmd.CreateParameter();
                PackageVersionParameter.ParameterName = "PackageVersion";
                PackageVersionParameter.Direction = ParameterDirection.Input;
                PackageVersionParameter.DbType = DbType.String;
                PackageVersionParameter.Size = 200;
                PackageVersionParameter.Value = PackageVersion != null ? PackageVersion : DBNull.Value;

                _ = cmd.Parameters.Add(PackageVersionParameter);

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

                _ = cmd.Parameters.Add(ConfigJsonParameter);

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

                _ = cmd.Parameters.Add(SnapshotIdParameter);
                _ = cmd.ExecuteNonQuery();

                SnapshotId = cmd.Parameters["SnapshotId"].Value is not null and not DBNull
                    ? (Guid)Convert.ChangeType(cmd.Parameters["SnapshotId"].Value, typeof(Guid))
                    : default;
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








    public async Task<Tuple<Guid?>> SpCreateSourceSnapshotAsync(Guid? IngestionRunId, string SnapshotUid, string RepoUrl, string Branch, string RepoCommit, string Language, string PackageName, string PackageVersion, string ConfigJson, Guid? SnapshotId)
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
                cmd.CommandText = @"dbo.sp_CreateSourceSnapshot";

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter SnapshotUidParameter = cmd.CreateParameter();
                SnapshotUidParameter.ParameterName = "SnapshotUid";
                SnapshotUidParameter.Direction = ParameterDirection.Input;
                SnapshotUidParameter.DbType = DbType.String;
                SnapshotUidParameter.Size = 1000;
                SnapshotUidParameter.Value = SnapshotUid != null ? SnapshotUid : DBNull.Value;

                _ = cmd.Parameters.Add(SnapshotUidParameter);

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

                _ = cmd.Parameters.Add(RepoUrlParameter);

                DbParameter BranchParameter = cmd.CreateParameter();
                BranchParameter.ParameterName = "Branch";
                BranchParameter.Direction = ParameterDirection.Input;
                BranchParameter.DbType = DbType.String;
                BranchParameter.Size = 200;
                BranchParameter.Value = Branch != null ? Branch : DBNull.Value;

                _ = cmd.Parameters.Add(BranchParameter);

                DbParameter RepoCommitParameter = cmd.CreateParameter();
                RepoCommitParameter.ParameterName = "RepoCommit";
                RepoCommitParameter.Direction = ParameterDirection.Input;
                RepoCommitParameter.DbType = DbType.String;
                RepoCommitParameter.Size = 200;
                RepoCommitParameter.Value = RepoCommit != null ? RepoCommit : DBNull.Value;

                _ = cmd.Parameters.Add(RepoCommitParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                LanguageParameter.Value = Language != null ? Language : DBNull.Value;

                _ = cmd.Parameters.Add(LanguageParameter);

                DbParameter PackageNameParameter = cmd.CreateParameter();
                PackageNameParameter.ParameterName = "PackageName";
                PackageNameParameter.Direction = ParameterDirection.Input;
                PackageNameParameter.DbType = DbType.String;
                PackageNameParameter.Size = 200;
                PackageNameParameter.Value = PackageName != null ? PackageName : DBNull.Value;

                _ = cmd.Parameters.Add(PackageNameParameter);

                DbParameter PackageVersionParameter = cmd.CreateParameter();
                PackageVersionParameter.ParameterName = "PackageVersion";
                PackageVersionParameter.Direction = ParameterDirection.Input;
                PackageVersionParameter.DbType = DbType.String;
                PackageVersionParameter.Size = 200;
                PackageVersionParameter.Value = PackageVersion != null ? PackageVersion : DBNull.Value;

                _ = cmd.Parameters.Add(PackageVersionParameter);

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

                _ = cmd.Parameters.Add(ConfigJsonParameter);

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

                _ = cmd.Parameters.Add(SnapshotIdParameter);
                _ = await cmd.ExecuteNonQueryAsync();

                SnapshotId = cmd.Parameters["SnapshotId"].Value is not null and not DBNull
                    ? (Guid)Convert.ChangeType(cmd.Parameters["SnapshotId"].Value, typeof(Guid))
                    : default;
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








    public void SpCreateTruthRun(Guid? SnapshotId, string SchemaVersion, ref Guid? TruthRunId)
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
                cmd.CommandText = @"dbo.sp_CreateTruthRun";

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

                _ = cmd.Parameters.Add(SnapshotIdParameter);

                DbParameter SchemaVersionParameter = cmd.CreateParameter();
                SchemaVersionParameter.ParameterName = "SchemaVersion";
                SchemaVersionParameter.Direction = ParameterDirection.Input;
                SchemaVersionParameter.DbType = DbType.String;
                SchemaVersionParameter.Size = 200;
                SchemaVersionParameter.Value = SchemaVersion != null ? SchemaVersion : DBNull.Value;

                _ = cmd.Parameters.Add(SchemaVersionParameter);

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

                _ = cmd.Parameters.Add(TruthRunIdParameter);
                _ = cmd.ExecuteNonQuery();

                TruthRunId = cmd.Parameters["TruthRunId"].Value is not null and not DBNull
                    ? (Guid)Convert.ChangeType(cmd.Parameters["TruthRunId"].Value, typeof(Guid))
                    : default;
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








    public async Task<Tuple<Guid?>> SpCreateTruthRunAsync(Guid? SnapshotId, string SchemaVersion, Guid? TruthRunId)
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
                cmd.CommandText = @"dbo.sp_CreateTruthRun";

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

                _ = cmd.Parameters.Add(SnapshotIdParameter);

                DbParameter SchemaVersionParameter = cmd.CreateParameter();
                SchemaVersionParameter.ParameterName = "SchemaVersion";
                SchemaVersionParameter.Direction = ParameterDirection.Input;
                SchemaVersionParameter.DbType = DbType.String;
                SchemaVersionParameter.Size = 200;
                SchemaVersionParameter.Value = SchemaVersion != null ? SchemaVersion : DBNull.Value;

                _ = cmd.Parameters.Add(SchemaVersionParameter);

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

                _ = cmd.Parameters.Add(TruthRunIdParameter);
                _ = await cmd.ExecuteNonQueryAsync();

                TruthRunId = cmd.Parameters["TruthRunId"].Value is not null and not DBNull
                    ? (Guid)Convert.ChangeType(cmd.Parameters["TruthRunId"].Value, typeof(Guid))
                    : default;
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








    public void SpDeleteSemanticIdentity(string Uid)
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
                cmd.CommandText = @"dbo.sp_DeleteSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                UidParameter.Value = Uid != null ? Uid : DBNull.Value;

                _ = cmd.Parameters.Add(UidParameter);
                _ = cmd.ExecuteNonQuery();
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








    public async Task SpDeleteSemanticIdentityAsync(string Uid)
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
                cmd.CommandText = @"dbo.sp_DeleteSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                UidParameter.Value = Uid != null ? Uid : DBNull.Value;

                _ = cmd.Parameters.Add(UidParameter);
                _ = await cmd.ExecuteNonQueryAsync();
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








    public void SpEndIngestionRun(Guid? IngestionRunId)
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
                cmd.CommandText = @"dbo.sp_EndIngestionRun";

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);
                _ = cmd.ExecuteNonQuery();
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








    public async Task SpEndIngestionRunAsync(Guid? IngestionRunId)
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
                cmd.CommandText = @"dbo.sp_EndIngestionRun";

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);
                _ = await cmd.ExecuteNonQueryAsync();
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








    public void SpEnsureSemanticIdentity(string Uid, string Kind, string Notes)
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
                cmd.CommandText = @"dbo.sp_EnsureSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                UidParameter.Value = Uid != null ? Uid : DBNull.Value;

                _ = cmd.Parameters.Add(UidParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 50;
                KindParameter.Value = Kind != null ? Kind : DBNull.Value;

                _ = cmd.Parameters.Add(KindParameter);

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

                _ = cmd.Parameters.Add(NotesParameter);
                _ = cmd.ExecuteNonQuery();
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








    public async Task SpEnsureSemanticIdentityAsync(string Uid, string Kind, string Notes)
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
                cmd.CommandText = @"dbo.sp_EnsureSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                UidParameter.Value = Uid != null ? Uid : DBNull.Value;

                _ = cmd.Parameters.Add(UidParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 50;
                KindParameter.Value = Kind != null ? Kind : DBNull.Value;

                _ = cmd.Parameters.Add(KindParameter);

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

                _ = cmd.Parameters.Add(NotesParameter);
                _ = await cmd.ExecuteNonQueryAsync();
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








    public List<SpGetChangesInIngestionRunResult> SpGetChangesInIngestionRun(Guid? IngestionRunId)
    {
        var result = new List<SpGetChangesInIngestionRunResult>();
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
                cmd.CommandText = @"dbo.sp_GetChangesInIngestionRun";

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        SpGetChangesInIngestionRunResult row = new();
                        if (fieldNames.Contains("artifact_kind") && !reader.IsDBNull(reader.GetOrdinal(@"artifact_kind")))
                        {
                            row.ArtifactKind = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"artifact_kind")), typeof(string));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        result.Add(row);
                    }
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








    public async Task<List<SpGetChangesInIngestionRunResult>> SpGetChangesInIngestionRunAsync(Guid? IngestionRunId)
    {
        var result = new List<SpGetChangesInIngestionRunResult>();
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
                cmd.CommandText = @"dbo.sp_GetChangesInIngestionRun";

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        SpGetChangesInIngestionRunResult row = new();
                        if (fieldNames.Contains("artifact_kind") && !reader.IsDBNull(reader.GetOrdinal(@"artifact_kind")))
                        {
                            row.ArtifactKind = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"artifact_kind")), typeof(string));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        result.Add(row);
                    }
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








    public List<DocPage> SpGetDocPageHistory(string SemanticUid)
    {
        var result = new List<DocPage>();
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
                cmd.CommandText = @"dbo.sp_GetDocPageHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        DocPage row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("source_snapshot_id") && !reader.IsDBNull(reader.GetOrdinal(@"source_snapshot_id")))
                        {
                            row.SourceSnapshotId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_snapshot_id")), typeof(Guid));
                        }

                        row.SourcePath = fieldNames.Contains("source_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_path")), typeof(string))
                            : null;

                        row.Title = fieldNames.Contains("title") && !reader.IsDBNull(reader.GetOrdinal(@"title"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"title")), typeof(string))
                            : null;

                        row.Language = fieldNames.Contains("language") && !reader.IsDBNull(reader.GetOrdinal(@"language"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"language")), typeof(string))
                            : null;

                        row.Url = fieldNames.Contains("url") && !reader.IsDBNull(reader.GetOrdinal(@"url"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"url")), typeof(string))
                            : null;

                        row.RawMarkdown = fieldNames.Contains("raw_markdown") && !reader.IsDBNull(reader.GetOrdinal(@"raw_markdown"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"raw_markdown")), typeof(string))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public async Task<List<DocPage>> SpGetDocPageHistoryAsync(string SemanticUid)
    {
        var result = new List<DocPage>();
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
                cmd.CommandText = @"dbo.sp_GetDocPageHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        DocPage row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("source_snapshot_id") && !reader.IsDBNull(reader.GetOrdinal(@"source_snapshot_id")))
                        {
                            row.SourceSnapshotId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_snapshot_id")), typeof(Guid));
                        }

                        row.SourcePath = fieldNames.Contains("source_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_path")), typeof(string))
                            : null;

                        row.Title = fieldNames.Contains("title") && !reader.IsDBNull(reader.GetOrdinal(@"title"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"title")), typeof(string))
                            : null;

                        row.Language = fieldNames.Contains("language") && !reader.IsDBNull(reader.GetOrdinal(@"language"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"language")), typeof(string))
                            : null;

                        row.Url = fieldNames.Contains("url") && !reader.IsDBNull(reader.GetOrdinal(@"url"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"url")), typeof(string))
                            : null;

                        row.RawMarkdown = fieldNames.Contains("raw_markdown") && !reader.IsDBNull(reader.GetOrdinal(@"raw_markdown"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"raw_markdown")), typeof(string))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public List<ApiMember> SpGetMemberHistory(string SemanticUid)
    {
        var result = new List<ApiMember>();
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
                cmd.CommandText = @"dbo.sp_GetMemberHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiMember row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("api_feature_id") && !reader.IsDBNull(reader.GetOrdinal(@"api_feature_id")))
                        {
                            row.ApiFeatureId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"api_feature_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.Kind = fieldNames.Contains("kind") && !reader.IsDBNull(reader.GetOrdinal(@"kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"kind")), typeof(string))
                            : null;

                        row.MethodKind = fieldNames.Contains("method_kind") && !reader.IsDBNull(reader.GetOrdinal(@"method_kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"method_kind")), typeof(string))
                            : null;

                        row.Accessibility = fieldNames.Contains("accessibility") && !reader.IsDBNull(reader.GetOrdinal(@"accessibility"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"accessibility")), typeof(string))
                            : null;

                        row.IsStatic = fieldNames.Contains("is_static") && !reader.IsDBNull(reader.GetOrdinal(@"is_static"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_static")), typeof(bool))
                            : null;

                        row.IsExtensionMethod = fieldNames.Contains("is_extension_method") && !reader.IsDBNull(reader.GetOrdinal(@"is_extension_method"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_extension_method")), typeof(bool))
                            : null;

                        row.IsAsync = fieldNames.Contains("is_async") && !reader.IsDBNull(reader.GetOrdinal(@"is_async"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_async")), typeof(bool))
                            : null;

                        row.IsVirtual = fieldNames.Contains("is_virtual") && !reader.IsDBNull(reader.GetOrdinal(@"is_virtual"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_virtual")), typeof(bool))
                            : null;

                        row.IsOverride = fieldNames.Contains("is_override") && !reader.IsDBNull(reader.GetOrdinal(@"is_override"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_override")), typeof(bool))
                            : null;

                        row.IsAbstract = fieldNames.Contains("is_abstract") && !reader.IsDBNull(reader.GetOrdinal(@"is_abstract"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_abstract")), typeof(bool))
                            : null;

                        row.IsSealed = fieldNames.Contains("is_sealed") && !reader.IsDBNull(reader.GetOrdinal(@"is_sealed"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_sealed")), typeof(bool))
                            : null;

                        row.IsReadonly = fieldNames.Contains("is_readonly") && !reader.IsDBNull(reader.GetOrdinal(@"is_readonly"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_readonly")), typeof(bool))
                            : null;

                        row.IsConst = fieldNames.Contains("is_const") && !reader.IsDBNull(reader.GetOrdinal(@"is_const"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_const")), typeof(bool))
                            : null;

                        row.IsUnsafe = fieldNames.Contains("is_unsafe") && !reader.IsDBNull(reader.GetOrdinal(@"is_unsafe"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_unsafe")), typeof(bool))
                            : null;

                        row.ReturnTypeUid = fieldNames.Contains("return_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"return_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"return_type_uid")), typeof(string))
                            : null;

                        row.ReturnNullable = fieldNames.Contains("return_nullable") && !reader.IsDBNull(reader.GetOrdinal(@"return_nullable"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"return_nullable")), typeof(string))
                            : null;

                        row.GenericParameters = fieldNames.Contains("generic_parameters") && !reader.IsDBNull(reader.GetOrdinal(@"generic_parameters"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_parameters")), typeof(string))
                            : null;

                        row.GenericConstraints = fieldNames.Contains("generic_constraints") && !reader.IsDBNull(reader.GetOrdinal(@"generic_constraints"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_constraints")), typeof(string))
                            : null;

                        row.Summary = fieldNames.Contains("summary") && !reader.IsDBNull(reader.GetOrdinal(@"summary"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"summary")), typeof(string))
                            : null;

                        row.Remarks = fieldNames.Contains("remarks") && !reader.IsDBNull(reader.GetOrdinal(@"remarks"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"remarks")), typeof(string))
                            : null;

                        row.Attributes = fieldNames.Contains("attributes") && !reader.IsDBNull(reader.GetOrdinal(@"attributes"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"attributes")), typeof(string))
                            : null;

                        row.SourceFilePath = fieldNames.Contains("source_file_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_file_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_file_path")), typeof(string))
                            : null;

                        row.SourceStartLine = fieldNames.Contains("source_start_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_start_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_start_line")), typeof(int))
                            : null;

                        row.SourceEndLine = fieldNames.Contains("source_end_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_end_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_end_line")), typeof(int))
                            : null;

                        row.MemberUidHash = fieldNames.Contains("member_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"member_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"member_uid_hash")), typeof(byte[]))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        row.UpdatedIngestionRunId = fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid))
                            : null;

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public async Task<List<ApiMember>> SpGetMemberHistoryAsync(string SemanticUid)
    {
        var result = new List<ApiMember>();
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
                cmd.CommandText = @"dbo.sp_GetMemberHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiMember row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("api_feature_id") && !reader.IsDBNull(reader.GetOrdinal(@"api_feature_id")))
                        {
                            row.ApiFeatureId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"api_feature_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.Kind = fieldNames.Contains("kind") && !reader.IsDBNull(reader.GetOrdinal(@"kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"kind")), typeof(string))
                            : null;

                        row.MethodKind = fieldNames.Contains("method_kind") && !reader.IsDBNull(reader.GetOrdinal(@"method_kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"method_kind")), typeof(string))
                            : null;

                        row.Accessibility = fieldNames.Contains("accessibility") && !reader.IsDBNull(reader.GetOrdinal(@"accessibility"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"accessibility")), typeof(string))
                            : null;

                        row.IsStatic = fieldNames.Contains("is_static") && !reader.IsDBNull(reader.GetOrdinal(@"is_static"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_static")), typeof(bool))
                            : null;

                        row.IsExtensionMethod = fieldNames.Contains("is_extension_method") && !reader.IsDBNull(reader.GetOrdinal(@"is_extension_method"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_extension_method")), typeof(bool))
                            : null;

                        row.IsAsync = fieldNames.Contains("is_async") && !reader.IsDBNull(reader.GetOrdinal(@"is_async"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_async")), typeof(bool))
                            : null;

                        row.IsVirtual = fieldNames.Contains("is_virtual") && !reader.IsDBNull(reader.GetOrdinal(@"is_virtual"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_virtual")), typeof(bool))
                            : null;

                        row.IsOverride = fieldNames.Contains("is_override") && !reader.IsDBNull(reader.GetOrdinal(@"is_override"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_override")), typeof(bool))
                            : null;

                        row.IsAbstract = fieldNames.Contains("is_abstract") && !reader.IsDBNull(reader.GetOrdinal(@"is_abstract"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_abstract")), typeof(bool))
                            : null;

                        row.IsSealed = fieldNames.Contains("is_sealed") && !reader.IsDBNull(reader.GetOrdinal(@"is_sealed"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_sealed")), typeof(bool))
                            : null;

                        row.IsReadonly = fieldNames.Contains("is_readonly") && !reader.IsDBNull(reader.GetOrdinal(@"is_readonly"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_readonly")), typeof(bool))
                            : null;

                        row.IsConst = fieldNames.Contains("is_const") && !reader.IsDBNull(reader.GetOrdinal(@"is_const"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_const")), typeof(bool))
                            : null;

                        row.IsUnsafe = fieldNames.Contains("is_unsafe") && !reader.IsDBNull(reader.GetOrdinal(@"is_unsafe"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_unsafe")), typeof(bool))
                            : null;

                        row.ReturnTypeUid = fieldNames.Contains("return_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"return_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"return_type_uid")), typeof(string))
                            : null;

                        row.ReturnNullable = fieldNames.Contains("return_nullable") && !reader.IsDBNull(reader.GetOrdinal(@"return_nullable"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"return_nullable")), typeof(string))
                            : null;

                        row.GenericParameters = fieldNames.Contains("generic_parameters") && !reader.IsDBNull(reader.GetOrdinal(@"generic_parameters"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_parameters")), typeof(string))
                            : null;

                        row.GenericConstraints = fieldNames.Contains("generic_constraints") && !reader.IsDBNull(reader.GetOrdinal(@"generic_constraints"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_constraints")), typeof(string))
                            : null;

                        row.Summary = fieldNames.Contains("summary") && !reader.IsDBNull(reader.GetOrdinal(@"summary"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"summary")), typeof(string))
                            : null;

                        row.Remarks = fieldNames.Contains("remarks") && !reader.IsDBNull(reader.GetOrdinal(@"remarks"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"remarks")), typeof(string))
                            : null;

                        row.Attributes = fieldNames.Contains("attributes") && !reader.IsDBNull(reader.GetOrdinal(@"attributes"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"attributes")), typeof(string))
                            : null;

                        row.SourceFilePath = fieldNames.Contains("source_file_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_file_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_file_path")), typeof(string))
                            : null;

                        row.SourceStartLine = fieldNames.Contains("source_start_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_start_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_start_line")), typeof(int))
                            : null;

                        row.SourceEndLine = fieldNames.Contains("source_end_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_end_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_end_line")), typeof(int))
                            : null;

                        row.MemberUidHash = fieldNames.Contains("member_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"member_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"member_uid_hash")), typeof(byte[]))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        row.UpdatedIngestionRunId = fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid))
                            : null;

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public List<ApiFeature> SpGetSemanticHistory(string SemanticUid)
    {
        var result = new List<ApiFeature>();
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
                cmd.CommandText = @"dbo.sp_GetSemanticHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiFeature row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        row.ApiTypeId = fieldNames.Contains("api_type_id") && !reader.IsDBNull(reader.GetOrdinal(@"api_type_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"api_type_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("truth_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"truth_run_id")))
                        {
                            row.TruthRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"truth_run_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.Language = fieldNames.Contains("language") && !reader.IsDBNull(reader.GetOrdinal(@"language"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"language")), typeof(string))
                            : null;

                        row.Description = fieldNames.Contains("description") && !reader.IsDBNull(reader.GetOrdinal(@"description"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"description")), typeof(string))
                            : null;

                        row.Tags = fieldNames.Contains("tags") && !reader.IsDBNull(reader.GetOrdinal(@"tags"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"tags")), typeof(string))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public async Task<List<ApiFeature>> SpGetSemanticHistoryAsync(string SemanticUid)
    {
        var result = new List<ApiFeature>();
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
                cmd.CommandText = @"dbo.sp_GetSemanticHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiFeature row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        row.ApiTypeId = fieldNames.Contains("api_type_id") && !reader.IsDBNull(reader.GetOrdinal(@"api_type_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"api_type_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("truth_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"truth_run_id")))
                        {
                            row.TruthRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"truth_run_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.Language = fieldNames.Contains("language") && !reader.IsDBNull(reader.GetOrdinal(@"language"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"language")), typeof(string))
                            : null;

                        row.Description = fieldNames.Contains("description") && !reader.IsDBNull(reader.GetOrdinal(@"description"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"description")), typeof(string))
                            : null;

                        row.Tags = fieldNames.Contains("tags") && !reader.IsDBNull(reader.GetOrdinal(@"tags"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"tags")), typeof(string))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public List<ApiType> SpGetTypeHistory(string SemanticUid)
    {
        var result = new List<ApiType>();
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
                cmd.CommandText = @"dbo.sp_GetTypeHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiType row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("source_snapshot_id") && !reader.IsDBNull(reader.GetOrdinal(@"source_snapshot_id")))
                        {
                            row.SourceSnapshotId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_snapshot_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.NamespacePath = fieldNames.Contains("namespace_path") && !reader.IsDBNull(reader.GetOrdinal(@"namespace_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"namespace_path")), typeof(string))
                            : null;

                        row.Kind = fieldNames.Contains("kind") && !reader.IsDBNull(reader.GetOrdinal(@"kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"kind")), typeof(string))
                            : null;

                        row.Accessibility = fieldNames.Contains("accessibility") && !reader.IsDBNull(reader.GetOrdinal(@"accessibility"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"accessibility")), typeof(string))
                            : null;

                        row.IsStatic = fieldNames.Contains("is_static") && !reader.IsDBNull(reader.GetOrdinal(@"is_static"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_static")), typeof(bool))
                            : null;

                        row.IsGeneric = fieldNames.Contains("is_generic") && !reader.IsDBNull(reader.GetOrdinal(@"is_generic"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_generic")), typeof(bool))
                            : null;

                        row.IsAbstract = fieldNames.Contains("is_abstract") && !reader.IsDBNull(reader.GetOrdinal(@"is_abstract"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_abstract")), typeof(bool))
                            : null;

                        row.IsSealed = fieldNames.Contains("is_sealed") && !reader.IsDBNull(reader.GetOrdinal(@"is_sealed"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_sealed")), typeof(bool))
                            : null;

                        row.IsRecord = fieldNames.Contains("is_record") && !reader.IsDBNull(reader.GetOrdinal(@"is_record"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_record")), typeof(bool))
                            : null;

                        row.IsRefLike = fieldNames.Contains("is_ref_like") && !reader.IsDBNull(reader.GetOrdinal(@"is_ref_like"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_ref_like")), typeof(bool))
                            : null;

                        row.BaseTypeUid = fieldNames.Contains("base_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"base_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"base_type_uid")), typeof(string))
                            : null;

                        row.Interfaces = fieldNames.Contains("interfaces") && !reader.IsDBNull(reader.GetOrdinal(@"interfaces"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"interfaces")), typeof(string))
                            : null;

                        row.ContainingTypeUid = fieldNames.Contains("containing_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"containing_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"containing_type_uid")), typeof(string))
                            : null;

                        row.GenericParameters = fieldNames.Contains("generic_parameters") && !reader.IsDBNull(reader.GetOrdinal(@"generic_parameters"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_parameters")), typeof(string))
                            : null;

                        row.GenericConstraints = fieldNames.Contains("generic_constraints") && !reader.IsDBNull(reader.GetOrdinal(@"generic_constraints"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_constraints")), typeof(string))
                            : null;

                        row.Summary = fieldNames.Contains("summary") && !reader.IsDBNull(reader.GetOrdinal(@"summary"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"summary")), typeof(string))
                            : null;

                        row.Remarks = fieldNames.Contains("remarks") && !reader.IsDBNull(reader.GetOrdinal(@"remarks"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"remarks")), typeof(string))
                            : null;

                        row.Attributes = fieldNames.Contains("attributes") && !reader.IsDBNull(reader.GetOrdinal(@"attributes"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"attributes")), typeof(string))
                            : null;

                        row.SourceFilePath = fieldNames.Contains("source_file_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_file_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_file_path")), typeof(string))
                            : null;

                        row.SourceStartLine = fieldNames.Contains("source_start_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_start_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_start_line")), typeof(int))
                            : null;

                        row.SourceEndLine = fieldNames.Contains("source_end_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_end_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_end_line")), typeof(int))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public async Task<List<ApiType>> SpGetTypeHistoryAsync(string SemanticUid)
    {
        var result = new List<ApiType>();
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
                cmd.CommandText = @"dbo.sp_GetTypeHistory";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiType row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("source_snapshot_id") && !reader.IsDBNull(reader.GetOrdinal(@"source_snapshot_id")))
                        {
                            row.SourceSnapshotId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_snapshot_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.NamespacePath = fieldNames.Contains("namespace_path") && !reader.IsDBNull(reader.GetOrdinal(@"namespace_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"namespace_path")), typeof(string))
                            : null;

                        row.Kind = fieldNames.Contains("kind") && !reader.IsDBNull(reader.GetOrdinal(@"kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"kind")), typeof(string))
                            : null;

                        row.Accessibility = fieldNames.Contains("accessibility") && !reader.IsDBNull(reader.GetOrdinal(@"accessibility"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"accessibility")), typeof(string))
                            : null;

                        row.IsStatic = fieldNames.Contains("is_static") && !reader.IsDBNull(reader.GetOrdinal(@"is_static"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_static")), typeof(bool))
                            : null;

                        row.IsGeneric = fieldNames.Contains("is_generic") && !reader.IsDBNull(reader.GetOrdinal(@"is_generic"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_generic")), typeof(bool))
                            : null;

                        row.IsAbstract = fieldNames.Contains("is_abstract") && !reader.IsDBNull(reader.GetOrdinal(@"is_abstract"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_abstract")), typeof(bool))
                            : null;

                        row.IsSealed = fieldNames.Contains("is_sealed") && !reader.IsDBNull(reader.GetOrdinal(@"is_sealed"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_sealed")), typeof(bool))
                            : null;

                        row.IsRecord = fieldNames.Contains("is_record") && !reader.IsDBNull(reader.GetOrdinal(@"is_record"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_record")), typeof(bool))
                            : null;

                        row.IsRefLike = fieldNames.Contains("is_ref_like") && !reader.IsDBNull(reader.GetOrdinal(@"is_ref_like"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_ref_like")), typeof(bool))
                            : null;

                        row.BaseTypeUid = fieldNames.Contains("base_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"base_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"base_type_uid")), typeof(string))
                            : null;

                        row.Interfaces = fieldNames.Contains("interfaces") && !reader.IsDBNull(reader.GetOrdinal(@"interfaces"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"interfaces")), typeof(string))
                            : null;

                        row.ContainingTypeUid = fieldNames.Contains("containing_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"containing_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"containing_type_uid")), typeof(string))
                            : null;

                        row.GenericParameters = fieldNames.Contains("generic_parameters") && !reader.IsDBNull(reader.GetOrdinal(@"generic_parameters"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_parameters")), typeof(string))
                            : null;

                        row.GenericConstraints = fieldNames.Contains("generic_constraints") && !reader.IsDBNull(reader.GetOrdinal(@"generic_constraints"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_constraints")), typeof(string))
                            : null;

                        row.Summary = fieldNames.Contains("summary") && !reader.IsDBNull(reader.GetOrdinal(@"summary"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"summary")), typeof(string))
                            : null;

                        row.Remarks = fieldNames.Contains("remarks") && !reader.IsDBNull(reader.GetOrdinal(@"remarks"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"remarks")), typeof(string))
                            : null;

                        row.Attributes = fieldNames.Contains("attributes") && !reader.IsDBNull(reader.GetOrdinal(@"attributes"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"attributes")), typeof(string))
                            : null;

                        row.SourceFilePath = fieldNames.Contains("source_file_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_file_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_file_path")), typeof(string))
                            : null;

                        row.SourceStartLine = fieldNames.Contains("source_start_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_start_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_start_line")), typeof(int))
                            : null;

                        row.SourceEndLine = fieldNames.Contains("source_end_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_end_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_end_line")), typeof(int))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public void SpRegisterSemanticIdentity(string Uid, string Kind, string Notes)
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
                cmd.CommandText = @"dbo.sp_RegisterSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                UidParameter.Value = Uid != null ? Uid : DBNull.Value;

                _ = cmd.Parameters.Add(UidParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 50;
                KindParameter.Value = Kind != null ? Kind : DBNull.Value;

                _ = cmd.Parameters.Add(KindParameter);

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

                _ = cmd.Parameters.Add(NotesParameter);
                _ = cmd.ExecuteNonQuery();
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








    public async Task SpRegisterSemanticIdentityAsync(string Uid, string Kind, string Notes)
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
                cmd.CommandText = @"dbo.sp_RegisterSemanticIdentity";

                DbParameter UidParameter = cmd.CreateParameter();
                UidParameter.ParameterName = "Uid";
                UidParameter.Direction = ParameterDirection.Input;
                UidParameter.DbType = DbType.String;
                UidParameter.Size = 1000;
                UidParameter.Value = Uid != null ? Uid : DBNull.Value;

                _ = cmd.Parameters.Add(UidParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 50;
                KindParameter.Value = Kind != null ? Kind : DBNull.Value;

                _ = cmd.Parameters.Add(KindParameter);

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

                _ = cmd.Parameters.Add(NotesParameter);
                _ = await cmd.ExecuteNonQueryAsync();
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








    public void SpUpsertApiFeature(string SemanticUid, Guid? TruthRunId, Guid? IngestionRunId, string Name, string Language, string Description, string Tags)
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
                cmd.CommandText = @"dbo.sp_UpsertApiFeature";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(TruthRunIdParameter);

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                NameParameter.Value = Name != null ? Name : DBNull.Value;

                _ = cmd.Parameters.Add(NameParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                LanguageParameter.Value = Language != null ? Language : DBNull.Value;

                _ = cmd.Parameters.Add(LanguageParameter);

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

                _ = cmd.Parameters.Add(DescriptionParameter);

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

                _ = cmd.Parameters.Add(TagsParameter);
                _ = cmd.ExecuteNonQuery();
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








    public async Task SpUpsertApiFeatureAsync(string SemanticUid, Guid? TruthRunId, Guid? IngestionRunId, string Name, string Language, string Description, string Tags)
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
                cmd.CommandText = @"dbo.sp_UpsertApiFeature";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(TruthRunIdParameter);

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                NameParameter.Value = Name != null ? Name : DBNull.Value;

                _ = cmd.Parameters.Add(NameParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                LanguageParameter.Value = Language != null ? Language : DBNull.Value;

                _ = cmd.Parameters.Add(LanguageParameter);

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

                _ = cmd.Parameters.Add(DescriptionParameter);

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

                _ = cmd.Parameters.Add(TagsParameter);
                _ = await cmd.ExecuteNonQueryAsync();
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








    public void SpUpsertApiMember(string SemanticUid, Guid? ApiTypeId, Guid? IngestionRunId, string Name, string Kind, string MethodKind, string Accessibility, bool? IsStatic, bool? IsExtensionMethod, bool? IsAsync, bool? IsVirtual, bool? IsOverride, bool? IsAbstract, bool? IsSealed, bool? IsReadOnly, bool? IsConst, bool? IsUnsafe, string ReturnTypeUid, string ReturnNullable, string GenericParameters, string GenericConstraints, string Summary, string Remarks, string Attributes, string SourceFilePath, int? SourceStartLine, int? SourceEndLine)
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
                cmd.CommandText = @"dbo.sp_UpsertApiMember";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(ApiTypeIdParameter);

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                NameParameter.Value = Name != null ? Name : DBNull.Value;

                _ = cmd.Parameters.Add(NameParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 200;
                KindParameter.Value = Kind != null ? Kind : DBNull.Value;

                _ = cmd.Parameters.Add(KindParameter);

                DbParameter MethodKindParameter = cmd.CreateParameter();
                MethodKindParameter.ParameterName = "MethodKind";
                MethodKindParameter.Direction = ParameterDirection.Input;
                MethodKindParameter.DbType = DbType.String;
                MethodKindParameter.Size = 200;
                MethodKindParameter.Value = MethodKind != null ? MethodKind : DBNull.Value;

                _ = cmd.Parameters.Add(MethodKindParameter);

                DbParameter AccessibilityParameter = cmd.CreateParameter();
                AccessibilityParameter.ParameterName = "Accessibility";
                AccessibilityParameter.Direction = ParameterDirection.Input;
                AccessibilityParameter.DbType = DbType.String;
                AccessibilityParameter.Size = 200;
                AccessibilityParameter.Value = Accessibility != null ? Accessibility : DBNull.Value;

                _ = cmd.Parameters.Add(AccessibilityParameter);

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

                _ = cmd.Parameters.Add(IsStaticParameter);

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

                _ = cmd.Parameters.Add(IsExtensionMethodParameter);

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

                _ = cmd.Parameters.Add(IsAsyncParameter);

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

                _ = cmd.Parameters.Add(IsVirtualParameter);

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

                _ = cmd.Parameters.Add(IsOverrideParameter);

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

                _ = cmd.Parameters.Add(IsAbstractParameter);

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

                _ = cmd.Parameters.Add(IsSealedParameter);

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

                _ = cmd.Parameters.Add(IsReadOnlyParameter);

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

                _ = cmd.Parameters.Add(IsConstParameter);

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

                _ = cmd.Parameters.Add(IsUnsafeParameter);

                DbParameter ReturnTypeUidParameter = cmd.CreateParameter();
                ReturnTypeUidParameter.ParameterName = "ReturnTypeUid";
                ReturnTypeUidParameter.Direction = ParameterDirection.Input;
                ReturnTypeUidParameter.DbType = DbType.String;
                ReturnTypeUidParameter.Size = 1000;
                ReturnTypeUidParameter.Value = ReturnTypeUid != null ? ReturnTypeUid : DBNull.Value;

                _ = cmd.Parameters.Add(ReturnTypeUidParameter);

                DbParameter ReturnNullableParameter = cmd.CreateParameter();
                ReturnNullableParameter.ParameterName = "ReturnNullable";
                ReturnNullableParameter.Direction = ParameterDirection.Input;
                ReturnNullableParameter.DbType = DbType.String;
                ReturnNullableParameter.Size = 50;
                ReturnNullableParameter.Value = ReturnNullable != null ? ReturnNullable : DBNull.Value;

                _ = cmd.Parameters.Add(ReturnNullableParameter);

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

                _ = cmd.Parameters.Add(GenericParametersParameter);

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

                _ = cmd.Parameters.Add(GenericConstraintsParameter);

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

                _ = cmd.Parameters.Add(SummaryParameter);

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

                _ = cmd.Parameters.Add(RemarksParameter);

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

                _ = cmd.Parameters.Add(AttributesParameter);

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

                _ = cmd.Parameters.Add(SourceFilePathParameter);

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

                _ = cmd.Parameters.Add(SourceStartLineParameter);

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

                _ = cmd.Parameters.Add(SourceEndLineParameter);
                _ = cmd.ExecuteNonQuery();
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








    public async Task SpUpsertApiMemberAsync(string SemanticUid, Guid? ApiTypeId, Guid? IngestionRunId, string Name, string Kind, string MethodKind, string Accessibility, bool? IsStatic, bool? IsExtensionMethod, bool? IsAsync, bool? IsVirtual, bool? IsOverride, bool? IsAbstract, bool? IsSealed, bool? IsReadOnly, bool? IsConst, bool? IsUnsafe, string ReturnTypeUid, string ReturnNullable, string GenericParameters, string GenericConstraints, string Summary, string Remarks, string Attributes, string SourceFilePath, int? SourceStartLine, int? SourceEndLine)
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
                cmd.CommandText = @"dbo.sp_UpsertApiMember";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(ApiTypeIdParameter);

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                NameParameter.Value = Name != null ? Name : DBNull.Value;

                _ = cmd.Parameters.Add(NameParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 200;
                KindParameter.Value = Kind != null ? Kind : DBNull.Value;

                _ = cmd.Parameters.Add(KindParameter);

                DbParameter MethodKindParameter = cmd.CreateParameter();
                MethodKindParameter.ParameterName = "MethodKind";
                MethodKindParameter.Direction = ParameterDirection.Input;
                MethodKindParameter.DbType = DbType.String;
                MethodKindParameter.Size = 200;
                MethodKindParameter.Value = MethodKind != null ? MethodKind : DBNull.Value;

                _ = cmd.Parameters.Add(MethodKindParameter);

                DbParameter AccessibilityParameter = cmd.CreateParameter();
                AccessibilityParameter.ParameterName = "Accessibility";
                AccessibilityParameter.Direction = ParameterDirection.Input;
                AccessibilityParameter.DbType = DbType.String;
                AccessibilityParameter.Size = 200;
                AccessibilityParameter.Value = Accessibility != null ? Accessibility : DBNull.Value;

                _ = cmd.Parameters.Add(AccessibilityParameter);

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

                _ = cmd.Parameters.Add(IsStaticParameter);

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

                _ = cmd.Parameters.Add(IsExtensionMethodParameter);

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

                _ = cmd.Parameters.Add(IsAsyncParameter);

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

                _ = cmd.Parameters.Add(IsVirtualParameter);

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

                _ = cmd.Parameters.Add(IsOverrideParameter);

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

                _ = cmd.Parameters.Add(IsAbstractParameter);

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

                _ = cmd.Parameters.Add(IsSealedParameter);

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

                _ = cmd.Parameters.Add(IsReadOnlyParameter);

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

                _ = cmd.Parameters.Add(IsConstParameter);

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

                _ = cmd.Parameters.Add(IsUnsafeParameter);

                DbParameter ReturnTypeUidParameter = cmd.CreateParameter();
                ReturnTypeUidParameter.ParameterName = "ReturnTypeUid";
                ReturnTypeUidParameter.Direction = ParameterDirection.Input;
                ReturnTypeUidParameter.DbType = DbType.String;
                ReturnTypeUidParameter.Size = 1000;
                ReturnTypeUidParameter.Value = ReturnTypeUid != null ? ReturnTypeUid : DBNull.Value;

                _ = cmd.Parameters.Add(ReturnTypeUidParameter);

                DbParameter ReturnNullableParameter = cmd.CreateParameter();
                ReturnNullableParameter.ParameterName = "ReturnNullable";
                ReturnNullableParameter.Direction = ParameterDirection.Input;
                ReturnNullableParameter.DbType = DbType.String;
                ReturnNullableParameter.Size = 50;
                ReturnNullableParameter.Value = ReturnNullable != null ? ReturnNullable : DBNull.Value;

                _ = cmd.Parameters.Add(ReturnNullableParameter);

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

                _ = cmd.Parameters.Add(GenericParametersParameter);

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

                _ = cmd.Parameters.Add(GenericConstraintsParameter);

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

                _ = cmd.Parameters.Add(SummaryParameter);

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

                _ = cmd.Parameters.Add(RemarksParameter);

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

                _ = cmd.Parameters.Add(AttributesParameter);

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

                _ = cmd.Parameters.Add(SourceFilePathParameter);

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

                _ = cmd.Parameters.Add(SourceStartLineParameter);

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

                _ = cmd.Parameters.Add(SourceEndLineParameter);
                _ = await cmd.ExecuteNonQueryAsync();
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








    public SpUpsertApiParameterMultipleResult SpUpsertApiParameter(Guid? api_member_id, string name, string type_uid, string nullable_annotation, int? position, string modifier, bool? has_default_value, string default_value_literal, Guid? ingestion_run_id)
    {
        SpUpsertApiParameterMultipleResult result = new();
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
                cmd.CommandText = @"dbo.sp_UpsertApiParameter";

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

                _ = cmd.Parameters.Add(api_member_idParameter);

                DbParameter nameParameter = cmd.CreateParameter();
                nameParameter.ParameterName = "name";
                nameParameter.Direction = ParameterDirection.Input;
                nameParameter.DbType = DbType.String;
                nameParameter.Size = 200;
                nameParameter.Value = name != null ? name : DBNull.Value;

                _ = cmd.Parameters.Add(nameParameter);

                DbParameter type_uidParameter = cmd.CreateParameter();
                type_uidParameter.ParameterName = "type_uid";
                type_uidParameter.Direction = ParameterDirection.Input;
                type_uidParameter.DbType = DbType.String;
                type_uidParameter.Size = 1000;
                type_uidParameter.Value = type_uid != null ? type_uid : DBNull.Value;

                _ = cmd.Parameters.Add(type_uidParameter);

                DbParameter nullable_annotationParameter = cmd.CreateParameter();
                nullable_annotationParameter.ParameterName = "nullable_annotation";
                nullable_annotationParameter.Direction = ParameterDirection.Input;
                nullable_annotationParameter.DbType = DbType.String;
                nullable_annotationParameter.Size = 50;
                nullable_annotationParameter.Value = nullable_annotation != null ? nullable_annotation : DBNull.Value;

                _ = cmd.Parameters.Add(nullable_annotationParameter);

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

                _ = cmd.Parameters.Add(positionParameter);

                DbParameter modifierParameter = cmd.CreateParameter();
                modifierParameter.ParameterName = "modifier";
                modifierParameter.Direction = ParameterDirection.Input;
                modifierParameter.DbType = DbType.String;
                modifierParameter.Size = 50;
                modifierParameter.Value = modifier != null ? modifier : DBNull.Value;

                _ = cmd.Parameters.Add(modifierParameter);

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

                _ = cmd.Parameters.Add(has_default_valueParameter);

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

                _ = cmd.Parameters.Add(default_value_literalParameter);

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

                _ = cmd.Parameters.Add(ingestion_run_idParameter);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    string[] fieldNames;
                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpUpsertApiParameterResult resultRow = new()
                        {
                            Id = fieldNames.Length == 1 && string.IsNullOrEmpty(fieldNames[0])
                                ? (Guid)Convert.ChangeType(reader.GetValue(0), typeof(Guid))
                                : fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id"))
                                    ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid))
                                    : null
                        };

                        result.SpUpsertApiParameterResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpUpsertApiParameterResult resultRow = new()
                        {
                            Id = fieldNames.Length == 1 && string.IsNullOrEmpty(fieldNames[0])
                                ? (Guid)Convert.ChangeType(reader.GetValue(0), typeof(Guid))
                                : fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id"))
                                    ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid))
                                    : null
                        };

                        result.SpUpsertApiParameterResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpUpsertApiParameterResult resultRow = new()
                        {
                            Id = fieldNames.Length == 1 && string.IsNullOrEmpty(fieldNames[0])
                                ? (Guid)Convert.ChangeType(reader.GetValue(0), typeof(Guid))
                                : fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id"))
                                    ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid))
                                    : null
                        };

                        result.SpUpsertApiParameterResults.Add(resultRow);
                    }

                    _ = reader.NextResult();
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








    public async Task<SpUpsertApiParameterMultipleResult> SpUpsertApiParameterAsync(Guid? api_member_id, string name, string type_uid, string nullable_annotation, int? position, string modifier, bool? has_default_value, string default_value_literal, Guid? ingestion_run_id)
    {
        SpUpsertApiParameterMultipleResult result = new();
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
                cmd.CommandText = @"dbo.sp_UpsertApiParameter";

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

                _ = cmd.Parameters.Add(api_member_idParameter);

                DbParameter nameParameter = cmd.CreateParameter();
                nameParameter.ParameterName = "name";
                nameParameter.Direction = ParameterDirection.Input;
                nameParameter.DbType = DbType.String;
                nameParameter.Size = 200;
                nameParameter.Value = name != null ? name : DBNull.Value;

                _ = cmd.Parameters.Add(nameParameter);

                DbParameter type_uidParameter = cmd.CreateParameter();
                type_uidParameter.ParameterName = "type_uid";
                type_uidParameter.Direction = ParameterDirection.Input;
                type_uidParameter.DbType = DbType.String;
                type_uidParameter.Size = 1000;
                type_uidParameter.Value = type_uid != null ? type_uid : DBNull.Value;

                _ = cmd.Parameters.Add(type_uidParameter);

                DbParameter nullable_annotationParameter = cmd.CreateParameter();
                nullable_annotationParameter.ParameterName = "nullable_annotation";
                nullable_annotationParameter.Direction = ParameterDirection.Input;
                nullable_annotationParameter.DbType = DbType.String;
                nullable_annotationParameter.Size = 50;
                nullable_annotationParameter.Value = nullable_annotation != null ? nullable_annotation : DBNull.Value;

                _ = cmd.Parameters.Add(nullable_annotationParameter);

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

                _ = cmd.Parameters.Add(positionParameter);

                DbParameter modifierParameter = cmd.CreateParameter();
                modifierParameter.ParameterName = "modifier";
                modifierParameter.Direction = ParameterDirection.Input;
                modifierParameter.DbType = DbType.String;
                modifierParameter.Size = 50;
                modifierParameter.Value = modifier != null ? modifier : DBNull.Value;

                _ = cmd.Parameters.Add(modifierParameter);

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

                _ = cmd.Parameters.Add(has_default_valueParameter);

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

                _ = cmd.Parameters.Add(default_value_literalParameter);

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

                _ = cmd.Parameters.Add(ingestion_run_idParameter);
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    string[] fieldNames;
                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpUpsertApiParameterResult resultRow = new()
                        {
                            Id = fieldNames.Length == 1 && string.IsNullOrEmpty(fieldNames[0])
                                ? (Guid)Convert.ChangeType(reader.GetValue(0), typeof(Guid))
                                : fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id"))
                                    ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid))
                                    : null
                        };

                        result.SpUpsertApiParameterResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpUpsertApiParameterResult resultRow = new()
                        {
                            Id = fieldNames.Length == 1 && string.IsNullOrEmpty(fieldNames[0])
                                ? (Guid)Convert.ChangeType(reader.GetValue(0), typeof(Guid))
                                : fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id"))
                                    ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid))
                                    : null
                        };

                        result.SpUpsertApiParameterResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpUpsertApiParameterResult resultRow = new()
                        {
                            Id = fieldNames.Length == 1 && string.IsNullOrEmpty(fieldNames[0])
                                ? (Guid)Convert.ChangeType(reader.GetValue(0), typeof(Guid))
                                : fieldNames.Contains(@"id") && !reader.IsDBNull(reader.GetOrdinal(@"id"))
                                    ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid))
                                    : null
                        };

                        result.SpUpsertApiParameterResults.Add(resultRow);
                    }

                    _ = reader.NextResult();
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








    public void SpUpsertApiType(string SemanticUid, Guid? SourceSnapshotId, Guid? IngestionRunId, string Name, string NamespacePath, string Kind, string Accessibility, bool? IsStatic, bool? IsGeneric, bool? IsAbstract, bool? IsSealed, bool? IsRecord, bool? IsRefLike, string BaseTypeUid, string Interfaces, string ContainingTypeUid, string GenericParameters, string GenericConstraints, string Summary, string Remarks, string Attributes, string SourceFilePath, int? SourceStartLine, int? SourceEndLine)
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
                cmd.CommandText = @"dbo.sp_UpsertApiType";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(SourceSnapshotIdParameter);

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                NameParameter.Value = Name != null ? Name : DBNull.Value;

                _ = cmd.Parameters.Add(NameParameter);

                DbParameter NamespacePathParameter = cmd.CreateParameter();
                NamespacePathParameter.ParameterName = "NamespacePath";
                NamespacePathParameter.Direction = ParameterDirection.Input;
                NamespacePathParameter.DbType = DbType.String;
                NamespacePathParameter.Size = 1000;
                NamespacePathParameter.Value = NamespacePath != null ? NamespacePath : DBNull.Value;

                _ = cmd.Parameters.Add(NamespacePathParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 200;
                KindParameter.Value = Kind != null ? Kind : DBNull.Value;

                _ = cmd.Parameters.Add(KindParameter);

                DbParameter AccessibilityParameter = cmd.CreateParameter();
                AccessibilityParameter.ParameterName = "Accessibility";
                AccessibilityParameter.Direction = ParameterDirection.Input;
                AccessibilityParameter.DbType = DbType.String;
                AccessibilityParameter.Size = 200;
                AccessibilityParameter.Value = Accessibility != null ? Accessibility : DBNull.Value;

                _ = cmd.Parameters.Add(AccessibilityParameter);

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

                _ = cmd.Parameters.Add(IsStaticParameter);

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

                _ = cmd.Parameters.Add(IsGenericParameter);

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

                _ = cmd.Parameters.Add(IsAbstractParameter);

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

                _ = cmd.Parameters.Add(IsSealedParameter);

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

                _ = cmd.Parameters.Add(IsRecordParameter);

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

                _ = cmd.Parameters.Add(IsRefLikeParameter);

                DbParameter BaseTypeUidParameter = cmd.CreateParameter();
                BaseTypeUidParameter.ParameterName = "BaseTypeUid";
                BaseTypeUidParameter.Direction = ParameterDirection.Input;
                BaseTypeUidParameter.DbType = DbType.String;
                BaseTypeUidParameter.Size = 1000;
                BaseTypeUidParameter.Value = BaseTypeUid != null ? BaseTypeUid : DBNull.Value;

                _ = cmd.Parameters.Add(BaseTypeUidParameter);

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

                _ = cmd.Parameters.Add(InterfacesParameter);

                DbParameter ContainingTypeUidParameter = cmd.CreateParameter();
                ContainingTypeUidParameter.ParameterName = "ContainingTypeUid";
                ContainingTypeUidParameter.Direction = ParameterDirection.Input;
                ContainingTypeUidParameter.DbType = DbType.String;
                ContainingTypeUidParameter.Size = 1000;
                ContainingTypeUidParameter.Value = ContainingTypeUid != null ? ContainingTypeUid : DBNull.Value;

                _ = cmd.Parameters.Add(ContainingTypeUidParameter);

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

                _ = cmd.Parameters.Add(GenericParametersParameter);

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

                _ = cmd.Parameters.Add(GenericConstraintsParameter);

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

                _ = cmd.Parameters.Add(SummaryParameter);

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

                _ = cmd.Parameters.Add(RemarksParameter);

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

                _ = cmd.Parameters.Add(AttributesParameter);

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

                _ = cmd.Parameters.Add(SourceFilePathParameter);

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

                _ = cmd.Parameters.Add(SourceStartLineParameter);

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

                _ = cmd.Parameters.Add(SourceEndLineParameter);
                _ = cmd.ExecuteNonQuery();
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








    public async Task SpUpsertApiTypeAsync(string SemanticUid, Guid? SourceSnapshotId, Guid? IngestionRunId, string Name, string NamespacePath, string Kind, string Accessibility, bool? IsStatic, bool? IsGeneric, bool? IsAbstract, bool? IsSealed, bool? IsRecord, bool? IsRefLike, string BaseTypeUid, string Interfaces, string ContainingTypeUid, string GenericParameters, string GenericConstraints, string Summary, string Remarks, string Attributes, string SourceFilePath, int? SourceStartLine, int? SourceEndLine)
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
                cmd.CommandText = @"dbo.sp_UpsertApiType";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(SourceSnapshotIdParameter);

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);

                DbParameter NameParameter = cmd.CreateParameter();
                NameParameter.ParameterName = "Name";
                NameParameter.Direction = ParameterDirection.Input;
                NameParameter.DbType = DbType.String;
                NameParameter.Size = 400;
                NameParameter.Value = Name != null ? Name : DBNull.Value;

                _ = cmd.Parameters.Add(NameParameter);

                DbParameter NamespacePathParameter = cmd.CreateParameter();
                NamespacePathParameter.ParameterName = "NamespacePath";
                NamespacePathParameter.Direction = ParameterDirection.Input;
                NamespacePathParameter.DbType = DbType.String;
                NamespacePathParameter.Size = 1000;
                NamespacePathParameter.Value = NamespacePath != null ? NamespacePath : DBNull.Value;

                _ = cmd.Parameters.Add(NamespacePathParameter);

                DbParameter KindParameter = cmd.CreateParameter();
                KindParameter.ParameterName = "Kind";
                KindParameter.Direction = ParameterDirection.Input;
                KindParameter.DbType = DbType.String;
                KindParameter.Size = 200;
                KindParameter.Value = Kind != null ? Kind : DBNull.Value;

                _ = cmd.Parameters.Add(KindParameter);

                DbParameter AccessibilityParameter = cmd.CreateParameter();
                AccessibilityParameter.ParameterName = "Accessibility";
                AccessibilityParameter.Direction = ParameterDirection.Input;
                AccessibilityParameter.DbType = DbType.String;
                AccessibilityParameter.Size = 200;
                AccessibilityParameter.Value = Accessibility != null ? Accessibility : DBNull.Value;

                _ = cmd.Parameters.Add(AccessibilityParameter);

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

                _ = cmd.Parameters.Add(IsStaticParameter);

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

                _ = cmd.Parameters.Add(IsGenericParameter);

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

                _ = cmd.Parameters.Add(IsAbstractParameter);

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

                _ = cmd.Parameters.Add(IsSealedParameter);

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

                _ = cmd.Parameters.Add(IsRecordParameter);

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

                _ = cmd.Parameters.Add(IsRefLikeParameter);

                DbParameter BaseTypeUidParameter = cmd.CreateParameter();
                BaseTypeUidParameter.ParameterName = "BaseTypeUid";
                BaseTypeUidParameter.Direction = ParameterDirection.Input;
                BaseTypeUidParameter.DbType = DbType.String;
                BaseTypeUidParameter.Size = 1000;
                BaseTypeUidParameter.Value = BaseTypeUid != null ? BaseTypeUid : DBNull.Value;

                _ = cmd.Parameters.Add(BaseTypeUidParameter);

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

                _ = cmd.Parameters.Add(InterfacesParameter);

                DbParameter ContainingTypeUidParameter = cmd.CreateParameter();
                ContainingTypeUidParameter.ParameterName = "ContainingTypeUid";
                ContainingTypeUidParameter.Direction = ParameterDirection.Input;
                ContainingTypeUidParameter.DbType = DbType.String;
                ContainingTypeUidParameter.Size = 1000;
                ContainingTypeUidParameter.Value = ContainingTypeUid != null ? ContainingTypeUid : DBNull.Value;

                _ = cmd.Parameters.Add(ContainingTypeUidParameter);

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

                _ = cmd.Parameters.Add(GenericParametersParameter);

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

                _ = cmd.Parameters.Add(GenericConstraintsParameter);

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

                _ = cmd.Parameters.Add(SummaryParameter);

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

                _ = cmd.Parameters.Add(RemarksParameter);

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

                _ = cmd.Parameters.Add(AttributesParameter);

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

                _ = cmd.Parameters.Add(SourceFilePathParameter);

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

                _ = cmd.Parameters.Add(SourceStartLineParameter);

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

                _ = cmd.Parameters.Add(SourceEndLineParameter);
                _ = await cmd.ExecuteNonQueryAsync();
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








    public void SpUpsertDocPage(string SemanticUid, Guid? SourceSnapshotId, Guid? IngestionRunId, string SourcePath, string Title, string Language, string Url, string RawMarkdown)
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
                cmd.CommandText = @"dbo.sp_UpsertDocPage";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(SourceSnapshotIdParameter);

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);

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

                _ = cmd.Parameters.Add(SourcePathParameter);

                DbParameter TitleParameter = cmd.CreateParameter();
                TitleParameter.ParameterName = "Title";
                TitleParameter.Direction = ParameterDirection.Input;
                TitleParameter.DbType = DbType.String;
                TitleParameter.Size = 400;
                TitleParameter.Value = Title != null ? Title : DBNull.Value;

                _ = cmd.Parameters.Add(TitleParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                LanguageParameter.Value = Language != null ? Language : DBNull.Value;

                _ = cmd.Parameters.Add(LanguageParameter);

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

                _ = cmd.Parameters.Add(UrlParameter);

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

                _ = cmd.Parameters.Add(RawMarkdownParameter);
                _ = cmd.ExecuteNonQuery();
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








    public async Task SpUpsertDocPageAsync(string SemanticUid, Guid? SourceSnapshotId, Guid? IngestionRunId, string SourcePath, string Title, string Language, string Url, string RawMarkdown)
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
                cmd.CommandText = @"dbo.sp_UpsertDocPage";

                DbParameter SemanticUidParameter = cmd.CreateParameter();
                SemanticUidParameter.ParameterName = "SemanticUid";
                SemanticUidParameter.Direction = ParameterDirection.Input;
                SemanticUidParameter.DbType = DbType.String;
                SemanticUidParameter.Size = 1000;
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(SourceSnapshotIdParameter);

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);

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

                _ = cmd.Parameters.Add(SourcePathParameter);

                DbParameter TitleParameter = cmd.CreateParameter();
                TitleParameter.ParameterName = "Title";
                TitleParameter.Direction = ParameterDirection.Input;
                TitleParameter.DbType = DbType.String;
                TitleParameter.Size = 400;
                TitleParameter.Value = Title != null ? Title : DBNull.Value;

                _ = cmd.Parameters.Add(TitleParameter);

                DbParameter LanguageParameter = cmd.CreateParameter();
                LanguageParameter.ParameterName = "Language";
                LanguageParameter.Direction = ParameterDirection.Input;
                LanguageParameter.DbType = DbType.String;
                LanguageParameter.Size = 200;
                LanguageParameter.Value = Language != null ? Language : DBNull.Value;

                _ = cmd.Parameters.Add(LanguageParameter);

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

                _ = cmd.Parameters.Add(UrlParameter);

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

                _ = cmd.Parameters.Add(RawMarkdownParameter);
                _ = await cmd.ExecuteNonQueryAsync();
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








    public SpVerifyIngestionRunMultipleResult SpVerifyIngestionRun(Guid? IngestionRunId)
    {
        SpVerifyIngestionRunMultipleResult result = new();
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
                cmd.CommandText = @"dbo.sp_VerifyIngestionRun";

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    string[] fieldNames;
                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpVerifyIngestionRunResult resultRow = new()
                        {
                            Category = fieldNames.Contains(@"category") && !reader.IsDBNull(reader.GetOrdinal(@"category"))
                                ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"category")), typeof(string))
                                : null,

                            Detail = fieldNames.Contains(@"detail") && !reader.IsDBNull(reader.GetOrdinal(@"detail"))
                                ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"detail")), typeof(string))
                                : null
                        };

                        result.SpVerifyIngestionRunResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpVerifyIngestionRunResult resultRow = new()
                        {
                            Category = fieldNames.Contains(@"category") && !reader.IsDBNull(reader.GetOrdinal(@"category"))
                                ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"category")), typeof(string))
                                : null,

                            Detail = fieldNames.Contains(@"detail") && !reader.IsDBNull(reader.GetOrdinal(@"detail"))
                                ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"detail")), typeof(string))
                                : null
                        };

                        result.SpVerifyIngestionRunResults.Add(resultRow);
                    }

                    _ = reader.NextResult();
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








    public async Task<SpVerifyIngestionRunMultipleResult> SpVerifyIngestionRunAsync(Guid? IngestionRunId)
    {
        SpVerifyIngestionRunMultipleResult result = new();
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
                cmd.CommandText = @"dbo.sp_VerifyIngestionRun";

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

                _ = cmd.Parameters.Add(IngestionRunIdParameter);
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    string[] fieldNames;
                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpVerifyIngestionRunResult resultRow = new()
                        {
                            Category = fieldNames.Contains(@"category") && !reader.IsDBNull(reader.GetOrdinal(@"category"))
                                ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"category")), typeof(string))
                                : null,

                            Detail = fieldNames.Contains(@"detail") && !reader.IsDBNull(reader.GetOrdinal(@"detail"))
                                ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"detail")), typeof(string))
                                : null
                        };

                        result.SpVerifyIngestionRunResults.Add(resultRow);
                    }

                    _ = reader.NextResult();

                    fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        SpVerifyIngestionRunResult resultRow = new()
                        {
                            Category = fieldNames.Contains(@"category") && !reader.IsDBNull(reader.GetOrdinal(@"category"))
                                ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"category")), typeof(string))
                                : null,

                            Detail = fieldNames.Contains(@"detail") && !reader.IsDBNull(reader.GetOrdinal(@"detail"))
                                ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"detail")), typeof(string))
                                : null
                        };

                        result.SpVerifyIngestionRunResults.Add(resultRow);
                    }

                    _ = reader.NextResult();
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

                _ = cmd.Parameters.Add(InputParameter);

                DbParameter returnValueParameter = cmd.CreateParameter();
                returnValueParameter.Direction = ParameterDirection.ReturnValue;
                returnValueParameter.DbType = DbType.Binary;
                returnValueParameter.Size = -1;
                _ = cmd.Parameters.Add(returnValueParameter);
                _ = cmd.ExecuteNonQuery();
                result = returnValueParameter.Value is not null and not DBNull
                    ? (byte[])Convert.ChangeType(returnValueParameter.Value, typeof(byte[]))
                    : default;
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

                _ = cmd.Parameters.Add(InputParameter);

                DbParameter returnValueParameter = cmd.CreateParameter();
                returnValueParameter.Direction = ParameterDirection.ReturnValue;
                returnValueParameter.DbType = DbType.Binary;
                returnValueParameter.Size = -1;
                _ = cmd.Parameters.Add(returnValueParameter);
                _ = await cmd.ExecuteNonQueryAsync();
                result = returnValueParameter.Value is not null and not DBNull
                    ? (byte[])Convert.ChangeType(returnValueParameter.Value, typeof(byte[]))
                    : default;
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








    public List<DocPage> FnGetDocPageAsOf(string SemanticUid, DateTime? AsOfUtc)
    {
        var result = new List<DocPage>();
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
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(AsOfUtcParameter);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        DocPage row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("source_snapshot_id") && !reader.IsDBNull(reader.GetOrdinal(@"source_snapshot_id")))
                        {
                            row.SourceSnapshotId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_snapshot_id")), typeof(Guid));
                        }

                        row.SourcePath = fieldNames.Contains("source_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_path")), typeof(string))
                            : null;

                        row.Title = fieldNames.Contains("title") && !reader.IsDBNull(reader.GetOrdinal(@"title"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"title")), typeof(string))
                            : null;

                        row.Language = fieldNames.Contains("language") && !reader.IsDBNull(reader.GetOrdinal(@"language"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"language")), typeof(string))
                            : null;

                        row.Url = fieldNames.Contains("url") && !reader.IsDBNull(reader.GetOrdinal(@"url"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"url")), typeof(string))
                            : null;

                        row.RawMarkdown = fieldNames.Contains("raw_markdown") && !reader.IsDBNull(reader.GetOrdinal(@"raw_markdown"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"raw_markdown")), typeof(string))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public async Task<List<DocPage>> FnGetDocPageAsOfAsync(string SemanticUid, DateTime? AsOfUtc)
    {
        var result = new List<DocPage>();
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
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(AsOfUtcParameter);
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        DocPage row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("source_snapshot_id") && !reader.IsDBNull(reader.GetOrdinal(@"source_snapshot_id")))
                        {
                            row.SourceSnapshotId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_snapshot_id")), typeof(Guid));
                        }

                        row.SourcePath = fieldNames.Contains("source_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_path")), typeof(string))
                            : null;

                        row.Title = fieldNames.Contains("title") && !reader.IsDBNull(reader.GetOrdinal(@"title"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"title")), typeof(string))
                            : null;

                        row.Language = fieldNames.Contains("language") && !reader.IsDBNull(reader.GetOrdinal(@"language"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"language")), typeof(string))
                            : null;

                        row.Url = fieldNames.Contains("url") && !reader.IsDBNull(reader.GetOrdinal(@"url"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"url")), typeof(string))
                            : null;

                        row.RawMarkdown = fieldNames.Contains("raw_markdown") && !reader.IsDBNull(reader.GetOrdinal(@"raw_markdown"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"raw_markdown")), typeof(string))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public List<ApiFeature> FnGetFeatureAsOf(string SemanticUid, DateTime? AsOfUtc)
    {
        var result = new List<ApiFeature>();
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
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(AsOfUtcParameter);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiFeature row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        row.ApiTypeId = fieldNames.Contains("api_type_id") && !reader.IsDBNull(reader.GetOrdinal(@"api_type_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"api_type_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("truth_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"truth_run_id")))
                        {
                            row.TruthRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"truth_run_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.Language = fieldNames.Contains("language") && !reader.IsDBNull(reader.GetOrdinal(@"language"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"language")), typeof(string))
                            : null;

                        row.Description = fieldNames.Contains("description") && !reader.IsDBNull(reader.GetOrdinal(@"description"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"description")), typeof(string))
                            : null;

                        row.Tags = fieldNames.Contains("tags") && !reader.IsDBNull(reader.GetOrdinal(@"tags"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"tags")), typeof(string))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public async Task<List<ApiFeature>> FnGetFeatureAsOfAsync(string SemanticUid, DateTime? AsOfUtc)
    {
        var result = new List<ApiFeature>();
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
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(AsOfUtcParameter);
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiFeature row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        row.ApiTypeId = fieldNames.Contains("api_type_id") && !reader.IsDBNull(reader.GetOrdinal(@"api_type_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"api_type_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("truth_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"truth_run_id")))
                        {
                            row.TruthRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"truth_run_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.Language = fieldNames.Contains("language") && !reader.IsDBNull(reader.GetOrdinal(@"language"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"language")), typeof(string))
                            : null;

                        row.Description = fieldNames.Contains("description") && !reader.IsDBNull(reader.GetOrdinal(@"description"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"description")), typeof(string))
                            : null;

                        row.Tags = fieldNames.Contains("tags") && !reader.IsDBNull(reader.GetOrdinal(@"tags"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"tags")), typeof(string))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public List<ApiMember> FnGetMemberAsOf(string SemanticUid, DateTime? AsOfUtc)
    {
        var result = new List<ApiMember>();
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
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(AsOfUtcParameter);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiMember row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("api_feature_id") && !reader.IsDBNull(reader.GetOrdinal(@"api_feature_id")))
                        {
                            row.ApiFeatureId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"api_feature_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.Kind = fieldNames.Contains("kind") && !reader.IsDBNull(reader.GetOrdinal(@"kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"kind")), typeof(string))
                            : null;

                        row.MethodKind = fieldNames.Contains("method_kind") && !reader.IsDBNull(reader.GetOrdinal(@"method_kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"method_kind")), typeof(string))
                            : null;

                        row.Accessibility = fieldNames.Contains("accessibility") && !reader.IsDBNull(reader.GetOrdinal(@"accessibility"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"accessibility")), typeof(string))
                            : null;

                        row.IsStatic = fieldNames.Contains("is_static") && !reader.IsDBNull(reader.GetOrdinal(@"is_static"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_static")), typeof(bool))
                            : null;

                        row.IsExtensionMethod = fieldNames.Contains("is_extension_method") && !reader.IsDBNull(reader.GetOrdinal(@"is_extension_method"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_extension_method")), typeof(bool))
                            : null;

                        row.IsAsync = fieldNames.Contains("is_async") && !reader.IsDBNull(reader.GetOrdinal(@"is_async"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_async")), typeof(bool))
                            : null;

                        row.IsVirtual = fieldNames.Contains("is_virtual") && !reader.IsDBNull(reader.GetOrdinal(@"is_virtual"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_virtual")), typeof(bool))
                            : null;

                        row.IsOverride = fieldNames.Contains("is_override") && !reader.IsDBNull(reader.GetOrdinal(@"is_override"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_override")), typeof(bool))
                            : null;

                        row.IsAbstract = fieldNames.Contains("is_abstract") && !reader.IsDBNull(reader.GetOrdinal(@"is_abstract"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_abstract")), typeof(bool))
                            : null;

                        row.IsSealed = fieldNames.Contains("is_sealed") && !reader.IsDBNull(reader.GetOrdinal(@"is_sealed"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_sealed")), typeof(bool))
                            : null;

                        row.IsReadonly = fieldNames.Contains("is_readonly") && !reader.IsDBNull(reader.GetOrdinal(@"is_readonly"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_readonly")), typeof(bool))
                            : null;

                        row.IsConst = fieldNames.Contains("is_const") && !reader.IsDBNull(reader.GetOrdinal(@"is_const"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_const")), typeof(bool))
                            : null;

                        row.IsUnsafe = fieldNames.Contains("is_unsafe") && !reader.IsDBNull(reader.GetOrdinal(@"is_unsafe"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_unsafe")), typeof(bool))
                            : null;

                        row.ReturnTypeUid = fieldNames.Contains("return_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"return_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"return_type_uid")), typeof(string))
                            : null;

                        row.ReturnNullable = fieldNames.Contains("return_nullable") && !reader.IsDBNull(reader.GetOrdinal(@"return_nullable"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"return_nullable")), typeof(string))
                            : null;

                        row.GenericParameters = fieldNames.Contains("generic_parameters") && !reader.IsDBNull(reader.GetOrdinal(@"generic_parameters"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_parameters")), typeof(string))
                            : null;

                        row.GenericConstraints = fieldNames.Contains("generic_constraints") && !reader.IsDBNull(reader.GetOrdinal(@"generic_constraints"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_constraints")), typeof(string))
                            : null;

                        row.Summary = fieldNames.Contains("summary") && !reader.IsDBNull(reader.GetOrdinal(@"summary"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"summary")), typeof(string))
                            : null;

                        row.Remarks = fieldNames.Contains("remarks") && !reader.IsDBNull(reader.GetOrdinal(@"remarks"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"remarks")), typeof(string))
                            : null;

                        row.Attributes = fieldNames.Contains("attributes") && !reader.IsDBNull(reader.GetOrdinal(@"attributes"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"attributes")), typeof(string))
                            : null;

                        row.SourceFilePath = fieldNames.Contains("source_file_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_file_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_file_path")), typeof(string))
                            : null;

                        row.SourceStartLine = fieldNames.Contains("source_start_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_start_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_start_line")), typeof(int))
                            : null;

                        row.SourceEndLine = fieldNames.Contains("source_end_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_end_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_end_line")), typeof(int))
                            : null;

                        row.MemberUidHash = fieldNames.Contains("member_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"member_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"member_uid_hash")), typeof(byte[]))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        row.UpdatedIngestionRunId = fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid))
                            : null;

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public async Task<List<ApiMember>> FnGetMemberAsOfAsync(string SemanticUid, DateTime? AsOfUtc)
    {
        var result = new List<ApiMember>();
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
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(AsOfUtcParameter);
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiMember row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("api_feature_id") && !reader.IsDBNull(reader.GetOrdinal(@"api_feature_id")))
                        {
                            row.ApiFeatureId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"api_feature_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.Kind = fieldNames.Contains("kind") && !reader.IsDBNull(reader.GetOrdinal(@"kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"kind")), typeof(string))
                            : null;

                        row.MethodKind = fieldNames.Contains("method_kind") && !reader.IsDBNull(reader.GetOrdinal(@"method_kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"method_kind")), typeof(string))
                            : null;

                        row.Accessibility = fieldNames.Contains("accessibility") && !reader.IsDBNull(reader.GetOrdinal(@"accessibility"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"accessibility")), typeof(string))
                            : null;

                        row.IsStatic = fieldNames.Contains("is_static") && !reader.IsDBNull(reader.GetOrdinal(@"is_static"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_static")), typeof(bool))
                            : null;

                        row.IsExtensionMethod = fieldNames.Contains("is_extension_method") && !reader.IsDBNull(reader.GetOrdinal(@"is_extension_method"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_extension_method")), typeof(bool))
                            : null;

                        row.IsAsync = fieldNames.Contains("is_async") && !reader.IsDBNull(reader.GetOrdinal(@"is_async"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_async")), typeof(bool))
                            : null;

                        row.IsVirtual = fieldNames.Contains("is_virtual") && !reader.IsDBNull(reader.GetOrdinal(@"is_virtual"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_virtual")), typeof(bool))
                            : null;

                        row.IsOverride = fieldNames.Contains("is_override") && !reader.IsDBNull(reader.GetOrdinal(@"is_override"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_override")), typeof(bool))
                            : null;

                        row.IsAbstract = fieldNames.Contains("is_abstract") && !reader.IsDBNull(reader.GetOrdinal(@"is_abstract"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_abstract")), typeof(bool))
                            : null;

                        row.IsSealed = fieldNames.Contains("is_sealed") && !reader.IsDBNull(reader.GetOrdinal(@"is_sealed"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_sealed")), typeof(bool))
                            : null;

                        row.IsReadonly = fieldNames.Contains("is_readonly") && !reader.IsDBNull(reader.GetOrdinal(@"is_readonly"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_readonly")), typeof(bool))
                            : null;

                        row.IsConst = fieldNames.Contains("is_const") && !reader.IsDBNull(reader.GetOrdinal(@"is_const"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_const")), typeof(bool))
                            : null;

                        row.IsUnsafe = fieldNames.Contains("is_unsafe") && !reader.IsDBNull(reader.GetOrdinal(@"is_unsafe"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_unsafe")), typeof(bool))
                            : null;

                        row.ReturnTypeUid = fieldNames.Contains("return_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"return_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"return_type_uid")), typeof(string))
                            : null;

                        row.ReturnNullable = fieldNames.Contains("return_nullable") && !reader.IsDBNull(reader.GetOrdinal(@"return_nullable"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"return_nullable")), typeof(string))
                            : null;

                        row.GenericParameters = fieldNames.Contains("generic_parameters") && !reader.IsDBNull(reader.GetOrdinal(@"generic_parameters"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_parameters")), typeof(string))
                            : null;

                        row.GenericConstraints = fieldNames.Contains("generic_constraints") && !reader.IsDBNull(reader.GetOrdinal(@"generic_constraints"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_constraints")), typeof(string))
                            : null;

                        row.Summary = fieldNames.Contains("summary") && !reader.IsDBNull(reader.GetOrdinal(@"summary"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"summary")), typeof(string))
                            : null;

                        row.Remarks = fieldNames.Contains("remarks") && !reader.IsDBNull(reader.GetOrdinal(@"remarks"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"remarks")), typeof(string))
                            : null;

                        row.Attributes = fieldNames.Contains("attributes") && !reader.IsDBNull(reader.GetOrdinal(@"attributes"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"attributes")), typeof(string))
                            : null;

                        row.SourceFilePath = fieldNames.Contains("source_file_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_file_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_file_path")), typeof(string))
                            : null;

                        row.SourceStartLine = fieldNames.Contains("source_start_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_start_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_start_line")), typeof(int))
                            : null;

                        row.SourceEndLine = fieldNames.Contains("source_end_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_end_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_end_line")), typeof(int))
                            : null;

                        row.MemberUidHash = fieldNames.Contains("member_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"member_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"member_uid_hash")), typeof(byte[]))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        row.UpdatedIngestionRunId = fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid))
                            : null;

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public List<ApiType> FnGetTypeAsOf(string SemanticUid, DateTime? AsOfUtc)
    {
        var result = new List<ApiType>();
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
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(AsOfUtcParameter);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiType row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("source_snapshot_id") && !reader.IsDBNull(reader.GetOrdinal(@"source_snapshot_id")))
                        {
                            row.SourceSnapshotId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_snapshot_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.NamespacePath = fieldNames.Contains("namespace_path") && !reader.IsDBNull(reader.GetOrdinal(@"namespace_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"namespace_path")), typeof(string))
                            : null;

                        row.Kind = fieldNames.Contains("kind") && !reader.IsDBNull(reader.GetOrdinal(@"kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"kind")), typeof(string))
                            : null;

                        row.Accessibility = fieldNames.Contains("accessibility") && !reader.IsDBNull(reader.GetOrdinal(@"accessibility"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"accessibility")), typeof(string))
                            : null;

                        row.IsStatic = fieldNames.Contains("is_static") && !reader.IsDBNull(reader.GetOrdinal(@"is_static"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_static")), typeof(bool))
                            : null;

                        row.IsGeneric = fieldNames.Contains("is_generic") && !reader.IsDBNull(reader.GetOrdinal(@"is_generic"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_generic")), typeof(bool))
                            : null;

                        row.IsAbstract = fieldNames.Contains("is_abstract") && !reader.IsDBNull(reader.GetOrdinal(@"is_abstract"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_abstract")), typeof(bool))
                            : null;

                        row.IsSealed = fieldNames.Contains("is_sealed") && !reader.IsDBNull(reader.GetOrdinal(@"is_sealed"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_sealed")), typeof(bool))
                            : null;

                        row.IsRecord = fieldNames.Contains("is_record") && !reader.IsDBNull(reader.GetOrdinal(@"is_record"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_record")), typeof(bool))
                            : null;

                        row.IsRefLike = fieldNames.Contains("is_ref_like") && !reader.IsDBNull(reader.GetOrdinal(@"is_ref_like"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_ref_like")), typeof(bool))
                            : null;

                        row.BaseTypeUid = fieldNames.Contains("base_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"base_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"base_type_uid")), typeof(string))
                            : null;

                        row.Interfaces = fieldNames.Contains("interfaces") && !reader.IsDBNull(reader.GetOrdinal(@"interfaces"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"interfaces")), typeof(string))
                            : null;

                        row.ContainingTypeUid = fieldNames.Contains("containing_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"containing_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"containing_type_uid")), typeof(string))
                            : null;

                        row.GenericParameters = fieldNames.Contains("generic_parameters") && !reader.IsDBNull(reader.GetOrdinal(@"generic_parameters"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_parameters")), typeof(string))
                            : null;

                        row.GenericConstraints = fieldNames.Contains("generic_constraints") && !reader.IsDBNull(reader.GetOrdinal(@"generic_constraints"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_constraints")), typeof(string))
                            : null;

                        row.Summary = fieldNames.Contains("summary") && !reader.IsDBNull(reader.GetOrdinal(@"summary"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"summary")), typeof(string))
                            : null;

                        row.Remarks = fieldNames.Contains("remarks") && !reader.IsDBNull(reader.GetOrdinal(@"remarks"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"remarks")), typeof(string))
                            : null;

                        row.Attributes = fieldNames.Contains("attributes") && !reader.IsDBNull(reader.GetOrdinal(@"attributes"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"attributes")), typeof(string))
                            : null;

                        row.SourceFilePath = fieldNames.Contains("source_file_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_file_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_file_path")), typeof(string))
                            : null;

                        row.SourceStartLine = fieldNames.Contains("source_start_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_start_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_start_line")), typeof(int))
                            : null;

                        row.SourceEndLine = fieldNames.Contains("source_end_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_end_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_end_line")), typeof(int))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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








    public async Task<List<ApiType>> FnGetTypeAsOfAsync(string SemanticUid, DateTime? AsOfUtc)
    {
        var result = new List<ApiType>();
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
                SemanticUidParameter.Value = SemanticUid != null ? SemanticUid : DBNull.Value;

                _ = cmd.Parameters.Add(SemanticUidParameter);

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

                _ = cmd.Parameters.Add(AsOfUtcParameter);
                using (IDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        ApiType row = new();
                        if (fieldNames.Contains("id") && !reader.IsDBNull(reader.GetOrdinal(@"id")))
                        {
                            row.Id = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("semantic_uid") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid")))
                        {
                            row.SemanticUid = (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid")), typeof(string));
                        }

                        if (fieldNames.Contains("source_snapshot_id") && !reader.IsDBNull(reader.GetOrdinal(@"source_snapshot_id")))
                        {
                            row.SourceSnapshotId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_snapshot_id")), typeof(Guid));
                        }

                        row.Name = fieldNames.Contains("name") && !reader.IsDBNull(reader.GetOrdinal(@"name"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"name")), typeof(string))
                            : null;

                        row.NamespacePath = fieldNames.Contains("namespace_path") && !reader.IsDBNull(reader.GetOrdinal(@"namespace_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"namespace_path")), typeof(string))
                            : null;

                        row.Kind = fieldNames.Contains("kind") && !reader.IsDBNull(reader.GetOrdinal(@"kind"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"kind")), typeof(string))
                            : null;

                        row.Accessibility = fieldNames.Contains("accessibility") && !reader.IsDBNull(reader.GetOrdinal(@"accessibility"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"accessibility")), typeof(string))
                            : null;

                        row.IsStatic = fieldNames.Contains("is_static") && !reader.IsDBNull(reader.GetOrdinal(@"is_static"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_static")), typeof(bool))
                            : null;

                        row.IsGeneric = fieldNames.Contains("is_generic") && !reader.IsDBNull(reader.GetOrdinal(@"is_generic"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_generic")), typeof(bool))
                            : null;

                        row.IsAbstract = fieldNames.Contains("is_abstract") && !reader.IsDBNull(reader.GetOrdinal(@"is_abstract"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_abstract")), typeof(bool))
                            : null;

                        row.IsSealed = fieldNames.Contains("is_sealed") && !reader.IsDBNull(reader.GetOrdinal(@"is_sealed"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_sealed")), typeof(bool))
                            : null;

                        row.IsRecord = fieldNames.Contains("is_record") && !reader.IsDBNull(reader.GetOrdinal(@"is_record"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_record")), typeof(bool))
                            : null;

                        row.IsRefLike = fieldNames.Contains("is_ref_like") && !reader.IsDBNull(reader.GetOrdinal(@"is_ref_like"))
                            ? (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_ref_like")), typeof(bool))
                            : null;

                        row.BaseTypeUid = fieldNames.Contains("base_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"base_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"base_type_uid")), typeof(string))
                            : null;

                        row.Interfaces = fieldNames.Contains("interfaces") && !reader.IsDBNull(reader.GetOrdinal(@"interfaces"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"interfaces")), typeof(string))
                            : null;

                        row.ContainingTypeUid = fieldNames.Contains("containing_type_uid") && !reader.IsDBNull(reader.GetOrdinal(@"containing_type_uid"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"containing_type_uid")), typeof(string))
                            : null;

                        row.GenericParameters = fieldNames.Contains("generic_parameters") && !reader.IsDBNull(reader.GetOrdinal(@"generic_parameters"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_parameters")), typeof(string))
                            : null;

                        row.GenericConstraints = fieldNames.Contains("generic_constraints") && !reader.IsDBNull(reader.GetOrdinal(@"generic_constraints"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"generic_constraints")), typeof(string))
                            : null;

                        row.Summary = fieldNames.Contains("summary") && !reader.IsDBNull(reader.GetOrdinal(@"summary"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"summary")), typeof(string))
                            : null;

                        row.Remarks = fieldNames.Contains("remarks") && !reader.IsDBNull(reader.GetOrdinal(@"remarks"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"remarks")), typeof(string))
                            : null;

                        row.Attributes = fieldNames.Contains("attributes") && !reader.IsDBNull(reader.GetOrdinal(@"attributes"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"attributes")), typeof(string))
                            : null;

                        row.SourceFilePath = fieldNames.Contains("source_file_path") && !reader.IsDBNull(reader.GetOrdinal(@"source_file_path"))
                            ? (string)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_file_path")), typeof(string))
                            : null;

                        row.SourceStartLine = fieldNames.Contains("source_start_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_start_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_start_line")), typeof(int))
                            : null;

                        row.SourceEndLine = fieldNames.Contains("source_end_line") && !reader.IsDBNull(reader.GetOrdinal(@"source_end_line"))
                            ? (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"source_end_line")), typeof(int))
                            : null;

                        if (fieldNames.Contains("version_number") && !reader.IsDBNull(reader.GetOrdinal(@"version_number")))
                        {
                            row.VersionNumber = (int)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"version_number")), typeof(int));
                        }

                        if (fieldNames.Contains("created_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"created_ingestion_run_id")))
                        {
                            row.CreatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"created_ingestion_run_id")), typeof(Guid));
                        }

                        if (fieldNames.Contains("updated_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"updated_ingestion_run_id")))
                        {
                            row.UpdatedIngestionRunId = (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"updated_ingestion_run_id")), typeof(Guid));
                        }

                        row.RemovedIngestionRunId = fieldNames.Contains("removed_ingestion_run_id") && !reader.IsDBNull(reader.GetOrdinal(@"removed_ingestion_run_id"))
                            ? (Guid)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"removed_ingestion_run_id")), typeof(Guid))
                            : null;

                        if (fieldNames.Contains("valid_from_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_from_utc")))
                        {
                            row.ValidFromUtc = (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_from_utc")), typeof(DateTime));
                        }

                        row.ValidToUtc = fieldNames.Contains("valid_to_utc") && !reader.IsDBNull(reader.GetOrdinal(@"valid_to_utc"))
                            ? (DateTime)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"valid_to_utc")), typeof(DateTime))
                            : null;

                        if (fieldNames.Contains("is_active") && !reader.IsDBNull(reader.GetOrdinal(@"is_active")))
                        {
                            row.IsActive = (bool)Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"is_active")), typeof(bool));
                        }

                        row.ContentHash = fieldNames.Contains("content_hash") && !reader.IsDBNull(reader.GetOrdinal(@"content_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"content_hash")), typeof(byte[]))
                            : null;

                        row.SemanticUidHash = fieldNames.Contains("semantic_uid_hash") && !reader.IsDBNull(reader.GetOrdinal(@"semantic_uid_hash"))
                            ? (byte[])Convert.ChangeType(reader.GetValue(reader.GetOrdinal(@"semantic_uid_hash")), typeof(byte[]))
                            : null;

                        result.Add(row);
                    }
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

    #endregion



    #region ApiFeature Mapping

    private void ApiFeatureMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<ApiFeature>().ToTable(@"api_feature", @"dbo");
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.ApiTypeId).HasColumnName(@"api_type_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.SemanticUid).HasColumnName(@"semantic_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.TruthRunId).HasColumnName(@"truth_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.Name).HasColumnName(@"name").HasColumnType(@"nvarchar(400)").ValueGeneratedNever().HasMaxLength(400);
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.Language).HasColumnName(@"language").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.Description).HasColumnName(@"description").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.Tags).HasColumnName(@"tags").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.VersionNumber).HasColumnName(@"version_number").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"created_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"updated_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"removed_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.ValidFromUtc).HasColumnName(@"valid_from_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.ValidToUtc).HasColumnName(@"valid_to_utc").HasColumnType(@"datetime2").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.IsActive).HasColumnName(@"is_active").HasColumnType(@"bit").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.ContentHash).HasColumnName(@"content_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<ApiFeature>().Property(x => x.SemanticUidHash).HasColumnName(@"semantic_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        _ = modelBuilder.Entity<ApiFeature>().HasKey(@"Id");
        _ = modelBuilder.Entity<ApiFeature>().HasIndex(@"SemanticUid", @"VersionNumber").IsUnique().HasDatabaseName(@"uq_api_feature_semantic_version");
    }








    partial void CustomizeApiFeatureMapping(ModelBuilder modelBuilder);

    #endregion



    #region ApiMember Mapping

    private void ApiMemberMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<ApiMember>().ToTable(@"api_member", @"dbo");
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.SemanticUid).HasColumnName(@"semantic_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.ApiFeatureId).HasColumnName(@"api_feature_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.Name).HasColumnName(@"name").HasColumnType(@"nvarchar(400)").ValueGeneratedNever().HasMaxLength(400);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.Kind).HasColumnName(@"kind").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.MethodKind).HasColumnName(@"method_kind").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.Accessibility).HasColumnName(@"accessibility").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.IsStatic).HasColumnName(@"is_static").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.IsExtensionMethod).HasColumnName(@"is_extension_method").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.IsAsync).HasColumnName(@"is_async").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.IsVirtual).HasColumnName(@"is_virtual").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.IsOverride).HasColumnName(@"is_override").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.IsAbstract).HasColumnName(@"is_abstract").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.IsSealed).HasColumnName(@"is_sealed").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.IsReadonly).HasColumnName(@"is_readonly").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.IsConst).HasColumnName(@"is_const").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.IsUnsafe).HasColumnName(@"is_unsafe").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.ReturnTypeUid).HasColumnName(@"return_type_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.ReturnNullable).HasColumnName(@"return_nullable").HasColumnType(@"nvarchar(50)").ValueGeneratedNever().HasMaxLength(50);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.GenericParameters).HasColumnName(@"generic_parameters").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.GenericConstraints).HasColumnName(@"generic_constraints").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.Summary).HasColumnName(@"summary").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.Remarks).HasColumnName(@"remarks").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.Attributes).HasColumnName(@"attributes").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.SourceFilePath).HasColumnName(@"source_file_path").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.SourceStartLine).HasColumnName(@"source_start_line").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.SourceEndLine).HasColumnName(@"source_end_line").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.MemberUidHash).HasColumnName(@"member_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.VersionNumber).HasColumnName(@"version_number").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"created_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"updated_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"removed_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.ValidFromUtc).HasColumnName(@"valid_from_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.ValidToUtc).HasColumnName(@"valid_to_utc").HasColumnType(@"datetime2").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.IsActive).HasColumnName(@"is_active").HasColumnType(@"bit").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.ContentHash).HasColumnName(@"content_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<ApiMember>().Property(x => x.SemanticUidHash).HasColumnName(@"semantic_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        _ = modelBuilder.Entity<ApiMember>().HasKey(@"Id");
        _ = modelBuilder.Entity<ApiMember>().HasIndex(@"ApiFeatureId", @"MemberUidHash").IsUnique().HasDatabaseName(@"ix_api_member_type_hash");
        _ = modelBuilder.Entity<ApiMember>().HasIndex(@"VersionNumber", @"SemanticUidHash").IsUnique().HasDatabaseName(@"uq_api_member_semantic_version");
    }








    partial void CustomizeApiMemberMapping(ModelBuilder modelBuilder);

    #endregion



    #region ApiMemberDiff Mapping

    private void ApiMemberDiffMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<ApiMemberDiff>().ToTable(@"api_member_diff", @"dbo");
        _ = modelBuilder.Entity<ApiMemberDiff>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<ApiMemberDiff>().Property(x => x.SnapshotDiffId).HasColumnName(@"snapshot_diff_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMemberDiff>().Property(x => x.MemberUid).HasColumnName(@"member_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ApiMemberDiff>().Property(x => x.ChangeKind).HasColumnName(@"change_kind").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ApiMemberDiff>().Property(x => x.OldSignature).HasColumnName(@"old_signature").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMemberDiff>().Property(x => x.NewSignature).HasColumnName(@"new_signature").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMemberDiff>().Property(x => x.Breaking).HasColumnName(@"breaking").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMemberDiff>().Property(x => x.DetailJson).HasColumnName(@"detail_json").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiMemberDiff>().HasKey(@"Id");
    }








    partial void CustomizeApiMemberDiffMapping(ModelBuilder modelBuilder);

    #endregion



    #region ApiParameter Mapping

    private void ApiParameterMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<ApiParameter>().ToTable(@"api_parameter", @"dbo");
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.ApiMemberId).HasColumnName(@"api_member_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.Name).HasColumnName(@"name").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.TypeUid).HasColumnName(@"type_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.NullableAnnotation).HasColumnName(@"nullable_annotation").HasColumnType(@"nvarchar(50)").ValueGeneratedNever().HasMaxLength(50);
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.Position).HasColumnName(@"position").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.Modifier).HasColumnName(@"modifier").HasColumnType(@"nvarchar(50)").ValueGeneratedNever().HasMaxLength(50);
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.HasDefaultValue).HasColumnName(@"has_default_value").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.DefaultValueLiteral).HasColumnName(@"default_value_literal").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.VersionNumber).HasColumnName(@"version_number").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"created_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"updated_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"removed_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.ValidFromUtc).HasColumnName(@"valid_from_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.ValidToUtc).HasColumnName(@"valid_to_utc").HasColumnType(@"datetime2").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.IsActive).HasColumnName(@"is_active").HasColumnType(@"bit").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.ContentHash).HasColumnName(@"content_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<ApiParameter>().Property(x => x.SemanticUidHash).HasColumnName(@"semantic_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<ApiParameter>().HasKey(@"Id");
        _ = modelBuilder.Entity<ApiParameter>().HasIndex(@"ApiMemberId", @"Position", @"VersionNumber").IsUnique().HasDatabaseName(@"uq_api_parameter_member_position_version");
    }








    partial void CustomizeApiParameterMapping(ModelBuilder modelBuilder);

    #endregion



    #region ApiType Mapping

    private void ApiTypeMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<ApiType>().ToTable(@"api_type", @"dbo");
        _ = modelBuilder.Entity<ApiType>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<ApiType>().Property(x => x.SemanticUid).HasColumnName(@"semantic_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ApiType>().Property(x => x.SourceSnapshotId).HasColumnName(@"source_snapshot_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.Name).HasColumnName(@"name").HasColumnType(@"nvarchar(400)").ValueGeneratedNever().HasMaxLength(400);
        _ = modelBuilder.Entity<ApiType>().Property(x => x.NamespacePath).HasColumnName(@"namespace_path").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ApiType>().Property(x => x.Kind).HasColumnName(@"kind").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ApiType>().Property(x => x.Accessibility).HasColumnName(@"accessibility").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ApiType>().Property(x => x.IsStatic).HasColumnName(@"is_static").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.IsGeneric).HasColumnName(@"is_generic").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.IsAbstract).HasColumnName(@"is_abstract").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.IsSealed).HasColumnName(@"is_sealed").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.IsRecord).HasColumnName(@"is_record").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.IsRefLike).HasColumnName(@"is_ref_like").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.BaseTypeUid).HasColumnName(@"base_type_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ApiType>().Property(x => x.Interfaces).HasColumnName(@"interfaces").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.ContainingTypeUid).HasColumnName(@"containing_type_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ApiType>().Property(x => x.GenericParameters).HasColumnName(@"generic_parameters").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.GenericConstraints).HasColumnName(@"generic_constraints").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.Summary).HasColumnName(@"summary").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.Remarks).HasColumnName(@"remarks").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.Attributes).HasColumnName(@"attributes").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.SourceFilePath).HasColumnName(@"source_file_path").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.SourceStartLine).HasColumnName(@"source_start_line").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<ApiType>().Property(x => x.SourceEndLine).HasColumnName(@"source_end_line").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<ApiType>().Property(x => x.VersionNumber).HasColumnName(@"version_number").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<ApiType>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"created_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"updated_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"removed_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.ValidFromUtc).HasColumnName(@"valid_from_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.ValidToUtc).HasColumnName(@"valid_to_utc").HasColumnType(@"datetime2").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiType>().Property(x => x.IsActive).HasColumnName(@"is_active").HasColumnType(@"bit").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        _ = modelBuilder.Entity<ApiType>().Property(x => x.ContentHash).HasColumnName(@"content_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<ApiType>().Property(x => x.SemanticUidHash).HasColumnName(@"semantic_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        _ = modelBuilder.Entity<ApiType>().HasKey(@"Id");
        _ = modelBuilder.Entity<ApiType>().HasIndex(@"VersionNumber", @"SemanticUidHash").IsUnique().HasDatabaseName(@"uq_api_type_semantic_version");
    }








    partial void CustomizeApiTypeMapping(ModelBuilder modelBuilder);

    #endregion



    #region ApiTypeDiff Mapping

    private void ApiTypeDiffMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<ApiTypeDiff>().ToTable(@"api_type_diff", @"dbo");
        _ = modelBuilder.Entity<ApiTypeDiff>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<ApiTypeDiff>().Property(x => x.SnapshotDiffId).HasColumnName(@"snapshot_diff_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiTypeDiff>().Property(x => x.TypeUid).HasColumnName(@"type_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ApiTypeDiff>().Property(x => x.ChangeKind).HasColumnName(@"change_kind").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ApiTypeDiff>().Property(x => x.DetailJson).HasColumnName(@"detail_json").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ApiTypeDiff>().HasKey(@"Id");
    }








    partial void CustomizeApiTypeDiffMapping(ModelBuilder modelBuilder);

    #endregion



    #region CodeBlock Mapping

    private void CodeBlockMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<CodeBlock>().ToTable(@"code_block", @"dbo");
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.DocSectionId).HasColumnName(@"doc_section_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.SemanticUid).HasColumnName(@"semantic_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.Language).HasColumnName(@"language").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.Content).HasColumnName(@"content").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.DeclaredPackages).HasColumnName(@"declared_packages").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.Tags).HasColumnName(@"tags").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.InlineComments).HasColumnName(@"inline_comments").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.VersionNumber).HasColumnName(@"version_number").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"created_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"updated_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"removed_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.ValidFromUtc).HasColumnName(@"valid_from_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.ValidToUtc).HasColumnName(@"valid_to_utc").HasColumnType(@"datetime2").ValueGeneratedNever();
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.IsActive).HasColumnName(@"is_active").HasColumnType(@"bit").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        _ = modelBuilder.Entity<CodeBlock>().Property(x => x.ContentHash).HasColumnName(@"content_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<CodeBlock>().HasKey(@"Id");
        _ = modelBuilder.Entity<CodeBlock>().HasIndex(@"SemanticUid", @"VersionNumber").IsUnique().HasDatabaseName(@"ix_code_block_semantic_version");
    }








    partial void CustomizeCodeBlockMapping(ModelBuilder modelBuilder);

    #endregion



    #region DocPage Mapping

    private void DocPageMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<DocPage>().ToTable(@"doc_page", @"dbo");
        _ = modelBuilder.Entity<DocPage>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<DocPage>().Property(x => x.SemanticUid).HasColumnName(@"semantic_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<DocPage>().Property(x => x.SourceSnapshotId).HasColumnName(@"source_snapshot_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<DocPage>().Property(x => x.SourcePath).HasColumnName(@"source_path").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<DocPage>().Property(x => x.Title).HasColumnName(@"title").HasColumnType(@"nvarchar(400)").ValueGeneratedNever().HasMaxLength(400);
        _ = modelBuilder.Entity<DocPage>().Property(x => x.Language).HasColumnName(@"language").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<DocPage>().Property(x => x.Url).HasColumnName(@"url").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<DocPage>().Property(x => x.RawMarkdown).HasColumnName(@"raw_markdown").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<DocPage>().Property(x => x.VersionNumber).HasColumnName(@"version_number").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<DocPage>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"created_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<DocPage>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"updated_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<DocPage>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"removed_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<DocPage>().Property(x => x.ValidFromUtc).HasColumnName(@"valid_from_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<DocPage>().Property(x => x.ValidToUtc).HasColumnName(@"valid_to_utc").HasColumnType(@"datetime2").ValueGeneratedNever();
        _ = modelBuilder.Entity<DocPage>().Property(x => x.IsActive).HasColumnName(@"is_active").HasColumnType(@"bit").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        _ = modelBuilder.Entity<DocPage>().Property(x => x.ContentHash).HasColumnName(@"content_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<DocPage>().Property(x => x.SemanticUidHash).HasColumnName(@"semantic_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        _ = modelBuilder.Entity<DocPage>().HasKey(@"Id");
        _ = modelBuilder.Entity<DocPage>().HasIndex(@"SemanticUid", @"VersionNumber").IsUnique().HasDatabaseName(@"uq_doc_page_semantic_version");
    }








    partial void CustomizeDocPageMapping(ModelBuilder modelBuilder);

    #endregion



    #region DocPageDiff Mapping

    private void DocPageDiffMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<DocPageDiff>().ToTable(@"doc_page_diff", @"dbo");
        _ = modelBuilder.Entity<DocPageDiff>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<DocPageDiff>().Property(x => x.SnapshotDiffId).HasColumnName(@"snapshot_diff_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<DocPageDiff>().Property(x => x.DocUid).HasColumnName(@"doc_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<DocPageDiff>().Property(x => x.ChangeKind).HasColumnName(@"change_kind").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<DocPageDiff>().Property(x => x.DetailJson).HasColumnName(@"detail_json").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<DocPageDiff>().HasKey(@"Id");
    }








    partial void CustomizeDocPageDiffMapping(ModelBuilder modelBuilder);

    #endregion



    #region DocSection Mapping

    private void DocSectionMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<DocSection>().ToTable(@"doc_section", @"dbo");
        _ = modelBuilder.Entity<DocSection>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<DocSection>().Property(x => x.DocPageId).HasColumnName(@"doc_page_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<DocSection>().Property(x => x.SemanticUid).HasColumnName(@"semantic_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<DocSection>().Property(x => x.Heading).HasColumnName(@"heading").HasColumnType(@"nvarchar(400)").ValueGeneratedNever().HasMaxLength(400);
        _ = modelBuilder.Entity<DocSection>().Property(x => x.Level).HasColumnName(@"level").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<DocSection>().Property(x => x.ContentMarkdown).HasColumnName(@"content_markdown").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<DocSection>().Property(x => x.OrderIndex).HasColumnName(@"order_index").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<DocSection>().Property(x => x.VersionNumber).HasColumnName(@"version_number").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<DocSection>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"created_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<DocSection>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"updated_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<DocSection>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"removed_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<DocSection>().Property(x => x.ValidFromUtc).HasColumnName(@"valid_from_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<DocSection>().Property(x => x.ValidToUtc).HasColumnName(@"valid_to_utc").HasColumnType(@"datetime2").ValueGeneratedNever();
        _ = modelBuilder.Entity<DocSection>().Property(x => x.IsActive).HasColumnName(@"is_active").HasColumnType(@"bit").IsRequired().ValueGeneratedNever().HasDefaultValueSql(@"1");
        _ = modelBuilder.Entity<DocSection>().Property(x => x.ContentHash).HasColumnName(@"content_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<DocSection>().Property(x => x.SemanticUidHash).HasColumnName(@"semantic_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        _ = modelBuilder.Entity<DocSection>().HasKey(@"Id");
        _ = modelBuilder.Entity<DocSection>().HasIndex(@"SemanticUid", @"VersionNumber").IsUnique().HasDatabaseName(@"uq_doc_section_semantic_version");
    }








    partial void CustomizeDocSectionMapping(ModelBuilder modelBuilder);

    #endregion



    #region ExecutionResult Mapping

    private void ExecutionResultMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<ExecutionResult>().ToTable(@"execution_result", @"dbo");
        _ = modelBuilder.Entity<ExecutionResult>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<ExecutionResult>().Property(x => x.ExecutionRunId).HasColumnName(@"execution_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ExecutionResult>().Property(x => x.SampleUid).HasColumnName(@"sample_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ExecutionResult>().Property(x => x.Status).HasColumnName(@"status").HasColumnType(@"nvarchar(100)").ValueGeneratedNever().HasMaxLength(100);
        _ = modelBuilder.Entity<ExecutionResult>().Property(x => x.BuildLog).HasColumnName(@"build_log").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ExecutionResult>().Property(x => x.RunLog).HasColumnName(@"run_log").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ExecutionResult>().Property(x => x.ExceptionJson).HasColumnName(@"exception_json").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ExecutionResult>().Property(x => x.DurationMs).HasColumnName(@"duration_ms").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<ExecutionResult>().HasKey(@"Id");
    }








    partial void CustomizeExecutionResultMapping(ModelBuilder modelBuilder);

    #endregion



    #region ExecutionRun Mapping

    private void ExecutionRunMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<ExecutionRun>().ToTable(@"execution_run", @"dbo");
        _ = modelBuilder.Entity<ExecutionRun>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<ExecutionRun>().Property(x => x.SnapshotId).HasColumnName(@"snapshot_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ExecutionRun>().Property(x => x.SampleRunId).HasColumnName(@"sample_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ExecutionRun>().Property(x => x.TimestampUtc).HasColumnName(@"timestamp_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ExecutionRun>().Property(x => x.EnvironmentJson).HasColumnName(@"environment_json").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ExecutionRun>().Property(x => x.SchemaVersion).HasColumnName(@"schema_version").HasColumnType(@"nvarchar(200)").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ExecutionRun>().HasKey(@"Id");
    }








    partial void CustomizeExecutionRunMapping(ModelBuilder modelBuilder);

    #endregion



    #region FeatureDocLink Mapping

    private void FeatureDocLinkMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<FeatureDocLink>().ToTable(@"feature_doc_link", @"dbo");
        _ = modelBuilder.Entity<FeatureDocLink>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<FeatureDocLink>().Property(x => x.FeatureId).HasColumnName(@"feature_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<FeatureDocLink>().Property(x => x.DocUid).HasColumnName(@"doc_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<FeatureDocLink>().Property(x => x.SectionUid).HasColumnName(@"section_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<FeatureDocLink>().HasKey(@"Id");
    }








    partial void CustomizeFeatureDocLinkMapping(ModelBuilder modelBuilder);

    #endregion



    #region FeatureMemberLink Mapping

    private void FeatureMemberLinkMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<FeatureMemberLink>().ToTable(@"feature_member_link", @"dbo");
        _ = modelBuilder.Entity<FeatureMemberLink>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<FeatureMemberLink>().Property(x => x.FeatureId).HasColumnName(@"feature_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<FeatureMemberLink>().Property(x => x.MemberUid).HasColumnName(@"member_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<FeatureMemberLink>().Property(x => x.Role).HasColumnName(@"role").HasColumnType(@"nvarchar(50)").ValueGeneratedNever().HasMaxLength(50);
        _ = modelBuilder.Entity<FeatureMemberLink>().HasKey(@"Id");
    }








    partial void CustomizeFeatureMemberLinkMapping(ModelBuilder modelBuilder);

    #endregion



    #region FeatureTypeLink Mapping

    private void FeatureTypeLinkMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<FeatureTypeLink>().ToTable(@"feature_type_link", @"dbo");
        _ = modelBuilder.Entity<FeatureTypeLink>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<FeatureTypeLink>().Property(x => x.FeatureId).HasColumnName(@"feature_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<FeatureTypeLink>().Property(x => x.TypeUid).HasColumnName(@"type_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<FeatureTypeLink>().Property(x => x.Role).HasColumnName(@"role").HasColumnType(@"nvarchar(50)").ValueGeneratedNever().HasMaxLength(50);
        _ = modelBuilder.Entity<FeatureTypeLink>().HasKey(@"Id");
    }








    partial void CustomizeFeatureTypeLinkMapping(ModelBuilder modelBuilder);

    #endregion



    #region IngestionRun Mapping

    private void IngestionRunMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<IngestionRun>().ToTable(@"ingestion_run", @"dbo");
        _ = modelBuilder.Entity<IngestionRun>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<IngestionRun>().Property(x => x.TimestampUtc).HasColumnName(@"timestamp_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<IngestionRun>().Property(x => x.SchemaVersion).HasColumnName(@"schema_version").HasColumnType(@"nvarchar(200)").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<IngestionRun>().Property(x => x.Notes).HasColumnName(@"notes").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<IngestionRun>().HasKey(@"Id");
    }








    partial void CustomizeIngestionRunMapping(ModelBuilder modelBuilder);

    #endregion



    #region RagChunk Mapping

    private void RagChunkMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<RagChunk>().ToTable(@"rag_chunk", @"dbo");
        _ = modelBuilder.Entity<RagChunk>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<RagChunk>().Property(x => x.RagRunId).HasColumnName(@"rag_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<RagChunk>().Property(x => x.ChunkUid).HasColumnName(@"chunk_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<RagChunk>().Property(x => x.Kind).HasColumnName(@"kind").HasColumnType(@"nvarchar(100)").ValueGeneratedNever().HasMaxLength(100);
        _ = modelBuilder.Entity<RagChunk>().Property(x => x.Text).HasColumnName(@"text").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<RagChunk>().Property(x => x.MetadataJson).HasColumnName(@"metadata_json").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<RagChunk>().Property(x => x.EmbeddingVector).HasColumnName(@"embedding_vector").HasColumnType(@"varchar(1536)").ValueGeneratedNever().HasMaxLength(1536);
        _ = modelBuilder.Entity<RagChunk>().HasKey(@"Id");
        _ = modelBuilder.Entity<RagChunk>().HasIndex(@"ChunkUid").IsUnique();
    }








    partial void CustomizeRagChunkMapping(ModelBuilder modelBuilder);

    #endregion



    #region RagRun Mapping

    private void RagRunMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<RagRun>().ToTable(@"rag_run", @"dbo");
        _ = modelBuilder.Entity<RagRun>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<RagRun>().Property(x => x.SnapshotId).HasColumnName(@"snapshot_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<RagRun>().Property(x => x.TimestampUtc).HasColumnName(@"timestamp_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<RagRun>().Property(x => x.SchemaVersion).HasColumnName(@"schema_version").HasColumnType(@"nvarchar(200)").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<RagRun>().HasKey(@"Id");
    }








    partial void CustomizeRagRunMapping(ModelBuilder modelBuilder);

    #endregion



    #region ReviewIssue Mapping

    private void ReviewIssueMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<ReviewIssue>().ToTable(@"review_issue", @"dbo");
        _ = modelBuilder.Entity<ReviewIssue>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<ReviewIssue>().Property(x => x.ReviewItemId).HasColumnName(@"review_item_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ReviewIssue>().Property(x => x.Code).HasColumnName(@"code").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ReviewIssue>().Property(x => x.Severity).HasColumnName(@"severity").HasColumnType(@"nvarchar(50)").ValueGeneratedNever().HasMaxLength(50);
        _ = modelBuilder.Entity<ReviewIssue>().Property(x => x.RelatedMemberUid).HasColumnName(@"related_member_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ReviewIssue>().Property(x => x.Details).HasColumnName(@"details").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ReviewIssue>().HasKey(@"Id");
    }








    partial void CustomizeReviewIssueMapping(ModelBuilder modelBuilder);

    #endregion



    #region ReviewItem Mapping

    private void ReviewItemMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<ReviewItem>().ToTable(@"review_item", @"dbo");
        _ = modelBuilder.Entity<ReviewItem>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<ReviewItem>().Property(x => x.ReviewRunId).HasColumnName(@"review_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ReviewItem>().Property(x => x.TargetKind).HasColumnName(@"target_kind").HasColumnType(@"nvarchar(50)").IsRequired().ValueGeneratedNever().HasMaxLength(50);
        _ = modelBuilder.Entity<ReviewItem>().Property(x => x.TargetUid).HasColumnName(@"target_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<ReviewItem>().Property(x => x.Status).HasColumnName(@"status").HasColumnType(@"nvarchar(50)").ValueGeneratedNever().HasMaxLength(50);
        _ = modelBuilder.Entity<ReviewItem>().Property(x => x.Summary).HasColumnName(@"summary").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<ReviewItem>().HasKey(@"Id");
    }








    partial void CustomizeReviewItemMapping(ModelBuilder modelBuilder);

    #endregion



    #region ReviewRun Mapping

    private void ReviewRunMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<ReviewRun>().ToTable(@"review_run", @"dbo");
        _ = modelBuilder.Entity<ReviewRun>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<ReviewRun>().Property(x => x.SnapshotId).HasColumnName(@"snapshot_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ReviewRun>().Property(x => x.TimestampUtc).HasColumnName(@"timestamp_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<ReviewRun>().Property(x => x.SchemaVersion).HasColumnName(@"schema_version").HasColumnType(@"nvarchar(200)").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<ReviewRun>().HasKey(@"Id");
    }








    partial void CustomizeReviewRunMapping(ModelBuilder modelBuilder);

    #endregion



    #region Sample Mapping

    private void SampleMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Sample>().ToTable(@"sample", @"dbo");
        _ = modelBuilder.Entity<Sample>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<Sample>().Property(x => x.SampleRunId).HasColumnName(@"sample_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<Sample>().Property(x => x.SampleUid).HasColumnName(@"sample_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<Sample>().Property(x => x.FeatureUid).HasColumnName(@"feature_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<Sample>().Property(x => x.Language).HasColumnName(@"language").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<Sample>().Property(x => x.Code).HasColumnName(@"code").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<Sample>().Property(x => x.EntryPoint).HasColumnName(@"entry_point").HasColumnType(@"nvarchar(400)").ValueGeneratedNever().HasMaxLength(400);
        _ = modelBuilder.Entity<Sample>().Property(x => x.TargetFramework).HasColumnName(@"target_framework").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<Sample>().Property(x => x.PackageReferences).HasColumnName(@"package_references").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<Sample>().Property(x => x.DerivedFromCodeUid).HasColumnName(@"derived_from_code_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<Sample>().Property(x => x.Tags).HasColumnName(@"tags").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<Sample>().HasKey(@"Id");
        _ = modelBuilder.Entity<Sample>().HasIndex(@"SampleUid").IsUnique();
    }








    partial void CustomizeSampleMapping(ModelBuilder modelBuilder);

    #endregion



    #region SampleApiMemberLink Mapping

    private void SampleApiMemberLinkMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<SampleApiMemberLink>().ToTable(@"sample_api_member_link", @"dbo");
        _ = modelBuilder.Entity<SampleApiMemberLink>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<SampleApiMemberLink>().Property(x => x.SampleId).HasColumnName(@"sample_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<SampleApiMemberLink>().Property(x => x.MemberUid).HasColumnName(@"member_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<SampleApiMemberLink>().HasKey(@"Id");
    }








    partial void CustomizeSampleApiMemberLinkMapping(ModelBuilder modelBuilder);

    #endregion



    #region SampleRun Mapping

    private void SampleRunMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<SampleRun>().ToTable(@"sample_run", @"dbo");
        _ = modelBuilder.Entity<SampleRun>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<SampleRun>().Property(x => x.SnapshotId).HasColumnName(@"snapshot_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<SampleRun>().Property(x => x.TimestampUtc).HasColumnName(@"timestamp_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<SampleRun>().Property(x => x.SchemaVersion).HasColumnName(@"schema_version").HasColumnType(@"nvarchar(200)").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<SampleRun>().HasKey(@"Id");
    }








    partial void CustomizeSampleRunMapping(ModelBuilder modelBuilder);

    #endregion



    #region SemanticIdentity Mapping

    private void SemanticIdentityMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<SemanticIdentity>().ToTable(@"semantic_identity", @"dbo");
        _ = modelBuilder.Entity<SemanticIdentity>().Property(x => x.UidHash).HasColumnName(@"uid_hash").HasColumnType(@"binary(32)").IsRequired().ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<SemanticIdentity>().Property(x => x.Uid).HasColumnName(@"uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<SemanticIdentity>().Property(x => x.Kind).HasColumnName(@"kind").HasColumnType(@"nvarchar(50)").IsRequired().ValueGeneratedNever().HasMaxLength(50);
        _ = modelBuilder.Entity<SemanticIdentity>().Property(x => x.CreatedUtc).HasColumnName(@"created_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<SemanticIdentity>().Property(x => x.Notes).HasColumnName(@"notes").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<SemanticIdentity>().HasKey(@"UidHash");
    }








    partial void CustomizeSemanticIdentityMapping(ModelBuilder modelBuilder);

    #endregion



    #region SnapshotDiff Mapping

    private void SnapshotDiffMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<SnapshotDiff>().ToTable(@"snapshot_diff", @"dbo");
        _ = modelBuilder.Entity<SnapshotDiff>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<SnapshotDiff>().Property(x => x.OldSnapshotId).HasColumnName(@"old_snapshot_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<SnapshotDiff>().Property(x => x.NewSnapshotId).HasColumnName(@"new_snapshot_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<SnapshotDiff>().Property(x => x.TimestampUtc).HasColumnName(@"timestamp_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<SnapshotDiff>().Property(x => x.SchemaVersion).HasColumnName(@"schema_version").HasColumnType(@"nvarchar(200)").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<SnapshotDiff>().HasKey(@"Id");
    }








    partial void CustomizeSnapshotDiffMapping(ModelBuilder modelBuilder);

    #endregion



    #region SourceSnapshot Mapping

    private void SourceSnapshotMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<SourceSnapshot>().ToTable(@"source_snapshot", @"dbo");
        _ = modelBuilder.Entity<SourceSnapshot>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<SourceSnapshot>().Property(x => x.IngestionRunId).HasColumnName(@"ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<SourceSnapshot>().Property(x => x.SnapshotUid).HasColumnName(@"snapshot_uid").HasColumnType(@"nvarchar(200)").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<SourceSnapshot>().Property(x => x.RepoUrl).HasColumnName(@"repo_url").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<SourceSnapshot>().Property(x => x.Branch).HasColumnName(@"branch").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<SourceSnapshot>().Property(x => x.RepoCommit).HasColumnName(@"repo_commit").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<SourceSnapshot>().Property(x => x.Language).HasColumnName(@"language").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<SourceSnapshot>().Property(x => x.PackageName).HasColumnName(@"package_name").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<SourceSnapshot>().Property(x => x.PackageVersion).HasColumnName(@"package_version").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<SourceSnapshot>().Property(x => x.ConfigJson).HasColumnName(@"config_json").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<SourceSnapshot>().Property(x => x.SnapshotUidHash).HasColumnName(@"snapshot_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedOnAddOrUpdate().HasMaxLength(32);
        _ = modelBuilder.Entity<SourceSnapshot>().HasKey(@"Id");
        _ = modelBuilder.Entity<SourceSnapshot>().HasIndex(@"SnapshotUidHash").IsUnique();
    }








    partial void CustomizeSourceSnapshotMapping(ModelBuilder modelBuilder);

    #endregion



    #region TruthRun Mapping

    private void TruthRunMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<TruthRun>().ToTable(@"truth_run", @"dbo");
        _ = modelBuilder.Entity<TruthRun>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql(@"newid()");
        _ = modelBuilder.Entity<TruthRun>().Property(x => x.SnapshotId).HasColumnName(@"snapshot_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<TruthRun>().Property(x => x.TimestampUtc).HasColumnName(@"timestamp_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<TruthRun>().Property(x => x.SchemaVersion).HasColumnName(@"schema_version").HasColumnType(@"nvarchar(200)").IsRequired().ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<TruthRun>().HasKey(@"Id");
    }








    partial void CustomizeTruthRunMapping(ModelBuilder modelBuilder);

    #endregion



    #region VApiFeatureCurrent Mapping

    private void VApiFeatureCurrentMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<VApiFeatureCurrent>().HasNoKey();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().ToView(@"v_api_feature_current", @"dbo");
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.ApiTypeId).HasColumnName(@"api_type_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.SemanticUid).HasColumnName(@"semantic_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.TruthRunId).HasColumnName(@"truth_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.Name).HasColumnName(@"name").HasColumnType(@"nvarchar(400)").ValueGeneratedNever().HasMaxLength(400);
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.Language).HasColumnName(@"language").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.Description).HasColumnName(@"description").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.Tags).HasColumnName(@"tags").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.VersionNumber).HasColumnName(@"version_number").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"created_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"updated_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"removed_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.ValidFromUtc).HasColumnName(@"valid_from_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.ValidToUtc).HasColumnName(@"valid_to_utc").HasColumnType(@"datetime2").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.IsActive).HasColumnName(@"is_active").HasColumnType(@"bit").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.ContentHash).HasColumnName(@"content_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<VApiFeatureCurrent>().Property(x => x.SemanticUidHash).HasColumnName(@"semantic_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
    }








    partial void CustomizeVApiFeatureCurrentMapping(ModelBuilder modelBuilder);

    #endregion



    #region VApiMemberCurrent Mapping

    private void VApiMemberCurrentMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<VApiMemberCurrent>().HasNoKey();
        _ = modelBuilder.Entity<VApiMemberCurrent>().ToView(@"v_api_member_current", @"dbo");
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.SemanticUid).HasColumnName(@"semantic_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ApiFeatureId).HasColumnName(@"api_feature_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Name).HasColumnName(@"name").HasColumnType(@"nvarchar(400)").ValueGeneratedNever().HasMaxLength(400);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Kind).HasColumnName(@"kind").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.MethodKind).HasColumnName(@"method_kind").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Accessibility).HasColumnName(@"accessibility").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsStatic).HasColumnName(@"is_static").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsExtensionMethod).HasColumnName(@"is_extension_method").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsAsync).HasColumnName(@"is_async").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsVirtual).HasColumnName(@"is_virtual").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsOverride).HasColumnName(@"is_override").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsAbstract).HasColumnName(@"is_abstract").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsSealed).HasColumnName(@"is_sealed").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsReadonly).HasColumnName(@"is_readonly").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsConst).HasColumnName(@"is_const").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsUnsafe).HasColumnName(@"is_unsafe").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ReturnTypeUid).HasColumnName(@"return_type_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ReturnNullable).HasColumnName(@"return_nullable").HasColumnType(@"nvarchar(50)").ValueGeneratedNever().HasMaxLength(50);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.GenericParameters).HasColumnName(@"generic_parameters").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.GenericConstraints).HasColumnName(@"generic_constraints").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Summary).HasColumnName(@"summary").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Remarks).HasColumnName(@"remarks").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.Attributes).HasColumnName(@"attributes").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.SourceFilePath).HasColumnName(@"source_file_path").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.SourceStartLine).HasColumnName(@"source_start_line").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.SourceEndLine).HasColumnName(@"source_end_line").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.MemberUidHash).HasColumnName(@"member_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.VersionNumber).HasColumnName(@"version_number").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"created_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"updated_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"removed_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ValidFromUtc).HasColumnName(@"valid_from_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ValidToUtc).HasColumnName(@"valid_to_utc").HasColumnType(@"datetime2").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.IsActive).HasColumnName(@"is_active").HasColumnType(@"bit").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.ContentHash).HasColumnName(@"content_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<VApiMemberCurrent>().Property(x => x.SemanticUidHash).HasColumnName(@"semantic_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
    }








    partial void CustomizeVApiMemberCurrentMapping(ModelBuilder modelBuilder);

    #endregion



    #region VApiTypeCurrent Mapping

    private void VApiTypeCurrentMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<VApiTypeCurrent>().HasNoKey();
        _ = modelBuilder.Entity<VApiTypeCurrent>().ToView(@"v_api_type_current", @"dbo");
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SemanticUid).HasColumnName(@"semantic_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SourceSnapshotId).HasColumnName(@"source_snapshot_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Name).HasColumnName(@"name").HasColumnType(@"nvarchar(400)").ValueGeneratedNever().HasMaxLength(400);
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.NamespacePath).HasColumnName(@"namespace_path").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Kind).HasColumnName(@"kind").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Accessibility).HasColumnName(@"accessibility").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsStatic).HasColumnName(@"is_static").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsGeneric).HasColumnName(@"is_generic").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsAbstract).HasColumnName(@"is_abstract").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsSealed).HasColumnName(@"is_sealed").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsRecord).HasColumnName(@"is_record").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsRefLike).HasColumnName(@"is_ref_like").HasColumnType(@"bit").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.BaseTypeUid).HasColumnName(@"base_type_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Interfaces).HasColumnName(@"interfaces").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.ContainingTypeUid).HasColumnName(@"containing_type_uid").HasColumnType(@"nvarchar(1000)").ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.GenericParameters).HasColumnName(@"generic_parameters").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.GenericConstraints).HasColumnName(@"generic_constraints").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Summary).HasColumnName(@"summary").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Remarks).HasColumnName(@"remarks").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.Attributes).HasColumnName(@"attributes").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SourceFilePath).HasColumnName(@"source_file_path").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SourceStartLine).HasColumnName(@"source_start_line").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SourceEndLine).HasColumnName(@"source_end_line").HasColumnType(@"int").ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.VersionNumber).HasColumnName(@"version_number").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"created_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"updated_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"removed_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.ValidFromUtc).HasColumnName(@"valid_from_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.ValidToUtc).HasColumnName(@"valid_to_utc").HasColumnType(@"datetime2").ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.IsActive).HasColumnName(@"is_active").HasColumnType(@"bit").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.ContentHash).HasColumnName(@"content_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<VApiTypeCurrent>().Property(x => x.SemanticUidHash).HasColumnName(@"semantic_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
    }








    partial void CustomizeVApiTypeCurrentMapping(ModelBuilder modelBuilder);

    #endregion



    #region VDocPageCurrent Mapping

    private void VDocPageCurrentMapping(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<VDocPageCurrent>().HasNoKey();
        _ = modelBuilder.Entity<VDocPageCurrent>().ToView(@"v_doc_page_current", @"dbo");
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.Id).HasColumnName(@"id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.SemanticUid).HasColumnName(@"semantic_uid").HasColumnType(@"nvarchar(1000)").IsRequired().ValueGeneratedNever().HasMaxLength(1000);
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.SourceSnapshotId).HasColumnName(@"source_snapshot_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.SourcePath).HasColumnName(@"source_path").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.Title).HasColumnName(@"title").HasColumnType(@"nvarchar(400)").ValueGeneratedNever().HasMaxLength(400);
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.Language).HasColumnName(@"language").HasColumnType(@"nvarchar(200)").ValueGeneratedNever().HasMaxLength(200);
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.Url).HasColumnName(@"url").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.RawMarkdown).HasColumnName(@"raw_markdown").HasColumnType(@"nvarchar(max)").ValueGeneratedNever();
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.VersionNumber).HasColumnName(@"version_number").HasColumnType(@"int").IsRequired().ValueGeneratedNever().HasPrecision(10, 0);
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.CreatedIngestionRunId).HasColumnName(@"created_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.UpdatedIngestionRunId).HasColumnName(@"updated_ingestion_run_id").HasColumnType(@"uniqueidentifier").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.RemovedIngestionRunId).HasColumnName(@"removed_ingestion_run_id").HasColumnType(@"uniqueidentifier").ValueGeneratedNever();
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.ValidFromUtc).HasColumnName(@"valid_from_utc").HasColumnType(@"datetime2").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.ValidToUtc).HasColumnName(@"valid_to_utc").HasColumnType(@"datetime2").ValueGeneratedNever();
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.IsActive).HasColumnName(@"is_active").HasColumnType(@"bit").IsRequired().ValueGeneratedNever();
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.ContentHash).HasColumnName(@"content_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
        _ = modelBuilder.Entity<VDocPageCurrent>().Property(x => x.SemanticUidHash).HasColumnName(@"semantic_uid_hash").HasColumnType(@"binary(32)").ValueGeneratedNever().HasMaxLength(32);
    }








    partial void CustomizeVDocPageCurrentMapping(ModelBuilder modelBuilder);

    #endregion



}





public class SpCheckTemporalConsistencyMultipleResult
{

    public SpCheckTemporalConsistencyMultipleResult()
    {
        SpCheckTemporalConsistencyResults = [];
        SpCheckTemporalConsistencyResults1 = [];
        SpCheckTemporalConsistencyResults2 = [];
        SpCheckTemporalConsistencyResults3 = [];
        SpCheckTemporalConsistencyResult1s = [];
        SpCheckTemporalConsistencyResult1s1 = [];
        SpCheckTemporalConsistencyResult1s2 = [];
        SpCheckTemporalConsistencyResult1s3 = [];
        SpCheckTemporalConsistencyResult2s = [];
        SpCheckTemporalConsistencyResult2s1 = [];
        SpCheckTemporalConsistencyResult2s2 = [];
        SpCheckTemporalConsistencyResult2s3 = [];
    }








    public List<SpCheckTemporalConsistencyResult> SpCheckTemporalConsistencyResults { get; }
    public List<SpCheckTemporalConsistencyResult> SpCheckTemporalConsistencyResults1 { get; private set; }
    public List<SpCheckTemporalConsistencyResult> SpCheckTemporalConsistencyResults2 { get; private set; }
    public List<SpCheckTemporalConsistencyResult> SpCheckTemporalConsistencyResults3 { get; private set; }
    public List<SpCheckTemporalConsistencyResult1> SpCheckTemporalConsistencyResult1s { get; }
    public List<SpCheckTemporalConsistencyResult1> SpCheckTemporalConsistencyResult1s1 { get; private set; }
    public List<SpCheckTemporalConsistencyResult1> SpCheckTemporalConsistencyResult1s2 { get; private set; }
    public List<SpCheckTemporalConsistencyResult1> SpCheckTemporalConsistencyResult1s3 { get; private set; }
    public List<SpCheckTemporalConsistencyResult2> SpCheckTemporalConsistencyResult2s { get; }
    public List<SpCheckTemporalConsistencyResult2> SpCheckTemporalConsistencyResult2s1 { get; private set; }
    public List<SpCheckTemporalConsistencyResult2> SpCheckTemporalConsistencyResult2s2 { get; private set; }
    public List<SpCheckTemporalConsistencyResult2> SpCheckTemporalConsistencyResult2s3 { get; private set; }
}





public class SpUpsertApiParameterMultipleResult
{

    public SpUpsertApiParameterMultipleResult()
    {
        SpUpsertApiParameterResults = [];
        SpUpsertApiParameterResults1 = [];
        SpUpsertApiParameterResults2 = [];
    }








    public List<SpUpsertApiParameterResult> SpUpsertApiParameterResults { get; }
    public List<SpUpsertApiParameterResult> SpUpsertApiParameterResults1 { get; private set; }
    public List<SpUpsertApiParameterResult> SpUpsertApiParameterResults2 { get; private set; }
}





public class SpVerifyIngestionRunMultipleResult
{

    public SpVerifyIngestionRunMultipleResult()
    {
        SpVerifyIngestionRunResults = [];
        SpVerifyIngestionRunResults1 = [];
    }








    public List<SpVerifyIngestionRunResult> SpVerifyIngestionRunResults { get; }
    public List<SpVerifyIngestionRunResult> SpVerifyIngestionRunResults1 { get; private set; }
}