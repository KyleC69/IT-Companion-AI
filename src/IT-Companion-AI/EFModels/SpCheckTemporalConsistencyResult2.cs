namespace ITCompanionAI;


public partial class SpCheckTemporalConsistencyResult2 : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    public SpCheckTemporalConsistencyResult2()
    {
        OnCreated();
    }








    public string TableName
    {
        get;
        set
        {
            if (field != value)
            {
                SendPropertyChanging("TableName");
                field = value;
                SendPropertyChanged("TableName");
            }
        }
    }





    public string SemanticUid
    {
        get;
        set
        {
            if (field != value)
            {
                SendPropertyChanging("SemanticUid");
                field = value;
                SendPropertyChanged("SemanticUid");
            }
        }
    }





    public int VersionNumber
    {
        get;
        set
        {
            if (field != value)
            {
                SendPropertyChanging("VersionNumber");
                field = value;
                SendPropertyChanged("VersionNumber");
            }
        }
    }





    public DateTime ValidFromUtc
    {
        get;
        set
        {
            if (field != value)
            {
                SendPropertyChanging("ValidFromUtc");
                field = value;
                SendPropertyChanged("ValidFromUtc");
            }
        }
    }





    public DateTime? ValidToUtc
    {
        get;
        set
        {
            if (field != value)
            {
                SendPropertyChanging("ValidToUtc");
                field = value;
                SendPropertyChanged("ValidToUtc");
            }
        }
    }





    public DateTime? NextFrom
    {
        get;
        set
        {
            if (field != value)
            {
                SendPropertyChanging("NextFrom");
                field = value;
                SendPropertyChanged("NextFrom");
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