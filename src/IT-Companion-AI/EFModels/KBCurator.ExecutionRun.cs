// Project Name: SKAgent
// File Name: KBCurator.ExecutionRun.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace ITCompanionAI.Entities;


public partial class ExecutionRun : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _EnvironmentJson;

    private Guid _Id;

    private Guid _SampleRunId;

    private string _SchemaVersion;

    private Guid _SnapshotId;

    private DateTime _TimestampUtc;







    public ExecutionRun()
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
    public Guid SnapshotId
    {
        get => _SnapshotId;
        set
        {
            if (_SnapshotId != value)
            {
                OnSnapshotIdChanging(value);
                SendPropertyChanging("SnapshotId");
                _SnapshotId = value;
                SendPropertyChanged("SnapshotId");
                OnSnapshotIdChanged();
            }
        }
    }

    [NotNullValidator()]
    public Guid SampleRunId
    {
        get => _SampleRunId;
        set
        {
            if (_SampleRunId != value)
            {
                OnSampleRunIdChanging(value);
                SendPropertyChanging("SampleRunId");
                _SampleRunId = value;
                SendPropertyChanged("SampleRunId");
                OnSampleRunIdChanged();
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

    public string EnvironmentJson
    {
        get => _EnvironmentJson;
        set
        {
            if (_EnvironmentJson != value)
            {
                OnEnvironmentJsonChanging(value);
                SendPropertyChanging("EnvironmentJson");
                _EnvironmentJson = value;
                SendPropertyChanged("EnvironmentJson");
                OnEnvironmentJsonChanged();
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
    partial void OnSnapshotIdChanging(Guid value);

    partial void OnSnapshotIdChanged();
    partial void OnSampleRunIdChanging(Guid value);

    partial void OnSampleRunIdChanged();
    partial void OnTimestampUtcChanging(DateTime value);

    partial void OnTimestampUtcChanged();
    partial void OnEnvironmentJsonChanging(string value);

    partial void OnEnvironmentJsonChanged();
    partial void OnSchemaVersionChanging(string value);

    partial void OnSchemaVersionChanged();

    #endregion
}