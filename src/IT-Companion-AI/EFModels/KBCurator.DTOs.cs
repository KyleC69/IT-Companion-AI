// Project Name: SKAgent
// File Name: KBCurator.DTOs.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public class ApiFeatureDto
{
    #region Constructors

    public ApiFeatureDto()
    {
    }







    public ApiFeatureDto(Guid id, Guid? apiTypeId, string semanticUid, Guid truthRunId, string name, string language, string description, string tags, int versionNumber, Guid createdIngestionRunId, Guid updatedIngestionRunId, Guid? removedIngestionRunId, DateTime validFromUtc, DateTime? validToUtc, bool isActive, byte[] contentHash, byte[] semanticUidHash, IngestionRunDto ingestionRun, ApiMemberDto apiMember)
    {

        Id = id;
        ApiTypeId = apiTypeId;
        SemanticUid = semanticUid;
        TruthRunId = truthRunId;
        Name = name;
        Language = language;
        Description = description;
        Tags = tags;
        VersionNumber = versionNumber;
        CreatedIngestionRunId = createdIngestionRunId;
        UpdatedIngestionRunId = updatedIngestionRunId;
        RemovedIngestionRunId = removedIngestionRunId;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        IsActive = isActive;
        ContentHash = contentHash;
        SemanticUidHash = semanticUidHash;
        IngestionRun = ingestionRun;
        ApiMember = apiMember;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    public Guid? ApiTypeId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SemanticUid { get; set; }

    [NotNullValidator()] public Guid TruthRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Name { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    public string Description { get; set; }

    public string Tags { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator()] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator()] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }

    #endregion

    #region Navigation Properties

    public IngestionRunDto IngestionRun { get; set; }

    public ApiMemberDto ApiMember { get; set; }

    #endregion
}




public class ApiMemberDto
{
    #region Constructors

    public ApiMemberDto()
    {
    }







