-- =============================================================================
-- 50_samples_execution.sql
-- Samples and execution results (non-temporal artifacts for now)
-- =============================================================================

CREATE TABLE [dbo].[sample](
    [id]                    UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_sample] PRIMARY KEY,
    [sample_run_id]         UNIQUEIDENTIFIER NOT NULL,
    [sample_uid]            NVARCHAR(1000)   NOT NULL,
    [feature_uid]           NVARCHAR(1000)   NULL,
    [language]              NVARCHAR(200)    NULL,
    [code]                  NVARCHAR(MAX)    NULL,
    [entry_point]           NVARCHAR(400)    NULL,
    [target_framework]      NVARCHAR(200)    NULL,
    [package_references]    NVARCHAR(MAX)    NULL,
    [derived_from_code_uid] NVARCHAR(1000)   NULL,
    [tags]                  NVARCHAR(MAX)    NULL
);
ALTER TABLE [dbo].[sample]
    ADD CONSTRAINT [df_sample_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[sample] WITH CHECK
    ADD CONSTRAINT [fk_sample_sample_run]
        FOREIGN KEY ([sample_run_id])
        REFERENCES [dbo].[sample_run]([id])
        ON DELETE CASCADE;

ALTER TABLE [dbo].[sample]
    ADD CONSTRAINT [uq_sample_uid] UNIQUE ([sample_uid]);

CREATE INDEX [ix_sample_feature_uid]
    ON [dbo].[sample]([feature_uid]);


CREATE TABLE [dbo].[execution_result](
    [id]              UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_execution_result] PRIMARY KEY,
    [execution_run_id] UNIQUEIDENTIFIER NOT NULL,
    [sample_uid]      NVARCHAR(1000)   NOT NULL,
    [status]          NVARCHAR(100)    NULL,
    [build_log]       NVARCHAR(MAX)    NULL,
    [run_log]         NVARCHAR(MAX)    NULL,
    [exception_json]  NVARCHAR(MAX)    NULL,
    [duration_ms]     INT              NULL
);
ALTER TABLE [dbo].[execution_result]
    ADD CONSTRAINT [df_execution_result_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[execution_result] WITH CHECK
    ADD CONSTRAINT [fk_execution_result_run]
        FOREIGN KEY ([execution_run_id])
        REFERENCES [dbo].[execution_run]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_execution_result_sample_status]
    ON [dbo].[execution_result]([sample_uid], [status]);


CREATE TABLE [dbo].[sample_api_member_link](
    [id]         UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_sample_api_member_link] PRIMARY KEY,
    [sample_id]  UNIQUEIDENTIFIER NOT NULL,
    [member_uid] NVARCHAR(1000)   NOT NULL
);
ALTER TABLE [dbo].[sample_api_member_link]
    ADD CONSTRAINT [df_sample_api_member_link_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[sample_api_member_link] WITH CHECK
    ADD CONSTRAINT [fk_sample_api_member_link_sample]
        FOREIGN KEY ([sample_id])
        REFERENCES [dbo].[sample]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_sample_api_member_link_member]
    ON [dbo].[sample_api_member_link]([member_uid]);