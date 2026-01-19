namespace ITCompanionAI.EFModels;


public partial class ApiMemberDiff : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private bool? _Breaking;

    private string _ChangeKind;

    private string _DetailJson;

    private Guid _Id;

    private string _MemberUid;

    private string _NewSignature;

    private string _OldSignature;

    private Guid _SnapshotDiffId;








    public ApiMemberDiff()
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





    public string MemberUid
    {
        get => _MemberUid;
        set
        {
            if (_MemberUid != value)
            {
                SendPropertyChanging("MemberUid");
                _MemberUid = value;
                SendPropertyChanged("MemberUid");
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





    public string OldSignature
    {
        get => _OldSignature;
        set
        {
            if (_OldSignature != value)
            {
                SendPropertyChanging("OldSignature");
                _OldSignature = value;
                SendPropertyChanged("OldSignature");
            }
        }
    }





    public string NewSignature
    {
        get => _NewSignature;
        set
        {
            if (_NewSignature != value)
            {
                SendPropertyChanging("NewSignature");
                _NewSignature = value;
                SendPropertyChanged("NewSignature");
            }
        }
    }





    public bool? Breaking
    {
        get => _Breaking;
        set
        {
            if (_Breaking != value)
            {
                SendPropertyChanging("Breaking");
                _Breaking = value;
                SendPropertyChanged("Breaking");
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