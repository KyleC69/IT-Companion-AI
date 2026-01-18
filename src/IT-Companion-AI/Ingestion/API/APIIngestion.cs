// Project Name: SKAgent
// File Name: APIIngestion.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ApiMember = ITCompanionAI.EFModels.ApiMember;
using ApiParameter = ITCompanionAI.EFModels.ApiParameter;
using ApiType = ITCompanionAI.EFModels.ApiType;
using KBContext = ITCompanionAI.EFModels.KBContext;

namespace ITCompanionAI.Ingestion.API;


/// <summary>
/// </summary>
public class APIIngestion : RoslynHarvesterBase
{
    private readonly KBContext _db;

    private Guid? runid;







    /// <summary>
    ///     Initializes a new instance of the <see cref="APIIngestion" /> class.
    /// </summary>
    /// <param name="db">Database context used for persistence.</param>
    public APIIngestion(KBContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }






    public async Task<Guid> StartIngestionAsync()
    {
        var schemaVersion = "1.0";
        var notes = "Initial Ingestion Attempt";

        // The solution serves as the root container for all projects, files, and code elements within the codebase being analyzed.
        // TODO: Move this path into configuration.
        SolutionManifest manifest = new();
       
       
        var slnpath=@"F:\SkApiRepo\src\semantic-kernel-dotnet-1.68.0\dotnet";
        // Load the solution from the directory.
        var solutionpath=@"F:\SkApiRepo\semantic-kernel\dotnet\";
        
        manifest = await LoadManifestAsync(Path.Combine(slnpath, "SK-release.slnf"));
        
            var solution =await LoadSolutionFromManifestAsync(manifest, solutionpath, CancellationToken.None);
        // Begin ingestion run.
        Guid ingestionRunId;
        {
            Tuple<Guid?> ingestionRunIdResult =
                await _db.SpBeginIngestionRunAsync(schemaVersion, notes, Guid.NewGuid()).ConfigureAwait(false);

            ingestionRunId = ingestionRunIdResult.Item1
                             ?? throw new InvalidOperationException(
                                 "sp_BeginIngestionRun did not return an ingestion run id.");

            runid = ingestionRunId;
        }

        // Create source snapshot tied to this ingestion run.
        Guid sourceSnapshotId;
        {
            var snapshotUid = Guid.NewGuid().ToString("D");
            var repoUrl = "";
            var branch = "";
            var repoCommit = "";
            var language = "csharp";
            var packageName = "";
            var packageVersion = "";
            var configJson = "{}";

            Tuple<Guid?> snapshotIdResult = await _db.SpCreateSourceSnapshotAsync(
                    ingestionRunId,
                    snapshotUid,
                    repoUrl,
                    branch,
                    repoCommit,
                    language,
                    packageName,
                    packageVersion,
                    configJson,
                    Guid.NewGuid())
                .ConfigureAwait(false);

            sourceSnapshotId = snapshotIdResult.Item1
                               ?? throw new InvalidOperationException(
                                   "CreateSourceSnapshot did not return a snapshot id.");
        }

        // Harvest symbols (types -> members -> parameters).

        // Types
        List<ApiType> apiTypes = new();
        await ExtractTypesAsync(solution, apiTypes, CancellationToken.None).ConfigureAwait(false);

        // Ensure IDs exist for all types BEFORE member extraction so ApiFeatureId is non-empty.
        foreach (var type in apiTypes)
        {
            if (type.Id == Guid.Empty)
            {
                type.Id = Guid.NewGuid();
            }
        }

        // Build type symbol map now that types are extracted.
        var roslynTypeSymbolsByUid = await BuildTypeSymbolMapAsync(
                solution,
                apiTypes.Select(t => t.SemanticUid),
                CancellationToken.None)
            .ConfigureAwait(false);

        // Members
        List<ApiMember> apiMembers = new();
        var roslynMemberSymbolsByUid = new Dictionary<string, ISymbol>(StringComparer.Ordinal);

        await ExtractMembersAsync(
                solution,
                apiTypes,
                roslynTypeSymbolsByUid,
                apiMembers,
                roslynMemberSymbolsByUid,
                CancellationToken.None)
            .ConfigureAwait(false);

        // Ensure IDs exist for all members BEFORE parameter extraction so ApiMemberId is non-empty.
        foreach (var member in apiMembers)
        {
            if (member.Id == Guid.Empty)
            {
                member.Id = Guid.NewGuid();
            }
        }

        // Parameters
        List<ApiParameter> apiParameters = new();
        await ExtractParametersAsync(
                solution,
                apiMembers,
                roslynMemberSymbolsByUid,
                apiParameters,
                CancellationToken.None)
            .ConfigureAwait(false);

        // Persist (types -> members -> parameters) in parent-first order.
        // Use the DB upsert sprocs as the canonical persistence API.
        Dictionary<string, Guid> typeIdBySemanticUid = new(StringComparer.Ordinal);
        Dictionary<string, Guid> memberIdBySemanticUid = new(StringComparer.Ordinal);

        // Types
        foreach (ApiType type in apiTypes)
        {
            if (string.IsNullOrWhiteSpace(type.SemanticUid))
            {
                continue;
            }

            type.SourceSnapshotId = sourceSnapshotId;
            type.CreatedIngestionRunId = ingestionRunId;
            type.UpdatedIngestionRunId = ingestionRunId;
            type.VersionNumber = type.VersionNumber == 0 ? 1 : type.VersionNumber;
            type.ValidFromUtc = type.ValidFromUtc == default ? DateTime.UtcNow : type.ValidFromUtc;
            type.IsActive = true;

            await _db.SpUpsertApiTypeAsync(
                    type.SemanticUid,
                    type.SourceSnapshotId,
                    ingestionRunId,
                    type.Name,
                    type.NamespacePath,
                    type.Kind,
                    type.Accessibility,
                    type.IsStatic,
                    type.IsGeneric,
                    type.IsAbstract,
                    type.IsSealed,
                    type.IsRecord,
                    type.IsRefLike,
                    type.BaseTypeUid,
                    type.Interfaces,
                    type.ContainingTypeUid,
                    type.GenericParameters,
                    type.GenericConstraints,
                    type.Summary,
                    type.Remarks,
                    type.Attributes,
                    type.SourceFilePath,
                    type.SourceStartLine,
                    type.SourceEndLine)
                .ConfigureAwait(false);

            typeIdBySemanticUid[type.SemanticUid] = type.Id;
        }

        // Members
        foreach (ApiMember member in apiMembers)
        {
            if (string.IsNullOrWhiteSpace(member.SemanticUid))
            {
                continue;
            }

            // Ensure the member's ApiFeatureId references the type we just upserted.
            if (member.ApiFeatureId == Guid.Empty && !string.IsNullOrWhiteSpace(member.SemanticUid))
            {
                // We expect ExtractMembersAsync to set ApiFeatureId from the type's Id.
                // If it's still empty, try to map via type semantic uid if available.
                // If we can't, skip to avoid corrupt linkage.
                continue;
            }

            member.CreatedIngestionRunId = ingestionRunId;
            member.UpdatedIngestionRunId = ingestionRunId;
            member.VersionNumber = member.VersionNumber == 0 ? 1 : member.VersionNumber;
            member.ValidFromUtc = member.ValidFromUtc == default ? DateTime.UtcNow : member.ValidFromUtc;
            member.IsActive = true;

            // MemberUidHash is required by mapping; compute stable hash if missing.
            if (member.MemberUidHash is null || member.MemberUidHash.Length == 0)
            {
                member.MemberUidHash = await _db.FnComputeContentHash256Async(member.SemanticUid).ConfigureAwait(false);
            }

            await _db.SpUpsertApiMemberAsync(
                    member.SemanticUid,
                    member.ApiFeatureId,
                    ingestionRunId,
                    member.Name,
                    member.Kind,
                    member.MethodKind,
                    member.Accessibility,
                    member.IsStatic,
                    member.IsExtensionMethod,
                    member.IsAsync,
                    member.IsVirtual,
                    member.IsOverride,
                    member.IsAbstract,
                    member.IsSealed,
                    member.IsReadonly,
                    member.IsConst,
                    member.IsUnsafe,
                    member.ReturnTypeUid,
                    member.ReturnNullable,
                    member.GenericParameters,
                    member.GenericConstraints,
                    member.Summary,
                    member.Remarks,
                    member.Attributes,
                    member.SourceFilePath,
                    member.SourceStartLine,
                    member.SourceEndLine)
                .ConfigureAwait(false);

            memberIdBySemanticUid[member.SemanticUid] = member.Id;
        }

        // Parameters
        foreach (ApiParameter p in apiParameters)
        {
            if (p.Id == Guid.Empty)
            {
                p.Id = Guid.NewGuid();
            }

            if (p.ApiMemberId == Guid.Empty)
            {
                // Should not happen now that member IDs are assigned before parameter extraction,
                // but keep the guard to avoid inserting orphaned parameters.
                continue;
            }

            p.CreatedIngestionRunId = ingestionRunId;
            p.UpdatedIngestionRunId = ingestionRunId;
            p.VersionNumber = p.VersionNumber == 0 ? 1 : p.VersionNumber;
            p.ValidFromUtc = p.ValidFromUtc == default ? DateTime.UtcNow : p.ValidFromUtc;
            p.IsActive = true;

            await _db.SpUpsertApiParameterAsync(
                    p.ApiMemberId,
                    p.Name,
                    p.TypeUid,
                    p.NullableAnnotation,
                    p.Position,
                    p.Modifier,
                    p.HasDefaultValue,
                    p.DefaultValueLiteral,
                    ingestionRunId)
                .ConfigureAwait(false);
        }

        // TODO: Features + doc pages orchestration.

        await _db.SpEndIngestionRunAsync(ingestionRunId).ConfigureAwait(false);

        return sourceSnapshotId;
    }

}





