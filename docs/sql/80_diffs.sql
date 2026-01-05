-- =============================================================================
-- 80_diffs.sql
-- Diff tables for snapshot-level reporting
-- =============================================================================

CREATE TABLE [dbo].[api_type_diff](
    [id]               UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_api_type_diff] PRIMARY KEY,
    [snapshot_diff_id] UNIQUEIDENTIFIER NOT NULL,
    [type_uid]         NVARCHAR(1000)   NOT NULL,
    [change_kind]      NVARCHAR(200)    NULL,
    [detail_json]      NVARCHAR(MAX)    NULL
);
ALTER TABLE [dbo].[api_type_diff]
    ADD CONSTRAINT [df_api_type_diff_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[api_type_diff] WITH CHECK
    ADD CONSTRAINT [fk_api_type_diff_snapshot]
        FOREIGN KEY ([snapshot_diff_id])
        REFERENCES [dbo].[snapshot_diff]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_api_type_diff_type]
    ON [dbo].[api_type_diff]([type_uid]);


CREATE TABLE [dbo].[api_member_diff](
    [id]               UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_api_member_diff] PRIMARY KEY,
    [snapshot_diff_id] UNIQUEIDENTIFIER NOT NULL,
    [member_uid]       NVARCHAR(1000)   NOT NULL,
    [change_kind]      NVARCHAR(200)    NULL,
    [old_signature]    NVARCHAR(MAX)    NULL,
    [new_signature]    NVARCHAR(MAX)    NULL,
    [breaking]         BIT              NULL,
    [detail_json]      NVARCHAR(MAX)    NULL
);
ALTER TABLE [dbo].[api_member_diff]
    ADD CONSTRAINT [df_api_member_diff_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[api_member_diff] WITH CHECK
    ADD CONSTRAINT [fk_api_member_diff_snapshot]
        FOREIGN KEY ([snapshot_diff_id])
        REFERENCES [dbo].[snapshot_diff]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_api_member_diff_member]
    ON [dbo].[api_member_diff]([member_uid]);


CREATE TABLE [dbo].[doc_page_diff](
    [id]               UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_doc_page_diff] PRIMARY KEY,
    [snapshot_diff_id] UNIQUEIDENTIFIER NOT NULL,
    [doc_uid]          NVARCHAR(1000)   NOT NULL,
    [change_kind]      NVARCHAR(200)    NULL,
    [detail_json]      NVARCHAR(MAX)    NULL
);
ALTER TABLE [dbo].[doc_page_diff]
    ADD CONSTRAINT [df_doc_page_diff_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[doc_page_diff] WITH CHECK
    ADD CONSTRAINT [fk_doc_page_diff_snapshot]
        FOREIGN KEY ([snapshot_diff_id])
        REFERENCES [dbo].[snapshot_diff]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_doc_page_diff_doc]
    ON [dbo].[doc_page_diff]([doc_uid]);