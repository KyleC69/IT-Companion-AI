// Project Name: SKAgent
// File Name: VApiMemberCurrent.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.KnowledgeBase;


public partial class VApiMemberCurrent
{
    public VApiMemberCurrent()
    {
        OnCreated();
    }







    [NotNullValidator] public Guid Id { get; set; }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator]
    public string SemanticUid { get; set; }

    [NotNullValidator] public Guid ApiTypeId { get; set; }

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

    [NotNullValidator] public int VersionNumber { get; set; }

    [NotNullValidator] public Guid CreatedIngestionRunId { get; set; }

    [NotNullValidator] public Guid UpdatedIngestionRunId { get; set; }

    public Guid? RemovedIngestionRunId { get; set; }

    [NotNullValidator] public DateTime ValidFromUtc { get; set; }

    public DateTime? ValidToUtc { get; set; }

    [NotNullValidator] public bool IsActive { get; set; }

    public byte[] ContentHash { get; set; }

    public byte[] SemanticUidHash { get; set; }

    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion
}