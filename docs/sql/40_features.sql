-- =============================================================================
-- 40_features.sql
-- Features as temporal artifacts + their links
-- =============================================================================

DROP TABLE IF EXISTS api_feature;

CREATE TABLE [dbo].[api_feature](
    [id]                       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_api_feature] PRIMARY KEY,
    [semantic_uid]             NVARCHAR(1000)   NOT NULL,
    [truth_run_id]             UNIQUEIDENTIFIER NOT NULL,
    [name]                     NVARCHAR(400)    NULL,
    [language]                 NVARCHAR(200)    NULL,
    [description]              NVARCHAR(MAX)    NULL,
    [tags]                     NVARCHAR(MAX)    NULL,

    -- Temporal
    [version_number]           INT              NOT NULL,
    [created_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [updated_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [removed_ingestion_run_id] UNIQUEIDENTIFIER NULL,
    [valid_from_utc]           DATETIME2(7)     NOT NULL,
    [valid_to_utc]             DATETIME2(7)     NULL,
    [is_active]                BIT              NOT NULL CONSTRAINT [df_api_feature_is_active] DEFAULT (1),
    [content_hash]             BINARY(32)       NULL
);



ALTER TABLE dbo.api_feature
ADD semantic_uid_hash AS CAST(HASHBYTES('SHA2_256', semantic_uid) AS BINARY(32)) PERSISTED;



ALTER TABLE [dbo].[api_feature]
    ADD CONSTRAINT [df_api_feature_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[api_feature] WITH CHECK
    ADD CONSTRAINT [fk_api_feature_truth_run]
        FOREIGN KEY ([truth_run_id])
        REFERENCES [dbo].[truth_run]([id])
        ON DELETE CASCADE;

ALTER TABLE [dbo].[api_feature] WITH CHECK
    ADD CONSTRAINT [fk_api_feature_created_ingestion_run]
        FOREIGN KEY ([created_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[api_feature] WITH CHECK
    ADD CONSTRAINT [fk_api_feature_updated_ingestion_run]
        FOREIGN KEY ([updated_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[api_feature] WITH CHECK
    ADD CONSTRAINT [fk_api_feature_removed_ingestion_run]
        FOREIGN KEY ([removed_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id])
        ON DELETE SET NULL;




ALTER TABLE dbo.api_feature
WITH CHECK ADD CONSTRAINT fk_api_feature_semantic_identity
    FOREIGN KEY (semantic_uid_hash)
    REFERENCES dbo.semantic_identity(uid_hash)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION;






ALTER TABLE [dbo].[api_feature]
    ADD CONSTRAINT [uq_api_feature_semantic_version]
        UNIQUE ([semantic_uid], [version_number]);

CREATE INDEX [ix_api_feature_semantic_active]
    ON [dbo].[api_feature]([semantic_uid], [is_active]);

CREATE INDEX [ix_api_feature_content_hash]
    ON [dbo].[api_feature]([content_hash]);


CREATE TABLE [dbo].[feature_doc_link](
    [id]          UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_feature_doc_link] PRIMARY KEY,
    [feature_id]  UNIQUEIDENTIFIER NOT NULL,
    [doc_uid]     NVARCHAR(1000)   NOT NULL,
    [section_uid] NVARCHAR(1000)   NULL
);
ALTER TABLE [dbo].[feature_doc_link]
    ADD CONSTRAINT [df_feature_doc_link_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[feature_doc_link] WITH CHECK
    ADD CONSTRAINT [fk_feature_doc_link_feature]
        FOREIGN KEY ([feature_id])
        REFERENCES [dbo].[api_feature]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_feature_doc_link_feature]
    ON [dbo].[feature_doc_link]([feature_id]);

CREATE INDEX [ix_feature_doc_link_doc]
    ON [dbo].[feature_doc_link]([doc_uid], [section_uid]);


CREATE TABLE [dbo].[feature_member_link](
    [id]         UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_feature_member_link] PRIMARY KEY,
    [feature_id] UNIQUEIDENTIFIER NOT NULL,
    [member_uid] NVARCHAR(1000)   NOT NULL,
    [role]       NVARCHAR(50)     NULL
);
ALTER TABLE [dbo].[feature_member_link]
    ADD CONSTRAINT [df_feature_member_link_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[feature_member_link] WITH CHECK
    ADD CONSTRAINT [fk_feature_member_link_feature]
        FOREIGN KEY ([feature_id])
        REFERENCES [dbo].[api_feature]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_feature_member_link_feature]
    ON [dbo].[feature_member_link]([feature_id]);

CREATE INDEX [ix_feature_member_link_member]
    ON [dbo].[feature_member_link]([member_uid]);


CREATE TABLE [dbo].[feature_type_link](
    [id]         UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_feature_type_link] PRIMARY KEY,
    [feature_id] UNIQUEIDENTIFIER NOT NULL,
    [type_uid]   NVARCHAR(1000)   NOT NULL,
    [role]       NVARCHAR(50)     NULL
);
ALTER TABLE [dbo].[feature_type_link]
    ADD CONSTRAINT [df_feature_type_link_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[feature_type_link] WITH CHECK
    ADD CONSTRAINT [fk_feature_type_link_feature]
        FOREIGN KEY ([feature_id])
        REFERENCES [dbo].[api_feature]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_feature_type_link_feature]
    ON [dbo].[feature_type_link]([feature_id]);

CREATE INDEX [ix_feature_type_link_type]
    ON [dbo].[feature_type_link]([type_uid]);