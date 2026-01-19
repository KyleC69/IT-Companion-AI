namespace ITCompanionAI.EFModels;


public partial class SourceSnapshot : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Branch;

    private string _ConfigJson;

    private Guid _Id;

    private Guid _IngestionRunId;

    private string _Language;

    private string _PackageName;

    private string _PackageVersion;

    private string _RepoCommit;

    private string _RepoUrl;

    private string _SnapshotUid;

    private byte[] _SnapshotUidHash;








    public SourceSnapshot()
    {
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





    public Guid IngestionRunId
    {
        get => _IngestionRunId;
        set
        {
            if (_IngestionRunId != value)
            {
                SendPropertyChanging("IngestionRunId");
                _IngestionRunId = value;
                SendPropertyChanged("IngestionRunId");
            }
        }
    }





    public string SnapshotUid
    {
        get => _SnapshotUid;
        set
        {
            if (_SnapshotUid != value)
            {
                SendPropertyChanging("SnapshotUid");
                _SnapshotUid = value;
                SendPropertyChanged("SnapshotUid");
            }
        }
    }





    public string RepoUrl
    {
        get => _RepoUrl;
        set
        {
            if (_RepoUrl != value)
            {
                SendPropertyChanging("RepoUrl");
                _RepoUrl = value;
                SendPropertyChanged("RepoUrl");
            }
        }
    }





    public string Branch
    {
        get => _Branch;
        set
        {
            if (_Branch != value)
            {
                SendPropertyChanging("Branch");
                _Branch = value;
                SendPropertyChanged("Branch");
            }
        }
    }





    public string RepoCommit
    {
        get => _RepoCommit;
        set
        {
            if (_RepoCommit != value)
            {
                SendPropertyChanging("RepoCommit");
                _RepoCommit = value;
                SendPropertyChanged("RepoCommit");
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





    public string PackageName
    {
        get => _PackageName;
        set
        {
            if (_PackageName != value)
            {
                SendPropertyChanging("PackageName");
                _PackageName = value;
                SendPropertyChanged("PackageName");
            }
        }
    }





    public string PackageVersion
    {
        get => _PackageVersion;
        set
        {
            if (_PackageVersion != value)
            {
                SendPropertyChanging("PackageVersion");
                _PackageVersion = value;
                SendPropertyChanged("PackageVersion");
            }
        }
    }





    public string ConfigJson
    {
        get => _ConfigJson;
        set
        {
            if (_ConfigJson != value)
            {
                SendPropertyChanging("ConfigJson");
                _ConfigJson = value;
                SendPropertyChanged("ConfigJson");
            }
        }
    }





    public byte[] SnapshotUidHash
    {
        get => _SnapshotUidHash;
        set
        {
            if (_SnapshotUidHash != value)
            {
                SendPropertyChanging("SnapshotUidHash");
                _SnapshotUidHash = value;
                SendPropertyChanged("SnapshotUidHash");
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