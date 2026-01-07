// Project Name: SKAgent
// File Name: KBCurator.Converters.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public static partial class ApiFeatureConverter
{
    public static ApiFeatureDto ToDto(this ApiFeature source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static ApiFeatureDto ToDtoWithRelated(this ApiFeature source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiFeatureDto();

        // Properties
        target.Id = source.Id;
        target.ApiTypeId = source.ApiTypeId;
        target.SemanticUid = source.SemanticUid;
        target.TruthRunId = source.TruthRunId;
        target.Name = source.Name;
        target.Language = source.Language;
        target.Description = source.Description;
        target.Tags = source.Tags;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // Navigation Properties
        if (level > 0)
        {
            target.IngestionRun = source.IngestionRun.ToDtoWithRelated(level - 1);
            target.ApiMember = source.ApiMember.ToDtoWithRelated(level - 1);
        }

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static ApiFeature ToEntity(this ApiFeatureDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiFeature();

        // Properties
        target.Id = source.Id;
        target.ApiTypeId = source.ApiTypeId;
        target.SemanticUid = source.SemanticUid;
        target.TruthRunId = source.TruthRunId;
        target.Name = source.Name;
        target.Language = source.Language;
        target.Description = source.Description;
        target.Tags = source.Tags;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<ApiFeatureDto> ToDtos(this IEnumerable<ApiFeature> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiFeatureDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<ApiFeatureDto> ToDtosWithRelated(this IEnumerable<ApiFeature> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiFeatureDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<ApiFeature> ToEntities(this IEnumerable<ApiFeatureDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiFeature> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(ApiFeature source, ApiFeatureDto target);

    static partial void OnEntityCreating(ApiFeatureDto source, ApiFeature target);
}




public static partial class ApiMemberConverter
{
    public static ApiMemberDto ToDto(this ApiMember source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static ApiMemberDto ToDtoWithRelated(this ApiMember source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiMemberDto();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.ApiFeatureId = source.ApiFeatureId;
        target.Name = source.Name;
        target.Kind = source.Kind;
        target.MethodKind = source.MethodKind;
        target.Accessibility = source.Accessibility;
        target.IsStatic = source.IsStatic;
        target.IsExtensionMethod = source.IsExtensionMethod;
        target.IsAsync = source.IsAsync;
        target.IsVirtual = source.IsVirtual;
        target.IsOverride = source.IsOverride;
        target.IsAbstract = source.IsAbstract;
        target.IsSealed = source.IsSealed;
        target.IsReadonly = source.IsReadonly;
        target.IsConst = source.IsConst;
        target.IsUnsafe = source.IsUnsafe;
        target.ReturnTypeUid = source.ReturnTypeUid;
        target.ReturnNullable = source.ReturnNullable;
        target.GenericParameters = source.GenericParameters;
        target.GenericConstraints = source.GenericConstraints;
        target.Summary = source.Summary;
        target.Remarks = source.Remarks;
        target.Attributes = source.Attributes;
        target.SourceFilePath = source.SourceFilePath;
        target.SourceStartLine = source.SourceStartLine;
        target.SourceEndLine = source.SourceEndLine;
        target.MemberUidHash = source.MemberUidHash;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // Navigation Properties
        if (level > 0)
        {
            target.IngestionRun = source.IngestionRun.ToDtoWithRelated(level - 1);
            target.ApiFeature = source.ApiFeature.ToDtoWithRelated(level - 1);
        }

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static ApiMember ToEntity(this ApiMemberDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiMember();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.ApiFeatureId = source.ApiFeatureId;
        target.Name = source.Name;
        target.Kind = source.Kind;
        target.MethodKind = source.MethodKind;
        target.Accessibility = source.Accessibility;
        target.IsStatic = source.IsStatic;
        target.IsExtensionMethod = source.IsExtensionMethod;
        target.IsAsync = source.IsAsync;
        target.IsVirtual = source.IsVirtual;
        target.IsOverride = source.IsOverride;
        target.IsAbstract = source.IsAbstract;
        target.IsSealed = source.IsSealed;
        target.IsReadonly = source.IsReadonly;
        target.IsConst = source.IsConst;
        target.IsUnsafe = source.IsUnsafe;
        target.ReturnTypeUid = source.ReturnTypeUid;
        target.ReturnNullable = source.ReturnNullable;
        target.GenericParameters = source.GenericParameters;
        target.GenericConstraints = source.GenericConstraints;
        target.Summary = source.Summary;
        target.Remarks = source.Remarks;
        target.Attributes = source.Attributes;
        target.SourceFilePath = source.SourceFilePath;
        target.SourceStartLine = source.SourceStartLine;
        target.SourceEndLine = source.SourceEndLine;
        target.MemberUidHash = source.MemberUidHash;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<ApiMemberDto> ToDtos(this IEnumerable<ApiMember> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiMemberDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<ApiMemberDto> ToDtosWithRelated(this IEnumerable<ApiMember> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiMemberDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<ApiMember> ToEntities(this IEnumerable<ApiMemberDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiMember> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(ApiMember source, ApiMemberDto target);

    static partial void OnEntityCreating(ApiMemberDto source, ApiMember target);
}




public static partial class ApiMemberDiffConverter
{
    public static ApiMemberDiffDto ToDto(this ApiMemberDiff source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static ApiMemberDiffDto ToDtoWithRelated(this ApiMemberDiff source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiMemberDiffDto();

        // Properties
        target.Id = source.Id;
        target.SnapshotDiffId = source.SnapshotDiffId;
        target.MemberUid = source.MemberUid;
        target.ChangeKind = source.ChangeKind;
        target.OldSignature = source.OldSignature;
        target.NewSignature = source.NewSignature;
        target.Breaking = source.Breaking;
        target.DetailJson = source.DetailJson;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static ApiMemberDiff ToEntity(this ApiMemberDiffDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiMemberDiff();

        // Properties
        target.Id = source.Id;
        target.SnapshotDiffId = source.SnapshotDiffId;
        target.MemberUid = source.MemberUid;
        target.ChangeKind = source.ChangeKind;
        target.OldSignature = source.OldSignature;
        target.NewSignature = source.NewSignature;
        target.Breaking = source.Breaking;
        target.DetailJson = source.DetailJson;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<ApiMemberDiffDto> ToDtos(this IEnumerable<ApiMemberDiff> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiMemberDiffDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<ApiMemberDiffDto> ToDtosWithRelated(this IEnumerable<ApiMemberDiff> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiMemberDiffDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<ApiMemberDiff> ToEntities(this IEnumerable<ApiMemberDiffDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiMemberDiff> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(ApiMemberDiff source, ApiMemberDiffDto target);

    static partial void OnEntityCreating(ApiMemberDiffDto source, ApiMemberDiff target);
}




public static partial class ApiParameterConverter
{
    public static ApiParameterDto ToDto(this ApiParameter source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static ApiParameterDto ToDtoWithRelated(this ApiParameter source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiParameterDto();

        // Properties
        target.Id = source.Id;
        target.ApiMemberId = source.ApiMemberId;
        target.Name = source.Name;
        target.TypeUid = source.TypeUid;
        target.NullableAnnotation = source.NullableAnnotation;
        target.Position = source.Position;
        target.Modifier = source.Modifier;
        target.HasDefaultValue = source.HasDefaultValue;
        target.DefaultValueLiteral = source.DefaultValueLiteral;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // Navigation Properties
        if (level > 0)
        {
            target.IngestionRun = source.IngestionRun.ToDtoWithRelated(level - 1);
        }

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static ApiParameter ToEntity(this ApiParameterDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiParameter();

        // Properties
        target.Id = source.Id;
        target.ApiMemberId = source.ApiMemberId;
        target.Name = source.Name;
        target.TypeUid = source.TypeUid;
        target.NullableAnnotation = source.NullableAnnotation;
        target.Position = source.Position;
        target.Modifier = source.Modifier;
        target.HasDefaultValue = source.HasDefaultValue;
        target.DefaultValueLiteral = source.DefaultValueLiteral;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<ApiParameterDto> ToDtos(this IEnumerable<ApiParameter> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiParameterDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<ApiParameterDto> ToDtosWithRelated(this IEnumerable<ApiParameter> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiParameterDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<ApiParameter> ToEntities(this IEnumerable<ApiParameterDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiParameter> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(ApiParameter source, ApiParameterDto target);

    static partial void OnEntityCreating(ApiParameterDto source, ApiParameter target);
}




public static partial class ApiTypeConverter
{
    public static ApiTypeDto ToDto(this ApiType source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static ApiTypeDto ToDtoWithRelated(this ApiType source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiTypeDto();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.SourceSnapshotId = source.SourceSnapshotId;
        target.Name = source.Name;
        target.NamespacePath = source.NamespacePath;
        target.Kind = source.Kind;
        target.Accessibility = source.Accessibility;
        target.IsStatic = source.IsStatic;
        target.IsGeneric = source.IsGeneric;
        target.IsAbstract = source.IsAbstract;
        target.IsSealed = source.IsSealed;
        target.IsRecord = source.IsRecord;
        target.IsRefLike = source.IsRefLike;
        target.BaseTypeUid = source.BaseTypeUid;
        target.Interfaces = source.Interfaces;
        target.ContainingTypeUid = source.ContainingTypeUid;
        target.GenericParameters = source.GenericParameters;
        target.GenericConstraints = source.GenericConstraints;
        target.Summary = source.Summary;
        target.Remarks = source.Remarks;
        target.Attributes = source.Attributes;
        target.SourceFilePath = source.SourceFilePath;
        target.SourceStartLine = source.SourceStartLine;
        target.SourceEndLine = source.SourceEndLine;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // Navigation Properties
        if (level > 0)
        {
            target.IngestionRun = source.IngestionRun.ToDtoWithRelated(level - 1);
        }

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static ApiType ToEntity(this ApiTypeDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiType();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.SourceSnapshotId = source.SourceSnapshotId;
        target.Name = source.Name;
        target.NamespacePath = source.NamespacePath;
        target.Kind = source.Kind;
        target.Accessibility = source.Accessibility;
        target.IsStatic = source.IsStatic;
        target.IsGeneric = source.IsGeneric;
        target.IsAbstract = source.IsAbstract;
        target.IsSealed = source.IsSealed;
        target.IsRecord = source.IsRecord;
        target.IsRefLike = source.IsRefLike;
        target.BaseTypeUid = source.BaseTypeUid;
        target.Interfaces = source.Interfaces;
        target.ContainingTypeUid = source.ContainingTypeUid;
        target.GenericParameters = source.GenericParameters;
        target.GenericConstraints = source.GenericConstraints;
        target.Summary = source.Summary;
        target.Remarks = source.Remarks;
        target.Attributes = source.Attributes;
        target.SourceFilePath = source.SourceFilePath;
        target.SourceStartLine = source.SourceStartLine;
        target.SourceEndLine = source.SourceEndLine;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<ApiTypeDto> ToDtos(this IEnumerable<ApiType> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiTypeDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<ApiTypeDto> ToDtosWithRelated(this IEnumerable<ApiType> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiTypeDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<ApiType> ToEntities(this IEnumerable<ApiTypeDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiType> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(ApiType source, ApiTypeDto target);

    static partial void OnEntityCreating(ApiTypeDto source, ApiType target);
}




public static partial class ApiTypeDiffConverter
{
    public static ApiTypeDiffDto ToDto(this ApiTypeDiff source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static ApiTypeDiffDto ToDtoWithRelated(this ApiTypeDiff source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiTypeDiffDto();

        // Properties
        target.Id = source.Id;
        target.SnapshotDiffId = source.SnapshotDiffId;
        target.TypeUid = source.TypeUid;
        target.ChangeKind = source.ChangeKind;
        target.DetailJson = source.DetailJson;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static ApiTypeDiff ToEntity(this ApiTypeDiffDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ApiTypeDiff();

        // Properties
        target.Id = source.Id;
        target.SnapshotDiffId = source.SnapshotDiffId;
        target.TypeUid = source.TypeUid;
        target.ChangeKind = source.ChangeKind;
        target.DetailJson = source.DetailJson;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<ApiTypeDiffDto> ToDtos(this IEnumerable<ApiTypeDiff> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiTypeDiffDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<ApiTypeDiffDto> ToDtosWithRelated(this IEnumerable<ApiTypeDiff> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiTypeDiffDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<ApiTypeDiff> ToEntities(this IEnumerable<ApiTypeDiffDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ApiTypeDiff> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(ApiTypeDiff source, ApiTypeDiffDto target);

    static partial void OnEntityCreating(ApiTypeDiffDto source, ApiTypeDiff target);
}




public static partial class CodeBlockConverter
{
    public static CodeBlockDto ToDto(this CodeBlock source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static CodeBlockDto ToDtoWithRelated(this CodeBlock source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new CodeBlockDto();

        // Properties
        target.Id = source.Id;
        target.DocSectionId = source.DocSectionId;
        target.SemanticUid = source.SemanticUid;
        target.Language = source.Language;
        target.Content = source.Content;
        target.DeclaredPackages = source.DeclaredPackages;
        target.Tags = source.Tags;
        target.InlineComments = source.InlineComments;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static CodeBlock ToEntity(this CodeBlockDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new CodeBlock();

        // Properties
        target.Id = source.Id;
        target.DocSectionId = source.DocSectionId;
        target.SemanticUid = source.SemanticUid;
        target.Language = source.Language;
        target.Content = source.Content;
        target.DeclaredPackages = source.DeclaredPackages;
        target.Tags = source.Tags;
        target.InlineComments = source.InlineComments;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<CodeBlockDto> ToDtos(this IEnumerable<CodeBlock> source)
    {
        if (source == null)
        {
            return null;
        }

        List<CodeBlockDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<CodeBlockDto> ToDtosWithRelated(this IEnumerable<CodeBlock> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<CodeBlockDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<CodeBlock> ToEntities(this IEnumerable<CodeBlockDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<CodeBlock> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(CodeBlock source, CodeBlockDto target);

    static partial void OnEntityCreating(CodeBlockDto source, CodeBlock target);
}




public static partial class DocPageConverter
{
    public static DocPageDto ToDto(this DocPage source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static DocPageDto ToDtoWithRelated(this DocPage source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new DocPageDto();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.SourceSnapshotId = source.SourceSnapshotId;
        target.SourcePath = source.SourcePath;
        target.Title = source.Title;
        target.Language = source.Language;
        target.Url = source.Url;
        target.RawMarkdown = source.RawMarkdown;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // Navigation Properties
        if (level > 0)
        {
            target.IngestionRun = source.IngestionRun.ToDtoWithRelated(level - 1);
        }

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static DocPage ToEntity(this DocPageDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new DocPage();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.SourceSnapshotId = source.SourceSnapshotId;
        target.SourcePath = source.SourcePath;
        target.Title = source.Title;
        target.Language = source.Language;
        target.Url = source.Url;
        target.RawMarkdown = source.RawMarkdown;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<DocPageDto> ToDtos(this IEnumerable<DocPage> source)
    {
        if (source == null)
        {
            return null;
        }

        List<DocPageDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<DocPageDto> ToDtosWithRelated(this IEnumerable<DocPage> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<DocPageDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<DocPage> ToEntities(this IEnumerable<DocPageDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<DocPage> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(DocPage source, DocPageDto target);

    static partial void OnEntityCreating(DocPageDto source, DocPage target);
}




public static partial class DocPageDiffConverter
{
    public static DocPageDiffDto ToDto(this DocPageDiff source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static DocPageDiffDto ToDtoWithRelated(this DocPageDiff source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new DocPageDiffDto();

        // Properties
        target.Id = source.Id;
        target.SnapshotDiffId = source.SnapshotDiffId;
        target.DocUid = source.DocUid;
        target.ChangeKind = source.ChangeKind;
        target.DetailJson = source.DetailJson;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static DocPageDiff ToEntity(this DocPageDiffDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new DocPageDiff();

        // Properties
        target.Id = source.Id;
        target.SnapshotDiffId = source.SnapshotDiffId;
        target.DocUid = source.DocUid;
        target.ChangeKind = source.ChangeKind;
        target.DetailJson = source.DetailJson;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<DocPageDiffDto> ToDtos(this IEnumerable<DocPageDiff> source)
    {
        if (source == null)
        {
            return null;
        }

        List<DocPageDiffDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<DocPageDiffDto> ToDtosWithRelated(this IEnumerable<DocPageDiff> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<DocPageDiffDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<DocPageDiff> ToEntities(this IEnumerable<DocPageDiffDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<DocPageDiff> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(DocPageDiff source, DocPageDiffDto target);

    static partial void OnEntityCreating(DocPageDiffDto source, DocPageDiff target);
}




public static partial class DocSectionConverter
{
    public static DocSectionDto ToDto(this DocSection source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static DocSectionDto ToDtoWithRelated(this DocSection source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new DocSectionDto();

        // Properties
        target.Id = source.Id;
        target.DocPageId = source.DocPageId;
        target.SemanticUid = source.SemanticUid;
        target.Heading = source.Heading;
        target.Level = source.Level;
        target.ContentMarkdown = source.ContentMarkdown;
        target.OrderIndex = source.OrderIndex;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // Navigation Properties
        if (level > 0)
        {
            target.IngestionRun = source.IngestionRun.ToDtoWithRelated(level - 1);
        }

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static DocSection ToEntity(this DocSectionDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new DocSection();

        // Properties
        target.Id = source.Id;
        target.DocPageId = source.DocPageId;
        target.SemanticUid = source.SemanticUid;
        target.Heading = source.Heading;
        target.Level = source.Level;
        target.ContentMarkdown = source.ContentMarkdown;
        target.OrderIndex = source.OrderIndex;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<DocSectionDto> ToDtos(this IEnumerable<DocSection> source)
    {
        if (source == null)
        {
            return null;
        }

        List<DocSectionDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<DocSectionDto> ToDtosWithRelated(this IEnumerable<DocSection> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<DocSectionDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<DocSection> ToEntities(this IEnumerable<DocSectionDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<DocSection> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(DocSection source, DocSectionDto target);

    static partial void OnEntityCreating(DocSectionDto source, DocSection target);
}




public static partial class ExecutionResultConverter
{
    public static ExecutionResultDto ToDto(this ExecutionResult source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static ExecutionResultDto ToDtoWithRelated(this ExecutionResult source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ExecutionResultDto();

        // Properties
        target.Id = source.Id;
        target.ExecutionRunId = source.ExecutionRunId;
        target.SampleUid = source.SampleUid;
        target.Status = source.Status;
        target.BuildLog = source.BuildLog;
        target.RunLog = source.RunLog;
        target.ExceptionJson = source.ExceptionJson;
        target.DurationMs = source.DurationMs;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static ExecutionResult ToEntity(this ExecutionResultDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ExecutionResult();

        // Properties
        target.Id = source.Id;
        target.ExecutionRunId = source.ExecutionRunId;
        target.SampleUid = source.SampleUid;
        target.Status = source.Status;
        target.BuildLog = source.BuildLog;
        target.RunLog = source.RunLog;
        target.ExceptionJson = source.ExceptionJson;
        target.DurationMs = source.DurationMs;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<ExecutionResultDto> ToDtos(this IEnumerable<ExecutionResult> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ExecutionResultDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<ExecutionResultDto> ToDtosWithRelated(this IEnumerable<ExecutionResult> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<ExecutionResultDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<ExecutionResult> ToEntities(this IEnumerable<ExecutionResultDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ExecutionResult> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(ExecutionResult source, ExecutionResultDto target);

    static partial void OnEntityCreating(ExecutionResultDto source, ExecutionResult target);
}




public static partial class ExecutionRunConverter
{
    public static ExecutionRunDto ToDto(this ExecutionRun source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static ExecutionRunDto ToDtoWithRelated(this ExecutionRun source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ExecutionRunDto();

        // Properties
        target.Id = source.Id;
        target.SnapshotId = source.SnapshotId;
        target.SampleRunId = source.SampleRunId;
        target.TimestampUtc = source.TimestampUtc;
        target.EnvironmentJson = source.EnvironmentJson;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static ExecutionRun ToEntity(this ExecutionRunDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ExecutionRun();

        // Properties
        target.Id = source.Id;
        target.SnapshotId = source.SnapshotId;
        target.SampleRunId = source.SampleRunId;
        target.TimestampUtc = source.TimestampUtc;
        target.EnvironmentJson = source.EnvironmentJson;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<ExecutionRunDto> ToDtos(this IEnumerable<ExecutionRun> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ExecutionRunDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<ExecutionRunDto> ToDtosWithRelated(this IEnumerable<ExecutionRun> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<ExecutionRunDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<ExecutionRun> ToEntities(this IEnumerable<ExecutionRunDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ExecutionRun> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(ExecutionRun source, ExecutionRunDto target);

    static partial void OnEntityCreating(ExecutionRunDto source, ExecutionRun target);
}




public static partial class FeatureDocLinkConverter
{
    public static FeatureDocLinkDto ToDto(this FeatureDocLink source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static FeatureDocLinkDto ToDtoWithRelated(this FeatureDocLink source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new FeatureDocLinkDto();

        // Properties
        target.Id = source.Id;
        target.FeatureId = source.FeatureId;
        target.DocUid = source.DocUid;
        target.SectionUid = source.SectionUid;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static FeatureDocLink ToEntity(this FeatureDocLinkDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new FeatureDocLink();

        // Properties
        target.Id = source.Id;
        target.FeatureId = source.FeatureId;
        target.DocUid = source.DocUid;
        target.SectionUid = source.SectionUid;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<FeatureDocLinkDto> ToDtos(this IEnumerable<FeatureDocLink> source)
    {
        if (source == null)
        {
            return null;
        }

        List<FeatureDocLinkDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<FeatureDocLinkDto> ToDtosWithRelated(this IEnumerable<FeatureDocLink> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<FeatureDocLinkDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<FeatureDocLink> ToEntities(this IEnumerable<FeatureDocLinkDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<FeatureDocLink> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(FeatureDocLink source, FeatureDocLinkDto target);

    static partial void OnEntityCreating(FeatureDocLinkDto source, FeatureDocLink target);
}




public static partial class FeatureMemberLinkConverter
{
    public static FeatureMemberLinkDto ToDto(this FeatureMemberLink source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static FeatureMemberLinkDto ToDtoWithRelated(this FeatureMemberLink source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new FeatureMemberLinkDto();

        // Properties
        target.Id = source.Id;
        target.FeatureId = source.FeatureId;
        target.MemberUid = source.MemberUid;
        target.Role = source.Role;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static FeatureMemberLink ToEntity(this FeatureMemberLinkDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new FeatureMemberLink();

        // Properties
        target.Id = source.Id;
        target.FeatureId = source.FeatureId;
        target.MemberUid = source.MemberUid;
        target.Role = source.Role;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<FeatureMemberLinkDto> ToDtos(this IEnumerable<FeatureMemberLink> source)
    {
        if (source == null)
        {
            return null;
        }

        List<FeatureMemberLinkDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<FeatureMemberLinkDto> ToDtosWithRelated(this IEnumerable<FeatureMemberLink> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<FeatureMemberLinkDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<FeatureMemberLink> ToEntities(this IEnumerable<FeatureMemberLinkDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<FeatureMemberLink> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(FeatureMemberLink source, FeatureMemberLinkDto target);

    static partial void OnEntityCreating(FeatureMemberLinkDto source, FeatureMemberLink target);
}




public static partial class FeatureTypeLinkConverter
{
    public static FeatureTypeLinkDto ToDto(this FeatureTypeLink source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static FeatureTypeLinkDto ToDtoWithRelated(this FeatureTypeLink source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new FeatureTypeLinkDto();

        // Properties
        target.Id = source.Id;
        target.FeatureId = source.FeatureId;
        target.TypeUid = source.TypeUid;
        target.Role = source.Role;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static FeatureTypeLink ToEntity(this FeatureTypeLinkDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new FeatureTypeLink();

        // Properties
        target.Id = source.Id;
        target.FeatureId = source.FeatureId;
        target.TypeUid = source.TypeUid;
        target.Role = source.Role;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<FeatureTypeLinkDto> ToDtos(this IEnumerable<FeatureTypeLink> source)
    {
        if (source == null)
        {
            return null;
        }

        List<FeatureTypeLinkDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<FeatureTypeLinkDto> ToDtosWithRelated(this IEnumerable<FeatureTypeLink> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<FeatureTypeLinkDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<FeatureTypeLink> ToEntities(this IEnumerable<FeatureTypeLinkDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<FeatureTypeLink> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(FeatureTypeLink source, FeatureTypeLinkDto target);

    static partial void OnEntityCreating(FeatureTypeLinkDto source, FeatureTypeLink target);
}




public static partial class IngestionRunConverter
{
    public static IngestionRunDto ToDto(this IngestionRun source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static IngestionRunDto ToDtoWithRelated(this IngestionRun source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new IngestionRunDto();

        // Properties
        target.Id = source.Id;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;
        target.Notes = source.Notes;

        // Navigation Properties
        if (level > 0)
        {
            target.ApiFeatures = source.ApiFeatures.ToDtosWithRelated(level - 1);
            target.ApiMembers = source.ApiMembers.ToDtosWithRelated(level - 1);
            target.ApiParameters = source.ApiParameters.ToDtosWithRelated(level - 1);
            target.ApiTypes = source.ApiTypes.ToDtosWithRelated(level - 1);
            target.DocPages = source.DocPages.ToDtosWithRelated(level - 1);
            target.DocSections = source.DocSections.ToDtosWithRelated(level - 1);
        }

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static IngestionRun ToEntity(this IngestionRunDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new IngestionRun();

        // Properties
        target.Id = source.Id;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;
        target.Notes = source.Notes;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<IngestionRunDto> ToDtos(this IEnumerable<IngestionRun> source)
    {
        if (source == null)
        {
            return null;
        }

        List<IngestionRunDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<IngestionRunDto> ToDtosWithRelated(this IEnumerable<IngestionRun> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<IngestionRunDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<IngestionRun> ToEntities(this IEnumerable<IngestionRunDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<IngestionRun> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(IngestionRun source, IngestionRunDto target);

    static partial void OnEntityCreating(IngestionRunDto source, IngestionRun target);
}




public static partial class RagChunkConverter
{
    public static RagChunkDto ToDto(this RagChunk source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static RagChunkDto ToDtoWithRelated(this RagChunk source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new RagChunkDto();

        // Properties
        target.Id = source.Id;
        target.RagRunId = source.RagRunId;
        target.ChunkUid = source.ChunkUid;
        target.Kind = source.Kind;
        target.Text = source.Text;
        target.MetadataJson = source.MetadataJson;
        target.EmbeddingVector = source.EmbeddingVector;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static RagChunk ToEntity(this RagChunkDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new RagChunk();

        // Properties
        target.Id = source.Id;
        target.RagRunId = source.RagRunId;
        target.ChunkUid = source.ChunkUid;
        target.Kind = source.Kind;
        target.Text = source.Text;
        target.MetadataJson = source.MetadataJson;
        target.EmbeddingVector = source.EmbeddingVector;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<RagChunkDto> ToDtos(this IEnumerable<RagChunk> source)
    {
        if (source == null)
        {
            return null;
        }

        List<RagChunkDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<RagChunkDto> ToDtosWithRelated(this IEnumerable<RagChunk> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<RagChunkDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<RagChunk> ToEntities(this IEnumerable<RagChunkDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<RagChunk> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(RagChunk source, RagChunkDto target);

    static partial void OnEntityCreating(RagChunkDto source, RagChunk target);
}




public static partial class RagRunConverter
{
    public static RagRunDto ToDto(this RagRun source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static RagRunDto ToDtoWithRelated(this RagRun source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new RagRunDto();

        // Properties
        target.Id = source.Id;
        target.SnapshotId = source.SnapshotId;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static RagRun ToEntity(this RagRunDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new RagRun();

        // Properties
        target.Id = source.Id;
        target.SnapshotId = source.SnapshotId;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<RagRunDto> ToDtos(this IEnumerable<RagRun> source)
    {
        if (source == null)
        {
            return null;
        }

        List<RagRunDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<RagRunDto> ToDtosWithRelated(this IEnumerable<RagRun> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<RagRunDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<RagRun> ToEntities(this IEnumerable<RagRunDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<RagRun> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(RagRun source, RagRunDto target);

    static partial void OnEntityCreating(RagRunDto source, RagRun target);
}




public static partial class ReviewIssueConverter
{
    public static ReviewIssueDto ToDto(this ReviewIssue source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static ReviewIssueDto ToDtoWithRelated(this ReviewIssue source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ReviewIssueDto();

        // Properties
        target.Id = source.Id;
        target.ReviewItemId = source.ReviewItemId;
        target.Code = source.Code;
        target.Severity = source.Severity;
        target.RelatedMemberUid = source.RelatedMemberUid;
        target.Details = source.Details;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static ReviewIssue ToEntity(this ReviewIssueDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ReviewIssue();

        // Properties
        target.Id = source.Id;
        target.ReviewItemId = source.ReviewItemId;
        target.Code = source.Code;
        target.Severity = source.Severity;
        target.RelatedMemberUid = source.RelatedMemberUid;
        target.Details = source.Details;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<ReviewIssueDto> ToDtos(this IEnumerable<ReviewIssue> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ReviewIssueDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<ReviewIssueDto> ToDtosWithRelated(this IEnumerable<ReviewIssue> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<ReviewIssueDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<ReviewIssue> ToEntities(this IEnumerable<ReviewIssueDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ReviewIssue> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(ReviewIssue source, ReviewIssueDto target);

    static partial void OnEntityCreating(ReviewIssueDto source, ReviewIssue target);
}




public static partial class ReviewItemConverter
{
    public static ReviewItemDto ToDto(this ReviewItem source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static ReviewItemDto ToDtoWithRelated(this ReviewItem source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ReviewItemDto();

        // Properties
        target.Id = source.Id;
        target.ReviewRunId = source.ReviewRunId;
        target.TargetKind = source.TargetKind;
        target.TargetUid = source.TargetUid;
        target.Status = source.Status;
        target.Summary = source.Summary;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static ReviewItem ToEntity(this ReviewItemDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ReviewItem();

        // Properties
        target.Id = source.Id;
        target.ReviewRunId = source.ReviewRunId;
        target.TargetKind = source.TargetKind;
        target.TargetUid = source.TargetUid;
        target.Status = source.Status;
        target.Summary = source.Summary;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<ReviewItemDto> ToDtos(this IEnumerable<ReviewItem> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ReviewItemDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<ReviewItemDto> ToDtosWithRelated(this IEnumerable<ReviewItem> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<ReviewItemDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<ReviewItem> ToEntities(this IEnumerable<ReviewItemDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ReviewItem> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(ReviewItem source, ReviewItemDto target);

    static partial void OnEntityCreating(ReviewItemDto source, ReviewItem target);
}




public static partial class ReviewRunConverter
{
    public static ReviewRunDto ToDto(this ReviewRun source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static ReviewRunDto ToDtoWithRelated(this ReviewRun source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ReviewRunDto();

        // Properties
        target.Id = source.Id;
        target.SnapshotId = source.SnapshotId;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static ReviewRun ToEntity(this ReviewRunDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new ReviewRun();

        // Properties
        target.Id = source.Id;
        target.SnapshotId = source.SnapshotId;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<ReviewRunDto> ToDtos(this IEnumerable<ReviewRun> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ReviewRunDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<ReviewRunDto> ToDtosWithRelated(this IEnumerable<ReviewRun> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<ReviewRunDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<ReviewRun> ToEntities(this IEnumerable<ReviewRunDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<ReviewRun> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(ReviewRun source, ReviewRunDto target);

    static partial void OnEntityCreating(ReviewRunDto source, ReviewRun target);
}




public static partial class SampleConverter
{
    public static SampleDto ToDto(this Sample source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SampleDto ToDtoWithRelated(this Sample source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SampleDto();

        // Properties
        target.Id = source.Id;
        target.SampleRunId = source.SampleRunId;
        target.SampleUid = source.SampleUid;
        target.FeatureUid = source.FeatureUid;
        target.Language = source.Language;
        target.Code = source.Code;
        target.EntryPoint = source.EntryPoint;
        target.TargetFramework = source.TargetFramework;
        target.PackageReferences = source.PackageReferences;
        target.DerivedFromCodeUid = source.DerivedFromCodeUid;
        target.Tags = source.Tags;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static Sample ToEntity(this SampleDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new Sample();

        // Properties
        target.Id = source.Id;
        target.SampleRunId = source.SampleRunId;
        target.SampleUid = source.SampleUid;
        target.FeatureUid = source.FeatureUid;
        target.Language = source.Language;
        target.Code = source.Code;
        target.EntryPoint = source.EntryPoint;
        target.TargetFramework = source.TargetFramework;
        target.PackageReferences = source.PackageReferences;
        target.DerivedFromCodeUid = source.DerivedFromCodeUid;
        target.Tags = source.Tags;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SampleDto> ToDtos(this IEnumerable<Sample> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SampleDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SampleDto> ToDtosWithRelated(this IEnumerable<Sample> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SampleDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<Sample> ToEntities(this IEnumerable<SampleDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<Sample> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(Sample source, SampleDto target);

    static partial void OnEntityCreating(SampleDto source, Sample target);
}




public static partial class SampleApiMemberLinkConverter
{
    public static SampleApiMemberLinkDto ToDto(this SampleApiMemberLink source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SampleApiMemberLinkDto ToDtoWithRelated(this SampleApiMemberLink source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SampleApiMemberLinkDto();

        // Properties
        target.Id = source.Id;
        target.SampleId = source.SampleId;
        target.MemberUid = source.MemberUid;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static SampleApiMemberLink ToEntity(this SampleApiMemberLinkDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SampleApiMemberLink();

        // Properties
        target.Id = source.Id;
        target.SampleId = source.SampleId;
        target.MemberUid = source.MemberUid;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SampleApiMemberLinkDto> ToDtos(this IEnumerable<SampleApiMemberLink> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SampleApiMemberLinkDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SampleApiMemberLinkDto> ToDtosWithRelated(this IEnumerable<SampleApiMemberLink> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SampleApiMemberLinkDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<SampleApiMemberLink> ToEntities(this IEnumerable<SampleApiMemberLinkDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SampleApiMemberLink> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(SampleApiMemberLink source, SampleApiMemberLinkDto target);

    static partial void OnEntityCreating(SampleApiMemberLinkDto source, SampleApiMemberLink target);
}




public static partial class SampleRunConverter
{
    public static SampleRunDto ToDto(this SampleRun source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SampleRunDto ToDtoWithRelated(this SampleRun source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SampleRunDto();

        // Properties
        target.Id = source.Id;
        target.SnapshotId = source.SnapshotId;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static SampleRun ToEntity(this SampleRunDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SampleRun();

        // Properties
        target.Id = source.Id;
        target.SnapshotId = source.SnapshotId;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SampleRunDto> ToDtos(this IEnumerable<SampleRun> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SampleRunDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SampleRunDto> ToDtosWithRelated(this IEnumerable<SampleRun> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SampleRunDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<SampleRun> ToEntities(this IEnumerable<SampleRunDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SampleRun> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(SampleRun source, SampleRunDto target);

    static partial void OnEntityCreating(SampleRunDto source, SampleRun target);
}




public static partial class SemanticIdentityConverter
{
    public static SemanticIdentityDto ToDto(this SemanticIdentity source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SemanticIdentityDto ToDtoWithRelated(this SemanticIdentity source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SemanticIdentityDto();

        // Properties
        target.Uid = source.Uid;
        target.UidHash = source.UidHash;
        target.Kind = source.Kind;
        target.CreatedUtc = source.CreatedUtc;
        target.Notes = source.Notes;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static SemanticIdentity ToEntity(this SemanticIdentityDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SemanticIdentity();

        // Properties
        target.Uid = source.Uid;
        target.UidHash = source.UidHash;
        target.Kind = source.Kind;
        target.CreatedUtc = source.CreatedUtc;
        target.Notes = source.Notes;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SemanticIdentityDto> ToDtos(this IEnumerable<SemanticIdentity> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SemanticIdentityDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SemanticIdentityDto> ToDtosWithRelated(this IEnumerable<SemanticIdentity> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SemanticIdentityDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<SemanticIdentity> ToEntities(this IEnumerable<SemanticIdentityDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SemanticIdentity> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(SemanticIdentity source, SemanticIdentityDto target);

    static partial void OnEntityCreating(SemanticIdentityDto source, SemanticIdentity target);
}




public static partial class SnapshotDiffConverter
{
    public static SnapshotDiffDto ToDto(this SnapshotDiff source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SnapshotDiffDto ToDtoWithRelated(this SnapshotDiff source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SnapshotDiffDto();

        // Properties
        target.Id = source.Id;
        target.OldSnapshotId = source.OldSnapshotId;
        target.NewSnapshotId = source.NewSnapshotId;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static SnapshotDiff ToEntity(this SnapshotDiffDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SnapshotDiff();

        // Properties
        target.Id = source.Id;
        target.OldSnapshotId = source.OldSnapshotId;
        target.NewSnapshotId = source.NewSnapshotId;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SnapshotDiffDto> ToDtos(this IEnumerable<SnapshotDiff> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SnapshotDiffDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SnapshotDiffDto> ToDtosWithRelated(this IEnumerable<SnapshotDiff> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SnapshotDiffDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<SnapshotDiff> ToEntities(this IEnumerable<SnapshotDiffDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SnapshotDiff> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(SnapshotDiff source, SnapshotDiffDto target);

    static partial void OnEntityCreating(SnapshotDiffDto source, SnapshotDiff target);
}




public static partial class SourceSnapshotConverter
{
    public static SourceSnapshotDto ToDto(this SourceSnapshot source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SourceSnapshotDto ToDtoWithRelated(this SourceSnapshot source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SourceSnapshotDto();

        // Properties
        target.Id = source.Id;
        target.IngestionRunId = source.IngestionRunId;
        target.SnapshotUid = source.SnapshotUid;
        target.RepoUrl = source.RepoUrl;
        target.Branch = source.Branch;
        target.RepoCommit = source.RepoCommit;
        target.Language = source.Language;
        target.PackageName = source.PackageName;
        target.PackageVersion = source.PackageVersion;
        target.ConfigJson = source.ConfigJson;
        target.SnapshotUidHash = source.SnapshotUidHash;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static SourceSnapshot ToEntity(this SourceSnapshotDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SourceSnapshot();

        // Properties
        target.Id = source.Id;
        target.IngestionRunId = source.IngestionRunId;
        target.SnapshotUid = source.SnapshotUid;
        target.RepoUrl = source.RepoUrl;
        target.Branch = source.Branch;
        target.RepoCommit = source.RepoCommit;
        target.Language = source.Language;
        target.PackageName = source.PackageName;
        target.PackageVersion = source.PackageVersion;
        target.ConfigJson = source.ConfigJson;
        target.SnapshotUidHash = source.SnapshotUidHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SourceSnapshotDto> ToDtos(this IEnumerable<SourceSnapshot> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SourceSnapshotDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SourceSnapshotDto> ToDtosWithRelated(this IEnumerable<SourceSnapshot> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SourceSnapshotDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<SourceSnapshot> ToEntities(this IEnumerable<SourceSnapshotDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SourceSnapshot> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(SourceSnapshot source, SourceSnapshotDto target);

    static partial void OnEntityCreating(SourceSnapshotDto source, SourceSnapshot target);
}




public static partial class TruthRunConverter
{
    public static TruthRunDto ToDto(this TruthRun source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static TruthRunDto ToDtoWithRelated(this TruthRun source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new TruthRunDto();

        // Properties
        target.Id = source.Id;
        target.SnapshotId = source.SnapshotId;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static TruthRun ToEntity(this TruthRunDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new TruthRun();

        // Properties
        target.Id = source.Id;
        target.SnapshotId = source.SnapshotId;
        target.TimestampUtc = source.TimestampUtc;
        target.SchemaVersion = source.SchemaVersion;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<TruthRunDto> ToDtos(this IEnumerable<TruthRun> source)
    {
        if (source == null)
        {
            return null;
        }

        List<TruthRunDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<TruthRunDto> ToDtosWithRelated(this IEnumerable<TruthRun> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<TruthRunDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<TruthRun> ToEntities(this IEnumerable<TruthRunDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<TruthRun> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(TruthRun source, TruthRunDto target);

    static partial void OnEntityCreating(TruthRunDto source, TruthRun target);
}




public static partial class VApiFeatureCurrentConverter
{
    public static VApiFeatureCurrentDto ToDto(this VApiFeatureCurrent source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static VApiFeatureCurrentDto ToDtoWithRelated(this VApiFeatureCurrent source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new VApiFeatureCurrentDto();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.TruthRunId = source.TruthRunId;
        target.Name = source.Name;
        target.Language = source.Language;
        target.Description = source.Description;
        target.Tags = source.Tags;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static VApiFeatureCurrent ToEntity(this VApiFeatureCurrentDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new VApiFeatureCurrent();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.TruthRunId = source.TruthRunId;
        target.Name = source.Name;
        target.Language = source.Language;
        target.Description = source.Description;
        target.Tags = source.Tags;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<VApiFeatureCurrentDto> ToDtos(this IEnumerable<VApiFeatureCurrent> source)
    {
        if (source == null)
        {
            return null;
        }

        List<VApiFeatureCurrentDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<VApiFeatureCurrentDto> ToDtosWithRelated(this IEnumerable<VApiFeatureCurrent> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<VApiFeatureCurrentDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<VApiFeatureCurrent> ToEntities(this IEnumerable<VApiFeatureCurrentDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<VApiFeatureCurrent> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(VApiFeatureCurrent source, VApiFeatureCurrentDto target);

    static partial void OnEntityCreating(VApiFeatureCurrentDto source, VApiFeatureCurrent target);
}




public static partial class VApiMemberCurrentConverter
{
    public static VApiMemberCurrentDto ToDto(this VApiMemberCurrent source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static VApiMemberCurrentDto ToDtoWithRelated(this VApiMemberCurrent source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new VApiMemberCurrentDto();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.ApiTypeId = source.ApiTypeId;
        target.Name = source.Name;
        target.Kind = source.Kind;
        target.MethodKind = source.MethodKind;
        target.Accessibility = source.Accessibility;
        target.IsStatic = source.IsStatic;
        target.IsExtensionMethod = source.IsExtensionMethod;
        target.IsAsync = source.IsAsync;
        target.IsVirtual = source.IsVirtual;
        target.IsOverride = source.IsOverride;
        target.IsAbstract = source.IsAbstract;
        target.IsSealed = source.IsSealed;
        target.IsReadonly = source.IsReadonly;
        target.IsConst = source.IsConst;
        target.IsUnsafe = source.IsUnsafe;
        target.ReturnTypeUid = source.ReturnTypeUid;
        target.ReturnNullable = source.ReturnNullable;
        target.GenericParameters = source.GenericParameters;
        target.GenericConstraints = source.GenericConstraints;
        target.Summary = source.Summary;
        target.Remarks = source.Remarks;
        target.Attributes = source.Attributes;
        target.SourceFilePath = source.SourceFilePath;
        target.SourceStartLine = source.SourceStartLine;
        target.SourceEndLine = source.SourceEndLine;
        target.MemberUidHash = source.MemberUidHash;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static VApiMemberCurrent ToEntity(this VApiMemberCurrentDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new VApiMemberCurrent();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.ApiTypeId = source.ApiTypeId;
        target.Name = source.Name;
        target.Kind = source.Kind;
        target.MethodKind = source.MethodKind;
        target.Accessibility = source.Accessibility;
        target.IsStatic = source.IsStatic;
        target.IsExtensionMethod = source.IsExtensionMethod;
        target.IsAsync = source.IsAsync;
        target.IsVirtual = source.IsVirtual;
        target.IsOverride = source.IsOverride;
        target.IsAbstract = source.IsAbstract;
        target.IsSealed = source.IsSealed;
        target.IsReadonly = source.IsReadonly;
        target.IsConst = source.IsConst;
        target.IsUnsafe = source.IsUnsafe;
        target.ReturnTypeUid = source.ReturnTypeUid;
        target.ReturnNullable = source.ReturnNullable;
        target.GenericParameters = source.GenericParameters;
        target.GenericConstraints = source.GenericConstraints;
        target.Summary = source.Summary;
        target.Remarks = source.Remarks;
        target.Attributes = source.Attributes;
        target.SourceFilePath = source.SourceFilePath;
        target.SourceStartLine = source.SourceStartLine;
        target.SourceEndLine = source.SourceEndLine;
        target.MemberUidHash = source.MemberUidHash;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<VApiMemberCurrentDto> ToDtos(this IEnumerable<VApiMemberCurrent> source)
    {
        if (source == null)
        {
            return null;
        }

        List<VApiMemberCurrentDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<VApiMemberCurrentDto> ToDtosWithRelated(this IEnumerable<VApiMemberCurrent> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<VApiMemberCurrentDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<VApiMemberCurrent> ToEntities(this IEnumerable<VApiMemberCurrentDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<VApiMemberCurrent> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(VApiMemberCurrent source, VApiMemberCurrentDto target);

    static partial void OnEntityCreating(VApiMemberCurrentDto source, VApiMemberCurrent target);
}




public static partial class VApiTypeCurrentConverter
{
    public static VApiTypeCurrentDto ToDto(this VApiTypeCurrent source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static VApiTypeCurrentDto ToDtoWithRelated(this VApiTypeCurrent source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new VApiTypeCurrentDto();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.SourceSnapshotId = source.SourceSnapshotId;
        target.Name = source.Name;
        target.NamespacePath = source.NamespacePath;
        target.Kind = source.Kind;
        target.Accessibility = source.Accessibility;
        target.IsStatic = source.IsStatic;
        target.IsGeneric = source.IsGeneric;
        target.IsAbstract = source.IsAbstract;
        target.IsSealed = source.IsSealed;
        target.IsRecord = source.IsRecord;
        target.IsRefLike = source.IsRefLike;
        target.BaseTypeUid = source.BaseTypeUid;
        target.Interfaces = source.Interfaces;
        target.ContainingTypeUid = source.ContainingTypeUid;
        target.GenericParameters = source.GenericParameters;
        target.GenericConstraints = source.GenericConstraints;
        target.Summary = source.Summary;
        target.Remarks = source.Remarks;
        target.Attributes = source.Attributes;
        target.SourceFilePath = source.SourceFilePath;
        target.SourceStartLine = source.SourceStartLine;
        target.SourceEndLine = source.SourceEndLine;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static VApiTypeCurrent ToEntity(this VApiTypeCurrentDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new VApiTypeCurrent();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.SourceSnapshotId = source.SourceSnapshotId;
        target.Name = source.Name;
        target.NamespacePath = source.NamespacePath;
        target.Kind = source.Kind;
        target.Accessibility = source.Accessibility;
        target.IsStatic = source.IsStatic;
        target.IsGeneric = source.IsGeneric;
        target.IsAbstract = source.IsAbstract;
        target.IsSealed = source.IsSealed;
        target.IsRecord = source.IsRecord;
        target.IsRefLike = source.IsRefLike;
        target.BaseTypeUid = source.BaseTypeUid;
        target.Interfaces = source.Interfaces;
        target.ContainingTypeUid = source.ContainingTypeUid;
        target.GenericParameters = source.GenericParameters;
        target.GenericConstraints = source.GenericConstraints;
        target.Summary = source.Summary;
        target.Remarks = source.Remarks;
        target.Attributes = source.Attributes;
        target.SourceFilePath = source.SourceFilePath;
        target.SourceStartLine = source.SourceStartLine;
        target.SourceEndLine = source.SourceEndLine;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<VApiTypeCurrentDto> ToDtos(this IEnumerable<VApiTypeCurrent> source)
    {
        if (source == null)
        {
            return null;
        }

        List<VApiTypeCurrentDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<VApiTypeCurrentDto> ToDtosWithRelated(this IEnumerable<VApiTypeCurrent> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<VApiTypeCurrentDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<VApiTypeCurrent> ToEntities(this IEnumerable<VApiTypeCurrentDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<VApiTypeCurrent> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(VApiTypeCurrent source, VApiTypeCurrentDto target);

    static partial void OnEntityCreating(VApiTypeCurrentDto source, VApiTypeCurrent target);
}




public static partial class VDocPageCurrentConverter
{
    public static VDocPageCurrentDto ToDto(this VDocPageCurrent source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static VDocPageCurrentDto ToDtoWithRelated(this VDocPageCurrent source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new VDocPageCurrentDto();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.SourceSnapshotId = source.SourceSnapshotId;
        target.SourcePath = source.SourcePath;
        target.Title = source.Title;
        target.Language = source.Language;
        target.Url = source.Url;
        target.RawMarkdown = source.RawMarkdown;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static VDocPageCurrent ToEntity(this VDocPageCurrentDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new VDocPageCurrent();

        // Properties
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;
        target.SourceSnapshotId = source.SourceSnapshotId;
        target.SourcePath = source.SourcePath;
        target.Title = source.Title;
        target.Language = source.Language;
        target.Url = source.Url;
        target.RawMarkdown = source.RawMarkdown;
        target.VersionNumber = source.VersionNumber;
        target.CreatedIngestionRunId = source.CreatedIngestionRunId;
        target.UpdatedIngestionRunId = source.UpdatedIngestionRunId;
        target.RemovedIngestionRunId = source.RemovedIngestionRunId;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.IsActive = source.IsActive;
        target.ContentHash = source.ContentHash;
        target.SemanticUidHash = source.SemanticUidHash;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<VDocPageCurrentDto> ToDtos(this IEnumerable<VDocPageCurrent> source)
    {
        if (source == null)
        {
            return null;
        }

        List<VDocPageCurrentDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<VDocPageCurrentDto> ToDtosWithRelated(this IEnumerable<VDocPageCurrent> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<VDocPageCurrentDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<VDocPageCurrent> ToEntities(this IEnumerable<VDocPageCurrentDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<VDocPageCurrent> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(VDocPageCurrent source, VDocPageCurrentDto target);

    static partial void OnEntityCreating(VDocPageCurrentDto source, VDocPageCurrent target);
}




public static partial class SpCheckTemporalConsistencyResultConverter
{
    public static SpCheckTemporalConsistencyResultDto ToDto(this SpCheckTemporalConsistencyResult source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SpCheckTemporalConsistencyResultDto ToDtoWithRelated(this SpCheckTemporalConsistencyResult source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpCheckTemporalConsistencyResultDto();

        // Properties
        target.TableName = source.TableName;
        target.SemanticUid = source.SemanticUid;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static SpCheckTemporalConsistencyResult ToEntity(this SpCheckTemporalConsistencyResultDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpCheckTemporalConsistencyResult();

        // Properties
        target.TableName = source.TableName;
        target.SemanticUid = source.SemanticUid;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SpCheckTemporalConsistencyResultDto> ToDtos(this IEnumerable<SpCheckTemporalConsistencyResult> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpCheckTemporalConsistencyResultDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SpCheckTemporalConsistencyResultDto> ToDtosWithRelated(this IEnumerable<SpCheckTemporalConsistencyResult> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SpCheckTemporalConsistencyResultDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<SpCheckTemporalConsistencyResult> ToEntities(this IEnumerable<SpCheckTemporalConsistencyResultDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpCheckTemporalConsistencyResult> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(SpCheckTemporalConsistencyResult source, SpCheckTemporalConsistencyResultDto target);

    static partial void OnEntityCreating(SpCheckTemporalConsistencyResultDto source, SpCheckTemporalConsistencyResult target);
}




public static partial class SpCheckTemporalConsistencyResult1Converter
{
    public static SpCheckTemporalConsistencyResult1Dto ToDto(this SpCheckTemporalConsistencyResult1 source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SpCheckTemporalConsistencyResult1Dto ToDtoWithRelated(this SpCheckTemporalConsistencyResult1 source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpCheckTemporalConsistencyResult1Dto();

        // Properties
        target.TableName = source.TableName;
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static SpCheckTemporalConsistencyResult1 ToEntity(this SpCheckTemporalConsistencyResult1Dto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpCheckTemporalConsistencyResult1();

        // Properties
        target.TableName = source.TableName;
        target.Id = source.Id;
        target.SemanticUid = source.SemanticUid;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SpCheckTemporalConsistencyResult1Dto> ToDtos(this IEnumerable<SpCheckTemporalConsistencyResult1> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpCheckTemporalConsistencyResult1Dto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SpCheckTemporalConsistencyResult1Dto> ToDtosWithRelated(this IEnumerable<SpCheckTemporalConsistencyResult1> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SpCheckTemporalConsistencyResult1Dto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<SpCheckTemporalConsistencyResult1> ToEntities(this IEnumerable<SpCheckTemporalConsistencyResult1Dto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpCheckTemporalConsistencyResult1> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(SpCheckTemporalConsistencyResult1 source, SpCheckTemporalConsistencyResult1Dto target);

    static partial void OnEntityCreating(SpCheckTemporalConsistencyResult1Dto source, SpCheckTemporalConsistencyResult1 target);
}




public static partial class SpCheckTemporalConsistencyResult2Converter
{
    public static SpCheckTemporalConsistencyResult2Dto ToDto(this SpCheckTemporalConsistencyResult2 source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SpCheckTemporalConsistencyResult2Dto ToDtoWithRelated(this SpCheckTemporalConsistencyResult2 source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpCheckTemporalConsistencyResult2Dto();

        // Properties
        target.TableName = source.TableName;
        target.SemanticUid = source.SemanticUid;
        target.VersionNumber = source.VersionNumber;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.NextFrom = source.NextFrom;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static SpCheckTemporalConsistencyResult2 ToEntity(this SpCheckTemporalConsistencyResult2Dto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpCheckTemporalConsistencyResult2();

        // Properties
        target.TableName = source.TableName;
        target.SemanticUid = source.SemanticUid;
        target.VersionNumber = source.VersionNumber;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;
        target.NextFrom = source.NextFrom;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SpCheckTemporalConsistencyResult2Dto> ToDtos(this IEnumerable<SpCheckTemporalConsistencyResult2> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpCheckTemporalConsistencyResult2Dto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SpCheckTemporalConsistencyResult2Dto> ToDtosWithRelated(this IEnumerable<SpCheckTemporalConsistencyResult2> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SpCheckTemporalConsistencyResult2Dto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<SpCheckTemporalConsistencyResult2> ToEntities(this IEnumerable<SpCheckTemporalConsistencyResult2Dto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpCheckTemporalConsistencyResult2> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(SpCheckTemporalConsistencyResult2 source, SpCheckTemporalConsistencyResult2Dto target);

    static partial void OnEntityCreating(SpCheckTemporalConsistencyResult2Dto source, SpCheckTemporalConsistencyResult2 target);
}




public static partial class SpGetChangesInIngestionRunResultConverter
{
    public static SpGetChangesInIngestionRunResultDto ToDto(this SpGetChangesInIngestionRunResult source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SpGetChangesInIngestionRunResultDto ToDtoWithRelated(this SpGetChangesInIngestionRunResult source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpGetChangesInIngestionRunResultDto();

        // Properties
        target.ArtifactKind = source.ArtifactKind;
        target.SemanticUid = source.SemanticUid;
        target.VersionNumber = source.VersionNumber;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static SpGetChangesInIngestionRunResult ToEntity(this SpGetChangesInIngestionRunResultDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpGetChangesInIngestionRunResult();

        // Properties
        target.ArtifactKind = source.ArtifactKind;
        target.SemanticUid = source.SemanticUid;
        target.VersionNumber = source.VersionNumber;
        target.ValidFromUtc = source.ValidFromUtc;
        target.ValidToUtc = source.ValidToUtc;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SpGetChangesInIngestionRunResultDto> ToDtos(this IEnumerable<SpGetChangesInIngestionRunResult> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpGetChangesInIngestionRunResultDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SpGetChangesInIngestionRunResultDto> ToDtosWithRelated(this IEnumerable<SpGetChangesInIngestionRunResult> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SpGetChangesInIngestionRunResultDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<SpGetChangesInIngestionRunResult> ToEntities(this IEnumerable<SpGetChangesInIngestionRunResultDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpGetChangesInIngestionRunResult> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(SpGetChangesInIngestionRunResult source, SpGetChangesInIngestionRunResultDto target);

    static partial void OnEntityCreating(SpGetChangesInIngestionRunResultDto source, SpGetChangesInIngestionRunResult target);
}




public static partial class SpUpsertApiParameterResultConverter
{
    public static SpUpsertApiParameterResultDto ToDto(this SpUpsertApiParameterResult source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SpUpsertApiParameterResultDto ToDtoWithRelated(this SpUpsertApiParameterResult source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpUpsertApiParameterResultDto();

        // Properties
        target.Id = source.Id;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static SpUpsertApiParameterResult ToEntity(this SpUpsertApiParameterResultDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpUpsertApiParameterResult();

        // Properties
        target.Id = source.Id;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SpUpsertApiParameterResultDto> ToDtos(this IEnumerable<SpUpsertApiParameterResult> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpUpsertApiParameterResultDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SpUpsertApiParameterResultDto> ToDtosWithRelated(this IEnumerable<SpUpsertApiParameterResult> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SpUpsertApiParameterResultDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<SpUpsertApiParameterResult> ToEntities(this IEnumerable<SpUpsertApiParameterResultDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpUpsertApiParameterResult> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(SpUpsertApiParameterResult source, SpUpsertApiParameterResultDto target);

    static partial void OnEntityCreating(SpUpsertApiParameterResultDto source, SpUpsertApiParameterResult target);
}




public static partial class SpVerifyIngestionRunResultConverter
{
    public static SpVerifyIngestionRunResultDto ToDto(this SpVerifyIngestionRunResult source)
    {
        return source.ToDtoWithRelated(0);
    }







    public static SpVerifyIngestionRunResultDto ToDtoWithRelated(this SpVerifyIngestionRunResult source, int level)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpVerifyIngestionRunResultDto();

        // Properties
        target.Category = source.Category;
        target.Detail = source.Detail;

        // User-defined partial method
        OnDtoCreating(source, target);

        return target;
    }







    public static SpVerifyIngestionRunResult ToEntity(this SpVerifyIngestionRunResultDto source)
    {
        if (source == null)
        {
            return null;
        }

        var target = new SpVerifyIngestionRunResult();

        // Properties
        target.Category = source.Category;
        target.Detail = source.Detail;

        // User-defined partial method
        OnEntityCreating(source, target);

        return target;
    }







    public static List<SpVerifyIngestionRunResultDto> ToDtos(this IEnumerable<SpVerifyIngestionRunResult> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpVerifyIngestionRunResultDto> target = source
            .Select(src => src.ToDto())
            .ToList();

        return target;
    }







    public static List<SpVerifyIngestionRunResultDto> ToDtosWithRelated(this IEnumerable<SpVerifyIngestionRunResult> source, int level)
    {
        if (source == null)
        {
            return null;
        }

        List<SpVerifyIngestionRunResultDto> target = source
            .Select(src => src.ToDtoWithRelated(level))
            .ToList();

        return target;
    }







    public static List<SpVerifyIngestionRunResult> ToEntities(this IEnumerable<SpVerifyIngestionRunResultDto> source)
    {
        if (source == null)
        {
            return null;
        }

        List<SpVerifyIngestionRunResult> target = source
            .Select(src => src.ToEntity())
            .ToList();

        return target;
    }







    static partial void OnDtoCreating(SpVerifyIngestionRunResult source, SpVerifyIngestionRunResultDto target);

    static partial void OnEntityCreating(SpVerifyIngestionRunResultDto source, SpVerifyIngestionRunResult target);
}