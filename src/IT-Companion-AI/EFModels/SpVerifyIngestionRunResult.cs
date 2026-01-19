namespace ITCompanionAI.EFModels;


public partial class SpVerifyIngestionRunResult : INotifyPropertyChanging, INotifyPropertyChanged
{

    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Category;

    private string _Detail;








    public SpVerifyIngestionRunResult()
    {
        OnCreated();
    }








    public string Category
    {
        get => _Category;
        set
        {
            if (_Category != value)
            {
                SendPropertyChanging("Category");
                _Category = value;
                SendPropertyChanged("Category");
            }
        }
    }





    public string Detail
    {
        get => _Detail;
        set
        {
            if (_Detail != value)
            {
                SendPropertyChanging("Detail");
                _Detail = value;
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