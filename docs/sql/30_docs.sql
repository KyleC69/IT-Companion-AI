-- =============================================================================
-- 30_docs.sql
-- Documentation: pages, sections, code blocks as temporal artifacts
-- =============================================================================
DROP TABLE IF EXISTS doc_section;
DROP TABLE IF EXISTS doc_page;


CREATE TABLE [dbo].[doc_page](
    [id]                       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_doc_page] PRIMARY KEY,
    [semantic_uid]             NVARCHAR(1000)   NOT NULL,
    [source_snapshot_id]       UNIQUEIDENTIFIER NOT NULL,
    [source_path]              NVARCHAR(MAX)    NULL,
    [title]                    NVARCHAR(400)    NULL,
    [language]                 NVARCHAR(200)    NULL,
    [url]                      NVARCHAR(MAX)    NULL,
    [raw_markdown]             NVARCHAR(MAX)    NULL,

    -- Temporal
    [version_number]           INT              NOT NULL,
    [created_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [updated_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [removed_ingestion_run_id] UNIQUEIDENTIFIER NULL,
    [valid_from_utc]           DATETIME2(7)     NOT NULL,
    [valid_to_utc]             DATETIME2(7)     NULL,
    [is_active]                BIT              NOT NULL CONSTRAINT [df_doc_page_is_active] DEFAULT (1),
    [content_hash]             BINARY(32)       NULL
);

ALTER TABLE dbo.doc_page
ADD semantic_uid_hash AS CAST(HASHBYTES('SHA2_256', semantic_uid) AS BINARY(32)) PERSISTED;



ALTER TABLE [dbo].[doc_page]
    ADD CONSTRAINT [df_doc_page_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[doc_page] WITH CHECK
    ADD CONSTRAINT [fk_doc_page_snapshot]
        FOREIGN KEY ([source_snapshot_id])
        REFERENCES [dbo].[source_snapshot]([id])
        ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE [dbo].[doc_page] WITH CHECK
    ADD CONSTRAINT [fk_doc_page_created_ingestion_run]
        FOREIGN KEY ([created_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[doc_page] WITH CHECK
    ADD CONSTRAINT [fk_doc_page_updated_ingestion_run]
        FOREIGN KEY ([updated_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[doc_page] WITH CHECK
    ADD CONSTRAINT [fk_doc_page_removed_ingestion_run]
        FOREIGN KEY ([removed_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id])
        ON DELETE NO ACTION ON UPDATE NO ACTION;





ALTER TABLE dbo.doc_page
WITH CHECK ADD CONSTRAINT fk_doc_page_semantic_identity
    FOREIGN KEY (semantic_uid_hash)
    REFERENCES dbo.semantic_identity(uid_hash)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION;





ALTER TABLE [dbo].[doc_page]
    ADD CONSTRAINT [uq_doc_page_semantic_version]
        UNIQUE ([semantic_uid], [version_number]);

CREATE INDEX [ix_doc_page_semantic_active]
    ON [dbo].[doc_page]([semantic_uid], [is_active]);

CREATE INDEX [ix_doc_page_semantic_valid_from]
    ON [dbo].[doc_page]([semantic_uid], [valid_from_utc]);

CREATE INDEX [ix_doc_page_content_hash]
    ON [dbo].[doc_page]([content_hash]);


CREATE TABLE [dbo].[doc_section](
    [id]                       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_doc_section] PRIMARY KEY,
    [doc_page_id]              UNIQUEIDENTIFIER NOT NULL,
    [semantic_uid]             NVARCHAR(1000)   NOT NULL,
    [heading]                  NVARCHAR(400)    NULL,
    [level]                    INT              NULL,
    [content_markdown]         NVARCHAR(MAX)    NULL,
    [order_index]              INT              NULL,

    -- Temporal
    [version_number]           INT              NOT NULL,
    [created_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [updated_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [removed_ingestion_run_id] UNIQUEIDENTIFIER NULL,
    [valid_from_utc]           DATETIME2(7)     NOT NULL,
    [valid_to_utc]             DATETIME2(7)     NULL,
    [is_active]                BIT              NOT NULL CONSTRAINT [df_doc_section_is_active] DEFAULT (1),
    [content_hash]             BINARY(32)       NULL
);
ALTER TABLE dbo.doc_section
ADD semantic_uid_hash AS CAST(HASHBYTES('SHA2_256', semantic_uid) AS BINARY(32)) PERSISTED;


ALTER TABLE [dbo].[doc_section]
    ADD CONSTRAINT [df_doc_section_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[doc_section] WITH CHECK
    ADD CONSTRAINT [fk_doc_section_page]
        FOREIGN KEY ([doc_page_id])
        REFERENCES [dbo].[doc_page]([id])
        ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE [dbo].[doc_section] WITH CHECK
    ADD CONSTRAINT [fk_doc_section_created_ingestion_run]
        FOREIGN KEY ([created_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[doc_section] WITH CHECK
    ADD CONSTRAINT [fk_doc_section_updated_ingestion_run]
        FOREIGN KEY ([updated_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[doc_section] WITH CHECK
    ADD CONSTRAINT [fk_doc_section_removed_ingestion_run]
        FOREIGN KEY ([removed_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id])
        ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.doc_section
WITH CHECK ADD CONSTRAINT fk_doc_section_semantic_identity
    FOREIGN KEY (semantic_uid_hash)
    REFERENCES dbo.semantic_identity(uid_hash)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION;

ALTER TABLE [dbo].[doc_section]
    ADD CONSTRAINT [uq_doc_section_semantic_version]
        UNIQUE ([semantic_uid], [version_number]);

CREATE INDEX [ix_doc_section_semantic_active]
    ON [dbo].[doc_section]([semantic_uid], [is_active]);

CREATE INDEX [ix_doc_section_doc_page_order]
    ON [dbo].[doc_section]([doc_page_id], [order_index]);

CREATE INDEX [ix_doc_section_content_hash]
    ON [dbo].[doc_section]([content_hash]);

    DROP TABLE IF EXISTS code_block;

CREATE TABLE [dbo].[code_block](
    [id]                       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_code_block] PRIMARY KEY,
    [doc_section_id]           UNIQUEIDENTIFIER NOT NULL,
    [semantic_uid]             NVARCHAR(1000)   NULL,
    [language]                 NVARCHAR(200)    NULL,
    [content]                  NVARCHAR(MAX)    NULL,
    [declared_packages]        NVARCHAR(MAX)    NULL,
    [tags]                     NVARCHAR(MAX)    NULL,
    [inline_comments]          NVARCHAR(MAX)    NULL,

    -- Temporal
    [version_number]           INT              NOT NULL,
    [created_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [updated_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [removed_ingestion_run_id] UNIQUEIDENTIFIER NULL,
    [valid_from_utc]           DATETIME2(7)     NOT NULL,
    [valid_to_utc]             DATETIME2(7)     NULL,
    [is_active]                BIT              NOT NULL CONSTRAINT [df_code_block_is_active] DEFAULT (1),
    [content_hash]             BINARY(32)       NULL
);
ALTER TABLE [dbo].[code_block]
    ADD CONSTRAINT [df_code_block_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[code_block] WITH CHECK
    ADD CONSTRAINT [fk_code_block_section]
        FOREIGN KEY ([doc_section_id])
        REFERENCES [dbo].[doc_section]([id])
       ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE [dbo].[code_block] WITH CHECK
    ADD CONSTRAINT [fk_code_block_created_ingestion_run]
        FOREIGN KEY ([created_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[code_block] WITH CHECK
    ADD CONSTRAINT [fk_code_block_updated_ingestion_run]
        FOREIGN KEY ([updated_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[code_block] WITH CHECK
    ADD CONSTRAINT [fk_code_block_removed_ingestion_run]
        FOREIGN KEY ([removed_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id])
        ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE UNIQUE NONCLUSTERED INDEX [ix_code_block_semantic_version]
    ON [dbo].[code_block]([semantic_uid], [version_number])
    WHERE [semantic_uid] IS NOT NULL;

CREATE INDEX [ix_code_block_content_hash]
    ON [dbo].[code_block]([content_hash]);