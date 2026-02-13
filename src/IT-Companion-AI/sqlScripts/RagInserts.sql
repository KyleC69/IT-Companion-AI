DECLARE
@SnapshotId UNIQUEIDENTIFIER = '6538B927-95B0-4780-A670-6F59FC2C3E78';
DECLARE
@RagRunId   UNIQUEIDENTIFIER = 'DF4DE650-F527-4950-916F-9C9F9A4D2BC4';

;
WITH ActivePages AS (SELECT p.*
                     FROM dbo.doc_page p
                     WHERE p.source_snapshot_id = @SnapshotId
                       AND p.is_active = 1),
     ActiveSections AS (SELECT s.*
                        FROM dbo.doc_section s
                                 JOIN ActivePages p ON p.id = s.doc_page_id
                        WHERE s.is_active = 1),
     ActiveCodeBlocks AS (SELECT cb.*
                          FROM dbo.code_block cb
                                   JOIN ActiveSections s ON s.id = cb.doc_section_id
                          WHERE cb.is_active = 1)

INSERT
INTO dbo.rag_chunk (
      id
    , rag_run_id
    , chunk_uid
    , kind
    , text
    , metadata_json
    , content_type
)
SELECT NEWID()          AS id,
       @RagRunId        AS rag_run_id,
       s.semantic_uid   AS chunk_uid,
       N'learn.section' AS kind,

       -- merged text: heading + markdown + code blocks
       LTRIM(RTRIM(
               CONCAT(
                       CASE
                           WHEN s.heading IS NOT NULL
                               THEN CONCAT(REPLICATE('#', ISNULL(s.level, 2)), ' ', s.heading, CHAR(13)+CHAR(10)+CHAR(13)+CHAR(10))
                           ELSE N'' END,
                       ISNULL(s.content_markdown, N''),
                       CASE
                           WHEN EXISTS (SELECT 1 FROM ActiveCodeBlocks cb2 WHERE cb2.doc_section_id = s.id)
                               THEN CHAR (13)+ CHAR (10)+
                           STRING_AGG(
                           CONCAT(
                           '```', ISNULL(cb.language, N''), CHAR (13)+ CHAR (10),
                           ISNULL(cb.content, N''), CHAR (13)+ CHAR (10),
                           '```', CHAR (13)+ CHAR (10)
                           ),
                           CHAR (13)+ CHAR (10)
                           ) WITHIN GROUP (ORDER BY cb.id)
                           ELSE N''
                           END
               )
             )) AS [text],

    -- metadata_json
    (SELECT 
        'microsoft-learn' AS [source],
        p.semantic_uid AS pageSemanticUid,
        p.title AS pageTitle,
        p.url AS pageUrl,
        s.heading AS sectionHeading,
        s.level AS sectionLevel,
        s.order_index AS sectionOrder
     FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)  AS metadata_json,

    N'markdown' AS content_type

FROM ActiveSections s
    JOIN ActivePages p
ON p.id = s.doc_page_id
    LEFT JOIN ActiveCodeBlocks cb
    ON cb.doc_section_id = s.id
GROUP BY
    p.semantic_uid,
    p.title,
    p.url,
    s.id,
    s.semantic_uid,
    s.heading,
    s.level,
    s.order_index,
    s.content_markdown;

/*
DECLARE @SnapshotId UNIQUEIDENTIFIER = '6538B927-95B0-4780-A670-6F59FC2C3E78';
BEGIN

    INSERT INTO rag_run (id, snapshot_id, timestamp_utc, schema_version) 
    VALUES (NEWID(), @SnapshotId, GETDATE(), '1.1');
    
    SELECT SCOPE_IDENTITY() AS LastInsertedId;

END

*/