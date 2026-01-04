// Project Name: SKAgent
// File Name: GitHubRoslynHarvesterBase.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Security.Cryptography;
using System.Text;

using ITCompanionAI.Helpers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

using Octokit;


namespace ITCompanionAI.AgentFramework.Ingestion;


/// <summary>
///     Base class for harvesters that need to retrieve source code from GitHub and analyze it with Roslyn workspaces.
/// </summary>
public abstract class GitHubRoslynHarvesterBase : HarvesterBase
{
    private readonly IGitHubClientFactory _gitHubClientFactory;





    /// <summary>
    ///     Initializes a new instance of the <see cref="GitHubRoslynHarvesterBase" /> class.
    /// </summary>
    /// <param name="gitHubClientFactory">Factory for creating authenticated GitHub clients.</param>
    protected GitHubRoslynHarvesterBase(IGitHubClientFactory gitHubClientFactory)
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

        async Task WalkAsync(string path)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyList<RepositoryContent> contents = string.IsNullOrWhiteSpace(path)
                ? await client.Repository.Content.GetAllContentsByRef(owner, repo, branch).ConfigureAwait(false)
                : await client.Repository.Content.GetAllContentsByRef(owner, repo, path, branch).ConfigureAwait(false);

            foreach (RepositoryContent item in contents)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!includePath(item.Path))
                {
                    continue;
                }

                if (item.Type.Value == ContentType.Dir)
                {
                    await WalkAsync(item.Path).ConfigureAwait(false);
                    continue;
                }

                if (item.Type.Value != ContentType.File)
                {
                    continue;
                }

                var localPath = Path.Combine(destinationDirectory, item.Path.Replace('/', Path.DirectorySeparatorChar));
                Directory.CreateDirectory(Path.GetDirectoryName(localPath)!);

                var bytes = item.EncodedContent is { Length: > 0 }
                    ? Convert.FromBase64String(item.EncodedContent)
                    : await client.Repository.Content.GetRawContentByRef(owner, repo, item.Path, branch)
                        .ConfigureAwait(false);

                await File.WriteAllBytesAsync(localPath, bytes, cancellationToken).ConfigureAwait(false);
            }
        }

        await WalkAsync(string.Empty).ConfigureAwait(false);
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
    protected static async Task<Solution> LoadSolutionAsync(string repositoryDirectory,
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
}