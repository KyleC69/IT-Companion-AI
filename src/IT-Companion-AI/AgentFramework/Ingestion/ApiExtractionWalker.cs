using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#region DTOs

public sealed class ApiTypeExtraction
{
    public string SemanticUid { get; set; }
    public string Name { get; set; }
    public string Kind { get; set; }
    public string Accessibility { get; set; }
    public List<string> Modifiers { get; set; }
    public string BaseType { get; set; }
    public List<string> Interfaces { get; set; }
    public List<string> GenericParameters { get; set; }
    public List<string> GenericConstraints { get; set; }
    public List<string> Attributes { get; set; }
    public string SummaryXml { get; set; }
    public string Namespace { get; set; }
    public string DeclaringType { get; set; }
    public string AssemblyName { get; set; }
    public string SourceFilePath { get; set; }
    public int StartLine { get; set; }
    public int EndLine { get; set; }
}

public sealed class ApiMemberExtraction
{
    public string SemanticUid { get; set; }
    public string Name { get; set; }
    public string Kind { get; set; }
    public string Accessibility { get; set; }
    public List<string> Modifiers { get; set; }
    public string ReturnType { get; set; }
    public List<ApiParameterExtraction> Parameters { get; set; }
    public List<string> GenericParameters { get; set; }
    public List<string> GenericConstraints { get; set; }
    public List<string> Attributes { get; set; }
    public string SummaryXml { get; set; }
    public string Namespace { get; set; }
    public string DeclaringType { get; set; }
    public string AssemblyName { get; set; }
    public string SourceFilePath { get; set; }
    public int StartLine { get; set; }
    public int EndLine { get; set; }
}

public sealed class ApiParameterExtraction
{
    public string SemanticUid { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Nullable { get; set; }
    public string Modifier { get; set; }
    public string DefaultValue { get; set; }
    public List<string> Attributes { get; set; }
}

public sealed class ApiExtractionResult
{
    public ApiExtractionResult(
        IReadOnlyList<ApiTypeExtraction> types,
        IReadOnlyList<ApiMemberExtraction> members)
    {
        Types = types;
        Members = members;
    }

    public IReadOnlyList<ApiTypeExtraction> Types { get; }
    public IReadOnlyList<ApiMemberExtraction> Members { get; }
}

#endregion

#region Walker

public sealed class ApiExtractionWalker : CSharpSyntaxWalker
{
    private readonly SemanticModel _semanticModel;
    private readonly string _filePath;
    private readonly string _assemblyName;

    public List<ApiTypeExtraction> Types { get; } = new();
    public List<ApiMemberExtraction> Members { get; } = new();

    public ApiExtractionWalker(SemanticModel semanticModel, string filePath, string assemblyName)
        : base(SyntaxWalkerDepth.StructuredTrivia)
    {
        _semanticModel = semanticModel;
        _filePath = filePath;
        _assemblyName = assemblyName;
    }

    // =========================
    // Type declarations
    // =========================

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        ExtractNamedType(node, "Class");
        base.VisitClassDeclaration(node);
    }

    public override void VisitStructDeclaration(StructDeclarationSyntax node)
    {
        ExtractNamedType(node, "Struct");
        base.VisitStructDeclaration(node);
    }

