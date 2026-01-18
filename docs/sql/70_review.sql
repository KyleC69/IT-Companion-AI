-- =============================================================================
-- 70_review.sql
-- Review runs, items, and issues
-- =============================================================================

CREATE TABLE [dbo].[review_item](
    [id]            UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_review_item] PRIMARY KEY,
    [review_run_id] UNIQUEIDENTIFIER NOT NULL,
    [target_kind]   NVARCHAR(50)     NOT NULL,
    [target_uid]    NVARCHAR(1000)   NOT NULL,
    [status]        NVARCHAR(50)     NULL,
    [summary]       NVARCHAR(MAX)    NULL
);
ALTER TABLE [dbo].[review_item]
    ADD CONSTRAINT [df_review_item_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[review_item] WITH CHECK
    ADD CONSTRAINT [fk_review_item_run]
        FOREIGN KEY ([review_run_id])
        REFERENCES [dbo].[review_run]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_review_item_target]
    ON [dbo].[review_item]([target_kind], [target_uid]);


CREATE TABLE [dbo].[review_issue](
    [id]               UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_review_issue] PRIMARY KEY,
    [review_item_id]   UNIQUEIDENTIFIER NOT NULL,
    [code]             NVARCHAR(200)    NULL,
    [severity]         NVARCHAR(50)     NULL,
    [related_member_uid] NVARCHAR(1000) NULL,
    [details]          NVARCHAR(MAX)    NULL
);
ALTER TABLE [dbo].[review_issue]
    ADD CONSTRAINT [df_review_issue_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[review_issue] WITH CHECK
    ADD CONSTRAINT [fk_review_issue_item]
        FOREIGN KEY ([review_item_id])
        REFERENCES [dbo].[review_item]([id])
        ON DELETE CASCADE;

CREATE INDEX [ix_review_issue_severity]
    ON [dbo].[review_issue]([severity]);