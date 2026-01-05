

**Planning EF integration layer**

Okay, I need to provide guidance for integrating EF Core with entry procs like BeginIngestionRun and EndIngestionRun. I’m thinking of creating an upsert process, possibly a more generic IngestArtifact, and covering how to use these in EF Core. Maybe I'll add a helper layer, using an additional .sql file and C# notes for clarity. I need to stay concise but thorough, including SQL for the procs and views for EF where necessary. I'll keep it clear and under length.You’re exactly right: EF can absolutely work with this, as long as we’re deliberate and explicit.

Below is the **recommended EF‑friendly layer**:

* SQL helpers (entry points for ingestion)
* Clear EF usage pattern (how to wire it)

Treat this as an additional artifact on top of the schema + helper layer you already have.

* * *

### 1. SQL: ingestion entry points

This gives you a **canonical way to start and finish an ingestion**, plus a single place to create snapshots and truth runs.
    /* =============================================================================
       95_ingestion_entrypoints.sql
       High-level ingestion helpers for EF / app orchestration
       ============================================================================= */

    SET NOCOUNT ON;
    GO

    /* -----------------------------------------------------------------------------
       BeginIngestionRun
       - Creates ingestion_run
       - Returns @IngestionRunId
       ----------------------------------------------------------------------------- */
    IF OBJECT_ID('dbo.BeginIngestionRun', 'P') IS NOT NULL
        DROP PROCEDURE dbo.BeginIngestionRun;
    GO

    CREATE PROCEDURE [dbo].[BeginIngestionRun]
        @SchemaVersion NVARCHAR(200),
        @Notes         NVARCHAR(MAX) = NULL,
        @IngestionRunId UNIQUEIDENTIFIER OUTPUT
    AS
    BEGIN
        SET NOCOUNT ON;

        DECLARE @NowUtc DATETIME2(7) = SYSUTCDATETIME();

        SET @IngestionRunId = NEWID();

        INSERT INTO dbo.ingestion_run (id, timestamp_utc, schema_version, notes)
        VALUES (@IngestionRunId, @NowUtc, @SchemaVersion, @Notes);
    END
    GO


    /* -----------------------------------------------------------------------------
       CreateSourceSnapshot
       - Creates source_snapshot bound to an ingestion_run
       - Returns @SnapshotId
       ----------------------------------------------------------------------------- */
    IF OBJECT_ID('dbo.CreateSourceSnapshot', 'P') IS NOT NULL
        DROP PROCEDURE dbo.CreateSourceSnapshot;
    GO

    CREATE PROCEDURE [dbo].[CreateSourceSnapshot]
        @IngestionRunId  UNIQUEIDENTIFIER,
        @SnapshotUid     NVARCHAR(1000),
        @RepoUrl         NVARCHAR(MAX)    = NULL,
        @Branch          NVARCHAR(200)    = NULL,
        @RepoCommit      NVARCHAR(200)    = NULL,
        @Language        NVARCHAR(200)    = NULL,
        @PackageName     NVARCHAR(200)    = NULL,
        @PackageVersion  NVARCHAR(200)    = NULL,
        @ConfigJson      NVARCHAR(MAX)    = NULL,
        @SnapshotId      UNIQUEIDENTIFIER OUTPUT
    AS
    BEGIN
        SET NOCOUNT ON;

        SET @SnapshotId = NEWID();

        INSERT INTO dbo.source_snapshot
        (
            id,
            ingestion_run_id,
            snapshot_uid,
            repo_url,
            branch,
            repo_commit,
            language,
            package_name,
            package_version,
            config_json
        )
        VALUES
        (
            @SnapshotId,
            @IngestionRunId,
            @SnapshotUid,
            @RepoUrl,
            @Branch,
            @RepoCommit,
            @Language,
            @PackageName,
            @PackageVersion,
            @ConfigJson
        );
    END
    GO


    /* -----------------------------------------------------------------------------
       CreateTruthRun
       - Creates a truth_run bound to a snapshot
       - Returns @TruthRunId
       ----------------------------------------------------------------------------- */
    IF OBJECT_ID('dbo.CreateTruthRun', 'P') IS NOT NULL
        DROP PROCEDURE dbo.CreateTruthRun;
    GO

    CREATE PROCEDURE [dbo].[CreateTruthRun]
        @SnapshotId    UNIQUEIDENTIFIER,
        @SchemaVersion NVARCHAR(200),
        @TruthRunId    UNIQUEIDENTIFIER OUTPUT
    AS
    BEGIN
        SET NOCOUNT ON;

        SET @TruthRunId = NEWID();

        INSERT INTO dbo.truth_run (id, snapshot_id, timestamp_utc, schema_version)
        VALUES (@TruthRunId, @SnapshotId, SYSUTCDATETIME(), @SchemaVersion);
    END
    GO


    /* -----------------------------------------------------------------------------
       EndIngestionRun
       - Currently just a marker; you can later add bookkeeping/metrics here.
       ----------------------------------------------------------------------------- */
    IF OBJECT_ID('dbo.EndIngestionRun', 'P') IS NOT NULL
        DROP PROCEDURE dbo.EndIngestionRun;
    GO

    CREATE PROCEDURE [dbo].[EndIngestionRun]
        @IngestionRunId UNIQUEIDENTIFIER
    AS
    BEGIN
        SET NOCOUNT ON;

        -- Placeholder for any closing logic, metrics aggregation, etc.
        -- Intentionally no-op for now, but gives you a stable contract.
        RETURN;
    END
    GO

