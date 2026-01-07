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
using ITCompanionAI.Models;

using Project = Microsoft.CodeAnalysis.Project;


namespace ITCompanionAI.AgentFramework.Ingestion;


/// <summary>
///     Base class for harvesters that need to retrieve source code from GitHub and analyze it with Roslyn workspaces.
/// </summary>
/// <remarks>
///     <para>
///         This type provides a small Roslyn-based ingestion pipeline intended to extract a repository's public API
///         surface
///         into persistence-friendly entities (<see cref="ApiType" />, <see cref="ApiMember" />,
///         <see cref="ApiParameter" />).
///     </para>
///     <para>
///         The pipeline is typically:
///     </para>
///     <list type="number">
///         <item>
///             <description>
///                 Acquire source (for example, via <see cref="DownloadRepositoryAsync" />).
///             </description>
///         </item>
///         <item>
///             <description>
///                 Load a Roslyn <see cref="Solution" /> from a directory (via
///                 <see cref="LoadSolutionFromDirectoryAsync" />).
///             </description>
///         </item>
///         <item>
///             <description>
///                 Walk the solution and extract:
///                 <list type="bullet">
///                     <item>
///                         <description>Types (<see cref="ExtractTypesAsync" />)</description>
///                     </item>
///                     <item>
///                         <description>Members (<see cref="ExtractMembersAsync" />)</description>
///                     </item>
///                     <item>
///                         <description>Parameters (<see cref="ExtractParametersAsync" />)</description>
///                     </item>
///                 </list>
///             </description>
///         </item>
///         <item>
///             <description>
///                 Assemble per-type trees of extracted results (<see cref="WalkSolutionAsync" />).
///             </description>
///         </item>
///     </list>
///     <para>
///         Identity is primarily based on Roslyn's <see cref="SymbolDisplayFormat.FullyQualifiedFormat" /> string, stored
///         as
///         <c>SemanticUid</c>. This makes extracted items stable across runs, and enables de-duplication of partial types
///         and
///         multi-file declarations.
///     </para>
///     <para>
///         Notes:
///         <list type="bullet">
///             <item>
///                 <description>
///                     The base class inherits <see cref="CSharpSyntaxWalker" /> for potential future syntactic walking,
///                     but the current extraction logic uses semantic information via <see cref="SemanticModel" />.
///                 </description>
///             </item>
///             <item>
///                 <description>
///                     <see cref="LoadSolutionFromDirectoryAsync" /> uses <see cref="AdhocWorkspace" /> (no MSBuild),
///                     so compilation is "best effort". Symbols that fail to bind are skipped.
///                 </description>
///             </item>
///         </list>
///     </para>
/// </remarks>
public abstract class RoslynHarvesterBase : CSharpSyntaxWalker
{
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

        Directory.CreateDirectory(destinationDirectory);

