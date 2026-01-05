/* =============================================================================
   90_helpers_full_temporal.sql
   Helper layer for temporal, Git-like “living artifacts”
   Depends on:
     - semantic_identity
     - api_type, api_member, api_parameter
     - doc_page
     - api_feature
     - ingestion_run, source_snapshot, truth_run
   ============================================================================= */

SET NOCOUNT ON;
GO

/* =============================================================================
   1. SEMANTIC IDENTITY HELPERS
   ============================================================================= */

IF OBJECT_ID('dbo.RegisterSemanticIdentity', 'P') IS NOT NULL
    DROP PROCEDURE dbo.RegisterSemanticIdentity;
GO

CREATE PROCEDURE [dbo].[RegisterSemanticIdentity]
    @Uid       NVARCHAR(1000),
    @Kind      NVARCHAR(50),
    @Notes     NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.semantic_identity WHERE uid = @Uid)
        RETURN;

    INSERT INTO dbo.semantic_identity (uid, kind, created_utc, notes)
    VALUES (@Uid, @Kind, SYSUTCDATETIME(), @Notes);
END
GO


IF OBJECT_ID('dbo.EnsureSemanticIdentity', 'P') IS NOT NULL
    DROP PROCEDURE dbo.EnsureSemanticIdentity;
GO

CREATE PROCEDURE [dbo].[EnsureSemanticIdentity]
    @Uid       NVARCHAR(1000),
    @Kind      NVARCHAR(50),
    @Notes     NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.semantic_identity WHERE uid = @Uid)
    BEGIN
        EXEC dbo.RegisterSemanticIdentity @Uid = @Uid, @Kind = @Kind, @Notes = @Notes;
    END
END
GO


IF OBJECT_ID('dbo.DeleteSemanticIdentity', 'P') IS NOT NULL
    DROP PROCEDURE dbo.DeleteSemanticIdentity;
GO

CREATE PROCEDURE [dbo].[DeleteSemanticIdentity]
    @Uid   NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;

    -- Safety: do not allow deleting identities that still have versions.
    IF EXISTS (SELECT 1 FROM dbo.api_type    WHERE semantic_uid = @Uid)
       OR EXISTS (SELECT 1 FROM dbo.api_member WHERE semantic_uid = @Uid)
       OR EXISTS (SELECT 1 FROM dbo.doc_page  WHERE semantic_uid = @Uid)
       OR EXISTS (SELECT 1 FROM dbo.api_feature WHERE semantic_uid = @Uid)
    BEGIN
        RAISERROR('Cannot delete semantic identity: versions still exist for this UID.', 16, 1);
        RETURN;
    END

    DELETE FROM dbo.semantic_identity WHERE uid = @Uid;
END
GO


/* =============================================================================
   2. CONTENT HASH HELPER (SQL Server scalar function, SHA2-256)
   ============================================================================= */

