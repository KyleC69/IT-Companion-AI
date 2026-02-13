namespace ITCompanionAI;


public partial class SpVerifyIngestionRunResult : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    public SpVerifyIngestionRunResult()
    {
        OnCreated();
    }








    public string Category
    {
        get;
        set
        {
            if (field != value)
            {
                SendPropertyChanging("Category");
                field = value;
                SendPropertyChanged("Category");
            }
        }
    }





    public string Detail
    {
        get;
        set
        {
            if (field != value)
            {
                SendPropertyChanging("Detail");
                field = value;
                SendPropertyChanged("Detail");
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