    public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        ExtractNamedType(node, "Interface");
        base.VisitInterfaceDeclaration(node);
    }

    public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
    {
        ExtractEnum(node);
        base.VisitEnumDeclaration(node);
    }

    public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
    {
        ExtractDelegate(node);
        base.VisitDelegateDeclaration(node);
    }

    // =========================
    // Member declarations
    // =========================

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        ExtractMethod(node);
        base.VisitMethodDeclaration(node);
    }

    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        ExtractProperty(node);
        base.VisitPropertyDeclaration(node);
    }

    public override void VisitIndexerDeclaration(IndexerDeclarationSyntax node)
    {
        ExtractIndexer(node);
        base.VisitIndexerDeclaration(node);
    }

    public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        ExtractField(node);
        base.VisitFieldDeclaration(node);
    }

    public override void VisitEventDeclaration(EventDeclarationSyntax node)
    {
        ExtractEvent(node);
        base.VisitEventDeclaration(node);
    }

    public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
    {
        ExtractEventField(node);
        base.VisitEventFieldDeclaration(node);
    }

    // =========================
    // Type extraction
    // =========================

    private void ExtractNamedType(TypeDeclarationSyntax node, string kind)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) as INamedTypeSymbol;
        if (symbol == null)
            return;

        var (start, end) = GetSpan(node);

        var dto = new ApiTypeExtraction
        {
            SemanticUid = GenerateSemanticUid(symbol),
            Name = symbol.Name,
            Kind = kind,
            Accessibility = symbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
            Modifiers = node.Modifiers.Select(m => m.Text).OrderBy(m => m).ToList(),
            BaseType = symbol.BaseType?.ToDisplayString(),
            Interfaces = symbol.AllInterfaces.Select(i => i.ToDisplayString()).OrderBy(i => i).ToList(),
            GenericParameters = symbol.TypeParameters.Select(t => t.Name).ToList(),
            GenericConstraints = symbol.TypeParameters
                .SelectMany(tp => tp.ConstraintTypes.Select(ct => ct.ToDisplayString()))
                .Distinct()
                .OrderBy(x => x)
                .ToList(),
            Attributes = symbol.GetAttributes()
                .Select(a => a.AttributeClass?.ToDisplayString())
                .Where(a => a != null)
                .OrderBy(a => a)
                .ToList(),
            SummaryXml = symbol.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: default),
            Namespace = symbol.ContainingNamespace?.ToDisplayString(),
            DeclaringType = symbol.ContainingType?.ToDisplayString(),
            AssemblyName = _assemblyName,
            SourceFilePath = _filePath,
            StartLine = start,
            EndLine = end
        };

        Types.Add(dto);
    }

    private void ExtractEnum(EnumDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) as INamedTypeSymbol;
        if (symbol == null)
            return;

        var (start, end) = GetSpan(node);

        var dto = new ApiTypeExtraction
        {
            SemanticUid = GenerateSemanticUid(symbol),
            Name = symbol.Name,
            Kind = "Enum",
            Accessibility = symbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
            Modifiers = node.Modifiers.Select(m => m.Text).OrderBy(m => m).ToList(),
            BaseType = symbol.EnumUnderlyingType?.ToDisplayString(),
            Interfaces = symbol.AllInterfaces.Select(i => i.ToDisplayString()).OrderBy(i => i).ToList(),
            GenericParameters = new List<string>(),
            GenericConstraints = new List<string>(),
            Attributes = symbol.GetAttributes()
                .Select(a => a.AttributeClass?.ToDisplayString())
                .Where(a => a != null)
                .OrderBy(a => a)
                .ToList(),
            SummaryXml = symbol.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: default),
            Namespace = symbol.ContainingNamespace?.ToDisplayString(),
            DeclaringType = symbol.ContainingType?.ToDisplayString(),
            AssemblyName = _assemblyName,
            SourceFilePath = _filePath,
            StartLine = start,
            EndLine = end
        };

        Types.Add(dto);
    }

    private void ExtractDelegate(DelegateDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) as INamedTypeSymbol;
        if (symbol == null)
            return;

        var invoke = symbol.DelegateInvokeMethod;
        var (start, end) = GetSpan(node);

        var dto = new ApiMemberExtraction
        {
            SemanticUid = GenerateSemanticUid(symbol),
            Name = symbol.Name,
            Kind = "Delegate",
            Accessibility = symbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
            Modifiers = node.Modifiers.Select(m => m.Text).OrderBy(m => m).ToList(),
            ReturnType = invoke?.ReturnType.ToDisplayString(),
            Parameters = invoke?.Parameters.Select(ConvertParameter).ToList() ?? new List<ApiParameterExtraction>(),
            GenericParameters = symbol.TypeParameters.Select(t => t.Name).ToList(),
            GenericConstraints = symbol.TypeParameters
                .SelectMany(tp => tp.ConstraintTypes.Select(ct => ct.ToDisplayString()))
                .Distinct()
                .OrderBy(x => x)
                .ToList(),
            Attributes = symbol.GetAttributes()
                .Select(a => a.AttributeClass?.ToDisplayString())
                .Where(a => a != null)
                .OrderBy(a => a)
                .ToList(),
            SummaryXml = symbol.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: default),
            Namespace = symbol.ContainingNamespace?.ToDisplayString(),
            DeclaringType = symbol.ContainingType?.ToDisplayString(),
            AssemblyName = _assemblyName,
            SourceFilePath = _filePath,
            StartLine = start,
            EndLine = end
        };

        Members.Add(dto);
    }

    // =========================
    // Method extraction
    // =========================

    private void ExtractMethod(MethodDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) as IMethodSymbol;
        if (symbol == null)
            return;

        var (start, end) = GetSpan(node);

        var dto = new ApiMemberExtraction
        {
            SemanticUid = GenerateSemanticUid(symbol),
            Name = symbol.Name,
            Kind = "Method",
            Accessibility = symbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
            Modifiers = node.Modifiers.Select(m => m.Text).OrderBy(m => m).ToList(),
            ReturnType = symbol.ReturnType.ToDisplayString(),
            Parameters = symbol.Parameters.Select(ConvertParameter).ToList(),
            GenericParameters = symbol.TypeParameters.Select(t => t.Name).ToList(),
            GenericConstraints = symbol.TypeParameters
                .SelectMany(tp => tp.ConstraintTypes.Select(ct => ct.ToDisplayString()))
                .Distinct()
                .OrderBy(x => x)
                .ToList(),
            Attributes = symbol.GetAttributes()
                .Select(a => a.AttributeClass?.ToDisplayString())
                .Where(a => a != null)
                .OrderBy(a => a)
                .ToList(),
            SummaryXml = symbol.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: default),
            Namespace = symbol.ContainingNamespace?.ToDisplayString(),
            DeclaringType = symbol.ContainingType?.ToDisplayString(),
            AssemblyName = _assemblyName,
            SourceFilePath = _filePath,
            StartLine = start,
            EndLine = end
        };

        Members.Add(dto);
    }

    // =========================
    // Property / indexer extraction
    // =========================

    private void ExtractProperty(PropertyDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) as IPropertySymbol;
        if (symbol == null)
            return;

        var (start, end) = GetSpan(node);

        var dto = new ApiMemberExtraction
        {
            SemanticUid = GenerateSemanticUid(symbol),
            Name = symbol.Name,
            Kind = "Property",
            Accessibility = symbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
            Modifiers = node.Modifiers.Select(m => m.Text).OrderBy(m => m).ToList(),
            ReturnType = symbol.Type.ToDisplayString(),
            Parameters = new List<ApiParameterExtraction>(),
            GenericParameters = new List<string>(),
            GenericConstraints = new List<string>(),
            Attributes = symbol.GetAttributes()
                .Select(a => a.AttributeClass?.ToDisplayString())
                .Where(a => a != null)
                .OrderBy(a => a)
                .ToList(),
            SummaryXml = symbol.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: default),
            Namespace = symbol.ContainingNamespace?.ToDisplayString(),
            DeclaringType = symbol.ContainingType?.ToDisplayString(),
            AssemblyName = _assemblyName,
            SourceFilePath = _filePath,
            StartLine = start,
            EndLine = end
        };

        Members.Add(dto);
    }

    private void ExtractIndexer(IndexerDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) as IPropertySymbol;
        if (symbol == null)
            return;

        var (start, end) = GetSpan(node);

        var dto = new ApiMemberExtraction
        {
            SemanticUid = GenerateSemanticUid(symbol),
            Name = "this",
            Kind = "Indexer",
            Accessibility = symbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
            Modifiers = node.Modifiers.Select(m => m.Text).OrderBy(m => m).ToList(),
            ReturnType = symbol.Type.ToDisplayString(),
            Parameters = symbol.Parameters.Select(ConvertParameter).ToList(),
            GenericParameters = new List<string>(),
            GenericConstraints = new List<string>(),
            Attributes = symbol.GetAttributes()
                .Select(a => a.AttributeClass?.ToDisplayString())
                .Where(a => a != null)
                .OrderBy(a => a)
                .ToList(),
            SummaryXml = symbol.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: default),
            Namespace = symbol.ContainingNamespace?.ToDisplayString(),
            DeclaringType = symbol.ContainingType?.ToDisplayString(),
            AssemblyName = _assemblyName,
            SourceFilePath = _filePath,
            StartLine = start,
            EndLine = end
        };

        Members.Add(dto);
    }

    // =========================
    // Field extraction
    // =========================

    private void ExtractField(FieldDeclarationSyntax node)
    {
        foreach (var variable in node.Declaration.Variables)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(variable) as IFieldSymbol;
            if (symbol == null)
                continue;

            var (start, end) = GetSpan(node);

            var dto = new ApiMemberExtraction
            {
                SemanticUid = GenerateSemanticUid(symbol),
                Name = symbol.Name,
                Kind = "Field",
                Accessibility = symbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
                Modifiers = node.Modifiers.Select(m => m.Text).OrderBy(m => m).ToList(),
                ReturnType = symbol.Type.ToDisplayString(),
                Parameters = new List<ApiParameterExtraction>(),
                GenericParameters = new List<string>(),
                GenericConstraints = new List<string>(),
                Attributes = symbol.GetAttributes()
                    .Select(a => a.AttributeClass?.ToDisplayString())
                    .Where(a => a != null)
                    .OrderBy(a => a)
                    .ToList(),
                SummaryXml = symbol.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: default),
                Namespace = symbol.ContainingNamespace?.ToDisplayString(),
                DeclaringType = symbol.ContainingType?.ToDisplayString(),
                AssemblyName = _assemblyName,
                SourceFilePath = _filePath,
                StartLine = start,
                EndLine = end
            };

            Members.Add(dto);
        }
    }

    // =========================
    // Event extraction
    // =========================

    private void ExtractEvent(EventDeclarationSyntax node)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(node) as IEventSymbol;
        if (symbol == null)
            return;

        var (start, end) = GetSpan(node);

        var dto = new ApiMemberExtraction
        {
            SemanticUid = GenerateSemanticUid(symbol),
            Name = symbol.Name,
            Kind = "Event",
            Accessibility = symbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
            Modifiers = node.Modifiers.Select(m => m.Text).OrderBy(m => m).ToList(),
            ReturnType = symbol.Type.ToDisplayString(),
            Parameters = GetEventParameters(symbol),
            GenericParameters = new List<string>(),
            GenericConstraints = new List<string>(),
            Attributes = symbol.GetAttributes()
                .Select(a => a.AttributeClass?.ToDisplayString())
                .Where(a => a != null)
                .OrderBy(a => a)
                .ToList(),
            SummaryXml = symbol.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: default),
            Namespace = symbol.ContainingNamespace?.ToDisplayString(),
            DeclaringType = symbol.ContainingType?.ToDisplayString(),
            AssemblyName = _assemblyName,
            SourceFilePath = _filePath,
            StartLine = start,
            EndLine = end
        };

        Members.Add(dto);
    }

    private void ExtractEventField(EventFieldDeclarationSyntax node)
    {
        foreach (var variable in node.Declaration.Variables)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(variable) as IEventSymbol;
            if (symbol == null)
                continue;

            var (start, end) = GetSpan(node);

            var dto = new ApiMemberExtraction
            {
                SemanticUid = GenerateSemanticUid(symbol),
                Name = symbol.Name,
                Kind = "Event",
                Accessibility = symbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
                Modifiers = node.Modifiers.Select(m => m.Text).OrderBy(m => m).ToList(),
                ReturnType = symbol.Type.ToDisplayString(),
                Parameters = GetEventParameters(symbol),
                GenericParameters = new List<string>(),
                GenericConstraints = new List<string>(),
                Attributes = symbol.GetAttributes()
                    .Select(a => a.AttributeClass?.ToDisplayString())
                    .Where(a => a != null)
                    .OrderBy(a => a)
                    .ToList(),
                SummaryXml = symbol.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: default),
                Namespace = symbol.ContainingNamespace?.ToDisplayString(),
                DeclaringType = symbol.ContainingType?.ToDisplayString(),
                AssemblyName = _assemblyName,
                SourceFilePath = _filePath,
                StartLine = start,
                EndLine = end
            };

            Members.Add(dto);
        }
    }

    private static List<ApiParameterExtraction> GetEventParameters(IEventSymbol symbol)
    {
        if (symbol.Type is not INamedTypeSymbol delegateType)
            return new List<ApiParameterExtraction>();

        var invoke = delegateType.DelegateInvokeMethod;
        if (invoke == null)
            return new List<ApiParameterExtraction>();

        return invoke.Parameters.Select(ConvertParameter).ToList();
    }

    // =========================
    // Shared helpers
    // =========================

    private static ApiParameterExtraction ConvertParameter(IParameterSymbol p)
    {
        return new ApiParameterExtraction
        {
            SemanticUid = GenerateSemanticUid(p),
            Name = p.Name,
            Type = p.Type.ToDisplayString(),
            Nullable = p.NullableAnnotation.ToString(),
            Modifier = p.RefKind.ToString().ToLowerInvariant(),
            DefaultValue = p.HasExplicitDefaultValue ? p.ExplicitDefaultValue?.ToString() : null,
            Attributes = p.GetAttributes()
                .Select(a => a.AttributeClass?.ToDisplayString())
                .Where(a => a != null)
                .OrderBy(a => a)
                .ToList()
        };
    }

    private static (int Start, int End) GetSpan(SyntaxNode node)
    {
        var span = node.GetLocation().GetLineSpan();
        return (
            span.StartLinePosition.Line + 1,
            span.EndLinePosition.Line + 1
        );
    }

    private static string GenerateSemanticUid(ISymbol symbol)
    {
        if (symbol == null)
            return null;

        var fqName = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var kind = symbol.Kind.ToString();
        var meta = symbol.MetadataName;

        return $"{kind}:{fqName}:{meta}";
    }
}

