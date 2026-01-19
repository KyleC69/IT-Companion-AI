namespace ITCompanionAI.EFModels;


public partial class SampleApiMemberLink : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private Guid _Id;

    private string _MemberUid;

    private Guid _SampleId;








    public SampleApiMemberLink()
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





    public Guid SampleId
    {
        get => _SampleId;
        set
        {
            if (_SampleId != value)
            {
                SendPropertyChanging("SampleId");
                _SampleId = value;
                SendPropertyChanged("SampleId");
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