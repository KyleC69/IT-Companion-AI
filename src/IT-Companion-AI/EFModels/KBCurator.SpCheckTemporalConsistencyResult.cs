// Project Name: SKAgent
// File Name: KBCurator.SpCheckTemporalConsistencyResult.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;


namespace ITCompanionAI.Entities;


public partial class SpCheckTemporalConsistencyResult : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _SemanticUid;

    private string _TableName;







    public SpCheckTemporalConsistencyResult()
    {
        OnCreated();
    }







    [NotNullValidator()]
    public string TableName
    {
        get => _TableName;
        set
        {
            if (_TableName != value)
            {
                OnTableNameChanging(value);
                SendPropertyChanging("TableName");
                _TableName = value;
                SendPropertyChanged("TableName");
                OnTableNameChanged();
            }
        }
    }

    [NotNullValidator()]
    public string SemanticUid
    {
        get => _SemanticUid;
        set
        {
            if (_SemanticUid != value)
            {
                OnSemanticUidChanging(value);
                SendPropertyChanging("SemanticUid");
                _SemanticUid = value;
                SendPropertyChanged("SemanticUid");
                OnSemanticUidChanged();
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
    partial void OnTableNameChanging(string value);

    partial void OnTableNameChanged();
    partial void OnSemanticUidChanging(string value);

    partial void OnSemanticUidChanged();

    #endregion
}