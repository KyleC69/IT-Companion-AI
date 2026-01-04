// Project Name: SKAgent
// File Name: ApiHarvester.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections.Immutable;

using ITCompanionAI.AgentFramework.Models;
using ITCompanionAI.AIVectorDb;
using ITCompanionAI.DatabaseContext;
using ITCompanionAI.Helpers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AgentFramework.Ingestion;


/// <summary>
///     Harvests API surface information from a GitHub repository by downloading source, loading it into a Roslyn
///     workspace,
///     and extracting types, members, and parameters into <see cref="api_type" />, <see cref="api_member" />, and
///     <see cref="api_parameter" /> tables.
/// </summary>
public sealed class ApiHarvester : GitHubRoslynHarvesterBase
{
    private readonly AIAgentRagContext _db;





    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiHarvester" /> class.
    /// </summary>
    /// <param name="db">EF Core DbContext used for persistence.</param>
    /// <param name="gitHubClientFactory">Factory for creating an authenticated GitHub client.</param>
    public ApiHarvester(AIAgentRagContext db, IGitHubClientFactory gitHubClientFactory)
        : base(gitHubClientFactory)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }





    /// <summary>
    ///     Downloads a repository, builds Roslyn semantic models, and persists the API tables.
    /// </summary>
    /// <param name="sourceSnapshotId">Snapshot id that all harvested types will be associated to.</param>
    /// <param name="owner">GitHub organization/user.</param>
    /// <param name="repo">Repository name.</param>
    /// <param name="branch">Branch or ref.</param>
    /// <param name="destinationDirectory">Destination local directory (will be created).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task HarvestAsync(
        Guid sourceSnapshotId,
        string owner,
        string repo,
        string branch,
        string destinationDirectory,
        CancellationToken cancellationToken)
    {
        if (sourceSnapshotId == Guid.Empty)
        {
            throw new ArgumentException("Snapshot id must be non-empty.", nameof(sourceSnapshotId));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(owner);
        ArgumentException.ThrowIfNullOrWhiteSpace(repo);
        ArgumentException.ThrowIfNullOrWhiteSpace(branch);
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationDirectory);

        static bool IncludePath(string path)
        {
            // Keep it tight: only C# and build/workspace files.
            return path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
                   || path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                   || path.EndsWith(".sln", StringComparison.OrdinalIgnoreCase)
                   || path.EndsWith(".props", StringComparison.OrdinalIgnoreCase)
                   || path.EndsWith(".targets", StringComparison.OrdinalIgnoreCase)
                   || path.EndsWith("global.json", StringComparison.OrdinalIgnoreCase)
                   || path.EndsWith("Directory.Build.props", StringComparison.OrdinalIgnoreCase)
                   || path.EndsWith("Directory.Build.targets", StringComparison.OrdinalIgnoreCase);
        }

        await DownloadRepositoryAsync(owner, repo, branch, destinationDirectory, IncludePath, cancellationToken)
            .ConfigureAwait(false);

        Solution solution = await LoadSolutionAsync(destinationDirectory, cancellationToken).ConfigureAwait(false);

        // Index existing for idempotent-ish reruns per snapshot.
        Dictionary<string, api_type> existingTypeByHash = await _db.api_types
            .Where(t => t.source_snapshot_id == sourceSnapshotId && t.type_uid_hash != null)
            .ToDictionaryAsync(t => t.type_uid_hash!, cancellationToken)
            .ConfigureAwait(false);

        Dictionary<Guid, HashSet<string>> existingMemberHashByTypeId = await _db.api_members
            .Where(m => m.member_uid_hash != null)
            .GroupBy(m => m.api_type_id)
            .ToDictionaryAsync(g => g.Key,
                g => g.Select(x => x.member_uid_hash!).ToHashSet(StringComparer.OrdinalIgnoreCase), cancellationToken)
            .ConfigureAwait(false);

        foreach (Project project in solution.Projects)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Compilation? compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
            if (compilation is null)
            {
                continue;
            }

            foreach (SyntaxTree tree in compilation.SyntaxTrees)
            {
                cancellationToken.ThrowIfCancellationRequested();

                SemanticModel semanticModel = compilation.GetSemanticModel(tree, true);
                SyntaxNode root = await tree.GetRootAsync(cancellationToken).ConfigureAwait(false);

                // class/struct/interface/record declarations
                foreach (BaseTypeDeclarationSyntax typeDecl in root.DescendantNodes()
                             .OfType<BaseTypeDeclarationSyntax>())
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var typeSymbol =
                        ModelExtensions.GetDeclaredSymbol(semanticModel, typeDecl, cancellationToken) as
                            INamedTypeSymbol;
                    if (typeSymbol is null || !IsApiEligible(typeSymbol))
                    {
                        continue;
                    }

                    var typeUid = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    if (string.IsNullOrWhiteSpace(typeUid))
                    {
                        continue;
                    }

                    var typeUidHash = Sha256Hex(typeUid);

                    if (!existingTypeByHash.TryGetValue(typeUidHash, out api_type? typeEntity))
                    {
                        typeEntity = CreateTypeEntity(sourceSnapshotId, typeSymbol, typeUid, typeUidHash);
                        existingTypeByHash[typeUidHash] = typeEntity;
                        _db.api_types.Add(typeEntity);
                    }

                    AddMembers(typeEntity, typeSymbol, existingMemberHashByTypeId);
                }
            }
        }

        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }





    private static bool IsApiEligible(INamedTypeSymbol typeSymbol)
    {
        if (typeSymbol.IsImplicitlyDeclared || typeSymbol.IsAnonymousType)
        {
            return false;
        }

        // Avoid error symbols.
        if (typeSymbol.TypeKind is TypeKind.Error)
        {
            return false;
        }

        return true;
    }





    private api_type CreateTypeEntity(Guid sourceSnapshotId, INamedTypeSymbol typeSymbol, string typeUid,
        string typeUidHash)
    {
        ApiSourceLocation? loc = SourceLocator.From(typeSymbol);

        return new api_type
        {
            id = Guid.NewGuid(),
            source_snapshot_id = sourceSnapshotId,
            type_uid = typeUid,
            type_uid_hash = typeUidHash,
            name = Truncate(typeSymbol.Name, 400),
            _namespace = Truncate(typeSymbol.ContainingNamespace?.ToDisplayString(), 400),
            kind = Truncate(typeSymbol.TypeKind.ToString(), 200),
            accessibility = Truncate(typeSymbol.DeclaredAccessibility.ToString(), 200),
            is_static = typeSymbol.IsStatic,
            is_generic = typeSymbol.IsGenericType,
            is_abstract = typeSymbol.IsAbstract,
            is_sealed = typeSymbol.IsSealed,
            is_record = typeSymbol.IsRecord,
            is_ref_like = typeSymbol.IsRefLikeType,
            base_type = Truncate(typeSymbol.BaseType?.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                400),
            interfaces = typeSymbol.Interfaces.Length == 0
                ? null
                : string.Join(";",
                    typeSymbol.Interfaces.Select(i => i.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))),
            containing_type_uid = typeSymbol.ContainingType is null
                ? null
                : Truncate(typeSymbol.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), 400),
            generic_parameters = typeSymbol.IsGenericType
                ? string.Join(",", typeSymbol.TypeParameters.Select(p => p.Name))
                : null,
            generic_constraints = typeSymbol.IsGenericType
                ? string.Join(";", typeSymbol.TypeParameters.Select(GetTypeParamConstraints))
                : null,
            attributes = GetAttributes(typeSymbol),
            source_file_path = loc?.FilePath,
            source_start_line = loc?.StartLine,
            source_end_line = loc?.EndLine
        };
    }





    private void AddMembers(
        api_type typeEntity,
        INamedTypeSymbol typeSymbol,
        Dictionary<Guid, HashSet<string>> existingMemberHashByTypeId)
    {
        if (!existingMemberHashByTypeId.TryGetValue(typeEntity.id, out HashSet<string>? existingHashes))
        {
            existingHashes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            existingMemberHashByTypeId[typeEntity.id] = existingHashes;
        }

        foreach (ISymbol symbol in typeSymbol.GetMembers())
        {
            if (symbol.IsImplicitlyDeclared)
            {
                continue;
            }

            if (symbol is INamedTypeSymbol)
            {
                // nested types harvested separately as api_type
                continue;
            }

            // Skip accessor methods; use property symbols instead.
            if (symbol is IMethodSymbol { MethodKind: MethodKind.PropertyGet or MethodKind.PropertySet })
            {
                continue;
            }

            // Skip static constructors.
            if (symbol is IMethodSymbol { MethodKind: MethodKind.StaticConstructor })
            {
                continue;
            }

            var memberUid = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (string.IsNullOrWhiteSpace(memberUid))
            {
                continue;
            }

            var memberUidHash = Sha256Hex(memberUid);
            if (existingHashes.Contains(memberUidHash))
            {
                continue;
            }

            api_member memberEntity = CreateMemberEntity(typeEntity.id, symbol, memberUid, memberUidHash);
            _db.api_members.Add(memberEntity);
            existingHashes.Add(memberUidHash);

            if (symbol is IMethodSymbol method)
            {
                AddParameters(memberEntity.id, method.Parameters);
            }

            if (symbol is IPropertySymbol prop && prop.IsIndexer)
            {
                AddParameters(memberEntity.id, prop.Parameters);
            }
        }
    }





    private api_member CreateMemberEntity(Guid typeId, ISymbol symbol, string memberUid, string memberUidHash)
    {
        ApiSourceLocation? loc = SourceLocator.From(symbol);

        string? returnType = null;
        string? returnNullable = null;
        bool? isAsync = null;
        string? methodKind = null;

        switch (symbol)
        {
            case IMethodSymbol method:
                returnType = Truncate(method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                    400);
                returnNullable = method.ReturnNullableAnnotation.ToString();
                methodKind = Truncate(method.MethodKind.ToString(), 200);
                isAsync = method.IsAsync || LooksLikeTask(method.ReturnType);
                break;


            case IPropertySymbol prop:
                returnType = Truncate(prop.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), 400);
                returnNullable = prop.NullableAnnotation.ToString();
                break;


            case IFieldSymbol field:
                returnType = Truncate(field.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), 400);
                returnNullable = field.NullableAnnotation.ToString();
                break;


            case IEventSymbol ev:
                returnType = Truncate(ev.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), 400);
                break;
        }

        return new api_member
        {
            id = Guid.NewGuid(),
            api_type_id = typeId,
            member_uid = memberUid,
            member_uid_hash = memberUidHash,
            name = Truncate(symbol.Name, 400),
            kind = Truncate(symbol.Kind.ToString(), 200),
            method_kind = methodKind,
            accessibility = Truncate(symbol.DeclaredAccessibility.ToString(), 200),
            is_static = symbol.IsStatic,
            is_virtual = symbol is IMethodSymbol m && m.IsVirtual,
            is_override = symbol is IMethodSymbol m2 && m2.IsOverride,
            is_abstract = symbol.IsAbstract,
            is_sealed = symbol.IsSealed,
            is_readonly = symbol is IFieldSymbol f && f.IsReadOnly,
            is_const = symbol is IFieldSymbol f2 && f2.IsConst,
            is_unsafe = symbol is IMethodSymbol m3 && m3.DeclaringSyntaxReferences.Any(syntaxRef =>
            {
                SyntaxNode syntax = syntaxRef.GetSyntax();
                return syntax is MethodDeclarationSyntax methodSyntax &&
                       methodSyntax.Modifiers.Any(mod => mod.IsKind(SyntaxKind.UnsafeKeyword));
            }),
            is_extension_method = symbol is IMethodSymbol m4 && m4.IsExtensionMethod,
            is_async = isAsync,
            return_type = returnType,
            return_nullable = Truncate(returnNullable, 50),
            generic_parameters = symbol is IMethodSymbol gm && gm.IsGenericMethod
                ? string.Join(",", gm.TypeParameters.Select(p => p.Name))
                : null,
            generic_constraints = symbol is IMethodSymbol gm2 && gm2.IsGenericMethod
                ? string.Join(";", gm2.TypeParameters.Select(GetTypeParamConstraints))
                : null,
            attributes = GetAttributes(symbol),
            source_file_path = loc?.FilePath,
            source_start_line = loc?.StartLine,
            source_end_line = loc?.EndLine
        };
    }





    private void AddParameters(Guid apiMemberId, IEnumerable<IParameterSymbol> parameters)
    {
        var pos = 0;
        foreach (IParameterSymbol p in parameters)
        {
            var entity = new api_parameter
            {
                id = Guid.NewGuid(),
                api_member_id = apiMemberId,
                name = Truncate(p.Name, 200),
                type = Truncate(p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), 400),
                nullable_annotation = Truncate(p.NullableAnnotation.ToString(), 50),
                position = pos,
                modifier = Truncate(GetParamModifier(p), 50),
                has_default_value = p.HasExplicitDefaultValue,
                default_value_literal = p.HasExplicitDefaultValue && p.ExplicitDefaultValue is not null
                    ? p.ExplicitDefaultValue.ToString()
                    : null
            };

            _db.api_parameters.Add(entity);
            pos++;
        }
    }





    private static bool LooksLikeTask(ITypeSymbol returnType)
    {
        var display = returnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        return display.StartsWith("global::System.Threading.Tasks.Task", StringComparison.Ordinal)
               || display.StartsWith("global::System.Threading.Tasks.ValueTask", StringComparison.Ordinal);
    }





    private static string? GetAttributes(ISymbol symbol)
    {
        ImmutableArray<AttributeData> attrs = symbol.GetAttributes();
        if (attrs.Length == 0)
        {
            return null;
        }

        return string.Join(
            ";",
            attrs.Select(a => a.AttributeClass?.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .Where(s => !string.IsNullOrWhiteSpace(s)));
    }





    private static string GetTypeParamConstraints(ITypeParameterSymbol tp)
    {
        List<string> parts = new();

        if (tp.HasConstructorConstraint)
        {
            parts.Add("new()");
        }

        if (tp.HasReferenceTypeConstraint)
        {
            parts.Add("class");
        }

        if (tp.HasUnmanagedTypeConstraint)
        {
            parts.Add("unmanaged");
        }

        if (tp.HasValueTypeConstraint)
        {
            parts.Add("struct");
        }

        foreach (ITypeSymbol ct in tp.ConstraintTypes)
            parts.Add(ct.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));

        return parts.Count == 0 ? string.Empty : string.Join(",", parts);
    }





    private static string? GetParamModifier(IParameterSymbol p)
    {
        if (p.IsParams)
        {
            return "params";
        }

        return p.RefKind switch
        {
            RefKind.In => "in",
            RefKind.Out => "out",
            RefKind.Ref => "ref",
            RefKind.RefReadOnlyParameter => "ref readonly",
            _ => null
        };
    }





    private static string? Truncate(string? value, int maxLen)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLen)
        {
            return value;
        }

        return value[..maxLen];
    }
}