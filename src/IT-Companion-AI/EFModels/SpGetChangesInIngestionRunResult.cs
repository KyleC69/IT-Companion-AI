namespace ITCompanionAI;


public partial class SpGetChangesInIngestionRunResult : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    public SpGetChangesInIngestionRunResult()
    {
        OnCreated();
    }








    public string ArtifactKind
    {
        get;
        set
        {
            if (field != value)
            {
                SendPropertyChanging("ArtifactKind");
                field = value;
                SendPropertyChanged("ArtifactKind");
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