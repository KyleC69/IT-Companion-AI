namespace ITCompanionAI.EFModels;


public partial class IngestionRun : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private IList<ApiFeature> _ApiFeatures;

    private IList<ApiMember> _ApiMembers;

    private IList<ApiParameter> _ApiParameters;

    private IList<ApiType> _ApiTypes;

    private IList<DocPage> _DocPages;

    private IList<DocSection> _DocSections;

    private Guid _Id;

    private string _Notes;

    private string _SchemaVersion;

    private DateTime _TimestampUtc;








    public IngestionRun()
    {
        _ApiFeatures = new List<ApiFeature>();
        _ApiMembers = new List<ApiMember>();
        _ApiParameters = new List<ApiParameter>();
        _ApiTypes = new List<ApiType>();
        _DocPages = new List<DocPage>();
        _DocSections = new List<DocSection>();
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





    public DateTime TimestampUtc
    {
        get => _TimestampUtc;
        set
        {
            if (_TimestampUtc != value)
            {
                SendPropertyChanging("TimestampUtc");
                _TimestampUtc = value;
                SendPropertyChanged("TimestampUtc");
            }
        }
    }





    public string SchemaVersion
    {
        get => _SchemaVersion;
        set
        {
            if (_SchemaVersion != value)
            {
                SendPropertyChanging("SchemaVersion");
                _SchemaVersion = value;
                SendPropertyChanged("SchemaVersion");
            }
        }
    }





    public string Notes
    {
        get => _Notes;
        set
        {
            if (_Notes != value)
            {
                SendPropertyChanging("Notes");
                _Notes = value;
                SendPropertyChanged("Notes");
            }
        }
    }





    public virtual IList<ApiFeature> ApiFeatures
    {
        get => _ApiFeatures;
        set => _ApiFeatures = value;
    }





    public virtual IList<ApiMember> ApiMembers
    {
        get => _ApiMembers;
        set => _ApiMembers = value;
    }





    public virtual IList<ApiParameter> ApiParameters
    {
        get => _ApiParameters;
        set => _ApiParameters = value;
    }





    public virtual IList<ApiType> ApiTypes
    {
        get => _ApiTypes;
        set => _ApiTypes = value;
    }





    public virtual IList<DocPage> DocPages
    {
        get => _DocPages;
        set => _DocPages = value;
    }





    public virtual IList<DocSection> DocSections
    {
        get => _DocSections;
        set => _DocSections = value;
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