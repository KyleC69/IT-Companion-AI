namespace ITCompanionAI.EFModels;


public partial class Sample : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Code;

    private string _DerivedFromCodeUid;

    private string _EntryPoint;

    private string _FeatureUid;

    private Guid _Id;

    private string _Language;

    private string _PackageReferences;

    private Guid _SampleRunId;

    private string _SampleUid;

    private string _Tags;

    private string _TargetFramework;








    public Sample()
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





    public Guid SampleRunId
    {
        get => _SampleRunId;
        set
        {
            if (_SampleRunId != value)
            {
                SendPropertyChanging("SampleRunId");
                _SampleRunId = value;
                SendPropertyChanged("SampleRunId");
            }
        }
    }





    public string SampleUid
    {
        get => _SampleUid;
        set
        {
            if (_SampleUid != value)
            {
                SendPropertyChanging("SampleUid");
                _SampleUid = value;
                SendPropertyChanged("SampleUid");
            }
        }
    }





    public string FeatureUid
    {
        get => _FeatureUid;
        set
        {
            if (_FeatureUid != value)
            {
                SendPropertyChanging("FeatureUid");
                _FeatureUid = value;
                SendPropertyChanged("FeatureUid");
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





    public string Code
    {
        get => _Code;
        set
        {
            if (_Code != value)
            {
                SendPropertyChanging("Code");
                _Code = value;
                SendPropertyChanged("Code");
            }
        }
    }





    public string EntryPoint
    {
        get => _EntryPoint;
        set
        {
            if (_EntryPoint != value)
            {
                SendPropertyChanging("EntryPoint");
                _EntryPoint = value;
                SendPropertyChanged("EntryPoint");
            }
        }
    }





    public string TargetFramework
    {
        get => _TargetFramework;
        set
        {
            if (_TargetFramework != value)
            {
                SendPropertyChanging("TargetFramework");
                _TargetFramework = value;
                SendPropertyChanged("TargetFramework");
            }
        }
    }





    public string PackageReferences
    {
        get => _PackageReferences;
        set
        {
            if (_PackageReferences != value)
            {
                SendPropertyChanging("PackageReferences");
                _PackageReferences = value;
                SendPropertyChanged("PackageReferences");
            }
        }
    }





    public string DerivedFromCodeUid
    {
        get => _DerivedFromCodeUid;
        set
        {
            if (_DerivedFromCodeUid != value)
            {
                SendPropertyChanging("DerivedFromCodeUid");
                _DerivedFromCodeUid = value;
                SendPropertyChanged("DerivedFromCodeUid");
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