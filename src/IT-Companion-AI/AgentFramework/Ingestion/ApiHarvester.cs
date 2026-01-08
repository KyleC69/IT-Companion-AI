using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;


namespace ITCompanionAI.AgentFramework.Ingestion;

public class ApiHarvester
{
    public ApiHarvester(string filePath) : base(filePath)
    {
    }







    public async Task<ApiExtractionResult> ExtractAsync(
        Solution solution,
        CancellationToken cancellationToken = default)
    {

        var filePath = """d:\SKApiRepoRoot\semantic-kernel\dotnet\src""";
        var walker =new ApiExtractionWalker(filePath);

        var result = walker.LoadSolutionFromDirectory(filePath);

        walker;





    }
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
    public static async Task<Solution> LoadSolutionFromDirectoryAsync(string repositoryDirectory,
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
