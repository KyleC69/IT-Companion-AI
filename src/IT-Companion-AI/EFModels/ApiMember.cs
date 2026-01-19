namespace ITCompanionAI.EFModels;


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








    public Guid Id
    {
        get => _Id;
        set
        {
            if (_Id != value)
            {
                SendPropertyChanging("Id");
                _Id = value;
                SendPropertyChanged("Id");
            }
        }
    }





    public string SemanticUid
    {
        get => _SemanticUid;
        set
        {
            if (_SemanticUid != value)
            {
                SendPropertyChanging("SemanticUid");
                _SemanticUid = value;
                SendPropertyChanged("SemanticUid");
            }
        }
    }





    public Guid ApiFeatureId
    {
        get => _ApiFeatureId;
        set
        {
            if (_ApiFeatureId != value)
            {
                SendPropertyChanging("ApiFeatureId");
                _ApiFeatureId = value;
                SendPropertyChanged("ApiFeatureId");
            }
        }
    }





    public string Name
    {
        get => _Name;
        set
        {
            if (_Name != value)
            {
                SendPropertyChanging("Name");
                _Name = value;
                SendPropertyChanged("Name");
            }
        }
    }





    public string Kind
    {
        get => _Kind;
        set
        {
            if (_Kind != value)
            {
                SendPropertyChanging("Kind");
                _Kind = value;
                SendPropertyChanged("Kind");
            }
        }
    }





    public string MethodKind
    {
        get => _MethodKind;
        set
        {
            if (_MethodKind != value)
            {
                SendPropertyChanging("MethodKind");
                _MethodKind = value;
                SendPropertyChanged("MethodKind");
            }
        }
    }





    public string Accessibility
    {
        get => _Accessibility;
        set
        {
            if (_Accessibility != value)
            {
                SendPropertyChanging("Accessibility");
                _Accessibility = value;
                SendPropertyChanged("Accessibility");
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
                SendPropertyChanging("IsStatic");
                _IsStatic = value;
                SendPropertyChanged("IsStatic");
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
                SendPropertyChanging("IsExtensionMethod");
                _IsExtensionMethod = value;
                SendPropertyChanged("IsExtensionMethod");
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
                SendPropertyChanging("IsAsync");
                _IsAsync = value;
                SendPropertyChanged("IsAsync");
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
                SendPropertyChanging("IsVirtual");
                _IsVirtual = value;
                SendPropertyChanged("IsVirtual");
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
                SendPropertyChanging("IsOverride");
                _IsOverride = value;
                SendPropertyChanged("IsOverride");
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
                SendPropertyChanging("IsAbstract");
                _IsAbstract = value;
                SendPropertyChanged("IsAbstract");
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
                SendPropertyChanging("IsSealed");
                _IsSealed = value;
                SendPropertyChanged("IsSealed");
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
                SendPropertyChanging("IsReadonly");
                _IsReadonly = value;
                SendPropertyChanged("IsReadonly");
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
                SendPropertyChanging("IsConst");
                _IsConst = value;
                SendPropertyChanged("IsConst");
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
                SendPropertyChanging("IsUnsafe");
                _IsUnsafe = value;
                SendPropertyChanged("IsUnsafe");
            }
        }
    }





    public string ReturnTypeUid
    {
        get => _ReturnTypeUid;
        set
        {
            if (_ReturnTypeUid != value)
            {
                SendPropertyChanging("ReturnTypeUid");
                _ReturnTypeUid = value;
                SendPropertyChanged("ReturnTypeUid");
            }
        }
    }





    public string ReturnNullable
    {
        get => _ReturnNullable;
        set
        {
            if (_ReturnNullable != value)
            {
                SendPropertyChanging("ReturnNullable");
                _ReturnNullable = value;
                SendPropertyChanged("ReturnNullable");
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
                SendPropertyChanging("GenericParameters");
                _GenericParameters = value;
                SendPropertyChanged("GenericParameters");
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
                SendPropertyChanging("GenericConstraints");
                _GenericConstraints = value;
                SendPropertyChanged("GenericConstraints");
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
                SendPropertyChanging("Summary");
                _Summary = value;
                SendPropertyChanged("Summary");
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
                SendPropertyChanging("Remarks");
                _Remarks = value;
                SendPropertyChanged("Remarks");
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
                SendPropertyChanging("Attributes");
                _Attributes = value;
                SendPropertyChanged("Attributes");
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
                SendPropertyChanging("SourceFilePath");
                _SourceFilePath = value;
                SendPropertyChanged("SourceFilePath");
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
                SendPropertyChanging("SourceStartLine");
                _SourceStartLine = value;
                SendPropertyChanged("SourceStartLine");
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
                SendPropertyChanging("SourceEndLine");
                _SourceEndLine = value;
                SendPropertyChanged("SourceEndLine");
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
                SendPropertyChanging("MemberUidHash");
                _MemberUidHash = value;
                SendPropertyChanged("MemberUidHash");
            }
        }
    }





    public int VersionNumber
    {
        get => _VersionNumber;
        set
        {
            if (_VersionNumber != value)
            {
                SendPropertyChanging("VersionNumber");
                _VersionNumber = value;
                SendPropertyChanged("VersionNumber");
            }
        }
    }





    public Guid CreatedIngestionRunId
    {
        get => _CreatedIngestionRunId;
        set
        {
            if (_CreatedIngestionRunId != value)
            {
                SendPropertyChanging("CreatedIngestionRunId");
                _CreatedIngestionRunId = value;
                SendPropertyChanged("CreatedIngestionRunId");
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
                SendPropertyChanging("UpdatedIngestionRunId");
                _UpdatedIngestionRunId = value;
                SendPropertyChanged("UpdatedIngestionRunId");
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
                SendPropertyChanging("RemovedIngestionRunId");
                _RemovedIngestionRunId = value;
                SendPropertyChanged("RemovedIngestionRunId");
            }
        }
    }





    public DateTime ValidFromUtc
    {
        get => _ValidFromUtc;
        set
        {
            if (_ValidFromUtc != value)
            {
                SendPropertyChanging("ValidFromUtc");
                _ValidFromUtc = value;
                SendPropertyChanged("ValidFromUtc");
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
                SendPropertyChanging("ValidToUtc");
                _ValidToUtc = value;
                SendPropertyChanged("ValidToUtc");
            }
        }
    }





    public bool IsActive
    {
        get => _IsActive;
        set
        {
            if (_IsActive != value)
            {
                SendPropertyChanging("IsActive");
                _IsActive = value;
                SendPropertyChanged("IsActive");
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
                SendPropertyChanging("ContentHash");
                _ContentHash = value;
                SendPropertyChanged("ContentHash");
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
                SendPropertyChanging("SemanticUidHash");
                _SemanticUidHash = value;
                SendPropertyChanged("SemanticUidHash");
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
                SendPropertyChanging("ApiFeature");
                _ApiFeature = value;
                SendPropertyChanged("ApiFeature");
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
                SendPropertyChanging("IngestionRun");
                _IngestionRun = value;
                SendPropertyChanged("IngestionRun");
            }
        }
    }





    public virtual event PropertyChangedEventHandler PropertyChanged;

    public virtual event PropertyChangingEventHandler PropertyChanging;



    #region Extensibility Method Definitions

    partial void OnCreated();

    #endregion








    protected virtual void SendPropertyChanging()
    {
        PropertyChanging?.Invoke(this, emptyChangingEventArgs);
    }








    protected virtual void SendPropertyChanging(string propertyName)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }








    protected virtual void SendPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}