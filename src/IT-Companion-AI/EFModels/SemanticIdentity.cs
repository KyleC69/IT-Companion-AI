namespace ITCompanionAI.EFModels;


public partial class SemanticIdentity : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private DateTime _CreatedUtc;

    private string _Kind;

    private string _Notes;

    private string _Uid;

    private byte[] _UidHash;








    public SemanticIdentity()
    {
        OnCreated();
    }








    public byte[] UidHash
    {
        get => _UidHash;
        set
        {
            if (_UidHash != value)
            {
                SendPropertyChanging("UidHash");
                _UidHash = value;
                SendPropertyChanged("UidHash");
            }
        }
    }





    public string Uid
    {
        get => _Uid;
        set
        {
            if (_Uid != value)
            {
                SendPropertyChanging("Uid");
                _Uid = value;
                SendPropertyChanged("Uid");
            }
        }
    }





    public string Kind
    {
        get => _Kind;
        set
        {
            if (_Kind != value)
            {
                SendPropertyChanging("Kind");
                _Kind = value;
                SendPropertyChanged("Kind");
            }
        }
    }





    public DateTime CreatedUtc
    {
        get => _CreatedUtc;
        set
        {
            if (_CreatedUtc != value)
            {
                SendPropertyChanging("CreatedUtc");
                _CreatedUtc = value;
                SendPropertyChanged("CreatedUtc");
            }
        }
    }





    public string Notes
    {
        get => _Notes;
        set
        {
            if (_Notes != value)
            {
                SendPropertyChanging("Notes");
                _Notes = value;
                SendPropertyChanged("Notes");
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