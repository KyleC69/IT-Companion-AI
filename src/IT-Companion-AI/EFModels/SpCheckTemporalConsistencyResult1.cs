namespace ITCompanionAI;


public partial class SpCheckTemporalConsistencyResult1 : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    public SpCheckTemporalConsistencyResult1()
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





    public Guid Id
    {
        get;
        set
        {
            if (field != value)
            {
                SendPropertyChanging("Id");
                field = value;
                SendPropertyChanged("Id");
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