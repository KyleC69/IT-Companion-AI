/* =============================================================================
   VerifyIngestionRun
   Full ingestion integrity validator
   ============================================================================= */

IF OBJECT_ID('dbo.VerifyIngestionRun', 'P') IS NOT NULL
    DROP PROCEDURE dbo.VerifyIngestionRun;
GO

CREATE PROCEDURE [dbo].[VerifyIngestionRun]
    @IngestionRunId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Issues TABLE
    (
        category NVARCHAR(200),
        detail   NVARCHAR(MAX)
    );

    /* -------------------------------------------------------------------------
       1. Validate ingestion_run exists
       ------------------------------------------------------------------------- */
    IF NOT EXISTS (SELECT 1 FROM dbo.ingestion_run WHERE id = @IngestionRunId)
    BEGIN
        INSERT INTO @Issues VALUES ('fatal', 'Ingestion run does not exist.');
        SELECT * FROM @Issues;
        RETURN;
    END

    /* -------------------------------------------------------------------------
       2. Validate snapshot exists
       ------------------------------------------------------------------------- */
    IF NOT EXISTS (SELECT 1 FROM dbo.source_snapshot WHERE ingestion_run_id = @IngestionRunId)
        INSERT INTO @Issues VALUES ('snapshot', 'No source_snapshot created for this ingestion run.');

    /* -------------------------------------------------------------------------
       3. Validate truth_run exists
       ------------------------------------------------------------------------- */
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.truth_run tr
        JOIN dbo.source_snapshot ss ON ss.id = tr.snapshot_id
        WHERE ss.ingestion_run_id = @IngestionRunId
    )
        INSERT INTO @Issues VALUES ('truth_run', 'No truth_run created for this ingestion run.');

    /* -------------------------------------------------------------------------
       4. Check temporal tables for rows created in this run
       ------------------------------------------------------------------------- */
    DECLARE @TemporalTables TABLE (name NVARCHAR(200));
    INSERT INTO @TemporalTables VALUES
        ('api_type'),
        ('api_member'),
        ('doc_page'),
        ('api_feature');

    DECLARE @tbl NVARCHAR(200);
    DECLARE cur CURSOR FOR SELECT name FROM @TemporalTables;
    OPEN cur;
    FETCH NEXT FROM cur INTO @tbl;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        DECLARE @sql NVARCHAR(MAX) =
            N'SELECT CASE WHEN EXISTS (SELECT 1 FROM dbo.' + @tbl +
            N' WHERE created_ingestion_run_id = @IngestionRunId) THEN 1 ELSE 0 END';

        DECLARE @hasRows BIT;
        EXEC sp_executesql @sql, N'@IngestionRunId UNIQUEIDENTIFIER, @hasRows BIT OUTPUT',
            @IngestionRunId=@IngestionRunId, @hasRows=@hasRows OUTPUT;

        IF @hasRows = 0
            INSERT INTO @Issues VALUES ('temporal', 'No rows created in ' + @tbl + ' for this ingestion run.');

        FETCH NEXT FROM cur INTO @tbl;
    END

    CLOSE cur;
    DEALLOCATE cur;

    /* -------------------------------------------------------------------------
       5. Semantic identity missing
       ------------------------------------------------------------------------- */
    INSERT INTO @Issues
    SELECT 'semantic_identity',
           'Missing semantic identity for UID: ' + t.semantic_uid
    FROM (
        SELECT semantic_uid FROM dbo.api_type
        UNION ALL SELECT semantic_uid FROM dbo.api_member
        UNION ALL SELECT semantic_uid FROM dbo.doc_page
        UNION ALL SELECT semantic_uid FROM dbo.api_feature
    ) t
    WHERE NOT EXISTS (SELECT 1 FROM dbo.semantic_identity si WHERE si.uid = t.semantic_uid);

    /* -------------------------------------------------------------------------
       6. Temporal inconsistencies
       ------------------------------------------------------------------------- */
    INSERT INTO @Issues
    SELECT 'temporal_overlap',
           CONCAT('Overlapping versions in api_type for UID ', semantic_uid)
    FROM (
        SELECT semantic_uid, version_number, valid_from_utc, valid_to_utc,
               LEAD(valid_from_utc) OVER (PARTITION BY semantic_uid ORDER BY valid_from_utc) AS next_from
        FROM dbo.api_type
    ) x
    WHERE valid_to_utc IS NOT NULL AND next_from IS NOT NULL AND valid_to_utc > next_from;

    INSERT INTO @Issues
    SELECT 'temporal_overlap',
           CONCAT('Overlapping versions in api_member for UID ', semantic_uid)
    FROM (
        SELECT semantic_uid, version_number, valid_from_utc, valid_to_utc,
               LEAD(valid_from_utc) OVER (PARTITION BY semantic_uid ORDER BY valid_from_utc) AS next_from
        FROM dbo.api_member
    ) x
    WHERE valid_to_utc IS NOT NULL AND next_from IS NOT NULL AND valid_to_utc > next_from;

    /* -------------------------------------------------------------------------
       7. Multiple active versions
       ------------------------------------------------------------------------- */
    INSERT INTO @Issues
    SELECT 'multiple_active',
           CONCAT('Multiple active versions for UID ', semantic_uid, ' in api_type')
    FROM dbo.api_type
    WHERE is_active = 1
    GROUP BY semantic_uid
    HAVING COUNT(*) > 1;

    INSERT INTO @Issues
    SELECT 'multiple_active',
           CONCAT('Multiple active versions for UID ', semantic_uid, ' in api_member')
    FROM dbo.api_member
    WHERE is_active = 1
    GROUP BY semantic_uid
    HAVING COUNT(*) > 1;

    /* -------------------------------------------------------------------------
       8. Orphaned links
       ------------------------------------------------------------------------- */
    INSERT INTO @Issues
    SELECT 'orphan_feature_doc_link',
           CONCAT('feature_doc_link references missing feature_id ', CAST(fdl.feature_id AS NVARCHAR(100)))
    FROM dbo.feature_doc_link fdl
    WHERE NOT EXISTS (SELECT 1 FROM dbo.api_feature f WHERE f.id = fdl.feature_id);

    INSERT INTO @Issues
    SELECT 'orphan_feature_member_link',
           CONCAT('feature_member_link references missing feature_id ', CAST(fml.feature_id AS NVARCHAR(100)))
    FROM dbo.feature_member_link fml
    WHERE NOT EXISTS (SELECT 1 FROM dbo.api_feature f WHERE f.id = fml.feature_id);

    /* -------------------------------------------------------------------------
       9. Missing content_hash
       ------------------------------------------------------------------------- */
    INSERT INTO @Issues
    SELECT 'missing_hash', CONCAT('Missing content_hash in api_type id ', CAST(id AS NVARCHAR(100)))
    FROM dbo.api_type WHERE content_hash IS NULL;

    INSERT INTO @Issues
    SELECT 'missing_hash', CONCAT('Missing content_hash in api_member id ', CAST(id AS NVARCHAR(100)))
    FROM dbo.api_member WHERE content_hash IS NULL;

    /* -------------------------------------------------------------------------
       10. Missing version_number
       ------------------------------------------------------------------------- */
    INSERT INTO @Issues
    SELECT 'missing_version', CONCAT('Missing version_number in api_type id ', CAST(id AS NVARCHAR(100)))
    FROM dbo.api_type WHERE version_number IS NULL;

    /* -------------------------------------------------------------------------
       11. Missing valid_from_utc
       ------------------------------------------------------------------------- */
    INSERT INTO @Issues
    SELECT 'missing_valid_from', CONCAT('Missing valid_from_utc in api_type id ', CAST(id AS NVARCHAR(100)))
    FROM dbo.api_type WHERE valid_from_utc IS NULL;

    /* -------------------------------------------------------------------------
       Return results
       ------------------------------------------------------------------------- */
    IF NOT EXISTS (SELECT 1 FROM @Issues)
        INSERT INTO @Issues VALUES ('ok', 'Ingestion run is fully consistent.');

    SELECT * FROM @Issues ORDER BY category, detail;
END
GO