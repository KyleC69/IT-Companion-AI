-- ============================================================
-- 4.2 TRUTH DB SCHEMA (SQL SERVER)
-- ============================================================

-- ============================================================
-- TRUTH RUN ROOT
-- ============================================================

CREATE TABLE truth_run
(
    id             UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    snapshot_id    UNIQUEIDENTIFIER NOT NULL,
    timestamp_utc  DATETIME2        NOT NULL,
    schema_version NVARCHAR(200)    NOT NULL,

    CONSTRAINT fk_truth_run_snapshot
        FOREIGN KEY (snapshot_id)
            REFERENCES source_snapshot (id)
            ON DELETE NO ACTION
);

CREATE INDEX idx_truth_run_snapshot_id
    ON truth_run (snapshot_id);

-- ============================================================
-- FEATURE
-- ============================================================

CREATE TABLE feature
(
    id                    UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    truth_run_id          UNIQUEIDENTIFIER NOT NULL,
    feature_uid           NVARCHAR(200)    NOT NULL,
    name                  NVARCHAR(400),
    language              NVARCHAR(200),
    description           NVARCHAR(MAX),
    tags                  NVARCHAR(MAX), -- JSON array
    introduced_in_version NVARCHAR(200),
    last_seen_version     NVARCHAR(200),

    CONSTRAINT fk_feature_truth_run
        FOREIGN KEY (truth_run_id)
            REFERENCES truth_run (id)
            ON DELETE CASCADE,

    CONSTRAINT uq_feature_uid UNIQUE (feature_uid)
);

CREATE INDEX idx_feature_truth_run_id
    ON feature (truth_run_id);

-- ============================================================
-- FEATURE → TYPE LINK
-- ============================================================

CREATE TABLE feature_type_link
(
    id         UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    feature_id UNIQUEIDENTIFIER NOT NULL,
    type_uid   NVARCHAR(200)    NOT NULL,
    role       NVARCHAR(50), -- primary | related

    CONSTRAINT fk_feature_type_link_feature
        FOREIGN KEY (feature_id)
            REFERENCES feature (id)
            ON DELETE CASCADE
);

CREATE INDEX idx_feature_type_link_feature_id
    ON feature_type_link (feature_id);

-- ============================================================
-- FEATURE → MEMBER LINK
-- ============================================================

CREATE TABLE feature_member_link
(
    id         UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    feature_id UNIQUEIDENTIFIER NOT NULL,
    member_uid NVARCHAR(200)    NOT NULL,
    role       NVARCHAR(50), -- primary | helper

    CONSTRAINT fk_feature_member_link_feature
        FOREIGN KEY (feature_id)
            REFERENCES feature (id)
            ON DELETE CASCADE
);

CREATE INDEX idx_feature_member_link_feature_id
    ON feature_member_link (feature_id);

-- ============================================================
-- FEATURE → DOC LINK
-- ============================================================

CREATE TABLE feature_doc_link
(
    id          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    feature_id  UNIQUEIDENTIFIER NOT NULL,
    doc_uid     NVARCHAR(200)    NOT NULL,
    section_uid NVARCHAR(200),

    CONSTRAINT fk_feature_doc_link_feature
        FOREIGN KEY (feature_id)
            REFERENCES feature (id)
            ON DELETE CASCADE
);

CREATE INDEX idx_feature_doc_link_feature_id
    ON feature_doc_link (feature_id);