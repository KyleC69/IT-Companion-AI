-- =============================================================================
-- 60_rag.sql
-- RAG chunks
-- =============================================================================

CREATE TABLE [dbo].[rag_chunk](
    [id]              UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_rag_chunk] PRIMARY KEY,
    [rag_run_id]      UNIQUEIDENTIFIER NOT NULL,
    [chunk_uid]       NVARCHAR(1000)   NOT NULL,
    [kind]            NVARCHAR(100)    NULL,
    [text]            NVARCHAR(MAX)    NULL,
    [metadata_json]   NVARCHAR(MAX)    NULL,
    [embedding_vector] [vector](1536, float32) NULL
);
ALTER TABLE [dbo].[rag_chunk]
    ADD CONSTRAINT [df_rag_chunk_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[rag_chunk] WITH CHECK
    ADD CONSTRAINT [fk_rag_chunk_run]
        FOREIGN KEY ([rag_run_id])
        REFERENCES [dbo].[rag_run]([id])
        ON DELETE CASCADE;

ALTER TABLE [dbo].[rag_chunk]
    ADD CONSTRAINT [uq_rag_chunk_uid] UNIQUE ([chunk_uid]);

CREATE INDEX [ix_rag_chunk_kind]
    ON [dbo].[rag_chunk]([kind]);