At this point you have:

* `BeginIngestionRun` → gets you `IngestionRunId`
* `CreateSourceSnapshot` → gets you `SnapshotId`
* `CreateTruthRun` → gets you `TruthRunId`
* `UpsertApiType`, `UpsertApiMember`, `UpsertDocPage`, `UpsertApiFeature` → do all temporal work
* `EndIngestionRun` → close it out

That’s your EF “command surface.”

* * *

### 2. Recommended EF usage pattern

Here’s the practical way to wire this into EF Core (rough outline, not a full project):

#### 2.1. Read model: map EF to views, not base tables

In your `DbContext`:
    public class ApiDbContext : DbContext
    {
        public DbSet<ApiTypeCurrent> ApiTypes { get; set; }
        public DbSet<ApiMemberCurrent> ApiMembers { get; set; }
        public DbSet<DocPageCurrent> DocPages { get; set; }
        public DbSet<ApiFeatureCurrent> ApiFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiTypeCurrent>()
                .HasNoKey()
                .ToView("v_api_type_current");

            modelBuilder.Entity<ApiMemberCurrent>()
                .HasNoKey()
                .ToView("v_api_member_current");

            modelBuilder.Entity<DocPageCurrent>()
                .HasNoKey()
                .ToView("v_doc_page_current");

            modelBuilder.Entity<ApiFeatureCurrent>()
                .HasNoKey()
                .ToView("v_api_feature_current");

            // Optionally: prevent EF migrations from touching the DB
            modelBuilder.Entity<ApiTypeCurrent>().Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<ApiMemberCurrent>().Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<DocPageCurrent>().Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<ApiFeatureCurrent>().Metadata.SetIsTableExcludedFromMigrations(true);
        }
    }

Important:

* Views are **read‑only models**
* EF will not try to `INSERT/UPDATE` them
* You always use stored procs for writes

#### 2.2. Write model: call stored procedures from EF

For ingestion, use `DbContext.Database.ExecuteSqlRaw` or `FromSqlInterpolated` where meaningful.

Example: starting an ingestion run from C#:
    public async Task<Guid> BeginIngestionRunAsync(ApiDbContext db, string schemaVersion, string? notes)
    {
        var ingestionRunIdParam = new SqlParameter
        {
            ParameterName = "@IngestionRunId",
            SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            Direction = System.Data.ParameterDirection.Output
        };

        await db.Database.ExecuteSqlRawAsync(
            "EXEC dbo.BeginIngestionRun @SchemaVersion = {0}, @Notes = {1}, @IngestionRunId = @IngestionRunId OUTPUT",
            schemaVersion,
            notes ?? (object)DBNull.Value,
            ingestionRunIdParam);

        return (Guid)ingestionRunIdParam.Value;
    }

Creating a snapshot:
    public async Task<Guid> CreateSnapshotAsync(
        ApiDbContext db,
        Guid ingestionRunId,
        string snapshotUid,
        string? repoUrl,
        string? branch,
        string? commit,
        string? language,
        string? packageName,
        string? packageVersion,
        string? configJson)
    {
        var snapshotIdParam = new SqlParameter
        {
            ParameterName = "@SnapshotId",
            SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            Direction = System.Data.ParameterDirection.Output
        };

        await db.Database.ExecuteSqlRawAsync(
            @"EXEC dbo.CreateSourceSnapshot
                  @IngestionRunId  = {0},
                  @SnapshotUid     = {1},
                  @RepoUrl         = {2},
                  @Branch          = {3},
                  @RepoCommit      = {4},
                  @Language        = {5},
                  @PackageName     = {6},
                  @PackageVersion  = {7},
                  @ConfigJson      = {8},
                  @SnapshotId      = @SnapshotId OUTPUT",
            ingestionRunId,
            snapshotUid,
            (object?)repoUrl ?? DBNull.Value,
            (object?)branch ?? DBNull.Value,
            (object?)commit ?? DBNull.Value,
            (object?)language ?? DBNull.Value,
            (object?)packageName ?? DBNull.Value,
            (object?)packageVersion ?? DBNull.Value,
            (object?)configJson ?? DBNull.Value,
            snapshotIdParam);

        return (Guid)snapshotIdParam.Value;
    }

