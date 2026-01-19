namespace ITCompanionAI.EFModels;


public partial class DocPage : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private byte[] _ContentHash;

    private Guid _CreatedIngestionRunId;

    private Guid _Id;

    private IngestionRun _IngestionRun;

    private bool _IsActive;

    private string _Language;

    private string _RawMarkdown;

    private Guid? _RemovedIngestionRunId;

    private string _SemanticUid;

    private byte[] _SemanticUidHash;

    private string _SourcePath;

    private Guid _SourceSnapshotId;

    private string _Title;

    private Guid _UpdatedIngestionRunId;

    private string _Url;

    private DateTime _ValidFromUtc;

    private DateTime? _ValidToUtc;

    private int _VersionNumber;








    public DocPage()
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





    public string SourcePath
    {
        get => _SourcePath;
        set
        {
            if (_SourcePath != value)
            {
                SendPropertyChanging("SourcePath");
                _SourcePath = value;
                SendPropertyChanged("SourcePath");
            }
        }
    }





    public string Title
    {
        get => _Title;
        set
        {
            if (_Title != value)
            {
                SendPropertyChanging("Title");
                _Title = value;
                SendPropertyChanged("Title");
            }
        }
    }





    public string Language
    {
        get => _Language;
        set
        {
            if (_Language != value)
            {
                SendPropertyChanging("Language");
                _Language = value;
                SendPropertyChanged("Language");
            }
        }
    }





    public string Url
    {
        get => _Url;
        set
        {
            if (_Url != value)
            {
                SendPropertyChanging("Url");
                _Url = value;
                SendPropertyChanged("Url");
            }
        }
    }





    public string RawMarkdown
    {
        get => _RawMarkdown;
        set
        {
            if (_RawMarkdown != value)
            {
                SendPropertyChanging("RawMarkdown");
                _RawMarkdown = value;
                SendPropertyChanged("RawMarkdown");
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