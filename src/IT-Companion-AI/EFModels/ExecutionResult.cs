namespace ITCompanionAI.EFModels;


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








    public Guid Id
    {
        get => _Id;
        set
        {
            if (_Id != value)
            {
                SendPropertyChanging("Id");
                _Id = value;
                SendPropertyChanged("Id");
            }
        }
    }





    public Guid ExecutionRunId
    {
        get => _ExecutionRunId;
        set
        {
            if (_ExecutionRunId != value)
            {
                SendPropertyChanging("ExecutionRunId");
                _ExecutionRunId = value;
                SendPropertyChanged("ExecutionRunId");
            }
        }
    }





    public string SampleUid
    {
        get => _SampleUid;
        set
        {
            if (_SampleUid != value)
            {
                SendPropertyChanging("SampleUid");
                _SampleUid = value;
                SendPropertyChanged("SampleUid");
            }
        }
    }





    public string Status
    {
        get => _Status;
        set
        {
            if (_Status != value)
            {
                SendPropertyChanging("Status");
                _Status = value;
                SendPropertyChanged("Status");
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
                SendPropertyChanging("BuildLog");
                _BuildLog = value;
                SendPropertyChanged("BuildLog");
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
                SendPropertyChanging("RunLog");
                _RunLog = value;
                SendPropertyChanged("RunLog");
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
                SendPropertyChanging("ExceptionJson");
                _ExceptionJson = value;
                SendPropertyChanged("ExceptionJson");
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
                SendPropertyChanging("DurationMs");
                _DurationMs = value;
                SendPropertyChanged("DurationMs");
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