-- =============================================================================
-- 00_core.sql
-- Core run + snapshot infrastructure (commit / snapshot model)
-- =============================================================================

CREATE TABLE [dbo].[ingestion_run](
    [id]             UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_ingestion_run] PRIMARY KEY,
    [timestamp_utc]  DATETIME2(7)     NOT NULL,
    [schema_version] NVARCHAR(200)    NOT NULL,
    [notes]          NVARCHAR(MAX)    NULL
);
ALTER TABLE [dbo].[ingestion_run]
    ADD CONSTRAINT [df_ingestion_run_id] DEFAULT (NEWID()) FOR [id];


CREATE TABLE [dbo].[source_snapshot](
    [id]               UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_source_snapshot] PRIMARY KEY,
    [ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [snapshot_uid]     NVARCHAR(1000)   NOT NULL,
    [repo_url]         NVARCHAR(MAX)    NULL,
    [branch]           NVARCHAR(200)    NULL,
    [repo_commit]      NVARCHAR(200)    NULL,
    [language]         NVARCHAR(200)    NULL,
    [package_name]     NVARCHAR(200)    NULL,
    [package_version]  NVARCHAR(200)    NULL,
    [config_json]      NVARCHAR(MAX)    NULL
   
);


ALTER TABLE [dbo].[source_snapshot]
    ADD CONSTRAINT [df_source_snapshot_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[source_snapshot] WITH CHECK
    ADD CONSTRAINT [fk_source_snapshot_ingestion_run]
        FOREIGN KEY ([ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id])
        ON DELETE CASCADE;

ALTER TABLE [dbo].[source_snapshot]
    ADD CONSTRAINT [uq_source_snapshot_uid] UNIQUE ([snapshot_uid]);

CREATE INDEX [ix_source_snapshot_ingestion_run]
    ON [dbo].[source_snapshot]([ingestion_run_id], [snapshot_uid]);


CREATE TABLE [dbo].[truth_run](
    [id]             UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_truth_run] PRIMARY KEY,
    [snapshot_id]    UNIQUEIDENTIFIER NOT NULL,
    [timestamp_utc]  DATETIME2(7)     NOT NULL,
    [schema_version] NVARCHAR(200)    NOT NULL
);
ALTER TABLE [dbo].[truth_run]
    ADD CONSTRAINT [df_truth_run_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[truth_run] WITH CHECK
    ADD CONSTRAINT [fk_truth_run_snapshot]
        FOREIGN KEY ([snapshot_id])
        REFERENCES [dbo].[source_snapshot]([id]);

CREATE INDEX [ix_truth_run_snapshot_time]
    ON [dbo].[truth_run]([snapshot_id], [timestamp_utc]);


CREATE TABLE [dbo].[sample_run](
    [id]             UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_sample_run] PRIMARY KEY,
    [snapshot_id]    UNIQUEIDENTIFIER NOT NULL,
    [timestamp_utc]  DATETIME2(7)     NOT NULL,
    [schema_version] NVARCHAR(200)    NOT NULL
);
ALTER TABLE [dbo].[sample_run]
    ADD CONSTRAINT [df_sample_run_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[sample_run] WITH CHECK
    ADD CONSTRAINT [fk_sample_run_snapshot]
        FOREIGN KEY ([snapshot_id])
        REFERENCES [dbo].[source_snapshot]([id]);

CREATE INDEX [ix_sample_run_snapshot_time]
    ON [dbo].[sample_run]([snapshot_id], [timestamp_utc]);


CREATE TABLE [dbo].[execution_run](
    [id]               UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_execution_run] PRIMARY KEY,
    [snapshot_id]      UNIQUEIDENTIFIER NOT NULL,
    [sample_run_id]    UNIQUEIDENTIFIER NOT NULL,
    [timestamp_utc]    DATETIME2(7)     NOT NULL,
    [environment_json] NVARCHAR(MAX)    NULL,
    [schema_version]   NVARCHAR(200)    NOT NULL
);
ALTER TABLE [dbo].[execution_run]
    ADD CONSTRAINT [df_execution_run_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[execution_run] WITH CHECK
    ADD CONSTRAINT [fk_execution_run_snapshot]
        FOREIGN KEY ([snapshot_id])
        REFERENCES [dbo].[source_snapshot]([id]);

ALTER TABLE [dbo].[execution_run] WITH CHECK
    ADD CONSTRAINT [fk_execution_run_sample_run]
        FOREIGN KEY ([sample_run_id])
        REFERENCES [dbo].[sample_run]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_execution_run_snapshot_time]
    ON [dbo].[execution_run]([snapshot_id], [timestamp_utc]);


CREATE TABLE [dbo].[review_run](
    [id]             UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_review_run] PRIMARY KEY,
    [snapshot_id]    UNIQUEIDENTIFIER NOT NULL,
    [timestamp_utc]  DATETIME2(7)     NOT NULL,
    [schema_version] NVARCHAR(200)    NOT NULL
);
ALTER TABLE [dbo].[review_run]
    ADD CONSTRAINT [df_review_run_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[review_run] WITH CHECK
    ADD CONSTRAINT [fk_review_run_snapshot]
        FOREIGN KEY ([snapshot_id])
        REFERENCES [dbo].[source_snapshot]([id]);

CREATE INDEX [ix_review_run_snapshot_time]
    ON [dbo].[review_run]([snapshot_id], [timestamp_utc]);


CREATE TABLE [dbo].[rag_run](
    [id]             UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_rag_run] PRIMARY KEY,
    [snapshot_id]    UNIQUEIDENTIFIER NOT NULL,
    [timestamp_utc]  DATETIME2(7)     NOT NULL,
    [schema_version] NVARCHAR(200)    NOT NULL
);
ALTER TABLE [dbo].[rag_run]
    ADD CONSTRAINT [df_rag_run_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[rag_run] WITH CHECK
    ADD CONSTRAINT [fk_rag_run_snapshot]
        FOREIGN KEY ([snapshot_id])
        REFERENCES [dbo].[source_snapshot]([id]);

CREATE INDEX [ix_rag_run_snapshot_time]
    ON [dbo].[rag_run]([snapshot_id], [timestamp_utc]);


CREATE TABLE [dbo].[snapshot_diff](
    [id]              UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_snapshot_diff] PRIMARY KEY,
    [old_snapshot_id] UNIQUEIDENTIFIER NOT NULL,
    [new_snapshot_id] UNIQUEIDENTIFIER NOT NULL,
    [timestamp_utc]   DATETIME2(7)     NOT NULL,
    [schema_version]  NVARCHAR(200)    NOT NULL
);
ALTER TABLE [dbo].[snapshot_diff]
    ADD CONSTRAINT [df_snapshot_diff_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[snapshot_diff] WITH CHECK
    ADD CONSTRAINT [fk_snapshot_diff_old]
        FOREIGN KEY ([old_snapshot_id])
        REFERENCES [dbo].[source_snapshot]([id]);

ALTER TABLE [dbo].[snapshot_diff] WITH CHECK
    ADD CONSTRAINT [fk_snapshot_diff_new]
        FOREIGN KEY ([new_snapshot_id])
        REFERENCES [dbo].[source_snapshot]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_snapshot_diff_old_new]
    ON [dbo].[snapshot_diff]([old_snapshot_id], [new_snapshot_id]);