IF OBJECT_ID('dbo.fn_ComputeContentHash256', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_ComputeContentHash256;
GO

CREATE FUNCTION [dbo].[fn_ComputeContentHash256]
(
    @Input NVARCHAR(MAX)
)
RETURNS BINARY(32)
AS
BEGIN
    RETURN HASHBYTES('SHA2_256', @Input);
END
GO


/* =============================================================================
   3. CURRENT TRUTH VIEWS
   ============================================================================= */

IF OBJECT_ID('dbo.v_api_type_current', 'V') IS NOT NULL
    DROP VIEW dbo.v_api_type_current;
GO

CREATE VIEW [dbo].[v_api_type_current]
AS
SELECT t.*
FROM dbo.api_type t
WHERE t.is_active = 1
  AND t.valid_to_utc IS NULL;
GO


IF OBJECT_ID('dbo.v_api_member_current', 'V') IS NOT NULL
    DROP VIEW dbo.v_api_member_current;
GO

CREATE VIEW [dbo].[v_api_member_current]
AS
SELECT m.*
FROM dbo.api_member m
WHERE m.is_active = 1
  AND m.valid_to_utc IS NULL;
GO


IF OBJECT_ID('dbo.v_doc_page_current', 'V') IS NOT NULL
    DROP VIEW dbo.v_doc_page_current;
GO

CREATE VIEW [dbo].[v_doc_page_current]
AS
SELECT d.*
FROM dbo.doc_page d
WHERE d.is_active = 1
  AND d.valid_to_utc IS NULL;
GO


IF OBJECT_ID('dbo.v_api_feature_current', 'V') IS NOT NULL
    DROP VIEW dbo.v_api_feature_current;
GO

CREATE VIEW [dbo].[v_api_feature_current]
AS
SELECT f.*
FROM dbo.api_feature f
WHERE f.is_active = 1
  AND f.valid_to_utc IS NULL;
GO


/* =============================================================================
   4. AS-OF FUNCTIONS
   ============================================================================= */

IF OBJECT_ID('dbo.fn_GetTypeAsOf', 'IF') IS NOT NULL
    DROP FUNCTION dbo.fn_GetTypeAsOf;
GO

CREATE FUNCTION [dbo].[fn_GetTypeAsOf]
(
    @SemanticUid NVARCHAR(1000),
    @AsOfUtc     DATETIME2(7)
)
RETURNS TABLE
AS
RETURN
(
    SELECT TOP (1) t.*
    FROM dbo.api_type t
    WHERE t.semantic_uid = @SemanticUid
      AND t.valid_from_utc <= @AsOfUtc
      AND (t.valid_to_utc IS NULL OR t.valid_to_utc > @AsOfUtc)
    ORDER BY t.valid_from_utc DESC
);
GO


IF OBJECT_ID('dbo.fn_GetMemberAsOf', 'IF') IS NOT NULL
    DROP FUNCTION dbo.fn_GetMemberAsOf;
GO

CREATE FUNCTION [dbo].[fn_GetMemberAsOf]
(
    @SemanticUid NVARCHAR(1000),
    @AsOfUtc     DATETIME2(7)
)
RETURNS TABLE
AS
RETURN
(
    SELECT TOP (1) m.*
    FROM dbo.api_member m
    WHERE m.semantic_uid = @SemanticUid
      AND m.valid_from_utc <= @AsOfUtc
      AND (m.valid_to_utc IS NULL OR m.valid_to_utc > @AsOfUtc)
    ORDER BY m.valid_from_utc DESC
);
GO


IF OBJECT_ID('dbo.fn_GetDocPageAsOf', 'IF') IS NOT NULL
    DROP FUNCTION dbo.fn_GetDocPageAsOf;
GO

CREATE FUNCTION [dbo].[fn_GetDocPageAsOf]
(
    @SemanticUid NVARCHAR(1000),
    @AsOfUtc     DATETIME2(7)
)
RETURNS TABLE
AS
RETURN
(
    SELECT TOP (1) d.*
    FROM dbo.doc_page d
    WHERE d.semantic_uid = @SemanticUid
      AND d.valid_from_utc <= @AsOfUtc
      AND (d.valid_to_utc IS NULL OR d.valid_to_utc > @AsOfUtc)
    ORDER BY d.valid_from_utc DESC
);
GO


IF OBJECT_ID('dbo.fn_GetFeatureAsOf', 'IF') IS NOT NULL
    DROP FUNCTION dbo.fn_GetFeatureAsOf;
GO

CREATE FUNCTION [dbo].[fn_GetFeatureAsOf]
(
    @SemanticUid NVARCHAR(1000),
    @AsOfUtc     DATETIME2(7)
)
RETURNS TABLE
AS
RETURN
(
    SELECT TOP (1) f.*
    FROM dbo.api_feature f
    WHERE f.semantic_uid = @SemanticUid
      AND f.valid_from_utc <= @AsOfUtc
      AND (f.valid_to_utc IS NULL OR f.valid_to_utc > @AsOfUtc)
    ORDER BY f.valid_from_utc DESC
);
GO


/* =============================================================================
   5. HISTORY HELPERS
   ============================================================================= */

IF OBJECT_ID('dbo.GetTypeHistory', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetTypeHistory;
GO

CREATE PROCEDURE [dbo].[GetTypeHistory]
    @SemanticUid NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.api_type t
    WHERE t.semantic_uid = @SemanticUid
    ORDER BY t.version_number;
END
GO


IF OBJECT_ID('dbo.GetMemberHistory', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetMemberHistory;
GO

CREATE PROCEDURE [dbo].[GetMemberHistory]
    @SemanticUid NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.api_member m
    WHERE m.semantic_uid = @SemanticUid
    ORDER BY m.version_number;
END
GO


IF OBJECT_ID('dbo.GetDocPageHistory', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetDocPageHistory;
GO

CREATE PROCEDURE [dbo].[GetDocPageHistory]
    @SemanticUid NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.doc_page d
    WHERE d.semantic_uid = @SemanticUid
    ORDER BY d.version_number;
END
GO


IF OBJECT_ID('dbo.GetFeatureHistory', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetFeatureHistory;
GO

CREATE PROCEDURE [dbo].[GetFeatureHistory]
    @SemanticUid NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.api_feature f
    WHERE f.semantic_uid = @SemanticUid
    ORDER BY f.version_number;
END
GO


/* =============================================================================
   6. VERSION UPSERT ENGINE (TYPE, MEMBER, DOC, FEATURE)
   - Ingestion never touches temporal logic directly.
   - Pattern: if no change => no new row; if changed => close old, open new.
   ============================================================================= */

-- TYPE UPSERT --------------------------------------------------------------

IF OBJECT_ID('dbo.UpsertApiType', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpsertApiType;
GO

CREATE PROCEDURE [dbo].[UpsertApiType]
(
    @SemanticUid             NVARCHAR(1000),
    @SourceSnapshotId        UNIQUEIDENTIFIER,
    @IngestionRunId          UNIQUEIDENTIFIER,

    @Name                    NVARCHAR(400)    = NULL,
    @NamespacePath           NVARCHAR(1000)   = NULL,
    @Kind                    NVARCHAR(200)    = NULL,
    @Accessibility           NVARCHAR(200)    = NULL,
    @IsStatic                BIT              = NULL,
    @IsGeneric               BIT              = NULL,
    @IsAbstract              BIT              = NULL,
    @IsSealed                BIT              = NULL,
    @IsRecord                BIT              = NULL,
    @IsRefLike               BIT              = NULL,
    @BaseTypeUid             NVARCHAR(1000)   = NULL,
    @Interfaces              NVARCHAR(MAX)    = NULL,
    @ContainingTypeUid       NVARCHAR(1000)   = NULL,
    @GenericParameters       NVARCHAR(MAX)    = NULL,
    @GenericConstraints      NVARCHAR(MAX)    = NULL,
    @Summary                 NVARCHAR(MAX)    = NULL,
    @Remarks                 NVARCHAR(MAX)    = NULL,
    @Attributes              NVARCHAR(MAX)    = NULL,
    @SourceFilePath          NVARCHAR(MAX)    = NULL,
    @SourceStartLine         INT              = NULL,
    @SourceEndLine           INT              = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NowUtc DATETIME2(7) = SYSUTCDATETIME();

    EXEC dbo.EnsureSemanticIdentity @Uid = @SemanticUid, @Kind = 'type';

    DECLARE @Payload NVARCHAR(MAX) =
        CONCAT_WS('|',
            ISNULL(@Name, ''),
            ISNULL(@NamespacePath, ''),
            ISNULL(@Kind, ''),
            ISNULL(@Accessibility, ''),
            COALESCE(CONVERT(NVARCHAR(1), @IsStatic), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsGeneric), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsAbstract), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsSealed), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsRecord), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsRefLike), '0'),
            ISNULL(@BaseTypeUid, ''),
            ISNULL(@Interfaces, ''),
            ISNULL(@ContainingTypeUid, ''),
            ISNULL(@GenericParameters, ''),
            ISNULL(@GenericConstraints, ''),
            ISNULL(@Summary, ''),
            ISNULL(@Remarks, ''),
            ISNULL(@Attributes, ''),
            ISNULL(@SourceFilePath, ''),
            COALESCE(CONVERT(NVARCHAR(20), @SourceStartLine), ''),
            COALESCE(CONVERT(NVARCHAR(20), @SourceEndLine), '')
        );

    DECLARE @NewHash BINARY(32) = dbo.fn_ComputeContentHash256(@Payload);

    DECLARE @CurrentId UNIQUEIDENTIFIER = NULL;
    DECLARE @CurrentVersion INT = 0;
    DECLARE @CurrentHash BINARY(32) = NULL;

    SELECT TOP (1)
        @CurrentId = t.id,
        @CurrentVersion = t.version_number,
        @CurrentHash = t.content_hash
    FROM dbo.api_type t
    WHERE t.semantic_uid = @SemanticUid
      AND t.is_active = 1
      AND t.valid_to_utc IS NULL
    ORDER BY t.version_number DESC;

    -- If no changes, exit early
    IF @CurrentId IS NOT NULL AND @CurrentHash = @NewHash
        RETURN;

    IF @CurrentId IS NOT NULL
    BEGIN
        UPDATE dbo.api_type
        SET is_active = 0,
            valid_to_utc = @NowUtc,
            updated_ingestion_run_id = @IngestionRunId
        WHERE id = @CurrentId;
    END

    INSERT INTO dbo.api_type
    (
        semantic_uid,
        source_snapshot_id,
        name,
        namespace_path,
        kind,
        accessibility,
        is_static,
        is_generic,
        is_abstract,
        is_sealed,
        is_record,
        is_ref_like,
        base_type_uid,
        interfaces,
        containing_type_uid,
        generic_parameters,
        generic_constraints,
        summary,
        remarks,
        attributes,
        source_file_path,
        source_start_line,
        source_end_line,
        version_number,
        created_ingestion_run_id,
        updated_ingestion_run_id,
        removed_ingestion_run_id,
        valid_from_utc,
        valid_to_utc,
        is_active,
        content_hash
    )
    VALUES
    (
        @SemanticUid,
        @SourceSnapshotId,
        @Name,
        @NamespacePath,
        @Kind,
        @Accessibility,
        @IsStatic,
        @IsGeneric,
        @IsAbstract,
        @IsSealed,
        @IsRecord,
        @IsRefLike,
        @BaseTypeUid,
        @Interfaces,
        @ContainingTypeUid,
        @GenericParameters,
        @GenericConstraints,
        @Summary,
        @Remarks,
        @Attributes,
        @SourceFilePath,
        @SourceStartLine,
        @SourceEndLine,
        @CurrentVersion + 1,
        @IngestionRunId,
        @IngestionRunId,
        NULL,
        @NowUtc,
        NULL,
        1,
        @NewHash
    );
END
GO


-- MEMBER UPSERT ------------------------------------------------------------

IF OBJECT_ID('dbo.UpsertApiMember', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpsertApiMember;
GO

CREATE PROCEDURE [dbo].[UpsertApiMember]
(
    @SemanticUid             NVARCHAR(1000),
    @ApiTypeId               UNIQUEIDENTIFIER,
    @IngestionRunId          UNIQUEIDENTIFIER,

    @Name                    NVARCHAR(400)    = NULL,
    @Kind                    NVARCHAR(200)    = NULL,
    @MethodKind              NVARCHAR(200)    = NULL,
    @Accessibility           NVARCHAR(200)    = NULL,
    @IsStatic                BIT              = NULL,
    @IsExtensionMethod       BIT              = NULL,
    @IsAsync                 BIT              = NULL,
    @IsVirtual               BIT              = NULL,
    @IsOverride              BIT              = NULL,
    @IsAbstract              BIT              = NULL,
    @IsSealed                BIT              = NULL,
    @IsReadOnly              BIT              = NULL,
    @IsConst                 BIT              = NULL,
    @IsUnsafe                BIT              = NULL,
    @ReturnTypeUid           NVARCHAR(1000)   = NULL,
    @ReturnNullable          NVARCHAR(50)     = NULL,
    @GenericParameters       NVARCHAR(MAX)    = NULL,
    @GenericConstraints      NVARCHAR(MAX)    = NULL,
    @Summary                 NVARCHAR(MAX)    = NULL,
    @Remarks                 NVARCHAR(MAX)    = NULL,
    @Attributes              NVARCHAR(MAX)    = NULL,
    @SourceFilePath          NVARCHAR(MAX)    = NULL,
    @SourceStartLine         INT              = NULL,
    @SourceEndLine           INT              = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NowUtc DATETIME2(7) = SYSUTCDATETIME();

    EXEC dbo.EnsureSemanticIdentity @Uid = @SemanticUid, @Kind = 'member';

    DECLARE @Payload NVARCHAR(MAX) =
        CONCAT_WS('|',
            ISNULL(@Name, ''),
            ISNULL(@Kind, ''),
            ISNULL(@MethodKind, ''),
            ISNULL(@Accessibility, ''),
            COALESCE(CONVERT(NVARCHAR(1), @IsStatic), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsExtensionMethod), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsAsync), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsVirtual), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsOverride), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsAbstract), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsSealed), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsReadOnly), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsConst), '0'),
            COALESCE(CONVERT(NVARCHAR(1), @IsUnsafe), '0'),
            ISNULL(@ReturnTypeUid, ''),
            ISNULL(@ReturnNullable, ''),
            ISNULL(@GenericParameters, ''),
            ISNULL(@GenericConstraints, ''),
            ISNULL(@Summary, ''),
            ISNULL(@Remarks, ''),
            ISNULL(@Attributes, ''),
            ISNULL(@SourceFilePath, ''),
            COALESCE(CONVERT(NVARCHAR(20), @SourceStartLine), ''),
            COALESCE(CONVERT(NVARCHAR(20), @SourceEndLine), '')
        );

    DECLARE @NewHash BINARY(32) = dbo.fn_ComputeContentHash256(@Payload);

    DECLARE @CurrentId UNIQUEIDENTIFIER = NULL;
    DECLARE @CurrentVersion INT = 0;
    DECLARE @CurrentHash BINARY(32) = NULL;

    SELECT TOP (1)
        @CurrentId = m.id,
        @CurrentVersion = m.version_number,
        @CurrentHash = m.content_hash
    FROM dbo.api_member m
    WHERE m.semantic_uid = @SemanticUid
      AND m.is_active = 1
      AND m.valid_to_utc IS NULL
    ORDER BY m.version_number DESC;

    IF @CurrentId IS NOT NULL AND @CurrentHash = @NewHash
        RETURN;

    IF @CurrentId IS NOT NULL
    BEGIN
        UPDATE dbo.api_member
        SET is_active = 0,
            valid_to_utc = @NowUtc,
            updated_ingestion_run_id = @IngestionRunId
        WHERE id = @CurrentId;
    END

    INSERT INTO dbo.api_member
    (
        semantic_uid,
        api_type_id,
        name,
        kind,
        method_kind,
        accessibility,
        is_static,
        is_extension_method,
        is_async,
        is_virtual,
        is_override,
        is_abstract,
        is_sealed,
        is_readonly,
        is_const,
        is_unsafe,
        return_type_uid,
        return_nullable,
        generic_parameters,
        generic_constraints,
        summary,
        remarks,
        attributes,
        source_file_path,
        source_start_line,
        source_end_line,
        member_uid_hash,
        version_number,
        created_ingestion_run_id,
        updated_ingestion_run_id,
        removed_ingestion_run_id,
        valid_from_utc,
        valid_to_utc,
        is_active,
        content_hash
    )
    VALUES
    (
        @SemanticUid,
        @ApiTypeId,
        @Name,
        @Kind,
        @MethodKind,
        @Accessibility,
        @IsStatic,
        @IsExtensionMethod,
        @IsAsync,
        @IsVirtual,
        @IsOverride,
        @IsAbstract,
        @IsSealed,
        @IsReadOnly,
        @IsConst,
        @IsUnsafe,
        @ReturnTypeUid,
        @ReturnNullable,
        @GenericParameters,
        @GenericConstraints,
        @Summary,
        @Remarks,
        @Attributes,
        @SourceFilePath,
        @SourceStartLine,
        @SourceEndLine,
        @NewHash,                -- reuse hash as member_uid_hash (or compute separately if desired)
        @CurrentVersion + 1,
        @IngestionRunId,
        @IngestionRunId,
        NULL,
        @NowUtc,
        NULL,
        1,
        @NewHash
    );
