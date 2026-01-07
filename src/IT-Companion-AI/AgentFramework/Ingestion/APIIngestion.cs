// Project Name: SKAgent
// File Name: APIIngestion.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using ITCompanionAI.Entities;
using ITCompanionAI.KCCurator;


namespace ITCompanionAI.AgentFramework.Ingestion;


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







    public async Task StartIngestionAsync()
    {
        var schemaVersion = "1.0";
        var notes = "Initial Ingestion Attempt";

        // The solution serves as the root container for all projects, files, and code elements within the codebase being analyzed.
        // TODO: Move this path into configuration.
        var solution = await LoadSolutionFromDirectoryAsync(@"D:\SkApiRepo\semantic-kernel\dotnet\src", CancellationToken.None).ConfigureAwait(false);

        // Begin ingestion run.
        Guid ingestionRunId;
        {
            Tuple<Guid?> ingestionRunIdResult = await _db.SpBeginIngestionRunAsync(schemaVersion, notes, Guid.NewGuid()).ConfigureAwait(false);
            ingestionRunId = ingestionRunIdResult.Item1 ?? throw new InvalidOperationException("sp_BeginIngestionRun did not return an ingestion run id.");
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

            sourceSnapshotId = snapshotIdResult.Item1 ?? throw new InvalidOperationException("CreateSourceSnapshot did not return a snapshot id.");
        }

        // Harvest symbols.
        List<ApiType> apiTypes = new();
        await ExtractTypesAsync(solution, apiTypes, CancellationToken.None).ConfigureAwait(false);

        // Build type symbol map now that types are extracted.
        var roslynTypeSymbolsByUid = await BuildTypeSymbolMapAsync(
                solution,
                apiTypes.Select(t => t.SemanticUid),
                CancellationToken.None)
            .ConfigureAwait(false);

        // Members are the functional components of the API.
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

        // Parameters define the inputs required by API members (primarily methods).
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

        foreach (ApiType type in apiTypes)
        {
            if (string.IsNullOrWhiteSpace(type.SemanticUid))
            {
                continue;
            }

            // Ensure IDs exist for relationship wiring.
            if (type.Id == Guid.Empty)
            {
                type.Id = Guid.NewGuid();
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

        foreach (ApiMember member in apiMembers)
        {
            if (string.IsNullOrWhiteSpace(member.SemanticUid))
            {
                continue;
            }

            if (member.Id == Guid.Empty)
            {
                member.Id = Guid.NewGuid();
            }

            // Ensure the member's ApiFeatureId references the type we just upserted.
            if (member.ApiFeatureId == Guid.Empty && !string.IsNullOrWhiteSpace(member.SemanticUid))
            {
                // ExtractMembersAsync set ApiTypeId from the type's Id; but if that's not stable, re-map via semantic uid.
                // (No additional Roslyn work here; just ensure the id is not empty.)
                // If it is still empty, skip.
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

        foreach (ApiParameter p in apiParameters)
        {
            if (p.Id == Guid.Empty)
            {
                p.Id = Guid.NewGuid();
            }

            if (p.ApiMemberId == Guid.Empty)
            {
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
        // There are EF models and DB sprocs available:
        // - ApiFeature (+ FeatureTypeLink/FeatureMemberLink/FeatureDocLink)
        // - DocPage (+ DocSection + CodeBlock)
        // Implement after the symbol->feature/doc mapping rules are finalized.

        await _db.SpEndIngestionRunAsync(ingestionRunId).ConfigureAwait(false);
    }
}