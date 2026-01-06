// Project Name: SKAgent
// File Name: Ingestion.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Ingestion.Contracts;

public sealed class IngestionArtifact
{
    public IngestionRunInfo IngestionRun { get; set; } = default!;
    public ApiSurface ApiSurface { get; set; } = new();
    public Docs Docs { get; set; } = new();
}




public sealed class IngestionRunInfo
{
    public string RunId { get; set; } = default!; // uuid/ulid string
    public DateTime TimestampUtc { get; set; }
    public string SchemaVersion { get; set; } = "1.0.0";
    public SourceSnapshotInfo SourceSnapshot { get; set; } = default!;
}




public sealed class SourceSnapshotInfo
{
    public string SnapshotId { get; set; } = default!; // uuid/ulid string
    public string RepoUrl { get; set; } = default!;
    public string Branch { get; set; } = default!;
    public string Commit { get; set; } = default!;
    public string Language { get; set; } = default!; // "csharp", "python", ...
    public string PackageName { get; set; } = default!;
    public string PackageVersion { get; set; } = default!;
    public SourceConfig Config { get; set; } = new();
}




public sealed class SourceConfig
{
    public bool IncludePrivate { get; set; }
    public List<string> DocPaths { get; set; } = [];
    public List<string> ApiRoots { get; set; } = [];
}




public sealed class ApiSurface
{
    public List<ApiTypeInfo> Types { get; set; } = [];
}




public sealed class ApiTypeInfo
{
    public string TypeUid { get; set; } = default!; // "csharp:Namespace.Type"
    public string Name { get; set; } = default!;
    public string Namespace { get; set; } = default!;
    public string Kind { get; set; } = default!; // "class", "interface", ...
    public string Accessibility { get; set; } = default!; // "public", "internal", ...
    public bool IsStatic { get; set; }
    public bool IsGeneric { get; set; }
    public List<string> GenericParameters { get; set; } = [];
    public string? Summary { get; set; }
    public string? Remarks { get; set; }
    public List<ApiAttributeInfo> Attributes { get; set; } = [];
    public List<ApiMemberInfo> Members { get; set; } = [];
}




public sealed class ApiAttributeInfo
{
    public string Name { get; set; } = default!;
    public List<string> CtorArguments { get; set; } = [];
}




public sealed class ApiMemberInfo
{
    public string MemberUid { get; set; } = default!; // "csharp:Namespace.Type::Method(...)"
    public string Name { get; set; } = default!;
    public string Kind { get; set; } = default!; // "method", "property", ...
    public string Accessibility { get; set; } = default!;
    public bool IsStatic { get; set; }
    public bool IsExtensionMethod { get; set; }
    public bool IsAsync { get; set; }
    public string? ReturnType { get; set; }
    public string? Summary { get; set; }
    public string? Remarks { get; set; }
    public List<string> GenericParameters { get; set; } = [];
    public List<ApiAttributeInfo> Attributes { get; set; } = [];
    public List<ApiParameterInfo> Parameters { get; set; } = [];
    public SourceLocationInfo? SourceLocation { get; set; }
    public List<ApiMemberDocLink> DocLinks { get; set; } = [];
}




public sealed class ApiParameterInfo
{
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public int Position { get; set; }
    public bool HasDefaultValue { get; set; }
    public string? DefaultValueLiteral { get; set; }
}




public sealed class SourceLocationInfo
{
    public string FilePath { get; set; } = default!;
    public int StartLine { get; set; }
    public int EndLine { get; set; }
}




public sealed class ApiMemberDocLink
{
    public string DocUid { get; set; } = default!; // "doc:csharp:..."
    public string SectionUid { get; set; } = default!; // "sec:..."
}




public sealed class Docs
{
    public List<DocPageInfo> Pages { get; set; } = [];
}




public sealed class DocPageInfo
{
    public string DocUid { get; set; } = default!; // "doc:csharp:..."
    public string SourcePath { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Language { get; set; } = default!;
    public string? Url { get; set; }
    public string? RawMarkdown { get; set; }
    public List<DocSectionInfo> Sections { get; set; } = [];
}




public sealed class DocSectionInfo
{
    public string SectionUid { get; set; } = default!; // "sec:..."
    public string Heading { get; set; } = default!;
    public int Level { get; set; }
    public int OrderIndex { get; set; }
    public string ContentMarkdown { get; set; } = default!;
    public List<CodeBlockInfo> CodeBlocks { get; set; } = [];
}




public sealed class CodeBlockInfo
{
    public string CodeUid { get; set; } = default!; // "code:..."
    public string Language { get; set; } = default!;
    public string Content { get; set; } = default!;
    public List<DeclaredPackageInfo> DeclaredPackages { get; set; } = [];
    public List<string> Tags { get; set; } = [];
    public string? InlineComments { get; set; }
}




public sealed class DeclaredPackageInfo
{
    public string Name { get; set; } = default!;
    public string? Version { get; set; }
}