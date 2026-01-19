namespace ITCompanionAI.EFModels;


public partial class ReviewItem : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private Guid _Id;

    private Guid _ReviewRunId;

    private string _Status;

    private string _Summary;

    private string _TargetKind;

    private string _TargetUid;








    public ReviewItem()
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





    public Guid ReviewRunId
    {
        get => _ReviewRunId;
        set
        {
            if (_ReviewRunId != value)
            {
                SendPropertyChanging("ReviewRunId");
                _ReviewRunId = value;
                SendPropertyChanged("ReviewRunId");
            }
        }
    }





    public string TargetKind
    {
        get => _TargetKind;
        set
        {
            if (_TargetKind != value)
            {
                SendPropertyChanging("TargetKind");
                _TargetKind = value;
                SendPropertyChanged("TargetKind");
            }
        }
    }





    public string TargetUid
    {
        get => _TargetUid;
        set
        {
            if (_TargetUid != value)
            {
                SendPropertyChanging("TargetUid");
                _TargetUid = value;
                SendPropertyChanged("TargetUid");
            }
        }
    }





    public string Status
    {
        get => _Status;
        set
        {
            if (_Status != value)
            {
                SendPropertyChanging("Status");
                _Status = value;
                SendPropertyChanged("Status");
            }
        }
    }





    public string Summary
    {
        get => _Summary;
        set
        {
            if (_Summary != value)
            {
                SendPropertyChanging("Summary");
                _Summary = value;
                SendPropertyChanged("Summary");
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