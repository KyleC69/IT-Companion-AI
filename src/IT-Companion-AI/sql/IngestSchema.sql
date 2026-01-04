-- ============================================================
-- 2.2 INGESTION DB SCHEMA (SQL SERVER)
-- ============================================================

-- Enable default UUID generation
-- SQL Server uses NEWID() or NEWSEQUENTIALID()

-- ============================================================
-- CORE
-- ============================================================

CREATE TABLE ingestion_run
(
    id             UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    timestamp_utc  DATETIME2        NOT NULL,
    schema_version NVARCHAR(200)    NOT NULL,
    notes          NVARCHAR(MAX)
);

CREATE TABLE source_snapshot
(
    id               UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    ingestion_run_id UNIQUEIDENTIFIER NOT NULL,
    snapshot_uid     NVARCHAR(200)    NOT NULL,
    repo_url         NVARCHAR(MAX),
    branch           NVARCHAR(200),
    repocommit       NVARCHAR(200),
    language         NVARCHAR(200),
    package_name     NVARCHAR(200),
    package_version  NVARCHAR(200),
    config_json      NVARCHAR(MAX),

    CONSTRAINT fk_source_snapshot_ingestion_run
        FOREIGN KEY (ingestion_run_id)
            REFERENCES ingestion_run (id)
            ON DELETE CASCADE,

    CONSTRAINT uq_source_snapshot_uid UNIQUE (snapshot_uid)
);

CREATE INDEX idx_source_snapshot_ingestion_run_id
    ON source_snapshot (ingestion_run_id);

-- ============================================================
-- API
-- ============================================================

CREATE TABLE api_type
(
    id                 UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    source_snapshot_id UNIQUEIDENTIFIER NOT NULL,
    type_uid           NVARCHAR(200)    NOT NULL,
    name               NVARCHAR(400),
    namespace          NVARCHAR(400),
    kind               NVARCHAR(200),
    accessibility      NVARCHAR(200),
    is_static          BIT,
    is_generic         BIT,
    generic_parameters NVARCHAR(MAX),
    summary            NVARCHAR(MAX),
    remarks            NVARCHAR(MAX),
    attributes         NVARCHAR(MAX),

    CONSTRAINT fk_api_type_snapshot
        FOREIGN KEY (source_snapshot_id)
            REFERENCES source_snapshot (id)
            ON DELETE CASCADE,

    CONSTRAINT uq_api_type_uid_per_snapshot
        UNIQUE (source_snapshot_id, type_uid)
);

CREATE INDEX idx_api_type_snapshot_id
    ON api_type (source_snapshot_id);

CREATE TABLE api_member
(
    id                  UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    api_type_id         UNIQUEIDENTIFIER NOT NULL,
    member_uid          NVARCHAR(200)    NOT NULL,
    name                NVARCHAR(400),
    kind                NVARCHAR(200),
    accessibility       NVARCHAR(200),
    is_static           BIT,
    is_extension_method BIT,
    is_async            BIT,
    return_type         NVARCHAR(400),
    summary             NVARCHAR(MAX),
    remarks             NVARCHAR(MAX),
    generic_parameters  NVARCHAR(MAX),
    attributes          NVARCHAR(MAX),
    source_file_path    NVARCHAR(MAX),
    source_start_line   INT,
    source_end_line     INT,

    CONSTRAINT fk_api_member_type
        FOREIGN KEY (api_type_id)
            REFERENCES api_type (id)
            ON DELETE CASCADE,

    CONSTRAINT uq_api_member_uid_per_type
        UNIQUE (api_type_id, member_uid)
);

CREATE INDEX idx_api_member_api_type_id
    ON api_member (api_type_id);

CREATE TABLE api_parameter
(
    id                    UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    api_member_id         UNIQUEIDENTIFIER NOT NULL,
    name                  NVARCHAR(200),
    type                  NVARCHAR(400),
    position              INT,
    has_default_value     BIT,
    default_value_literal NVARCHAR(MAX),

    CONSTRAINT fk_api_parameter_member
        FOREIGN KEY (api_member_id)
            REFERENCES api_member (id)
            ON DELETE CASCADE
);

CREATE INDEX idx_api_parameter_member_id
    ON api_parameter (api_member_id);

CREATE TABLE api_member_doc_link
(
    id            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    api_member_id UNIQUEIDENTIFIER NOT NULL,
    doc_uid       NVARCHAR(200)    NOT NULL,
    section_uid   NVARCHAR(200),

    CONSTRAINT fk_api_member_doc_link_member
        FOREIGN KEY (api_member_id)
            REFERENCES api_member (id)
            ON DELETE CASCADE
);

CREATE INDEX idx_api_member_doc_link_member_id
    ON api_member_doc_link (api_member_id);

-- ============================================================
-- DOCS & CODE
-- ============================================================

CREATE TABLE doc_page
(
    id                 UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    source_snapshot_id UNIQUEIDENTIFIER NOT NULL,
    doc_uid            NVARCHAR(200)    NOT NULL,
    source_path        NVARCHAR(MAX),
    title              NVARCHAR(400),
    language           NVARCHAR(200),
    url                NVARCHAR(MAX),
    raw_markdown       NVARCHAR(MAX),

    CONSTRAINT fk_doc_page_snapshot
        FOREIGN KEY (source_snapshot_id)
            REFERENCES source_snapshot (id)
            ON DELETE CASCADE,

    CONSTRAINT uq_doc_page_uid_per_snapshot
        UNIQUE (source_snapshot_id, doc_uid)
);

CREATE INDEX idx_doc_page_snapshot_id
    ON doc_page (source_snapshot_id);

CREATE TABLE doc_section
(
    id               UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    doc_page_id      UNIQUEIDENTIFIER NOT NULL,
    section_uid      NVARCHAR(200)    NOT NULL,
    heading          NVARCHAR(400),
    level            INT,
    content_markdown NVARCHAR(MAX),
    order_index      INT,

    CONSTRAINT fk_doc_section_page
        FOREIGN KEY (doc_page_id)
            REFERENCES doc_page (id)
            ON DELETE CASCADE
);

CREATE INDEX idx_doc_section_page_id
    ON doc_section (doc_page_id);

CREATE TABLE code_block
(
    id                UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    doc_section_id    UNIQUEIDENTIFIER NOT NULL,
    code_uid          NVARCHAR(200),
    language          NVARCHAR(200),
    content           NVARCHAR(MAX),
    declared_packages NVARCHAR(MAX),
    tags              NVARCHAR(MAX), -- stored as JSON array
    inline_comments   NVARCHAR(MAX),

    CONSTRAINT fk_code_block_section
        FOREIGN KEY (doc_section_id)
            REFERENCES doc_section (id)
            ON DELETE CASCADE
);

CREATE INDEX idx_code_block_section_id
    ON code_block (doc_section_id);