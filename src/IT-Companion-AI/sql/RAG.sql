-- ============================================================
-- 7.2 RAG DB SCHEMA (SQL SERVER)
-- ============================================================

-- ============================================================
-- RAG RUN ROOT
-- ============================================================

CREATE TABLE rag_run (
    id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    snapshot_id     UNIQUEIDENTIFIER NOT NULL,
    timestamp_utc   DATETIME2        NOT NULL,
    schema_version  NVARCHAR(200)    NOT NULL,

    CONSTRAINT fk_rag_run_snapshot
        FOREIGN KEY (snapshot_id)
        REFERENCES source_snapshot(id)
        ON DELETE NO ACTION
);

CREATE INDEX idx_rag_run_snapshot_id
    ON rag_run(snapshot_id);

-- ============================================================
-- RAG CHUNK
-- ============================================================

CREATE TABLE rag_chunk (
    id               UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    rag_run_id       UNIQUEIDENTIFIER NOT NULL,
    chunk_uid        NVARCHAR(200) NOT NULL,
    kind             NVARCHAR(100),
    text             NVARCHAR(MAX),
    metadata_json    NVARCHAR(MAX),      -- JSON
    embedding_vector VECTOR(1536),     -- vector embedding

    CONSTRAINT fk_rag_chunk_run
        FOREIGN KEY (rag_run_id)
        REFERENCES rag_run(id)
        ON DELETE CASCADE,

    CONSTRAINT uq_rag_chunk_uid UNIQUE (chunk_uid)
);

CREATE INDEX idx_rag_chunk_run_id
    ON rag_chunk(rag_run_id);

CREATE INDEX idx_rag_chunk_kind
    ON rag_chunk(kind);