namespace ITCompanionAI.EFModels;


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





    public Guid SourceSnapshotId
    {
        get => _SourceSnapshotId;
        set
        {
            if (_SourceSnapshotId != value)
            {
                SendPropertyChanging("SourceSnapshotId");
                _SourceSnapshotId = value;
                SendPropertyChanged("SourceSnapshotId");
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





    public string NamespacePath
    {
        get => _NamespacePath;
        set
        {
            if (_NamespacePath != value)
            {
                SendPropertyChanging("NamespacePath");
                _NamespacePath = value;
                SendPropertyChanged("NamespacePath");
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





    public bool? IsGeneric
    {
        get => _IsGeneric;
        set
        {
            if (_IsGeneric != value)
            {
                SendPropertyChanging("IsGeneric");
                _IsGeneric = value;
                SendPropertyChanged("IsGeneric");
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





    public bool? IsRecord
    {
        get => _IsRecord;
        set
        {
            if (_IsRecord != value)
            {
                SendPropertyChanging("IsRecord");
                _IsRecord = value;
                SendPropertyChanged("IsRecord");
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
                SendPropertyChanging("IsRefLike");
                _IsRefLike = value;
                SendPropertyChanged("IsRefLike");
            }
        }
    }





    public string BaseTypeUid
    {
        get => _BaseTypeUid;
        set
        {
            if (_BaseTypeUid != value)
            {
                SendPropertyChanging("BaseTypeUid");
                _BaseTypeUid = value;
                SendPropertyChanged("BaseTypeUid");
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
                SendPropertyChanging("Interfaces");
                _Interfaces = value;
                SendPropertyChanged("Interfaces");
            }
        }
    }





    public string ContainingTypeUid
    {
        get => _ContainingTypeUid;
        set
        {
            if (_ContainingTypeUid != value)
            {
                SendPropertyChanging("ContainingTypeUid");
                _ContainingTypeUid = value;
                SendPropertyChanged("ContainingTypeUid");
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





    public Guid UpdatedIngestionRunId
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
        PropertyChangingEventHandler handler = PropertyChanging;
        if (handler != null)
        {
            handler(this, emptyChangingEventArgs);
        }
    }








    protected virtual void SendPropertyChanging(string propertyName)
    {
        PropertyChangingEventHandler handler = PropertyChanging;
        if (handler != null)
        {
            handler(this, new PropertyChangingEventArgs(propertyName));
        }
    }








    protected virtual void SendPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}