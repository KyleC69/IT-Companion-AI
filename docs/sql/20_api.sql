-- =============================================================================
-- 20_api.sql
-- API types, members, parameters: living, temporal artifacts
-- =============================================================================
DROP TABLE IF EXISTS [dbo].[api_type];

CREATE TABLE [dbo].[api_type](
    [id]                       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_api_type] PRIMARY KEY,
    [semantic_uid]             NVARCHAR(1000)   NOT NULL,
    [source_snapshot_id]       UNIQUEIDENTIFIER NOT NULL,
    [name]                     NVARCHAR(400)    NULL,
    [namespace_path]           NVARCHAR(1000)   NULL,
    [kind]                     NVARCHAR(200)    NULL,
    [accessibility]            NVARCHAR(200)    NULL,
    [is_static]                BIT              NULL,
    [is_generic]               BIT              NULL,
    [is_abstract]              BIT              NULL,
    [is_sealed]                BIT              NULL,
    [is_record]                BIT              NULL,
    [is_ref_like]              BIT              NULL,
    [base_type_uid]            NVARCHAR(1000)   NULL,
    [interfaces]               NVARCHAR(MAX)    NULL,
    [containing_type_uid]      NVARCHAR(1000)   NULL,
    [generic_parameters]       NVARCHAR(MAX)    NULL,
    [generic_constraints]      NVARCHAR(MAX)    NULL,
    [summary]                  NVARCHAR(MAX)    NULL,
    [remarks]                  NVARCHAR(MAX)    NULL,
    [attributes]               NVARCHAR(MAX)    NULL,
    [source_file_path]         NVARCHAR(MAX)    NULL,
    [source_start_line]        INT              NULL,
    [source_end_line]          INT              NULL,

    -- Temporal
    [version_number]           INT              NOT NULL,
    [created_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [updated_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [removed_ingestion_run_id] UNIQUEIDENTIFIER NULL,
    [valid_from_utc]           DATETIME2(7)     NOT NULL,
    [valid_to_utc]             DATETIME2(7)     NULL,
    [is_active]                BIT              NOT NULL CONSTRAINT [df_api_type_is_active] DEFAULT (1),
    [content_hash]             BINARY(32)       NULL
);
ALTER TABLE [dbo].[api_type]
    ADD CONSTRAINT [df_api_type_id] DEFAULT (NEWID()) FOR [id];

    ALTER TABLE dbo.api_type
ADD semantic_uid_hash AS CAST(HASHBYTES('SHA2_256', semantic_uid) AS BINARY(32)) PERSISTED;






DROP TABLE IF EXISTS [dbo].[api_member];
CREATE TABLE [dbo].[api_member](
    [id]                       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_api_member] PRIMARY KEY,
    [semantic_uid]             NVARCHAR(1000)   NOT NULL,
    [api_type_id]              UNIQUEIDENTIFIER NOT NULL,
    [name]                     NVARCHAR(400)    NULL,
    [kind]                     NVARCHAR(200)    NULL,
    [method_kind]              NVARCHAR(200)    NULL,
    [accessibility]            NVARCHAR(200)    NULL,
    [is_static]                BIT              NULL,
    [is_extension_method]      BIT              NULL,
    [is_async]                 BIT              NULL,
    [is_virtual]               BIT              NULL,
    [is_override]              BIT              NULL,
    [is_abstract]              BIT              NULL,
    [is_sealed]                BIT              NULL,
    [is_readonly]              BIT              NULL,
    [is_const]                 BIT              NULL,
    [is_unsafe]                BIT              NULL,
    [return_type_uid]          NVARCHAR(1000)   NULL,
    [return_nullable]          NVARCHAR(50)     NULL,
    [generic_parameters]       NVARCHAR(MAX)    NULL,
    [generic_constraints]      NVARCHAR(MAX)    NULL,
    [summary]                  NVARCHAR(MAX)    NULL,
    [remarks]                  NVARCHAR(MAX)    NULL,
    [attributes]               NVARCHAR(MAX)    NULL,
    [source_file_path]         NVARCHAR(MAX)    NULL,
    [source_start_line]        INT              NULL,
    [source_end_line]          INT              NULL,
    [member_uid_hash]          BINARY(32)       NULL,

    -- Temporal
    [version_number]           INT              NOT NULL,
    [created_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [updated_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [removed_ingestion_run_id] UNIQUEIDENTIFIER NULL,
    [valid_from_utc]           DATETIME2(7)     NOT NULL,
    [valid_to_utc]             DATETIME2(7)     NULL,
    [is_active]                BIT              NOT NULL CONSTRAINT [df_api_member_is_active] DEFAULT (1),
    [content_hash]             BINARY(32)       NULL
);

    ALTER TABLE dbo.api_member
ADD semantic_uid_hash AS CAST(HASHBYTES('SHA2_256', semantic_uid) AS BINARY(32)) PERSISTED;




    DROP TABLE IF EXISTS [dbo].[api_parameter];


CREATE TABLE [dbo].[api_parameter](
    [id]                       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_api_parameter] PRIMARY KEY,
    [semantic_uid]             NVARCHAR(1000)   NOT NULL,
    [api_member_id]            UNIQUEIDENTIFIER NOT NULL,
    [name]                     NVARCHAR(200)    NULL,
    [type_uid]                 NVARCHAR(1000)   NULL,
    [nullable_annotation]      NVARCHAR(50)     NULL,
    [position]                 INT              NULL,
    [modifier]                 NVARCHAR(50)     NULL,
    [has_default_value]        BIT              NULL,
    [default_value_literal]    NVARCHAR(MAX)    NULL,

    -- Temporal
    [version_number]           INT              NOT NULL,
    [created_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [updated_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [removed_ingestion_run_id] UNIQUEIDENTIFIER NULL,
    [valid_from_utc]           DATETIME2(7)     NOT NULL,
    [valid_to_utc]             DATETIME2(7)     NULL,
    [is_active]                BIT              NOT NULL CONSTRAINT [df_api_parameter_is_active] DEFAULT (1),
    [content_hash]             BINARY(32)       NULL
);

  -- =============================================================================
-- 20_api.sql
-- API types, members, parameters: living, temporal artifacts
-- =============================================================================
    DROP TABLE IF EXISTS [dbo].[api_parameter];


DROP TABLE IF EXISTS [dbo].[api_member];

DROP TABLE IF EXISTS [dbo].[api_type];

CREATE TABLE [dbo].[api_type](
    [id]                       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_api_type] PRIMARY KEY,
    [semantic_uid]             NVARCHAR(1000)   NOT NULL,
    [source_snapshot_id]       UNIQUEIDENTIFIER NOT NULL,
    [name]                     NVARCHAR(400)    NULL,
    [namespace_path]           NVARCHAR(1000)   NULL,
    [kind]                     NVARCHAR(200)    NULL,
    [accessibility]            NVARCHAR(200)    NULL,
    [is_static]                BIT              NULL,
    [is_generic]               BIT              NULL,
    [is_abstract]              BIT              NULL,
    [is_sealed]                BIT              NULL,
    [is_record]                BIT              NULL,
    [is_ref_like]              BIT              NULL,
    [base_type_uid]            NVARCHAR(1000)   NULL,
    [interfaces]               NVARCHAR(MAX)    NULL,
    [containing_type_uid]      NVARCHAR(1000)   NULL,
    [generic_parameters]       NVARCHAR(MAX)    NULL,
    [generic_constraints]      NVARCHAR(MAX)    NULL,
    [summary]                  NVARCHAR(MAX)    NULL,
    [remarks]                  NVARCHAR(MAX)    NULL,
    [attributes]               NVARCHAR(MAX)    NULL,
    [source_file_path]         NVARCHAR(MAX)    NULL,
    [source_start_line]        INT              NULL,
    [source_end_line]          INT              NULL,

    -- Temporal
    [version_number]           INT              NOT NULL,
    [created_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [updated_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [removed_ingestion_run_id] UNIQUEIDENTIFIER NULL,
    [valid_from_utc]           DATETIME2(7)     NOT NULL,
    [valid_to_utc]             DATETIME2(7)     NULL,
    [is_active]                BIT              NOT NULL CONSTRAINT [df_api_type_is_active] DEFAULT (1),
    [content_hash]             BINARY(32)       NULL
);







CREATE TABLE [dbo].[api_member](
    [id]                       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_api_member] PRIMARY KEY,
    [semantic_uid]             NVARCHAR(1000)   NOT NULL,
    [api_type_id]              UNIQUEIDENTIFIER NOT NULL,
    [name]                     NVARCHAR(400)    NULL,
    [kind]                     NVARCHAR(200)    NULL,
    [method_kind]              NVARCHAR(200)    NULL,
    [accessibility]            NVARCHAR(200)    NULL,
    [is_static]                BIT              NULL,
    [is_extension_method]      BIT              NULL,
    [is_async]                 BIT              NULL,
    [is_virtual]               BIT              NULL,
    [is_override]              BIT              NULL,
    [is_abstract]              BIT              NULL,
    [is_sealed]                BIT              NULL,
    [is_readonly]              BIT              NULL,
    [is_const]                 BIT              NULL,
    [is_unsafe]                BIT              NULL,
    [return_type_uid]          NVARCHAR(1000)   NULL,
    [return_nullable]          NVARCHAR(50)     NULL,
    [generic_parameters]       NVARCHAR(MAX)    NULL,
    [generic_constraints]      NVARCHAR(MAX)    NULL,
    [summary]                  NVARCHAR(MAX)    NULL,
    [remarks]                  NVARCHAR(MAX)    NULL,
    [attributes]               NVARCHAR(MAX)    NULL,
    [source_file_path]         NVARCHAR(MAX)    NULL,
    [source_start_line]        INT              NULL,
    [source_end_line]          INT              NULL,
    [member_uid_hash]          BINARY(32)       NULL,

    -- Temporal
    [version_number]           INT              NOT NULL,
    [created_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [updated_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [removed_ingestion_run_id] UNIQUEIDENTIFIER NULL,
    [valid_from_utc]           DATETIME2(7)     NOT NULL,
    [valid_to_utc]             DATETIME2(7)     NULL,
    [is_active]                BIT              NOT NULL CONSTRAINT [df_api_member_is_active] DEFAULT (1),
    [content_hash]             BINARY(32)       NULL
);







CREATE TABLE [dbo].[api_parameter](
    [id]                       UNIQUEIDENTIFIER NOT NULL CONSTRAINT [pk_api_parameter] PRIMARY KEY,
    [api_member_id]            UNIQUEIDENTIFIER NOT NULL,
    [name]                     NVARCHAR(200)    NULL,
    [type_uid]                 NVARCHAR(1000)   NULL,
    [nullable_annotation]      NVARCHAR(50)     NULL,
    [position]                 INT              NULL,
    [modifier]                 NVARCHAR(50)     NULL,
    [has_default_value]        BIT              NULL,
    [default_value_literal]    NVARCHAR(MAX)    NULL,

    -- Temporal
    [version_number]           INT              NOT NULL,
    [created_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [updated_ingestion_run_id] UNIQUEIDENTIFIER NOT NULL,
    [removed_ingestion_run_id] UNIQUEIDENTIFIER NULL,
    [valid_from_utc]           DATETIME2(7)     NOT NULL,
    [valid_to_utc]             DATETIME2(7)     NULL,
    [is_active]                BIT              NOT NULL CONSTRAINT [df_api_parameter_is_active] DEFAULT (1),
    [content_hash]             BINARY(32)       NULL
);



ALTER TABLE [dbo].[api_type]
    ADD CONSTRAINT [df_api_type_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE dbo.api_type
    ADD semantic_uid_hash AS CAST(HASHBYTES('SHA2_256', semantic_uid) AS BINARY(32)) PERSISTED;

ALTER TABLE dbo.api_member
    ADD semantic_uid_hash AS CAST(HASHBYTES('SHA2_256', semantic_uid) AS BINARY(32)) PERSISTED;



ALTER TABLE [dbo].[api_type]
    ADD CONSTRAINT [uq_api_type_semantic_version]
        UNIQUE ([semantic_uid_hash], [version_number]);

ALTER TABLE [dbo].[api_member]
    ADD CONSTRAINT [uq_api_member_semantic_version]
        UNIQUE ([semantic_uid_hash], [version_number]);



ALTER TABLE [dbo].[api_type] WITH CHECK
    ADD CONSTRAINT [fk_api_type_snapshot]
        FOREIGN KEY ([source_snapshot_id])
        REFERENCES [dbo].[source_snapshot]([id])
        ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE [dbo].[api_type] WITH CHECK
    ADD CONSTRAINT [fk_api_type_created_ingestion_run]
        FOREIGN KEY ([created_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[api_type] WITH CHECK
    ADD CONSTRAINT [fk_api_type_updated_ingestion_run]
        FOREIGN KEY ([updated_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[api_type] WITH CHECK
    ADD CONSTRAINT [fk_api_type_removed_ingestion_run]
        FOREIGN KEY ([removed_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id])
        ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE dbo.api_type
    WITH CHECK ADD CONSTRAINT fk_api_type_semantic_identity
        FOREIGN KEY (semantic_uid_hash)
        REFERENCES dbo.semantic_identity(uid_hash)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION;

CREATE INDEX [ix_api_type_semantic_active]
    ON [dbo].[api_type]([semantic_uid_hash], [is_active]);

CREATE INDEX [ix_api_type_semantic_valid_from]
    ON [dbo].[api_type]([semantic_uid_hash], [valid_from_utc]);

CREATE INDEX [ix_api_type_content_hash]
    ON [dbo].[api_type]([content_hash]);

ALTER TABLE [dbo].[api_member]
    ADD CONSTRAINT [df_api_member_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[api_member] WITH CHECK
    ADD CONSTRAINT [fk_api_member_type]
        FOREIGN KEY ([api_type_id])
        REFERENCES [dbo].[api_type]([id])
        ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE [dbo].[api_member] WITH CHECK
    ADD CONSTRAINT [fk_api_member_created_ingestion_run]
        FOREIGN KEY ([created_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[api_member] WITH CHECK
    ADD CONSTRAINT [fk_api_member_updated_ingestion_run]
        FOREIGN KEY ([updated_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[api_member] WITH CHECK
    ADD CONSTRAINT [fk_api_member_removed_ingestion_run]
        FOREIGN KEY ([removed_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id])
        ON DELETE SET NULL;

ALTER TABLE [dbo].[api_member] WITH CHECK
    ADD CONSTRAINT [fk_api_member_semantic_identity]
        FOREIGN KEY ([semantic_uid_hash])
        REFERENCES [dbo].[semantic_identity]([uid_hash])
        ON DELETE NO ACTION
        ON UPDATE NO ACTION;

CREATE UNIQUE NONCLUSTERED INDEX [ix_api_member_type_hash]
    ON [dbo].[api_member]([api_type_id], [member_uid_hash]);

CREATE INDEX [ix_api_member_semantic_active]
    ON [dbo].[api_member]([semantic_uid_hash], [is_active]);

CREATE INDEX [ix_api_member_semantic_valid_from]
    ON [dbo].[api_member]([semantic_uid_hash], [valid_from_utc]);

CREATE INDEX [ix_api_member_content_hash]
    ON [dbo].[api_member]([content_hash]);

ALTER TABLE [dbo].[api_parameter]
    ADD CONSTRAINT [df_api_parameter_id] DEFAULT (NEWID()) FOR [id];

ALTER TABLE [dbo].[api_parameter] WITH CHECK
    ADD CONSTRAINT [fk_api_parameter_member]
        FOREIGN KEY ([api_member_id])
        REFERENCES [dbo].[api_member]([id])
        ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE [dbo].[api_parameter] WITH CHECK
    ADD CONSTRAINT [fk_api_parameter_created_ingestion_run]
        FOREIGN KEY ([created_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[api_parameter] WITH CHECK
    ADD CONSTRAINT [fk_api_parameter_updated_ingestion_run]
        FOREIGN KEY ([updated_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id]);

ALTER TABLE [dbo].[api_parameter] WITH CHECK
    ADD CONSTRAINT [fk_api_parameter_removed_ingestion_run]
        FOREIGN KEY ([removed_ingestion_run_id])
        REFERENCES [dbo].[ingestion_run]([id])
        ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE [dbo].[api_parameter]
    ADD CONSTRAINT [uq_api_parameter_member_position_version]
        UNIQUE ([api_member_id], [position], [version_number]);

CREATE INDEX [ix_api_parameter_member_position_active]
    ON [dbo].[api_parameter]([api_member_id], [position], [is_active]);

CREATE INDEX [ix_api_parameter_content_hash]
    ON [dbo].[api_parameter]([content_hash]);