        GitHubClient client = _gitHubClientFactory.CreateClient();
    }







    /// <summary>
    ///     Creates a Roslyn <see cref="Solution" /> for a local repository directory.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method builds an in-memory <see cref="Solution" /> using <see cref="AdhocWorkspace" /> and a single
    ///         project.
    ///         It is designed for "source only" analysis scenarios where MSBuild is unavailable or undesirable.
    ///     </para>
    ///     <para>
    ///         All <c>.cs</c> files under <paramref name="repositoryDirectory" /> are added as documents. Minimal framework
    ///         references are included (<see cref="object" />, LINQ, <see cref="Task" />).
    ///     </para>
    ///     <para>
    ///         Because this is not MSBuild-backed, some symbols may remain unresolved (missing references, conditional
    ///         compilation,
    ///         multi-targeting, source generators, etc.). Downstream extraction methods are defensive and skip unbindable
    ///         symbols.
    ///     </para>
    /// </remarks>
    /// <param name="repositoryDirectory">Directory containing source files.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created solution.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="repositoryDirectory" /> is null/empty/whitespace.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when no C# source files are found under <paramref name="repositoryDirectory" />.
    /// </exception>
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
    /// <remarks>
    ///     This helper is used when an entity needs a deterministic fingerprint derived from stable input data.
    ///     The returned string is lowercase hex.
    /// </remarks>
    /// <param name="value">Input value to hash.</param>
    /// <returns>Lowercase SHA-256 hex string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value" /> is <c>null</c>.</exception>
    protected static string Sha256Hex(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }







    /// <summary>
    ///     Extracts type declarations from the specified Roslyn <see cref="Solution" /> and populates
    ///     <paramref name="output" />
    ///     with <see cref="ApiType" /> entities.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         For each project/document, the syntax tree is scanned for <see cref="BaseTypeDeclarationSyntax" /> nodes
    ///         (classes, structs, interfaces, enums, records). Each declaration is bound to an <see cref="INamedTypeSymbol" />
    ///         using the document's <see cref="SemanticModel" />.
    ///     </para>
    ///     <para>
    ///         Symbols that fail to bind (exceptions from Roslyn, null symbols) or bind to <see cref="TypeKind.Error" /> are
    ///         skipped.
    ///     </para>
    ///     <para>
    ///         The resulting <see cref="ApiType" /> includes:
    ///         <list type="bullet">
    ///             <item>
    ///                 <description><c>SemanticUid</c>: fully-qualified Roslyn display string used as the dedupe key</description>
    ///             </item>
    ///             <item>
    ///                 <description>Kind/accessibility and common modifiers (static/generic/abstract/etc.)</description>
    ///             </item>
    ///             <item>
    ///                 <description>Base type and implemented interfaces (as fully-qualified UIDs)</description>
    ///             </item>
    ///             <item>
    ///                 <description>Containing type UID (for nested types)</description>
    ///             </item>
    ///             <item>
    ///                 <description>Generic parameters and constraints (stored as delimited strings)</description>
    ///             </item>
    ///             <item>
    ///                 <description>Attribute types present on the declaration</description>
    ///             </item>
    ///             <item>
    ///                 <description>XML doc <c>&lt;summary&gt;</c> / <c>&lt;remarks&gt;</c> (best-effort)</description>
    ///             </item>
    ///             <item>
    ///                 <description>Source location (file path + 1-based start/end lines)</description>
    ///             </item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         This method does not de-duplicate partial types; that is handled by <see cref="WalkSolutionAsync" />.
    ///     </para>
    /// </remarks>
    /// <param name="solution">The Roslyn <see cref="Solution" /> to analyze.</param>
    /// <param name="output">The list to populate with extracted types.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="solution" /> or <paramref name="output" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    ///     Thrown if cancellation is requested via <paramref name="ct" />.
    /// </exception>
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
                        //  Id = default,   autogegen
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
                        IngestionRun = null

                    });
                }
            }
        }
    }







    /// <summary>
    ///     Extracts member declarations for the provided <paramref name="types" /> and populates <paramref name="output" />
    ///     with
    ///     <see cref="ApiMember" /> entities.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method requires a pre-built mapping of type UID to Roslyn symbol
    ///         (<paramref name="roslynTypeSymbolsByUid" />), typically created by <see cref="BuildTypeSymbolMapAsync" />.
    ///     </para>
    ///     <para>
    ///         For each resolved type symbol:
    ///         <list type="bullet">
    ///             <item>
    ///                 <description>Enumerates members via <see cref="INamedTypeSymbol.GetMembers()" /></description>
    ///             </item>
    ///             <item>
    ///                 <description>Skips implicit members and accessor methods (get/set/add/remove)</description>
    ///             </item>
    ///             <item>
    ///                 <description>Skips backing fields associated with properties/events</description>
    ///             </item>
    ///             <item>
    ///                 <description>Skips members with <see cref="Accessibility.NotApplicable" /></description>
    ///             </item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         For each included member, this method captures:
    ///         <list type="bullet">
    ///             <item>
    ///                 <description><c>SemanticUid</c> as a fully-qualified symbol display string</description>
    ///             </item>
    ///             <item>
    ///                 <description>Member kind/method kind/accessibility and common modifiers</description>
    ///             </item>
    ///             <item>
    ///                 <description>Return type UID + nullable annotation (methods/properties only)</description>
    ///             </item>
    ///             <item>
    ///                 <description>Generic parameters/constraints for generic methods (stored as delimited strings)</description>
    ///             </item>
    ///             <item>
    ///                 <description>Attributes and XML doc summary/remarks (best-effort)</description>
    ///             </item>
    ///             <item>
    ///                 <description>Source location if syntax is available (file path + 1-based start/end lines)</description>
    ///             </item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         Additionally, a Roslyn symbol map is built in <paramref name="roslynMemberSymbolsByUid" /> so that parameters
    ///         can be
    ///         extracted later without needing another semantic walk.
    ///     </para>
    /// </remarks>
    /// <param name="solution">The solution being analyzed (currently not used beyond project enumeration).</param>
    /// <param name="types">Types to extract members for.</param>
    /// <param name="roslynTypeSymbolsByUid">Map of type semantic UID to Roslyn type symbol.</param>
    /// <param name="output">List to populate with extracted members.</param>
    /// <param name="roslynMemberSymbolsByUid">Map of member semantic UID to Roslyn symbol.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when any required argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    ///     Thrown when cancellation is requested via <paramref name="ct" />.
    /// </exception>
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







    /// <summary>
    ///     Extracts method parameters for the provided <paramref name="members" /> list and populates
    ///     <paramref name="output" /> with
    ///     <see cref="ApiParameter" /> entities.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method uses <paramref name="roslynMemberSymbolsByUid" /> to resolve a member's Roslyn symbol by its
    ///         <c>SemanticUid</c>. Only members whose symbol is an <see cref="IMethodSymbol" /> participate in parameter
    ///         extraction.
    ///     </para>
    ///     <para>
    ///         For each parameter, this records its position, type UID, nullability, ref-kind modifier (ref/out/in),
    ///         and default value literal (if present).
    ///     </para>
    ///     <para>
    ///         The <paramref name="solution" /> parameter is currently unused; it is accepted for signature symmetry with
    ///         other
    ///         extraction steps and potential future expansion.
    ///     </para>
    /// </remarks>
    /// <param name="solution">The solution being analyzed (currently unused).</param>
    /// <param name="members">Members to extract parameters for.</param>
    /// <param name="roslynMemberSymbolsByUid">Map of member semantic UID to Roslyn symbol.</param>
    /// <param name="output">List to populate with extracted parameters.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A completed task.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="members" />, <paramref name="roslynMemberSymbolsByUid" />, or
    ///     <paramref name="output" />
    ///     is <c>null</c>.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    ///     Thrown when cancellation is requested via <paramref name="ct" />.
    /// </exception>
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
    ///     Walks an entire Roslyn <see cref="Solution" /> and builds a collection of <see cref="SyntaxTypeTree" /> objects
    ///     representing the extracted API surface.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This is the high-level orchestrator for extraction. It:
    ///     </para>
    ///     <list type="number">
    ///         <item>
    ///             <description>
    ///                 Extracts all types via <see cref="ExtractTypesAsync" />.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 De-duplicates types by <c>SemanticUid</c> (helps handle partial types and duplicates across documents).
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 Builds a type symbol map via <see cref="BuildTypeSymbolMapAsync" /> for efficient member extraction.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 Assigns deterministic runtime <see cref="Guid" /> IDs for linking members/parameters without a database
    ///                 context.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 Extracts members via <see cref="ExtractMembersAsync" />, then de-duplicates members by
    ///                 <c>SemanticUid</c>.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 Extracts parameters via <see cref="ExtractParametersAsync" />.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 Groups members/parameters and assembles a <see cref="SyntaxTypeTree" /> per type.
    ///             </description>
    ///         </item>
    ///     </list>
    ///     <para>
    ///         De-duplication prefers entries that have source information (file path) and then sorts by file path to keep
    ///         selection stable across runs.
    ///     </para>
    /// </remarks>
    /// <param name="solution">The Roslyn solution to analyze.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A collection of syntax type trees covering every discovered type.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="solution" /> is <c>null</c>.</exception>
    /// <exception cref="OperationCanceledException">
    ///     Thrown when cancellation is requested via <paramref name="ct" />.
    /// </exception>
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
            .GroupBy(m => m.ApiFeatureId)
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







    /// <summary>
    ///     Builds a lookup of Roslyn type symbols keyed by the fully-qualified semantic UID string.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method scans each document in the solution for type declarations and binds them to
    ///         <see cref="INamedTypeSymbol" /> instances. Only symbols whose UID is present in
    ///         <paramref name="typeSemanticUids" />
    ///         are retained.
    ///     </para>
    ///     <para>
    ///         The returned map is used by <see cref="ExtractMembersAsync" /> to avoid re-binding type symbols per member
    ///         extraction pass. When multiple declarations exist (e.g., partial types), the first-seen symbol is retained;
    ///         higher-level dedupe/selection occurs in <see cref="WalkSolutionAsync" />.
    ///     </para>
    /// </remarks>
    /// <param name="solution">The Roslyn solution to scan.</param>
    /// <param name="typeSemanticUids">Type semantic UIDs to include in the result.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>
    ///     A dictionary that maps type semantic UID strings to Roslyn <see cref="INamedTypeSymbol" /> instances.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="solution" /> or
    ///     <paramref name="typeSemanticUids" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="OperationCanceledException">Thrown when cancellation is requested via <paramref name="ct" />.</exception>
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







    /// <summary>
    ///     Extracts <c>&lt;summary&gt;</c> and <c>&lt;remarks&gt;</c> values from Roslyn XML documentation (best-effort).
    /// </summary>
    /// <remarks>
    ///     Roslyn returns XML documentation as a fragment string. This helper parses the XML and returns trimmed text values.
    ///     If parsing fails or tags are missing, <c>null</c> values are returned.
    /// </remarks>
    /// <param name="xml">XML documentation string as returned by Roslyn.</param>
    /// <returns>A tuple of extracted summary and remarks.</returns>
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







    /// <summary>
    ///     Converts a syntax <see cref="TextSpan" /> to source file/line information.
    /// </summary>
    /// <remarks>
    ///     Start/end line numbers are 1-based to align with typical editor line numbering.
    /// </remarks>
    /// <param name="tree">Syntax tree owning <paramref name="span" />.</param>
    /// <param name="span">Text span to translate.</param>
    /// <returns>The file path and 1-based start/end line numbers, or <c>null</c> values when unavailable.</returns>
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







    /// <summary>
    ///     Builds a compact, persistence-friendly constraint string for a generic type parameter.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The returned format is: <c>{Name}:{constraint1},{constraint2},...</c>.
    ///     </para>
    ///     <para>
    ///         Constraints include known keywords (<c>class</c>, <c>struct</c>, <c>notnull</c>, <c>unmanaged</c>, <c>new()</c>
    ///         )
    ///         and any explicit constraint types (fully-qualified).
    ///     </para>
    ///     <para>
    ///         If the parameter has no constraints, <c>null</c> is returned.
    ///     </para>
    /// </remarks>
    /// <param name="p">The Roslyn type parameter symbol.</param>
    /// <returns>
    ///     A constraint string, or <c>null</c> when <paramref name="p" /> is <c>null</c> or has no constraints.
    /// </returns>
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




/// <summary>
///     Simple wrapper over a list of <see cref="SyntaxTypeTree" /> that exposes it as an <see cref="IReadOnlyList{T}" />.
/// </summary>
/// <remarks>
///     This provides a dedicated type to represent a collection of extracted syntax/type trees and keeps callers from
///     relying on
///     the underlying concrete list type.
/// </remarks>
public class ApiSyntaxTreeCollections : IReadOnlyList<SyntaxTypeTree>
{
    private readonly List<SyntaxTypeTree> _trees;







    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiSyntaxTreeCollections" /> class.
    /// </summary>
    /// <param name="trees">The trees to wrap.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="trees" /> is <c>null</c>.</exception>
    public ApiSyntaxTreeCollections(IEnumerable<SyntaxTypeTree> trees)
    {
        ArgumentNullException.ThrowIfNull(trees);
        _trees = trees.ToList();
    }







    /// <summary>
    ///     Gets the <see cref="SyntaxTypeTree" /> at the given index.
    /// </summary>
    /// <param name="index">Zero-based index.</param>
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