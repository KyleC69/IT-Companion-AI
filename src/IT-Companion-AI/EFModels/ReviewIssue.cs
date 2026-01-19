namespace ITCompanionAI.EFModels;


public partial class ReviewIssue : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Code;

    private string _Details;

    private Guid _Id;

    private string _RelatedMemberUid;

    private Guid _ReviewItemId;

    private string _Severity;








    public ReviewIssue()
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





    public Guid ReviewItemId
    {
        get => _ReviewItemId;
        set
        {
            if (_ReviewItemId != value)
            {
                SendPropertyChanging("ReviewItemId");
                _ReviewItemId = value;
                SendPropertyChanged("ReviewItemId");
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





    public string Severity
    {
        get => _Severity;
        set
        {
            if (_Severity != value)
            {
                SendPropertyChanging("Severity");
                _Severity = value;
                SendPropertyChanged("Severity");
            }
        }
    }





    public string RelatedMemberUid
    {
        get => _RelatedMemberUid;
        set
        {
            if (_RelatedMemberUid != value)
            {
                SendPropertyChanging("RelatedMemberUid");
                _RelatedMemberUid = value;
                SendPropertyChanged("RelatedMemberUid");
            }
        }
    }





    public string Details
    {
        get => _Details;
        set
        {
            if (_Details != value)
            {
                SendPropertyChanging("Details");
                _Details = value;
                SendPropertyChanged("Details");
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