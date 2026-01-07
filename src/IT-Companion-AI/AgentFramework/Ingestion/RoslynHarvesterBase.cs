// Project Name: SKAgent
// File Name: RoslynHarvesterBase.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

using ITCompanionAI.Entities;
using ITCompanionAI.Helpers;
using ITCompanionAI.KBCurator;
using ITCompanionAI.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Octokit;

using Project = Microsoft.CodeAnalysis.Project;


namespace ITCompanionAI.AgentFramework.Ingestion;


/// <summary>
///     Base class for harvesters that need to retrieve source code from GitHub and analyze it with Roslyn workspaces.
/// </summary>
public abstract class RoslynHarvesterBase : CSharpSyntaxWalker
{
    private readonly IGitHubClientFactory _gitHubClientFactory;







    public RoslynHarvesterBase()
    {
    }







    /// <summary>
    ///     Initializes a new instance of the <see cref="RoslynHarvesterBase" /> class.
    /// </summary>
    /// <param name="gitHubClientFactory">Factory for creating authenticated GitHub clients.</param>
    protected RoslynHarvesterBase(IGitHubClientFactory gitHubClientFactory)
    {
        _gitHubClientFactory = gitHubClientFactory ?? throw new ArgumentNullException(nameof(gitHubClientFactory));
    }







    /// <summary>
    ///     Downloads repository contents for a given branch into a local directory.
    /// </summary>
    /// <remarks>
    ///     This implementation uses the GitHub Contents API, traversing directories recursively.
    ///     It is intended for small to medium repositories; for large repositories consider using archive downloads.
    /// </remarks>
    protected async Task DownloadRepositoryAsync(
        string owner,
        string repo,
        string branch,
        string destinationDirectory,
        Func<string, bool> includePath,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(owner);
        ArgumentException.ThrowIfNullOrWhiteSpace(repo);
        ArgumentException.ThrowIfNullOrWhiteSpace(branch);
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationDirectory);
        includePath ??= static _ => true;

        Directory.CreateDirectory(destinationDirectory);

