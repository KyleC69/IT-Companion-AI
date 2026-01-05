// Project Name: SKAgent
// File Name: IngestionInterceptor.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using ITCompanionAI.KnowledgeBase;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace ITCompanionAI.EFContext;


public class IngestionInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? context = eventData.Context;
        if (context == null)
        {
            return result;
        }

        await HandleAddedEntitiesAsync(context, cancellationToken);

        return result;
    }







    private async Task HandleAddedEntitiesAsync(DbContext context, CancellationToken ct)
    {
        List<EntityEntry> entries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added)
            .ToList();

        foreach (EntityEntry entry in entries)
            switch (entry.Entity)
            {
                case ApiType type:
                    await HandleApiTypeAsync(context, entry, type, ct);
                    break;


                case ApiMember member:
                    await HandleApiMemberAsync(context, entry, member, ct);
                    break;


                case ApiParameter param:
                    await HandleApiParameterAsync(context, entry, param, ct);
                    break;
            }
    }







    private async Task HandleApiTypeAsync(
        DbContext context,
        EntityEntry entry,
        ApiType type,
        CancellationToken ct)
    {



        // Assign canonical ID returned by SQL


        // Mark as handled
        entry.State = EntityState.Unchanged;
    }







    private async Task HandleApiMemberAsync(
        DbContext context,
        EntityEntry entry,
        ApiMember member,
        CancellationToken ct)
    {


        entry.Property(nameof(ApiMember.Id)).CurrentValue = member.Id;
        entry.State = EntityState.Unchanged;
    }







    private async Task HandleApiParameterAsync(
        DbContext context,
        EntityEntry entry,
        ApiParameter param,
        CancellationToken ct)
    {


        entry.Property(nameof(ApiParameter.Id)).CurrentValue = param.Id;
        entry.State = EntityState.Unchanged;
    }
}
/*
 * CREATE PROCEDURE [dbo].[UpsertApiMember]
(
    @SemanticUid             NVARCHAR(1000),
    @ApiTypeId               UNIQUEIDENTIFIER,
    @IngestionRunId          UNIQUEIDENTIFIER,

    @Name                    NVARCHAR(400)    = NULL,
    @Kind                    NVARCHAR(200)    = NULL,
    @MethodKind              NVARCHAR(200)    = NULL,
    @Accessibility           NVARCHAR(200)    = NULL,
    @IsStatic                BIT              = NULL,
    @IsExtensionMethod       BIT              = NULL,
    @IsAsync                 BIT              = NULL,
    @IsVirtual               BIT              = NULL,
    @IsOverride              BIT              = NULL,
    @IsAbstract              BIT              = NULL,
    @IsSealed                BIT              = NULL,
    @IsReadOnly              BIT              = NULL,
    @IsConst                 BIT              = NULL,
    @IsUnsafe                BIT              = NULL,
    @ReturnTypeUid           NVARCHAR(1000)   = NULL,
    @ReturnNullable          NVARCHAR(50)     = NULL,
    @GenericParameters       NVARCHAR(MAX)    = NULL,
    @GenericConstraints      NVARCHAR(MAX)    = NULL,
    @Summary                 NVARCHAR(MAX)    = NULL,
    @Remarks                 NVARCHAR(MAX)    = NULL,
    @Attributes              NVARCHAR(MAX)    = NULL,
    @SourceFilePath          NVARCHAR(MAX)    = NULL,
    @SourceStartLine         INT              = NULL,
    @SourceEndLine           INT              = NULL
)
CREATE PROCEDURE [dbo].[UpsertApiType]
(
    @SemanticUid             NVARCHAR(1000),
    @SourceSnapshotId        UNIQUEIDENTIFIER,
    @IngestionRunId          UNIQUEIDENTIFIER,

    @Name                    NVARCHAR(400)    = NULL,
    @NamespacePath           NVARCHAR(1000)   = NULL,
    @Kind                    NVARCHAR(200)    = NULL,
    @Accessibility           NVARCHAR(200)    = NULL,
    @IsStatic                BIT              = NULL,
    @IsGeneric               BIT              = NULL,
    @IsAbstract              BIT              = NULL,
    @IsSealed                BIT              = NULL,
    @IsRecord                BIT              = NULL,
    @IsRefLike               BIT              = NULL,
    @BaseTypeUid             NVARCHAR(1000)   = NULL,
    @Interfaces              NVARCHAR(MAX)    = NULL,
    @ContainingTypeUid       NVARCHAR(1000)   = NULL,
    @GenericParameters       NVARCHAR(MAX)    = NULL,
    @GenericConstraints      NVARCHAR(MAX)    = NULL,
    @Summary                 NVARCHAR(MAX)    = NULL,
    @Remarks                 NVARCHAR(MAX)    = NULL,
    @Attributes              NVARCHAR(MAX)    = NULL,
    @SourceFilePath          NVARCHAR(MAX)    = NULL,
    @SourceStartLine         INT              = NULL,
    @SourceEndLine           INT              = NULL
)



CREATE   PROCEDURE dbo.UpsertApiParameter
(
    @ApiMember_id            UNIQUEIDENTIFIER,
    @name                     NVARCHAR(200),
    @type_uid                 NVARCHAR(1000),
    @nullable_annotation      NVARCHAR(50),
    @position                 INT,
    @modifier                 NVARCHAR(50),
    @has_default_value        BIT,
    @default_value_literal    NVARCHAR(MAX),
    @ingestion_run_id         UNIQUEIDENTIFIER
)
*/