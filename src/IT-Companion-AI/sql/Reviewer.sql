-- ============================================================
-- 6.2 REVIEW DB SCHEMA (SQL SERVER)
-- ============================================================

-- ============================================================
-- REVIEW RUN ROOT
-- ============================================================

CREATE TABLE review_run (
    id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    snapshot_id     UNIQUEIDENTIFIER NOT NULL,
    timestamp_utc   DATETIME2        NOT NULL,
    schema_version  NVARCHAR(200)    NOT NULL,

    CONSTRAINT fk_review_run_snapshot
        FOREIGN KEY (snapshot_id)
        REFERENCES source_snapshot(id)
        ON DELETE NO ACTION
);

CREATE INDEX idx_review_run_snapshot_id
    ON review_run(snapshot_id);

-- ============================================================
-- REVIEW ITEM
-- ============================================================

CREATE TABLE review_item (
    id             UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    review_run_id  UNIQUEIDENTIFIER NOT NULL,
    target_kind    NVARCHAR(50) NOT NULL,   -- sample | feature | doc | api_member
    target_uid     NVARCHAR(200) NOT NULL,  -- sample_uid, feature_uid, doc_uid, member_uid
    status         NVARCHAR(50),            -- approved | disputed | risky | deprecated
    summary        NVARCHAR(MAX),

    CONSTRAINT fk_review_item_run
        FOREIGN KEY (review_run_id)
        REFERENCES review_run(id)
        ON DELETE CASCADE
);

CREATE INDEX idx_review_item_run_id
    ON review_item(review_run_id);

CREATE INDEX idx_review_item_target_uid
    ON review_item(target_uid);

-- ============================================================
-- REVIEW ISSUE
-- ============================================================

CREATE TABLE review_issue (
    id                 UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    review_item_id     UNIQUEIDENTIFIER NOT NULL,
    code               NVARCHAR(200),
    severity           NVARCHAR(50),        -- info | warning | error
    related_member_uid NVARCHAR(200),
    details            NVARCHAR(MAX),

    CONSTRAINT fk_review_issue_item
        FOREIGN KEY (review_item_id)
        REFERENCES review_item(id)
        ON DELETE CASCADE
);

CREATE INDEX idx_review_issue_item_id
    ON review_issue(review_item_id);

CREATE INDEX idx_review_issue_related_member_uid
    ON review_issue(related_member_uid);