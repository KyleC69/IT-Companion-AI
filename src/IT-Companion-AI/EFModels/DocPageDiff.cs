namespace ITCompanionAI.EFModels;


public partial class DocPageDiff : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _ChangeKind;

    private string _DetailJson;

    private string _DocUid;

    private Guid _Id;

    private Guid _SnapshotDiffId;








    public DocPageDiff()
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





    public Guid SnapshotDiffId
    {
        get => _SnapshotDiffId;
        set
        {
            if (_SnapshotDiffId != value)
            {
                SendPropertyChanging("SnapshotDiffId");
                _SnapshotDiffId = value;
                SendPropertyChanged("SnapshotDiffId");
            }
        }
    }





    public string DocUid
    {
        get => _DocUid;
        set
        {
            if (_DocUid != value)
            {
                SendPropertyChanging("DocUid");
                _DocUid = value;
                SendPropertyChanged("DocUid");
            }
        }
    }





    public string ChangeKind
    {
        get => _ChangeKind;
        set
        {
            if (_ChangeKind != value)
            {
                SendPropertyChanging("ChangeKind");
                _ChangeKind = value;
                SendPropertyChanged("ChangeKind");
            }
        }
    }





    public string DetailJson
    {
        get => _DetailJson;
        set
        {
            if (_DetailJson != value)
            {
                SendPropertyChanging("DetailJson");
                _DetailJson = value;
                SendPropertyChanged("DetailJson");
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