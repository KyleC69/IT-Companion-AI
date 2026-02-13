using System.Security.Cryptography;
using System.Text.Json;

using ITCompanionAI.Services;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;

using Octokit;

using Document = Microsoft.CodeAnalysis.Document;
using Project = Microsoft.CodeAnalysis.Project;




namespace ITCompanionAI.Ingestion.API;





public class RoslynHarvesterBase : CSharpSyntaxWalker
{


    private static readonly string[] ReferenceAssemblyFileNames =
    {
            "System.Runtime.dll",
            "System.Collections.dll",
            "System.Linq.dll",
            "System.Threading.Tasks.dll",
            "System.Console.dll",
            "netstandard.dll"
    };

    private readonly IGitHubClientFactory _gitHubClientFactory;








    /// <summary>
    ///     Initializes a new instance of the <see cref="RoslynHarvesterBase" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor exists to support derived types that may rely on alternative initialization patterns
    ///     (for example DI containers that set fields/properties later).
    /// </remarks>
    public RoslynHarvesterBase()
    {
    }








    /// <summary>
    ///     Initializes a new instance of the <see cref="RoslynHarvesterBase" /> class.
    /// </summary>
    /// <param name="gitHubClientFactory">Factory for creating authenticated GitHub clients.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="gitHubClientFactory" /> is <c>null</c>.
    /// </exception>
    protected RoslynHarvesterBase(IGitHubClientFactory gitHubClientFactory)
    {
        _gitHubClientFactory = gitHubClientFactory ?? throw new ArgumentNullException(nameof(gitHubClientFactory));
    }








    /// <summary>
    ///     Downloads repository contents for a given branch into a local directory.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method is intended to hydrate a local working directory that can then be parsed by Roslyn.
    ///     </para>
    ///     <para>
    ///         The planned implementation uses the GitHub Contents API via <see cref="GitHubClient" /> and recursively
    ///         traverses
    ///         directories to download files. Callers may filter paths via <paramref name="includePath" /> to ignore
    ///         non-source content (for example build artifacts, docs, large assets, etc.).
    ///     </para>
    ///     <para>
    ///         Intended for small-to-medium repositories. For very large repositories, archive downloads or git clone are
    ///         typically
    ///         faster than enumerating every item using the Contents API.
    ///     </para>
    /// </remarks>
    /// <param name="owner">The GitHub account/organization that owns the repository.</param>
    /// <param name="repo">The repository name.</param>
    /// <param name="branch">The branch to download (for example <c>main</c>).</param>
    /// <param name="destinationDirectory">Local directory where content is written.</param>
    /// <param name="includePath">
    ///     Predicate used to include/exclude a path relative to repository root. If <c>null</c>, all paths are included.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="owner" />, <paramref name="repo" />, <paramref name="branch" />, or
    ///     <paramref name="destinationDirectory" /> is null/empty/whitespace.
    /// </exception>
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

        _ = Directory.CreateDirectory(destinationDirectory);

        GitHubClient client = _gitHubClientFactory.CreateClient();