Upserting a type:
    public async Task UpsertTypeAsync(
        ApiDbContext db,
        Guid ingestionRunId,
        Guid snapshotId,
        string semanticUid,
        ApiTypePayload payload)
    {
        await db.Database.ExecuteSqlRawAsync(
            @"EXEC dbo.UpsertApiType
                  @SemanticUid       = {0},
                  @SourceSnapshotId  = {1},
                  @IngestionRunId    = {2},
                  @Name              = {3},
                  @NamespacePath     = {4},
                  @Kind              = {5},
                  @Accessibility     = {6},
                  @IsStatic          = {7},
                  @IsGeneric         = {8},
                  @IsAbstract        = {9},
                  @IsSealed          = {10},
                  @IsRecord          = {11},
                  @IsRefLike         = {12},
                  @BaseTypeUid       = {13},
                  @Interfaces        = {14},
                  @ContainingTypeUid = {15},
                  @GenericParameters = {16},
                  @GenericConstraints= {17},
                  @Summary           = {18},
                  @Remarks           = {19},
                  @Attributes        = {20},
                  @SourceFilePath    = {21},
                  @SourceStartLine   = {22},
                  @SourceEndLine     = {23}",
            semanticUid,
            snapshotId,
            ingestionRunId,
            (object?)payload.Name ?? DBNull.Value,
            (object?)payload.NamespacePath ?? DBNull.Value,
            (object?)payload.Kind ?? DBNull.Value,
            (object?)payload.Accessibility ?? DBNull.Value,
            (object?)payload.IsStatic ?? DBNull.Value,
            (object?)payload.IsGeneric ?? DBNull.Value,
            (object?)payload.IsAbstract ?? DBNull.Value,
            (object?)payload.IsSealed ?? DBNull.Value,
            (object?)payload.IsRecord ?? DBNull.Value,
            (object?)payload.IsRefLike ?? DBNull.Value,
            (object?)payload.BaseTypeUid ?? DBNull.Value,
            (object?)payload.Interfaces ?? DBNull.Value,
            (object?)payload.ContainingTypeUid ?? DBNull.Value,
            (object?)payload.GenericParameters ?? DBNull.Value,
            (object?)payload.GenericConstraints ?? DBNull.Value,
            (object?)payload.Summary ?? DBNull.Value,
            (object?)payload.Remarks ?? DBNull.Value,
            (object?)payload.Attributes ?? DBNull.Value,
            (object?)payload.SourceFilePath ?? DBNull.Value,
            (object?)payload.SourceStartLine ?? DBNull.Value,
            (object?)payload.SourceEndLine ?? DBNull.Value
        );
    }

Same pattern for `UpsertApiMember`, `UpsertDocPage`, `UpsertApiFeature`.

* * *

### 3. Recommended ingestion flow (conceptual)

From your app/agent:

1. **Start run:**
   * Call `BeginIngestionRun` → get `IngestionRunId`
2. **Create snapshot:**
   * Call `CreateSourceSnapshot` → get `SnapshotId`
3. **Create truth run (optional but recommended):**
   * Call `CreateTruthRun` → get `TruthRunId`
4. **Ingest artifacts:**
   * For each type → `UpsertApiType`
   * For each member → `UpsertApiMember`
   * For each doc page → `UpsertDocPage`
   * For each feature → `UpsertApiFeature`
5. **End run:**
   * Call `EndIngestionRun`

EF’s job is:

* Manage connections, transactions, and mapping to **views**
* Call stored procedures for **all writes**

* * *

### 4. Guardrails to set in EF

A few things I strongly recommend:

* **Do not scaffold entities for the base temporal tables** (`api_type`, `api_member`, etc.) as mutable DbSets.
  * If you scaffold them for diagnostics, mark them as:
    * `HasNoKey()` or
    * Excluded from migrations and never used for `SaveChanges()`
* **Do not use EF migrations** to evolve this schema; manage the SQL as artifacts.
* **Do not let EF generate INSERT/UPDATE/DELETE SQL** for temporal tables.
* Treat the DB as the **source of truth**, EF as a **client of the contract**, not the owner of the schema.

* * *