#endregion

#region Orchestrator

public sealed class ApiExtractionOrchestrator
{
    public async Task<ApiExtractionResult> ExtractAsync(
        Solution solution,
        CancellationToken cancellationToken = default)
    {
        var allTypes = new List<ApiTypeExtraction>();
        var allMembers = new List<ApiMemberExtraction>();

        foreach (var project in solution.Projects.Where(p => p.Language == LanguageNames.CSharp))
        {
            var compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
            if (compilation == null)
                continue;

            var assemblyName = compilation.AssemblyName;

            foreach (var document in project.Documents.Where(d => d.SourceCodeKind == SourceCodeKind.Regular))
            {
                var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
                if (syntaxRoot == null)
                    continue;

                var semanticModel = compilation.GetSemanticModel(syntaxRoot.SyntaxTree);
                var walker = new ApiExtractionWalker(
                    semanticModel,
                    document.FilePath ?? document.Name,
                    assemblyName);

                walker.Visit(syntaxRoot);

                allTypes.AddRange(walker.Types);
                allMembers.AddRange(walker.Members);
            }
        }

        return new ApiExtractionResult(allTypes, allMembers);
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
                loader: TextLoader.From(TextAndVersion.Create(SourceText.From(text, System.Text.Encoding.UTF8),
                    VersionStamp.Create(), file)),
                filePath: file));
        }

        return solution;
    }





}

#endregion