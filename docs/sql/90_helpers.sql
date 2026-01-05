-- =============================================================================
-- 90_helpers.sql
-- Helper views and stored procedures for temporal truth
-- =============================================================================

-- Current truth for types
CREATE VIEW [dbo].[v_api_type_current]
AS
SELECT t.*
FROM dbo.api_type t
WHERE t.is_active = 1
  AND t.valid_to_utc IS NULL;
GO

-- Current truth for members
CREATE VIEW [dbo].[v_api_member_current]
AS
SELECT m.*
FROM dbo.api_member m
WHERE m.is_active = 1
  AND m.valid_to_utc IS NULL;
GO


-- Get current type by semantic UID
CREATE PROCEDURE [dbo].[GetCurrentTypeBySemanticUid]
    @SemanticUid NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1) *
    FROM dbo.api_type t
    WHERE t.semantic_uid = @SemanticUid
      AND t.is_active = 1
      AND t.valid_to_utc IS NULL
    ORDER BY t.version_number DESC;
END
GO


-- Get type history by semantic UID
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


-- Get type "as of" a given UTC time
CREATE PROCEDURE [dbo].[GetTypeAsOf]
    @SemanticUid NVARCHAR(1000),
    @AsOfUtc     DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1) *
    FROM dbo.api_type t
    WHERE t.semantic_uid = @SemanticUid
      AND t.valid_from_utc <= @AsOfUtc
      AND (t.valid_to_utc IS NULL OR t.valid_to_utc > @AsOfUtc)
    ORDER BY t.valid_from_utc DESC;
END
GO


-- Get current members for a type semantic UID
CREATE PROCEDURE [dbo].[GetCurrentMembersByTypeSemanticUid]
    @TypeSemanticUid NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.*
    FROM dbo.api_member m
    JOIN dbo.api_type t ON m.api_type_id = t.id
    WHERE t.semantic_uid = @TypeSemanticUid
      AND t.is_active = 1
      AND t.valid_to_utc IS NULL
      AND m.is_active = 1
      AND m.valid_to_utc IS NULL
    ORDER BY m.name;
END
GO


-- Get member "as of" a given time by member semantic UID
CREATE PROCEDURE [dbo].[GetMemberAsOf]
    @MemberSemanticUid NVARCHAR(1000),
    @AsOfUtc           DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1) *
    FROM dbo.api_member m
    WHERE m.semantic_uid = @MemberSemanticUid
      AND m.valid_from_utc <= @AsOfUtc
      AND (m.valid_to_utc IS NULL OR m.valid_to_utc > @AsOfUtc)
    ORDER BY m.valid_from_utc DESC;
END
GO