public sealed class IngestionVerifier
{
    private readonly KBContext _db;

    public IngestionVerifier(KBContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task VerifyApiTypesAsync(Guid sourceSnapshotId, CancellationToken ct = default)
    {
        if (sourceSnapshotId == Guid.Empty)
        {
            throw new ArgumentException("sourceSnapshotId cannot be empty.", nameof(sourceSnapshotId));
        }



        var types = await _db.ApiTypes
            .Where(t => t.SourceSnapshotId == sourceSnapshotId && t.IsActive)
            .ToListAsync(ct)
            .ConfigureAwait(false);

        foreach (var t in types)
        {
            // 1. SemanticUid must not be null/empty
            if (string.IsNullOrWhiteSpace(t.SemanticUid))
            {
                throw new InvalidOperationException($"ApiType {t.Id} has empty SemanticUid.");
            }

            // 2. Kind / accessibility must not be null
            if (string.IsNullOrWhiteSpace(t.Kind) || string.IsNullOrWhiteSpace(t.Accessibility))
            {
                throw new InvalidOperationException($"ApiType {t.SemanticUid} missing kind/accessibility.");
            }

            // 3. If IsGeneric, generic_parameters must be non-null
            if (t.IsGeneric == true && string.IsNullOrWhiteSpace(t.GenericParameters))
            {
                throw new InvalidOperationException($"Generic ApiType {t.SemanticUid} missing GenericParameters.");
            }

            // 4. If interface type, base_type_uid should be null and interfaces can be non-empty
            if (string.Equals(t.Kind, "Interface", StringComparison.OrdinalIgnoreCase)
                && t.BaseTypeUid is not null
                && !t.BaseTypeUid.Equals("object", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Interface {t.SemanticUid} has unexpected base type {t.BaseTypeUid}.");
            }

            // 5. Optionally recompute hash and compare (expensive, but good for spot checks)
            // var payload = BuildApiTypePayload(t);
            // var expectedHex = Sha256Hex(payload);
            // var expectedBytes = Convert.FromHexString(expectedHex);
            // if (!t.ContentHash.SequenceEqual(expectedBytes))
            // {
            //     throw new InvalidOperationException($"Hash mismatch for {t.SemanticUid}.");
            // }
        }
    }

    public async Task VerifyMembersAndParametersAsync(Guid sourceSnapshotId, CancellationToken ct = default)
    {
        var types = await _db.ApiTypes
            .Where(t => t.SourceSnapshotId == sourceSnapshotId && t.IsActive)
            .Select(t => t.Id)
            .ToListAsync(ct)
            .ConfigureAwait(false);

        var typeIdSet = new HashSet<Guid>(types);

        var members = await _db.ApiMembers
            .Where(m => typeIdSet.Contains(m.ApiFeatureId) && m.IsActive)
            .ToListAsync(ct)
            .ConfigureAwait(false);

        foreach (var m in members)
        {
            if (string.IsNullOrWhiteSpace(m.SemanticUid))
            {
                throw new InvalidOperationException($"ApiMember {m.Id} has empty SemanticUid.");
            }

            if (!typeIdSet.Contains(m.ApiFeatureId))
            {
                throw new InvalidOperationException($"ApiMember {m.SemanticUid} references missing type {m.ApiFeatureId}.");
            }
        }

        var memberIds = new HashSet<Guid>(members.Select(m => m.Id));

        var parameters = await _db.ApiParameters
            .Where(p => memberIds.Contains(p.ApiMemberId) && p.IsActive)
            .ToListAsync(ct)
            .ConfigureAwait(false);

        foreach (var p in parameters)
        {
            if (!memberIds.Contains(p.ApiMemberId))
            {
                throw new InvalidOperationException($"ApiParameter {p.Id} references missing member {p.ApiMemberId}.");
            }

            if (string.IsNullOrWhiteSpace(p.TypeUid))
            {
                throw new InvalidOperationException($"ApiParameter {p.Id} for member {p.ApiMemberId} missing TypeUid.");
            }
        }
    }
}