        GitHubClient client = _gitHubClientFactory.CreateClient();
    }







    /// <summary>
    ///     Creates a Roslyn <see cref="Solution" /> for a local repository directory.
    /// </summary>
    /// <remarks>
    ///     This uses <see cref="AdhocWorkspace" /> to avoid requiring MSBuild tooling.
    ///     It parses all .cs files and creates a single project with basic framework references.
    /// </remarks>
    /// <param name="repositoryDirectory">Directory containing source files.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created solution.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected static async Task<Solution> LoadSolutionFromDirectoryAsync(string repositoryDirectory,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(repositoryDirectory);

        List<string> csFiles = Directory.EnumerateFiles(repositoryDirectory, "*.cs", SearchOption.AllDirectories)
            .ToList();
        if (csFiles.Count == 0)
        {
            throw new InvalidOperationException($"No C# source files found under '{repositoryDirectory}'.");
        }

        using var workspace = new AdhocWorkspace();

        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(
            projectId,
            VersionStamp.Create(),
            "Repo",
            "Repo",
            LanguageNames.CSharp,
            compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
            parseOptions: new CSharpParseOptions(LanguageVersion.Latest));

        Solution solution = workspace.CurrentSolution.AddProject(projectInfo);

        var refs = new[]
        {
            typeof(object).Assembly.Location,
            typeof(Enumerable).Assembly.Location,
            typeof(Task).Assembly.Location
        };

        foreach (var r in refs.Distinct(StringComparer.OrdinalIgnoreCase))
            solution = solution.AddMetadataReference(projectId, MetadataReference.CreateFromFile(r));

        foreach (var file in csFiles)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var text = await File.ReadAllTextAsync(file, cancellationToken).ConfigureAwait(false);
            var docId = DocumentId.CreateNewId(projectId);
            solution = solution.AddDocument(DocumentInfo.Create(
                docId,
                Path.GetFileName(file),
                loader: TextLoader.From(TextAndVersion.Create(SourceText.From(text, Encoding.UTF8),
                    VersionStamp.Create(), file)),
                filePath: file));
        }

        return solution;
    }







    /// <summary>
    ///     Computes a stable 64-char hex SHA-256 hash for uniqueness columns.
    /// </summary>
    protected static string Sha256Hex(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }







    public static async Task ExtractTypesAsync(
        Solution solution,
        List<ApiType> output,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(solution);
        ArgumentNullException.ThrowIfNull(output);

        foreach (Project project in solution.Projects)
        {
            ct.ThrowIfCancellationRequested();

            Compilation? compilation = await project.GetCompilationAsync(ct).ConfigureAwait(false);
            if (compilation is null)
            {
                continue;
            }

            foreach (Document document in project.Documents)
            {
                ct.ThrowIfCancellationRequested();

                SyntaxNode? root = await document.GetSyntaxRootAsync(ct).ConfigureAwait(false);
                if (root is null)
                {
                    continue;
                }

                SemanticModel? semanticModel = await document.GetSemanticModelAsync(ct).ConfigureAwait(false);
                if (semanticModel is null)
                {
                    continue;
                }

                IEnumerable<BaseTypeDeclarationSyntax> typeDecls = root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>();
                foreach (BaseTypeDeclarationSyntax typeDecl in typeDecls)
                {
                    ct.ThrowIfCancellationRequested();

                    INamedTypeSymbol? symbol;
                    try
                    {
                        symbol = semanticModel.GetDeclaredSymbol(typeDecl, ct);
                    }
                    catch
                    {
                        continue;
                    }

                    if (symbol is null)
                    {
                        continue;
                    }

                    if (symbol.TypeKind is TypeKind.Error)
                    {
                        continue;
                    }

                    var semanticUid = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    var name = symbol.Name;
                    var ns = symbol.ContainingNamespace?.IsGlobalNamespace == true
                        ? null
                        : symbol.ContainingNamespace?.ToDisplayString();

                    var kind = symbol.TypeKind.ToString();
                    var accessibility = symbol.DeclaredAccessibility.ToString();

                    bool? isStatic = symbol.IsStatic;
                    bool? isGeneric = symbol.IsGenericType;
                    bool? isAbstract = symbol.IsAbstract;
                    bool? isSealed = symbol.IsSealed;
                    bool? isRecord = typeDecl is RecordDeclarationSyntax;
                    bool? isRefLike = symbol.IsRefLikeType;

                    var baseTypeUid = symbol.BaseType is null
                        ? null
                        : symbol.BaseType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                    var interfaces = symbol.Interfaces.Length == 0
                        ? null
                        : string.Join(";", symbol.Interfaces.Select(i => i.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)));

                    var containingTypeUid = symbol.ContainingType is null
                        ? null
                        : symbol.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                    var genericParameters = symbol.TypeParameters.Length == 0
                        ? null
                        : string.Join(",", symbol.TypeParameters.Select(p => p.Name));

                    var genericConstraints = symbol.TypeParameters.Length == 0
                        ? null
                        : string.Join(";", symbol.TypeParameters.Select(p => BuildTypeParameterConstraintString(p))
                            .Where(s => !string.IsNullOrWhiteSpace(s)));

                    var attributes = symbol.GetAttributes().Length == 0
                        ? null
                        : string.Join(";", symbol.GetAttributes().Select(a => a.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
                            .Where(s => !string.IsNullOrWhiteSpace(s)));

                    var xml = symbol.GetDocumentationCommentXml(cancellationToken: ct);
                    var (summary, remarks) = ExtractSummaryAndRemarks(xml);

                    var (filePath, startLine, endLine) = GetSourceSpan(typeDecl.SyntaxTree, typeDecl.Span);

                    Debug.Assert(filePath != null, nameof(filePath) + " != null");
                    output.Add(new ApiType
                    {
                        Id = default,
                        SemanticUid = semanticUid,
                        SourceSnapshotId = default,
                        Name = name,
                        NamespacePath = ns,
                        Kind = kind,
                        Accessibility = accessibility,
                        IsStatic = isStatic,
                        IsGeneric = isGeneric,
                        IsAbstract = isAbstract,
                        IsSealed = isSealed,
                        IsRecord = isRecord,
                        IsRefLike = isRefLike,
                        BaseTypeUid = baseTypeUid,
                        Interfaces = interfaces,
                        ContainingTypeUid = containingTypeUid,
                        GenericParameters = genericParameters,
                        GenericConstraints = genericConstraints,
                        Summary = summary,
                        Remarks = remarks!,
                        Attributes = attributes,
                        SourceFilePath = filePath,
                        SourceStartLine = startLine,
                        SourceEndLine = endLine,
                        VersionNumber = 0,
                        CreatedIngestionRunId = default,
                        UpdatedIngestionRunId = default,
                        RemovedIngestionRunId = null,
                        ValidFromUtc = default,
                        ValidToUtc = null,
                        IsActive = false,
                        ContentHash = new byte[]
                        {
                        },
                        SemanticUidHash = new byte[]
                        {
                        },
                        SourceSnapshot = null,
                        IngestionRun_CreatedIngestionRunId = null,
                        IngestionRun_UpdatedIngestionRunId = null,
                        IngestionRun_RemovedIngestionRunId = null
                    });
                }
            }
        }
    }







    /// <summary>
    ///     Reinstate the correct ExtractMembersAsync overload signature that accepts the `types` list explicitly;
    ///     this matches existing call sites and fixes the undefined `types` usage.
    /// </summary>
    public static async Task ExtractMembersAsync(
        Solution solution,
        IReadOnlyList<ApiType> types,
        IReadOnlyDictionary<string, INamedTypeSymbol> roslynTypeSymbolsByUid,
        List<ApiMember> output,
        Dictionary<string, ISymbol> roslynMemberSymbolsByUid,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(solution);
        ArgumentNullException.ThrowIfNull(types);
        ArgumentNullException.ThrowIfNull(roslynTypeSymbolsByUid);
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(roslynMemberSymbolsByUid);

        foreach (Project project in solution.Projects)
        {
            ct.ThrowIfCancellationRequested();

            Compilation? compilation = await project.GetCompilationAsync(ct).ConfigureAwait(false);
            if (compilation is null)
            {
                continue;
            }

            foreach (ApiType type in types)
            {
                ct.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(type.SemanticUid))
                {
                    continue;
                }

                if (!roslynTypeSymbolsByUid.TryGetValue(type.SemanticUid, out INamedTypeSymbol? typeSymbol) || typeSymbol is null)
                {
                    continue;
                }

                foreach (ISymbol member in typeSymbol.GetMembers())
                {
                    ct.ThrowIfCancellationRequested();

                    if (member.IsImplicitlyDeclared)
                    {
                        continue;
                    }

                    if (member is IMethodSymbol method && method.MethodKind is MethodKind.PropertyGet or MethodKind.PropertySet or MethodKind.EventAdd or MethodKind.EventRemove)
                    {
                        continue;
                    }

                    if (member is IFieldSymbol { AssociatedSymbol: not null })
                    {
                        continue;
                    }

                    if (member.DeclaredAccessibility == Accessibility.NotApplicable)
                    {
                        continue;
                    }

                    SyntaxNode? memberDecl = null;
                    try
                    {
                        memberDecl = member.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax(ct);
                    }
                    catch
                    {
                        memberDecl = null;
                    }

                    var (filePath, startLine, endLine) = memberDecl is null
                        ? (null, null, null)
                        : GetSourceSpan(memberDecl.SyntaxTree, memberDecl.Span);

                    var semanticUid = member.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    var name = member.Name;

                    var kind = member.Kind.ToString();
                    var methodKind = member is IMethodSymbol ms ? ms.MethodKind.ToString() : null;
                    var accessibility = member.DeclaredAccessibility.ToString();

                    bool? isStatic = member.IsStatic;
                    bool? isExtensionMethod = member is IMethodSymbol ext && ext.IsExtensionMethod;
                    bool? isAsync = member is IMethodSymbol asyncM && asyncM.IsAsync;
                    bool? isVirtual = member.IsVirtual;
                    bool? isOverride = member.IsOverride;
                    bool? isAbstract = member.IsAbstract;
                    bool? isSealed = member.IsSealed;
                    bool? isReadOnly = member is IPropertySymbol prop && prop.IsReadOnly;
                    bool? isConst = member is IFieldSymbol field && field.IsConst;
                    bool? isUnsafe = memberDecl is MemberDeclarationSyntax mds && mds.Modifiers.Any(SyntaxKind.UnsafeKeyword);

                    var returnTypeUid = member is IMethodSymbol methodSym
                        ? methodSym.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                        : member is IPropertySymbol propSym
                            ? propSym.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                            : null;

                    var returnNullable = member is IMethodSymbol methodNullable
                        ? methodNullable.ReturnNullableAnnotation.ToString()
                        : member is IPropertySymbol propNullable
                            ? propNullable.NullableAnnotation.ToString()
                            : null;

                    var genericParameters = member is IMethodSymbol genM && genM.TypeParameters.Length > 0
                        ? string.Join(",", genM.TypeParameters.Select(p => p.Name))
                        : null;

                    var genericConstraints = member is IMethodSymbol genMc && genMc.TypeParameters.Length > 0
                        ? string.Join(";", genMc.TypeParameters.Select(p => BuildTypeParameterConstraintString(p))
                            .Where(s => !string.IsNullOrWhiteSpace(s)))
                        : null;

                    var attributes = member.GetAttributes().Length == 0
                        ? null
                        : string.Join(";", member.GetAttributes().Select(a => a.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
                            .Where(s => !string.IsNullOrWhiteSpace(s)));

                    var xml = member.GetDocumentationCommentXml(cancellationToken: ct);
                    var (summary, remarks) = ExtractSummaryAndRemarks(xml);

                    var efMember = new ApiMember
                    {
                        SemanticUid = semanticUid,
                        ApiTypeId = type.Id,
                        Name = name,
                        Kind = kind,
                        MethodKind = methodKind,
                        Accessibility = accessibility,
                        IsStatic = isStatic,
                        IsExtensionMethod = isExtensionMethod,
                        IsAsync = isAsync,
                        IsVirtual = isVirtual,
                        IsOverride = isOverride,
                        IsAbstract = isAbstract,
                        IsSealed = isSealed,
                        IsReadonly = isReadOnly,
                        IsConst = isConst,
                        IsUnsafe = isUnsafe,
                        ReturnTypeUid = returnTypeUid,
                        ReturnNullable = returnNullable,
                        GenericParameters = genericParameters,
                        GenericConstraints = genericConstraints!,
                        Summary = summary,
                        Remarks = remarks,
                        Attributes = attributes,
                        SourceFilePath = filePath,
                        SourceStartLine = startLine,
                        SourceEndLine = endLine
                    };

                    output.Add(efMember);
                    roslynMemberSymbolsByUid[semanticUid] = member;
                }
            }
        }
    }







    public static Task ExtractParametersAsync(
        Solution solution,
        IReadOnlyList<ApiMember> members,
        IReadOnlyDictionary<string, ISymbol> roslynMemberSymbolsByUid,
        List<ApiParameter> output,
        CancellationToken ct)
    {
        _ = solution;

        ArgumentNullException.ThrowIfNull(members);
        ArgumentNullException.ThrowIfNull(roslynMemberSymbolsByUid);
        ArgumentNullException.ThrowIfNull(output);

        foreach (ApiMember member in members)
        {
            ct.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(member.SemanticUid))
            {
                continue;
            }

            if (!roslynMemberSymbolsByUid.TryGetValue(member.SemanticUid, out ISymbol? symbol) || symbol is not IMethodSymbol method)
            {
                continue;
            }

            for (var i = 0; i < method.Parameters.Length; i++)
            {
                ct.ThrowIfCancellationRequested();

                IParameterSymbol p = method.Parameters[i];

                var modifier = p.RefKind != RefKind.None
                    ? p.RefKind.ToString().ToLowerInvariant()
                    : null;

                var hasDefault = p.HasExplicitDefaultValue;
                var defaultLiteral = hasDefault ? p.ExplicitDefaultValue?.ToString() : null;

                output.Add(new ApiParameter
                {
                    ApiMemberId = member.Id,
                    Name = p.Name,
                    TypeUid = p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    NullableAnnotation = p.NullableAnnotation.ToString(),
                    Position = p.Ordinal,
                    Modifier = modifier,
                    HasDefaultValue = hasDefault,
                    DefaultValueLiteral = defaultLiteral
                });
            }
        }

        return Task.CompletedTask;
    }







    /// <summary>
    ///     Walks an entire Roslyn <see cref="Solution" /> and builds a single object representing the complete extracted API
    ///     surface.
    /// </summary>
    /// <param name="solution">The Roslyn solution to analyze.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A collection of syntax type trees covering every discovered type.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<IEnumerable<SyntaxTypeTree>> WalkSolutionAsync(Solution solution, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(solution);

        List<ApiType> types = new();
        await ExtractTypesAsync(solution, types, ct).ConfigureAwait(false);

        // Dedupe types by semantic uid to avoid multiple declarations (partials) across documents.
        List<ApiType> uniqueTypes = types
            .Where(t => !string.IsNullOrWhiteSpace(t.SemanticUid))
            .GroupBy(t => t.SemanticUid, StringComparer.Ordinal)
            .Select(g => g.OrderByDescending(x => x.SourceFilePath is not null)
                .ThenBy(x => x.SourceFilePath, StringComparer.OrdinalIgnoreCase)
                .First())
            .ToList();

        // Build Roslyn type symbol map once.
        Dictionary<string, INamedTypeSymbol> roslynTypeSymbolsByUid = await BuildTypeSymbolMapAsync(solution, uniqueTypes.Select(t => t.SemanticUid), ct).ConfigureAwait(false);

        // Assign deterministic Ids so members/parameters can link without needing a DbContext.
        foreach (ApiType t in uniqueTypes)
            if (t.Id == Guid.Empty)
            {
                t.Id = Guid.NewGuid();
            }

        Dictionary<string, ISymbol> roslynMemberSymbolsByUid = new(StringComparer.Ordinal);
        List<ApiMember> members = new(Math.Max(128, uniqueTypes.Count * 8));
        await ExtractMembersAsync(solution, uniqueTypes, roslynTypeSymbolsByUid, members, roslynMemberSymbolsByUid, ct).ConfigureAwait(false);

        // Dedupe members by semantic uid.
        List<ApiMember> uniqueMembers = members
            .Where(m => !string.IsNullOrWhiteSpace(m.SemanticUid))
            .GroupBy(m => m.SemanticUid, StringComparer.Ordinal)
            .Select(g => g.OrderByDescending(x => x.SourceFilePath is not null)
                .ThenBy(x => x.SourceFilePath, StringComparer.OrdinalIgnoreCase)
                .First())
            .ToList();

        foreach (ApiMember m in uniqueMembers)
            if (m.Id == Guid.Empty)
            {
                m.Id = Guid.NewGuid();
            }

        List<ApiParameter> parameters = new(Math.Max(256, uniqueMembers.Count * 2));
        await ExtractParametersAsync(solution, uniqueMembers, roslynMemberSymbolsByUid, parameters, ct).ConfigureAwait(false);

        // Group for fast assembly per type.
        Dictionary<Guid, IReadOnlyList<ApiMember>> membersByType = uniqueMembers
            .GroupBy(m => m.ApiTypeId)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<ApiMember>)g.ToList());

        Dictionary<Guid, IReadOnlyList<ApiParameter>> parametersByMember = parameters
            .GroupBy(p => p.ApiMemberId)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<ApiParameter>)g.OrderBy(x => x.Position).ThenBy(x => x.Name, StringComparer.Ordinal).ToList());

        List<SyntaxTypeTree> trees = new(uniqueTypes.Count);
        foreach (ApiType t in uniqueTypes.OrderBy(t => t.SemanticUid, StringComparer.Ordinal))
        {
            ct.ThrowIfCancellationRequested();

            membersByType.TryGetValue(t.Id, out IReadOnlyList<ApiMember>? typeMembers);
            typeMembers ??= Array.Empty<ApiMember>();

            List<ApiParameter> typeParameters = new();
            if (typeMembers.Count > 0)
            {
                foreach (ApiMember m in typeMembers)
                    if (parametersByMember.TryGetValue(m.Id, out IReadOnlyList<ApiParameter>? mp))
                    {
                        typeParameters.AddRange(mp);
                    }
            }

            trees.Add(new SyntaxTypeTree(t, typeMembers.ToList(), typeParameters));
        }

        return trees;
    }







    public static async Task<Dictionary<string, INamedTypeSymbol>> BuildTypeSymbolMapAsync(
        Solution solution,
        IEnumerable<string> typeSemanticUids,
        CancellationToken ct)
    {
        HashSet<string> typeUidSet = new(typeSemanticUids.Where(u => !string.IsNullOrWhiteSpace(u)), StringComparer.Ordinal);
        Dictionary<string, INamedTypeSymbol> result = new(StringComparer.Ordinal);

        if (typeUidSet.Count == 0)
        {
            return result;
        }

        foreach (Project project in solution.Projects)
        {
            ct.ThrowIfCancellationRequested();

            Compilation? compilation = await project.GetCompilationAsync(ct).ConfigureAwait(false);
            if (compilation is null)
            {
                continue;
            }

            foreach (Document document in project.Documents)
            {
                ct.ThrowIfCancellationRequested();

                SyntaxNode? root = await document.GetSyntaxRootAsync(ct).ConfigureAwait(false);
                if (root is null)
                {
                    continue;
                }

                SemanticModel? semanticModel = await document.GetSemanticModelAsync(ct).ConfigureAwait(false);
                if (semanticModel is null)
                {
                    continue;
                }

                foreach (BaseTypeDeclarationSyntax typeDecl in root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
                {
                    ct.ThrowIfCancellationRequested();

                    INamedTypeSymbol? symbol;
                    try
                    {
                        symbol = semanticModel.GetDeclaredSymbol(typeDecl, ct);
                    }
                    catch
                    {
                        continue;
                    }

                    if (symbol is null)
                    {
                        continue;
                    }

                    var uid = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    if (!typeUidSet.Contains(uid))
                    {
                        continue;
                    }

                    // Prefer the first-seen declaration per project; higher-level dedupe happens in WalkSolutionAsync.
                    result.TryAdd(uid, symbol);
                }
            }
        }

        return result;
    }







    private static (string? Summary, string? Remarks) ExtractSummaryAndRemarks(string? xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            return (null, null);
        }

        try
        {
            var doc = XDocument.Parse(xml);
            var summary = doc.Root?.Element("summary")?.Value?.Trim();
            var remarks = doc.Root?.Element("remarks")?.Value?.Trim();
            return (string.IsNullOrWhiteSpace(summary) ? null : summary,
                string.IsNullOrWhiteSpace(remarks) ? null : remarks);
        }
        catch
        {
            return (null, null);
        }
    }







    public static (string? FilePath, int? StartLine, int? EndLine) GetSourceSpan(SyntaxTree tree, TextSpan span)
    {
        if (tree is null)
        {
            return (null, null, null);
        }

        FileLinePositionSpan lineSpan = tree.GetLineSpan(span);
        var startLine = lineSpan.StartLinePosition.Line + 1;
        var endLine = lineSpan.EndLinePosition.Line + 1;
        return (tree.FilePath, startLine, endLine);
    }







    public static string? BuildTypeParameterConstraintString(ITypeParameterSymbol p)
    {
        if (p is null)
        {
            return null;
        }

        List<string> parts = new();

        if (p.HasReferenceTypeConstraint)
        {
            parts.Add("class");
        }

        if (p.HasValueTypeConstraint)
        {
            parts.Add("struct");
        }

        if (p.HasNotNullConstraint)
        {
            parts.Add("notnull");
        }

        if (p.HasUnmanagedTypeConstraint)
        {
            parts.Add("unmanaged");
        }

        foreach (ITypeSymbol t in p.ConstraintTypes)
            parts.Add(t.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

        if (p.HasConstructorConstraint)
        {
            parts.Add("new()");
        }

        return parts.Count == 0 ? null : $"{p.Name}:{string.Join(",", parts)}";
    }
}




public class ApiSyntaxTreeCollections : IReadOnlyList<SyntaxTypeTree>
{
    private readonly List<SyntaxTypeTree> _trees;







    public ApiSyntaxTreeCollections(IEnumerable<SyntaxTypeTree> trees)
    {
        _trees = trees.ToList();
    }







    public SyntaxTypeTree this[int index] => _trees[index];

    public int Count => _trees.Count;







    public IEnumerator<SyntaxTypeTree> GetEnumerator()
    {
        return _trees.GetEnumerator();
    }







    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}