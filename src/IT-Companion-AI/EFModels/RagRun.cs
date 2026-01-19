namespace ITCompanionAI.EFModels;


public partial class RagRun : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private Guid _Id;

    private string _SchemaVersion;

    private Guid _SnapshotId;

    private DateTime _TimestampUtc;








    public RagRun()
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