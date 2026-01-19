namespace ITCompanionAI.EFModels;


public partial class ApiTypeDiff : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _ChangeKind;

    private string _DetailJson;

    private Guid _Id;

    private Guid _SnapshotDiffId;

    private string _TypeUid;








    public ApiTypeDiff()
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