// Project Name: SKAgent
// File Name: KBCurator.SpCheckTemporalConsistencyResult2.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;


namespace ITCompanionAI.Entities;


public partial class SpCheckTemporalConsistencyResult2 : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private DateTime? _NextFrom;

    private string _SemanticUid;

    private string _TableName;

    private DateTime _ValidFromUtc;

    private DateTime? _ValidToUtc;

    private int _VersionNumber;







    public SpCheckTemporalConsistencyResult2()
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

    [NotNullValidator()]
    public int VersionNumber
    {
        get => _VersionNumber;
        set
        {
            if (_VersionNumber != value)
            {
                OnVersionNumberChanging(value);
                SendPropertyChanging("VersionNumber");
                _VersionNumber = value;
                SendPropertyChanged("VersionNumber");
                OnVersionNumberChanged();
            }
        }
    }

    [NotNullValidator()]
    public DateTime ValidFromUtc
    {
        get => _ValidFromUtc;
        set
        {
            if (_ValidFromUtc != value)
            {
                OnValidFromUtcChanging(value);
                SendPropertyChanging("ValidFromUtc");
                _ValidFromUtc = value;
                SendPropertyChanged("ValidFromUtc");
                OnValidFromUtcChanged();
            }
        }
    }

    public DateTime? ValidToUtc
    {
        get => _ValidToUtc;
        set
        {
            if (_ValidToUtc != value)
            {
                OnValidToUtcChanging(value);
                SendPropertyChanging("ValidToUtc");
                _ValidToUtc = value;
                SendPropertyChanged("ValidToUtc");
                OnValidToUtcChanged();
            }
        }
    }

    public DateTime? NextFrom
    {
        get => _NextFrom;
        set
        {
            if (_NextFrom != value)
            {
                OnNextFromChanging(value);
                SendPropertyChanging("NextFrom");
                _NextFrom = value;
                SendPropertyChanged("NextFrom");
                OnNextFromChanged();
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
    partial void OnVersionNumberChanging(int value);

    partial void OnVersionNumberChanged();
    partial void OnValidFromUtcChanging(DateTime value);

    partial void OnValidFromUtcChanged();
    partial void OnValidToUtcChanging(DateTime? value);

    partial void OnValidToUtcChanged();
    partial void OnNextFromChanging(DateTime? value);

    partial void OnNextFromChanged();

    #endregion
}