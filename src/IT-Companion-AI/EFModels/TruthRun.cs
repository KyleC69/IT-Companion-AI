namespace ITCompanionAI.EFModels;


public partial class TruthRun : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private Guid _Id;

    private string _SchemaVersion;

    private Guid _SnapshotId;

    private DateTime _TimestampUtc;








    public TruthRun()
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





    public Guid SnapshotId
    {
        get => _SnapshotId;
        set
        {
            if (_SnapshotId != value)
            {
                SendPropertyChanging("SnapshotId");
                _SnapshotId = value;
                SendPropertyChanged("SnapshotId");
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