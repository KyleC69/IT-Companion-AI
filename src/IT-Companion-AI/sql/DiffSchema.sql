-- ============================================================
-- 3.2 DIFF DB SCHEMA (SQL SERVER)
-- ============================================================

-- ============================================================
-- SNAPSHOT DIFF ROOT
-- ============================================================

CREATE TABLE snapshot_diff
(
    id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    old_snapshot_id UNIQUEIDENTIFIER NOT NULL,
    new_snapshot_id UNIQUEIDENTIFIER NOT NULL,
    timestamp_utc   DATETIME2        NOT NULL,
    schema_version  NVARCHAR(200)    NOT NULL,

    -- Do NOT cascade on old_snapshot_id
    CONSTRAINT fk_snapshot_diff_old
        FOREIGN KEY (old_snapshot_id)
            REFERENCES source_snapshot (id)
            ON DELETE NO ACTION,

    -- Cascade only on new_snapshot_id
    CONSTRAINT fk_snapshot_diff_new
        FOREIGN KEY (new_snapshot_id)
            REFERENCES source_snapshot (id)
            ON DELETE CASCADE
);


CREATE INDEX idx_snapshot_diff_old_snapshot_id
    ON snapshot_diff (old_snapshot_id);

CREATE INDEX idx_snapshot_diff_new_snapshot_id
    ON snapshot_diff (new_snapshot_id);

-- ============================================================
-- API TYPE DIFF
-- ============================================================

CREATE TABLE api_type_diff
(
    id               UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    snapshot_diff_id UNIQUEIDENTIFIER NOT NULL,
    type_uid         NVARCHAR(200)    NOT NULL,
    change_kind      NVARCHAR(200),
    detail_json      NVARCHAR(MAX),

    CONSTRAINT fk_api_type_diff_snapshot
        FOREIGN KEY (snapshot_diff_id)
            REFERENCES snapshot_diff (id)
            ON DELETE CASCADE
);

CREATE INDEX idx_api_type_diff_snapshot_id
    ON api_type_diff (snapshot_diff_id);

-- ============================================================
-- API MEMBER DIFF
-- ============================================================

CREATE TABLE api_member_diff
(
    id               UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    snapshot_diff_id UNIQUEIDENTIFIER NOT NULL,
    member_uid       NVARCHAR(200)    NOT NULL,
    change_kind      NVARCHAR(200),
    old_signature    NVARCHAR(MAX),
    new_signature    NVARCHAR(MAX),
    breaking         BIT,
    detail_json      NVARCHAR(MAX),

    CONSTRAINT fk_api_member_diff_snapshot
        FOREIGN KEY (snapshot_diff_id)
            REFERENCES snapshot_diff (id)
            ON DELETE CASCADE
);

CREATE INDEX idx_api_member_diff_snapshot_id
    ON api_member_diff (snapshot_diff_id);

-- ============================================================
-- DOC PAGE DIFF
-- ============================================================

CREATE TABLE doc_page_diff
(
    id               UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    snapshot_diff_id UNIQUEIDENTIFIER NOT NULL,
    doc_uid          NVARCHAR(200)    NOT NULL,
    change_kind      NVARCHAR(200),
    detail_json      NVARCHAR(MAX),

    CONSTRAINT fk_doc_page_diff_snapshot
        FOREIGN KEY (snapshot_diff_id)
            REFERENCES snapshot_diff (id)
            ON DELETE CASCADE
);

CREATE INDEX idx_doc_page_diff_snapshot_id
    ON doc_page_diff (snapshot_diff_id);