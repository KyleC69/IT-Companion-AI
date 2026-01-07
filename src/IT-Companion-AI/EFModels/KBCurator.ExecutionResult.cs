// Project Name: SKAgent
// File Name: KBCurator.ExecutionResult.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel;


namespace ITCompanionAI.Entities;


public partial class ExecutionResult : INotifyPropertyChanging, INotifyPropertyChanged
{
    private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new(string.Empty);

    private string _BuildLog;

    private int? _DurationMs;

    private string _ExceptionJson;

    private Guid _ExecutionRunId;

    private Guid _Id;

    private string _RunLog;

    private string _SampleUid;

    private string _Status;







    public ExecutionResult()
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
    public Guid ExecutionRunId
    {
        get => _ExecutionRunId;
        set
        {
            if (_ExecutionRunId != value)
            {
                OnExecutionRunIdChanging(value);
                SendPropertyChanging("ExecutionRunId");
                _ExecutionRunId = value;
                SendPropertyChanged("ExecutionRunId");
                OnExecutionRunIdChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 1000, RangeBoundaryType.Inclusive)]
    [NotNullValidator()]
    public string SampleUid
    {
        get => _SampleUid;
        set
        {
            if (_SampleUid != value)
            {
                OnSampleUidChanging(value);
                SendPropertyChanging("SampleUid");
                _SampleUid = value;
                SendPropertyChanged("SampleUid");
                OnSampleUidChanged();
            }
        }
    }

    [StringLengthValidator(0, RangeBoundaryType.Ignore, 100, RangeBoundaryType.Inclusive)]
    public string Status
    {
        get => _Status;
        set
        {
            if (_Status != value)
            {
                OnStatusChanging(value);
                SendPropertyChanging("Status");
                _Status = value;
                SendPropertyChanged("Status");
                OnStatusChanged();
            }
        }
    }

    public string BuildLog
    {
        get => _BuildLog;
        set
        {
            if (_BuildLog != value)
            {
                OnBuildLogChanging(value);
                SendPropertyChanging("BuildLog");
                _BuildLog = value;
                SendPropertyChanged("BuildLog");
                OnBuildLogChanged();
            }
        }
    }

    public string RunLog
    {
        get => _RunLog;
        set
        {
            if (_RunLog != value)
            {
                OnRunLogChanging(value);
                SendPropertyChanging("RunLog");
                _RunLog = value;
                SendPropertyChanged("RunLog");
                OnRunLogChanged();
            }
        }
    }

    public string ExceptionJson
    {
        get => _ExceptionJson;
        set
        {
            if (_ExceptionJson != value)
            {
                OnExceptionJsonChanging(value);
                SendPropertyChanging("ExceptionJson");
                _ExceptionJson = value;
                SendPropertyChanged("ExceptionJson");
                OnExceptionJsonChanged();
            }
        }
    }

    public int? DurationMs
    {
        get => _DurationMs;
        set
        {
            if (_DurationMs != value)
            {
                OnDurationMsChanging(value);
                SendPropertyChanging("DurationMs");
                _DurationMs = value;
                SendPropertyChanged("DurationMs");
                OnDurationMsChanged();
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
    partial void OnExecutionRunIdChanging(Guid value);

    partial void OnExecutionRunIdChanged();
    partial void OnSampleUidChanging(string value);

    partial void OnSampleUidChanged();
    partial void OnStatusChanging(string value);

    partial void OnStatusChanged();
    partial void OnBuildLogChanging(string value);

    partial void OnBuildLogChanged();
    partial void OnRunLogChanging(string value);

    partial void OnRunLogChanged();
    partial void OnExceptionJsonChanging(string value);

    partial void OnExceptionJsonChanged();
    partial void OnDurationMsChanging(int? value);

    partial void OnDurationMsChanged();

    #endregion
}