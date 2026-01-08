// Project Name: SKAgent
// File Name: KBCurator.SemanticIdentity.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.Entities;


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







    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string Uid
    {
        get => _Uid;
        set
        {
            if (_Uid != value)
            {
                OnUidChanging(value);
                SendPropertyChanging("Uid");
                _Uid = value;
                SendPropertyChanged("Uid");
                OnUidChanged();
            }
        }
    }

    [NotNullValidator()]
    public byte[] UidHash
    {
        get => _UidHash;
        set
        {
            if (_UidHash != value)
            {
                OnUidHashChanging(value);
                SendPropertyChanging("UidHash");
                _UidHash = value;
                SendPropertyChanged("UidHash");
                OnUidHashChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 50, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string Kind
    {
        get => _Kind;
        set
        {
            if (_Kind != value)
            {
                OnKindChanging(value);
                SendPropertyChanging("Kind");
                _Kind = value;
                SendPropertyChanged("Kind");
                OnKindChanged();
            }
        }
    }

    [NotNullValidator()]
    public DateTime CreatedUtc
    {
        get => _CreatedUtc;
        set
        {
            if (_CreatedUtc != value)
            {
                OnCreatedUtcChanging(value);
                SendPropertyChanging("CreatedUtc");
                _CreatedUtc = value;
                SendPropertyChanged("CreatedUtc");
                OnCreatedUtcChanged();
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
                OnNotesChanging(value);
                SendPropertyChanging("Notes");
                _Notes = value;
                SendPropertyChanged("Notes");
                OnNotesChanged();
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
    partial void OnUidChanging(string value);

    partial void OnUidChanged();
    partial void OnUidHashChanging(byte[] value);

    partial void OnUidHashChanged();
    partial void OnKindChanging(string value);

    partial void OnKindChanged();
    partial void OnCreatedUtcChanging(DateTime value);

    partial void OnCreatedUtcChanged();
    partial void OnNotesChanging(string value);

    partial void OnNotesChanged();

    #endregion
}