        // NOTE: Implementation intentionally omitted for now. This method's signature and role
        //       are preserved for future GitHub-based ingestion.
    }








    public static async Task<SolutionManifest> LoadManifestAsync(
            string manifestPath,
            CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(manifestPath);

        if (!File.Exists(manifestPath))
        {
            throw new FileNotFoundException($"Manifest file not found: {manifestPath}");
        }

        await using FileStream stream = File.OpenRead(manifestPath);

        SolutionManifest manifest = await JsonSerializer.DeserializeAsync<SolutionManifest>(
                stream,
                new JsonSerializerOptions
                {
                        PropertyNameCaseInsensitive = true,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                },
                cancellationToken);

        return manifest is null ? throw new InvalidOperationException("Failed to deserialize solution manifest.") : manifest;
    }








    public static async Task<Solution> LoadSolutionFromManifestAsync(
            SolutionManifest manifest,
            string repoRoot,
            CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(manifest);
        ArgumentException.ThrowIfNullOrWhiteSpace(repoRoot);
        cancellationToken.ThrowIfCancellationRequested();

        // Required for MSBuildWorkspace to function
        var properties = new Dictionary<string, string>
        {
                ["LoadMetadataForReferencedProjects"] = "false"
        };

        using MSBuildWorkspace workspace = MSBuildWorkspace.Create(properties);
        workspace.LoadMetadataForReferencedProjects = false;

        foreach (var relativeProjectPath in manifest.solution.projects)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(relativeProjectPath))
            {
                continue;
            }

            var fullProjectPath = Path.Combine(repoRoot, relativeProjectPath);

            if (!File.Exists(fullProjectPath))
            {
                throw new FileNotFoundException($"Project file not found: {fullProjectPath}");
            }

            // This loads the project, references, analyzers, and documents

            _ = await workspace.OpenProjectAsync(fullProjectPath, null, cancellationToken);
        }

        Solution solution = workspace.CurrentSolution;

        return !solution.Projects.Any() ? throw new InvalidOperationException("No projects were loaded from the manifest.") : solution;
    }








    private static IEnumerable<string> GetMetadataReferencePaths(string repositoryDirectory = null)
    {
        HashSet<string> yielded = new(StringComparer.OrdinalIgnoreCase);

        var tpa = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string;
        if (!string.IsNullOrWhiteSpace(tpa))
        {
            foreach (var path in tpa.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
            {
                var fileName = Path.GetFileName(path);
                if (fileName is not null
                    && ReferenceAssemblyFileNames.Contains(fileName, StringComparer.OrdinalIgnoreCase)
                    && File.Exists(path)
                    && yielded.Add(path))
                {
                    yield return path;
                }
            }
        }

        string[] fallbackAssemblies =
        {
                typeof(object).Assembly.Location,
                typeof(Enumerable).Assembly.Location,
                typeof(Task).Assembly.Location,
                typeof(Uri).Assembly.Location,
                typeof(ValueTuple<int>).Assembly.Location,
                typeof(IAsyncEnumerable<int>).Assembly.Location
        };

        foreach (var fallback in fallbackAssemblies)
            if (!string.IsNullOrWhiteSpace(fallback) && File.Exists(fallback) && yielded.Add(fallback))
            {
                yield return fallback;
            }

        // NOTE: We no longer depend on bin/obj being present in the repo. This scan
        //       is effectively a no-op for source-only snapshots, but harmless.
        if (!string.IsNullOrWhiteSpace(repositoryDirectory) && Directory.Exists(repositoryDirectory))
        {
            var binDirs = Directory.EnumerateDirectories(repositoryDirectory, "bin", SearchOption.AllDirectories);
            foreach (var binDir in binDirs)
            {
                foreach (var dll in Directory.EnumerateFiles(binDir, "*.dll", SearchOption.AllDirectories))
                    if (File.Exists(dll) && yielded.Add(dll))
                    {
                        yield return dll;
                    }
            }
        }
    }








    private static string GetDocumentName(string repositoryRoot, string filePath)
    {
        try
        {
            var relative = Path.GetRelativePath(repositoryRoot, filePath);
            if (!string.IsNullOrWhiteSpace(relative) && !relative.StartsWith("..", StringComparison.Ordinal))
            {
                return relative.Replace(Path.DirectorySeparatorChar, '/');
            }
        }
        catch
        {
            // Fallback handled below.
        }

        return Path.GetFileName(filePath);
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








    /// <summary>
    ///     Hybrid type extraction: semantic when available, syntactic fallback when semantic binding fails.
    /// </summary>
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

            // Compilation is not strictly required here, but forcing it can help Roslyn cache semantic info.
            _ = await project.GetCompilationAsync(ct).ConfigureAwait(false);

            foreach (Document document in project.Documents)
            {
                ct.ThrowIfCancellationRequested();

                SyntaxNode root = await document.GetSyntaxRootAsync(ct).ConfigureAwait(false);
                if (root is null)
                {
                    continue;
                }

                SemanticModel semanticModel = await document.GetSemanticModelAsync(ct).ConfigureAwait(false);

                var typeDecls = root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>();
                foreach (BaseTypeDeclarationSyntax typeDecl in typeDecls)
                {
                    ct.ThrowIfCancellationRequested();

                    ApiType apiType;

                    INamedTypeSymbol symbol = null;
                    if (semanticModel is not null)
                    {
                        try
                        {
                            symbol = semanticModel.GetDeclaredSymbol(typeDecl, ct);
                        }
                        catch
                        {
                            symbol = null;
                        }
                    }

                    apiType = symbol is not null && symbol.TypeKind is not TypeKind.Error
                            ? CreateApiTypeFromSymbol(symbol, typeDecl, ct)
                            : CreateApiTypeFromSyntax(typeDecl);

                    if (string.IsNullOrWhiteSpace(apiType.SemanticUid))
                    {
                        continue;
                    }

                    output.Add(apiType);
                }
            }
        }
    }








    private static ApiType CreateApiTypeFromSymbol(INamedTypeSymbol symbol, BaseTypeDeclarationSyntax typeDecl, CancellationToken ct)
    {
        var semanticUid = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

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

        var baseTypeUid = symbol.BaseType is null || symbol.BaseType.SpecialType == SpecialType.System_Object
                ? null
                : symbol.BaseType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var interfaces = symbol.AllInterfaces.Length == 0
                ? null
                : string.Join(";",
                        symbol.AllInterfaces
                                .Select(i => i.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
                                .OrderBy(s => s, StringComparer.Ordinal));

        var containingTypeUid = symbol.ContainingType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var genericParameters = symbol.TypeParameters.Length == 0
                ? null
                : string.Join(",",
                        symbol.TypeParameters
                                .Select(p => p.Name)
                                .OrderBy(s => s, StringComparer.Ordinal));

        var genericConstraints = symbol.TypeParameters.Length == 0
                ? null
                : string.Join(";",
                        symbol.TypeParameters
                                .Select(BuildTypeParameterConstraintString)
                                .Where(s => !string.IsNullOrWhiteSpace(s))
                                .OrderBy(s => s, StringComparer.Ordinal));

        var attributes = symbol.GetAttributes().Length == 0
                ? null
                : string.Join(";",
                        symbol.GetAttributes()
                                .Select(a => a.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
                                .Where(s => !string.IsNullOrWhiteSpace(s))
                                .OrderBy(s => s, StringComparer.Ordinal));

        var xml = symbol.GetDocumentationCommentXml(cancellationToken: ct);
        var (summary, remarks) = ExtractSummaryAndRemarks(xml);

        var (filePath, startLine, endLine) = GetSourceSpan(typeDecl.SyntaxTree, typeDecl.Span);

        return new ApiType
        {
                SemanticUid = semanticUid,
                SourceSnapshotId = default,
                Name = symbol.Name,
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
                Remarks = remarks,
                Attributes = attributes,
                SourceFilePath = NormalizePath(filePath),
                SourceStartLine = startLine,
                SourceEndLine = endLine,
                VersionNumber = 0,
                CreatedIngestionRunId = default,
                UpdatedIngestionRunId = default,
                RemovedIngestionRunId = null,
                ValidFromUtc = default,
                ValidToUtc = null,
                IsActive = false,
                ContentHash = Array.Empty<byte>(),
                SemanticUidHash = Array.Empty<byte>()
                // IngestionRun = null
        };
    }








    private static ApiType CreateApiTypeFromSyntax(BaseTypeDeclarationSyntax typeDecl)
    {
        var name = typeDecl.Identifier.Text;

        var ns = GetNamespaceFromSyntax(typeDecl);

        var semanticUid = BuildSemanticUidFromSyntax(ns, name, typeDecl);

        var kind = typeDecl switch
        {
                ClassDeclarationSyntax => "Class",
                StructDeclarationSyntax => "Struct",
                InterfaceDeclarationSyntax => "Interface",
                EnumDeclarationSyntax => "Enum",
                RecordDeclarationSyntax => "Record",
                _ => "Unknown"
        };

        var accessibility = GetAccessibilityFromModifiers(typeDecl.Modifiers) ?? "Internal";

        bool? isStatic = typeDecl.Modifiers.Any(SyntaxKind.StaticKeyword);
        bool? isAbstract = typeDecl.Modifiers.Any(SyntaxKind.AbstractKeyword);
        bool? isSealed = typeDecl.Modifiers.Any(SyntaxKind.SealedKeyword);
        bool? isRecord = typeDecl is RecordDeclarationSyntax;
        bool? isGeneric = (typeDecl as TypeDeclarationSyntax)?.TypeParameterList?.Parameters.Count > 0;

        string baseTypeUid = null;
        string interfaces = null;
        if (typeDecl.BaseList is { Types: { Count: > 0 } baseTypes })
        {
            var typeNames = baseTypes
                    .Select(t => t.Type.ToString().Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();

            if (typeNames.Count > 0)
            {
                if (typeDecl is ClassDeclarationSyntax && typeNames.Count > 0)
                {
                    baseTypeUid = typeNames[0];
                    if (typeNames.Count > 1)
                    {
                        interfaces = string.Join(";", typeNames.Skip(1).OrderBy(s => s, StringComparer.Ordinal));
                    }
                }
                else
                {
                    interfaces = string.Join(";", typeNames.OrderBy(s => s, StringComparer.Ordinal));
                }
            }
        }

        var (filePath, startLine, endLine) = GetSourceSpan(typeDecl.SyntaxTree, typeDecl.Span);

        var attrs = ExtractAttributeTypeNamesFromSyntax(typeDecl.AttributeLists);

        var (summary, remarks) = ExtractSummaryAndRemarksFromSyntax(typeDecl);

        var genericParameters = (typeDecl as TypeDeclarationSyntax)?.TypeParameterList?.Parameters;
        var genericParams = genericParameters is { Count: > 0 }
                ? string.Join(",",
                        genericParameters.Select<TypeParameterSyntax, string>(p => p.Identifier.Text).OrderBy(s => s, StringComparer.Ordinal))
                : null;

        var genericConstraints = ExtractGenericConstraintsFromSyntax(typeDecl);

        return new ApiType
        {
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
                IsRefLike = null,
                BaseTypeUid = baseTypeUid,
                Interfaces = interfaces,
                ContainingTypeUid = null,
                GenericParameters = genericParams,
                GenericConstraints = genericConstraints,
                Summary = summary,
                Remarks = remarks,
                Attributes = attrs,
                SourceFilePath = NormalizePath(filePath),
                SourceStartLine = startLine,
                SourceEndLine = endLine,
                VersionNumber = 0,
                CreatedIngestionRunId = default,
                UpdatedIngestionRunId = default,
                RemovedIngestionRunId = null,
                ValidFromUtc = default,
                ValidToUtc = null,
                IsActive = false,
                ContentHash = Array.Empty<byte>(),
                SemanticUidHash = Array.Empty<byte>()
                //IngestionRun = null
        };
    }








    /// <summary>
    ///     Hybrid member extraction: semantic when type symbols are available, syntactic fallback otherwise.
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

        // Semantic path: use Roslyn symbols where available.
        foreach (ApiType type in types)
        {
            ct.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(type.SemanticUid))
            {
                continue;
            }

            if (roslynTypeSymbolsByUid.TryGetValue(type.SemanticUid, out INamedTypeSymbol typeSymbol) &&
                typeSymbol is not null)
            {
                foreach (ISymbol member in typeSymbol.GetMembers())
                {
                    ct.ThrowIfCancellationRequested();

                    if (member.IsImplicitlyDeclared)
                    {
                        continue;
                    }

                    if (member is IMethodSymbol method &&
                        method.MethodKind is MethodKind.PropertyGet or MethodKind.PropertySet or MethodKind.EventAdd
                                or MethodKind.EventRemove)
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

                    SyntaxNode memberDecl = null;
                    try
                    {
                        memberDecl = await member.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntaxAsync(ct);
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
                    bool? isUnsafe = memberDecl is MemberDeclarationSyntax mds &&
                                     mds.Modifiers.Any(SyntaxKind.UnsafeKeyword);

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
                            ? string.Join(";",
                                    genMc.TypeParameters.Select(BuildTypeParameterConstraintString)
                                            .Where(s => !string.IsNullOrWhiteSpace(s)))
                            : null;

                    var attributes = member.GetAttributes().Length == 0
                            ? null
                            : string.Join(";",
                                    member.GetAttributes()
                                            .Select(a => a.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
                                            .Where(s => !string.IsNullOrWhiteSpace(s)));

                    var xml = member.GetDocumentationCommentXml(cancellationToken: ct);
                    var (summary, remarks) = ExtractSummaryAndRemarks(xml);

                    ApiMember efMember = new()
                    {
                            SemanticUid = semanticUid,
                            ApiFeatureId = type.Id,
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
                            GenericConstraints = genericConstraints,
                            Summary = summary,
                            Remarks = remarks,
                            Attributes = attributes,
                            SourceFilePath = NormalizePath(filePath),
                            SourceStartLine = startLine,
                            SourceEndLine = endLine
                    };

                    output.Add(efMember);
                    roslynMemberSymbolsByUid[semanticUid] = member;
                }

                continue;
            }

            // Syntactic fallback for types that don't have bound symbols.
            await ExtractMembersFromSyntaxForTypeAsync(solution, type, output, ct).ConfigureAwait(false);
        }
    }








    private static async Task ExtractMembersFromSyntaxForTypeAsync(
            Solution solution,
            ApiType type,
            List<ApiMember> output,
            CancellationToken ct)
    {
        // Match by namespace + name. This is best-effort but stable enough for fallback.
        foreach (Project project in solution.Projects)
        {
            ct.ThrowIfCancellationRequested();

            foreach (Document document in project.Documents)
            {
                ct.ThrowIfCancellationRequested();

                SyntaxNode root = await document.GetSyntaxRootAsync(ct).ConfigureAwait(false);
                if (root is null)
                {
                    continue;
                }

                var typeDecls = root.DescendantNodes().OfType<TypeDeclarationSyntax>();
                foreach (TypeDeclarationSyntax typeDecl in typeDecls)
                {
                    ct.ThrowIfCancellationRequested();

                    var typeName = typeDecl.Identifier.Text;
                    var ns = GetNamespaceFromSyntax(typeDecl);

                    if (!string.Equals(typeName, type.Name, StringComparison.Ordinal) ||
                        !string.Equals(ns, type.NamespacePath, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    foreach (MemberDeclarationSyntax memberDecl in typeDecl.Members)
                    {
                        ct.ThrowIfCancellationRequested();

                        ApiMember apiMember = CreateApiMemberFromSyntax(memberDecl, type);
                        if (apiMember is null)
                        {
                            continue;
                        }

                        output.Add(apiMember);
                    }
                }
            }
        }
    }








    private static ApiMember CreateApiMemberFromSyntax(MemberDeclarationSyntax memberDecl, ApiType owningType)
    {
        var name = memberDecl switch
        {
                MethodDeclarationSyntax m => m.Identifier.Text,
                PropertyDeclarationSyntax p => p.Identifier.Text,
                EventDeclarationSyntax e => e.Identifier.Text,
                IndexerDeclarationSyntax => "this[]",
                ConstructorDeclarationSyntax c => c.Identifier.Text,
                _ => string.Empty
        };

        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var kind = memberDecl.Kind().ToString();
        var methodKind = memberDecl is MethodDeclarationSyntax ? "Ordinary" : null;

        var accessibility = GetAccessibilityFromModifiers(memberDecl.Modifiers) ?? "Private";

        bool? isStatic = memberDecl.Modifiers.Any(SyntaxKind.StaticKeyword);
        bool? isAbstract = memberDecl.Modifiers.Any(SyntaxKind.AbstractKeyword);
        bool? isVirtual = memberDecl.Modifiers.Any(SyntaxKind.VirtualKeyword);
        bool? isOverride = memberDecl.Modifiers.Any(SyntaxKind.OverrideKeyword);
        bool? isSealed = memberDecl.Modifiers.Any(SyntaxKind.SealedKeyword);
        bool? isAsync = memberDecl.Modifiers.Any(SyntaxKind.AsyncKeyword);
        bool? isUnsafe = memberDecl.Modifiers.Any(SyntaxKind.UnsafeKeyword);
        bool? isReadonly = memberDecl is PropertyDeclarationSyntax pds &&
                           pds.Modifiers.Any(SyntaxKind.ReadOnlyKeyword);
        bool? isConst = memberDecl is FieldDeclarationSyntax fds &&
                        fds.Modifiers.Any(SyntaxKind.ConstKeyword);

        var returnType = memberDecl switch
        {
                MethodDeclarationSyntax m => m.ReturnType.ToString().Trim(),
                PropertyDeclarationSyntax p => p.Type.ToString().Trim(),
                IndexerDeclarationSyntax i => i.Type.ToString().Trim(),
                _ => null
        };

        var attrs = ExtractAttributeTypeNamesFromSyntax(memberDecl.AttributeLists);

        var (filePath, startLine, endLine) = GetSourceSpan(memberDecl.SyntaxTree, memberDecl.Span);

        var (summary, remarks) = ExtractSummaryAndRemarksFromSyntax(memberDecl);

        var semanticUid = BuildMemberSemanticUidFromSyntax(owningType, memberDecl, name, returnType);

        return new ApiMember
        {
                SemanticUid = semanticUid,
                ApiFeatureId = owningType.Id,
                Name = name,
                Kind = kind,
                MethodKind = methodKind,
                Accessibility = accessibility,
                IsStatic = isStatic,
                IsExtensionMethod = false,
                IsAsync = isAsync,
                IsVirtual = isVirtual,
                IsOverride = isOverride,
                IsAbstract = isAbstract,
                IsSealed = isSealed,
                IsReadonly = isReadonly,
                IsConst = isConst,
                IsUnsafe = isUnsafe,
                ReturnTypeUid = returnType,
                ReturnNullable = null,
                GenericParameters = null,
                GenericConstraints = null,
                Summary = summary,
                Remarks = remarks,
                Attributes = attrs,
                SourceFilePath = NormalizePath(filePath),
                SourceStartLine = startLine,
                SourceEndLine = endLine
        };
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

            if (!roslynMemberSymbolsByUid.TryGetValue(member.SemanticUid, out ISymbol symbol))
            {
                continue;
            }

            IReadOnlyList<IParameterSymbol> parameters = symbol switch
            {
                    IMethodSymbol method => method.Parameters,
                    IPropertySymbol prop when prop.IsIndexer => prop.Parameters,
                    IEventSymbol ev when ev.Type is INamedTypeSymbol delegateType
                                         && delegateType.DelegateInvokeMethod is { } invoke
                            => invoke.Parameters,
                    _ => Array.Empty<IParameterSymbol>()
            };

            if (parameters.Count == 0)
            {
                continue;
            }

            for (var i = 0; i < parameters.Count; i++)
            {
                ct.ThrowIfCancellationRequested();

                IParameterSymbol p = parameters[i];

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








    public static string ComputeApiTypeContentHashHex(ApiType t)
    {
        var payload = BuildApiTypePayload(t);
        return Sha256Hex(payload);
    }








    private static string NormalizePath(string path)
    {
        return string.IsNullOrWhiteSpace(path) ? null : path.Replace('\\', '/');
    }








    public static string BuildApiTypePayload(ApiType t)
    {
        static string BoolToBitString(bool? value)
        {
            return value == true ? "1" : "0";
        }



        return string.Join("|", new[]
        {
                t.Name ?? string.Empty,
                t.NamespacePath ?? string.Empty,
                t.Kind ?? string.Empty,
                t.Accessibility ?? string.Empty,
                BoolToBitString(t.IsStatic),
                BoolToBitString(t.IsGeneric),
                BoolToBitString(t.IsAbstract),
                BoolToBitString(t.IsSealed),
                BoolToBitString(t.IsRecord),
                BoolToBitString(t.IsRefLike),
                t.BaseTypeUid ?? string.Empty,
                t.Interfaces ?? string.Empty,
                t.ContainingTypeUid ?? string.Empty,
                t.GenericParameters ?? string.Empty,
                t.GenericConstraints ?? string.Empty,
                t.Summary ?? string.Empty,
                t.Remarks ?? string.Empty,
                t.Attributes ?? string.Empty
        });
    }








    /// <summary>
    ///     Walks an entire Roslyn <see cref="Solution" /> and builds a collection of <see cref="SyntaxTypeTree" /> objects.
    /// </summary>
    public static async Task<IEnumerable<SyntaxTypeTree>> WalkSolutionAsync(Solution solution, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(solution);

        List<ApiType> types = [];
        await ExtractTypesAsync(solution, types, ct).ConfigureAwait(false);

        // Dedupe types by semantic uid to avoid multiple declarations (partials) across documents.
        var uniqueTypes = types
                .Where(t => !string.IsNullOrWhiteSpace(t.SemanticUid))
                .GroupBy(t => t.SemanticUid, StringComparer.Ordinal)
                .Select(g =>
                {
                    // Prefer entries with source info, then shortest path, then lexicographically
                    return g
                            .OrderByDescending(x => x.SourceFilePath is not null)
                            .ThenBy(x => x.SourceFilePath?.Length ?? int.MaxValue)
                            .ThenBy(x => x.SourceFilePath, StringComparer.OrdinalIgnoreCase)
                            .First();
                })
                .ToList();

        // Build Roslyn type symbol map once.
        var roslynTypeSymbolsByUid =
                await BuildTypeSymbolMapAsync(solution, uniqueTypes.Select(t => t.SemanticUid), ct).ConfigureAwait(false);

        // Assign deterministic Ids so members/parameters can link without needing a DbContext.
        foreach (ApiType t in uniqueTypes)
            if (t.Id == Guid.Empty)
            {
                t.Id = Guid.NewGuid();
            }

        Dictionary<string, ISymbol> roslynMemberSymbolsByUid = new(StringComparer.Ordinal);
        List<ApiMember> members = new(Math.Max(128, uniqueTypes.Count * 8));
        await ExtractMembersAsync(solution, uniqueTypes, roslynTypeSymbolsByUid, members, roslynMemberSymbolsByUid, ct)
                .ConfigureAwait(false);

        // Dedupe members by semantic uid.
        var uniqueMembers = members
                .Where(m => !string.IsNullOrWhiteSpace(m.SemanticUid))
                .GroupBy(m => m.SemanticUid, StringComparer.Ordinal)
                .Select(g =>
                {
                    return g
                            .OrderByDescending(x => x.SourceFilePath is not null)
                            .ThenBy(x => x.SourceFilePath?.Length ?? int.MaxValue)
                            .ThenBy(x => x.SourceFilePath, StringComparer.OrdinalIgnoreCase)
                            .First();
                })
                .ToList();

        foreach (ApiMember m in uniqueMembers)
            if (m.Id == Guid.Empty)
            {
                m.Id = Guid.NewGuid();
            }

        List<ApiParameter> parameters = new(Math.Max(256, uniqueMembers.Count * 2));
        await ExtractParametersAsync(solution, uniqueMembers, roslynMemberSymbolsByUid, parameters, ct)
                .ConfigureAwait(false);

        // Group for fast assembly per type.
        var membersByType = uniqueMembers
                .GroupBy(m => m.ApiFeatureId)
                .ToDictionary(g => g.Key, g => (IReadOnlyList<ApiMember>)g.ToList());

        var parametersByMember = parameters
                .GroupBy(p => p.ApiMemberId)
                .ToDictionary(g => g.Key,
                        g => (IReadOnlyList<ApiParameter>)g.OrderBy(x => x.Position)
                                .ThenBy(x => x.Name, StringComparer.Ordinal)
                                .ToList());

        List<SyntaxTypeTree> trees = new(uniqueTypes.Count);
        foreach (ApiType t in uniqueTypes.OrderBy(t => t.SemanticUid, StringComparer.Ordinal))
        {
            ct.ThrowIfCancellationRequested();

            _ = membersByType.TryGetValue(t.Id, out var typeMembers);
            typeMembers ??= Array.Empty<ApiMember>();

            List<ApiParameter> typeParameters = [];
            if (typeMembers.Count > 0)
            {
                foreach (ApiMember m in typeMembers)
                    if (parametersByMember.TryGetValue(m.Id, out var mp))
                    {
                        typeParameters.AddRange(mp);
                    }
            }

            trees.Add(new SyntaxTypeTree(t, typeMembers.ToList(), typeParameters));
        }

        return trees;
    }








    /// <summary>
    ///     Builds a lookup of Roslyn type symbols keyed by the fully-qualified semantic UID string.
    /// </summary>
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

            Compilation compilation = await project.GetCompilationAsync(ct).ConfigureAwait(false);
            if (compilation is null)
            {
                continue;
            }

            foreach (Document document in project.Documents)
            {
                ct.ThrowIfCancellationRequested();

                SyntaxNode root = await document.GetSyntaxRootAsync(ct).ConfigureAwait(false);
                if (root is null)
                {
                    continue;
                }

                SemanticModel semanticModel = await document.GetSemanticModelAsync(ct).ConfigureAwait(false);
                if (semanticModel is null)
                {
                    continue;
                }

                foreach (BaseTypeDeclarationSyntax typeDecl in root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
                {
                    ct.ThrowIfCancellationRequested();

                    INamedTypeSymbol symbol;
                    try
                    {
                        symbol = (INamedTypeSymbol)ModelExtensions.GetDeclaredSymbol(semanticModel, typeDecl, ct);
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
                    _ = result.TryAdd(uid, symbol);
                }
            }
        }

        return result;
    }








    /// <summary>
    ///     Extracts &lt;summary&gt; and &lt;remarks&gt; from Roslyn XML documentation (semantic path).
    /// </summary>
    private static (string Summary, string Remarks) ExtractSummaryAndRemarks(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            return (null, null);
        }

        try
        {
            XDocument doc = XDocument.Parse(xml);
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








    /// <summary>
    ///     Extracts &lt;summary&gt; and &lt;remarks&gt; from XML doc comments in syntax trivia (fallback path).
    /// </summary>
    private static (string Summary, string Remarks) ExtractSummaryAndRemarksFromSyntax(MemberDeclarationSyntax member)
    {
        SyntaxTrivia trivia = member.GetLeadingTrivia()
                .FirstOrDefault(t =>
                        t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
                        t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));

        if (trivia == default)
        {
            return (null, null);
        }

        SyntaxNode structure = trivia.GetStructure();
        if (structure is not DocumentationCommentTriviaSyntax doc)
        {
            return (null, null);
        }

        var summary = doc.Content
                .OfType<XmlElementSyntax>()
                .FirstOrDefault(e => e.StartTag.Name.LocalName.Text == "summary")
                ?.Content.ToString().Trim();

        var remarks = doc.Content
                .OfType<XmlElementSyntax>()
                .FirstOrDefault(e => e.StartTag.Name.LocalName.Text == "remarks")
                ?.Content.ToString().Trim();

        return (string.IsNullOrWhiteSpace(summary) ? null : summary,
                string.IsNullOrWhiteSpace(remarks) ? null : remarks);
    }








    /// <summary>
    ///     Converts a syntax <see cref="TextSpan" /> to source file/line information.
    /// </summary>
    public static (string FilePath, int? StartLine, int? EndLine) GetSourceSpan(SyntaxTree tree, TextSpan span)
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








    /// <summary>
    ///     Builds a compact constraint string for a generic type parameter from Roslyn symbols.
    /// </summary>
    public static string BuildTypeParameterConstraintString(ITypeParameterSymbol p)
    {
        if (p is null)
        {
            return null;
        }

        List<string> parts = [];

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

        foreach (ITypeSymbol t in p.ConstraintTypes) parts.Add(t.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

        if (p.HasConstructorConstraint)
        {
            parts.Add("new()");
        }

        return parts.Count == 0 ? null : $"{p.Name}:{string.Join(",", parts)}";
    }








    private static string GetNamespaceFromSyntax(SyntaxNode node)
    {
        NamespaceDeclarationSyntax nsDecl = node.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
        if (nsDecl is not null)
        {
            return nsDecl.Name.ToString();
        }

        FileScopedNamespaceDeclarationSyntax fileNsDecl = node.Ancestors().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
        return fileNsDecl?.Name.ToString();
    }








    private static string BuildSemanticUidFromSyntax(string ns, string name, BaseTypeDeclarationSyntax typeDecl)
    {
        var typeParams = (typeDecl as TypeDeclarationSyntax)?.TypeParameterList?.Parameters;
        var genericSuffix = typeParams is { Count: > 0 }
                ? $"<{string.Join(",", typeParams.Select<TypeParameterSyntax, string>(p => p.Identifier.Text).OrderBy(s => s, StringComparer.Ordinal))}>"
                : string.Empty;

        return string.IsNullOrWhiteSpace(ns) ? $"global::{name}{genericSuffix}" : $"global::{ns}.{name}{genericSuffix}";
    }








    private static string GetAccessibilityFromModifiers(SyntaxTokenList modifiers)
    {
        return modifiers.Any(SyntaxKind.PublicKeyword)
                ? "Public"
                : modifiers.Any(SyntaxKind.InternalKeyword) && modifiers.Any(SyntaxKind.ProtectedKeyword)
                        ? "ProtectedInternal"
                        : modifiers.Any(SyntaxKind.InternalKeyword)
                                ? "Internal"
                                : modifiers.Any(SyntaxKind.ProtectedKeyword)
                                        ? "Protected"
                                        : modifiers.Any(SyntaxKind.PrivateKeyword)
                                                ? "Private"
                                                : null;
    }








    private static string ExtractAttributeTypeNamesFromSyntax(SyntaxList<AttributeListSyntax> attributeLists)
    {
        var names = attributeLists
                .SelectMany(al => al.Attributes)
                .Select(a => a.Name.ToString().Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.Ordinal)
                .OrderBy(s => s, StringComparer.Ordinal)
                .ToList();

        return names.Count == 0 ? null : string.Join(";", names);
    }








    private static string ExtractGenericConstraintsFromSyntax(BaseTypeDeclarationSyntax typeDecl)
    {
        if (typeDecl is not TypeDeclarationSyntax tds || tds.ConstraintClauses.Count == 0)
        {
            return null;
        }

        var constraints = new List<string>();

        foreach (TypeParameterConstraintClauseSyntax clause in tds.ConstraintClauses)
        {
            var name = clause.Name.Identifier.Text;
            var parts = new List<string>();

            foreach (TypeParameterConstraintSyntax c in clause.Constraints)
                switch (c)
                {
                    case ClassOrStructConstraintSyntax cs when cs.ClassOrStructKeyword.IsKind(SyntaxKind.ClassKeyword):
                        parts.Add("class");
                        break;
                    case ClassOrStructConstraintSyntax cs when cs.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword):
                        parts.Add("struct");
                        break;
                    case TypeConstraintSyntax tc:
                        parts.Add(tc.Type.ToString().Trim());
                        break;
                    case ConstructorConstraintSyntax:
                        parts.Add("new()");
                        break;
                }

            if (parts.Count > 0)
            {
                constraints.Add($"{name}:{string.Join(",", parts)}");
            }
        }

        return constraints.Count == 0 ? null : string.Join(";", constraints.OrderBy(s => s, StringComparer.Ordinal));
    }








    private static string BuildMemberSemanticUidFromSyntax(ApiType owningType, MemberDeclarationSyntax memberDecl, string name, string returnType)
    {
        // Best-effort UID based on owning type UID + member name + return type + parameter list signature.
        // This does not have to match Roslyn's exact format; it just needs to be stable within this system.
        var paramSig = memberDecl switch
        {
                MethodDeclarationSyntax m =>
                        $"({string.Join(",", m.ParameterList.Parameters.Select(p => p.Type?.ToString().Trim() ?? "object"))})",
                ConstructorDeclarationSyntax c =>
                        $"({string.Join(",", c.ParameterList.Parameters.Select(p => p.Type?.ToString().Trim() ?? "object"))})",
                IndexerDeclarationSyntax i =>
                        $"[{string.Join(",", i.ParameterList.Parameters.Select(p => p.Type?.ToString().Trim() ?? "object"))}]",
                _ => string.Empty
        };

        var owningUid = owningType.SemanticUid ?? $"{owningType.NamespacePath}.{owningType.Name}";
        var rt = string.IsNullOrWhiteSpace(returnType) ? "void" : returnType;

        return $"{owningUid}.{name}{{{rt}{paramSig}}}";
    }








    public sealed class SolutionManifest
    {
        public SolutionInfo solution { get; set; }





        public sealed class SolutionInfo
        {
            public string path { get; set; }
            public List<string> projects { get; set; }
        }
    }
}





/// <summary>
///     Simple wrapper over a list of <see cref="SyntaxTypeTree" /> that exposes it as an <see cref="IReadOnlyList{T}" />.
/// </summary>
public class ApiSyntaxTreeCollections : IReadOnlyList<SyntaxTypeTree>
{
    private readonly List<SyntaxTypeTree> _trees;








    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiSyntaxTreeCollections" /> class.
    /// </summary>
    /// <param name="trees">The trees to wrap.</param>
    public ApiSyntaxTreeCollections(IEnumerable<SyntaxTypeTree> trees)
    {
        ArgumentNullException.ThrowIfNull(trees);
        _trees = trees.ToList();
    }








    /// <summary>
    ///     Gets the <see cref="SyntaxTypeTree" /> at the given index.
    /// </summary>
    public SyntaxTypeTree this[int index] => _trees[index];





    /// <summary>
    ///     Gets the number of trees in the collection.
    /// </summary>
    public int Count => _trees.Count;








    /// <summary>
    ///     Returns an enumerator that iterates through the contained trees.
    /// </summary>
    public IEnumerator<SyntaxTypeTree> GetEnumerator()
    {
        return _trees.GetEnumerator();
    }








    /// <summary>
    ///     Returns a non-generic enumerator that iterates through the contained trees.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}