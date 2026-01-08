// Project Name: SKAgent
// File Name: KBCurator.ApiMember.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public partial class ApiMember : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Accessibility;

    private ApiFeature _ApiFeature;

    private Guid _ApiFeatureId;

    private string _Attributes;

    private byte[] _ContentHash;

    private Guid _CreatedIngestionRunId;

    private string _GenericConstraints;

    private string _GenericParameters;

    private Guid _Id;

    private IngestionRun _IngestionRun;

    private bool? _IsAbstract;

    private bool _IsActive;

    private bool? _IsAsync;

    private bool? _IsConst;

    private bool? _IsExtensionMethod;

    private bool? _IsOverride;

    private bool? _IsReadonly;

    private bool? _IsSealed;

    private bool? _IsStatic;

    private bool? _IsUnsafe;

    private bool? _IsVirtual;

    private string _Kind;

    private byte[] _MemberUidHash;

    private string _MethodKind;

    private string _Name;

    private string _Remarks;

    private Guid? _RemovedIngestionRunId;

    private string _ReturnNullable;

    private string _ReturnTypeUid;

    private string _SemanticUid;

    private byte[] _SemanticUidHash;

    private int? _SourceEndLine;

    private string _SourceFilePath;

    private int? _SourceStartLine;

    private string _Summary;

    private Guid? _UpdatedIngestionRunId;

    private DateTime _ValidFromUtc;

    private DateTime? _ValidToUtc;

    private int _VersionNumber;







    public ApiMember()
    {
        _IsActive = true;
        OnCreated();
    }







    [NotNullValidator()]
    public Guid Id
    {
        get => _Id;
        set
        {
            if (_Id != value)
            {
                OnIdChanging(value);
                SendPropertyChanging("Id");
                _Id = value;
                SendPropertyChanged("Id");
                OnIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SemanticUid
    {
        get => _SemanticUid;
        set
        {
            if (_SemanticUid != value)
            {
                OnSemanticUidChanging(value);
                SendPropertyChanging("SemanticUid");
                _SemanticUid = value;
                SendPropertyChanged("SemanticUid");
                OnSemanticUidChanged();
            }
        }
    }

    [NotNullValidator()]
    public Guid ApiFeatureId
    {
        get => _ApiFeatureId;
        set
        {
            if (_ApiFeatureId != value)
            {
                OnApiFeatureIdChanging(value);
                SendPropertyChanging("ApiFeatureId");
                _ApiFeatureId = value;
                SendPropertyChanged("ApiFeatureId");
                OnApiFeatureIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 400, RangeBoundaryType.Inclusive)]
    public string Name
    {
        get => _Name;
        set
        {
            if (_Name != value)
            {
                OnNameChanging(value);
                SendPropertyChanging("Name");
                _Name = value;
                SendPropertyChanged("Name");
                OnNameChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Kind
    {
        get => _Kind;
        set
        {
            if (_Kind != value)
            {
                OnKindChanging(value);
                SendPropertyChanging("Kind");
                _Kind = value;
                SendPropertyChanged("Kind");
                OnKindChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string MethodKind
    {
        get => _MethodKind;
        set
        {
            if (_MethodKind != value)
            {
                OnMethodKindChanging(value);
                SendPropertyChanging("MethodKind");
                _MethodKind = value;
                SendPropertyChanged("MethodKind");
                OnMethodKindChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    public string Accessibility
    {
        get => _Accessibility;
        set
        {
            if (_Accessibility != value)
            {
                OnAccessibilityChanging(value);
                SendPropertyChanging("Accessibility");
                _Accessibility = value;
                SendPropertyChanged("Accessibility");
                OnAccessibilityChanged();
            }
        }
    }

    public bool? IsStatic
    {
        get => _IsStatic;
        set
        {
            if (_IsStatic != value)
            {
                OnIsStaticChanging(value);
                SendPropertyChanging("IsStatic");
                _IsStatic = value;
                SendPropertyChanged("IsStatic");
                OnIsStaticChanged();
            }
        }
    }

    public bool? IsExtensionMethod
    {
        get => _IsExtensionMethod;
        set
        {
            if (_IsExtensionMethod != value)
            {
                OnIsExtensionMethodChanging(value);
                SendPropertyChanging("IsExtensionMethod");
                _IsExtensionMethod = value;
                SendPropertyChanged("IsExtensionMethod");
                OnIsExtensionMethodChanged();
            }
        }
    }

    public bool? IsAsync
    {
        get => _IsAsync;
        set
        {
            if (_IsAsync != value)
            {
                OnIsAsyncChanging(value);
                SendPropertyChanging("IsAsync");
                _IsAsync = value;
                SendPropertyChanged("IsAsync");
                OnIsAsyncChanged();
            }
        }
    }

    public bool? IsVirtual
    {
        get => _IsVirtual;
        set
        {
            if (_IsVirtual != value)
            {
                OnIsVirtualChanging(value);
                SendPropertyChanging("IsVirtual");
                _IsVirtual = value;
                SendPropertyChanged("IsVirtual");
                OnIsVirtualChanged();
            }
        }
    }

    public bool? IsOverride
    {
        get => _IsOverride;
        set
        {
            if (_IsOverride != value)
            {
                OnIsOverrideChanging(value);
                SendPropertyChanging("IsOverride");
                _IsOverride = value;
                SendPropertyChanged("IsOverride");
                OnIsOverrideChanged();
            }
        }
    }

    public bool? IsAbstract
    {
        get => _IsAbstract;
        set
        {
            if (_IsAbstract != value)
            {
                OnIsAbstractChanging(value);
                SendPropertyChanging("IsAbstract");
                _IsAbstract = value;
                SendPropertyChanged("IsAbstract");
                OnIsAbstractChanged();
            }
        }
    }

    public bool? IsSealed
    {
        get => _IsSealed;
        set
        {
            if (_IsSealed != value)
            {
                OnIsSealedChanging(value);
                SendPropertyChanging("IsSealed");
                _IsSealed = value;
                SendPropertyChanged("IsSealed");
                OnIsSealedChanged();
            }
        }
    }

    public bool? IsReadonly
    {
        get => _IsReadonly;
        set
        {
            if (_IsReadonly != value)
            {
                OnIsReadonlyChanging(value);
                SendPropertyChanging("IsReadonly");
                _IsReadonly = value;
                SendPropertyChanged("IsReadonly");
                OnIsReadonlyChanged();
            }
        }
    }

    public bool? IsConst
    {
        get => _IsConst;
        set
        {
            if (_IsConst != value)
            {
                OnIsConstChanging(value);
                SendPropertyChanging("IsConst");
                _IsConst = value;
                SendPropertyChanged("IsConst");
                OnIsConstChanged();
            }
        }
    }

    public bool? IsUnsafe
    {
        get => _IsUnsafe;
        set
        {
            if (_IsUnsafe != value)
            {
                OnIsUnsafeChanging(value);
                SendPropertyChanging("IsUnsafe");
                _IsUnsafe = value;
                SendPropertyChanged("IsUnsafe");
                OnIsUnsafeChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string ReturnTypeUid
    {
        get => _ReturnTypeUid;
        set
        {
            if (_ReturnTypeUid != value)
            {
                OnReturnTypeUidChanging(value);
                SendPropertyChanging("ReturnTypeUid");
                _ReturnTypeUid = value;
                SendPropertyChanged("ReturnTypeUid");
                OnReturnTypeUidChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    public string ReturnNullable
    {
        get => _ReturnNullable;
        set
        {
            if (_ReturnNullable != value)
            {
                OnReturnNullableChanging(value);
                SendPropertyChanging("ReturnNullable");
                _ReturnNullable = value;
                SendPropertyChanged("ReturnNullable");
                OnReturnNullableChanged();
            }
        }
    }

    public string GenericParameters
    {
        get => _GenericParameters;
        set
        {
            if (_GenericParameters != value)
            {
                OnGenericParametersChanging(value);
                SendPropertyChanging("GenericParameters");
                _GenericParameters = value;
                SendPropertyChanged("GenericParameters");
                OnGenericParametersChanged();
            }
        }
    }

    public string GenericConstraints
    {
        get => _GenericConstraints;
        set
        {
            if (_GenericConstraints != value)
            {
                OnGenericConstraintsChanging(value);
                SendPropertyChanging("GenericConstraints");
                _GenericConstraints = value;
                SendPropertyChanged("GenericConstraints");
                OnGenericConstraintsChanged();
            }
        }
    }

    public string Summary
    {
        get => _Summary;
        set
        {
            if (_Summary != value)
            {
                OnSummaryChanging(value);
                SendPropertyChanging("Summary");
                _Summary = value;
                SendPropertyChanged("Summary");
                OnSummaryChanged();
            }
        }
    }

    public string Remarks
    {
        get => _Remarks;
        set
        {
            if (_Remarks != value)
            {
                OnRemarksChanging(value);
                SendPropertyChanging("Remarks");
                _Remarks = value;
                SendPropertyChanged("Remarks");
                OnRemarksChanged();
            }
        }
    }

    public string Attributes
    {
        get => _Attributes;
        set
        {
            if (_Attributes != value)
            {
                OnAttributesChanging(value);
                SendPropertyChanging("Attributes");
                _Attributes = value;
                SendPropertyChanged("Attributes");
                OnAttributesChanged();
            }
        }
    }

    public string SourceFilePath
    {
        get => _SourceFilePath;
        set
        {
            if (_SourceFilePath != value)
            {
                OnSourceFilePathChanging(value);
                SendPropertyChanging("SourceFilePath");
                _SourceFilePath = value;
                SendPropertyChanged("SourceFilePath");
                OnSourceFilePathChanged();
            }
        }
    }

    public int? SourceStartLine
    {
        get => _SourceStartLine;
        set
        {
            if (_SourceStartLine != value)
            {
                OnSourceStartLineChanging(value);
                SendPropertyChanging("SourceStartLine");
                _SourceStartLine = value;
                SendPropertyChanged("SourceStartLine");
                OnSourceStartLineChanged();
            }
        }
    }

    public int? SourceEndLine
    {
        get => _SourceEndLine;
        set
        {
            if (_SourceEndLine != value)
            {
                OnSourceEndLineChanging(value);
                SendPropertyChanging("SourceEndLine");
                _SourceEndLine = value;
                SendPropertyChanged("SourceEndLine");
                OnSourceEndLineChanged();
            }
        }
    }

    public byte[] MemberUidHash
    {
        get => _MemberUidHash;
        set
        {
            if (_MemberUidHash != value)
            {
                OnMemberUidHashChanging(value);
                SendPropertyChanging("MemberUidHash");
                _MemberUidHash = value;
                SendPropertyChanged("MemberUidHash");
                OnMemberUidHashChanged();
            }
        }
    }

    [NotNullValidator()]
    public int VersionNumber
    {
        get => _VersionNumber;
        set
        {
            if (_VersionNumber != value)
            {
                OnVersionNumberChanging(value);
                SendPropertyChanging("VersionNumber");
                _VersionNumber = value;
                SendPropertyChanged("VersionNumber");
                OnVersionNumberChanged();
            }
        }
    }

    [NotNullValidator()]
    public Guid CreatedIngestionRunId
    {
        get => _CreatedIngestionRunId;
        set
        {
            if (_CreatedIngestionRunId != value)
            {
                OnCreatedIngestionRunIdChanging(value);
                SendPropertyChanging("CreatedIngestionRunId");
                _CreatedIngestionRunId = value;
                SendPropertyChanged("CreatedIngestionRunId");
                OnCreatedIngestionRunIdChanged();
            }
        }
    }

    public Guid? UpdatedIngestionRunId
    {
        get => _UpdatedIngestionRunId;
        set
        {
            if (_UpdatedIngestionRunId != value)
            {
                OnUpdatedIngestionRunIdChanging(value);
                SendPropertyChanging("UpdatedIngestionRunId");
                _UpdatedIngestionRunId = value;
                SendPropertyChanged("UpdatedIngestionRunId");
                OnUpdatedIngestionRunIdChanged();
            }
        }
    }

    public Guid? RemovedIngestionRunId
    {
        get => _RemovedIngestionRunId;
        set
        {
            if (_RemovedIngestionRunId != value)
            {
                OnRemovedIngestionRunIdChanging(value);
                SendPropertyChanging("RemovedIngestionRunId");
                _RemovedIngestionRunId = value;
                SendPropertyChanged("RemovedIngestionRunId");
                OnRemovedIngestionRunIdChanged();
            }
        }
    }

    [NotNullValidator()]
    public DateTime ValidFromUtc
    {
        get => _ValidFromUtc;
        set
        {
            if (_ValidFromUtc != value)
            {
                OnValidFromUtcChanging(value);
                SendPropertyChanging("ValidFromUtc");
                _ValidFromUtc = value;
                SendPropertyChanged("ValidFromUtc");
                OnValidFromUtcChanged();
            }
        }
    }

    public DateTime? ValidToUtc
    {
        get => _ValidToUtc;
        set
        {
            if (_ValidToUtc != value)
            {
                OnValidToUtcChanging(value);
                SendPropertyChanging("ValidToUtc");
                _ValidToUtc = value;
                SendPropertyChanged("ValidToUtc");
                OnValidToUtcChanged();
            }
        }
    }

    [NotNullValidator()]
    public bool IsActive
    {
        get => _IsActive;
        set
        {
            if (_IsActive != value)
            {
                OnIsActiveChanging(value);
                SendPropertyChanging("IsActive");
                _IsActive = value;
                SendPropertyChanged("IsActive");
                OnIsActiveChanged();
            }
        }
    }

    public byte[] ContentHash
    {
        get => _ContentHash;
        set
        {
            if (_ContentHash != value)
            {
                OnContentHashChanging(value);
                SendPropertyChanging("ContentHash");
                _ContentHash = value;
                SendPropertyChanged("ContentHash");
                OnContentHashChanged();
            }
        }
    }

    public byte[] SemanticUidHash
    {
        get => _SemanticUidHash;
        set
        {
            if (_SemanticUidHash != value)
            {
                OnSemanticUidHashChanging(value);
                SendPropertyChanging("SemanticUidHash");
                _SemanticUidHash = value;
                SendPropertyChanged("SemanticUidHash");
                OnSemanticUidHashChanged();
            }
        }
    }


    public virtual IngestionRun IngestionRun
    {
        get => _IngestionRun;
        set
        {
            if (_IngestionRun != value)
            {
                OnIngestionRunChanging(value);
                SendPropertyChanging("IngestionRun");
                _IngestionRun = value;
                SendPropertyChanged("IngestionRun");
                OnIngestionRunChanged();
            }
        }
    }


    public virtual ApiFeature ApiFeature
    {
        get => _ApiFeature;
        set
        {
            if (_ApiFeature != value)
            {
                OnApiFeatureChanging(value);
                SendPropertyChanging("ApiFeature");
                _ApiFeature = value;
                SendPropertyChanged("ApiFeature");
                OnApiFeatureChanged();
            }
        }
    }

    public virtual event PropertyChangedEventHandler PropertyChanged;

    public virtual event PropertyChangingEventHandler PropertyChanging;







    protected virtual void SendPropertyChanging()
    {
        PropertyChangingEventHandler? handler = this.PropertyChanging;
        if (handler != null)
        {
            handler(this, emptyChangingEventArgs);
        }
    }







    protected virtual void SendPropertyChanging(string propertyName)
    {
        PropertyChangingEventHandler? handler = this.PropertyChanging;
        if (handler != null)
        {
            handler(this, new PropertyChangingEventArgs(propertyName));
        }
    }







    protected virtual void SendPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler? handler = this.PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }







    #region Extensibility Method Definitions

    partial void OnCreated();
    partial void OnIdChanging(Guid value);

    partial void OnIdChanged();
    partial void OnSemanticUidChanging(string value);

    partial void OnSemanticUidChanged();
    partial void OnApiFeatureIdChanging(Guid value);

    partial void OnApiFeatureIdChanged();
    partial void OnNameChanging(string value);

    partial void OnNameChanged();
    partial void OnKindChanging(string value);

    partial void OnKindChanged();
    partial void OnMethodKindChanging(string value);

    partial void OnMethodKindChanged();
    partial void OnAccessibilityChanging(string value);

    partial void OnAccessibilityChanged();
    partial void OnIsStaticChanging(bool? value);

    partial void OnIsStaticChanged();
    partial void OnIsExtensionMethodChanging(bool? value);

    partial void OnIsExtensionMethodChanged();
    partial void OnIsAsyncChanging(bool? value);

    partial void OnIsAsyncChanged();
    partial void OnIsVirtualChanging(bool? value);

    partial void OnIsVirtualChanged();
    partial void OnIsOverrideChanging(bool? value);

    partial void OnIsOverrideChanged();
    partial void OnIsAbstractChanging(bool? value);

    partial void OnIsAbstractChanged();
    partial void OnIsSealedChanging(bool? value);

    partial void OnIsSealedChanged();
    partial void OnIsReadonlyChanging(bool? value);

    partial void OnIsReadonlyChanged();
    partial void OnIsConstChanging(bool? value);

    partial void OnIsConstChanged();
    partial void OnIsUnsafeChanging(bool? value);

    partial void OnIsUnsafeChanged();
    partial void OnReturnTypeUidChanging(string value);

    partial void OnReturnTypeUidChanged();
    partial void OnReturnNullableChanging(string value);

    partial void OnReturnNullableChanged();
    partial void OnGenericParametersChanging(string value);

    partial void OnGenericParametersChanged();
    partial void OnGenericConstraintsChanging(string value);

    partial void OnGenericConstraintsChanged();
    partial void OnSummaryChanging(string value);

    partial void OnSummaryChanged();
    partial void OnRemarksChanging(string value);

    partial void OnRemarksChanged();
    partial void OnAttributesChanging(string value);

    partial void OnAttributesChanged();
    partial void OnSourceFilePathChanging(string value);

    partial void OnSourceFilePathChanged();
    partial void OnSourceStartLineChanging(int? value);

    partial void OnSourceStartLineChanged();
    partial void OnSourceEndLineChanging(int? value);

    partial void OnSourceEndLineChanged();
    partial void OnMemberUidHashChanging(byte[] value);

    partial void OnMemberUidHashChanged();
    partial void OnVersionNumberChanging(int value);

    partial void OnVersionNumberChanged();
    partial void OnCreatedIngestionRunIdChanging(Guid value);

    partial void OnCreatedIngestionRunIdChanged();
    partial void OnUpdatedIngestionRunIdChanging(Guid? value);

    partial void OnUpdatedIngestionRunIdChanged();
    partial void OnRemovedIngestionRunIdChanging(Guid? value);

    partial void OnRemovedIngestionRunIdChanged();
    partial void OnValidFromUtcChanging(DateTime value);

    partial void OnValidFromUtcChanged();
    partial void OnValidToUtcChanging(DateTime? value);

    partial void OnValidToUtcChanged();
    partial void OnIsActiveChanging(bool value);

    partial void OnIsActiveChanged();
    partial void OnContentHashChanging(byte[] value);

    partial void OnContentHashChanged();
    partial void OnSemanticUidHashChanging(byte[] value);

    partial void OnSemanticUidHashChanged();
    partial void OnIngestionRunChanging(IngestionRun value);

    partial void OnIngestionRunChanged();
    partial void OnApiFeatureChanging(ApiFeature value);

    partial void OnApiFeatureChanged();

    #endregion
}