// Project Name: SKAgent
// File Name: RoslynHarvesterBase.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

using ITCompanionAI.Helpers;

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
public abstract class RoslynHarvesterBase
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
        List<ApiTypeDescriptor> output,
        CancellationToken ct)
    {
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

                    var symbol = semanticModel.GetDeclaredSymbol(typeDecl, ct);
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

                    output.Add(new ApiTypeDescriptor(
                        semanticUid,
                        name,
                        ns,
                        kind,
                        accessibility,
                        isStatic,
                        isGeneric,
                        isAbstract,
                        isSealed,
                        isRecord,
                        isRefLike,
                        baseTypeUid,
                        interfaces,
                        containingTypeUid,
                        genericParameters,
                        genericConstraints,
                        summary,
                        remarks,
                        attributes,
                        filePath,
                        startLine,
                        endLine,
                        symbol));
                }
            }
        }
    }







    public static async Task ExtractMembersAsync(
        Solution solution,
        IReadOnlyList<ApiTypeDescriptor> types,
        List<ApiMemberDescriptor> output,
        CancellationToken ct)
    {
        Dictionary<string, INamedTypeSymbol> typeByUid = types.ToDictionary(t => t.SemanticUid, t => t.Symbol, StringComparer.Ordinal);

        foreach (Project project in solution.Projects)
        {
            ct.ThrowIfCancellationRequested();

            Compilation? compilation = await project.GetCompilationAsync(ct).ConfigureAwait(false);
            if (compilation is null)
            {
                continue;
            }

            foreach ((var typeUid, INamedTypeSymbol typeSymbol) in typeByUid)
            {
                ct.ThrowIfCancellationRequested();

                // Ensure the type symbol belongs to the current compilation.
                // If it doesn't, still use the original symbol; Roslyn symbols are stable enough for metadata extraction.
                foreach (ISymbol member in typeSymbol.GetMembers())
                {
                    ct.ThrowIfCancellationRequested();

                    if (member is IMethodSymbol method && method.MethodKind is MethodKind.PropertyGet or MethodKind.PropertySet or MethodKind.EventAdd or MethodKind.EventRemove)
                    {
                        continue;
                    }

                    if (member is IMethodSymbol m && m.MethodKind == MethodKind.Constructor)
                    {
                        // include constructors
                    }

                    if (member.DeclaredAccessibility == Accessibility.NotApplicable)
                    {
                        continue;
                    }

                    SyntaxNode? memberDecl = member.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax(ct);
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

                    output.Add(new ApiMemberDescriptor(
                        semanticUid,
                        typeUid,
                        name,
                        kind,
                        methodKind,
                        accessibility,
                        isStatic,
                        isExtensionMethod,
                        isAsync,
                        isVirtual,
                        isOverride,
                        isAbstract,
                        isSealed,
                        isReadOnly,
                        isConst,
                        isUnsafe,
                        returnTypeUid,
                        returnNullable,
                        genericParameters,
                        genericConstraints,
                        summary,
                        remarks,
                        attributes,
                        filePath,
                        startLine,
                        endLine,
                        member));
                }
            }
        }
    }







    public static Task ExtractParametersAsync(
        Solution solution,
        IReadOnlyList<ApiMemberDescriptor> members,
        List<ApiParameterDescriptor> output,
        CancellationToken ct)
    {
        _ = solution;

        foreach (ApiMemberDescriptor member in members)
        {
            ct.ThrowIfCancellationRequested();

            if (member.Symbol is not IMethodSymbol method)
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

                output.Add(new ApiParameterDescriptor(
                    member.SemanticUid,
                    p.Name,
                    p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    p.NullableAnnotation.ToString(),
                    p.Ordinal,
                    modifier,
                    hasDefault,
                    defaultLiteral));
            }
        }

        return Task.CompletedTask;
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







    public sealed record ApiTypeDescriptor(
        string SemanticUid,
        string? Name,
        string? NamespacePath,
        string? Kind,
        string? Accessibility,
        bool? IsStatic,
        bool? IsGeneric,
        bool? IsAbstract,
        bool? IsSealed,
        bool? IsRecord,
        bool? IsRefLike,
        string? BaseTypeUid,
        string? Interfaces,
        string? ContainingTypeUid,
        string? GenericParameters,
        string? GenericConstraints,
        string? Summary,
        string? Remarks,
        string? Attributes,
        string? SourceFilePath,
        int? SourceStartLine,
        int? SourceEndLine,
        INamedTypeSymbol Symbol);




    public sealed record ApiMemberDescriptor(
        string SemanticUid,
        string TypeSemanticUid,
        string? Name,
        string? Kind,
        string? MethodKind,
        string? Accessibility,
        bool? IsStatic,
        bool? IsExtensionMethod,
        bool? IsAsync,
        bool? IsVirtual,
        bool? IsOverride,
        bool? IsAbstract,
        bool? IsSealed,
        bool? IsReadOnly,
        bool? IsConst,
        bool? IsUnsafe,
        string? ReturnTypeUid,
        string? ReturnNullable,
        string? GenericParameters,
        string? GenericConstraints,
        string? Summary,
        string? Remarks,
        string? Attributes,
        string? SourceFilePath,
        int? SourceStartLine,
        int? SourceEndLine,
        ISymbol Symbol);




    public sealed record ApiParameterDescriptor(
        string MemberSemanticUid,
        string Name,
        string TypeUid,
        string? NullableAnnotation,
        int Position,
        string? Modifier,
        bool HasDefaultValue,
        string? DefaultValueLiteral);
}