namespace ITCompanionAI.EFModels;


public partial class CodeBlock : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Content;

    private byte[] _ContentHash;

    private Guid _CreatedIngestionRunId;

    private string _DeclaredPackages;

    private Guid _DocSectionId;

    private Guid _Id;

    private string _InlineComments;

    private bool _IsActive;

    private string _Language;

    private Guid? _RemovedIngestionRunId;

    private string _SemanticUid;

    private string _Tags;

    private Guid _UpdatedIngestionRunId;

    private DateTime _ValidFromUtc;

    private DateTime? _ValidToUtc;

    private int _VersionNumber;








    public CodeBlock()
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





    public Guid DocSectionId
    {
        get => _DocSectionId;
        set
        {
            if (_DocSectionId != value)
            {
                SendPropertyChanging("DocSectionId");
                _DocSectionId = value;
                SendPropertyChanged("DocSectionId");
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





    public string Content
    {
        get => _Content;
        set
        {
            if (_Content != value)
            {
                SendPropertyChanging("Content");
                _Content = value;
                SendPropertyChanged("Content");
            }
        }
    }





    public string DeclaredPackages
    {
        get => _DeclaredPackages;
        set
        {
            if (_DeclaredPackages != value)
            {
                SendPropertyChanging("DeclaredPackages");
                _DeclaredPackages = value;
                SendPropertyChanged("DeclaredPackages");
            }
        }
    }





    public string Tags
    {
        get => _Tags;
        set
        {
            if (_Tags != value)
            {
                SendPropertyChanging("Tags");
                _Tags = value;
                SendPropertyChanged("Tags");
            }
        }
    }





    public string InlineComments
    {
        get => _InlineComments;
        set
        {
            if (_InlineComments != value)
            {
                SendPropertyChanging("InlineComments");
                _InlineComments = value;
                SendPropertyChanged("InlineComments");
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