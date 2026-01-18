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