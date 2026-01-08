// Project Name: SKAgent
// File Name: KBCurator.ApiType.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public partial class ApiType : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Accessibility;

    private string _Attributes;

    private string _BaseTypeUid;

    private string _ContainingTypeUid;

    private byte[] _ContentHash;

    private Guid _CreatedIngestionRunId;

    private string _GenericConstraints;

    private string _GenericParameters;

    private Guid _Id;

    private IngestionRun _IngestionRun;

    private string _Interfaces;

    private bool? _IsAbstract;

    private bool _IsActive;

    private bool? _IsGeneric;

    private bool? _IsRecord;

    private bool? _IsRefLike;

    private bool? _IsSealed;

    private bool? _IsStatic;

    private string _Kind;

    private string _Name;

    private string _NamespacePath;

    private string _Remarks;

    private Guid? _RemovedIngestionRunId;

    private string _SemanticUid;

    private byte[] _SemanticUidHash;

    private int? _SourceEndLine;

    private string _SourceFilePath;

    private Guid _SourceSnapshotId;

    private int? _SourceStartLine;

    private string _Summary;

    private Guid _UpdatedIngestionRunId;

    private DateTime _ValidFromUtc;

    private DateTime? _ValidToUtc;

    private int _VersionNumber;







    public ApiType()
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
    public Guid SourceSnapshotId
    {
        get => _SourceSnapshotId;
        set
        {
            if (_SourceSnapshotId != value)
            {
                OnSourceSnapshotIdChanging(value);
                SendPropertyChanging("SourceSnapshotId");
                _SourceSnapshotId = value;
                SendPropertyChanged("SourceSnapshotId");
                OnSourceSnapshotIdChanged();
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

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string NamespacePath
    {
        get => _NamespacePath;
        set
        {
            if (_NamespacePath != value)
            {
                OnNamespacePathChanging(value);
                SendPropertyChanging("NamespacePath");
                _NamespacePath = value;
                SendPropertyChanged("NamespacePath");
                OnNamespacePathChanged();
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

    public bool? IsGeneric
    {
        get => _IsGeneric;
        set
        {
            if (_IsGeneric != value)
            {
                OnIsGenericChanging(value);
                SendPropertyChanging("IsGeneric");
                _IsGeneric = value;
                SendPropertyChanged("IsGeneric");
                OnIsGenericChanged();
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

    public bool? IsRecord
    {
        get => _IsRecord;
        set
        {
            if (_IsRecord != value)
            {
                OnIsRecordChanging(value);
                SendPropertyChanging("IsRecord");
                _IsRecord = value;
                SendPropertyChanged("IsRecord");
                OnIsRecordChanged();
            }
        }
    }

    public bool? IsRefLike
    {
        get => _IsRefLike;
        set
        {
            if (_IsRefLike != value)
            {
                OnIsRefLikeChanging(value);
                SendPropertyChanging("IsRefLike");
                _IsRefLike = value;
                SendPropertyChanged("IsRefLike");
                OnIsRefLikeChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string BaseTypeUid
    {
        get => _BaseTypeUid;
        set
        {
            if (_BaseTypeUid != value)
            {
                OnBaseTypeUidChanging(value);
                SendPropertyChanging("BaseTypeUid");
                _BaseTypeUid = value;
                SendPropertyChanged("BaseTypeUid");
                OnBaseTypeUidChanged();
            }
        }
    }

    public string Interfaces
    {
        get => _Interfaces;
        set
        {
            if (_Interfaces != value)
            {
                OnInterfacesChanging(value);
                SendPropertyChanging("Interfaces");
                _Interfaces = value;
                SendPropertyChanged("Interfaces");
                OnInterfacesChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    public string ContainingTypeUid
    {
        get => _ContainingTypeUid;
        set
        {
            if (_ContainingTypeUid != value)
            {
                OnContainingTypeUidChanging(value);
                SendPropertyChanging("ContainingTypeUid");
                _ContainingTypeUid = value;
                SendPropertyChanged("ContainingTypeUid");
                OnContainingTypeUidChanged();
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

    [NotNullValidator()]
    public Guid UpdatedIngestionRunId
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
    partial void OnSourceSnapshotIdChanging(Guid value);

    partial void OnSourceSnapshotIdChanged();
    partial void OnNameChanging(string value);

    partial void OnNameChanged();
    partial void OnNamespacePathChanging(string value);

    partial void OnNamespacePathChanged();
    partial void OnKindChanging(string value);

    partial void OnKindChanged();
    partial void OnAccessibilityChanging(string value);

    partial void OnAccessibilityChanged();
    partial void OnIsStaticChanging(bool? value);

    partial void OnIsStaticChanged();
    partial void OnIsGenericChanging(bool? value);

    partial void OnIsGenericChanged();
    partial void OnIsAbstractChanging(bool? value);

    partial void OnIsAbstractChanged();
    partial void OnIsSealedChanging(bool? value);

    partial void OnIsSealedChanged();
    partial void OnIsRecordChanging(bool? value);

    partial void OnIsRecordChanged();
    partial void OnIsRefLikeChanging(bool? value);

    partial void OnIsRefLikeChanged();
    partial void OnBaseTypeUidChanging(string value);

    partial void OnBaseTypeUidChanged();
    partial void OnInterfacesChanging(string value);

    partial void OnInterfacesChanged();
    partial void OnContainingTypeUidChanging(string value);

    partial void OnContainingTypeUidChanged();
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
    partial void OnVersionNumberChanging(int value);

    partial void OnVersionNumberChanged();
    partial void OnCreatedIngestionRunIdChanging(Guid value);

    partial void OnCreatedIngestionRunIdChanged();
    partial void OnUpdatedIngestionRunIdChanging(Guid value);

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

    #endregion
}