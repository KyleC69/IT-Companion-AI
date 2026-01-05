-- =============================================================================
-- 10_semantic_identity.sql
-- Global registry of semantic identities (types, members, docs, features, etc.)
-- Key-length-safe version using SHA-256 hash indexing
-- =============================================================================

CREATE TABLE [dbo].[semantic_identity](
    -- Original UID (long, human-readable, semantic)
    [uid]               NVARCHAR(1000) NOT NULL,

    -- Hash of UID (32 bytes, safe for PK and indexing)
    [uid_hash]          AS CAST(HASHBYTES('SHA2_256', [uid]) AS BINARY(32)) PERSISTED,

    [kind]              NVARCHAR(50)   NOT NULL,   -- 'type','member','parameter','doc_page','doc_section','code_block','feature', etc.
    [created_utc]       DATETIME2(7)   NOT NULL,
    [notes]             NVARCHAR(MAX)  NULL,

    -- Primary key now uses the hash (safe, fixed size)
    CONSTRAINT [pk_semantic_identity] PRIMARY KEY CLUSTERED ([uid_hash])
);

-- Secondary index for queries by kind + UID hash
CREATE INDEX [ix_semantic_identity_kind_uid_hash]
    ON [dbo].[semantic_identity]([kind], [uid_hash]);