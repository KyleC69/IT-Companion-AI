-- ============================================================
-- 5.3 SAMPLE & EXECUTION DB SCHEMA (SQL SERVER)
-- ============================================================

-- ============================================================
-- SAMPLE RUN ROOT
-- ============================================================

CREATE TABLE sample_run (
    id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    snapshot_id     UNIQUEIDENTIFIER NOT NULL,
    timestamp_utc   DATETIME2        NOT NULL,
    schema_version  NVARCHAR(200)    NOT NULL,

    CONSTRAINT fk_sample_run_snapshot
        FOREIGN KEY (snapshot_id)
        REFERENCES source_snapshot(id)
        ON DELETE NO ACTION
);

CREATE INDEX idx_sample_run_snapshot_id
    ON sample_run(snapshot_id);

-- ============================================================
-- SAMPLE
-- ============================================================

CREATE TABLE sample (
    id                     UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    sample_run_id          UNIQUEIDENTIFIER NOT NULL,
    sample_uid             NVARCHAR(200) NOT NULL,
    feature_uid            NVARCHAR(200),      -- references feature.feature_uid (logical link)
    language               NVARCHAR(200),
    code                   NVARCHAR(MAX),
    entry_point            NVARCHAR(400),
    target_framework       NVARCHAR(200),
    package_references     NVARCHAR(MAX),      -- JSON
    derived_from_code_uid  NVARCHAR(200),
    tags                   NVARCHAR(MAX),      -- JSON array

    CONSTRAINT fk_sample_sample_run
        FOREIGN KEY (sample_run_id)
        REFERENCES sample_run(id)
        ON DELETE CASCADE,

    CONSTRAINT uq_sample_uid UNIQUE (sample_uid)
);

CREATE INDEX idx_sample_sample_run_id
    ON sample(sample_run_id);

CREATE INDEX idx_sample_feature_uid
    ON sample(feature_uid);

-- ============================================================
-- SAMPLE → API MEMBER LINK
-- ============================================================

CREATE TABLE sample_api_member_link (
    id          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    sample_id   UNIQUEIDENTIFIER NOT NULL,
    member_uid  NVARCHAR(200) NOT NULL,

    CONSTRAINT fk_sample_api_member_link_sample
        FOREIGN KEY (sample_id)
        REFERENCES sample(id)
        ON DELETE CASCADE
);

CREATE INDEX idx_sample_api_member_link_sample_id
    ON sample_api_member_link(sample_id);

-- ============================================================
-- EXECUTION RUN
-- ============================================================

CREATE TABLE execution_run (
    id               UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    snapshot_id      UNIQUEIDENTIFIER NOT NULL,
    sample_run_id    UNIQUEIDENTIFIER NOT NULL,
    timestamp_utc    DATETIME2        NOT NULL,
    environment_json NVARCHAR(MAX),
    schema_version   NVARCHAR(200) NOT NULL,

    CONSTRAINT fk_execution_run_snapshot
        FOREIGN KEY (snapshot_id)
        REFERENCES source_snapshot(id)
        ON DELETE NO ACTION,

    CONSTRAINT fk_execution_run_sample_run
        FOREIGN KEY (sample_run_id)
        REFERENCES sample_run(id)
        ON DELETE CASCADE
);

CREATE INDEX idx_execution_run_snapshot_id
    ON execution_run(snapshot_id);

CREATE INDEX idx_execution_run_sample_run_id
    ON execution_run(sample_run_id);

-- ============================================================
-- EXECUTION RESULT
-- ============================================================

CREATE TABLE execution_result (
    id                UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    execution_run_id  UNIQUEIDENTIFIER NOT NULL,
    sample_uid        NVARCHAR(200) NOT NULL,
    status            NVARCHAR(100),
    build_log         NVARCHAR(MAX),
    run_log           NVARCHAR(MAX),
    exception_json    NVARCHAR(MAX),
    duration_ms       INT,

    CONSTRAINT fk_execution_result_run
        FOREIGN KEY (execution_run_id)
        REFERENCES execution_run(id)
        ON DELETE CASCADE
);

CREATE INDEX idx_execution_result_run_id
    ON execution_result(execution_run_id);

CREATE INDEX idx_execution_result_sample_uid
    ON execution_result(sample_uid);