// Project Name: SKAgent
// File Name: KBCurator.VerifyIngestionRunResult.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


public partial class VerifyIngestionRunResult : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _Category;

    private string _Detail;







    public VerifyIngestionRunResult()
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
                OnCategoryChanging(value);
                SendPropertyChanging("Category");
                _Category = value;
                SendPropertyChanged("Category");
                OnCategoryChanged();
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
                OnDetailChanging(value);
                SendPropertyChanging("Detail");
                _Detail = value;
                SendPropertyChanged("Detail");
                OnDetailChanged();
            }
        }
    }

    public virtual event PropertyChangedEventHandler PropertyChanged;

    public virtual event PropertyChangingEventHandler PropertyChanging;







    protected virtual void SendPropertyChanging()
    {
        PropertyChangingEventHandler? handler = this.PropertyChanging;
        if (handler != null)
        {
            handler(this, emptyChangingEventArgs);
        }
    }







    protected virtual void SendPropertyChanging(string propertyName)
    {
        PropertyChangingEventHandler? handler = this.PropertyChanging;
        if (handler != null)
        {
            handler(this, new PropertyChangingEventArgs(propertyName));
        }
    }







    protected virtual void SendPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler? handler = this.PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }







    #region Extensibility Method Definitions

    partial void OnCreated();
    partial void OnCategoryChanging(string value);

    partial void OnCategoryChanged();
    partial void OnDetailChanging(string value);

    partial void OnDetailChanged();

    #endregion
}