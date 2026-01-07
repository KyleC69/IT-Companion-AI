// Project Name: SKAgent
// File Name: KBCurator.SnapshotDiff.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;


namespace ITCompanionAI.Entities;


public partial class SnapshotDiff : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private Guid _Id;

    private Guid _NewSnapshotId;

    private Guid _OldSnapshotId;

    private string _SchemaVersion;

    private DateTime _TimestampUtc;







    public SnapshotDiff()
    {
        OnCreated();
    }







    [NotNullValidator()]
    public Guid Id
    {
        get => _Id;
        set
        {
            if (_Id != value)
            {
                OnIdChanging(value);
                SendPropertyChanging("Id");
                _Id = value;
                SendPropertyChanged("Id");
                OnIdChanged();
            }
        }
    }

    [NotNullValidator()]
    public Guid OldSnapshotId
    {
        get => _OldSnapshotId;
        set
        {
            if (_OldSnapshotId != value)
            {
                OnOldSnapshotIdChanging(value);
                SendPropertyChanging("OldSnapshotId");
                _OldSnapshotId = value;
                SendPropertyChanged("OldSnapshotId");
                OnOldSnapshotIdChanged();
            }
        }
    }

    [NotNullValidator()]
    public Guid NewSnapshotId
    {
        get => _NewSnapshotId;
        set
        {
            if (_NewSnapshotId != value)
            {
                OnNewSnapshotIdChanging(value);
                SendPropertyChanging("NewSnapshotId");
                _NewSnapshotId = value;
                SendPropertyChanged("NewSnapshotId");
                OnNewSnapshotIdChanged();
            }
        }
    }

    [NotNullValidator()]
    public DateTime TimestampUtc
    {
        get => _TimestampUtc;
        set
        {
            if (_TimestampUtc != value)
            {
                OnTimestampUtcChanging(value);
                SendPropertyChanging("TimestampUtc");
                _TimestampUtc = value;
                SendPropertyChanged("TimestampUtc");
                OnTimestampUtcChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 200, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SchemaVersion
    {
        get => _SchemaVersion;
        set
        {
            if (_SchemaVersion != value)
            {
                OnSchemaVersionChanging(value);
                SendPropertyChanging("SchemaVersion");
                _SchemaVersion = value;
                SendPropertyChanged("SchemaVersion");
                OnSchemaVersionChanged();
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
    partial void OnIdChanging(Guid value);

    partial void OnIdChanged();
    partial void OnOldSnapshotIdChanging(Guid value);

    partial void OnOldSnapshotIdChanged();
    partial void OnNewSnapshotIdChanging(Guid value);

    partial void OnNewSnapshotIdChanged();
    partial void OnTimestampUtcChanging(DateTime value);

    partial void OnTimestampUtcChanged();
    partial void OnSchemaVersionChanging(string value);

    partial void OnSchemaVersionChanged();

    #endregion
}