    public ApiMemberDto(Guid id, string semanticUid, Guid apiFeatureId, string name, string kind, string methodKind, string accessibility, bool? isStatic, bool? isExtensionMethod, bool? isAsync, bool? isVirtual, bool? isOverride, bool? isAbstract, bool? isSealed, bool? isReadonly, bool? isConst, bool? isUnsafe, string returnTypeUid, string returnNullable, string genericParameters, string genericConstraints, string summary, string remarks, string attributes, string sourceFilePath, int? sourceStartLine, int? sourceEndLine, byte[] memberUidHash, int versionNumber, Guid createdIngestionRunId, Guid? updatedIngestionRunId, Guid? removedIngestionRunId, DateTime validFromUtc, DateTime? validToUtc, bool isActive, byte[] contentHash, byte[] semanticUidHash, IngestionRunDto ingestionRun, ApiFeatureDto apiFeature)
    {

        Id = id;
        SemanticUid = semanticUid;
        ApiFeatureId = apiFeatureId;
        Name = name;
        Kind = kind;
        MethodKind = methodKind;
        Accessibility = accessibility;
        IsStatic = isStatic;
        IsExtensionMethod = isExtensionMethod;
        IsAsync = isAsync;
        IsVirtual = isVirtual;
        IsOverride = isOverride;
        IsAbstract = isAbstract;
        IsSealed = isSealed;
        IsReadonly = isReadonly;
        IsConst = isConst;
        IsUnsafe = isUnsafe;
        ReturnTypeUid = returnTypeUid;
        ReturnNullable = returnNullable;
        GenericParameters = genericParameters;
        GenericConstraints = genericConstraints;
        Summary = summary;
        Remarks = remarks;
        Attributes = attributes;
        SourceFilePath = sourceFilePath;
        SourceStartLine = sourceStartLine;
        SourceEndLine = sourceEndLine;
        MemberUidHash = memberUidHash;
        VersionNumber = versionNumber;
        CreatedIngestionRunId = createdIngestionRunId;
        UpdatedIngestionRunId = updatedIngestionRunId;
        RemovedIngestionRunId = removedIngestionRunId;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        IsActive = isActive;
        ContentHash = contentHash;
        SemanticUidHash = semanticUidHash;
        IngestionRun = ingestionRun;
        ApiFeature = apiFeature;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SemanticUid { get; set; }

    [NotNullValidator()] public Guid ApiFeatureId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Name { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Kind { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string MethodKind { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Accessibility { get; set; }

    public bool? IsStatic { get; set; }

    public bool? IsExtensionMethod { get; set; }

    public bool? IsAsync { get; set; }

    public bool? IsVirtual { get; set; }

    public bool? IsOverride { get; set; }

    public bool? IsAbstract { get; set; }

    public bool? IsSealed { get; set; }

    public bool? IsReadonly { get; set; }

    public bool? IsConst { get; set; }

    public bool? IsUnsafe { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string ReturnTypeUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string ReturnNullable { get; set; }

    public string GenericParameters { get; set; }

    public string GenericConstraints { get; set; }

    public string Summary { get; set; }

    public string Remarks { get; set; }

    public string Attributes { get; set; }

    public string SourceFilePath { get; set; }

    public int? SourceStartLine { get; set; }

    public int? SourceEndLine { get; set; }

    public byte[] MemberUidHash { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public Guid CreatedIngestionRunId { get; set; }

    public Guid? UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator()] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }

    #endregion

    #region Navigation Properties

    public IngestionRunDto IngestionRun { get; set; }

    public ApiFeatureDto ApiFeature { get; set; }

    #endregion
}




public class ApiMemberDiffDto
{
    #region Constructors

    public ApiMemberDiffDto()
    {
    }







    public ApiMemberDiffDto(Guid id, Guid snapshotDiffId, string memberUid, string changeKind, string oldSignature, string newSignature, bool? breaking, string detailJson)
    {

        Id = id;
        SnapshotDiffId = snapshotDiffId;
        MemberUid = memberUid;
        ChangeKind = changeKind;
        OldSignature = oldSignature;
        NewSignature = newSignature;
        Breaking = breaking;
        DetailJson = detailJson;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid SnapshotDiffId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string MemberUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string ChangeKind { get; set; }

    public string OldSignature { get; set; }

    public string NewSignature { get; set; }

    public bool? Breaking { get; set; }

    public string DetailJson { get; set; }

    #endregion
}




public class ApiParameterDto
{
    #region Navigation Properties

    public IngestionRunDto IngestionRun { get; set; }

    #endregion

    #region Constructors

    public ApiParameterDto()
    {
    }







    public ApiParameterDto(Guid id, Guid apiMemberId, string name, string typeUid, string nullableAnnotation, int? position, string modifier, bool? hasDefaultValue, string defaultValueLiteral, int versionNumber, Guid createdIngestionRunId, Guid updatedIngestionRunId, Guid? removedIngestionRunId, DateTime validFromUtc, DateTime? validToUtc, bool isActive, byte[] contentHash, byte[] semanticUidHash, IngestionRunDto ingestionRun)
    {

        Id = id;
        ApiMemberId = apiMemberId;
        Name = name;
        TypeUid = typeUid;
        NullableAnnotation = nullableAnnotation;
        Position = position;
        Modifier = modifier;
        HasDefaultValue = hasDefaultValue;
        DefaultValueLiteral = defaultValueLiteral;
        VersionNumber = versionNumber;
        CreatedIngestionRunId = createdIngestionRunId;
        UpdatedIngestionRunId = updatedIngestionRunId;
        RemovedIngestionRunId = removedIngestionRunId;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        IsActive = isActive;
        ContentHash = contentHash;
        SemanticUidHash = semanticUidHash;
        IngestionRun = ingestionRun;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid ApiMemberId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Name { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string TypeUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string NullableAnnotation { get; set; }

    public int? Position { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string Modifier { get; set; }

    public bool? HasDefaultValue { get; set; }

    public string DefaultValueLiteral { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator()] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator()] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }

    #endregion
}




public class ApiTypeDto
{
    #region Navigation Properties

    public IngestionRunDto IngestionRun { get; set; }

    #endregion

    #region Constructors

    public ApiTypeDto()
    {
    }







    public ApiTypeDto(Guid id, string semanticUid, Guid sourceSnapshotId, string name, string namespacePath, string kind, string accessibility, bool? isStatic, bool? isGeneric, bool? isAbstract, bool? isSealed, bool? isRecord, bool? isRefLike, string baseTypeUid, string interfaces, string containingTypeUid, string genericParameters, string genericConstraints, string summary, string remarks, string attributes, string sourceFilePath, int? sourceStartLine, int? sourceEndLine, int versionNumber, Guid createdIngestionRunId, Guid updatedIngestionRunId, Guid? removedIngestionRunId, DateTime validFromUtc, DateTime? validToUtc, bool isActive, byte[] contentHash, byte[] semanticUidHash, IngestionRunDto ingestionRun)
    {

        Id = id;
        SemanticUid = semanticUid;
        SourceSnapshotId = sourceSnapshotId;
        Name = name;
        NamespacePath = namespacePath;
        Kind = kind;
        Accessibility = accessibility;
        IsStatic = isStatic;
        IsGeneric = isGeneric;
        IsAbstract = isAbstract;
        IsSealed = isSealed;
        IsRecord = isRecord;
        IsRefLike = isRefLike;
        BaseTypeUid = baseTypeUid;
        Interfaces = interfaces;
        ContainingTypeUid = containingTypeUid;
        GenericParameters = genericParameters;
        GenericConstraints = genericConstraints;
        Summary = summary;
        Remarks = remarks;
        Attributes = attributes;
        SourceFilePath = sourceFilePath;
        SourceStartLine = sourceStartLine;
        SourceEndLine = sourceEndLine;
        VersionNumber = versionNumber;
        CreatedIngestionRunId = createdIngestionRunId;
        UpdatedIngestionRunId = updatedIngestionRunId;
        RemovedIngestionRunId = removedIngestionRunId;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        IsActive = isActive;
        ContentHash = contentHash;
        SemanticUidHash = semanticUidHash;
        IngestionRun = ingestionRun;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SemanticUid { get; set; }

    [NotNullValidator()] public Guid SourceSnapshotId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Name { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string NamespacePath { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Kind { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Accessibility { get; set; }

    public bool? IsStatic { get; set; }

    public bool? IsGeneric { get; set; }

    public bool? IsAbstract { get; set; }

    public bool? IsSealed { get; set; }

    public bool? IsRecord { get; set; }

    public bool? IsRefLike { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string BaseTypeUid { get; set; }

    public string Interfaces { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string ContainingTypeUid { get; set; }

    public string GenericParameters { get; set; }

    public string GenericConstraints { get; set; }

    public string Summary { get; set; }

    public string Remarks { get; set; }

    public string Attributes { get; set; }

    public string SourceFilePath { get; set; }

    public int? SourceStartLine { get; set; }

    public int? SourceEndLine { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator()] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator()] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }

    #endregion
}




public class ApiTypeDiffDto
{
    #region Constructors

    public ApiTypeDiffDto()
    {
    }







    public ApiTypeDiffDto(Guid id, Guid snapshotDiffId, string typeUid, string changeKind, string detailJson)
    {

        Id = id;
        SnapshotDiffId = snapshotDiffId;
        TypeUid = typeUid;
        ChangeKind = changeKind;
        DetailJson = detailJson;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid SnapshotDiffId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string TypeUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string ChangeKind { get; set; }

    public string DetailJson { get; set; }

    #endregion
}




public class CodeBlockDto
{
    #region Constructors

    public CodeBlockDto()
    {
    }







    public CodeBlockDto(Guid id, Guid docSectionId, string semanticUid, string language, string content, string declaredPackages, string tags, string inlineComments, int versionNumber, Guid createdIngestionRunId, Guid updatedIngestionRunId, Guid? removedIngestionRunId, DateTime validFromUtc, DateTime? validToUtc, bool isActive, byte[] contentHash)
    {

        Id = id;
        DocSectionId = docSectionId;
        SemanticUid = semanticUid;
        Language = language;
        Content = content;
        DeclaredPackages = declaredPackages;
        Tags = tags;
        InlineComments = inlineComments;
        VersionNumber = versionNumber;
        CreatedIngestionRunId = createdIngestionRunId;
        UpdatedIngestionRunId = updatedIngestionRunId;
        RemovedIngestionRunId = removedIngestionRunId;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        IsActive = isActive;
        ContentHash = contentHash;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid DocSectionId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string SemanticUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    public string Content { get; set; }

    public string DeclaredPackages { get; set; }

    public string Tags { get; set; }

    public string InlineComments { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator()] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator()] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    #endregion
}




public class DocPageDto
{
    #region Navigation Properties

    public IngestionRunDto IngestionRun { get; set; }

    #endregion

    #region Constructors

    public DocPageDto()
    {
    }







    public DocPageDto(Guid id, string semanticUid, Guid sourceSnapshotId, string sourcePath, string title, string language, string url, string rawMarkdown, int versionNumber, Guid createdIngestionRunId, Guid updatedIngestionRunId, Guid? removedIngestionRunId, DateTime validFromUtc, DateTime? validToUtc, bool isActive, byte[] contentHash, byte[] semanticUidHash, IngestionRunDto ingestionRun)
    {

        Id = id;
        SemanticUid = semanticUid;
        SourceSnapshotId = sourceSnapshotId;
        SourcePath = sourcePath;
        Title = title;
        Language = language;
        Url = url;
        RawMarkdown = rawMarkdown;
        VersionNumber = versionNumber;
        CreatedIngestionRunId = createdIngestionRunId;
        UpdatedIngestionRunId = updatedIngestionRunId;
        RemovedIngestionRunId = removedIngestionRunId;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        IsActive = isActive;
        ContentHash = contentHash;
        SemanticUidHash = semanticUidHash;
        IngestionRun = ingestionRun;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SemanticUid { get; set; }

    [NotNullValidator()] public Guid SourceSnapshotId { get; set; }

    public string SourcePath { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Title { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    public string Url { get; set; }

    public string RawMarkdown { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator()] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator()] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }

    #endregion
}




public class DocPageDiffDto
{
    #region Constructors

    public DocPageDiffDto()
    {
    }







    public DocPageDiffDto(Guid id, Guid snapshotDiffId, string docUid, string changeKind, string detailJson)
    {

        Id = id;
        SnapshotDiffId = snapshotDiffId;
        DocUid = docUid;
        ChangeKind = changeKind;
        DetailJson = detailJson;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid SnapshotDiffId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string DocUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string ChangeKind { get; set; }

    public string DetailJson { get; set; }

    #endregion
}




public class DocSectionDto
{
    #region Navigation Properties

    public IngestionRunDto IngestionRun { get; set; }

    #endregion

    #region Constructors

    public DocSectionDto()
    {
    }







    public DocSectionDto(Guid id, Guid docPageId, string semanticUid, string heading, int? level, string contentMarkdown, int? orderIndex, int versionNumber, Guid createdIngestionRunId, Guid updatedIngestionRunId, Guid? removedIngestionRunId, DateTime validFromUtc, DateTime? validToUtc, bool isActive, byte[] contentHash, byte[] semanticUidHash, IngestionRunDto ingestionRun)
    {

        Id = id;
        DocPageId = docPageId;
        SemanticUid = semanticUid;
        Heading = heading;
        Level = level;
        ContentMarkdown = contentMarkdown;
        OrderIndex = orderIndex;
        VersionNumber = versionNumber;
        CreatedIngestionRunId = createdIngestionRunId;
        UpdatedIngestionRunId = updatedIngestionRunId;
        RemovedIngestionRunId = removedIngestionRunId;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        IsActive = isActive;
        ContentHash = contentHash;
        SemanticUidHash = semanticUidHash;
        IngestionRun = ingestionRun;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid DocPageId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SemanticUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Heading { get; set; }

    public int? Level { get; set; }

    public string ContentMarkdown { get; set; }

    public int? OrderIndex { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator()] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator()] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }

    #endregion
}




public class ExecutionResultDto
{
    #region Constructors

    public ExecutionResultDto()
    {
    }







    public ExecutionResultDto(Guid id, Guid executionRunId, string sampleUid, string status, string buildLog, string runLog, string exceptionJson, int? durationMs)
    {

        Id = id;
        ExecutionRunId = executionRunId;
        SampleUid = sampleUid;
        Status = status;
        BuildLog = buildLog;
        RunLog = runLog;
        ExceptionJson = exceptionJson;
        DurationMs = durationMs;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid ExecutionRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SampleUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 100, RangeBoundaryType.Inclusive)]
    public string Status { get; set; }

    public string BuildLog { get; set; }

    public string RunLog { get; set; }

    public string ExceptionJson { get; set; }

    public int? DurationMs { get; set; }

    #endregion
}




public class ExecutionRunDto
{
    #region Constructors

    public ExecutionRunDto()
    {
    }







    public ExecutionRunDto(Guid id, Guid snapshotId, Guid sampleRunId, DateTime timestampUtc, string environmentJson, string schemaVersion)
    {

        Id = id;
        SnapshotId = snapshotId;
        SampleRunId = sampleRunId;
        TimestampUtc = timestampUtc;
        EnvironmentJson = environmentJson;
        SchemaVersion = schemaVersion;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid SnapshotId { get; set; }

    [NotNullValidator()] public Guid SampleRunId { get; set; }

    [NotNullValidator()] public DateTime TimestampUtc { get; set; }

    public string EnvironmentJson { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SchemaVersion { get; set; }

    #endregion
}




public class FeatureDocLinkDto
{
    #region Constructors

    public FeatureDocLinkDto()
    {
    }







    public FeatureDocLinkDto(Guid id, Guid featureId, string docUid, string sectionUid)
    {

        Id = id;
        FeatureId = featureId;
        DocUid = docUid;
        SectionUid = sectionUid;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid FeatureId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string DocUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string SectionUid { get; set; }

    #endregion
}




public class FeatureMemberLinkDto
{
    #region Constructors

    public FeatureMemberLinkDto()
    {
    }







    public FeatureMemberLinkDto(Guid id, Guid featureId, string memberUid, string role)
    {

        Id = id;
        FeatureId = featureId;
        MemberUid = memberUid;
        Role = role;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid FeatureId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string MemberUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string Role { get; set; }

    #endregion
}




public class FeatureTypeLinkDto
{
    #region Constructors

    public FeatureTypeLinkDto()
    {
    }







    public FeatureTypeLinkDto(Guid id, Guid featureId, string typeUid, string role)
    {

        Id = id;
        FeatureId = featureId;
        TypeUid = typeUid;
        Role = role;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid FeatureId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string TypeUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string Role { get; set; }

    #endregion
}




public class IngestionRunDto
{
    #region Constructors

    public IngestionRunDto()
    {
    }







    public IngestionRunDto(Guid id, DateTime timestampUtc, string schemaVersion, string notes, List<ApiFeatureDto> apiFeatures, List<ApiMemberDto> apiMembers, List<ApiParameterDto> apiParameters, List<ApiTypeDto> apiTypes, List<DocPageDto> docPages, List<DocSectionDto> docSections)
    {

        Id = id;
        TimestampUtc = timestampUtc;
        SchemaVersion = schemaVersion;
        Notes = notes;
        ApiFeatures = apiFeatures;
        ApiMembers = apiMembers;
        ApiParameters = apiParameters;
        ApiTypes = apiTypes;
        DocPages = docPages;
        DocSections = docSections;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public DateTime TimestampUtc { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SchemaVersion { get; set; }

    public string Notes { get; set; }

    #endregion

    #region Navigation Properties

    public List<ApiFeatureDto> ApiFeatures { get; set; }

    public List<ApiMemberDto> ApiMembers { get; set; }

    public List<ApiParameterDto> ApiParameters { get; set; }

    public List<ApiTypeDto> ApiTypes { get; set; }

    public List<DocPageDto> DocPages { get; set; }

    public List<DocSectionDto> DocSections { get; set; }

    #endregion
}




public class RagChunkDto
{
    #region Constructors

    public RagChunkDto()
    {
    }







    public RagChunkDto(Guid id, Guid ragRunId, string chunkUid, string kind, string text, string metadataJson, string embeddingVector)
    {

        Id = id;
        RagRunId = ragRunId;
        ChunkUid = chunkUid;
        Kind = kind;
        Text = text;
        MetadataJson = metadataJson;
        EmbeddingVector = embeddingVector;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid RagRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string ChunkUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 100, RangeBoundaryType.Inclusive)]
    public string Kind { get; set; }

    public string Text { get; set; }

    public string MetadataJson { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1536, RangeBoundaryType.Inclusive)]
    public string EmbeddingVector { get; set; }

    #endregion
}




public class RagRunDto
{
    #region Constructors

    public RagRunDto()
    {
    }







    public RagRunDto(Guid id, Guid snapshotId, DateTime timestampUtc, string schemaVersion)
    {

        Id = id;
        SnapshotId = snapshotId;
        TimestampUtc = timestampUtc;
        SchemaVersion = schemaVersion;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid SnapshotId { get; set; }

    [NotNullValidator()] public DateTime TimestampUtc { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SchemaVersion { get; set; }

    #endregion
}




public class ReviewIssueDto
{
    #region Constructors

    public ReviewIssueDto()
    {
    }







    public ReviewIssueDto(Guid id, Guid reviewItemId, string code, string severity, string relatedMemberUid, string details)
    {

        Id = id;
        ReviewItemId = reviewItemId;
        Code = code;
        Severity = severity;
        RelatedMemberUid = relatedMemberUid;
        Details = details;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid ReviewItemId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Code { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string Severity { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string RelatedMemberUid { get; set; }

    public string Details { get; set; }

    #endregion
}




public class ReviewItemDto
{
    #region Constructors

    public ReviewItemDto()
    {
    }







    public ReviewItemDto(Guid id, Guid reviewRunId, string targetKind, string targetUid, string status, string summary)
    {

        Id = id;
        ReviewRunId = reviewRunId;
        TargetKind = targetKind;
        TargetUid = targetUid;
        Status = status;
        Summary = summary;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid ReviewRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string TargetKind { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string TargetUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string Status { get; set; }

    public string Summary { get; set; }

    #endregion
}




public class ReviewRunDto
{
    #region Constructors

    public ReviewRunDto()
    {
    }







    public ReviewRunDto(Guid id, Guid snapshotId, DateTime timestampUtc, string schemaVersion)
    {

        Id = id;
        SnapshotId = snapshotId;
        TimestampUtc = timestampUtc;
        SchemaVersion = schemaVersion;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid SnapshotId { get; set; }

    [NotNullValidator()] public DateTime TimestampUtc { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SchemaVersion { get; set; }

    #endregion
}




public class SampleDto
{
    #region Constructors

    public SampleDto()
    {
    }







    public SampleDto(Guid id, Guid sampleRunId, string sampleUid, string featureUid, string language, string code, string entryPoint, string targetFramework, string packageReferences, string derivedFromCodeUid, string tags)
    {

        Id = id;
        SampleRunId = sampleRunId;
        SampleUid = sampleUid;
        FeatureUid = featureUid;
        Language = language;
        Code = code;
        EntryPoint = entryPoint;
        TargetFramework = targetFramework;
        PackageReferences = packageReferences;
        DerivedFromCodeUid = derivedFromCodeUid;
        Tags = tags;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid SampleRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SampleUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string FeatureUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    public string Code { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string EntryPoint { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string TargetFramework { get; set; }

    public string PackageReferences { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string DerivedFromCodeUid { get; set; }

    public string Tags { get; set; }

    #endregion
}




public class SampleApiMemberLinkDto
{
    #region Constructors

    public SampleApiMemberLinkDto()
    {
    }







    public SampleApiMemberLinkDto(Guid id, Guid sampleId, string memberUid)
    {

        Id = id;
        SampleId = sampleId;
        MemberUid = memberUid;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid SampleId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string MemberUid { get; set; }

    #endregion
}




public class SampleRunDto
{
    #region Constructors

    public SampleRunDto()
    {
    }







    public SampleRunDto(Guid id, Guid snapshotId, DateTime timestampUtc, string schemaVersion)
    {

        Id = id;
        SnapshotId = snapshotId;
        TimestampUtc = timestampUtc;
        SchemaVersion = schemaVersion;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid SnapshotId { get; set; }

    [NotNullValidator()] public DateTime TimestampUtc { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SchemaVersion { get; set; }

    #endregion
}




public class SemanticIdentityDto
{
    #region Constructors

    public SemanticIdentityDto()
    {
    }







    public SemanticIdentityDto(string uid, byte[] uidHash, string kind, DateTime createdUtc, string notes)
    {

        Uid = uid;
        UidHash = uidHash;
        Kind = kind;
        CreatedUtc = createdUtc;
        Notes = notes;
    }

    #endregion

    #region Properties

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string Uid { get; set; }

    [NotNullValidator()] public byte[] UidHash { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string Kind { get; set; }

    [NotNullValidator()] public DateTime CreatedUtc { get; set; }

    public string Notes { get; set; }

    #endregion
}




public class SnapshotDiffDto
{
    #region Constructors

    public SnapshotDiffDto()
    {
    }







    public SnapshotDiffDto(Guid id, Guid oldSnapshotId, Guid newSnapshotId, DateTime timestampUtc, string schemaVersion)
    {

        Id = id;
        OldSnapshotId = oldSnapshotId;
        NewSnapshotId = newSnapshotId;
        TimestampUtc = timestampUtc;
        SchemaVersion = schemaVersion;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid OldSnapshotId { get; set; }

    [NotNullValidator()] public Guid NewSnapshotId { get; set; }

    [NotNullValidator()] public DateTime TimestampUtc { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SchemaVersion { get; set; }

    #endregion
}




public class SourceSnapshotDto
{
    #region Constructors

    public SourceSnapshotDto()
    {
    }







    public SourceSnapshotDto(Guid id, Guid ingestionRunId, string snapshotUid, string repoUrl, string branch, string repoCommit, string language, string packageName, string packageVersion, string configJson, byte[] snapshotUidHash)
    {

        Id = id;
        IngestionRunId = ingestionRunId;
        SnapshotUid = snapshotUid;
        RepoUrl = repoUrl;
        Branch = branch;
        RepoCommit = repoCommit;
        Language = language;
        PackageName = packageName;
        PackageVersion = packageVersion;
        ConfigJson = configJson;
        SnapshotUidHash = snapshotUidHash;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid IngestionRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SnapshotUid { get; set; }

    public string RepoUrl { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Branch { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string RepoCommit { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string PackageName { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string PackageVersion { get; set; }

    public string ConfigJson { get; set; }

    public byte[] SnapshotUidHash { get; set; }

    #endregion
}




public class TruthRunDto
{
    #region Constructors

    public TruthRunDto()
    {
    }







    public TruthRunDto(Guid id, Guid snapshotId, DateTime timestampUtc, string schemaVersion)
    {

        Id = id;
        SnapshotId = snapshotId;
        TimestampUtc = timestampUtc;
        SchemaVersion = schemaVersion;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public Guid SnapshotId { get; set; }

    [NotNullValidator()] public DateTime TimestampUtc { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SchemaVersion { get; set; }

    #endregion
}




public class VApiFeatureCurrentDto
{
    #region Constructors

    public VApiFeatureCurrentDto()
    {
    }







    public VApiFeatureCurrentDto(Guid id, string semanticUid, Guid truthRunId, string name, string language, string description, string tags, int versionNumber, Guid createdIngestionRunId, Guid updatedIngestionRunId, Guid? removedIngestionRunId, DateTime validFromUtc, DateTime? validToUtc, bool isActive, byte[] contentHash, byte[] semanticUidHash)
    {

        Id = id;
        SemanticUid = semanticUid;
        TruthRunId = truthRunId;
        Name = name;
        Language = language;
        Description = description;
        Tags = tags;
        VersionNumber = versionNumber;
        CreatedIngestionRunId = createdIngestionRunId;
        UpdatedIngestionRunId = updatedIngestionRunId;
        RemovedIngestionRunId = removedIngestionRunId;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        IsActive = isActive;
        ContentHash = contentHash;
        SemanticUidHash = semanticUidHash;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SemanticUid { get; set; }

    [NotNullValidator()] public Guid TruthRunId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Name { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    public string Description { get; set; }

    public string Tags { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator()] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator()] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }

    #endregion
}




public class VApiMemberCurrentDto
{
    #region Constructors

    public VApiMemberCurrentDto()
    {
    }







    public VApiMemberCurrentDto(Guid id, string semanticUid, Guid apiTypeId, string name, string kind, string methodKind, string accessibility, bool? isStatic, bool? isExtensionMethod, bool? isAsync, bool? isVirtual, bool? isOverride, bool? isAbstract, bool? isSealed, bool? isReadonly, bool? isConst, bool? isUnsafe, string returnTypeUid, string returnNullable, string genericParameters, string genericConstraints, string summary, string remarks, string attributes, string sourceFilePath, int? sourceStartLine, int? sourceEndLine, byte[] memberUidHash, int versionNumber, Guid createdIngestionRunId, Guid updatedIngestionRunId, Guid? removedIngestionRunId, DateTime validFromUtc, DateTime? validToUtc, bool isActive, byte[] contentHash, byte[] semanticUidHash)
    {

        Id = id;
        SemanticUid = semanticUid;
        ApiTypeId = apiTypeId;
        Name = name;
        Kind = kind;
        MethodKind = methodKind;
        Accessibility = accessibility;
        IsStatic = isStatic;
        IsExtensionMethod = isExtensionMethod;
        IsAsync = isAsync;
        IsVirtual = isVirtual;
        IsOverride = isOverride;
        IsAbstract = isAbstract;
        IsSealed = isSealed;
        IsReadonly = isReadonly;
        IsConst = isConst;
        IsUnsafe = isUnsafe;
        ReturnTypeUid = returnTypeUid;
        ReturnNullable = returnNullable;
        GenericParameters = genericParameters;
        GenericConstraints = genericConstraints;
        Summary = summary;
        Remarks = remarks;
        Attributes = attributes;
        SourceFilePath = sourceFilePath;
        SourceStartLine = sourceStartLine;
        SourceEndLine = sourceEndLine;
        MemberUidHash = memberUidHash;
        VersionNumber = versionNumber;
        CreatedIngestionRunId = createdIngestionRunId;
        UpdatedIngestionRunId = updatedIngestionRunId;
        RemovedIngestionRunId = removedIngestionRunId;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        IsActive = isActive;
        ContentHash = contentHash;
        SemanticUidHash = semanticUidHash;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SemanticUid { get; set; }

    [NotNullValidator()] public Guid ApiTypeId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Name { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Kind { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string MethodKind { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Accessibility { get; set; }

    public bool? IsStatic { get; set; }

    public bool? IsExtensionMethod { get; set; }

    public bool? IsAsync { get; set; }

    public bool? IsVirtual { get; set; }

    public bool? IsOverride { get; set; }

    public bool? IsAbstract { get; set; }

    public bool? IsSealed { get; set; }

    public bool? IsReadonly { get; set; }

    public bool? IsConst { get; set; }

    public bool? IsUnsafe { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string ReturnTypeUid { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string ReturnNullable { get; set; }

    public string GenericParameters { get; set; }

    public string GenericConstraints { get; set; }

    public string Summary { get; set; }

    public string Remarks { get; set; }

    public string Attributes { get; set; }

    public string SourceFilePath { get; set; }

    public int? SourceStartLine { get; set; }

    public int? SourceEndLine { get; set; }

    public byte[] MemberUidHash { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator()] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator()] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }

    #endregion
}




public class VApiTypeCurrentDto
{
    #region Constructors

    public VApiTypeCurrentDto()
    {
    }







    public VApiTypeCurrentDto(Guid id, string semanticUid, Guid sourceSnapshotId, string name, string namespacePath, string kind, string accessibility, bool? isStatic, bool? isGeneric, bool? isAbstract, bool? isSealed, bool? isRecord, bool? isRefLike, string baseTypeUid, string interfaces, string containingTypeUid, string genericParameters, string genericConstraints, string summary, string remarks, string attributes, string sourceFilePath, int? sourceStartLine, int? sourceEndLine, int versionNumber, Guid createdIngestionRunId, Guid updatedIngestionRunId, Guid? removedIngestionRunId, DateTime validFromUtc, DateTime? validToUtc, bool isActive, byte[] contentHash, byte[] semanticUidHash)
    {

        Id = id;
        SemanticUid = semanticUid;
        SourceSnapshotId = sourceSnapshotId;
        Name = name;
        NamespacePath = namespacePath;
        Kind = kind;
        Accessibility = accessibility;
        IsStatic = isStatic;
        IsGeneric = isGeneric;
        IsAbstract = isAbstract;
        IsSealed = isSealed;
        IsRecord = isRecord;
        IsRefLike = isRefLike;
        BaseTypeUid = baseTypeUid;
        Interfaces = interfaces;
        ContainingTypeUid = containingTypeUid;
        GenericParameters = genericParameters;
        GenericConstraints = genericConstraints;
        Summary = summary;
        Remarks = remarks;
        Attributes = attributes;
        SourceFilePath = sourceFilePath;
        SourceStartLine = sourceStartLine;
        SourceEndLine = sourceEndLine;
        VersionNumber = versionNumber;
        CreatedIngestionRunId = createdIngestionRunId;
        UpdatedIngestionRunId = updatedIngestionRunId;
        RemovedIngestionRunId = removedIngestionRunId;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        IsActive = isActive;
        ContentHash = contentHash;
        SemanticUidHash = semanticUidHash;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SemanticUid { get; set; }

    [NotNullValidator()] public Guid SourceSnapshotId { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Name { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string NamespacePath { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Kind { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Accessibility { get; set; }

    public bool? IsStatic { get; set; }

    public bool? IsGeneric { get; set; }

    public bool? IsAbstract { get; set; }

    public bool? IsSealed { get; set; }

    public bool? IsRecord { get; set; }

    public bool? IsRefLike { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string BaseTypeUid { get; set; }

    public string Interfaces { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string ContainingTypeUid { get; set; }

    public string GenericParameters { get; set; }

    public string GenericConstraints { get; set; }

    public string Summary { get; set; }

    public string Remarks { get; set; }

    public string Attributes { get; set; }

    public string SourceFilePath { get; set; }

    public int? SourceStartLine { get; set; }

    public int? SourceEndLine { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator()] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator()] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }

    #endregion
}




public class VDocPageCurrentDto
{
    #region Constructors

    public VDocPageCurrentDto()
    {
    }







    public VDocPageCurrentDto(Guid id, string semanticUid, Guid sourceSnapshotId, string sourcePath, string title, string language, string url, string rawMarkdown, int versionNumber, Guid createdIngestionRunId, Guid updatedIngestionRunId, Guid? removedIngestionRunId, DateTime validFromUtc, DateTime? validToUtc, bool isActive, byte[] contentHash, byte[] semanticUidHash)
    {

        Id = id;
        SemanticUid = semanticUid;
        SourceSnapshotId = sourceSnapshotId;
        SourcePath = sourcePath;
        Title = title;
        Language = language;
        Url = url;
        RawMarkdown = rawMarkdown;
        VersionNumber = versionNumber;
        CreatedIngestionRunId = createdIngestionRunId;
        UpdatedIngestionRunId = updatedIngestionRunId;
        RemovedIngestionRunId = removedIngestionRunId;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        IsActive = isActive;
        ContentHash = contentHash;
        SemanticUidHash = semanticUidHash;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public Guid Id { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SemanticUid { get; set; }

    [NotNullValidator()] public Guid SourceSnapshotId { get; set; }

    public string SourcePath { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Title { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Language { get; set; }

    public string Url { get; set; }

    public string RawMarkdown { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator()] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator()] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }

    #endregion
}




public class SpCheckTemporalConsistencyResultDto
{
    #region Constructors

    public SpCheckTemporalConsistencyResultDto()
    {
    }







    public SpCheckTemporalConsistencyResultDto(string tableName, string semanticUid)
    {

        TableName = tableName;
        SemanticUid = semanticUid;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public string TableName { get; set; }

    [NotNullValidator()] public string SemanticUid { get; set; }

    #endregion
}




public class SpCheckTemporalConsistencyResult1Dto
{
    #region Constructors

    public SpCheckTemporalConsistencyResult1Dto()
    {
    }







    public SpCheckTemporalConsistencyResult1Dto(string tableName, Guid id, string semanticUid)
    {

        TableName = tableName;
        Id = id;
        SemanticUid = semanticUid;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public string TableName { get; set; }

    [NotNullValidator()] public Guid Id { get; set; }

    [NotNullValidator()] public string SemanticUid { get; set; }

    #endregion
}




public class SpCheckTemporalConsistencyResult2Dto
{
    #region Constructors

    public SpCheckTemporalConsistencyResult2Dto()
    {
    }







    public SpCheckTemporalConsistencyResult2Dto(string tableName, string semanticUid, int versionNumber, DateTime validFromUtc, DateTime? validToUtc, DateTime? nextFrom)
    {

        TableName = tableName;
        SemanticUid = semanticUid;
        VersionNumber = versionNumber;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
        NextFrom = nextFrom;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public string TableName { get; set; }

    [NotNullValidator()] public string SemanticUid { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    public DateTime? NextFrom { get; set; }

    #endregion
}




public class SpGetChangesInIngestionRunResultDto
{
    #region Constructors

    public SpGetChangesInIngestionRunResultDto()
    {
    }







    public SpGetChangesInIngestionRunResultDto(string artifactKind, string semanticUid, int versionNumber, DateTime validFromUtc, DateTime? validToUtc)
    {

        ArtifactKind = artifactKind;
        SemanticUid = semanticUid;
        VersionNumber = versionNumber;
        ValidFromUtc = validFromUtc;
        ValidToUtc = validToUtc;
    }

    #endregion

    #region Properties

    [NotNullValidator()] public string ArtifactKind { get; set; }

    [NotNullValidator()] public string SemanticUid { get; set; }

    [NotNullValidator()] public int VersionNumber { get; set; }

    [NotNullValidator()] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    #endregion
}




public class SpUpsertApiParameterResultDto
{
    #region Properties

    public Guid? Id { get; set; }

    #endregion

    #region Constructors

    public SpUpsertApiParameterResultDto()
    {
    }







    public SpUpsertApiParameterResultDto(Guid? id)
    {

        Id = id;
    }

    #endregion
}




public class SpVerifyIngestionRunResultDto
{
    #region Constructors

    public SpVerifyIngestionRunResultDto()
    {
    }







    public SpVerifyIngestionRunResultDto(string category, string detail)
    {

        Category = category;
        Detail = detail;
    }

    #endregion

    #region Properties

    public string Category { get; set; }

    public string Detail { get; set; }

    #endregion
}