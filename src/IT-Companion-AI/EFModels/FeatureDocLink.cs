namespace ITCompanionAI.EFModels;


public partial class FeatureDocLink : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _DocUid;

    private Guid _FeatureId;

    private Guid _Id;

    private string _SectionUid;








    public FeatureDocLink()
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





    public Guid FeatureId
    {
        get => _FeatureId;
        set
        {
            if (_FeatureId != value)
            {
                SendPropertyChanging("FeatureId");
                _FeatureId = value;
                SendPropertyChanged("FeatureId");
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





    public string SectionUid
    {
        get => _SectionUid;
        set
        {
            if (_SectionUid != value)
            {
                SendPropertyChanging("SectionUid");
                _SectionUid = value;
                SendPropertyChanged("SectionUid");
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