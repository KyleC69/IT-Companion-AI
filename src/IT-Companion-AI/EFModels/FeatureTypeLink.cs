namespace ITCompanionAI.EFModels;


public partial class FeatureTypeLink : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private Guid _FeatureId;

    private Guid _Id;

    private string _Role;

    private string _TypeUid;








    public FeatureTypeLink()
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





    public string Role
    {
        get => _Role;
        set
        {
            if (_Role != value)
            {
                SendPropertyChanging("Role");
                _Role = value;
                SendPropertyChanged("Role");
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