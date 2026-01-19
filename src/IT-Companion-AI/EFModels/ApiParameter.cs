namespace ITCompanionAI.EFModels;


public partial class ApiParameter : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private Guid _ApiMemberId;

    private byte[] _ContentHash;

    private Guid _CreatedIngestionRunId;

    private string _DefaultValueLiteral;

    private bool? _HasDefaultValue;

    private Guid _Id;

    private IngestionRun _IngestionRun;

    private bool _IsActive;

    private string _Modifier;

    private string _Name;

    private string _NullableAnnotation;

    private int? _Position;

    private Guid? _RemovedIngestionRunId;

    private byte[] _SemanticUidHash;

    private string _TypeUid;

    private Guid _UpdatedIngestionRunId;

    private DateTime _ValidFromUtc;

    private DateTime? _ValidToUtc;

    private int _VersionNumber;








    public ApiParameter()
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





    public Guid ApiMemberId
    {
        get => _ApiMemberId;
        set
        {
            if (_ApiMemberId != value)
            {
                SendPropertyChanging("ApiMemberId");
                _ApiMemberId = value;
                SendPropertyChanged("ApiMemberId");
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





    public string TypeUid
    {
        get => _TypeUid;
        set
        {
            if (_TypeUid != value)
            {
                SendPropertyChanging("TypeUid");
                _TypeUid = value;
                SendPropertyChanged("TypeUid");
            }
        }
    }





    public string NullableAnnotation
    {
        get => _NullableAnnotation;
        set
        {
            if (_NullableAnnotation != value)
            {
                SendPropertyChanging("NullableAnnotation");
                _NullableAnnotation = value;
                SendPropertyChanged("NullableAnnotation");
            }
        }
    }





    public int? Position
    {
        get => _Position;
        set
        {
            if (_Position != value)
            {
                SendPropertyChanging("Position");
                _Position = value;
                SendPropertyChanged("Position");
            }
        }
    }





    public string Modifier
    {
        get => _Modifier;
        set
        {
            if (_Modifier != value)
            {
                SendPropertyChanging("Modifier");
                _Modifier = value;
                SendPropertyChanged("Modifier");
            }
        }
    }





    public bool? HasDefaultValue
    {
        get => _HasDefaultValue;
        set
        {
            if (_HasDefaultValue != value)
            {
                SendPropertyChanging("HasDefaultValue");
                _HasDefaultValue = value;
                SendPropertyChanged("HasDefaultValue");
            }
        }
    }





    public string DefaultValueLiteral
    {
        get => _DefaultValueLiteral;
        set
        {
            if (_DefaultValueLiteral != value)
            {
                SendPropertyChanging("DefaultValueLiteral");
                _DefaultValueLiteral = value;
                SendPropertyChanged("DefaultValueLiteral");
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