END
GO


-- DOC PAGE UPSERT ----------------------------------------------------------

IF OBJECT_ID('dbo.UpsertDocPage', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpsertDocPage;
GO

CREATE PROCEDURE [dbo].[UpsertDocPage]
(
    @SemanticUid             NVARCHAR(1000),
    @SourceSnapshotId        UNIQUEIDENTIFIER,
    @IngestionRunId          UNIQUEIDENTIFIER,

    @SourcePath              NVARCHAR(MAX)    = NULL,
    @Title                   NVARCHAR(400)    = NULL,
    @Language                NVARCHAR(200)    = NULL,
    @Url                     NVARCHAR(MAX)    = NULL,
    @RawMarkdown             NVARCHAR(MAX)    = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NowUtc DATETIME2(7) = SYSUTCDATETIME();

    EXEC dbo.EnsureSemanticIdentity @Uid = @SemanticUid, @Kind = 'doc_page';

    DECLARE @Payload NVARCHAR(MAX) =
        CONCAT_WS('|',
            ISNULL(@SourcePath, ''),
            ISNULL(@Title, ''),
            ISNULL(@Language, ''),
            ISNULL(@Url, ''),
            ISNULL(@RawMarkdown, '')
        );

    DECLARE @NewHash BINARY(32) = dbo.fn_ComputeContentHash256(@Payload);

    DECLARE @CurrentId UNIQUEIDENTIFIER = NULL;
    DECLARE @CurrentVersion INT = 0;
    DECLARE @CurrentHash BINARY(32) = NULL;

    SELECT TOP (1)
        @CurrentId = d.id,
        @CurrentVersion = d.version_number,
        @CurrentHash = d.content_hash
    FROM dbo.doc_page d
    WHERE d.semantic_uid = @SemanticUid
      AND d.is_active = 1
      AND d.valid_to_utc IS NULL
    ORDER BY d.version_number DESC;

    IF @CurrentId IS NOT NULL AND @CurrentHash = @NewHash
        RETURN;

    IF @CurrentId IS NOT NULL
    BEGIN
        UPDATE dbo.doc_page
        SET is_active = 0,
            valid_to_utc = @NowUtc,
            updated_ingestion_run_id = @IngestionRunId
        WHERE id = @CurrentId;
    END

    INSERT INTO dbo.doc_page
    (
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
    )
    VALUES
    (
        @SemanticUid,
        @SourceSnapshotId,
        @SourcePath,
        @Title,
        @Language,
        @Url,
        @RawMarkdown,
        @CurrentVersion + 1,
        @IngestionRunId,
        @IngestionRunId,
        NULL,
        @NowUtc,
        NULL,
        1,
        @NewHash
    );
END
GO


-- FEATURE UPSERT -----------------------------------------------------------

IF OBJECT_ID('dbo.UpsertApiFeature', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpsertApiFeature;
GO

CREATE PROCEDURE [dbo].[UpsertApiFeature]
(
    @SemanticUid             NVARCHAR(1000),
    @TruthRunId              UNIQUEIDENTIFIER,
    @IngestionRunId          UNIQUEIDENTIFIER,

    @Name                    NVARCHAR(400)    = NULL,
    @Language                NVARCHAR(200)    = NULL,
    @Description             NVARCHAR(MAX)    = NULL,
    @Tags                    NVARCHAR(MAX)    = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NowUtc DATETIME2(7) = SYSUTCDATETIME();

    EXEC dbo.EnsureSemanticIdentity @Uid = @SemanticUid, @Kind = 'feature';

    DECLARE @Payload NVARCHAR(MAX) =
        CONCAT_WS('|',
            ISNULL(@Name, ''),
            ISNULL(@Language, ''),
            ISNULL(@Description, ''),
            ISNULL(@Tags, '')
        );

    DECLARE @NewHash BINARY(32) = dbo.fn_ComputeContentHash256(@Payload);

    DECLARE @CurrentId UNIQUEIDENTIFIER = NULL;
    DECLARE @CurrentVersion INT = 0;
    DECLARE @CurrentHash BINARY(32) = NULL;

    SELECT TOP (1)
        @CurrentId = f.id,
        @CurrentVersion = f.version_number,
        @CurrentHash = f.content_hash
    FROM dbo.api_feature f
    WHERE f.semantic_uid = @SemanticUid
      AND f.is_active = 1
      AND f.valid_to_utc IS NULL
    ORDER BY f.version_number DESC;

    IF @CurrentId IS NOT NULL AND @CurrentHash = @NewHash
        RETURN;

    IF @CurrentId IS NOT NULL
    BEGIN
        UPDATE dbo.api_feature
        SET is_active = 0,
            valid_to_utc = @NowUtc,
            updated_ingestion_run_id = @IngestionRunId
        WHERE id = @CurrentId;
    END

    INSERT INTO dbo.api_feature
    (
        semantic_uid,
        truth_run_id,
        name,
        language,
        description,
        tags,
        version_number,
        created_ingestion_run_id,
        updated_ingestion_run_id,
        removed_ingestion_run_id,
        valid_from_utc,
        valid_to_utc,
        is_active,
        content_hash
    )
    VALUES
    (
        @SemanticUid,
        @TruthRunId,
        @Name,
        @Language,
        @Description,
        @Tags,
        @CurrentVersion + 1,
        @IngestionRunId,
        @IngestionRunId,
        NULL,
        @NowUtc,
        NULL,
        1,
        @NewHash
    );
END
GO


/* =============================================================================
   7. CHANGE / DIFF HELPERS
   ============================================================================= */

IF OBJECT_ID('dbo.GetChangesInIngestionRun', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetChangesInIngestionRun;
GO

CREATE PROCEDURE [dbo].[GetChangesInIngestionRun]
    @IngestionRunId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Types changed in this ingestion run
    SELECT
        'type'         AS artifact_kind,
        t.semantic_uid,
        t.version_number,
        t.valid_from_utc,
        t.valid_to_utc
    FROM dbo.api_type t
    WHERE t.created_ingestion_run_id = @IngestionRunId
       OR t.updated_ingestion_run_id = @IngestionRunId

    UNION ALL

    -- Members changed
    SELECT
        'member'       AS artifact_kind,
        m.semantic_uid,
        m.version_number,
        m.valid_from_utc,
        m.valid_to_utc
    FROM dbo.api_member m
    WHERE m.created_ingestion_run_id = @IngestionRunId
       OR m.updated_ingestion_run_id = @IngestionRunId

    UNION ALL

    -- Docs changed
    SELECT
        'doc_page'     AS artifact_kind,
        d.semantic_uid,
        d.version_number,
        d.valid_from_utc,
        d.valid_to_utc
    FROM dbo.doc_page d
    WHERE d.created_ingestion_run_id = @IngestionRunId
       OR d.updated_ingestion_run_id = @IngestionRunId

    UNION ALL

    -- Features changed
    SELECT
        'feature'      AS artifact_kind,
        f.semantic_uid,
        f.version_number,
        f.valid_from_utc,
        f.valid_to_utc
    FROM dbo.api_feature f
    WHERE f.created_ingestion_run_id = @IngestionRunId
       OR f.updated_ingestion_run_id = @IngestionRunId

    ORDER BY artifact_kind, semantic_uid, version_number;
END
GO


/* =============================================================================
   8. TEMPORAL CONSISTENCY CHECKER
   ============================================================================= */

IF OBJECT_ID('dbo.CheckTemporalConsistency', 'P') IS NOT NULL
    DROP PROCEDURE dbo.CheckTemporalConsistency;
GO

CREATE PROCEDURE [dbo].[CheckTemporalConsistency]
AS
BEGIN
    SET NOCOUNT ON;

    -- 1) Ensure exactly one active version per semantic_uid per table
    SELECT 'api_type' AS table_name, semantic_uid
    FROM dbo.api_type
    WHERE is_active = 1
    GROUP BY semantic_uid
    HAVING COUNT(*) > 1;

    SELECT 'api_member' AS table_name, semantic_uid
    FROM dbo.api_member
    WHERE is_active = 1
    GROUP BY semantic_uid
    HAVING COUNT(*) > 1;

    SELECT 'doc_page' AS table_name, semantic_uid
    FROM dbo.doc_page
    WHERE is_active = 1
    GROUP BY semantic_uid
    HAVING COUNT(*) > 1;

    SELECT 'api_feature' AS table_name, semantic_uid
    FROM dbo.api_feature
    WHERE is_active = 1
    GROUP BY semantic_uid
    HAVING COUNT(*) > 1;

    -- 2) Check that active rows have valid_to_utc IS NULL
    SELECT 'api_type' AS table_name, id, semantic_uid
    FROM dbo.api_type
    WHERE is_active = 1 AND valid_to_utc IS NOT NULL;

    SELECT 'api_member' AS table_name, id, semantic_uid
    FROM dbo.api_member
    WHERE is_active = 1 AND valid_to_utc IS NOT NULL;

    SELECT 'doc_page' AS table_name, id, semantic_uid
    FROM dbo.doc_page
    WHERE is_active = 1 AND valid_to_utc IS NOT NULL;

    SELECT 'api_feature' AS table_name, id, semantic_uid
    FROM dbo.api_feature
    WHERE is_active = 1 AND valid_to_utc IS NOT NULL;

    -- 3) Check for overlapping validity ranges per semantic_uid
    ;WITH TypeVersions AS (
        SELECT
            semantic_uid,
            version_number,
            valid_from_utc,
            valid_to_utc,
            LEAD(valid_from_utc) OVER (PARTITION BY semantic_uid ORDER BY valid_from_utc) AS next_from
        FROM dbo.api_type
    )
    SELECT 'api_type' AS table_name, *
    FROM TypeVersions
    WHERE valid_to_utc IS NOT NULL
      AND next_from IS NOT NULL
      AND valid_to_utc > next_from;

    ;WITH MemberVersions AS (
        SELECT
            semantic_uid,
            version_number,
            valid_from_utc,
            valid_to_utc,
            LEAD(valid_from_utc) OVER (PARTITION BY semantic_uid ORDER BY valid_from_utc) AS next_from
        FROM dbo.api_member
    )
    SELECT 'api_member' AS table_name, *
    FROM MemberVersions
    WHERE valid_to_utc IS NOT NULL
      AND next_from IS NOT NULL
      AND valid_to_utc > next_from;

    ;WITH DocVersions AS (
        SELECT
            semantic_uid,
            version_number,
            valid_from_utc,
            valid_to_utc,
            LEAD(valid_from_utc) OVER (PARTITION BY semantic_uid ORDER BY valid_from_utc) AS next_from
        FROM dbo.doc_page
    )
    SELECT 'doc_page' AS table_name, *
    FROM DocVersions
    WHERE valid_to_utc IS NOT NULL
      AND next_from IS NOT NULL
      AND valid_to_utc > next_from;

    ;WITH FeatureVersions AS (
        SELECT
            semantic_uid,
            version_number,
            valid_from_utc,
            valid_to_utc,
            LEAD(valid_from_utc) OVER (PARTITION BY semantic_uid ORDER BY valid_from_utc) AS next_from
        FROM dbo.api_feature
    )
    SELECT 'api_feature' AS table_name, *
    FROM FeatureVersions
    WHERE valid_to_utc IS NOT NULL
      AND next_from IS NOT NULL
      AND valid_to_utc > next_from;
END
GO


/* =============================================================================
   9. TEMPORAL COMPACTION (CONTENT-HASH-BASED)
   - Removes redundant versions with identical content_hash.
   - Pattern shown for types; apply to others if desired.
   ============================================================================= */

IF OBJECT_ID('dbo.CompactTypeHistory', 'P') IS NOT NULL
    DROP PROCEDURE dbo.CompactTypeHistory;
GO

CREATE PROCEDURE [dbo].[CompactTypeHistory]
    @SemanticUid NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH OrderedVersions AS (
        SELECT
            id,
            semantic_uid,
            version_number,
            valid_from_utc,
            valid_to_utc,
            content_hash,
            LAG(content_hash) OVER (PARTITION BY semantic_uid ORDER BY version_number) AS prev_hash
        FROM dbo.api_type
        WHERE semantic_uid = @SemanticUid
    )
    DELETE t
    FROM dbo.api_type t
    JOIN OrderedVersions ov ON ov.id = t.id
    WHERE ov.prev_hash = ov.content_hash;
END
GO